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
    [Description("Parse image directory to get unused images")]
    [DisplayName("List unused images")]
    [Category("Tools/Images")]
    class CheckUnusedImages : ITool
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

                ProgressItem piPrepare = progressDlg.Add("Prepare ...", "get species", 0, 3);
                // retrouve la liste de toutes les especes / sous especes
                List<TaxonTreeNode> Species = new List<TaxonTreeNode>();
                TaxonUtils.OriginalRoot.GetAllChildrenRecursively(Species, ClassicRankEnum.Espece);
                piPrepare.Update(1, "get sub species");
                List<TaxonTreeNode> SubSpecies = new List<TaxonTreeNode>();
                TaxonUtils.OriginalRoot.GetAllChildrenRecursively(SubSpecies, ClassicRankEnum.SousEspece);
                piPrepare.Update(2, "all images name (can be long)");
                // et la liste des fichiers dans le répertoires d'images
                string PathImages = TaxonImages.Manager.Path;
                int TotalImages = 0;
                Dictionary<string, bool> FileUsed = new Dictionary<string, bool>();
                Directory.GetFiles(PathImages, "*.jpg", SearchOption.AllDirectories).ToList().ForEach(f => { FileUsed[f.ToLower()] = false; TotalImages++; });
                piPrepare.Update(3, "init done");

                ProgressItem piSpecies = progressDlg.Add("Prepare ...", "get species", 0, Species.Count);
                List<TaxonTreeNode> SpeciesWithImage = new List<TaxonTreeNode>();
                List<TaxonTreeNode> SpeciesWithBadImage = new List<TaxonTreeNode>();
                List<TaxonTreeNode> SpeciesWithoutImage = new List<TaxonTreeNode>();
                int count = 0;
                foreach (TaxonTreeNode node in Species)
                {
                    piSpecies.Update(++count);
                    if (node.Desc.Images == null)
                    {
                        SpeciesWithoutImage.Add(node);
                        continue;
                    }

                    bool badImage = false;
                    bool goodImage = false;
                    foreach (TaxonImageDesc imageDesc in node.Desc.Images)
                    {
                        if (imageDesc.IsALink) continue;
                        string path = imageDesc.GetPath(node.Desc).ToLower();
                        bool used;
                        if (!FileUsed.TryGetValue(path, out used))
                            badImage = true;
                        else
                        {
                            goodImage = true;
                            if (used)
                                Console.WriteLine("image " + path + " used several time");
                            FileUsed[path] = true;
                        }
                    }
                    if (badImage) SpeciesWithBadImage.Add(node);
                    if (goodImage) SpeciesWithImage.Add(node);
                }

                ProgressItem piSubSpecies = progressDlg.Add("Prepare ...", "get species", 0, Species.Count);
                List<TaxonTreeNode> SubSpeciesWithImage = new List<TaxonTreeNode>();
                List<TaxonTreeNode> SubSpeciesWithBadImage = new List<TaxonTreeNode>();
                List<TaxonTreeNode> SubSpeciesWithoutImage = new List<TaxonTreeNode>();
                count = 0;
                foreach (TaxonTreeNode node in SubSpecies)
                {
                    piSubSpecies.Update(++count);
                    if (node.Desc.Images == null)
                    {
                        SubSpeciesWithoutImage.Add(node);
                        continue;
                    }

                    bool badImage = false;
                    bool goodImage = false;
                    foreach (TaxonImageDesc imageDesc in node.Desc.Images)
                    {
                        if (imageDesc.IsALink) continue;
                        string path = imageDesc.GetPath(node.Desc).ToLower();
                        bool used;
                        if (!FileUsed.TryGetValue(path, out used))
                            badImage = true;
                        else
                        {
                            goodImage = true;
                            if (used)
                                Console.WriteLine("image " + path + " used several time");
                            FileUsed[path] = true;
                        }
                    }
                    if (badImage) SubSpeciesWithBadImage.Add(node);
                    if (goodImage) SubSpeciesWithImage.Add(node);
                }

                List<string> unusedfiles = FileUsed.Where(p => !p.Value).Select(p => p.Key).ToList();

                string message = "Check unused image summary : \n\n";
                message += String.Format("    Total species     : {0}, {1} with images, {2} with bad images, {3} without\n", Species.Count, SpeciesWithImage.Count, SpeciesWithBadImage.Count, SpeciesWithoutImage.Count);
                message += String.Format("    Total sub/species : {0}, {1} with images, {2} with bad images, {3} without\n\n", SubSpecies.Count, SubSpeciesWithImage.Count, SubSpeciesWithBadImage.Count, SubSpeciesWithoutImage.Count);
                message += String.Format("    Total images      : {0}, {1} are unused\n\n", TotalImages, unusedfiles.Count);
                message += String.Format("for more details, look at CheckUnusedImages.log file");
                Loggers.WriteInformation(LogTags.Image, message);

                try
                {
                    string file = Path.Combine(TaxonUtils.GetLogPath(), "CheckUnusedImages.log");
                    if (File.Exists(file)) File.Delete(file);
                    using (StreamWriter outfile = new StreamWriter(file))
                    {
                        outfile.WriteLine("CheckUnusedImages result ( " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " )\n");
                        outfile.WriteLine(string.Format("Unused images ( {0} ) :", unusedfiles.Count));
                        unusedfiles.ForEach(f => outfile.WriteLine(string.Format("    " + f)));
                        outfile.WriteLine("\n");

                        outfile.WriteLine(string.Format("Species without bad images ( {0} ) :", SpeciesWithBadImage.Count));
                        foreach (TaxonTreeNode node in SpeciesWithBadImage)
                            outfile.WriteLine(string.Format("    " + node.Desc.RefMultiName.Main));
                        outfile.WriteLine("\n");

                        outfile.WriteLine(string.Format("Sub/Species with bad images ( {0} ) :", SubSpeciesWithBadImage.Count));
                        foreach (TaxonTreeNode node in SubSpeciesWithBadImage)
                            outfile.WriteLine(string.Format("    " + node.Desc.RefMultiName.Main));

                        outfile.WriteLine("\n");
                    }
                }
                catch (Exception e)
                {
                    string error = "Exception while saving results in CheckUnusedImages.log: \n\n";
                    error += e.Message;
                    if (e.InnerException != null)
                        error += "\n" + e.InnerException.Message;
                    Loggers.WriteError(LogTags.Image, error);
                }
            }
        }
    }
}
