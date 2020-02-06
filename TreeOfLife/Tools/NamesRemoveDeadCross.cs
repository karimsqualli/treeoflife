using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Remove Extinct cross from file name and put it in taxon flags")]
    [DisplayName("Remove Extinct cross")]
    [Category("Tools/Names")]
    [PresentInMode(false, false)]
    class RemoveExtinctCross : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            int countDescChanged = 0;
            int countExtinct = 0;
            string logFile = Path.Combine(TaxonUtils.GetLogPath(), "RemoveExtinctCross.log");
            using (StreamWriter log = new StreamWriter(logFile))
            {
                log.WriteLine("Remove '†' (" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ")\n");

                TaxonUtils.OriginalRoot.ParseNodeDesc((d) =>
                {
                    if (d.RefMultiName.Main.Length == 0) return;
                    if (d.RefMultiName.Main[d.RefMultiName.Main.Length - 1] == '†')
                    {
                        log.WriteLine("  " + d.RefMultiName.Main);
                        List<string> names = d.RefMultiName.GetAll().ToList();
                        for (int i = 0; i < names.Count; i++)
                            names[i] = names[i].Replace("†", "").Trim();
                        d.RefMultiName = new Helpers.MultiName(names);
                        d.Flags |= (uint)FlagsEnum.Extinct;
                        countDescChanged++;
                        countExtinct++;
                    }
                    else if ((d.Flags & (uint)FlagsEnum.Extinct) != 0)
                        countExtinct++;
                });

                string message = "Remove Extinct Cross:\n";
                message += String.Format("{0} extinct taxon renamed\n", countDescChanged);
                message += String.Format("{0} extinct taxons (with flags)\n", countExtinct);
                Loggers.WriteInformation(LogTags.Data, message);
                log.Write(message);
            }
        }
    }


}
