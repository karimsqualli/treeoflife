using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace TreeOfLife.Controls
{
    public partial class TaxonListImage : Localization.UserControl
    {
        public TaxonListImage()
        {
            InitializeComponent();
            taxonImageControl.Data.OnCurrentImageIndexChanged += ImageControl_OnCurrentImageIndexChanged;
            taxonImageControl.OnContextMenuOpening += ImageControl_OnContextMenuOpening;
        }

        private TaxonTreeNode _Taxon = null;
        public TaxonTreeNode Taxon
        {
            get { return _Taxon; }
            set
            {
                if (_Taxon == value) return;
                _Taxon = value;
                UpdateUI();
            }
        }

        public void SetImage( TaxonImageDesc _image )
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Tag == _image)
                    row.Selected = true;
            }
        }

        public void UpdateUI()
        {
            dataGridView1.Rows.Clear();

            if (_Taxon != null && _Taxon.Desc.Images != null)
            {
                foreach (TaxonImageDesc image in _Taxon.Desc.Images)
                {
                    string name = image.GetName(_Taxon.Desc);
                    string path = image.IsALink ? image.GetLink() : image.GetPath(_Taxon.Desc);
                    string collection = image.GetCollectionName();
                    dataGridView1.Rows.Add(new object[] { name, collection, !image.Secondary, image.Index.ToString(), path });
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = image;
                }
            }

            if (dataGridView1.RowCount > 0)
                dataGridView1.Rows[0].Selected = true;

            taxonImageControl.CurrentTaxon = null;
            taxonImageControl.CurrentTaxon = _Taxon;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
                taxonImageControl.SetImageDesc(dataGridView1.SelectedRows[0].Tag as TaxonImageDesc);
        }

        void ImageControl_OnCurrentImageIndexChanged(object sender, EventArgs e)
        {
            TaxonImageDesc cur = taxonImageControl.GetImageDesc();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Tag == cur)
                    row.Selected = true;
            }
        }

        private void ImageControl_OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (Taxon == null) return;
            ContextMenuStrip menu = sender as ContextMenuStrip;
            menu.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem menuItem;
            menuItem = new ToolStripMenuItem(Localization.Manager.Get( "_GotoFile", "Goto file"), null, new System.EventHandler(OnGotoFile));
            string fileToSelect = taxonImageControl.GetImagePath();
            menuItem.Enabled = fileToSelect != null && System.IO.File.Exists(fileToSelect);
            menuItem.Tag = fileToSelect;
            menu.Items.Add(menuItem);
            //menu.Items.Add("Browse for image file", null, new System.EventHandler(OnBrowseImage));
        }

        private void OnGotoFile(object sender, EventArgs e)
        {
            string fileToSelect = (sender as ToolStripMenuItem).Tag as string;
            string args = string.Format("/Select, {0}", fileToSelect);
            ProcessStartInfo pfi = new ProcessStartInfo("Explorer.exe", args);
            System.Diagnostics.Process.Start(pfi);
        }
        
    }
}
