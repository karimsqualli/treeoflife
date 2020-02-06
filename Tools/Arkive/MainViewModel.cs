using System.Collections.Generic;

namespace Arkive
{
    class MainViewModel : ViewModel
    {
        private string _OutputFolder;
        public string OutputFolder
        {
            get { return _OutputFolder; }
            set
            {
                _OutputFolder = value;
                LogFilename = System.IO.Path.Combine(_OutputFolder, "arkive.log");
                SpeciesFilename = System.IO.Path.Combine(_OutputFolder, "species.txt");
                ImageUrlsFilename = System.IO.Path.Combine(_OutputFolder, "imageurls.txt");
                ImageCsvFilename = System.IO.Path.Combine(_OutputFolder, "links.csv");
                RaisePropertyChanged();
            }
        }

        string _LogFilename;
        public string LogFilename
        {
            get { return _LogFilename; }
            set
            {
                _LogFilename = value;
                RaisePropertyChanged();
            }
        }

        string _SpeciesFilename;
        public string SpeciesFilename
        {
            get { return _SpeciesFilename; }
            set
            {
                _SpeciesFilename = value;
                RaisePropertyChanged();
            }
        }

        bool _UseSpeciesLocalFile = false;
        public bool UseSpeciesLocalFile
        {
            get { return _UseSpeciesLocalFile; }
            set { _UseSpeciesLocalFile = value; RaisePropertyChanged(); }
        }

        List<string> _Species = new List<string>();
        public List<string> Species
        {
            get { return _Species; }
            set { _Species = value; RaisePropertyChanged(); }
        }

        string _ImageUrlsFilename;
        public string ImageUrlsFilename
        {
            get { return _ImageUrlsFilename; }
            set
            {
                _ImageUrlsFilename = value;
                RaisePropertyChanged();
            }
        }

        bool _UseImageUrlsLocalFile = false;
        public bool UseImageUrlsLocalFile
        {
            get { return _UseImageUrlsLocalFile; }
            set { _UseImageUrlsLocalFile = value; RaisePropertyChanged(); }
        }

        Dictionary<string, List<NetworkFile>> _ImagesUrls = new Dictionary<string, List<NetworkFile>>();
        public Dictionary<string, List<NetworkFile>> ImagesUrls
        {
            get { return _ImagesUrls; }
            set
            {
                _ImagesUrls = value;
                int count = 0;
                foreach (var list in _ImagesUrls.Values) count += list.Count;
                ImageUrlsCount = count;
            }
        }

        int _ImageUrlsCount = 0;
        public int ImageUrlsCount
        {
            get { return _ImageUrlsCount; }
            set { _ImageUrlsCount = value; RaisePropertyChanged(); }
        }

        string _ImageCsvFilename;
        public string ImageCsvFilename
        {
            get { return _ImageCsvFilename; }
            set
            {
                _ImageCsvFilename = value;
                RaisePropertyChanged();
            }
        }

        Dictionary<string, List<string>> _LocalImages = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> LocalImages
        {
            get { return _LocalImages; }
            set
            {
                _LocalImages = value;
                int count = 0;
                foreach (var list in _LocalImages.Values) count += list.Count;
                LocalImagesCount = count;
            }
        }

        int _LocalImagesCount = 0;
        public int LocalImagesCount
        {
            get { return _LocalImagesCount; }
            set { _LocalImagesCount = value; RaisePropertyChanged(); }
        }

        ArkiveImages.DiffResult _Diff = new ArkiveImages.DiffResult();
        public ArkiveImages.DiffResult Diff
        {
            get { return _Diff; }
            set { _Diff = value; RaisePropertyChanged(); RaisePropertyChanged("DiffDesc"); }
        }

        public string DiffDesc
        {
            get { return "new images: " + Diff.MissingImages + ", old images: " + Diff.OldImages; }
        }




    }
}
