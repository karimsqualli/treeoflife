using Flurl;
using Ionic.Zip;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TreeOfLife
{
    [XmlRoot("tol")]
    public class InitFile
    {
        [XmlElement("api")]
        public string api { get; set; }

    }

    public class TolDatas
    {
        private static string tolAppDataFolder;
        private static string dataUrl = "";

        static TolDatas()
        {
            FormAbout.SetSplashScreenMessage(".. Initializing data ...");

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose init file";
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "xml files (*.xml)|*.xml";
            ofd.Multiselect = false;
            ofd.AddExtension = true;


            ofd.ShowDialog();
            XmlSerializer serializer = new XmlSerializer(typeof(InitFile));
            using (FileStream fileStream = new FileStream(ofd.FileName, FileMode.Open))
            {
                InitFile file = (InitFile)serializer.Deserialize(fileStream);

                dataUrl = file.api;
            }

            Init();
        }

        static void Init()
        {
            string globalAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tolAppDataFolder = Path.Combine(globalAppDataFolder, "TOL");

            if (! Directory.Exists(tolAppDataFolder))
            {
                Directory.CreateDirectory(tolAppDataFolder);
                fetchInitData(tolAppDataFolder);
            }
        }

        private static void fetchInitData(string folder)
        {
            string zipFilePath = Path.Combine(folder, "init.zip");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Url.Combine(dataUrl, "appdata", "init.zip"), zipFilePath);
            }

            ZipFile zip = ZipFile.Read(zipFilePath);
            zip.ExtractAll(folder, ExtractExistingFileAction.OverwriteSilently);
        }

        public static string ImageDataPath()
        {
            return Path.Combine(tolAppDataFolder, "Datas", "Images");
        }

        internal static string CommentDataPath()
        {
            return Path.Combine(tolAppDataFolder, "Datas", "Comments");
        }

        public static string SoundsDataPath() {
            return Path.Combine(tolAppDataFolder, "Datas", "Sounds");
        }

        public static string DownloadSound(TaxonTreeNode currentTaxon)
        {
            string path = Path.Combine(SoundsDataPath(), currentTaxon.Desc.RefMultiName.Main) + ".wma";

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Url.Combine(dataUrl, "sounds", currentTaxon.Desc.RefMultiName.Main), path);
            }

            return path;
        }

        public static string LocationPath(string taxonFileName)
        {
            return Path.Combine(tolAppDataFolder, "Datas", taxonFileName + "_location");
        }

        public static string DataFolder()
        {
            return tolAppDataFolder;
        }
    }
}