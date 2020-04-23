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
        public bool classicRankSet { get; set; } = false;
        public bool redListSet { get; set; } = false;

        public AddTaxonForm()
        {
            InitializeComponent();

            classicRankCB.DropDownStyle = ComboBoxStyle.DropDownList;
            classicRankCB.Text = string.Empty;

            foreach (string rank in Enum.GetNames(typeof(ClassicRankEnum)))
            {
                classicRankCB.Items.Add(rank);
            }


            RedListCategoryCB.DropDownStyle = ComboBoxStyle.DropDownList;
            classicRankCB.Text = string.Empty;

            foreach (string category in Enum.GetNames(typeof(RedListCategoryEnum)))
            {
                RedListCategoryCB.Items.Add(category);
            }

            AcceptButton = addButton;
            addButton.DialogResult = DialogResult.OK;
            addButton.Enabled = false;
            CancelButton = cancelButton;
            cancelButton.DialogResult = DialogResult.Cancel;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            classicRankSet = true;

            if (classicRankSet && redListSet)
            {
                addButton.Enabled = true;
            }
        }

        private void RedListCategoryCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            redListSet = true;

            if (classicRankSet && redListSet)
            {
                addButton.Enabled = true;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("cb1 : " + (classicRankCB.Text == string.Empty));
            if (classicRankCB.Text == string.Empty)
            {
                errorLabel.Text = "Please select a classic rank value";
                return;
            }

            Console.WriteLine("cb2 : " + (RedListCategory.Text == string.Empty));
            if (RedListCategory.Text == string.Empty)
            {
                errorLabel.Text = "Please select a red list category";
                return;
            }

            string nameString = nameTextBox.Text;

            if (nameString == string.Empty)
            {
                nameString = "Unnamed";
            }

            Helpers.MultiName name = new Helpers.MultiName(nameString);
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
