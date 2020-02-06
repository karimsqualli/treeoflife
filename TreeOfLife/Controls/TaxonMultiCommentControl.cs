using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace TreeOfLife.Controls
{
    public partial class TaxonMultiCommentControl : UserControl
    {
        //---------------------------------------------------------------------------------
        public TaxonMultiCommentControl()
        {
            InitializeComponent();
            webBrowser.ObjectForScripting = new ScriptInterface() { Owner = this };
        }

        [ComVisible(true)]
        public class ScriptInterface
        {
            public TaxonMultiCommentControl Owner;
            public void ScriptCall(string _name, int _index)
            {
                if (_index >= 0 && _index < Owner._HtmlDivs.Count )
                {
                    TaxonTreeNode node = Owner._HtmlDivs[_index].Taxon;
                    if (node != null && _name == node.Desc.RefMainName)
                    {
                        TaxonUtils.GotoTaxon(node);
                    }
                }
            }
        }

        //---------------------------------------------------------------------------------
        void SetComments( TaxonComments.TaxonCommentRequestResult _result )
        {
            _HtmlDivs.Clear();
            foreach (var tuple in _result.Comments)
            {
                string name = tuple.Item1.Desc.RefMainName;
                string frenchName = tuple.Item1.Desc.FrenchMainName;
                string comment = tuple.Item2;

                if (string.IsNullOrEmpty(name)) name = "unnamed";
                string displayName = name;
                if (!string.IsNullOrEmpty(frenchName)) displayName += " ( " + frenchName + " )";

                if (comment != null)
                {
                    int indexStart = comment.IndexOf("<body>");
                    if (indexStart == -1) return;
                    indexStart += 6;
                    int indexEnd = comment.IndexOf("</body>");
                    if (indexEnd == -1) return;
                    if (indexEnd <= indexStart) return;

                    string body = comment.Substring(indexStart, indexEnd - indexStart);

                    _HtmlDivs.Add(new HtmlDiv() { Name = name, Content = body, DisplayName = displayName, Taxon = tuple.Item1 });
                }
                else
                    _HtmlDivs.Add(new HtmlDiv() { Name = name, Content = null, DisplayName = displayName, Taxon = tuple.Item1 });
            }
            BuildHtml();
            webBrowser.DocumentText = HtmlDocument;
        }

        //---------------------------------------------------------------------------------
        void ClearComments()
        {
            webBrowser.DocumentText = "";
            _HtmlDivs.Clear();
            HasCollapsibleDivs = false;
        }

        //---------------------------------------------------------------------------------
        TaxonTreeNode _Current = null;
        public void RefreshContent(bool _force = false)
        {
            TaxonTreeNode node = _MainTaxon;
            bool recursive = Recursive;
            if (_AlternativeTaxon != null)
            {
                bool hasComment = TaxonComments.CommentFile(_AlternativeTaxon) != null;
                if (!hasComment && AddFranceMap)
                    hasComment = (TaxonUtils.Locations != null) && TaxonUtils.Locations.LocationByTaxon.ContainsKey(_AlternativeTaxon.GetOriginal());
                if (hasComment)
                {
                    node = _AlternativeTaxon;
                    recursive = false;
                }
            }

            if (!_force && _Current == node)
            {
                OnDocumentCompleted?.Invoke(this, EventArgs.Empty);
                return;
            }

            _Current = node;
            ClearComments();
            if (node != null)
            {
                TaxonComments.Manager.GetComment(this, node, recursive, OnCommentLoaded);
                BuildHtml();
                webBrowser.DocumentText = HtmlDocument;
            }
        }

        //---------------------------------------------------------------------------------
        TaxonTreeNode _MainTaxon = null;
        public TaxonTreeNode MainTaxon
        {
            get { return _MainTaxon; }
            set
            {
                _MainTaxon = value;
                _AlternativeTaxon = null;
                RefreshContent();
            }
        }

        //---------------------------------------------------------------------------------
        void OnCommentLoaded(object _owner, TaxonComments.TaxonCommentRequestResult _result )
        {
            if (_owner != this) return;
            BeginInvoke((Action)(() => SetComments( _result )));
        }

        //---------------------------------------------------------------------------------
        TaxonTreeNode _AlternativeTaxon = null;
        public TaxonTreeNode AlternativeTaxon
        {
            get { return _AlternativeTaxon; }
            set
            {
                if (_AlternativeTaxon == value) return;
                _AlternativeTaxon = value;
                RefreshContent();
            }
        }

        //---------------------------------------------------------------------------------
        public event EventHandler OnDocumentCompleted = null;
        void WebBrowser_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            int i;
            for (i = 0; i < webBrowser.Document.Links.Count; i++)
            {
                webBrowser.Document.Links[i].Click += new HtmlElementEventHandler(this.LinkClick);
            }

            OnDocumentCompleted?.Invoke(this, EventArgs.Empty);
        }

        //---------------------------------------------------------------------------------
        private void LinkClick(object sender, System.EventArgs e)
        {
            HtmlElement element = (HtmlElement)sender;
            string id = element.Id;
            string href = element.GetAttribute("href");

            System.Diagnostics.Process.Start(href); 
        }

        //---------------------------------------------------------------------------------
        public bool Recursive { get; set; }
        public bool AddTitleForFirstEmpty { get; set; }
        public bool AddTitleForEmpty { get; set; }

        //=========================================================================================
        // HTML 
        //

        //---------------------------------------------------------------------------------
        class HtmlDiv
        {
            public string Name;
            public string DisplayName;
            public string Content;
            public TaxonTreeNode Taxon;
        }

        class AllHtmlDivs : List<HtmlDiv>
        {
            public bool CanCollapseExpand()
            {
                foreach (HtmlDiv div in this)
                    if (div.Content != null) return true;
                return false;
            }

            string AllFunction( string _name )
            {
                string result = "";
                foreach (HtmlDiv div in this)
                {
                    if (div.Content != null) 
                        result += "  " + _name + "('" + div.Name + "');\n";
                }
                if (result != "")
                    result = "function " + _name + "all() {\n" + result + "}\n";
                return result;
            }

            public string HideAllFunction() { return AllFunction("hide"); }
            public string ShowAllFunction() { return AllFunction("show"); }
        }

        //---------------------------------------------------------------------------------
        AllHtmlDivs _HtmlDivs = new AllHtmlDivs();

        //---------------------------------------------------------------------------------
        bool _AddFranceMap = true;
        public bool AddFranceMap
        {
            get { return _AddFranceMap; }
            set
            {
                if (_AddFranceMap == value) return;
                _AddFranceMap = value;
            }
        }

        int  _MapFileCount = 0;
        string BuildFranceMap( string _name )
        {
            if (TaxonUtils.Locations == null || _Current == null) return "";
            TaxonTreeNode original = _Current.GetOriginal();
            if (original == null || !TaxonUtils.Locations.LocationByTaxon.TryGetValue(original, out string Ids))
                return "";

            France.Map map = new France.Map();
            foreach (string id in Ids.Split("|".ToCharArray()))
                map.SetTheme(id, "on");

            _MapFileCount = (_MapFileCount + 1) % 100;
            string file = "comment_france_map" + _MapFileCount.ToString("D3") + ".jpg";
            string path = Path.Combine(TaxonUtils.GetTempPath(), file);

            if (File.Exists(path)) File.Delete(path);

            map.CreateTexture(path, 0.6f);

            int count = 0;
            while (!File.Exists(path) && ++count < 4 )
                System.Threading.Thread.Sleep(500);

            string result = "";
            result += "<p><img src=\"" + path + "\" xmlns = \"\" /></p>";
            return result;
        }

        //---------------------------------------------------------------------------------
        public bool HasContent()
        {
            if (_HtmlDivs == null || _HtmlDivs.Count == 0) return false;
            foreach (HtmlDiv div in _HtmlDivs)
                if (div.Content != null) return true;
            return false;
        }
        public string HtmlDocument { get; private set; } = "Loading...";

        //---------------------------------------------------------------------------------
        public event EventHandler OnHasCollapsibleDivsChanged = null;
        bool _HasCollapsibleDivs = false;
        public bool HasCollapsibleDivs 
        { 
            get { return _HasCollapsibleDivs; }
            set
            {
                if (_HasCollapsibleDivs == value) return;
                _HasCollapsibleDivs = value;
                OnHasCollapsibleDivsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //---------------------------------------------------------------------------------
        void BuildHtml()
        {
            HtmlDocument = "<html>\n";
            HtmlDocument += TaxonComments.Manager.HtmlHeader;

            HtmlDocument += "  <body>\n";

            if (_HtmlDivs.Count == 0)
            {
                HtmlDocument += "    <h2>Loading ...</h2>\n";
            }
            else
            {
                int index = HtmlDocument.LastIndexOf("</script>");
                HtmlDocument = HtmlDocument.Insert(index, _HtmlDivs.ShowAllFunction() );
                HtmlDocument = HtmlDocument.Insert(index, _HtmlDivs.HideAllFunction());

                bool first = true;
                for (int i = 0; i < _HtmlDivs.Count; i++)
                {
                    HtmlDiv div = _HtmlDivs[i];

                    string calltitle = " ondblclick =\"callcsharp('" + div.Name + "', " + i.ToString() + ")\"";
                    if (div.Content == null)
                    {
                        if ( (first && AddTitleForFirstEmpty) || AddTitleForEmpty || (first && AddFranceMap) )
                        {
                            HtmlDocument += "    <h2 class=\"taxonTitle\""+ calltitle +">\n";
                            HtmlDocument += "      " + div.DisplayName + "\n";
                            HtmlDocument += "    </h2>\n";
                            if (AddFranceMap) HtmlDocument += BuildFranceMap(div.Name);
                        }
                    }
                    else
                    {
                        HtmlDocument += "    <h2 class=\"taxonTitle\"" + calltitle + ">\n";
                        HtmlDocument += "      <button id=\"" + div.Name + "Hide\" onClick=\"hide('" + div.Name + "')\" class=\"buttonCollapse\" >-</button>\n";
                        HtmlDocument += "      <button id=\"" + div.Name + "Show\" onClick=\"show('" + div.Name+ "')\" class=\"buttonExpand\" >+</button>\n";
                        HtmlDocument += "      " + div.DisplayName + "\n";
                        HtmlDocument += "    </h2>\n";

                        if (first && AddFranceMap) HtmlDocument += BuildFranceMap(div.Name);

                        HtmlDocument += "    <div id=\"" + div.Name+ "\" class=\"taxon\" >\n";
                        HtmlDocument += "\n\n" + div.Content + "\n\n\n";
                        HtmlDocument += "    </div>\n";
                    }
                    first = false;
                }
            }
            HtmlDocument += "  </body>\n";
            HtmlDocument += "</html>";

            HasCollapsibleDivs = _HtmlDivs.CanCollapseExpand();

            string path = Path.Combine(TaxonUtils.GetTempPath(), "HtmlDocument.html");
            VinceToolbox.fileFunctions.saveTextFile(path, HtmlDocument);
        }

        //---------------------------------------------------------------------------------
        public void CollapseAll()
        {
            if (!HasCollapsibleDivs) return;
            webBrowser.Document.InvokeScript("hideall");
        }

        //---------------------------------------------------------------------------------
        public void ExpandAll()
        {
            if (!HasCollapsibleDivs) return;
            webBrowser.Document.InvokeScript("showall");
        }
    }
}
