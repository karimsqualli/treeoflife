using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TreeOfLife.Tools
{
    [Description("Create location data according to current graph ...")]
    [DisplayName("Create from directory")]
    [Category("Tools/Location")]
    class LocationCreate : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    return;
                TaxonLocations.CreateFromDirectory(TaxonUtils.OriginalRoot, fbd.SelectedPath);
                TaxonUtils.Locations = TaxonLocations.Load(TaxonUtils.OriginalRoot);

            }
        }
    }
}
