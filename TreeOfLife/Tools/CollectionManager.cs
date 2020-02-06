using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeOfLife.Controls;
using TreeOfLife.TaxonDialog;

namespace TreeOfLife.Tools
{
    [Description("Open a dialog to manage images collections")]
    [DisplayName("Manage")]
    [Category("Tools/Collections")]
    [PresentInMode(true, true)]
    class CollectionManager : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            CollectionManagerDialog dlg = new CollectionManagerDialog();
            dlg.ShowDialog();
            TaxonUtils.OriginalRoot.UpdateAvailableImages();
            TaxonImages.Manager.Clear();
            TaxonControlList.OnAvailableImagesChanged();
            TaxonControlList.OnReselectTaxon();
        }
    }
}


