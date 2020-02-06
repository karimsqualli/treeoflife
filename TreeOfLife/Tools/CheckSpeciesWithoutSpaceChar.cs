using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("List all taxon with Species classik rank and with only one word")]
    [DisplayName("List one word species")]
    [Category("Tools/Taxons")]
    class CheckSpeciesWithoutSpaceChar : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            // retrouve la liste de toutes les especes
            List<TaxonTreeNode> Species = new List<TaxonTreeNode>();
            TaxonUtils.OriginalRoot.GetAllChildrenRecursively(Species, ClassicRankEnum.Espece);

            List<TaxonTreeNode> badTaxons = new List<TaxonTreeNode>();
            foreach (TaxonTreeNode t in Species)
            {
                if (t.Desc.RefMultiName.Main.Contains(" ")) continue;
                badTaxons.Add(t);
            }

            string message = "Check species named with only one word : ";
            if (badTaxons.Count == 0)
            {
                message += "none found\n";
                Loggers.WriteInformation(LogTags.Data, message);
                return;
            }

            message += String.Format("found {0} species\n\n", badTaxons.Count);
            message += String.Format("for more details, look at CheckSpeciesWithoutSpaceChar.log file");
            Loggers.WriteInformation(LogTags.Data, message);

            string file = Path.Combine(TaxonUtils.GetLogPath(), "CheckSpeciesWithoutSpaceChar.log");
            if (File.Exists(file)) File.Delete(file);

            using (StreamWriter outfile = new StreamWriter(file))
            {
                outfile.WriteLine("CheckSpeciesWithoutSpaceChar result ( " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " )\n");
                outfile.WriteLine("Find " + badTaxons.Count.ToString() + " species named with only one word\n");

                foreach (TaxonTreeNode taxon in badTaxons)
                {
                    outfile.WriteLine("    " + taxon.GetHierarchicalName());
                }
            }
        }
    }
}
