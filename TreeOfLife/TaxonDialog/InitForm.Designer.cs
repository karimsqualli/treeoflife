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
            this.errorLabel = new System.Windows.Forms.Label();
            this.validateButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.selectDataDirectoryButton = new System.Windows.Forms.Button();
            this.dataDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.urlServerTextBox = new System.Windows.Forms.TextBox();
            this.offlineModeButton = new System.Windows.Forms.RadioButton();
            this.onlineModeButton = new System.Windows.Forms.RadioButton();
            this.treeGroupBox = new System.Windows.Forms.GroupBox();
            this.loadTreeRadioButton = new System.Windows.Forms.RadioButton();
            this.emptyTreeRadioButton = new System.Windows.Forms.RadioButton();
            this.treeFileNameTextBox = new System.Windows.Forms.TextBox();
            this.loadTreeFileButon = new System.Windows.Forms.Button();
            this.modeGroupBox.SuspendLayout();
            this.treeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // modeGroupBox
            // 
            this.modeGroupBox.Controls.Add(this.errorLabel);
            this.modeGroupBox.Controls.Add(this.selectDataDirectoryButton);
            this.modeGroupBox.Controls.Add(this.dataDirectoryTextBox);
            this.modeGroupBox.Controls.Add(this.urlServerTextBox);
            this.modeGroupBox.Controls.Add(this.offlineModeButton);
            this.modeGroupBox.Controls.Add(this.onlineModeButton);
            this.modeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.modeGroupBox.Name = "modeGroupBox";
            this.modeGroupBox.Size = new System.Drawing.Size(558, 191);
            this.modeGroupBox.TabIndex = 0;
            this.modeGroupBox.TabStop = false;
            this.modeGroupBox.Text = "Sélection du mode d\'initialisation";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.ForeColor = System.Drawing.Color.Crimson;
            this.errorLabel.Location = new System.Drawing.Point(3, 215);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(0, 17);
            this.errorLabel.TabIndex = 7;
            // 
            // validateButton
            // 
            this.validateButton.Location = new System.Drawing.Point(193, 359);
            this.validateButton.Name = "validateButton";
            this.validateButton.Size = new System.Drawing.Size(75, 23);
            this.validateButton.TabIndex = 6;
            this.validateButton.Text = "Valider";
            this.validateButton.UseVisualStyleBackColor = true;
            this.validateButton.Click += new System.EventHandler(this.validateButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.Location = new System.Drawing.Point(300, 359);
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
            this.selectDataDirectoryButton.Text = "Sélectionner le répertoire de données ...";
            this.selectDataDirectoryButton.UseVisualStyleBackColor = true;
            this.selectDataDirectoryButton.Click += new System.EventHandler(this.selectDataDirectoryButton_Click);
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
            // treeGroupBox
            // 
            this.treeGroupBox.Controls.Add(this.loadTreeFileButon);
            this.treeGroupBox.Controls.Add(this.treeFileNameTextBox);
            this.treeGroupBox.Controls.Add(this.emptyTreeRadioButton);
            this.treeGroupBox.Controls.Add(this.loadTreeRadioButton);
            this.treeGroupBox.Location = new System.Drawing.Point(12, 210);
            this.treeGroupBox.Name = "treeGroupBox";
            this.treeGroupBox.Size = new System.Drawing.Size(548, 143);
            this.treeGroupBox.TabIndex = 7;
            this.treeGroupBox.TabStop = false;
            this.treeGroupBox.Text = "Tree";
            // 
            // loadTreeRadioButton
            // 
            this.loadTreeRadioButton.AutoSize = true;
            this.loadTreeRadioButton.Checked = true;
            this.loadTreeRadioButton.Location = new System.Drawing.Point(10, 22);
            this.loadTreeRadioButton.Name = "loadTreeRadioButton";
            this.loadTreeRadioButton.Size = new System.Drawing.Size(102, 21);
            this.loadTreeRadioButton.TabIndex = 0;
            this.loadTreeRadioButton.TabStop = true;
            this.loadTreeRadioButton.Text = "Load a tree";
            this.loadTreeRadioButton.UseVisualStyleBackColor = true;
            this.loadTreeRadioButton.CheckedChanged += new System.EventHandler(this.loadTreeRadioButton_CheckedChanged);
            // 
            // emptyTreeRadioButton
            // 
            this.emptyTreeRadioButton.AutoSize = true;
            this.emptyTreeRadioButton.Location = new System.Drawing.Point(10, 88);
            this.emptyTreeRadioButton.Name = "emptyTreeRadioButton";
            this.emptyTreeRadioButton.Size = new System.Drawing.Size(97, 21);
            this.emptyTreeRadioButton.TabIndex = 1;
            this.emptyTreeRadioButton.Text = "Empty tree";
            this.emptyTreeRadioButton.UseVisualStyleBackColor = true;
            this.emptyTreeRadioButton.CheckedChanged += new System.EventHandler(this.emptyTreeRadioButton_CheckedChanged);
            // 
            // treeFileNameTextBox
            // 
            this.treeFileNameTextBox.Enabled = false;
            this.treeFileNameTextBox.Location = new System.Drawing.Point(10, 50);
            this.treeFileNameTextBox.Name = "treeFileNameTextBox";
            this.treeFileNameTextBox.Size = new System.Drawing.Size(246, 22);
            this.treeFileNameTextBox.TabIndex = 2;
            // 
            // loadTreeFileButon
            // 
            this.loadTreeFileButon.Location = new System.Drawing.Point(272, 48);
            this.loadTreeFileButon.Name = "loadTreeFileButon";
            this.loadTreeFileButon.Size = new System.Drawing.Size(276, 23);
            this.loadTreeFileButon.TabIndex = 3;
            this.loadTreeFileButon.Text = "Sélectionner";
            this.loadTreeFileButon.UseVisualStyleBackColor = true;
            this.loadTreeFileButon.Click += new System.EventHandler(this.loadTreeFileButon_Click);
            // 
            // InitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 394);
            this.Controls.Add(this.treeGroupBox);
            this.Controls.Add(this.modeGroupBox);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.validateButton);
            this.Name = "InitForm";
            this.Text = "Initialisation treeOfLife";
            this.modeGroupBox.ResumeLayout(false);
            this.modeGroupBox.PerformLayout();
            this.treeGroupBox.ResumeLayout(false);
            this.treeGroupBox.PerformLayout();
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
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.GroupBox treeGroupBox;
        private System.Windows.Forms.Button loadTreeFileButon;
        private System.Windows.Forms.TextBox treeFileNameTextBox;
        private System.Windows.Forms.RadioButton emptyTreeRadioButton;
        private System.Windows.Forms.RadioButton loadTreeRadioButton;
    }
}