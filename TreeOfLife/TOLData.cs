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
    public class TOLData
    {
        private static string soundsUrl { get; set; } = "";

        public static bool offline { get; set; } = false;
        public static string rootDirectory { get; set; } = "";
        public static string appDataDirectory { get; internal set; } = "";

        static TOLData()
        {
            string globalAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appDataDirectory = Path.Combine(globalAppDataFolder, "TOL");
            rootDirectory = appDataDirectory;
        }

        public static void Init()
        {
            FormAbout.SetSplashScreenMessage(".. Initializing data ...");

            new InitForm(appDataDirectory).ShowDialog();
        }

        public static void initSounds()
        {
            if (offline)
            {
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(SoundCollection));
            using (FileStream fileStream = new FileStream(Path.Combine(SoundsDataPath(), "_infos.xml"), FileMode.Open))
            {
                SoundCollection result = (SoundCollection)serializer.Deserialize(fileStream);
                soundsUrl = result.Location;
            }
        }

        public static string ImageDataPath()
        {
            return Path.Combine(rootDirectory, "Datas", "Images");
        }

        internal static string CommentDataPath()
        {
            return Path.Combine(rootDirectory, "Datas", "Comments");
        }

        public static string SoundsDataPath() {
            return Path.Combine(rootDirectory, "Datas", "Sounds");
        }

        public static string FindSound(TaxonTreeNode taxon)
        {
            string path = Path.Combine(SoundsDataPath(), taxon.Desc.RefMultiName.Main) + ".wma";

            if (! offline) {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(Url.Combine(soundsUrl, taxon.Desc.RefMultiName.Main), path);
                }
            }

            return path;
        }

        public static string LocationPath(string taxonFileName)
        {
            return Path.Combine(rootDirectory, "Datas", taxonFileName + "_location");
        }

        public static void SaveConfigAfterInitialization()
        {
            TaxonUtils.MyConfig.offline = offline;
            TaxonUtils.MyConfig.rootDirectory = rootDirectory;
            TaxonUtils.MyConfig.dataInitialized = true;
        }
    }
}