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
    public partial class SynonymsManagerDialog : Localization.Form
    {
        public SynonymsManagerDialog(Synonyms S)
        {
            InitializeComponent();
            FillDataGrid(S);
            SaveNeeded = false;
        }

        void FillDataGrid(List<Synonym> _list )
        {
            dataGridView1.Rows.Clear();
            foreach (Synonym data in _list)
            {
                int index = dataGridView1.Rows.Add(new object[] { data.Name, data.AllSynonyms });
                dataGridView1.Rows[index].Tag = data;
                dataGridView1.Rows[index].Cells[0].ReadOnly = data.Desc != null;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (SaveNeeded)
            {
                DialogResult result = MessageBox.Show("Some modifications not saved, quit anyway ?", "Synonyms", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    e.Cancel = true;
            }
        }

        void Save()
        {
            List<Synonym> list = new List<Synonym>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string name = row.Cells[0].Value as string;
                string syns = row.Cells[1].Value as string;
                if (name == null || syns == null) continue;
                name = name.Trim();
                syns = syns.ToLower().Trim();

                if (row.Tag != null && row.Tag is Synonym)
                {
                    Synonym S = row.Tag as Synonym;
                    if (S.Desc == null)
                    {
                        if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(syns)) continue;
                        S.Name = name;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(syns))
                        {
                            S.Desc.RefMultiName = new Helpers.MultiName(S.Desc.RefMultiName.Main);
                            continue;
                        }
                    }
                    S.AllSynonyms = syns;
                    list.Add(S);
                }
                else
                {

                    if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(syns)) continue;
                    list.Add(new Synonym(name, syns));
                }
            }

            TaxonSearch searchTool = null;
            foreach (Synonym s in list)
            {
                if (s.Desc == null)
                {
                    if (searchTool == null)
                        searchTool = new TaxonSearch(TaxonUtils.OriginalRoot);
                    TaxonTreeNode node = searchTool.FindOne(s.Name);
                    if (node != null)
                        s.Desc = node.Desc;
                }
                /*
                 * todo ??
                if (s.Desc != null)
                    s.Desc.AlternativeNames = s.AllSynonyms;
                else
                    s.Name = "*" + s.Name.Trim('*').Trim();
                    */
            }

            FillDataGrid(list);
            SaveNeeded = false;
        }
        
        public bool SaveNeeded
        {
            get { return buttonSave.Enabled; }
            private set { buttonSave.Enabled = value; }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            SaveNeeded = true;
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SaveNeeded = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            SaveNeeded = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
