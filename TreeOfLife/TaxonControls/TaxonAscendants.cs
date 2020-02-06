using System.Collections.Generic;
using System.ComponentModel;

namespace TreeOfLife
{
    [Description("List ascendants of selected taxon")]
    [DisplayName("Ascendants")]
    [Controls.Icon("TaxonAscendant")]
    public partial class TaxonAscendants : Controls.TaxonControl
    {
        //---------------------------------------------------------------------------------
        public TaxonAscendants()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Ascendants"; }

        //---------------------------------------------------------------------------------
        public override void OnSelectTaxon(TaxonTreeNode _taxon)
        {
            Selected = _taxon;
            OnBelowChanged(_taxon);
        }

        //---------------------------------------------------------------------------------
        public override void OnTaxonChanged(object sender, TaxonTreeNode _taxon) 
        {
            UpdateList();
        }

        //---------------------------------------------------------------------------------
        public override void OnBelowChanged(TaxonTreeNode _taxon)
        {
            int index = _taxon == null ? -1 : listAscendants.Items.IndexOf(_taxon);
            listAscendants.SelectedIndex = index;
        }

        //---------------------------------------------------------------------------------
        TaxonTreeNode _Selected = null;
        public TaxonTreeNode Selected
        {
            get { return _Selected; }
            set
            {
                if (_Selected == value) return;
                _Selected = value;
                UpdateList();
            }
        }

        //---------------------------------------------------------------------------------
        void UpdateList()
        {
            listAscendants.Items.Clear();
            if (_Selected == null) return;
            List<TaxonTreeNode> Ascendants = new List<TaxonTreeNode>();
            _Selected.GetAllParents(Ascendants, true, true, false );
            listAscendants.Items.AddRange(Ascendants.ToArray());
        }
    }
}
