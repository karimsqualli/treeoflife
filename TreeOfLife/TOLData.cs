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
        public static string rootDataFolder { get; set; } = "";

        private static string tolAppDataFolder { get; set; } = "";
        private static string soundsUrl { get; set; } = "";

        public static bool offline { get; set; } = false;

        static TOLData()
        {
            string globalAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tolAppDataFolder = Path.Combine(globalAppDataFolder, "TOL");
            rootDataFolder = tolAppDataFolder;
        }

        public static void Init()
        {
            FormAbout.SetSplashScreenMessage(".. Initializing data ...");

            new InitForm(tolAppDataFolder).ShowDialog();
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
            return Path.Combine(rootDataFolder, "Datas", "Images");
        }

        internal static string CommentDataPath()
        {
            return Path.Combine(rootDataFolder, "Datas", "Comments");
        }

        public static string SoundsDataPath() {
            return Path.Combine(rootDataFolder, "Datas", "Sounds");
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
            return Path.Combine(rootDataFolder, "Datas", taxonFileName + "_location");
        }

        public static string DataFolder()
        {
            return tolAppDataFolder;
        }
    }
}