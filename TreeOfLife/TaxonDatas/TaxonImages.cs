using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Net;
using Flurl;

namespace TreeOfLife
{
    //=========================================================================================
    // Images manager
    //
    class TaxonImages : Dictionary<string, TaxonImage>, IDisposable
    {
        public static TaxonImages Manager { get; } = new TaxonImages();

        //=========================================================================================
        // Collections
        //
        private Dictionary<int, ImageCollection> Collections { get; set; }

        public IEnumerable<ImageCollection> CollectionsEnumerable() { return Collections.Values; }

        private string _Path = "";
        public string Path
        {
            get { return _Path; }
            set
            {
                _DefaultImageCollection = null;

                _Path = value;
                Collections = ImageCollection.BuildDictionary(_Path);

                foreach (ImageCollection collection in Collections.Values)
                {
                    if (!collection.IsDefault) continue;
                    DefaultImageCollection = collection;
                    break;
                }
                if (DefaultImageCollection == null)
                    DefaultImageCollection = Collections.Values.FirstOrDefault();
            }
        }

        public void RebuildCollections()
        {
            Path = Path;
        }

        public void SaveCollections()
        {
            ImageCollection.SaveInfos(Collections);
        }
            

        ImageCollection _DefaultImageCollection = null;
        public ImageCollection DefaultImageCollection
        {
            get
            {
                return _DefaultImageCollection;
            }
            set 
            {
                _DefaultImageCollection = value;
                foreach (ImageCollection collection in Collections.Values)
                {
                    bool isdefault = (collection == value);
                    if (isdefault == collection.IsDefault) continue;
                    {
                        collection.IsDefault = isdefault;
                        collection.SaveInfos();
                    }
                }
            }
        }

        public bool CollectionIsAvailable(int _collectionID)
        {
            if (!Collections.ContainsKey(_collectionID))
                return false;
            return Collections[_collectionID].UseIt;
        }

        public ImageCollection Collection(int _collectionID)
        {
            if (!Collections.ContainsKey(_collectionID))
                return null;
            return Collections[_collectionID];
        }

        public string CollectionPath(int _collectionID)
        {
            if (!Collections.ContainsKey(_collectionID))
                return null;
            return Collections[_collectionID].Path;
        }

        public string CollectionName(int _collectionID)
        {
            if (!Collections.ContainsKey(_collectionID))
                return null;
            return Collections[_collectionID].Name;
        }

        public ImageCollection GetByName( string _name )
        {
            TaxonImages.Manager.RebuildCollections();
            foreach (ImageCollection collection in Collections.Values)
                if (collection.Name.ToLower() == _name.ToLower())
                    return collection;
            return null;
        }

        public ImageCollection GetOrCreateCollection( string _name )
        {
            ImageCollection collection = GetByName(_name);
            if (collection == null)
            {
                try
                {
                    string collectionPath = Path + System.IO.Path.DirectorySeparatorChar + _name;
                    Directory.CreateDirectory(collectionPath);
                    TaxonImages.Manager.RebuildCollections();
                    collection = GetByName(_name);
                }
                catch (Exception e)
                {
                    string error = "Exception while creating new collection: \n\n";
                    error += e.Message;
                    if (e.InnerException != null) error += "\n" + e.InnerException.Message;
                    Loggers.WriteError(LogTags.Collection, error);
                }
            }
            return collection;
        }

        //=========================================================================================
        // decompose filename
        // 
        public class SplitImageFilenameResult
        {
            public TaxonImageDesc Desc = new TaxonImageDesc();
            public string TaxonName = null;
        }

        public SplitImageFilenameResult SplitImageFilename( string _path, bool checkExt = true, ImageCollection collection = null )
        {
            if (checkExt)
            {
                string fileext = System.IO.Path.GetExtension(_path).ToLower();
                if (fileext != ".jpg") return null;
            }

            if (collection == null)
            {
                if (!_path.ToLower().StartsWith(Path.ToLower())) return null;
                string folder = System.IO.Path.GetDirectoryName(_path);
                folder = folder.Substring(Path.Length).Trim("/\\".ToCharArray());
                collection = GetByName(folder);
                if (collection == null) return null;
            }

            SplitImageFilenameResult result = new SplitImageFilenameResult();
            result.Desc.CollectionId = collection.Id;
            result.Desc.Secondary = false;
            result.Desc.Index = -1;

            string name = System.IO.Path.GetFileNameWithoutExtension(_path);
            int idx = name.LastIndexOf('_');
            if (idx != -1)
            {
                string ext = name.Substring(idx + 1).ToLower();
                if (ext[0] == 's')
                {
                    result.Desc.Secondary = true;
                    ext = ext.Substring(1);
                }
                if (int.TryParse(ext, out int number))
                {
                    result.Desc.Index = number;
                    result.TaxonName = name.Remove(idx);
                    return result;
                }
            }
            result.TaxonName = name;
            return result;
        }


        //=========================================================================================
        // List images
        // todo : sort / filter by category / prio
        public List<TaxonImageDesc> GetListImages(TaxonDesc _taxon)
        {
            return _taxon.AvailableImages;
        }

        public List<TaxonImageDesc> GetListMainImages(TaxonDesc _taxon)
        {
            List<TaxonImageDesc> available = _taxon.AvailableImages;
            if (available == null) return null;
            List<TaxonImageDesc> list = new List<TaxonImageDesc>();
            foreach (TaxonImageDesc image in available)
            {
                if (image.Secondary) continue;
                list.Add(image);
            }
            return list;
        }

        //=========================================================================================
        public Image GetSmallImage(TaxonTreeNode t) { return GetImage(t, 48); }
        public Image GetMediumImage(TaxonTreeNode t) { return GetImage(t, 512); }

        public Image GetImage(TaxonTreeNode t, int size)
        {
            if (!t.Desc.HasImage) return null;

            lock (this)
            {
                if (this.ContainsKey(t.Desc.RefMultiName.Main))
                {
                    if (size < 64)
                        return this[t.Desc.RefMultiName.Main].SmallImage;
                    else
                        return this[t.Desc.RefMultiName.Main].MediumImage;
                }
                else
                {
                    List<TaxonImageDesc> list = TaxonImages.Manager.GetListImages(t.Desc);
                    if (list != null && list.Count > 0)
                    {
                        this[t.Desc.RefMultiName.Main] = new TaxonImage
                        {
                            path = list[0].GetPath(t.Desc),
                            link = list[0].GetLink(),

                            linkcachefile = list[0].GetImageCacheFile()
                        };

                        ImageCollection collection = Collection(list[0].CollectionId);
                        if (collection.IsDistant())
                        {
                            this[t.Desc.RefMultiName.Main].link = Url.Combine(collection.Location, t.Desc.RefMultiName.Main, list[0].Index.ToString());
                            this[t.Desc.RefMultiName.Main].distant = true;
                            this[t.Desc.RefMultiName.Main].linkcachefile = list[0].getDistantImageCacheFile(t.Desc);
                        }

                        ToTreat.Add(this[t.Desc.RefMultiName.Main]);
                        timer.Enabled = true;
                    }
                }
            }
            return null;
        }

        private System.Timers.Timer timer = null;
        private TaxonImages()
        {
            timer = new System.Timers.Timer(10);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerEvent);
            TimerRequestFull_Init();
        }

        public void Dispose()
        {
            if (timer != null) { timer.Dispose(); timer = null; }
            TimerRequestFull_Dispose();
            //if (_BW != null) { _BW.Dispose(); _BW = null; }
            //if (bwFullImage != null) { bwFullImage.Dispose(); bwFullImage = null; }
        }

        List<TaxonImage> ToTreat = new List<TaxonImage>();
        TaxonImage Current = null;

        private void OnTimerEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            lock (this)
            {
                if (ToTreat.Count > 0)
                {
                    Current = ToTreat[ToTreat.Count - 1];
                    ToTreat.RemoveAt(ToTreat.Count - 1);

                    BackgroundWorker bw = new BackgroundWorker() { WorkerReportsProgress = false, WorkerSupportsCancellation = false };
                    bw.DoWork += new DoWorkEventHandler(BWDoWork);
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWCompleted);
                    bw.RunWorkerAsync();
                }
                timer.Enabled = false;
            }
        }

        private void BWDoWork(object sender, DoWorkEventArgs e)
        {
            lock (this)
            {
                Current.Load();
            }
        }

        private void BWCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            bw.Dispose();
            timer.Enabled = true;
        }

        public delegate void OnFullImageLoaded(TaxonTreeNode _taxon, Image _image);

        class FullImageRequest
        {
            public TaxonTreeNode Taxon;
            public TaxonImageDesc ImageDesc;
            public OnFullImageLoaded CompletionCallback;
            public DateTime TimeStart;
            public int MillisecondsToWait;
        }

        class BackgroundWorkerFullImage : BackgroundWorker
        {
            public FullImageRequest Request;
            public Image LoadedImage = null;
        }

        public void GetFullImage(TaxonTreeNode _taxon, TaxonImageDesc _image, OnFullImageLoaded _callback)
        {
            GetFullImage(new FullImageRequest() { Taxon = _taxon, ImageDesc = _image, CompletionCallback = _callback });
        }

        void GetFullImage(FullImageRequest _request)
        {
            BackgroundWorkerFullImage bwFullImage = new BackgroundWorkerFullImage()
            {
                Request = _request,
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = true
            };
            bwFullImage.DoWork += new DoWorkEventHandler(BWFullImage_DoWork);
            bwFullImage.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWFullImage_Completed);
            bwFullImage.RunWorkerAsync();
        }


        private void BWFullImage_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!(sender is BackgroundWorkerFullImage data)) return;

            TaxonImageDesc desc = data.Request.ImageDesc;

            string path;

            ImageCollection collection = Collection(desc.CollectionId);
            string link = "";
            try
            {
                // Todo: Prevent distant collections from having links or local images
                if (desc != null && desc.IsALink)
                {
                    path = desc.GetImageCacheFile();
                    if (!File.Exists(path))
                    {
                        link = desc.GetLink();
                        if (string.IsNullOrEmpty(link)) return;
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(new Uri(link), path);
                        }
                    }
                    if (!File.Exists(path))
                        return;
                }
                else if (collection.IsDistant())
                {
                    path = desc.getDistantImageCacheFile(data.Request.Taxon.Desc);
                    if (!File.Exists(path))
                    {
                        link = collection.getDistantImageLink(data.Request.Taxon.Desc.RefMultiName.Main, desc.Index);
                        Console.WriteLine("link : " + link);
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(new Uri(link), path);
                            Console.WriteLine("downloaded !");
                        }
                        if (!File.Exists(path))
                            return;
                    }
                }
                else
                {
                    path = data.Request.ImageDesc.GetPath(data.Request.Taxon.Desc);
                    if (!File.Exists(path))
                        return;
                }


                try
                {
                    data.LoadedImage = VinceToolbox.Helpers.BitmapHelper.GetImage(path);
                    //data.LoadedImage.Save("c:/temp.bmp");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } catch (WebException exception)
            {
                Loggers.WriteWarning(LogTags.Image, "Unable to download image file : " + link);
                data.LoadedImage = null;
            }
        }

        private void BWFullImage_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(sender is BackgroundWorkerFullImage data)) return;
            data.Request.CompletionCallback?.Invoke(data.Request.Taxon,data.LoadedImage);
        }

        private System.Timers.Timer timerRequestFull = null;
        private Dictionary<object, FullImageRequest> WaitingRequestDico = new Dictionary<object, FullImageRequest>();

        private void TimerRequestFull_Init()
        {
            timerRequestFull = new System.Timers.Timer(10);
            timerRequestFull.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerRequestFullEvent);
        }

        private void TimerRequestFull_Dispose()
        {
            if (timerRequestFull != null) { timerRequestFull.Dispose(); timerRequestFull = null; }
            WaitingRequestDico.Clear();
        }

        private void OnTimerRequestFullEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            lock (WaitingRequestDico)
            {
                DateTime now = DateTime.Now;
                foreach ( KeyValuePair<object, FullImageRequest> pair in WaitingRequestDico )
                {
                    if ((now - pair.Value.TimeStart).TotalMilliseconds > pair.Value.MillisecondsToWait )
                    {
                        GetFullImage(pair.Value);
                        WaitingRequestDico.Remove(pair.Key);
                        return;
                    }
                }
                if (WaitingRequestDico.Count == 0)
                    timerRequestFull.Enabled = false;
            }
        }

        public void RegisterDelayedGetFullImage(object _Owner, TaxonTreeNode _taxon, TaxonImageDesc _image, OnFullImageLoaded _callback)
        {
            if (_taxon == null || _image == null) return;
            FullImageRequest request = new FullImageRequest() { Taxon = _taxon, ImageDesc = _image, CompletionCallback = _callback };
            request.TimeStart = DateTime.Now;
            request.MillisecondsToWait = 200;
            lock (WaitingRequestDico)
            {
                WaitingRequestDico[_Owner] = request;
            }
            timerRequestFull.Enabled = true;
        }

        public void UnregisterDelayedGetFullImage(object _Owner)
        {
            lock (WaitingRequestDico)
            {
                WaitingRequestDico.Remove(_Owner);
            }
        }
    }

    //=========================================================================================
    // one loaded TaxonImage
    class TaxonImage : IDisposable
    {
        public string path = null;
        public string link = null;
        public string linkcachefile = null;
        public bool distant = false;
        bool init = false;
        bool valid = false;
        Image smallImage = null;
        Image mediumImage = null;

        public void Dispose()
        {
            if (smallImage != null) { smallImage.Dispose(); smallImage = null; }
            if (mediumImage != null) { mediumImage.Dispose(); mediumImage = null; }
        }

        public void Load()
        {
            init = true;
            valid = false;

            string finalpath = path;

            if ( !string.IsNullOrEmpty(link) && linkcachefile != null)
            {
                finalpath = linkcachefile;
                if (!File.Exists(linkcachefile))
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(new Uri(link), linkcachefile);
                    }
                }
            }

            if (!File.Exists(finalpath))
                return;
            try
            {
                Image full = VinceToolbox.Helpers.BitmapHelper.GetImage(finalpath);
                smallImage = new Bitmap(48, 48);
                Graphics e = Graphics.FromImage(smallImage);
                e.DrawImage(full, Rectangle.FromLTRB(0, 0, 48, 48));
                e.Dispose();
                mediumImage = new Bitmap(512, 512);
                e = Graphics.FromImage(mediumImage);
                e.DrawImage(full, Rectangle.FromLTRB(0, 0, 512, 512));
                e.Dispose();
                full.Dispose();
                valid = true;
            }
            catch { }
        }

        public Image SmallImage
        {
            get
            {
                if (!init) return null;
                if (!valid) return null;
                return smallImage;
            }
        }

        public Image MediumImage
        {
            get
            {
                if (!init) return null;
                if (!valid) return null;
                return mediumImage;
            }
        }
    }
}
