using Flurl;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using TreeOfLife.TaxonDatas;

namespace TreeOfLife
{
    public static class TOLData
    {
        public static string serverUrl = "";

        private static string soundsUrl { get; set; } = "";

        public static bool offline { get; set; } = false;

        public static List<string> availableSounds { get; set; } = new List<string>();

        public static string AppDataDirectory()
        {
            string globalAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(globalAppDataFolder, "TOL");
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

            try
            {
                using (WebClient client = new WebClient())
                {
                    string collection = string.Empty;

                    collection = client.DownloadString(soundsUrl);

                    availableSounds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(collection);
                }
            } catch (WebException e)
            {
                Loggers.WriteError(LogTags.Sound, "Unable to fetch sound collection from server. Reason is " + e.ToString());
            }
        }

        public static string ImageDataPath()
        {
            return Path.Combine(TaxonUtils.MyConfig.rootDirectory, "Datas", "Images");
        }

        internal static string CommentDataPath()
        {
            return Path.Combine(TaxonUtils.MyConfig.rootDirectory, "Datas", "Comments");
        }

        public static string SoundsDataPath() {
            return Path.Combine(TaxonUtils.MyConfig.rootDirectory, "Datas", "Sounds");
        }

        public static string SoundsUrl()
        {
            return Url.Combine(TaxonUtils.MyConfig.serverUrl, "sounds");
        }

        public static string FindSound(TaxonTreeNode taxon)
        {
            if (TaxonUtils.MyConfig.offline) return null;

            string path = Path.Combine(SoundsDataPath(), taxon.Desc.RefMultiName.Main) + ".wma";
            Console.WriteLine("path : " + path);
            if (! offline) {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(Url.Combine(SoundsUrl(), taxon.Desc.RefMultiName.Main), path);
                }
            }

            return path;
        }

        public static string LocationPath(string taxonFileName)
        {
            return Path.Combine(TaxonUtils.MyConfig.rootDirectory, "Datas", taxonFileName + "_location");
        }

        public static void SaveConfigAfterInitialization()
        {

        }
    }
}