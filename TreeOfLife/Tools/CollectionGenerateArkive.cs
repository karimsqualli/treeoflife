using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TreeOfLife.Controls;
using TreeOfLife.TaxonDialog;

namespace TreeOfLife.Tools
{
    [Description("Generate Arkive Collection from Arkive Photos")]
    [DisplayName("Generate Arkive")]
    [SortName("z")]
    [Category("Tools/Collections")]
    [PresentInMode(false, false)]
    class GenerateArkiveCollection : ITool
    {
        public bool CanActivate()
        {
            return true;
        }

        public void Activate()
        {
            Task taskDeleteOldCollection = null;
            Task taskGatherExistentSpecies = null;
            Dictionary<string, int> existentSpecies = new Dictionary<string, int>();

            ImageCollection arkiveCollection = TaxonImages.Manager.GetByName("Arkive");
            if (arkiveCollection != null)
            {
                QuestionDialog dlg = new QuestionDialog
                (
                    "Arkive collection already exists !\nWhat do you want to do ?",
                    "Confirm ... ",
                    new TaxonDialog.AnswersDesc().
                        Add("Delete", "delete all content of old arkive collection", 0).
                        Add("Merge", "import only image for species newly found", 1).
                        Add("Cancel", "stop the generation of collection", 2)
                );
                dlg.ShowDialog();
                OneAnswerDesc answer = dlg.Answer;

                if (answer == null || answer.ID == 2)
                    return;

                /*string message = "Arkive collection exist, remove it ?";
                DialogResult result = MessageBox.Show(message, "Confirm ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;*/

                if (answer.ID == 0)
                    taskDeleteOldCollection = Task.Factory.StartNew(() => Directory.Delete(arkiveCollection.Path, true));
                else
                    taskGatherExistentSpecies = Task.Factory.StartNew(() =>
                        {
                            foreach (string file in Directory.EnumerateFiles(arkiveCollection.Path))
                            {
                                string species = Path.GetFileNameWithoutExtension(file).Split('_')[0];
                                if (!existentSpecies.ContainsKey(species))
                                    existentSpecies[species] = 0;
                                existentSpecies[species] = existentSpecies[species] + 1;
                            }
                        });
            }

            string folderArkive;
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select Folder where Arkive images are stored";
                fbd.SelectedPath = TaxonUtils.GetTaxonPath();
                DialogResult result = fbd.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    return;
                folderArkive = fbd.SelectedPath;
            }

            int lengthFolderArkive = folderArkive.Length;

            using (ProgressDialog progressDlg = new ProgressDialog())
            {
                progressDlg.StartPosition = FormStartPosition.CenterScreen;
                progressDlg.Show();

                ProgressItem piInitSearch = progressDlg.Add("Initialize searching", "", 0, 2);
                TaxonSearch searching = new TaxonSearch(TaxonUtils.OriginalRoot, true, true);
                piInitSearch.Update(1);
                string[] foldersLevel1 = Directory.GetDirectories(folderArkive);
                piInitSearch.Update(2);
                piInitSearch.End();

                Dictionary<string, string> unknownFolder = new Dictionary<string, string>();
                Dictionary<string, TaxonTreeNode> knownFolder = new Dictionary<string, TaxonTreeNode>();
                int missedPhotos = 0;
                int importedPhotos = 0;
                int alreadyImportedPhotos = 0;
                int newlyImportedPhotos = 0;

                ProgressItem piParse = progressDlg.Add("Parse arkive folders", "", 0, foldersLevel1.Length);
                for (uint i = 0; i < foldersLevel1.Length; i++)
                {
                    piParse.Update(i, foldersLevel1[i].Substring(lengthFolderArkive + 1));
                    int folder1Length = foldersLevel1[i].Length + 1;
                    
                    string[] foldersLevel2 = Directory.GetDirectories(foldersLevel1[i]);
                    foreach (string folder2 in foldersLevel2)
                    {
                        //if (folder2.ToLower().Contains("acropora"))
                        //    Console.WriteLine(folder2);

                        string[] photos = Directory.GetFiles(folder2, "*.jpg");
                        if (photos.Length == 0) continue;

                        string name = folder2.Substring(folder1Length).Replace('-', ' ').ToLower().Trim();
                        TaxonTreeNode node = searching.FindOne(name);
                        if (node == null)
                        {
                            unknownFolder[folder2] = name;
                            missedPhotos += photos.Length;
                        }
                        else
                            knownFolder[folder2] = node;
                    }
                }
                piParse.Update(piParse.Max, knownFolder.Count.ToString() + " found, " + unknownFolder.Count.ToString() + " not.");
                
                if (taskDeleteOldCollection != null && !taskDeleteOldCollection.IsCompleted)
                {
                    ProgressItem piClean = progressDlg.Add("Clean old collection", "", 0, 1);
                    taskDeleteOldCollection.Wait();
                    piClean.Update(1);
                    piClean.End();
                }

                if (taskGatherExistentSpecies != null && !taskGatherExistentSpecies.IsCompleted)
                {
                    ProgressItem piAnalyseOld = progressDlg.Add("Analyse old collection", "", 0, 1);
                    taskGatherExistentSpecies.Wait();
                    piAnalyseOld.Update(1);
                    piAnalyseOld.End();
                }

                arkiveCollection = TaxonImages.Manager.GetOrCreateCollection("Arkive");
                if (arkiveCollection == null)
                    return;
                arkiveCollection.Desc = "Collection generated from images taken from Arkive site : http://www.arkive.org";
                arkiveCollection.Desc += "Generated date : " + DateTime.Now.ToString();
                arkiveCollection.SaveInfos();
                ProgressItem piPopulate = progressDlg.Add("Populate collection", "", 0, knownFolder.Count);
                foreach (KeyValuePair<string, TaxonTreeNode> pair in knownFolder)
                {
                    string speciesName = pair.Value.Desc.RefMultiName.Main;
                    piPopulate.Update(piPopulate.Current + 1, speciesName);

                    string[] photos = Directory.GetFiles(pair.Key, "*.jpg");
                    importedPhotos += photos.Length;

                    if (existentSpecies.ContainsKey(speciesName))
                    {
                        if (existentSpecies[speciesName] == photos.Length)
                        {
                            alreadyImportedPhotos += photos.Length;
                            continue;
                        }
                        File.Delete(arkiveCollection.Path + Path.DirectorySeparatorChar + speciesName + "*.*");
                    }

                    newlyImportedPhotos += photos.Length;

                    for (int index =0; index < photos.Length; index ++)
                    {
                        string newName = speciesName + "_" + index.ToString() + ".jpg";
                        File.Copy(photos[index], arkiveCollection.Path + Path.DirectorySeparatorChar + newName);
                    }
                }
                piPopulate.Update(piPopulate.Max, importedPhotos.ToString() + " photos imported.");

                string message0 = (unknownFolder.Count + knownFolder.Count).ToString() + " total folders found\n";
                string message1 = knownFolder.Count.ToString() + " with associated taxons ( " + importedPhotos.ToString() + " photos imported )";
                if (newlyImportedPhotos != importedPhotos)
                    message1 += " ( merging : " + newlyImportedPhotos + " new photos";
                string message2 = unknownFolder.Count.ToString() + " names not found ( " + missedPhotos.ToString() + " photos left behind )";
                string message = message0 + "\n" + message1 + "\n" + message2 + "\n\n" + "for more details, look at GenerateArkiveCollection.log file";
                if (unknownFolder.Count > 0)
                {
                    message += "\nA list of taxons is generated with all name not found : " + Path.Combine( TaxonList.GetFolder(),"ArkiveNames.txt");
                }
                Loggers.WriteInformation(LogTags.Data, message);

                try
                {
                    List<KeyValuePair<string, string>> unknowns = unknownFolder.ToList();
                    unknowns.Sort((x, y) => x.Value.CompareTo(y.Value));

                    string file = Path.Combine(TaxonUtils.GetLogPath(), "GenerateArkiveCollection.log");
                    if (File.Exists(file)) File.Delete(file);
                    using (StreamWriter outfile = new StreamWriter(file))
                    {
                        outfile.WriteLine("GenerateArkiveCollection results:");
                        outfile.WriteLine();
                        outfile.WriteLine("  " + message0);
                        outfile.WriteLine("  " + message1);
                        outfile.WriteLine("  " + message2);
                        outfile.WriteLine();
                        if (unknowns.Count > 0)
                        {
                            outfile.WriteLine("List of not found names:");
                            outfile.WriteLine();

                            int maxLength = 0;
                            foreach (KeyValuePair<string, string> pair in unknowns)
                                maxLength = Math.Max(maxLength, pair.Value.Length);
                            foreach (KeyValuePair<string, string> pair in unknowns)
                                outfile.WriteLine("  " + pair.Value.PadRight(maxLength) + " => " + pair.Key);
                            outfile.WriteLine();
                        }
                    }

                    if (unknowns.Count > 0)
                    {
                        file = Path.Combine(TaxonList.GetFolder(), "ArkiveNames.txt");
                        if (File.Exists(file)) File.Delete(file);
                        using (StreamWriter outfile = new StreamWriter(file))
                        {
                            foreach (KeyValuePair<string, string> pair in unknowns)
                                outfile.WriteLine(pair.Value);
                        }
                    }
                }
                catch (Exception e)
                {
                    string error = "Exception while saving results in GenerateArkiveCollection.log: \n\n";
                    error += e.Message;
                    if (e.InnerException != null) error += "\n" + e.InnerException.Message;
                    Loggers.WriteError(LogTags.Data, error);
                }
            }
        }
    }
}


