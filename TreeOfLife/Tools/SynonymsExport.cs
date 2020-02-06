using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Export latin synonyms to a file")]
    [DisplayName("Export")]
    [Category("Tools/Synonyms")]
    class ExportSynonyms: ITool
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
                    if (d.Desc.RefMultiName.HasSynonym)
                    {
                        //sw.WriteLine(d.GetHierarchicalName() + ";" + d.Desc.RefMultiName.Full);
                        sw.WriteLine(d.Desc.RefMultiName.Full);
                        count++;
                    }
                });
            }

            string message = "Export synonyms:";
            message += String.Format("{0} taxon with synonyms exported", count);
            Loggers.WriteInformation(LogTags.Data, message);
        }
    }


}
