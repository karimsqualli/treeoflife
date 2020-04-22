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
        private static string tolAppDataFolder { get; set; } = "";
        private static string soundsUrl { get; set; } = "";

        static TOLData()
        {
            string globalAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tolAppDataFolder = Path.Combine(globalAppDataFolder, "TOL");
        }

        public static void Init()
        {
            FormAbout.SetSplashScreenMessage(".. Initializing data ...");

            new InitForm(tolAppDataFolder).ShowDialog();
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