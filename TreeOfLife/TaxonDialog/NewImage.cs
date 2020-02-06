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
    public partial class NewImage : Localization.Form
    {
        public NewImage( TaxonTreeNode _node, Image _newImage )
        {
            _Node = _node;
            _NewImage = _newImage;
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        TaxonTreeNode _Node;
        Image _NewImage;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _LockUpdate = true;

            foreach (ImageCollection collection in TaxonImages.Manager.CollectionsEnumerable())
                comboBoxCollection.Items.Add(collection);

            checkBoxMinor.Checked = false;
            pictureBoxNew.Image = _NewImage;
            taxonListImage1.Taxon = _Node;

            _LockUpdate = false;

            ImageCollection def = TaxonImages.Manager.DefaultImageCollection;
            if (def != null)
                comboBoxCollection.SelectedItem = def ?? comboBoxCollection.Items[0];

            numericUpDown1.Value = GetUnusedIndex();


            UpdateNameAndPath();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            TaxonImages.Manager.DefaultImageCollection = comboBoxCollection.SelectedItem as ImageCollection;
            base.OnClosing(e);
        }

        public bool CreateWithoutUI()
        {
            bool exist = false;
            TaxonImageDesc imageDesc = GetImageDesc( out exist );
            if (exist) return false;

            if (_Node.Desc.Images == null)
                _Node.Desc.Images = new List<TaxonImageDesc>();
            _Node.Desc.Images.Add(imageDesc);
            _NewImage.Save(imageDesc.GetPath(_Node.Desc));
            return true;
        }

        int GetUnusedIndex()
        {
            if (_Node == null) return 0;

            ImageCollection collection = comboBoxCollection.SelectedItem as ImageCollection;
            if (collection == null)
                collection = TaxonImages.Manager.DefaultImageCollection;
            bool minor = checkBoxMinor.Checked;

            List<int> usedIndex = new List<int>();
            foreach (TaxonImageDesc existent in _Node.Desc.Images)
            {
                if (existent.CollectionId != collection.Id) continue;
                if (existent.Secondary != minor) continue;
                if (existent.Index >= 0)
                    usedIndex.Add(existent.Index);
            }
            if (usedIndex.Count == 0) return 0;
            usedIndex.Sort();
            if (usedIndex[0] != 0) return 0;
            if (usedIndex.Count == 1) return 1;
            for (int i = 1; i < usedIndex.Count; i++)
                if (usedIndex[i] - usedIndex[i - 1] > 1) return usedIndex[i - 1] + 1;
            return usedIndex[usedIndex.Count - 1] + 1;
        }

        bool _LockUpdate = false;
        void UpdateNameAndPath()
        {
            if (_LockUpdate)
                return;

            bool exist = false;
            TaxonImageDesc imageDesc = GetImageDesc(out exist);

            textBoxPath.Text = imageDesc.IsALink ? imageDesc.GetLink() : imageDesc.GetPath(_Node.Desc);
            if (exist) taxonListImage1.SetImage(imageDesc);
            buttonWrite.Text = (exist ? "Overwrite" : "Create") + " " + imageDesc.GetName(_Node.Desc);
        }

        public TaxonImageDesc GetImageDesc()
        {
            bool exist = false;
            TaxonImageDesc imageDesc = GetImageDesc(out exist);
            return exist ? imageDesc : null;
        }

        TaxonImageDesc GetImageDesc(out bool _exist )
        {
            ImageCollection collection = comboBoxCollection.SelectedItem as ImageCollection;
            if (collection == null)
                collection = TaxonImages.Manager.DefaultImageCollection;

            int index = (int) numericUpDown1.Value;
            bool minor = checkBoxMinor.Checked;

            _exist = false;
            if (_Node.Desc.Images != null)
            {
                foreach (TaxonImageDesc existent in _Node.Desc.Images)
                {
                    if (existent.CollectionId != collection.Id) continue;
                    if (existent.Index != index) continue;
                    if (existent.Secondary != minor) continue;
                    _exist = true;
                    return existent;
                }
            }

            TaxonImageDesc imageDesc = new TaxonImageDesc() { CollectionId = collection.Id, Index = index, Secondary = minor };
            return imageDesc;
        }

        private void comboBoxCollection_SelectedIndexChanged(object sender, EventArgs e) { UpdateNameAndPath(); }
        private void checkBoxMinor_CheckStateChanged(object sender, EventArgs e) { UpdateNameAndPath(); }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { UpdateNameAndPath(); }
        
        private void textBoxIndex_TextChanged(object sender, EventArgs e)
        {
            UpdateNameAndPath();
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            bool exist = false;
            TaxonImageDesc imageDesc = GetImageDesc(out exist);

            if (exist)
            {
                string message = imageDesc.GetName(_Node.Desc) + " will be overwritten by new image";
                message += "\n\nProceed ?";
                DialogResult result = MessageBox.Show(message, "Overwrite ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
            }
            else
            {
                if (_Node.Desc.Images == null)
                    _Node.Desc.Images = new List<TaxonImageDesc>();
                _Node.Desc.Images.Add(imageDesc);
            }

            _NewImage.Save(imageDesc.GetPath(_Node.Desc));
            DialogResult = DialogResult.OK;
            Close();
        }

        
    }
}
