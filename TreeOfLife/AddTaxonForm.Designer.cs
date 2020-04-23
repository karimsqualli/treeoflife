namespace TreeOfLife
{
    partial class AddTaxonForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.frenchNameLabel = new System.Windows.Forms.Label();
            this.frenchNameTextBox = new System.Windows.Forms.TextBox();
            this.classicRankLabel = new System.Windows.Forms.Label();
            this.RedListCategory = new System.Windows.Forms.Label();
            this.classicRankCB = new System.Windows.Forms.ComboBox();
            this.RedListCategoryCB = new System.Windows.Forms.ComboBox();
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(13, 13);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(57, 17);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name : ";
            this.NameLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(13, 34);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(269, 22);
            this.nameTextBox.TabIndex = 1;
            // 
            // frenchNameLabel
            // 
            this.frenchNameLabel.AutoSize = true;
            this.frenchNameLabel.Location = new System.Drawing.Point(13, 63);
            this.frenchNameLabel.Name = "frenchNameLabel";
            this.frenchNameLabel.Size = new System.Drawing.Size(103, 17);
            this.frenchNameLabel.TabIndex = 2;
            this.frenchNameLabel.Text = "French name : ";
            // 
            // frenchNameTextBox
            // 
            this.frenchNameTextBox.Location = new System.Drawing.Point(13, 84);
            this.frenchNameTextBox.Name = "frenchNameTextBox";
            this.frenchNameTextBox.Size = new System.Drawing.Size(269, 22);
            this.frenchNameTextBox.TabIndex = 3;
            // 
            // classicRankLabel
            // 
            this.classicRankLabel.AutoSize = true;
            this.classicRankLabel.Location = new System.Drawing.Point(13, 113);
            this.classicRankLabel.Name = "classicRankLabel";
            this.classicRankLabel.Size = new System.Drawing.Size(84, 17);
            this.classicRankLabel.TabIndex = 4;
            this.classicRankLabel.Text = "Classic rank";
            // 
            // RedListCategory
            // 
            this.RedListCategory.AutoSize = true;
            this.RedListCategory.Location = new System.Drawing.Point(10, 159);
            this.RedListCategory.Name = "RedListCategory";
            this.RedListCategory.Size = new System.Drawing.Size(114, 17);
            this.RedListCategory.TabIndex = 6;
            this.RedListCategory.Text = "Red list category";
            // 
            // classicRankCB
            // 
            this.classicRankCB.FormattingEnabled = true;
            this.classicRankCB.Location = new System.Drawing.Point(13, 134);
            this.classicRankCB.Name = "classicRankCB";
            this.classicRankCB.Size = new System.Drawing.Size(269, 24);
            this.classicRankCB.TabIndex = 7;
            this.classicRankCB.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // RedListCategoryCB
            // 
            this.RedListCategoryCB.FormattingEnabled = true;
            this.RedListCategoryCB.Location = new System.Drawing.Point(13, 180);
            this.RedListCategoryCB.Name = "RedListCategoryCB";
            this.RedListCategoryCB.Size = new System.Drawing.Size(269, 24);
            this.RedListCategoryCB.TabIndex = 8;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(49, 227);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 34);
            this.addButton.TabIndex = 9;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(166, 226);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 35);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // AddTaxonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 273);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.RedListCategoryCB);
            this.Controls.Add(this.classicRankCB);
            this.Controls.Add(this.RedListCategory);
            this.Controls.Add(this.classicRankLabel);
            this.Controls.Add(this.frenchNameTextBox);
            this.Controls.Add(this.frenchNameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.NameLabel);
            this.Name = "AddTaxonForm";
            this.Text = "Créer un taxon";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label frenchNameLabel;
        private System.Windows.Forms.TextBox frenchNameTextBox;
        private System.Windows.Forms.Label classicRankLabel;
        private System.Windows.Forms.Label RedListCategory;
        private System.Windows.Forms.ComboBox classicRankCB;
        private System.Windows.Forms.ComboBox RedListCategoryCB;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button cancelButton;
    }
}