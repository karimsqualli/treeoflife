using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeOfLife
{
    public partial class TagBatchImportDialog : Form
    {
        public TagBatchImportDialog()
        {
            InitializeComponent();
            textBoxSource.Text = TaxonUtils.MyConfig.Options.TagBatchImportSourceFolder;
            textBoxDestination.Text = TaxonUtils.MyConfig.Options.TagBatchImportDestinationFolder;
            switch (TaxonUtils.MyConfig.Options.TagBatchImportOverwrite)
            {
                case "always": radioButtonOverwrite.Checked = true; break;
                case "never": radioButtonLeaveIt.Checked = true; break;
            }

            Task.Factory.StartNew(() =>
            {
                _Searchtool = new TaxonSearch(TaxonUtils.OriginalRoot, true, true);
                Console.WriteLine("_Searchtool completed");
            });
        }

        TaxonSearch _Searchtool;

        private void TagBatchImportDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            TaxonUtils.MyConfig.Options.TagBatchImportSourceFolder = textBoxSource.Text;
            TaxonUtils.MyConfig.Options.TagBatchImportDestinationFolder = textBoxDestination.Text;
            if (radioButtonOverwrite.Checked) TaxonUtils.MyConfig.Options.TagBatchImportOverwrite = "always";
            if (radioButtonLeaveIt.Checked) TaxonUtils.MyConfig.Options.TagBatchImportOverwrite = "never";
            if (radioButtonOverwriteOlder.Checked) TaxonUtils.MyConfig.Options.TagBatchImportOverwrite = "ifolder";
        }

        private void ButtonBrowseSource_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select source folder";
                if (textBoxSource.Text != "")
                    fbd.SelectedPath = textBoxSource.Text;
                else
                    fbd.SelectedPath = TaxonUtils.GetTaxonPath();
                DialogResult result = fbd.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    return;
                textBoxSource.Text = fbd.SelectedPath;
            }
        }

        private void ButtonBrowseDestination_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select destination folder";
                if (textBoxDestination.Text != "")
                    fbd.SelectedPath = textBoxDestination.Text;
                else
                    fbd.SelectedPath = TaxonUtils.GetTaxonPath();
                DialogResult result = fbd.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    return;
                textBoxDestination.Text = fbd.SelectedPath;
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            string logFile = Path.Combine(TaxonUtils.GetLogPath(), "TagBatchImport.log");
            using (StreamWriter log = new StreamWriter(logFile))
            {
                string[] srcFiles = null;
                bool error = false;
                if (!Directory.Exists(textBoxSource.Text))
                {
                    log.WriteLine("Folder " + textBoxSource.Text + " does not exists");
                    error = true;
                }
                else
                {
                    try
                    {
                        srcFiles = Directory.GetFiles(textBoxSource.Text, "*.txt");
                        if (srcFiles.Length == 0)
                        {
                            log.WriteLine("Folder " + textBoxSource.Text + " contains no txt files");
                            error = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("Exception while getting list of txt file in folder " + textBoxSource.Text);
                        log.WriteLine("    " + ex.Message);
                        error = true;
                    }
                }

                if (!Directory.Exists(textBoxDestination.Text))
                {
                    log.WriteLine("Folder " + textBoxDestination.Text + " does not exists");
                    error = true;
                }

                if (error)
                {
                    log.WriteLine("Initialization error, stop batch import");
                    return;
                }

                bool overwriteAlways = radioButtonOverwrite.Checked;
                bool overwriteNever = radioButtonLeaveIt.Checked;
                int skippedFiles = 0;
                int errorFiles = 0;

                foreach ( string file in srcFiles)
                {
                    log.WriteLine();
                    log.WriteLine("--------------------------------------------------");
                    log.WriteLine("---> Import " + file);

                    string destinationFile = Path.Combine(textBoxDestination.Text, Path.GetFileNameWithoutExtension(file));
                    destinationFile = Path.ChangeExtension(destinationFile, ".lot");
                    if (File.Exists(destinationFile))
                    {
                        if (overwriteNever)
                        {
                            log.WriteLine("Import skipped, file " + destinationFile + " already exists");
                            skippedFiles++;
                            continue;
                        }
                        else if (!overwriteAlways)
                        {
                            DateTime datedest = File.GetLastWriteTime(destinationFile);
                            DateTime datesrc = File.GetLastWriteTime(file);
                            if (datedest >= datesrc )
                            {
                                log.WriteLine("Import skipped, file " + destinationFile + " already exists and is newer than source file");
                                skippedFiles++;
                                continue;
                            }
                        }

                        try
                        {
                            File.Delete(destinationFile);
                        }
                        catch
                        {
                            log.WriteLine("Import skipped, file " + destinationFile + " already exists");
                            log.WriteLine("And got an error while trying to delete it");
                            errorFiles++;
                            continue;
                        }
                    }

                    while (_Searchtool == null)
                    {
                        Console.WriteLine("Wait search tool");
                        Thread.Sleep(100);
                    }

                    TaxonList.ImportFileResult result = TaxonList.ImportFile(file, _Searchtool, false);

                    TaxonList list = new TaxonList { HasFile = true, FileName = destinationFile };
                    list.FromTaxonTreeNodeList(result.List);

                    try
                    {
                        list.Save(false, TaxonList.FileFilterIndexEnum.ListOfTaxons);
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("Exception while saving list to " + destinationFile);
                        log.WriteLine("    " + ex.Message);
                        errorFiles++;
                        continue;
                    }

                    log.WriteLine(string.Format("    taxons found: {0}", result.TaxonsFound));
                    log.WriteLine(string.Format("    taxons not found: {0}", result.TaxonNotFound));
                    log.WriteLine(string.Format("for more details, look at " + result.LogFilename + " file"));
                    log.WriteLine(string.Format("==> Saved in " + destinationFile));
                }

                string message = "Batch import done,\n";
                message += string.Format("{0} total files scanned\n", srcFiles.Length);
                message += string.Format("{0} files skipped\n", skippedFiles);
                message += string.Format("{0} files not imported due to an error\n", errorFiles);
                message += string.Format("{0} files imported\n", srcFiles.Length - skippedFiles - errorFiles);
                message += string.Format("for more details, look at {0}", logFile);
                Loggers.WriteInformation(LogTags.Data, message);
                log.WriteLine("");
                log.WriteLine("==================================================");
                log.Write(message);
            }
        }
    }
}
