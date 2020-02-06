using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    /*
    [Description("Update HasImage flag on Espece and Sub espece taxon")]
    [DisplayName("Update image flag")]
    [Category("JacoTools/Images")]
    class UpdateImageFlag // : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            // retrouve la liste de toutes les especes / sous especes
            List<TaxonTreeNode> Species = new List<TaxonTreeNode>();
            TaxonUtils.OriginalRoot.getAllChildrenRecursively(Species, ClassicRankEnum.Espece);
            TaxonUtils.OriginalRoot.getAllChildrenRecursively(Species, ClassicRankEnum.SousEspece);

            int countWith = 0;
            int countWithout = 0;
            int countNewWith = 0;
            int countNewWithout = 0;

            foreach (TaxonTreeNode t in Species)
            {
                string imagePath = TaxonUtils.GetImageFullPath(t);
                if (File.Exists(imagePath))
                {
                    countWith++;
                    if (!t.Desc.HasImage)
                    {
                        countNewWith++;
                        t.Desc.HasImage = true;
                    }
                }
                else
                {
                    countWithout++;
                    if (t.Desc.HasImage)
                    {
                        countNewWithout++;
                        t.Desc.HasImage = false;
                    }
                }
            }

            string message = "Update image flag : \n\n";
            message += String.Format("    Total with image    : {0}, (new: {1})\n", countWith, countNewWith);
            message += String.Format("    Total without image : {0}, (new: {1})", countWithout, countNewWithout );
            LogMessages.AddInformation(LogTags.Data, message);
        }
    }
    */
}
