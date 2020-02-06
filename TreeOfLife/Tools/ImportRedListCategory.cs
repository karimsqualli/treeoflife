using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    

    [Description("Import red list category from a file, each line begins with taxon latin name followed by information in JacFile format")]
    [DisplayName("Import Red List Category")]
    [Category("Tools/Taxons")]
    [PresentInMode(false, true)]
    class ImportRedListCategory : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Jac Files (*.txt)|*.txt",
                Multiselect = false,
                AddExtension = true
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (!File.Exists(ofd.FileName)) return;

            TaxonTreeNode importNodes = TaxonTreeNode.Load(ofd.FileName);

            string shortName = Path.GetFileNameWithoutExtension(ofd.FileName);

            int errorNoTaxon = 0;
            int errorTwoManyTaxon = 0;
            int redListCategoryChanged = 0;

            string logFilenameErrors = Path.Combine(TaxonUtils.GetLogPath(), "ImportRedListCategory_" + shortName + ".log");
            using (StreamWriter log = new StreamWriter(logFilenameErrors))
            {
                if (importNodes == null || importNodes.Children == null || importNodes.Children.Count == 0)
                {
                    log.WriteLine("no taxon found in file " + ofd.FileName);
                    Loggers.WriteInformation(LogTags.Data, "no taxon found in file " + ofd.FileName);
                    return;
                }

                TaxonSearch searchTool = new TaxonSearch(TaxonUtils.OriginalRoot, true, true);

                foreach (TaxonTreeNode node in importNodes.Children)
                {
                    List<TaxonTreeNode> dests = searchTool.FindAll(node.Desc.RefMainName);
                    if (dests == null || dests.Count == 0)
                    {
                        errorNoTaxon++;
                        log.WriteLine("Error, no matching taxons found for that name: " + node.Desc.RefMainName);
                    }
                    else if (dests.Count > 1)
                    {
                        errorTwoManyTaxon++;
                        log.WriteLine("Error, two many taxons found for that name: " + node.Desc.RefMainName);
                    }
                    else
                    {
                        if (dests[0].Desc.RedListCategory == node.Desc.RedListCategory)
                            log.WriteLine(node.Desc.RefMainName + " no change, is already " + node.Desc.RedListCategory.ToString());
                        else
                        {
                            redListCategoryChanged++;
                            log.WriteLine(node.Desc.RefMainName + " redlist category change from " + dests[0].Desc.RedListCategory.ToString() + " to " + node.Desc.RedListCategory.ToString());
                            dests[0].Desc.RedListCategory = node.Desc.RedListCategory;
                        }
                    }
                }
            }

                
            string message = "Importing red list category from: " + ofd.FileName + "\n";
            message += String.Format("    Total taxons: {0}\n", importNodes.Children.Count);
            message += String.Format("    taxon not found: {0}\n", errorNoTaxon);
            message += String.Format("    taxon find more than one time: {0}\n", errorTwoManyTaxon);
            message += String.Format("    changed taxons: {0}\n", redListCategoryChanged);
            if (redListCategoryChanged > 0)
                message += string.Format("some data has been changed do not forget to save\n");
            message += String.Format("for more details, look at ImportRedListCategory_{0}.log files", shortName);
            Loggers.WriteInformation(LogTags.Data, message);
        }
    }
}
