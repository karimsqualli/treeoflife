using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeOfLife.TaxonDialog;


namespace TreeOfLife.Tools
{
    [Description("Open a dialog to manage synonyms")]
    [DisplayName("Edit")]
    [Category("Tools/Synonyms")]
    [PresentInMode(true, true)]
    class SynonymsEdition : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        public void Activate()
        {
            Synonyms S = new Synonyms(TaxonUtils.OriginalRoot);
            S.Edit();
        }
    }
}
