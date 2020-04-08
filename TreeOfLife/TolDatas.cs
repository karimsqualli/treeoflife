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
            FormAbout.SetSplashScreenMessage(".. Initializing data ...");

            Init();
        }

        static void Init()
        {
            string globalAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tolAppDataFolder = Path.Combine(globalAppDataFolder, "TOL");
            string dataUrl = "";

            if (! Directory.Exists(tolAppDataFolder))
            {
                Directory.CreateDirectory(tolAppDataFolder);


                string zipFilePath = Path.Combine(tolAppDataFolder, "init.zip");

                bool ok = false;
                while (! ok )
                {

                    dataUrl = Microsoft.VisualBasic.Interaction.InputBox("Veuillez saisir l'adresse du serveur", "Accès au serveur", "http://localhost:8888/", 0, 0);

                    try
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(Url.Combine(dataUrl, "appdata", "init.zip"), zipFilePath);
                        }

                        ok = true;
                    } catch (WebException e)
                    {
                        Loggers.WriteError(LogTags.Program, "error while fetching initialization data");
                    }

                }

                ZipFile zip = ZipFile.Read(zipFilePath);
                zip.ExtractAll(tolAppDataFolder, ExtractExistingFileAction.OverwriteSilently);
                soundsUrl = Url.Combine(dataUrl, "sounds");
            } else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SoundCollection));
                using (FileStream fileStream = new FileStream(Path.Combine(SoundsDataPath(), "_infos.xml"), FileMode.Open))
                {
                    SoundCollection result = (SoundCollection)serializer.Deserialize(fileStream);
                    soundsUrl = result.Location;
                }
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