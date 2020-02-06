using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Parse all taxon to list taxon with image but without french name")]
    [DisplayName("List images without French name")]
    [Category("Tools/Images")]
    class CheckImagesWithoutFrenchNames : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            List<TaxonTreeNode> ListTaxons = new List<TaxonTreeNode>();
            TaxonUtils.OriginalRoot.GetAllChildrenWithImageRecursively(ListTaxons);

            List<TaxonTreeNode> ListTaxonWithoutFrench = new List<TaxonTreeNode>();

            foreach (TaxonTreeNode taxon in ListTaxons)
            {
                if (taxon.Desc.HasFrenchName)
                    ListTaxonWithoutFrench.Add(taxon);
            }

            string message = "Check taxon with image but without French name:\n";
            message += String.Format("    Total found: {0}\n", ListTaxonWithoutFrench.Count );
            message += String.Format("for more details, look at CheckImagesWithoutFrenchNames.log file");
            Loggers.WriteInformation(LogTags.Image, message);

            try
            {
                string file = Path.Combine( TaxonUtils.GetLogPath(), "CheckImagesWithoutFrenchNames.log");
                if (File.Exists(file)) File.Delete(file);
                using (StreamWriter outfile = new StreamWriter(file))
                {
                    outfile.WriteLine("CheckImagesWithoutFrenchNames result ( " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " )\n");
                    outfile.WriteLine(string.Format("Taxon found: {0}", ListTaxonWithoutFrench.Count));

                    foreach (TaxonTreeNode taxon in ListTaxonWithoutFrench)
                        outfile.WriteLine("    " + taxon.GetHierarchicalName());
                }
            }
            catch (Exception e)
            {
                string error = "Exception while saving results in CheckImagesWithoutFrenchNames.log: \n\n";
                error += e.Message;
                if (e.InnerException != null)
                    error += "\n" + e.InnerException.Message;
                Loggers.WriteError(LogTags.Data, error);
            }
        }
    }
}
