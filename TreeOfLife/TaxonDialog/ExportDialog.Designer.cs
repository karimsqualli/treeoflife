namespace TreeOfLife.TaxonDialog
{
    partial class ExportDialog
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
            this.labelFilename = new System.Windows.Forms.Label();
            this.textBoxFilename = new System.Windows.Forms.TextBox();
            this.buttonBrowseFilename = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelFormat = new System.Windows.Forms.Label();
            this.checkBoxFormatTxt = new System.Windows.Forms.CheckBox();
            this.checkBoxFormatCsv = new System.Windows.Forms.CheckBox();
            this.checkBoxFormatHtml = new System.Windows.Forms.CheckBox();
            this.labelInfos = new System.Windows.Forms.Label();
            this.checkBoxLatin = new System.Windows.Forms.CheckBox();
            this.checkBoxFrench = new System.Windows.Forms.CheckBox();
            this.checkBoxClassicRank = new System.Windows.Forms.CheckBox();
            this.checkBoxAscendants = new System.Windows.Forms.CheckBox();
            this.checkBoxImages = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelFilename
            // 
            this.labelFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFilename.AutoSize = true;
            this.labelFilename.Location = new System.Drawing.Point(12, 147);
            this.labelFilename.Name = "labelFilename";
            this.labelFilename.Size = new System.Drawing.Size(68, 13);
            this.labelFilename.TabIndex = 0;
            this.labelFilename.Text = "Export to file:";
            // 
            // textBoxFilename
            // 
            this.textBoxFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilename.Location = new System.Drawing.Point(40, 163);
            this.textBoxFilename.Name = "textBoxFilename";
            this.textBoxFilename.Size = new System.Drawing.Size(274, 20);
            this.textBoxFilename.TabIndex = 1;
            // 
            // buttonBrowseFilename
            // 
            this.buttonBrowseFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseFilename.Location = new System.Drawing.Point(315, 161);
            this.buttonBrowseFilename.Name = "buttonBrowseFilename";
            this.buttonBrowseFilename.Size = new System.Drawing.Size(23, 23);
            this.buttonBrowseFilename.TabIndex = 2;
            this.buttonBrowseFilename.Text = "...";
            this.buttonBrowseFilename.UseVisualStyleBackColor = true;
            this.buttonBrowseFilename.Click += new System.EventHandler(this.buttonBrowseFilename_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(182, 207);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 23);
            this.buttonExport.TabIndex = 3;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(263, 207);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelFormat
            // 
            this.labelFormat.AutoSize = true;
            this.labelFormat.Location = new System.Drawing.Point(12, 10);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(58, 13);
            this.labelFormat.TabIndex = 4;
            this.labelFormat.Text = "File Format";
            // 
            // checkBoxFormatTxt
            // 
            this.checkBoxFormatTxt.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxFormatTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxFormatTxt.Location = new System.Drawing.Point(32, 30);
            this.checkBoxFormatTxt.Name = "checkBoxFormatTxt";
            this.checkBoxFormatTxt.Size = new System.Drawing.Size(110, 30);
            this.checkBoxFormatTxt.TabIndex = 5;
            this.checkBoxFormatTxt.Text = "Text (.txt)";
            this.checkBoxFormatTxt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxFormatTxt.UseVisualStyleBackColor = true;
            this.checkBoxFormatTxt.CheckedChanged += new System.EventHandler(this.checkBoxFormatTxt_CheckedChanged);
            // 
            // checkBoxFormatCsv
            // 
            this.checkBoxFormatCsv.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxFormatCsv.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxFormatCsv.Location = new System.Drawing.Point(32, 69);
            this.checkBoxFormatCsv.Name = "checkBoxFormatCsv";
            this.checkBoxFormatCsv.Size = new System.Drawing.Size(110, 30);
            this.checkBoxFormatCsv.TabIndex = 6;
            this.checkBoxFormatCsv.Text = "Excel (.csv)";
            this.checkBoxFormatCsv.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxFormatCsv.UseVisualStyleBackColor = true;
            this.checkBoxFormatCsv.CheckedChanged += new System.EventHandler(this.checkBoxFormatCsv_CheckedChanged);
            // 
            // checkBoxFormatHtml
            // 
            this.checkBoxFormatHtml.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxFormatHtml.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxFormatHtml.Location = new System.Drawing.Point(32, 107);
            this.checkBoxFormatHtml.Name = "checkBoxFormatHtml";
            this.checkBoxFormatHtml.Size = new System.Drawing.Size(110, 30);
            this.checkBoxFormatHtml.TabIndex = 7;
            this.checkBoxFormatHtml.Text = ".html";
            this.checkBoxFormatHtml.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxFormatHtml.UseVisualStyleBackColor = true;
            this.checkBoxFormatHtml.CheckedChanged += new System.EventHandler(this.checkBoxFormatHtml_CheckedChanged);
            // 
            // labelInfos
            // 
            this.labelInfos.AutoSize = true;
            this.labelInfos.Location = new System.Drawing.Point(199, 10);
            this.labelInfos.Name = "labelInfos";
            this.labelInfos.Size = new System.Drawing.Size(30, 13);
            this.labelInfos.TabIndex = 8;
            this.labelInfos.Text = "Infos";
            // 
            // checkBoxLatin
            // 
            this.checkBoxLatin.AutoSize = true;
            this.checkBoxLatin.Checked = true;
            this.checkBoxLatin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLatin.Location = new System.Drawing.Point(219, 30);
            this.checkBoxLatin.Name = "checkBoxLatin";
            this.checkBoxLatin.Size = new System.Drawing.Size(80, 17);
            this.checkBoxLatin.TabIndex = 9;
            this.checkBoxLatin.Text = "Latin Name";
            this.checkBoxLatin.UseVisualStyleBackColor = true;
            // 
            // checkBoxFrench
            // 
            this.checkBoxFrench.AutoSize = true;
            this.checkBoxFrench.Checked = true;
            this.checkBoxFrench.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFrench.Location = new System.Drawing.Point(219, 53);
            this.checkBoxFrench.Name = "checkBoxFrench";
            this.checkBoxFrench.Size = new System.Drawing.Size(90, 17);
            this.checkBoxFrench.TabIndex = 10;
            this.checkBoxFrench.Text = "French Name";
            this.checkBoxFrench.UseVisualStyleBackColor = true;
            // 
            // checkBoxClassicRank
            // 
            this.checkBoxClassicRank.AutoSize = true;
            this.checkBoxClassicRank.Checked = true;
            this.checkBoxClassicRank.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxClassicRank.Location = new System.Drawing.Point(219, 76);
            this.checkBoxClassicRank.Name = "checkBoxClassicRank";
            this.checkBoxClassicRank.Size = new System.Drawing.Size(83, 17);
            this.checkBoxClassicRank.TabIndex = 11;
            this.checkBoxClassicRank.Text = "Classic rank";
            this.checkBoxClassicRank.UseVisualStyleBackColor = true;
            // 
            // checkBoxAscendants
            // 
            this.checkBoxAscendants.AutoSize = true;
            this.checkBoxAscendants.Location = new System.Drawing.Point(219, 99);
            this.checkBoxAscendants.Name = "checkBoxAscendants";
            this.checkBoxAscendants.Size = new System.Drawing.Size(82, 17);
            this.checkBoxAscendants.TabIndex = 12;
            this.checkBoxAscendants.Text = "Ascendants";
            this.checkBoxAscendants.UseVisualStyleBackColor = true;
            // 
            // checkBoxImages
            // 
            this.checkBoxImages.AutoSize = true;
            this.checkBoxImages.Checked = true;
            this.checkBoxImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxImages.Location = new System.Drawing.Point(219, 120);
            this.checkBoxImages.Name = "checkBoxImages";
            this.checkBoxImages.Size = new System.Drawing.Size(60, 17);
            this.checkBoxImages.TabIndex = 12;
            this.checkBoxImages.Text = "Images";
            this.checkBoxImages.UseVisualStyleBackColor = true;
            // 
            // ExportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 242);
            this.Controls.Add(this.checkBoxImages);
            this.Controls.Add(this.checkBoxAscendants);
            this.Controls.Add(this.checkBoxClassicRank);
            this.Controls.Add(this.checkBoxFrench);
            this.Controls.Add(this.checkBoxLatin);
            this.Controls.Add(this.labelInfos);
            this.Controls.Add(this.checkBoxFormatHtml);
            this.Controls.Add(this.checkBoxFormatCsv);
            this.Controls.Add(this.checkBoxFormatTxt);
            this.Controls.Add(this.labelFormat);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonBrowseFilename);
            this.Controls.Add(this.textBoxFilename);
            this.Controls.Add(this.labelFilename);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExportDialog";
            this.Text = "Export ...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFilename;
        private System.Windows.Forms.TextBox textBoxFilename;
        private System.Windows.Forms.Button buttonBrowseFilename;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelFormat;
        private System.Windows.Forms.CheckBox checkBoxFormatTxt;
        private System.Windows.Forms.CheckBox checkBoxFormatCsv;
        private System.Windows.Forms.CheckBox checkBoxFormatHtml;
        private System.Windows.Forms.Label labelInfos;
        private System.Windows.Forms.CheckBox checkBoxLatin;
        private System.Windows.Forms.CheckBox checkBoxFrench;
        private System.Windows.Forms.CheckBox checkBoxClassicRank;
        private System.Windows.Forms.CheckBox checkBoxAscendants;
        private System.Windows.Forms.CheckBox checkBoxImages;
    }
}