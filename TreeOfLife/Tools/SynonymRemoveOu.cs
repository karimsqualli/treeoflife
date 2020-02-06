using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace TreeOfLife.Tools
{
    [Description("Replace ' ou ' keyword to separate synonyms with ';' ")]
    [DisplayName("Remove ' ou ' keyword")]
    [Category("Tools/Synonyms")]
    [PresentInMode(false, false)]
    class SynonymRemoveOu : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            List<TaxonDesc> taxonToModify = new List<TaxonDesc>();
            TaxonUtils.OriginalRoot.ParseNodeDesc((d) => { if (d.HasFrenchName && d.FrenchMultiName.Full.Contains(" ou ")) taxonToModify.Add(d); });

            string logFile = Path.Combine(TaxonUtils.GetLogPath(), "SynonymRemoveOu.log");
            using (StreamWriter log = new StreamWriter(logFile))
            {
                log.WriteLine("Replace synonym separator ' ou ' with ';' (" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ")\n");
                log.WriteLine("  found " + taxonToModify.Count + " taxon that need to be changed\n\n");

                string[] separatorArray = new string[] { " ou " };
                foreach (TaxonDesc node in taxonToModify)
                {
                    string oldName = node.FrenchMultiName.Full;
                    node.FrenchMultiName = new Helpers.MultiName(string.Join(Helpers.MultiName.SeparatorAsString, node.FrenchMultiName.Full.Split(separatorArray, StringSplitOptions.RemoveEmptyEntries)));
                    log.WriteLine("(" + node.RefMainName + ")" + oldName + " => " + node.FrenchMultiName.Full);
                }
            }

            string message = "Replacing synonym separator ' ou ' with ';' \n";
            message += taxonToModify.Count + " taxons impacted\n";
            message += "for more details, look at SynonymRemoveOu.log files";
            Loggers.WriteInformation(LogTags.Data, message);
        }
    }
}
