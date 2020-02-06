using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TreeOfLife.Tools
{
    [Description("Parse all taxon to remove underscore found in name")]
    [DisplayName("Remove underscore in names")]
    [Category("Tools/Taxons")]
    [PresentInMode(false, false)]
    class RemoveUnderscoreInName : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        int NumNameChanged;
        public void Activate()
        {
            NumNameChanged = 0;
            TaxonUtils.OriginalRoot.ParseNodeDesc(RemoveUnderscore);

            string message = "Remove duplicate names:";
            message += String.Format("{0} names changed", NumNameChanged);
            Loggers.WriteInformation(LogTags.Data, message);
        }

        void RemoveUnderscore(TaxonDesc _desc)
        {
            if (!_desc.RefAllNames.Contains('_')) return;
            _desc.RefMultiName = new Helpers.MultiName(_desc.RefAllNames.Replace('_', ' '));
            NumNameChanged++;
        }
    }
}
