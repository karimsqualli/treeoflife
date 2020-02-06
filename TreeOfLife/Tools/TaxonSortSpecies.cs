using System;
using System.ComponentModel;

namespace TreeOfLife.Tools
{
    [Description("Sort all taxon children")]
    [DisplayName("Sort children")]
    [Category("Tools/Taxons")]
    //[PresentInMode(false, true)]
    class TaxonSortSpecies : ITool
    {
        public bool CanActivate()
        {
            return TaxonUtils.OriginalRoot != null;
        }

        int NumTaxonSorted;
        public void Activate()
        {
            NumTaxonSorted = 0;
            TaxonUtils.OriginalRoot.ParseNode(SortChildren);

            string message = "Sort operation done: ";
            message += String.Format("{0} taxons with children sorted", NumTaxonSorted);
            Loggers.WriteInformation(LogTags.Data, message);
        }

        void SortChildren(TaxonTreeNode _taxon)
        {
            if (!_taxon.HasChildren) return;
            _taxon.SortChildren();
            NumTaxonSorted++;
        }
        
    }
}

