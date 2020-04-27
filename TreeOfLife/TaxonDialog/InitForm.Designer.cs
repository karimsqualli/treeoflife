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
            this.validateButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.treeGroupBox = new System.Windows.Forms.GroupBox();
            this.loadTreeFileButon = new System.Windows.Forms.Button();
            this.treeFileNameTextBox = new System.Windows.Forms.TextBox();
            this.emptyTreeRadioButton = new System.Windows.Forms.RadioButton();
            this.loadTreeRadioButton = new System.Windows.Forms.RadioButton();
            this.dataSettingsControl1 = new TreeOfLife.Controls.DataSettingsControl();
            this.treeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // validateButton
            // 
            this.validateButton.Location = new System.Drawing.Point(193, 418);
            this.validateButton.Name = "validateButton";
            this.validateButton.Size = new System.Drawing.Size(75, 23);
            this.validateButton.TabIndex = 6;
            this.validateButton.Text = "Valider";
            this.validateButton.UseVisualStyleBackColor = true;
            this.validateButton.Click += new System.EventHandler(this.validateButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.Location = new System.Drawing.Point(303, 418);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(75, 23);
            this.quitButton.TabIndex = 5;
            this.quitButton.Text = "Quitter";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // treeGroupBox
            // 
            this.treeGroupBox.Controls.Add(this.loadTreeFileButon);
            this.treeGroupBox.Controls.Add(this.treeFileNameTextBox);
            this.treeGroupBox.Controls.Add(this.emptyTreeRadioButton);
            this.treeGroupBox.Controls.Add(this.loadTreeRadioButton);
            this.treeGroupBox.Location = new System.Drawing.Point(12, 260);
            this.treeGroupBox.Name = "treeGroupBox";
            this.treeGroupBox.Size = new System.Drawing.Size(548, 143);
            this.treeGroupBox.TabIndex = 7;
            this.treeGroupBox.TabStop = false;
            this.treeGroupBox.Text = "Tree";
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
            // treeFileNameTextBox
            // 
            this.treeFileNameTextBox.Enabled = false;
            this.treeFileNameTextBox.Location = new System.Drawing.Point(10, 50);
            this.treeFileNameTextBox.Name = "treeFileNameTextBox";
            this.treeFileNameTextBox.Size = new System.Drawing.Size(246, 22);
            this.treeFileNameTextBox.TabIndex = 2;
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
            // dataSettingsControl1
            // 
            this.dataSettingsControl1.Location = new System.Drawing.Point(12, 13);
            this.dataSettingsControl1.Name = "dataSettingsControl1";
            this.dataSettingsControl1.Size = new System.Drawing.Size(566, 242);
            this.dataSettingsControl1.TabIndex = 8;
            // 
            // InitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 453);
            this.Controls.Add(this.dataSettingsControl1);
            this.Controls.Add(this.treeGroupBox);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.validateButton);
            this.Name = "InitForm";
            this.Text = "Initialisation treeOfLife";
            this.treeGroupBox.ResumeLayout(false);
            this.treeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button validateButton;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.GroupBox treeGroupBox;
        private System.Windows.Forms.Button loadTreeFileButon;
        private System.Windows.Forms.TextBox treeFileNameTextBox;
        private System.Windows.Forms.RadioButton emptyTreeRadioButton;
        private System.Windows.Forms.RadioButton loadTreeRadioButton;
        private Controls.DataSettingsControl dataSettingsControl1;
    }
}