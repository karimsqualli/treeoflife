using Flurl;
using Ionic.Zip;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using TreeOfLife.TaxonDatas;

namespace TreeOfLife
{
    public class TolDatas
    {
        private static string tolAppDataFolder;
        private static string soundsUrl = "";

        static TolDatas()
        {
            string globalAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tolAppDataFolder = Path.Combine(globalAppDataFolder, "TOL");
        }

        public static void Init()
        {
            FormAbout.SetSplashScreenMessage(".. Initializing data ...");
            string dataUrl = "";

            if (!Directory.Exists(tolAppDataFolder))
            {
                Directory.CreateDirectory(tolAppDataFolder);
            } else {
                string soundsDataPath = SoundsDataPath();
                if (Directory.Exists(soundsDataPath))
                {
                    DirectoryInfo soundsDir = new DirectoryInfo(SoundsDataPath());
                    soundsDir.Delete(true);
                }

                string imageDataPath = ImageDataPath();
                if (Directory.Exists(imageDataPath))
                {
                    DirectoryInfo imagesDir = new DirectoryInfo(ImageDataPath());
                    imagesDir.Delete(true);
                }

                string commentDataPath = CommentDataPath();
                if (Directory.Exists(commentDataPath))
                {
                    DirectoryInfo commentsDir = new DirectoryInfo(CommentDataPath());
                    commentsDir.Delete(true);
                }

                string locationDataPath = LocationPath(TaxonUtils.MyConfig.TaxonFileName);
                if (Directory.Exists(locationDataPath))
                {
                    DirectoryInfo locationDir = new DirectoryInfo(locationDataPath);
                    locationDir.Delete(true);
                }
            }

            string zipFilePath = Path.Combine(tolAppDataFolder, "init.zip");

            bool ok = false;
            while (!ok)
            {

                dataUrl = Microsoft.VisualBasic.Interaction.InputBox("Veuillez saisir l'adresse du serveur", "Accès au serveur", "http://localhost:8888/", 0, 0);

                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(Url.Combine(dataUrl, "appdata", "init.zip"), zipFilePath);
                    }

                    ok = true;
                }
                catch (WebException e)
                {
                    Loggers.WriteError(LogTags.Program, "error while fetching initialization data");
                }

            }

            ZipFile zip = ZipFile.Read(zipFilePath);
            zip.ExtractAll(tolAppDataFolder, ExtractExistingFileAction.OverwriteSilently);

            TaxonUtils.MyConfig.dataInitialized = true;
        }

        public static void initSounds()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SoundCollection));
            using (FileStream fileStream = new FileStream(Path.Combine(SoundsDataPath(), "_infos.xml"), FileMode.Open))
            {
                SoundCollection result = (SoundCollection)serializer.Deserialize(fileStream);
                soundsUrl = result.Location;
            }
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
                client.DownloadFile(Url.Combine(soundsUrl, currentTaxon.Desc.RefMultiName.Main), path);
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