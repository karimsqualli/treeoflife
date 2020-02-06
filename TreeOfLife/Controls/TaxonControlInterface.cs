using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TreeOfLife.Controls
{
    //=============================================================================================
    // Interface for Taxon Control (Form or UserControl)
    //
    public interface ITaxonControl : GUI.GuiControlInterface
    {
        //-------------------------------------------------------------------
        void SetRoot(TaxonTreeNode _taxon);

        //-------------------------------------------------------------------
        void OnSelectTaxon(TaxonTreeNode _taxon);
        void OnReselectTaxon(TaxonTreeNode _taxon);
        void OnBelowChanged(TaxonTreeNode _taxonBelow);
        void OnRefreshAll();
        void OnRefreshGraph();
        void OnViewRectangleChanged(Rectangle R);

        //-------------------------------------------------------------------
        void OnTaxonChanged(object sender, TaxonTreeNode _taxon);

        //-------------------------------------------------------------------
        void OnOptionChanged();

        //-------------------------------------------------------------------
        void OnAvailableImagesChanged();
    }
}
