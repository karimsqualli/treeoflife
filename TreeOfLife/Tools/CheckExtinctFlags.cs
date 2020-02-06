using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace TreeOfLife.Tools
{
    [Description("List taxons with Extinct or Extinct inherited flag")]
    [DisplayName("List extincts")]
    [Category("Tools/Taxons")]
    [PresentInMode(false, true)]
    class CheckExtinctFlags : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            List<TaxonTreeNode> extincts = new List<TaxonTreeNode>();
            List<TaxonTreeNode> extinctsInherited = new List<TaxonTreeNode>();

            string logFile = Path.Combine(TaxonUtils.GetLogPath(), "ExtinctsTaxons.log");
            using (StreamWriter log = new StreamWriter(logFile))
            {
                log.WriteLine("Move extinct flag to red list category EX (" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ")\n");

                TaxonUtils.OriginalRoot.ParseNode((d) =>
                {
                    if (d.Desc.HasFlag(FlagsEnum.Extinct)) extincts.Add(d);
                    if (d.Desc.HasFlag(FlagsEnum.ExtinctInherited)) extinctsInherited.Add(d);
                });

                log.WriteLine("taxon with extincts flag: " + extincts.Count.ToString());
                foreach (TaxonTreeNode node in extincts)
                    log.WriteLine("    [" + node.Desc.RedListCategory.ToString() + "] " + node.GetHierarchicalName());
                log.WriteLine();

                log.WriteLine("taxon with extincts inherited flag: " + extincts.Count.ToString());
                foreach (TaxonTreeNode node in extinctsInherited)
                    log.WriteLine("    [" + node.Desc.RedListCategory.ToString() + "] " + node.GetHierarchicalName());
                log.WriteLine();


                string message = "Move extinct flag to red list category EX results:\n";
                message += string.Format("{0} taxons with extinct flag\n", extincts.Count.ToString());
                message += string.Format("{0} taxons with extinct inherited flag\n", extinctsInherited.Count.ToString());
                message += string.Format("for more details, look at {0}", logFile);
                Loggers.WriteInformation(LogTags.Data, message);
                log.Write(message);
            }
        }
    }
}
