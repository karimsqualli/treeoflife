using Flurl;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    public partial class InitForm : Form
    {
        public string AppDataFolder { get; set; } = "";

        public InitForm(string appDataFolder)
        {
            this.AppDataFolder = appDataFolder;
            TopMost = true;

            InitializeComponent();

            dataDirectoryTextBox.Enabled = false;
            selectDataDirectoryButton.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            urlServerTextBox.Enabled = true;
            dataDirectoryTextBox.Enabled = false;
            selectDataDirectoryButton.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataDirectoryTextBox.Enabled = true;
            selectDataDirectoryButton.Enabled = true;
            urlServerTextBox.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void validateButton_Click(object sender, EventArgs e)
        {
            bool success = false;
            bool offline = offlineModeButton.Checked;

            if (! offline)
            {
                success = downloadInitData(urlServerTextBox.Text);
            } else
            {
                success = selectOffLineDataFolder();
            }

            if (success)
            {
                Close();
            }
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
                errorLabel.Text = "Folder does not contains 'Comments' subfolder";

                return false;
            }

            if (!Directory.Exists(Path.Combine(selectedFolder, "Datas", "Images")))
            {
                errorLabel.Text = "Folder does not contains 'Images' subfolder";

                return false;
            }

            if (!Directory.Exists(Path.Combine(selectedFolder, "Datas", "Sounds")))
            {
                errorLabel.Text = "Folder does not contains 'Sounds' subfolder";

                return false;
            }

            TOLData.offline = true;
            TOLData.rootDirectory = selectedFolder;

            TOLData.SaveConfigAfterInitialization();

            return true;
        }

        private bool downloadInitData(string serverUrl)
        {
            string zipFilePath = Path.Combine(AppDataFolder, "init.zip");

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


            if (!Directory.Exists(AppDataFolder))
            {
                Directory.CreateDirectory(AppDataFolder);
            }
            else
            {
                string soundsDataPath = TOLData.SoundsDataPath();
                if (Directory.Exists(soundsDataPath))
                {
                    DirectoryInfo soundsDir = new DirectoryInfo(soundsDataPath);
                    soundsDir.Delete(true);
                }

                string imageDataPath = TOLData.ImageDataPath();
                if (Directory.Exists(imageDataPath))
                {
                    DirectoryInfo imagesDir = new DirectoryInfo(imageDataPath);
                    imagesDir.Delete(true);
                }

                string commentDataPath = TOLData.CommentDataPath();
                if (Directory.Exists(commentDataPath))
                {
                    DirectoryInfo commentsDir = new DirectoryInfo(commentDataPath);
                    commentsDir.Delete(true);
                }

                string locationDataPath = TOLData.LocationPath(TaxonUtils.MyConfig.TaxonFileName);
                if (Directory.Exists(locationDataPath))
                {
                    DirectoryInfo locationDir = new DirectoryInfo(locationDataPath);
                    locationDir.Delete(true);
                }
            }

            ZipFile zip = ZipFile.Read(zipFilePath);
            zip.ExtractAll(AppDataFolder, ExtractExistingFileAction.OverwriteSilently);

            TOLData.offline = false;
            TOLData.rootDirectory = TOLData.appDataDirectory;

            TOLData.SaveConfigAfterInitialization();

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
