using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace TreeOfLife.TaxonDialog
{
    public partial class GenerateNewDatabaseDialog : Localization.Form
    {
        public GenerateNewDatabaseDialog(TaxonTreeNode _taxon, GenerateNewDatabaseConfig _config )
        {
            InitializeComponent();
            _Taxon = _taxon;
            _Config = _config;
            if (_Taxon == null) return;
            labelTaxon.Text = _Taxon.Desc.RefMainName;
            labelTaxon.Tag = new Localization.Tag { Ignore = true };
            labelFolder.Text = Path.Combine(TaxonUtils.GetTaxonPath(),"NewDatas");
            ApplyConfig();
        }

        TaxonTreeNode _Taxon;
        GenerateNewDatabaseConfig _Config;

        private void GenerateNewDatabaseDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        void ApplyConfig()
        {
            if (_Config == null) return;
            if (!String.IsNullOrEmpty(_Config.Folder))
                labelFolder.Text = _Config.Folder;
            checkBoxTaxonNameAsSubFolder.Checked = _Config.TaxonNameAsSubFolder;
            checkBoxExe.Checked = _Config.CopyExe;
            checkBoxExportAscendants.Checked = _Config.ExportAscendants;
            checkBoxExportComments.Checked = _Config.CopyComments;
            checkBoxExportPhotos.Checked = _Config.CopyImages;
            checkBoxExportSounds.Checked = _Config.CopySounds;
        }

        void SaveConfig()
        {
            if (_Config == null) return;
            _Config.Folder = labelFolder.Text;
            _Config.TaxonNameAsSubFolder = checkBoxTaxonNameAsSubFolder.Checked;
            _Config.CopyExe = checkBoxExe.Checked;
            _Config.ExportAscendants = checkBoxExportAscendants.Checked;
            _Config.CopyComments = checkBoxExportComments.Checked;
            _Config.CopyImages = checkBoxExportPhotos.Checked;
            _Config.CopySounds = checkBoxExportSounds.Checked;
        }


        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select Folder where new database will be saved";
                fbd.SelectedPath = TaxonUtils.GetTaxonPath();
                DialogResult result = fbd.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    return;
                labelFolder.Text = fbd.SelectedPath;
            }
        }

        private void buttonClean_Click(object sender, EventArgs e)
        {
            string folder = BuildFolder();
            if (!Directory.Exists(folder))
                MessageBox.Show("Directory doesn't exits\nNothing to clean !", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                DialogResult result = MessageBox.Show("Remove directory\n    " + folder + "\nAre you sure ?", "???", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;

                try
                {
                    Directory.Delete(folder, true);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private string BuildFolder()
        {
            string folder = labelFolder.Text;
            if (checkBoxTaxonNameAsSubFolder.Checked) folder = Path.Combine(folder, labelTaxon.Text);
            return folder;
        }

        private bool CheckFolder()
        {
            string folder = BuildFolder();
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            DirectoryInfo dir1 = new DirectoryInfo(folder);
            DirectoryInfo dir0 = new DirectoryInfo(TaxonUtils.GetTaxonPath());

            return (dir0.FullName.ToLower() != dir1.FullName.ToLower());
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (!CheckFolder()) return;

            using (ProgressDialog progressDlg = new ProgressDialog())
            {
                progressDlg.StartPosition = FormStartPosition.CenterScreen;
                progressDlg.Show();

                string folder = BuildFolder();
                string filename = labelTaxon.Text + ".tol";

                string logFile = Path.Combine(folder, "generate.log");
                string treeFileTol = Path.Combine(folder, filename);
                string treeFileXml = Path.ChangeExtension(treeFileTol, "xml");

                bool comments = checkBoxExportComments.Checked;
                bool photos = checkBoxExportPhotos.Checked;
                bool sounds = checkBoxExportSounds.Checked;
                bool exe = checkBoxExe.Checked;
                bool ascendants = checkBoxExportAscendants.Checked;


                using (StreamWriter log = new StreamWriter(logFile))
                {
                    log.WriteLine("Start generate data for ");
                    log.WriteLine("  taxon   : " + _Taxon.Desc.RefMainName);
                    log.WriteLine("  in graph: " + TaxonUtils.MainGraph?.Description);

                    TaxonTreeNode nodeToExport = _Taxon;
                    if (ascendants)
                    {
                        TaxonTreeNode Current = _Taxon;
                        while (Current != null && Current.Father != null)
                        {
                            TaxonTreeNode fatherNode = new TaxonTreeNode();
                            fatherNode.Father = Current.Father.Father;
                            fatherNode.Desc = Current.Father.Desc;
                            fatherNode.Children.Add(nodeToExport);
                            nodeToExport = fatherNode;
                            Current = Current.Father;
                        }
                    }

                    ProgressItem piSaveData = progressDlg.Add("Saving main file", null, 0, 3);
                    log.WriteLine("");
                    log.WriteLine("Save tree in " + treeFileXml);
                    TaxonUtils.Save(nodeToExport, treeFileXml);
                    piSaveData.Update(1);
                    log.WriteLine("Save tree in " + treeFileTol);
                    TaxonUtils.Save(nodeToExport, treeFileTol);
                    piSaveData.Update(2);

                    int count = 0;
                    nodeToExport.ParseNode((node) => { count++; });
                    piSaveData.Update(3);
                    piSaveData.End();

                    log.WriteLine("");
                    log.WriteLine("Total nodes    : " + count);
                    log.WriteLine("Export ascendants: " + ascendants);
                    log.WriteLine("Export comments: " + comments);
                    log.WriteLine("Export images  : " + photos);
                    log.WriteLine("Export sounds  : " + sounds);

                    ExportData exportData = new ExportData(TaxonUtils.GetTaxonPath(), folder, log);

                    if (comments)
                    {
                        log.WriteLine("");
                        log.WriteLine("Exporting comments");
                        exportData.ProgressItem = progressDlg.Add("Exporting comments", "", 0, count - 1);
                        nodeToExport.ParseNode((node) => ExportComments(node, exportData));
                        exportData.ProgressItem.End();
                    }

                    if (photos)
                    {
                        log.WriteLine("");
                        log.WriteLine("Exporting photos");
                        exportData.ProgressItem = progressDlg.Add("Exporting photos", "", 0, count - 1);
                        nodeToExport.ParseNodeDesc((desc) => ExportPhotos(desc, exportData));
                        exportData.ProgressItem.End();
                    }

                    if (sounds)
                    {
                        log.WriteLine("");
                        log.WriteLine("Exporting sounds");
                        exportData.ProgressItem = progressDlg.Add("Exporting sounds", "", 0, count - 1);
                        nodeToExport.ParseNodeDesc((desc) => ExportSounds(desc, exportData));
                        exportData.ProgressItem.End();
                    }

                    if (exe)
                    {
                        exportData.ProgressItem = progressDlg.Add("Exporting exe/config", "", 0, 2);
                        string sourceExe = Assembly.GetEntryAssembly().Location;
                        string destExe = Path.Combine(exportData.NewPath, Path.GetFileName(sourceExe));
                        File.Copy(sourceExe, destExe);
                        exportData.ProgressItem.Update(1);

                        string sourceConfig = TaxonUtils.GetConfigFileName(TaxonUtils.MyConfig.Name);
                        string destConfig = Path.Combine(exportData.NewPath, "Config", Path.GetFileName(sourceConfig));

                        string saveFileName = TaxonUtils.MyConfig.TaxonFileName;
                        string savePath = TaxonUtils.MyConfig.TaxonPath;
                        TaxonUtils.MyConfig.TaxonFileName = filename;
                        TaxonUtils.MyConfig.TaxonPath = folder;
                        TaxonUtils.MyConfig.Save(destConfig, destExe);
                        TaxonUtils.MyConfig.TaxonFileName = saveFileName;
                        TaxonUtils.MyConfig.TaxonPath = savePath;
                        exportData.ProgressItem.Update(2);
                        exportData.ProgressItem.End();
                    }
                }

                MessageBox.Show("Generate done");
            }
            Close();
        }

        class ExportData
        {
            public ExportData( string _old, string _new, StreamWriter _log )
            {
                OldPath = _old.ToLower() + Path.DirectorySeparatorChar;
                OldPathLength = OldPath.Length;
                NewPath = _new;
                Log = _log;
            }

            public string Transform( string _old )
            {
                if (!_old.ToLower().StartsWith(OldPath)) return null;
                string sub = _old.Substring(OldPathLength);
                return Path.Combine(NewPath, sub);
            }

            public string NewPath;
            public string OldPath;
            public int OldPathLength;

            public StreamWriter Log;

            public ProgressItem ProgressItem;

            public Dictionary<string, bool> CollectionDone = new Dictionary<string, bool>();
        }
        
        bool CopyFile( string _filename, bool _testDirectory, ExportData _data )
        {
            string newfile = _data.Transform(_filename);
            if (newfile == null) return false;
            if (_testDirectory)
            {
                string folder = Path.GetDirectoryName(newfile);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
            if (File.Exists(_filename) && !File.Exists(newfile))
                File.Copy(_filename, newfile);
            return true;
        }

        bool CopyDirectory(string _folder, ExportData _data)
        {
            string newFolder = _data.Transform(_folder);
            if (newFolder == null) return false;
            CopyDir.Copy(_folder, newFolder);
            return true;
        }

        void CopyCollectionInfo( string _folder, ExportData _data)
        {
            if (_data.CollectionDone.ContainsKey(_folder)) return;
            _data.CollectionDone[_folder] = true;
            
            string newFolder = _data.Transform( _folder) ;
            if (newFolder == null)
            {
                _data.Log.WriteLine("Error transferring folder " + _folder + " in CopyCollecionInfo");
                return;
            }

            Directory.CreateDirectory(newFolder);
            string fileDesc = Path.Combine(_folder, "_infos.xml");
            if (File.Exists(fileDesc))
                CopyFile(fileDesc, false, _data);

            _data.Log.WriteLine("CopyCollectionInfo " + _folder);
            _data.Log.WriteLine("    new folder: " + newFolder + ", " + (Directory.Exists(newFolder) ? "created" : "error, not created"));
            _data.Log.WriteLine("    collection info : " + (File.Exists(_data.Transform(fileDesc)) ? "created" : "error, not created"));
        }

        void ExportComments(TaxonTreeNode _node, ExportData _data)
        {
            _data.ProgressItem.Update(_data.ProgressItem.Current + 1);
            List<CommentFileDesc> comments = TaxonComments.GetAllCommentFile(_node);
            if (comments == null) return;
            foreach (CommentFileDesc desc in comments)
            {
                if (desc.Collection == null)
                {
                    _data.Log.WriteLine("error: " + _node.Desc.RefMainName + " comment collection is null" );
                    continue;
                }

                CopyCollectionInfo(desc.Collection.Path, _data);
                CopyFile(desc.GetHtmlName(), false, _data);
                CopyDirectory(desc.GetHtmlFilesDir(), _data);
            }
        }

        void ExportPhotos(TaxonDesc desc, ExportData _data)
        {
            _data.ProgressItem.Update(_data.ProgressItem.Current + 1);
            if (desc.Images == null) return;
            foreach (TaxonImageDesc image in desc.Images)
            {
                if (image.IsALink) continue;
                ImageCollection collection = image.GetCollection();
                if (collection == null)
                {
                    _data.Log.WriteLine("error: " + desc.RefMainName + " image collection is null");
                    continue;
                }
                CopyCollectionInfo(collection.Path, _data);
                CopyFile(image.GetPath(desc), false, _data);
            }
        }

        void ExportSounds(TaxonDesc desc, ExportData _data)
        {
            _data.ProgressItem.Update(_data.ProgressItem.Current + 1);
            if (desc.HasSound)
                CopyFile(TaxonUtils.GetSoundFullPath(desc), true, _data);
        }

        
    }

    public class GenerateNewDatabaseConfig
    {
        public GenerateNewDatabaseConfig() { }

        public string Folder = null;
        public bool TaxonNameAsSubFolder = false;
        public bool CopyExe = true;
        public bool CopyComments = true;
        public bool CopyImages = true;
        public bool CopySounds = true;
        public bool ExportAscendants = true;

    }

    class CopyDir
    {
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            if (!Directory.Exists(sourceDirectory))
                return;
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
