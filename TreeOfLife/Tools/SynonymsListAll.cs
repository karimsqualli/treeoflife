using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using TreeOfLife.Tools;

namespace TreeOfLife.Tools
{
    [Description("List all taxon with synonyms and their synonyms")]
    [DisplayName("List all taxon with synonyms")]
    [Category("Tools/Synonyms")]
    [PresentInMode(false, true)]
    class SynonymListAll : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            List<TaxonTreeNode> ListTaxonsLatin = new List<TaxonTreeNode>();
            TaxonUtils.OriginalRoot.ParseNode((d) => { if (d.Desc.RefMultiName.HasSynonym) ListTaxonsLatin.Add(d); });

            List<TaxonTreeNode> ListTaxonsFrench = new List<TaxonTreeNode>();
            TaxonUtils.OriginalRoot.ParseNode((d) => { if (d.Desc.HasFrenchName && d.Desc.FrenchMultiName.HasSynonym) ListTaxonsFrench.Add(d); });

            string message = "List all taxon with synonyms:\n";
            message += String.Format("    Taxons with latin synonyms : {0}\n", ListTaxonsLatin.Count);
            message += String.Format("    Taxons with french synonyms : {0}\n", ListTaxonsFrench.Count);
            message += String.Format("for more details, look at SynonymListAll_*.log file");
            Loggers.WriteInformation(LogTags.Image, message);

            List<Tuple<string, List<TaxonTreeNode>>> outputs = new List<Tuple<string, List<TaxonTreeNode>>>()
            {
                new Tuple<string, List<TaxonTreeNode>>("latin", ListTaxonsLatin),
                new Tuple<string, List<TaxonTreeNode>>("french", ListTaxonsFrench)
            };

            try
            {
                foreach (Tuple<string, List<TaxonTreeNode>> tuple in outputs)
                {
                    string logFilename = Path.Combine(TaxonUtils.GetLogPath(), "SynonymListAll_" + tuple.Item1 + ".log");
                    if (File.Exists(logFilename)) File.Delete(logFilename);
                    using (StreamWriter outfile = new StreamWriter(logFilename))
                    {
                        outfile.WriteLine("SynonymsListAll " + tuple.Item1 + " results ( " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " )\n");
                        outfile.WriteLine(string.Format("Taxon found: {0}", tuple.Item2.Count));

                        foreach (TaxonTreeNode taxon in tuple.Item2)
                        {
                            outfile.WriteLine("    " + taxon.GetHierarchicalName());

                            Helpers.MultiName multiName = (tuple.Item1 == "french") ? taxon.Desc.FrenchMultiName : taxon.Desc.RefMultiName;
                            string[] otherNames = multiName.GetSynonymsArray();
                            if (otherNames == null || otherNames.Length == 0)
                            {
                                outfile.WriteLine("    " + taxon.Desc.RefMainName + " has no synonyms (should not happen here!!)");
                                continue;
                            }

                            outfile.WriteLine("    " + taxon.Desc.RefMainName + " has " + otherNames.Length.ToString() + " synonym" + (otherNames.Length == 1 ? "" : "s"));
                            foreach (string synonym in otherNames)
                                outfile.WriteLine("        " + synonym);
                        }
                    }
                    System.Diagnostics.Process.Start(logFilename);
                }
            }
            catch (Exception e)
            {
                string error = "Exception while saving results in SynonymsListAll.log: \n\n";
                error += e.Message;
                if (e.InnerException != null)
                    error += "\n" + e.InnerException.Message;
                Loggers.WriteError(LogTags.Data, error);
            }
        }
    }
}
