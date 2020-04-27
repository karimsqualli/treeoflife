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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            button1.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            bool success = dataSettingsControl1.updateData();

            if (success)
            {
                TaxonUtils.MyConfig.offline = dataSettingsControl1.Offline;

                if (dataSettingsControl1.Offline)
                {
                    TaxonUtils.initCollections();
                }

                Close();
            } else
            {
                button1.Enabled = true;
            }
        }
    }
}
