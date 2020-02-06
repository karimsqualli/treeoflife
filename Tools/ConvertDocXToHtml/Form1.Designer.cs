namespace ConvertDocXToHtml
{
    partial class FormConvertDocX2Html
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxBrowseSource = new System.Windows.Forms.TextBox();
            this.textBoxBrowseTarget = new System.Windows.Forms.TextBox();
            this.buttonBrouseSource = new System.Windows.Forms.Button();
            this.buttonBrowseTarget = new System.Windows.Forms.Button();
            this.listBoxResult = new System.Windows.Forms.ListBox();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.radioButtonSourceFolder = new System.Windows.Forms.RadioButton();
            this.radioButtonSourceFile = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sources DocX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Target Html";
            // 
            // textBoxBrowseSource
            // 
            this.textBoxBrowseSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBrowseSource.Location = new System.Drawing.Point(20, 20);
            this.textBoxBrowseSource.Name = "textBoxBrowseSource";
            this.textBoxBrowseSource.ReadOnly = true;
            this.textBoxBrowseSource.Size = new System.Drawing.Size(262, 20);
            this.textBoxBrowseSource.TabIndex = 2;
            // 
            // textBoxBrowseTarget
            // 
            this.textBoxBrowseTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBrowseTarget.Location = new System.Drawing.Point(20, 70);
            this.textBoxBrowseTarget.Name = "textBoxBrowseTarget";
            this.textBoxBrowseTarget.ReadOnly = true;
            this.textBoxBrowseTarget.Size = new System.Drawing.Size(262, 20);
            this.textBoxBrowseTarget.TabIndex = 3;
            // 
            // buttonBrouseSource
            // 
            this.buttonBrouseSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrouseSource.Location = new System.Drawing.Point(288, 18);
            this.buttonBrouseSource.Name = "buttonBrouseSource";
            this.buttonBrouseSource.Size = new System.Drawing.Size(25, 23);
            this.buttonBrouseSource.TabIndex = 4;
            this.buttonBrouseSource.Text = "...";
            this.buttonBrouseSource.UseVisualStyleBackColor = true;
            this.buttonBrouseSource.Click += new System.EventHandler(this.buttonBrouseSource_Click);
            // 
            // buttonBrowseTarget
            // 
            this.buttonBrowseTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseTarget.Location = new System.Drawing.Point(288, 68);
            this.buttonBrowseTarget.Name = "buttonBrowseTarget";
            this.buttonBrowseTarget.Size = new System.Drawing.Size(25, 23);
            this.buttonBrowseTarget.TabIndex = 5;
            this.buttonBrowseTarget.Text = "...";
            this.buttonBrowseTarget.UseVisualStyleBackColor = true;
            this.buttonBrowseTarget.Click += new System.EventHandler(this.buttonBrowseTarget_Click);
            // 
            // listBoxResult
            // 
            this.listBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxResult.FormattingEnabled = true;
            this.listBoxResult.Location = new System.Drawing.Point(6, 134);
            this.listBoxResult.Name = "listBoxResult";
            this.listBoxResult.Size = new System.Drawing.Size(307, 121);
            this.listBoxResult.TabIndex = 6;
            // 
            // buttonConvert
            // 
            this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvert.Location = new System.Drawing.Point(6, 105);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(307, 23);
            this.buttonConvert.TabIndex = 7;
            this.buttonConvert.Text = "<<< Convert >>>";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // radioButtonSourceFolder
            // 
            this.radioButtonSourceFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonSourceFolder.AutoSize = true;
            this.radioButtonSourceFolder.Location = new System.Drawing.Point(259, 1);
            this.radioButtonSourceFolder.Name = "radioButtonSourceFolder";
            this.radioButtonSourceFolder.Size = new System.Drawing.Size(54, 17);
            this.radioButtonSourceFolder.TabIndex = 8;
            this.radioButtonSourceFolder.TabStop = true;
            this.radioButtonSourceFolder.Text = "Folder";
            this.radioButtonSourceFolder.UseVisualStyleBackColor = true;
            this.radioButtonSourceFolder.CheckedChanged += new System.EventHandler(this.radioButtonSourceFolder_CheckedChanged);
            // 
            // radioButtonSourceFile
            // 
            this.radioButtonSourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonSourceFile.AutoSize = true;
            this.radioButtonSourceFile.Location = new System.Drawing.Point(212, 1);
            this.radioButtonSourceFile.Name = "radioButtonSourceFile";
            this.radioButtonSourceFile.Size = new System.Drawing.Size(41, 17);
            this.radioButtonSourceFile.TabIndex = 8;
            this.radioButtonSourceFile.TabStop = true;
            this.radioButtonSourceFile.Text = "File";
            this.radioButtonSourceFile.UseVisualStyleBackColor = true;
            this.radioButtonSourceFile.CheckedChanged += new System.EventHandler(this.radioButtonSourceFile_CheckedChanged);
            // 
            // FormConvertDocX2Html
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 262);
            this.Controls.Add(this.radioButtonSourceFile);
            this.Controls.Add(this.radioButtonSourceFolder);
            this.Controls.Add(this.buttonConvert);
            this.Controls.Add(this.listBoxResult);
            this.Controls.Add(this.buttonBrowseTarget);
            this.Controls.Add(this.buttonBrouseSource);
            this.Controls.Add(this.textBoxBrowseTarget);
            this.Controls.Add(this.textBoxBrowseSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormConvertDocX2Html";
            this.Text = "Convert DocX => Html";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConvertDocX2Html_FormClosing);
            this.Load += new System.EventHandler(this.FormConvertDocX2Html_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxBrowseSource;
        private System.Windows.Forms.TextBox textBoxBrowseTarget;
        private System.Windows.Forms.Button buttonBrouseSource;
        private System.Windows.Forms.Button buttonBrowseTarget;
        private System.Windows.Forms.ListBox listBoxResult;
        private System.Windows.Forms.Button buttonConvert;
        private System.Windows.Forms.RadioButton radioButtonSourceFolder;
        private System.Windows.Forms.RadioButton radioButtonSourceFile;
    }
}

