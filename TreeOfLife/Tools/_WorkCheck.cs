using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace TreeOfLife.Tools
{
    [Description("Temporary check, should not be here")]
    [DisplayName("extinct check")]
    [Category("Tools")]
    [PresentInMode(false, false)]
    class _WorkCheck : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            string logFile = Path.Combine(TaxonUtils.GetLogPath(), "_TempCheck.log");
            using (StreamWriter log = new StreamWriter(logFile))
            {
                TaxonUtils.OriginalRoot.ParseNode((d) =>
                {
                    if ( d.Desc.RefMultiName.Full.IndexOf( '†' ) != -1 || (d.Desc.FrenchMultiName != null && d.Desc.FrenchMultiName.Full.Contains('†')))
                    {
                        count1++;
                        log.WriteLine("Cross in name for " + d.GetHierarchicalName());
                    }

                    if (d.Desc.RedListCategory == RedListCategoryEnum.Extinct)
                    {
                        if (d.Desc.HasFlag(FlagsEnum.Extinct) || d.Desc.HasFlag(FlagsEnum.ExtinctInherited))
                        {
                            d.Desc.SetFlagValue(FlagsEnum.Extinct, false);
                            d.Desc.SetFlagValue(FlagsEnum.ExtinctInherited, false);
                            count4++;
                        }
                    }
                    else
                    { 
                        if (d.Desc.HasFlag(FlagsEnum.ExtinctInherited))
                        {
                            log.WriteLine("Extinct inherited: " + d.GetHierarchicalName());
                            count2++;
                        }
                        if (d.Desc.HasFlag(FlagsEnum.Extinct))
                        {
                            log.WriteLine("Extinct: " + d.GetHierarchicalName());
                            count3++;
                        }
                    }
                });

                string message = "Extinct check:\n";
                message += string.Format("{0} with cross within name\n", count1);
                message += string.Format("{0} with extinct inherited flag\n", count2);
                message += string.Format("{0} with extinct flag\n", count3);
                if (count4 > 0)
                {
                    message += string.Format("{0} remove extinct or extinct inherited flag\n", count4);
                    message += string.Format("some data has been changed do not forget to save\n");
                }
                message += "more informations in " + logFile + " file.\n";
                Loggers.WriteInformation(LogTags.Data, message);
                log.Write(message);
            }
        }
    }
}
