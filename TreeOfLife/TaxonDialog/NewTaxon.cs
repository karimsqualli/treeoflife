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
    public partial class NewTaxon : Localization.Form
    {
        public NewTaxon()
        {
            InitializeComponent();
            textBox1_TextChanged(this, null);
            CheckNameUsage = false;
        }

        public NewTaxon( string _name ) : this()
        {
            textBox1.Text = _name;
        }

        public bool CheckNameUsage { get; set; }
        public string TaxonName { get ; private set; }
        public bool CreateUnnamed { get { return String.IsNullOrWhiteSpace(textBox1.Text); } }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text))
                buttonCreate.Text = "Create unnamed";
            else
                buttonCreate.Text = "Create";

            //buttonCreate.Enabled = !String.IsNullOrWhiteSpace(textBox1.Text);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.OK;

            TaxonName = textBox1.Text.Trim();
            if (!CreateUnnamed && CheckNameUsage)
            {
                TaxonTreeNode otherNode = TaxonUtils.Root.FindTaxonByName(TaxonName.ToLower());
                if (otherNode != null)
                {
                    string message = "Name is already used, Create anyway ?";
                    message += "\n\nClick no to change name, Cancel to abort creation";
                    result = MessageBox.Show(message, "Warning !", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                        return;

                    if (result == DialogResult.Yes)
                        result = DialogResult.OK;
                }
            }

            DialogResult = result;
            Close();
        }
    }
}
