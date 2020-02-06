using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    public partial class TaxonListBoxFloating : Form
    {
        public TaxonListBoxFloating()
        {
            InitializeComponent();
        }

        public Controls.TaxonListBox TaxonListBox { get => taxonListBox; }

        public void SetContent( List<TaxonTreeNodeNamed> _nodes)
        {
            taxonListBox.SuspendLayout();
            taxonListBox.DataSource = null;
            if (_nodes != null)
                taxonListBox.DataSource = _nodes;
            taxonListBox.SelectedIndex = -1;
            taxonListBox.ResumeLayout();
        }
    }
}
