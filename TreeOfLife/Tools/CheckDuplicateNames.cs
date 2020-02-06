using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Parse all taxon to look for duplicates name")]
    [DisplayName("Check duplicate names")]
    [Category("Tools/Taxons")]
    class CheckDuplicateNames : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {

            DialogResult result = MessageBox.Show("Generate IDs for doublon without ID ?", "Quit ?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            bool generateId = result == DialogResult.Yes;

            Dictionary<string, object> dico = new Dictionary<string, object>();

            List<TaxonTreeNode> All = new List<TaxonTreeNode>() { TaxonUtils.OriginalRoot };
            TaxonUtils.OriginalRoot.GetAllChildrenRecursively(All);

            List<string> doublons = new List<string>();

            foreach ( TaxonTreeNode node in All )
            {
                if (node.Desc.IsUnnamed)
                    continue;

                string name = node.Desc.RefMultiName.Main;
                
                if (dico.ContainsKey(name) )
                {
                    if (dico[name] is TaxonTreeNode)
                    {
                        doublons.Add(name);
                        List<TaxonTreeNode> list = new List<TaxonTreeNode>();
                        list.Add(dico[name] as TaxonTreeNode);
                        dico[name] = list;
                    }
                    (dico[name] as List<TaxonTreeNode>).Add(node);
                }
                else
                    dico[name] = node;
            }


            string file = Path.Combine(TaxonUtils.GetLogPath(), "CheckDuplicateNames.log");
            if (File.Exists(file)) File.Delete(file);

            UInt32 maxId = 0;
            int createIds = 0;
            using (StreamWriter outfile = new StreamWriter(file))
            {
                outfile.WriteLine("CheckDuplicateNames result ( " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " )\n");
                outfile.WriteLine("Find " +  doublons.Count.ToString() + " duplicated names\n");
                
                foreach (string dupName in doublons)
                {
                    List<TaxonTreeNode> duplicatas = dico[dupName] as List<TaxonTreeNode>;
                    if (duplicatas == null)
                        continue;

                    outfile.WriteLine("[ " +  dupName + " ], " + duplicatas.Count.ToString() + " occurences:");

                    foreach (TaxonTreeNode node in duplicatas)
                    {
                        string descId = "[id: " + node.Desc.OTTID + "]";
                        if (generateId && node.Desc.OTTID == 0)
                        {
                            if (maxId == 0)
                                maxId = TaxonIds.GetUnusedTolId(TaxonUtils.OriginalRoot);
                            node.Desc.OTTID = maxId++;
                            descId = "[id created: " + node.Desc.OTTID + "]";
                            createIds++;
                        }
                        outfile.WriteLine("    " + node.GetHierarchicalName() + " " + descId );
                    }
                }
            }
            
            string message = "Check duplicate names : \n\n";
            message += String.Format("    duplicate names : {0}\n\n", doublons.Count );
            if (createIds > 0)
                message += String.Format("    {0} ids have been created, do not forget to save\n\n", createIds );
            message += String.Format("for more details, look at CheckDuplicateNames.log file");
            Loggers.WriteInformation(LogTags.Data, message);
        }
    }
}
