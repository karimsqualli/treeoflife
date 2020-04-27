using Flurl;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    public partial class InitForm : Localization.Form
    {
        private bool emptyTree { get; set; } = false;
        public bool quit { get; set; } = false;

        public InitForm()
        {
            TopMost = true;

            InitializeComponent();
        }

        private void validateButton_Click(object sender, EventArgs e)
        {
            validateButton.Enabled = false;
            bool success = dataSettingsControl1.updateData();
            
            if (success)
            {
                if (emptyTree)
                {
                    TaxonUtils.MyConfig.emptyTreeAtStartup = true;
                    TaxonUtils.MyConfig.TaxonFileName = "";
                }
                Close();
            } else
            {
                validateButton.Enabled = true;
            }
        }

        private void loadTreeFileButon_Click(object sender, EventArgs e)
        {
            TaxonUtils.TaxonDataInit();
            treeFileNameTextBox.Text = TaxonUtils.GetTaxonFileName();
        }

        private void loadTreeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            loadTreeFileButon.Enabled = true;
            emptyTree = false;
        }

        private void emptyTreeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            loadTreeFileButon.Enabled = false;
            emptyTree = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            quit = true;
            Close();
        }
    }
}
