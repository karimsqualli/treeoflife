using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.TaxonDialog
{
    // todo clean
    public partial class SynonymsDialog : Localization.Form
    {
        public SynonymsDialog(Helpers.MultiName _multiName )
        {
            InitializeComponent();
            _Name = _multiName;
            Result = null;
            if (_Name != null)
            {
                textBoxName.Text = _Name.Main;
                string[] list = _Name.GetSynonymsArray();
                list?.ToList().ForEach( name => dataGridView1.Rows.Add(new object[] { name }));
            }
            UpdateNeeded = false;
            DialogResult = DialogResult.Cancel;
        }

        private Helpers.MultiName _Name;
        public Helpers.MultiName Result;

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (UpdateNeeded)
            {
                DialogResult result = MessageBox.Show("Some modifications not saved, quit anyway ?", "Alternative names", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    e.Cancel = true;
            }
        }

        void UpdateDesc()
        {
            if (_Name == null) return;
            
            List<string> list = new List<string>() { _Name.Main };
            foreach (DataGridViewRow  row in dataGridView1.Rows)
            {
                string name = row.Cells[0].Value as string;
                if (string.IsNullOrWhiteSpace(name)) continue;
                name = name.Trim().ToLower();
                if ( !list.Contains(name)) 
                    list.Add(name);
            }
            Result = new Helpers.MultiName(string.Join(Helpers.MultiName.SeparatorAsString, list));
            UpdateNeeded = false;
        }
        
        public bool UpdateNeeded
        {
            get { return buttonOK.Enabled; }
            private set { buttonOK.Enabled = value; }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateNeeded = true;
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateNeeded = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateNeeded = true;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            UpdateDesc();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
