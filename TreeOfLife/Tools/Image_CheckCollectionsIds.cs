using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Check Collections Ids")]
    [DisplayName("Check Collections Ids")]
    [Category("Tools/Images")]
    class CheckCollectionsIds: ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        class Data
        {
            public Data()
            {
                foreach (ImageCollection col in TaxonImages.Manager.CollectionsEnumerable())
                    Exists[col.Id] = 0;
            }

            public int TotalExistsImage() { return Exists.Values.Sum(); }
            public int TotalDontExistsImage() { return DontExists.Values.Sum(); }

            public Dictionary<int, int> Exists = new Dictionary<int, int>();
            public Dictionary<int, int> DontExists = new Dictionary<int, int>();
        }

        private void CheckCollectionIds(TaxonTreeNode _node, object _data)
        {
            if (_node.Desc.Images == null) return;
            Data data = _data as Data;
            foreach (TaxonImageDesc imageDesc in _node.Desc.Images)
            {
                int count = 0;
                if (TaxonImages.Manager.Collection(imageDesc.CollectionId) != null)
                {
                    if (!data.Exists.TryGetValue(imageDesc.CollectionId, out count))
                        count = 0;
                    data.Exists[imageDesc.CollectionId] = count + 1;
                }
                else
                {
                    if (!data.DontExists.TryGetValue(imageDesc.CollectionId, out count))
                        count = 0;
                    data.DontExists[imageDesc.CollectionId] = count + 1;
                }
            }
        }

        public void Activate()
        {
            Data data = new Data();
            TaxonUtils.OriginalRoot.ParseNode(CheckCollectionIds, data);

            string message = "CheckCollectionsIds results: \n\n";
            message += String.Format("    Existents collections  : {0} ids, {1} total images\n", data.Exists.Count, data.TotalExistsImage() );
            message += String.Format("    Inexistents collections: {0} ids, {1} total images\n", data.DontExists.Count, data.TotalDontExistsImage());
            message += String.Format("for more details, look at CheckCollectionsIds.log file");
            Loggers.WriteInformation(LogTags.Image, message);

            try
            {
                string file = Path.Combine(TaxonUtils.GetLogPath(), "CheckCollectionsIds.log");
                if (File.Exists(file)) File.Delete(file);
                using (StreamWriter outfile = new StreamWriter(file))
                {
                    outfile.WriteLine("CheckCollectionsIds result ( " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " )\n");
                    outfile.WriteLine("\n");
                    outfile.WriteLine(string.Format("Existent collections ( {0} ) :", data.Exists.Count));
                    foreach (KeyValuePair<int, int> p in data.Exists)
                    {
                        ImageCollection ic = TaxonImages.Manager.Collection(p.Key);
                        string desc = "    " + p.Key + " (" + p.Value + " images)";
                        if (ic == null)
                            outfile.WriteLine(desc + " not found in Manager");
                        else
                            outfile.WriteLine(desc + " = " + ic.Name);
                    }
                    outfile.WriteLine("\n");
                    outfile.WriteLine(string.Format("Inexistent collections ( {0} ) :", data.DontExists.Count));
                    foreach (KeyValuePair<int, int> p in data.DontExists)
                    {
                        string desc = "    " + p.Key + " (" + p.Value + " images)";
                        outfile.WriteLine(desc);
                    }
                }
            }
            catch (Exception e)
            {
                string error = "Exception while saving results in CheckCollectionsIds.log: \n\n";
                error += e.Message;
                if (e.InnerException != null)
                    error += "\n" + e.InnerException.Message;
                Loggers.WriteError(LogTags.Image, error);
            }
        }
    }
}
