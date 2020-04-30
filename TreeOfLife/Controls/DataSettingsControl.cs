using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Flurl;
using Ionic.Zip;

namespace TreeOfLife.Controls
{
    public partial class DataSettingsControl : Localization.UserControl
    {
        public DataSettingsControl()
        {
            InitializeComponent();

            dataDirectoryTextBox.Enabled = false;
            selectDataDirectoryButton.Enabled = false;
        }

        private void onlineModeButton_CheckedChanged(object sender, EventArgs e)
        {
            urlServerTextBox.Enabled = true;
            dataDirectoryTextBox.Enabled = false;
            selectDataDirectoryButton.Enabled = false;
        }

        private void offlineModeButton_CheckedChanged(object sender, EventArgs e)
        {
            selectDataDirectoryButton.Enabled = true;
            urlServerTextBox.Enabled = false;
        }

        public bool updateData()
        {
            bool success = false;
            bool offline = offlineModeButton.Checked;

            if (!offline)
            {
                success = downloadInitData();
            }
            else
            {
                success = selectOffLineDataFolder();
            }

            return success;
        }

        private bool downloadInitData()
        {
            string appDataFolder = TOLData.AppDataDirectory();
            string serverUrl = urlServerTextBox.Text;
            string zipFilePath = Path.Combine(appDataFolder, "init.zip");

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(Url.Combine(serverUrl, "appdata", "init.zip"), zipFilePath);
                }
            }
            catch (WebException e)
            {
                errorLabel.Text = "Can't download init data from " + serverUrl;
                return false;
            }


            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
            }
            else
            {
                string soundsDataPath = Path.Combine(TOLData.AppDataDirectory(), "Datas", "Sounds");
                if (Directory.Exists(soundsDataPath))
                {
                    DirectoryInfo soundsDir = new DirectoryInfo(soundsDataPath);
                    soundsDir.Delete(true);
                }

                string imageDataPath = Path.Combine(TOLData.AppDataDirectory(), "Datas", "Images");
                if (Directory.Exists(imageDataPath))
                {
                    DirectoryInfo imagesDir = new DirectoryInfo(imageDataPath);
                    imagesDir.Delete(true);
                }

                string commentDataPath = Path.Combine(TOLData.AppDataDirectory(), "Datas", "Comments");
                if (Directory.Exists(commentDataPath))
                {
                    DirectoryInfo commentsDir = new DirectoryInfo(commentDataPath);
                    commentsDir.Delete(true);
                }

                string locationDataPath = Path.Combine(TOLData.AppDataDirectory(), "Datas", TaxonUtils.MyConfig.TaxonFileName + "_location");
                if (Directory.Exists(locationDataPath))
                {
                    DirectoryInfo locationDir = new DirectoryInfo(locationDataPath);
                    locationDir.Delete(true);
                }
            }

            ZipFile zip = ZipFile.Read(zipFilePath);
            zip.ExtractAll(appDataFolder, ExtractExistingFileAction.OverwriteSilently);

            TOLData.offline = false;
            TaxonUtils.MyConfig.rootDirectory = TOLData.AppDataDirectory();
            TaxonUtils.MyConfig.serverUrl = urlServerTextBox.Text;
            TaxonUtils.MyConfig.offline = false;
            TaxonUtils.MyConfig.dataInitialized = true;

            return true;
        }

        private bool selectOffLineDataFolder()
        {
            string selectedFolder = dataDirectoryTextBox.Text;

            if (string.IsNullOrWhiteSpace(selectedFolder))
            {
                return false;
            }

            if (!Directory.Exists(Path.Combine(selectedFolder, "Datas", "Comments")))
            {
                errorLabel.Text = "Folder does not contains 'Datas\\Comments' subfolder";

                return false;
            }

            if (!Directory.Exists(Path.Combine(selectedFolder, "Datas", "Images")))
            {
                errorLabel.Text = "Folder does not contains 'Datas\\Images' subfolder";

                return false;
            }

            if (!Directory.Exists(Path.Combine(selectedFolder, "Datas", "Sounds")))
            {
                errorLabel.Text = "Folder does not contains 'Datas\\Sounds' subfolder";

                return false;
            }

            TOLData.offline = true;
            TaxonUtils.MyConfig.rootDirectory = selectedFolder;

            TaxonUtils.MyConfig.offline = true;
            TaxonUtils.MyConfig.dataInitialized = true;
            TaxonUtils.MyConfig.serverUrl = "";

            return true;
        }

        private void selectDataDirectoryButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    errorLabel.Text = "Invalid directory";
                }

                dataDirectoryTextBox.Text = dialog.SelectedPath;
            }
        }
    }
}
