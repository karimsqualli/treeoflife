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
    public partial class AddTaxonForm : Form
    {
        public TaxonTreeNode node { get; set; } = null;

        public AddTaxonForm()
        {
            InitializeComponent();

            foreach (string rank in Enum.GetNames(typeof(ClassicRankEnum)))
            {
                classicRankCB.Items.Add(rank);
            }

            foreach (string category in Enum.GetNames(typeof(RedListCategoryEnum)))
            {
                RedListCategoryCB.Items.Add(category);
            }

            AcceptButton = addButton;
            addButton.DialogResult = DialogResult.OK;
            CancelButton = cancelButton;
            cancelButton.DialogResult = DialogResult.Cancel;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Helpers.MultiName name = new Helpers.MultiName(nameTextBox.Text);
            Helpers.MultiName frenchName = new Helpers.MultiName(frenchNameTextBox.Text);
            ClassicRankEnum classicRank = ClassicRankEnum.None;
            Enum.TryParse<ClassicRankEnum>(classicRankCB.Text, out classicRank);
            RedListCategoryEnum redListCategory = RedListCategoryEnum.NotEvaluated;
            Enum.TryParse<RedListCategoryEnum>(RedListCategoryCB.Text, out redListCategory);

            TaxonDesc desc = new TaxonDesc
            {
                RefMultiName = name,
                ClassicRank = classicRank,
                RedListCategory = redListCategory,
                FrenchMultiName = frenchName
            };


            node = new TaxonTreeNode
            {
                Desc = desc
            };

            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
