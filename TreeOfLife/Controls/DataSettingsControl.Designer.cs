namespace TreeOfLife.Controls
{
    partial class DataSettingsControl
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.modeGroupBox = new System.Windows.Forms.GroupBox();
            this.errorLabel = new System.Windows.Forms.Label();
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
            this.modeGroupBox.Controls.Add(this.errorLabel);
            this.modeGroupBox.Controls.Add(this.selectDataDirectoryButton);
            this.modeGroupBox.Controls.Add(this.dataDirectoryTextBox);
            this.modeGroupBox.Controls.Add(this.urlServerTextBox);
            this.modeGroupBox.Controls.Add(this.offlineModeButton);
            this.modeGroupBox.Controls.Add(this.onlineModeButton);
            this.modeGroupBox.Location = new System.Drawing.Point(0, 0);
            this.modeGroupBox.Name = "modeGroupBox";
            this.modeGroupBox.Size = new System.Drawing.Size(558, 239);
            this.modeGroupBox.TabIndex = 1;
            this.modeGroupBox.TabStop = false;
            this.modeGroupBox.Text = "Sélection du mode d\'initialisation des données";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.ForeColor = System.Drawing.Color.Crimson;
            this.errorLabel.Location = new System.Drawing.Point(6, 198);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(0, 17);
            this.errorLabel.TabIndex = 7;
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
            this.offlineModeButton.CheckedChanged += new System.EventHandler(this.offlineModeButton_CheckedChanged);
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
            this.onlineModeButton.CheckedChanged += new System.EventHandler(this.onlineModeButton_CheckedChanged);
            // 
            // DataSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.modeGroupBox);
            this.Name = "DataSettingsControl";
            this.Size = new System.Drawing.Size(566, 242);
            this.modeGroupBox.ResumeLayout(false);
            this.modeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox modeGroupBox;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Button selectDataDirectoryButton;
        private System.Windows.Forms.TextBox dataDirectoryTextBox;
        private System.Windows.Forms.TextBox urlServerTextBox;
        private System.Windows.Forms.RadioButton offlineModeButton;
        private System.Windows.Forms.RadioButton onlineModeButton;
    }
}
