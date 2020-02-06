namespace TreeOfLife
{
    partial class TagBatchImportDialog
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSource = new System.Windows.Forms.TextBox();
            this.textBoxDestination = new System.Windows.Forms.TextBox();
            this.buttonBrowseSource = new System.Windows.Forms.Button();
            this.buttonBrowseDestination = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonOverwrite = new System.Windows.Forms.RadioButton();
            this.radioButtonOverwriteOlder = new System.Windows.Forms.RadioButton();
            this.radioButtonLeaveIt = new System.Windows.Forms.RadioButton();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Where are the files to import ?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(3, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Where do I copy generated taxon list ?";
            // 
            // textBoxSource
            // 
            this.textBoxSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSource.Location = new System.Drawing.Point(63, 22);
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.Size = new System.Drawing.Size(429, 20);
            this.textBoxSource.TabIndex = 2;
            // 
            // textBoxDestination
            // 
            this.textBoxDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDestination.Location = new System.Drawing.Point(63, 72);
            this.textBoxDestination.Name = "textBoxDestination";
            this.textBoxDestination.Size = new System.Drawing.Size(429, 20);
            this.textBoxDestination.TabIndex = 3;
            // 
            // buttonBrowseSource
            // 
            this.buttonBrowseSource.Image = global::TreeOfLife.Properties.Resources.open_folder;
            this.buttonBrowseSource.Location = new System.Drawing.Point(33, 20);
            this.buttonBrowseSource.Name = "buttonBrowseSource";
            this.buttonBrowseSource.Size = new System.Drawing.Size(24, 24);
            this.buttonBrowseSource.TabIndex = 4;
            this.buttonBrowseSource.UseVisualStyleBackColor = true;
            this.buttonBrowseSource.Click += new System.EventHandler(this.ButtonBrowseSource_Click);
            // 
            // buttonBrowseDestination
            // 
            this.buttonBrowseDestination.Image = global::TreeOfLife.Properties.Resources.open_folder;
            this.buttonBrowseDestination.Location = new System.Drawing.Point(33, 70);
            this.buttonBrowseDestination.Name = "buttonBrowseDestination";
            this.buttonBrowseDestination.Size = new System.Drawing.Size(24, 24);
            this.buttonBrowseDestination.TabIndex = 4;
            this.buttonBrowseDestination.UseVisualStyleBackColor = true;
            this.buttonBrowseDestination.Click += new System.EventHandler(this.ButtonBrowseDestination_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(3, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "If generated file already exists ?";
            // 
            // radioButtonOverwrite
            // 
            this.radioButtonOverwrite.AutoSize = true;
            this.radioButtonOverwrite.ForeColor = System.Drawing.Color.White;
            this.radioButtonOverwrite.Location = new System.Drawing.Point(33, 120);
            this.radioButtonOverwrite.Name = "radioButtonOverwrite";
            this.radioButtonOverwrite.Size = new System.Drawing.Size(68, 17);
            this.radioButtonOverwrite.TabIndex = 5;
            this.radioButtonOverwrite.Text = "overwrite";
            this.radioButtonOverwrite.UseVisualStyleBackColor = true;
            // 
            // radioButtonOverwriteOlder
            // 
            this.radioButtonOverwriteOlder.AutoSize = true;
            this.radioButtonOverwriteOlder.Checked = true;
            this.radioButtonOverwriteOlder.ForeColor = System.Drawing.Color.White;
            this.radioButtonOverwriteOlder.Location = new System.Drawing.Point(107, 120);
            this.radioButtonOverwriteOlder.Name = "radioButtonOverwriteOlder";
            this.radioButtonOverwriteOlder.Size = new System.Drawing.Size(124, 17);
            this.radioButtonOverwriteOlder.TabIndex = 5;
            this.radioButtonOverwriteOlder.TabStop = true;
            this.radioButtonOverwriteOlder.Text = "overwrite only if older";
            this.radioButtonOverwriteOlder.UseVisualStyleBackColor = true;
            // 
            // radioButtonLeaveIt
            // 
            this.radioButtonLeaveIt.AutoSize = true;
            this.radioButtonLeaveIt.ForeColor = System.Drawing.Color.White;
            this.radioButtonLeaveIt.Location = new System.Drawing.Point(237, 120);
            this.radioButtonLeaveIt.Name = "radioButtonLeaveIt";
            this.radioButtonLeaveIt.Size = new System.Drawing.Size(65, 17);
            this.radioButtonLeaveIt.TabIndex = 5;
            this.radioButtonLeaveIt.Text = "leave it !";
            this.radioButtonLeaveIt.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(337, 143);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.Location = new System.Drawing.Point(417, 143);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(75, 23);
            this.buttonImport.TabIndex = 6;
            this.buttonImport.Text = "Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.ButtonImport_Click);
            // 
            // TagBatchImportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(504, 176);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.radioButtonLeaveIt);
            this.Controls.Add(this.radioButtonOverwriteOlder);
            this.Controls.Add(this.radioButtonOverwrite);
            this.Controls.Add(this.buttonBrowseDestination);
            this.Controls.Add(this.buttonBrowseSource);
            this.Controls.Add(this.textBoxDestination);
            this.Controls.Add(this.textBoxSource);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(800, 215);
            this.MinimumSize = new System.Drawing.Size(300, 215);
            this.Name = "TagBatchImportDialog";
            this.Text = "Tag batch import";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TagBatchImportDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.TextBox textBoxDestination;
        private System.Windows.Forms.Button buttonBrowseSource;
        private System.Windows.Forms.Button buttonBrowseDestination;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonOverwrite;
        private System.Windows.Forms.RadioButton radioButtonOverwriteOlder;
        private System.Windows.Forms.RadioButton radioButtonLeaveIt;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonImport;
    }
}