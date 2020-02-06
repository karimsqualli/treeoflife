using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Export french names in a file")]
    [DisplayName("Export french names")]
    [Category("Tools/Names")]
    class ExportFrenchNames : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "txt files (*.txt)|*.txt";
            sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            if (sfd.ShowDialog() != DialogResult.OK) return;
            if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);

            int count = 0;
            using (StreamWriter sw = new StreamWriter(sfd.FileName))
            {
                TaxonUtils.OriginalRoot.ParseNode((d) =>
                {
                    if (d.Desc.HasFrenchName)
                    {
                        sw.WriteLine(d.GetHierarchicalName() + ";" + d.Desc.FrenchAllNames);
                        count++;
                    }
                });
            }


            string message = "Export french names:";
            message += String.Format("{0} taxon with french name(s) exported", count);
            Loggers.WriteInformation(LogTags.Data, message);
        }
    }


}
