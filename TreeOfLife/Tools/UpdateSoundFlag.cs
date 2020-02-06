using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Update HasSound flag on Espece and Sub espece taxon")]
    [DisplayName("Update sounds")]
    [Category("Tools/Sounds")]
    [PresentInMode(true, true)]
    class UpdateSoundFlag : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            // retrouve la liste de toutes les especes / sous especes
            List<TaxonTreeNode> Species = new List<TaxonTreeNode>();
            TaxonUtils.OriginalRoot.GetAllChildrenRecursively(Species, ClassicRankEnum.Espece);
            TaxonUtils.OriginalRoot.GetAllChildrenRecursively(Species, ClassicRankEnum.SousEspece);

            int beforeWith = 0;
            int beforeWithout = 0;

            Dictionary<string, TaxonTreeNode> dico = new Dictionary<string, TaxonTreeNode>();
            foreach (TaxonTreeNode node in Species)
            {
                if (node.Desc.HasSound)
                    beforeWith++;
                else
                    beforeWithout++;

                node.Desc.HasSound = false;
                dico[node.Desc.RefMultiName.Main.ToLower()] = node;
            }

            string[] files = System.IO.Directory.GetFiles(TaxonUtils.GetSoundDirectory(), "*.wma");

            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(file).ToLower();
                if (dico.ContainsKey(name))
                    dico[name].Desc.HasSound = true;
            }

            int afterWith = 0;
            int afterWithout = 0;

            foreach (TaxonTreeNode node in Species)
            {
                if (node.Desc.HasSound)
                    afterWith++;
                else
                    afterWithout++;
            }

            string message = "Update sound flag : \n\n";
            message += String.Format("    Total with sound    : {0}, (before: {1})\n", afterWith, beforeWith);
            message += String.Format("    Total without sound : {0}, (before: {1})\n\n", afterWithout, beforeWithout);
            Loggers.WriteInformation(LogTags.Sound, message);
        }
    }
}
