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
    [Description("Selection history")]
    [DisplayName("History")]
    [Controls.IconAttribute("TaxonHistory")]
    public partial class TaxonHistory : Controls.TaxonControl
    {
        //---------------------------------------------------------------------------------
        public TaxonHistory()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "History"; }

        //---------------------------------------------------------------------------------
        public override void OnSelectTaxon(TaxonTreeNode _taxon)
        {
            taxonListBox.DataSource = null;
            taxonListBox.DataSource = TaxonUtils.History;
        }

        //---------------------------------------------------------------------------------
        public override void OnTaxonChanged(object sender, TaxonTreeNode _taxon)
        {
            taxonListBox.DataSource = null;
            taxonListBox.DataSource = TaxonUtils.History;
        }
    }
}
