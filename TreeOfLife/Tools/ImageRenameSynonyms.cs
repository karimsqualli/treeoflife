using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeOfLife.TaxonDialog;

namespace TreeOfLife.Tools
{
    [Description("Try to associate unused images with a synonym, if possible rename it to be associated with main taxon")]
    [DisplayName("Rename image used by synonyms")]
    [Category("Tools/Images")]
    class ImageRenameSynonyms : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            using (ProgressDialog progressDlg = new ProgressDialog())
            {
                progressDlg.StartPosition = FormStartPosition.CenterScreen;
                progressDlg.Show();

                ProgressItem piPrepare = progressDlg.Add("Prepare ...", "get taxon with images", 0, 2);
                // retrouve la liste de toutes les especes / sous especes
                List<TaxonDesc> Nodes = new List<TaxonDesc>();
                TaxonUtils.OriginalRoot.ParseNodeDesc((d) => { if (d.HasImage) Nodes.Add(d); });
                piPrepare.Update(1, "all images name (can be long)");
                // et la liste des fichiers dans le répertoires d'images
                string PathImages = TaxonImages.Manager.Path;
                int TotalImages = 0;
                Dictionary<string, bool> FileUsed = new Dictionary<string, bool>();
                Directory.GetFiles(PathImages, "*.jpg", SearchOption.AllDirectories).ToList().ForEach(f => { FileUsed[f.ToLower()] = false; TotalImages++; });
                piPrepare.Update(2, "init done");

                ProgressItem piTaxonDesc = progressDlg.Add("Parse Taxon with images ...", "", 0, Nodes.Count);
                int count = 0;
                foreach (TaxonDesc desc in Nodes)
                {
                    piTaxonDesc.Update(++count);
                    foreach (TaxonImageDesc imageDesc in desc.Images)
                    {
                        if (imageDesc.IsALink) continue;
                        string path = imageDesc.GetPath(desc).ToLower();
                        if (FileUsed.ContainsKey(path))
                            FileUsed[path] = true;
                    }
                }

                List<string> unusedfiles = FileUsed.Where(p => !p.Value).Select(p => p.Key).ToList();
                ProgressItem piFinder = progressDlg.Add("Init finder...", "", 0, 1);
                TaxonSearch finder = new TaxonSearch(TaxonUtils.OriginalRoot, true, true);
                piFinder.Update(1);

                int noTaxonFound = 0;
                int severalTaxonFound = 0;
                int renamedFiles = 0;
                int sameName = 0;
                int errorWhileRenaming = 0;

                string logfile = Path.Combine(TaxonUtils.GetLogPath(), "ImageRenameSynonyms.log");
                if (File.Exists(logfile)) File.Delete(logfile);
                using (StreamWriter log = new StreamWriter(logfile))
                { 
                    ProgressItem piUnused = progressDlg.Add("Treat unused images ...", "", 0, unusedfiles.Count);
                    foreach (string filename in unusedfiles)
                    {
                        piUnused.Inc();
                        TaxonImages.SplitImageFilenameResult splitResult = TaxonImages.Manager.SplitImageFilename(filename);
                        if (splitResult == null) continue;

                        List<TaxonTreeNode> nodes = finder.FindAll(splitResult.TaxonName);
                        string textline = "[" + (nodes == null ? 0 : nodes.Count) + "] " + splitResult.TaxonName + ": ";
                        if (nodes == null || nodes.Count == 0)
                        {
                            noTaxonFound++;
                            textline += "no taxon found";
                        }
                        else if (nodes.Count > 1)
                        {
                            severalTaxonFound++;
                            textline += "too many taxons found";
                        }
                        else if (filename.ToLower() == splitResult.Desc.GetPath(nodes[0].Desc).ToLower())
                        {
                            sameName++;
                            textline += "same name, probably unused due to rank";
                        }
                        else
                        {
                            TaxonTreeNode node = nodes[0];
                            TaxonImageDesc desc = splitResult.Desc;
                            desc.FillForNewFile(node.Desc);
                            string newFilename = desc.GetPath(node.Desc);
                            textline += " rename " + System.IO.Path.GetFileName(filename) + " in " + System.IO.Path.GetFileName(newFilename);
                            try
                            {
                                System.IO.File.Move(filename, newFilename);
                                renamedFiles++;
                            }
                            catch (System.Exception e)
                            {
                                textline += "[error] " + e.Message;
                                errorWhileRenaming++;
                            }
                        }
                        log.WriteLine(textline);
                    }
                }

                string message = "Rename unused file using synonyms name : \n\n";
                message += String.Format("    Total treated files: {0}\n", unusedfiles.Count );
                message += String.Format("    No match: {0}\n", noTaxonFound);
                message += String.Format("    Several matches: {0}\n", severalTaxonFound);
                message += String.Format("    New name is same as unused: {0}\n", sameName);
                message += String.Format("    Renamed files: {0}\n", renamedFiles);
                message += String.Format("    Error renaming: {0}\n", errorWhileRenaming);
                message += String.Format("Same renaming occurs, do not forget to update images\n");
                message += String.Format("for more details, look at ImageRenameSynonyms.log file");
                Loggers.WriteInformation(LogTags.Image, message);
            }
        }
    }
}
