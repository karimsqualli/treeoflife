using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Check latin name")]
    [DisplayName("Check names")]
    [Category("Tools/Names")]
    class CheckNames : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            int count1 = 0;
            int count2 = 0;

            char[] forbiddenChar = "<>:\"/\\|?*".ToCharArray();
            
            string logFile = Path.Combine(TaxonUtils.GetLogPath(), "CheckNames.log");
            using (StreamWriter log = new StreamWriter(logFile))
            {
                TaxonUtils.OriginalRoot.ParseNode((d) =>
                {
                    string name = d.Desc.RefMultiName.Main;
                    if (name.Trim() != name)
                    {
                        count1++;
                        log.WriteLine("empty spaces in " + d.GetHierarchicalName());
                    }
                    if (name.IndexOfAny(forbiddenChar) != -1)
                    {
                        count2++;
                        log.WriteLine("forbidden char in " + d.GetHierarchicalName());
                    }
                });

                string message = "Name check:\n";
                message += string.Format("{0} with spaces (or tab) at start or end of name\n", count1);
                message += string.Format("{0} with forbidden characters\n", count2);
                message += "more informations in " + logFile + " file.\n";
                Loggers.WriteInformation(LogTags.Data, message);
                log.Write(message);
            }
        }
    }


}
