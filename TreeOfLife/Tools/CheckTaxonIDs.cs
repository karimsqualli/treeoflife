using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace TreeOfLife.Tools
{
    //todo : check that tool

    [Description("Check taxon OTT Ids ...")]
    [DisplayName("Check Ids")]
    [Category("Tools/Taxons")]
    class CheckTaxonIDs : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            TaxonIds Data = TaxonIds.Compute(TaxonUtils.OriginalRoot);

            string message = "Check Ids:\n";
            message += String.Format("    Total Ids used: {0} (min = {1}, max = {2})\n", Data.Ids.Count, Data.MinId, Data.MaxId );
            if (Data.TolIds.Count > 0)
                message += String.Format("    Total TOL Ids used: {0} (min = {1}, max = {2})\n", Data.TolIds.Count, Data.MinTolId, Data.MaxTolId );
            message += String.Format("    Number of ranges: {0}\n", Data.Ranges.Count);
            message += String.Format("    Total Duplicate: {0}\n", Data.Duplicates.Count);
            message += String.Format("    Total without Id: {0}\n", Data.NoIds.Count);
            message += String.Format("for more details, look at CheckTaxonIDs.log file");
            Loggers.WriteInformation(LogTags.Data, message);

            try
            {
                string file = Path.Combine(TaxonUtils.GetLogPath(), "CheckTaxonIDs.log");
                if (File.Exists(file)) File.Delete(file);
                using (StreamWriter outfile = new StreamWriter(file))
                {
                    outfile.WriteLine("CheckTaxonIDs results:\n");

                    outfile.WriteLine(string.Format("Ranges ( {0} ) :", Data.Ranges.Count));
                    foreach (Tuple<uint, uint> tuple in Data.Ranges)
                        outfile.WriteLine(string.Format("    from {0} to {1} ({2})", tuple.Item1, tuple.Item2, tuple.Item2 - tuple.Item1 + 1));
                    outfile.WriteLine("");
                    
                    outfile.WriteLine(string.Format("Duplicates ( {0} ) :", Data.Duplicates.Count));
                    foreach (Tuple<TaxonDesc, TaxonDesc> tuple in Data.Duplicates)
                        outfile.WriteLine(string.Format("    {0}({1}), {2}({3})", tuple.Item1.RefMultiName.Main, tuple.Item1.OTTID, tuple.Item2.RefMultiName.Main, tuple.Item2.OTTID));
                    outfile.WriteLine("");

                    outfile.WriteLine(string.Format("Without id ( {0} ) :", Data.NoIds.Count));
                    foreach (TaxonDesc node in Data.NoIds)
                        outfile.WriteLine("    " + node.RefMultiName.Main);
                    outfile.WriteLine("");

                    outfile.WriteLine(string.Format("Tols id ( {0} ) :", Data.TolIds.Count));
                    foreach (KeyValuePair<UInt32, TaxonDesc> pair in Data.TolIds)
                        outfile.WriteLine("    " + pair.Key + " ( FirstTolID + " + (pair.Key - TaxonIds.FirstTolID) + " ) : " + pair.Value.RefMultiName.Main);
                    outfile.WriteLine("");
                }
            }
            catch (Exception e)
            {
                string error = "Exception while saving results in CheckTaxonIDs.log: \n\n";
                error += e.Message;
                if (e.InnerException != null)
                    error += "\n" + e.InnerException.Message;
                Loggers.WriteError(LogTags.Data, error);
            }
        }
    }
}
