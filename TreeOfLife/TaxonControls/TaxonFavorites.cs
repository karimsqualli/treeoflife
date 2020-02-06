using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    [Description("List of saved favorites")]
    [DisplayName("Favorites")]
    [Controls.IconAttribute("TaxonFavorites")]
    public partial class TaxonFavorites : Controls.TaxonControl
    {
        //---------------------------------------------------------------------------------
        public TaxonFavorites()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Favorites"; }

        //---------------------------------------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            taxonListBox.DataSource = TaxonUtils.Favorites;
            TaxonUtils.OnFavoritesChanged += TaxonUtils_OnFavoritesChanged;
        }

        //---------------------------------------------------------------------------------
        protected override void OnClose()
        {
            base.OnClose();
            TaxonUtils.OnFavoritesChanged -= TaxonUtils_OnFavoritesChanged;
        }

        //---------------------------------------------------------------------------------
        void TaxonUtils_OnFavoritesChanged(object sender, EventArgs e)
        {
            taxonListBox.DataSource = null;
            taxonListBox.DataSource = TaxonUtils.Favorites;
        }

        //---------------------------------------------------------------------------------
        public override void OnSelectTaxon(TaxonTreeNode _taxon)
        {
            if (TaxonUtils.Favorites.Contains(_taxon))
                taxonListBox.SelectedItem = _taxon;
            else
                taxonListBox.SelectedItem = null;
        }

        //---------------------------------------------------------------------------------
        public override void OnTaxonChanged(object sender, TaxonTreeNode _taxon)
        {
            taxonListBox.DataSource = null;
            taxonListBox.DataSource = TaxonUtils.Favorites;
        }
    }
}
