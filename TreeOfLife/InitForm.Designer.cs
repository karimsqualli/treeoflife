namespace TreeOfLife
{
    partial class InitForm
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
            this.modeGroupBox = new System.Windows.Forms.GroupBox();
            this.validateButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.selectDataDirectoryButton = new System.Windows.Forms.Button();
            this.dataDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.urlServerTextBox = new System.Windows.Forms.TextBox();
            this.offlineModeButton = new System.Windows.Forms.RadioButton();
            this.onlineModeButton = new System.Windows.Forms.RadioButton();
            this.modeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // modeGroupBox
            // 
            this.modeGroupBox.Controls.Add(this.validateButton);
            this.modeGroupBox.Controls.Add(this.quitButton);
            this.modeGroupBox.Controls.Add(this.selectDataDirectoryButton);
            this.modeGroupBox.Controls.Add(this.dataDirectoryTextBox);
            this.modeGroupBox.Controls.Add(this.urlServerTextBox);
            this.modeGroupBox.Controls.Add(this.offlineModeButton);
            this.modeGroupBox.Controls.Add(this.onlineModeButton);
            this.modeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.modeGroupBox.Name = "modeGroupBox";
            this.modeGroupBox.Size = new System.Drawing.Size(776, 426);
            this.modeGroupBox.TabIndex = 0;
            this.modeGroupBox.TabStop = false;
            this.modeGroupBox.Text = "Sélection du mode d\'initialisation";
            // 
            // validateButton
            // 
            this.validateButton.Location = new System.Drawing.Point(181, 263);
            this.validateButton.Name = "validateButton";
            this.validateButton.Size = new System.Drawing.Size(75, 23);
            this.validateButton.TabIndex = 6;
            this.validateButton.Text = "Valider";
            this.validateButton.UseVisualStyleBackColor = true;
            this.validateButton.Click += new System.EventHandler(this.validateButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.Location = new System.Drawing.Point(272, 263);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(75, 23);
            this.quitButton.TabIndex = 5;
            this.quitButton.Text = "Quitter";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // selectDataDirectoryButton
            // 
            this.selectDataDirectoryButton.Location = new System.Drawing.Point(272, 152);
            this.selectDataDirectoryButton.Name = "selectDataDirectoryButton";
            this.selectDataDirectoryButton.Size = new System.Drawing.Size(276, 23);
            this.selectDataDirectoryButton.TabIndex = 4;
            this.selectDataDirectoryButton.Text = "Sélectionner le répertoire de données";
            this.selectDataDirectoryButton.UseVisualStyleBackColor = true;
            // 
            // dataDirectoryTextBox
            // 
            this.dataDirectoryTextBox.Location = new System.Drawing.Point(6, 153);
            this.dataDirectoryTextBox.Name = "dataDirectoryTextBox";
            this.dataDirectoryTextBox.Size = new System.Drawing.Size(250, 22);
            this.dataDirectoryTextBox.TabIndex = 3;
            // 
            // urlServerTextBox
            // 
            this.urlServerTextBox.Location = new System.Drawing.Point(6, 72);
            this.urlServerTextBox.Name = "urlServerTextBox";
            this.urlServerTextBox.Size = new System.Drawing.Size(250, 22);
            this.urlServerTextBox.TabIndex = 2;
            this.urlServerTextBox.Text = "http://localhost:8888/";
            // 
            // offlineModeButton
            // 
            this.offlineModeButton.AutoSize = true;
            this.offlineModeButton.Location = new System.Drawing.Point(0, 113);
            this.offlineModeButton.Name = "offlineModeButton";
            this.offlineModeButton.Size = new System.Drawing.Size(130, 21);
            this.offlineModeButton.TabIndex = 1;
            this.offlineModeButton.TabStop = true;
            this.offlineModeButton.Text = "Mode hors ligne";
            this.offlineModeButton.UseVisualStyleBackColor = true;
            this.offlineModeButton.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // onlineModeButton
            // 
            this.onlineModeButton.AutoSize = true;
            this.onlineModeButton.Checked = true;
            this.onlineModeButton.Location = new System.Drawing.Point(6, 31);
            this.onlineModeButton.Name = "onlineModeButton";
            this.onlineModeButton.Size = new System.Drawing.Size(118, 21);
            this.onlineModeButton.TabIndex = 0;
            this.onlineModeButton.TabStop = true;
            this.onlineModeButton.Text = "Mode en ligne";
            this.onlineModeButton.UseVisualStyleBackColor = true;
            this.onlineModeButton.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // InitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 353);
            this.Controls.Add(this.modeGroupBox);
            this.Name = "InitForm";
            this.Text = "Initialisation treeOfLife";
            this.modeGroupBox.ResumeLayout(false);
            this.modeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox modeGroupBox;
        private System.Windows.Forms.Button selectDataDirectoryButton;
        private System.Windows.Forms.TextBox dataDirectoryTextBox;
        private System.Windows.Forms.TextBox urlServerTextBox;
        private System.Windows.Forms.RadioButton offlineModeButton;
        private System.Windows.Forms.RadioButton onlineModeButton;
        private System.Windows.Forms.Button validateButton;
        private System.Windows.Forms.Button quitButton;
    }
}