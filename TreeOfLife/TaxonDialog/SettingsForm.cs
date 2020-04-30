using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeOfLife.Controls;

namespace TreeOfLife.TaxonDialog
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            bool success = dataSettingsControl1.updateData();

            if (success)
            {
                TaxonUtils.initCollections();
                TaxonUtils.OriginalRoot.UpdateAvailableImages();
                TaxonControlList.OnAvailableImagesChanged();
                Close();
            } else
            {
                button1.Enabled = true;
            }
        }
    }
}
