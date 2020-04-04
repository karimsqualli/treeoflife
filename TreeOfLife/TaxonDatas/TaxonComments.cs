using Flurl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace TreeOfLife
{
    public sealed class TaxonComments : IDisposable
    {
        //=========================================================================================
        // Options : comment format and watch or not
        //

        //---------------------------------------------------------------------------------------
        static bool _ShowEmpty = true;
        public static bool ShowEmpty
        {
            get { return _ShowEmpty; }
            set 
            { 
                _ShowEmpty = value;
                OnShowEmptyChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static event EventHandler OnShowEmptyChanged = null;

        //---------------------------------------------------------------------------------------
        static bool _WatchComment = false;
        public static bool WatchComment
        {
            get { return _WatchComment; }
            set
            {
                if (_WatchComment == value) return;
                _WatchComment = value;
                if (_WatchComment)
                    WatcherStart();
                else
                    WatcherStop();
            }
        }

        //---------------------------------------------------------------------------------------
        public static TaxonComments Manager { get; } = new TaxonComments();

        //---------------------------------------------------------------------------------------
        private Thread _Thread = null;
        private static bool _Exit = false;

        private TaxonComments() 
        {
            _Exit = false;

            ThreadStart start = new ThreadStart(TaxonCommentsLoop);
            _Thread = new Thread(start);
            _Thread.Start();
        }

        public void OnConfigLoaded()
        {
            InitHtml();
            if (_WatchComment) Watch();
        }

        //---------------------------------------------------------------------------------------
        public void Stop()
        {
            _Exit = true;
            _LockEvent.Set();
            WatcherStop();
        }

        //---------------------------------------------------------------------------------------
        public void Dispose()
        {
            if (_LockEvent != null) _LockEvent.Dispose();
            _LockEvent = null;
            if (_Watcher != null) _Watcher.Dispose();
            _Watcher = null;
        }

        //=========================================================================================
        // Collections
        //

        public List<CommentsCollection> Collections { get; private set; }
        public List<CommentsCollection> AvailableCollections { get; private set; }
        

        private string _Path = "";
        public string Path
        {
            get { return _Path; }
            set
            {
                _Path = value;
                if (!Directory.Exists(_Path))
                    Directory.CreateDirectory(_Path);

                Collections = CommentsCollection.BuildList(_Path);
                AvailableCollections = CommentsCollection.Available(Collections);
            }
        }

        public void RebuildCollections()
        {
            Path = Path;
        }

        public void UpdateAvailableCollections()
        {
            AvailableCollections = CommentsCollection.Available(Collections);
        }

        public void SaveCollections()
        {
            CommentsCollection.SaveInfos(Collections);
        }

        public bool CanMoveUp(CommentsCollection _collection)
        {
            int index = AvailableCollections.IndexOf(_collection);
            return (index > 0);
        }

        public void MoveUp(CommentsCollection _collection)
        {
            if (!CanMoveUp(_collection)) return;
            foreach (CommentsCollection collection in AvailableCollections)
                collection.PriorityOrder = 2 * (collection.PriorityOrder+1);
            _collection.PriorityOrder -= 3;
            CommentsCollection.SortList(Collections);
        }

        public bool CanMoveDown(CommentsCollection _collection)
        {
            int index = AvailableCollections.IndexOf(_collection);
            return (index != -1) && (index != AvailableCollections.Count - 1);
        }

        public void MoveDown(CommentsCollection _collection)
        {
            if (!CanMoveDown(_collection)) return;
            foreach (CommentsCollection collection in AvailableCollections)
                collection.PriorityOrder = 2 * (collection.PriorityOrder + 1);
            _collection.PriorityOrder += 3;
            CommentsCollection.SortList(Collections);
        }

        //=========================================================================================
        // Thread loop
        //

        //---------------------------------------------------------------------------------------
        ManualResetEvent _LockEvent = new ManualResetEvent(true); 
        void TaxonCommentsLoop()
        {
            while (!_Exit)
            {
                _LockEvent.WaitOne();

                if (_ListRequest.Count == 0)
                {
                    _LockEvent.Reset();
                    _Thread.Priority = ThreadPriority.Lowest;
                    continue;
                }

                TaxonCommentRequest request = PopRequest();
                if (request == null) continue;

                string comment = null;
                if (!GetCommentFromMemory(request.CurrentTaxon, ref comment))
                {
                    CommentFileDesc commentFile = CommentFile(request.CurrentTaxon);
                    
                    if (commentFile!= null)
                    {
                        try
                        {
                            if (commentFile.Collection.IsDistant())
                            {
                                using (WebClient client = new WebClient())
                                {
                                    client.Encoding = System.Text.Encoding.UTF8;
                                    comment = client.DownloadString(commentFile.GetDistantPath());
                                }
                            } else
                            {

                                VinceToolbox.fileFunctions.readTextFile(commentFile.GetHtmlName(), ref comment);
                            }
                            comment = TransformHTMLComment(comment, commentFile);
                            Console.WriteLine(comment);
                            StoreCommentInMemory(request.CurrentTaxon, comment);
                        }
                        catch { }
                    }
                }
                //request.Callback(request.Owner, request.MainTaxon, request.CurrentTaxon, comment);

                if (request.Result == null)
                    request.Result = new TaxonCommentRequestResult() { Main = request.MainTaxon };
                request.Result.Comments.Add(new Tuple<TaxonTreeNode, string>(request.CurrentTaxon, comment));


                if (request.Recursive && request.CurrentTaxon.Father != null)
                {
                    request.CurrentTaxon = request.CurrentTaxon.Father;
                    PushRequest(request);
                }
                else
                    request.Callback(request.Owner, request.Result );
            }
        }

        //=========================================================================================
        // Request list
        //

        List<TaxonCommentRequest> _ListRequest = new List<TaxonCommentRequest>();

        //---------------------------------------------------------------------------------------
        private void PushRequest( TaxonCommentRequest _request )
        {
            lock (_ListRequest)
            {
                _ListRequest.Add(_request);
            }
            _Thread.Priority = ThreadPriority.Normal;
            _LockEvent.Set();
        }

        //---------------------------------------------------------------------------------------
        private TaxonCommentRequest PopRequest()
        {
            TaxonCommentRequest result = null;
            lock (_ListRequest)
            {
                if (_ListRequest.Count > 0)
                {
                    result = _ListRequest[0];
                    _ListRequest.RemoveAt(0);
                }
            }
            return result;
        }

        //---------------------------------------------------------------------------------------
        private void FreeRequest( object _owner  )
        {
            lock (_ListRequest)
            {
                List<TaxonCommentRequest> cleanList = new List<TaxonCommentRequest>();
                foreach (TaxonCommentRequest request in _ListRequest)
                {
                    if (request.Owner == _owner) continue;
                    cleanList.Add(request);
                }
                _ListRequest = cleanList;
            }
        }

        //=========================================================================================
        // Request
        //

        public delegate void OnCommentLoaded(object _owner, TaxonCommentRequestResult _result);
        class TaxonCommentRequest
        {
            public object Owner;
            public TaxonTreeNode MainTaxon;
            public TaxonTreeNode CurrentTaxon;
            public bool Recursive;
            public OnCommentLoaded Callback;
            public TaxonCommentRequestResult Result;
        }

        public class TaxonCommentRequestResult
        {
            public TaxonTreeNode Main = null;
            public List<Tuple<TaxonTreeNode, string>> Comments = new List<Tuple<TaxonTreeNode, string>>();
        }

        //---------------------------------------------------------------------------------------
        public void GetComment(object _owner, TaxonTreeNode _t, bool _recursive, OnCommentLoaded _callback)
        {
            FreeComment(_owner);

            TaxonCommentRequest request = new TaxonCommentRequest
            {
                Owner = _owner,
                MainTaxon = _t,
                CurrentTaxon = _t,
                Recursive = _recursive,
                Callback = _callback
            };
            PushRequest(request);
        }

        //---------------------------------------------------------------------------------------
        public void FreeComment(object _owner)
        {
            FreeRequest(_owner);
        }

        //---------------------------------------------------------------------------------------
        public static string CommentFilename(TaxonDesc _desc)
        {
            if (_desc.IsUnnamed)
            {
                if (_desc.OTTID == 0) return null;
                return "Unnamed_" + _desc.OTTID.ToString();
            }

            string name = _desc.RefMultiName.Main;
            if (_desc.HasFlag(FlagsEnum.IncludeIdInFilenames))
                name += "_" + _desc.OTTID.ToString();
            return name;
        }

        //---------------------------------------------------------------------------------------
        public static CommentFileDesc CommentFile(TaxonTreeNode _node)
        {
            if (_node == null) return null;
            string filename = CommentFilename(_node.Desc);
            Console.WriteLine("foo " + filename);
            CommentFileDesc result;
            foreach (CommentsCollection collection in TaxonComments.Manager.AvailableCollections)
            {
                if (filename != null && collection.IsDistant() && collection.DistantReferences.ContainsKey(filename))
                {
                    result = new CommentFileDesc(collection, filename);
                }
                else
                {
                    result = CommentFileDesc.CreateOnlyIfFileExists(collection, filename);
                }
                if (result != null) return result;
            }
            return null;
        }

        //---------------------------------------------------------------------------------------
        public static List<CommentFileDesc> GetAllCommentFile(TaxonTreeNode _node)
        {
            if (_node == null) return null;
            List<CommentFileDesc> result = null;

            foreach (CommentsCollection collection in TaxonComments.Manager.AvailableCollections)
            {
                if (!_node.Desc.IsUnnamed)
                {
                    string path = CommentFileDesc.BuildHtlmName(collection, _node.Desc.RefMultiName.Main);
                    if (File.Exists(path))
                    {
                        if (result == null) result = new List<CommentFileDesc>();
                        result.Add(new CommentFileDesc(collection, _node.Desc.RefMultiName.Main));
                    }
                }
                if (_node.Desc.OTTID != 0)
                {
                    string path = CommentFileDesc.BuildHtlmName(collection, _node.Desc.OTTID.ToString());
                    if (File.Exists(path))
                    {
                        if (result == null) result = new List<CommentFileDesc>();
                        result.Add(new CommentFileDesc(collection, _node.Desc.OTTID.ToString()));
                    }
  
                }
            }
            return result;
        }

        //---------------------------------------------------------------------------------------
        public enum CommentFileCreateResult
        {
            Success,
            ExistsAlready,
            NoNameAndID,
            Failed, 
            NoCollection
        }

        public static CommentFileCreateResult CommentFileCreate(TaxonTreeNode _node)
        {
            if (_node == null) return CommentFileCreateResult.Failed;

            if (TaxonComments.Manager.AvailableCollections.Count == 0) return CommentFileCreateResult.NoCollection;
            CommentsCollection collection = TaxonComments.Manager.AvailableCollections[0];

            string name = CommentFilename( _node.Desc );
            if (string.IsNullOrEmpty(name)) return CommentFileCreateResult.NoNameAndID;

            string path = CommentFileDesc.BuildHtlmName(collection, name);
            if (File.Exists(path)) 
                return CommentFileCreateResult.ExistsAlready;
            
            Manager.CreateHtmlComment(path, name); 
            Manager.CleanCommentInMemory(0);
            return CommentFileCreateResult.Success;
        }

        //=========================================================================================
        // List of stored comment
        //

        Dictionary<TaxonTreeNode, string> _DicoComments = new Dictionary<TaxonTreeNode, string>();
        List<TaxonTreeNode> _ListComments = new List<TaxonTreeNode>();

        static readonly int MaxMemory = 1024 * 1024 * 64; // 64 Mo max memory taken by comments
        int CurrentMemory = 0;
        
        //---------------------------------------------------------------------------------------
        bool GetCommentFromMemory(TaxonTreeNode _taxon, ref string _comment )
        {
            if (!_DicoComments.ContainsKey(_taxon)) return false;
            _ListComments.Remove(_taxon);
            _ListComments.Add(_taxon);
            _comment = _DicoComments[_taxon];
            return true;
        }

        //---------------------------------------------------------------------------------------
        void StoreCommentInMemory(TaxonTreeNode _taxon, string _comment)
        {
            if (_DicoComments.ContainsKey(_taxon))
            {
                _ListComments.Remove(_taxon);
                if (_DicoComments[_taxon] != null)
                    CurrentMemory -= _DicoComments[_taxon].Length;
            }
            _DicoComments[_taxon] = _comment;
            _ListComments.Add(_taxon);
            if (_comment != null)
                CurrentMemory += _comment.Length;
            CleanCommentInMemory(MaxMemory);
        }

        //---------------------------------------------------------------------------------------
        public void CleanCommentInMemory(int _limit)
        {
            if (_limit == 0)
            {
                _ListComments.Clear();
                _DicoComments.Clear();
                CurrentMemory = 0;
                return;
            }

            while (CurrentMemory > _limit && _ListComments.Count > 0)
            {
                TaxonTreeNode taxon = _ListComments[0];
                _ListComments.RemoveAt(0);
                if (_DicoComments[taxon] != null)
                    CurrentMemory -= _DicoComments[taxon].Length;
                _DicoComments.Remove(taxon);
            }
        }

        //=========================================================================================
        // Watcher
        //

        static void WatcherStart()
        {
            if (Manager == null) return;
            if (Manager._Watcher != null) return;
            Manager.Watch();
        }

        static void WatcherStop()
        {
            if (Manager == null) return;
            if (Manager._Watcher == null) return;
            Manager._Watcher = null;
        }

        FileSystemWatcher _Watcher = null;
        
        private void Watch()
        {
            if (TaxonUtils.MyConfig == null) return;
            _Watcher = new FileSystemWatcher
            {
                Path = TaxonComments.Manager.Path,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*",
                IncludeSubdirectories = true
            };
            _Watcher.Changed += new FileSystemEventHandler(OnChanged);
            _Watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (!WatchComment) return;
            CleanCommentInMemory(0);
            TaxonUtils.MainWindow.Invoke(new Action(() => OnChangedDelegate(e)));
        }

        void OnChangedDelegate(FileSystemEventArgs e)
        {
            InitHtml();
            if (TaxonUtils.SelectedTaxon() == null) return;
            if (System.IO.Path.GetFileNameWithoutExtension(e.Name).ToLower() == TaxonUtils.SelectedTaxon().Desc.RefMultiName.Main.ToLower() )
                TreeOfLife.Controls.TaxonControlList.OnReselectTaxon(TaxonUtils.MainGraph.Selected); 
            else if (e.Name.StartsWith("_Style") || e.Name.StartsWith("_Template") )
                TreeOfLife.Controls.TaxonControlList.OnReselectTaxon(TaxonUtils.MainGraph.Selected); 
        }

        //=========================================================================================
        // HTML
        //

        string TransformHTMLComment(string comment, CommentFileDesc _desc)
        {
            if (_desc.IsDistantComment())
            {
                return comment.Replace("<img src=\"", "<img src=\"" + _desc.Collection.Location);
            }
            return comment.Replace("<img src=\"", "<img src=\"" + _desc.GetPath() + "\\");
        }

        void CreateHtmlComment(string _filename, string _name)
        {
            try
            {
                string content = _HtmlTemplate.Replace("<taxon>", _name);
                VinceToolbox.fileFunctions.saveTextFile(_filename, content);
            }
            catch (Exception e)
            {
                Loggers.WriteError(LogTags.Comment, "Create failed:\n" + e.Message);
            }
        }

        void InitHtml()
        {
            try
            {
                string folderStyle = TaxonComments.Manager.Path + "\\_Styles";
                if (!Directory.Exists(folderStyle))
                    Directory.CreateDirectory(folderStyle);

                string fileStyle = folderStyle + "\\style.css";
                if (!File.Exists(fileStyle))
                    VinceToolbox.fileFunctions.saveTextFile(fileStyle, _DefaultHtmlStyle);
                VinceToolbox.fileFunctions.readTextFile(fileStyle, ref _HtmlStyle);
            }
            catch
            {
                _HtmlStyle = _DefaultHtmlStyle;
            }

            try
            {
                string folderTemplate = TaxonComments.Manager.Path + "\\_Template";
                if (!Directory.Exists(folderTemplate))
                    Directory.CreateDirectory(folderTemplate);

                string fileHeader = folderTemplate + "\\header.html";
                if (!File.Exists(fileHeader))
                    VinceToolbox.fileFunctions.saveTextFile(fileHeader, _DefaultHtmlHead);
                VinceToolbox.fileFunctions.readTextFile(fileHeader, ref _HtmlHeader );

                string fileTemplate = folderTemplate + "\\template.html";
                if (!File.Exists(fileTemplate))
                    VinceToolbox.fileFunctions.saveTextFile(fileTemplate, _DefaultHtmlTemplate);
                VinceToolbox.fileFunctions.readTextFile(fileTemplate, ref _HtmlTemplate );
            }
            catch
            {
                _HtmlHeader = _DefaultHtmlHead;
                _HtmlTemplate = _DefaultHtmlTemplate;
            }

            _HtmlTemplate = "" + _DefaultHtmlTemplate;
            _HtmlHeader = "" + _DefaultHtmlHead;
            _HtmlHeader = _HtmlHeader.Replace( "<style></style>", "<style>\n" + _HtmlStyle + "</style>" );
        }

        private string _HtmlStyle;
        private readonly string _DefaultHtmlStyle =
            ".taxonTitle\r\n" +
            "{\r\n" +
            "    color:white; \r\n" +
            "    background:lavender; \r\n" +
            "    font-size:16px; \r\n" +
            "    margin-top: 1px;\r\n" +
            "    margin-bottom:1px;\r\n" +
            "}\r\n" +
            "button { color: #444444; width:1.5em; height:1.5em; border: 1px solid #444444; }\r\n" +
            ".buttonCollapse{}\r\n" +
            ".buttonExpand{display:none;}\r\n" +
            ".taxon{ color:black;} \r\n" +
            "\r\n" +
            ".figureRef\r\n" +
            "{\r\n" +
            "    font-weight:bold;\r\n" +
            "    color:#7F7F7F;\r\n" +
            "}";

        private string _HtmlHeader;
        public string HtmlHeader { get { return _HtmlHeader; } }
        private readonly string _DefaultHtmlHead =
            "  <head>\r\n" +
            "    <meta charset=\"utf-8\" />\r\n" +
            "    <title>Taxon</title>\r\n" +
            "    <style></style>\r\n" +
            "    <script type='text/javascript'>\r\n" +
            "      function hide(divName)\r\n" +
            "      {\r\n" +
            "        document.getElementById(divName).style.display = 'none';\r\n" +
            "        document.getElementById(divName+'Hide').style.display = 'none';\r\n" +
            "        document.getElementById(divName+'Show').style.display = 'inline';\r\n" +
            "      }\r\n" +
            "      function show(divName)\r\n" +
            "      {\r\n" +
            "        document.getElementById(divName).style.display = 'block';\r\n" +
            "        document.getElementById(divName+'Hide').style.display = 'inline';\r\n" +
            "        document.getElementById(divName+'Show').style.display = 'none';\r\n" +
            "      }\r\n" +
            "      function callcsharp(divName, index)\r\n"+
            "      {\r\n" +
            "        window.external.ScriptCall(divName, index);\r\n" +
            "      }\r\n" +
            "    </script>\r\n" +
            "  </head>\r\n";

        private string _HtmlTemplate;
        public string HtmlTemplate { get { return _HtmlTemplate; } }
        private readonly string _DefaultHtmlTemplate =
            "<html>\r\n" +
            "  <head>\r\n" +
            "    <meta charset=\"utf-8\" />\r\n" +
            "    <title><taxon></title>\r\n" +
            "    <link rel=\"stylesheet\" href=\"_Styles/style.css\" />\r\n" +
            "  </head>\r\n" +
            "  <body>\r\n" +
            "    <ul>\r\n" +
            "      <li>Derived characters: <strong><taxon></strong></li>\r\n" +
            "    </ul>\r\n" +
            "  </body>\r\n" +
            "</html>";
    }

    //---------------------------------------------------------------------------------------
    public class CommentFileDesc
    {
        public CommentFileDesc(CommentsCollection _collection,  string _name )
        {
            Collection = _collection;
            Name = _name;
        }

        public bool IsDistantComment()
        {
            return Collection.IsDistant();
        }

        public static CommentFileDesc CreateOnlyIfFileExists(CommentsCollection _collection, string _name)
        {
            try
            {
                string path = Path.Combine(_collection.Path, _name + ".html");
                if (File.Exists(path))
                    return new CommentFileDesc(_collection, _name);
            }
            catch(Exception e)
            {
                Loggers.WriteError(LogTags.Data, "Loading comment for " + _name + " raise an exception:\n" + e.Message);
            }
            return null;
        }

        public readonly CommentsCollection Collection;
        public readonly string Name;

        public string GetPath() { return Collection.Path; }
        public string GetHtmlName() { return Collection.Path + System.IO.Path.DirectorySeparatorChar + Name + ".html"; }

        public string GetDistantImagePath() { return Collection.Location + "/"; }
        public string GetDistantPath() { return Url.Combine(Collection.Location, Name); }
        public string GetHtmlFilesDir() { return Collection.Path + System.IO.Path.DirectorySeparatorChar + Name; }

        public static string BuildHtlmName(CommentsCollection _collection, string _name) { return _collection.Path + System.IO.Path.DirectorySeparatorChar + _name + ".html"; }

    }
}
