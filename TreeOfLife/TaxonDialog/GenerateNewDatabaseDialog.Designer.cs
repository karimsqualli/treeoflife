namespace TreeOfLife.TaxonDialog
{
    partial class GenerateNewDatabaseDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelTaxon = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelFolder = new System.Windows.Forms.Label();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxExportComments = new System.Windows.Forms.CheckBox();
            this.checkBoxExportPhotos = new System.Windows.Forms.CheckBox();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.checkBoxExportSounds = new System.Windows.Forms.CheckBox();
            this.checkBoxExe = new System.Windows.Forms.CheckBox();
            this.checkBoxTaxonNameAsSubFolder = new System.Windows.Forms.CheckBox();
            this.checkBoxExportAscendants = new System.Windows.Forms.CheckBox();
            this.buttonClean = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Taxon";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTaxon
            // 
            this.labelTaxon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTaxon.BackColor = System.Drawing.SystemColors.Window;
            this.labelTaxon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTaxon.Location = new System.Drawing.Point(50, 5);
            this.labelTaxon.Name = "labelTaxon";
            this.labelTaxon.Size = new System.Drawing.Size(324, 22);
            this.labelTaxon.TabIndex = 1;
            this.labelTaxon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Folder";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFolder
            // 
            this.labelFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFolder.BackColor = System.Drawing.SystemColors.Window;
            this.labelFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelFolder.Location = new System.Drawing.Point(50, 36);
            this.labelFolder.Name = "labelFolder";
            this.labelFolder.Size = new System.Drawing.Size(297, 23);
            this.labelFolder.TabIndex = 3;
            this.labelFolder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.Location = new System.Drawing.Point(346, 36);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(28, 23);
            this.buttonBrowse.TabIndex = 4;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Options";
            // 
            // checkBoxExportComments
            // 
            this.checkBoxExportComments.AutoSize = true;
            this.checkBoxExportComments.Checked = true;
            this.checkBoxExportComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExportComments.Location = new System.Drawing.Point(50, 133);
            this.checkBoxExportComments.Name = "checkBoxExportComments";
            this.checkBoxExportComments.Size = new System.Drawing.Size(107, 17);
            this.checkBoxExportComments.TabIndex = 6;
            this.checkBoxExportComments.Text = "Export comments";
            this.checkBoxExportComments.UseVisualStyleBackColor = true;
            // 
            // checkBoxExportPhotos
            // 
            this.checkBoxExportPhotos.AutoSize = true;
            this.checkBoxExportPhotos.Checked = true;
            this.checkBoxExportPhotos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExportPhotos.Location = new System.Drawing.Point(50, 153);
            this.checkBoxExportPhotos.Name = "checkBoxExportPhotos";
            this.checkBoxExportPhotos.Size = new System.Drawing.Size(91, 17);
            this.checkBoxExportPhotos.TabIndex = 7;
            this.checkBoxExportPhotos.Text = "Export photos";
            this.checkBoxExportPhotos.UseVisualStyleBackColor = true;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerate.Location = new System.Drawing.Point(8, 208);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(366, 23);
            this.buttonGenerate.TabIndex = 9;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // checkBoxExportSounds
            // 
            this.checkBoxExportSounds.AutoSize = true;
            this.checkBoxExportSounds.Checked = true;
            this.checkBoxExportSounds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExportSounds.Location = new System.Drawing.Point(50, 173);
            this.checkBoxExportSounds.Name = "checkBoxExportSounds";
            this.checkBoxExportSounds.Size = new System.Drawing.Size(93, 17);
            this.checkBoxExportSounds.TabIndex = 7;
            this.checkBoxExportSounds.Text = "Export sounds";
            this.checkBoxExportSounds.UseVisualStyleBackColor = true;
            // 
            // checkBoxExe
            // 
            this.checkBoxExe.AutoSize = true;
            this.checkBoxExe.Checked = true;
            this.checkBoxExe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExe.Location = new System.Drawing.Point(215, 113);
            this.checkBoxExe.Name = "checkBoxExe";
            this.checkBoxExe.Size = new System.Drawing.Size(111, 17);
            this.checkBoxExe.TabIndex = 6;
            this.checkBoxExe.Text = "Copy exe && config";
            this.checkBoxExe.UseVisualStyleBackColor = true;
            // 
            // checkBoxTaxonNameAsSubFolder
            // 
            this.checkBoxTaxonNameAsSubFolder.AutoSize = true;
            this.checkBoxTaxonNameAsSubFolder.Location = new System.Drawing.Point(50, 63);
            this.checkBoxTaxonNameAsSubFolder.Name = "checkBoxTaxonNameAsSubFolder";
            this.checkBoxTaxonNameAsSubFolder.Size = new System.Drawing.Size(163, 17);
            this.checkBoxTaxonNameAsSubFolder.TabIndex = 10;
            this.checkBoxTaxonNameAsSubFolder.Text = "Add taxon name as subfolder";
            this.checkBoxTaxonNameAsSubFolder.UseVisualStyleBackColor = true;
            // 
            // checkBoxExportAscendants
            // 
            this.checkBoxExportAscendants.AutoSize = true;
            this.checkBoxExportAscendants.Location = new System.Drawing.Point(50, 113);
            this.checkBoxExportAscendants.Name = "checkBoxExportAscendants";
            this.checkBoxExportAscendants.Size = new System.Drawing.Size(127, 17);
            this.checkBoxExportAscendants.TabIndex = 11;
            this.checkBoxExportAscendants.Text = "Export all ascendants";
            this.checkBoxExportAscendants.UseVisualStyleBackColor = true;
            // 
            // buttonClean
            // 
            this.buttonClean.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClean.Location = new System.Drawing.Point(299, 59);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Size = new System.Drawing.Size(75, 23);
            this.buttonClean.TabIndex = 12;
            this.buttonClean.Text = "Clean folder";
            this.buttonClean.UseVisualStyleBackColor = true;
            this.buttonClean.Click += new System.EventHandler(this.buttonClean_Click);
            // 
            // GenerateNewDatabaseDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 231);
            this.Controls.Add(this.buttonClean);
            this.Controls.Add(this.checkBoxExportAscendants);
            this.Controls.Add(this.checkBoxTaxonNameAsSubFolder);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.checkBoxExportSounds);
            this.Controls.Add(this.checkBoxExportPhotos);
            this.Controls.Add(this.checkBoxExe);
            this.Controls.Add(this.checkBoxExportComments);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.labelFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelTaxon);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(800, 270);
            this.MinimumSize = new System.Drawing.Size(300, 270);
            this.Name = "GenerateNewDatabaseDialog";
            this.Text = "Generate new database";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GenerateNewDatabaseDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTaxon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelFolder;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxExportComments;
        private System.Windows.Forms.CheckBox checkBoxExportPhotos;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.CheckBox checkBoxExportSounds;
        private System.Windows.Forms.CheckBox checkBoxExe;
        private System.Windows.Forms.CheckBox checkBoxTaxonNameAsSubFolder;
        private System.Windows.Forms.CheckBox checkBoxExportAscendants;
        private System.Windows.Forms.Button buttonClean;
    }
}