using Flurl;
using Ionic.Zip;
using System;
using System.IO;
using System.Net;

namespace TreeOfLife
{
    public class TolDatas
    {
        private string tolAppDataFolder;


        public void Init()
        {
            string globalAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tolAppDataFolder = Path.Combine(globalAppDataFolder, "TOL");

            if (! Directory.Exists(tolAppDataFolder))
            {
                Directory.CreateDirectory(tolAppDataFolder);
                fetchInitData(tolAppDataFolder);
            }
        }

        private void fetchInitData(string folder)
        {
            string zipFilePath = Path.Combine(folder, "init.zip");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri("http://localhost:8888/appdata/init.zip"), zipFilePath);
            }

            ZipFile zip = ZipFile.Read(zipFilePath);
            zip.ExtractAll(folder, ExtractExistingFileAction.OverwriteSilently);
        }

        public string ImageDataPath()
        {
            return Path.Combine(tolAppDataFolder, "Datas", "Images");
        }

        internal string CommentDataPath()
        {
            return Path.Combine(tolAppDataFolder, "Datas", "Comments");
        }


        public string SoundsDataPath() {
            return Path.Combine(tolAppDataFolder, "Datas", "Sounds");
        }

        internal string DownloadSound(TaxonTreeNode currentTaxon)
        {
            string path = Path.Combine(SoundsDataPath(), currentTaxon.Desc.RefMultiName.Main) + ".wma";

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Url.Combine("http://localhost:8888", "sounds", currentTaxon.Desc.RefMultiName.Main), path);
            }

            return path;
        }
    }
}