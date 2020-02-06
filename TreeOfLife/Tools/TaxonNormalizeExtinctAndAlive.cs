using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TreeOfLife.Tools
{
    [Description("Force extinct / alive status in certain situation")]
    [DisplayName("Normalize extinct/alive status")]
    [Category("Tools/Taxons")]
    [PresentInMode(false,false)]
    class TaxonNormalizeExtinctAlive : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        int NewExtinct;
        int NewAlive;
        int ExtinctWithSuffix;
        public void Activate()
        {
            NewExtinct = 0;
            NewAlive = 0;
            ExtinctWithSuffix = 0;
            TaxonUtils.OriginalRoot.ParseNodeChildrenFirst(CheckExtinctStatus);

            string message = "Check extinct operation done: ";
            message += String.Format("\n  {0} taxons bcame alive", NewAlive);
            message += String.Format("\n  {0} taxons became extinct", NewExtinct);
            message += String.Format("\n  {0} taxons have to became extinct but use cross suffix, ", ExtinctWithSuffix);
            message += String.Format("\n      Remove dead cross tool before");
            Loggers.WriteInformation(LogTags.Data, message);
        }

        void CheckExtinctStatus(TaxonTreeNode _taxon)
        {
            if (!_taxon.HasChildren) return;

            bool someChildAlive = false;
            foreach (TaxonTreeNode node in _taxon.Children)
                if (!node.Desc.IsExtinct)
                {
                    someChildAlive = true;
                    break;
                }

            if (someChildAlive)
            {
                if (_taxon.Desc.IsExtinct)
                {
                    if (!_taxon.Desc.HasFlag(FlagsEnum.Extinct))
                        ExtinctWithSuffix++;
                    else
                    {
                        _taxon.Desc.UnsetFlag(FlagsEnum.Extinct);
                        NewAlive++;
                    }
                }
            }
            else
            {
                if (!_taxon.Desc.IsExtinct)
                {
                    _taxon.Desc.SetFlag(FlagsEnum.Extinct);
                    NewExtinct++;
                }
            }
        }
    }
}

