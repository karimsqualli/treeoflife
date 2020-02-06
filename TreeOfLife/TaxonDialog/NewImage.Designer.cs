namespace TreeOfLife.TaxonDialog
{
    partial class NewImage
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
            this.labelCollection = new System.Windows.Forms.Label();
            this.labelIndex = new System.Windows.Forms.Label();
            this.comboBoxCollection = new System.Windows.Forms.ComboBox();
            this.checkBoxMinor = new System.Windows.Forms.CheckBox();
            this.textBoxIndex = new System.Windows.Forms.TextBox();
            this.labelFullPath = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxNew = new System.Windows.Forms.PictureBox();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonWrite = new System.Windows.Forms.Button();
            this.taxonListImage1 = new TreeOfLife.Controls.TaxonListImage();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCollection
            // 
            this.labelCollection.AutoSize = true;
            this.labelCollection.Location = new System.Drawing.Point(3, 6);
            this.labelCollection.Name = "labelCollection";
            this.labelCollection.Size = new System.Drawing.Size(53, 13);
            this.labelCollection.TabIndex = 0;
            this.labelCollection.Text = "Collection";
            // 
            // labelIndex
            // 
            this.labelIndex.AutoSize = true;
            this.labelIndex.Location = new System.Drawing.Point(269, 6);
            this.labelIndex.Name = "labelIndex";
            this.labelIndex.Size = new System.Drawing.Size(33, 13);
            this.labelIndex.TabIndex = 1;
            this.labelIndex.Text = "Index";
            // 
            // comboBoxCollection
            // 
            this.comboBoxCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCollection.FormattingEnabled = true;
            this.comboBoxCollection.Location = new System.Drawing.Point(62, 3);
            this.comboBoxCollection.Name = "comboBoxCollection";
            this.comboBoxCollection.Size = new System.Drawing.Size(201, 21);
            this.comboBoxCollection.TabIndex = 3;
            this.comboBoxCollection.SelectedIndexChanged += new System.EventHandler(this.comboBoxCollection_SelectedIndexChanged);
            // 
            // checkBoxMinor
            // 
            this.checkBoxMinor.AutoSize = true;
            this.checkBoxMinor.Location = new System.Drawing.Point(414, 5);
            this.checkBoxMinor.Name = "checkBoxMinor";
            this.checkBoxMinor.Size = new System.Drawing.Size(52, 17);
            this.checkBoxMinor.TabIndex = 4;
            this.checkBoxMinor.Text = "Minor";
            this.checkBoxMinor.UseVisualStyleBackColor = true;
            this.checkBoxMinor.CheckStateChanged += new System.EventHandler(this.checkBoxMinor_CheckStateChanged);
            // 
            // textBoxIndex
            // 
            this.textBoxIndex.Location = new System.Drawing.Point(308, 3);
            this.textBoxIndex.Name = "textBoxIndex";
            this.textBoxIndex.Size = new System.Drawing.Size(30, 20);
            this.textBoxIndex.TabIndex = 5;
            this.textBoxIndex.TextChanged += new System.EventHandler(this.textBoxIndex_TextChanged);
            // 
            // labelFullPath
            // 
            this.labelFullPath.AutoSize = true;
            this.labelFullPath.Location = new System.Drawing.Point(11, 39);
            this.labelFullPath.Name = "labelFullPath";
            this.labelFullPath.Size = new System.Drawing.Size(45, 13);
            this.labelFullPath.TabIndex = 2;
            this.labelFullPath.Text = "FullPath";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.taxonListImage1);
            this.splitContainer1.Size = new System.Drawing.Size(483, 490);
            this.splitContainer1.SplitterDistance = 327;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.numericUpDown1);
            this.splitContainer2.Panel1.Controls.Add(this.buttonWrite);
            this.splitContainer2.Panel1.Controls.Add(this.textBoxPath);
            this.splitContainer2.Panel1.Controls.Add(this.labelFullPath);
            this.splitContainer2.Panel1.Controls.Add(this.textBoxIndex);
            this.splitContainer2.Panel1.Controls.Add(this.labelCollection);
            this.splitContainer2.Panel1.Controls.Add(this.checkBoxMinor);
            this.splitContainer2.Panel1.Controls.Add(this.labelIndex);
            this.splitContainer2.Panel1.Controls.Add(this.comboBoxCollection);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pictureBoxNew);
            this.splitContainer2.Size = new System.Drawing.Size(483, 327);
            this.splitContainer2.SplitterDistance = 105;
            this.splitContainer2.TabIndex = 0;
            // 
            // pictureBoxNew
            // 
            this.pictureBoxNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxNew.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxNew.Name = "pictureBoxNew";
            this.pictureBoxNew.Size = new System.Drawing.Size(483, 218);
            this.pictureBoxNew.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxNew.TabIndex = 0;
            this.pictureBoxNew.TabStop = false;
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(62, 36);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(409, 20);
            this.textBoxPath.TabIndex = 7;
            // 
            // buttonWrite
            // 
            this.buttonWrite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWrite.Location = new System.Drawing.Point(12, 69);
            this.buttonWrite.Name = "buttonWrite";
            this.buttonWrite.Size = new System.Drawing.Size(459, 23);
            this.buttonWrite.TabIndex = 7;
            this.buttonWrite.Text = "Write";
            this.buttonWrite.UseVisualStyleBackColor = true;
            this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
            // 
            // taxonListImage1
            // 
            this.taxonListImage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taxonListImage1.Location = new System.Drawing.Point(0, 0);
            this.taxonListImage1.Name = "taxonListImage1";
            this.taxonListImage1.Size = new System.Drawing.Size(483, 159);
            this.taxonListImage1.TabIndex = 0;
            this.taxonListImage1.Taxon = null;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(344, 4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // NewImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 490);
            this.Controls.Add(this.splitContainer1);
            this.Name = "NewImage";
            this.Text = "NewImage";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCollection;
        private System.Windows.Forms.Label labelIndex;
        private System.Windows.Forms.ComboBox comboBoxCollection;
        private System.Windows.Forms.CheckBox checkBoxMinor;
        private System.Windows.Forms.TextBox textBoxIndex;
        private System.Windows.Forms.Label labelFullPath;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox pictureBoxNew;
        private Controls.TaxonListImage taxonListImage1;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonWrite;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}