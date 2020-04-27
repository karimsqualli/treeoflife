namespace TreeOfLife.TaxonDialog
{
    partial class SettingsForm
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
            this.dataSettingsControl1 = new TreeOfLife.Controls.DataSettingsControl();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dataSettingsControl1
            // 
            this.dataSettingsControl1.Location = new System.Drawing.Point(4, 4);
            this.dataSettingsControl1.Name = "dataSettingsControl1";
            this.dataSettingsControl1.Size = new System.Drawing.Size(566, 242);
            this.dataSettingsControl1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(237, 255);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Valider";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 290);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataSettingsControl1);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DataSettingsControl dataSettingsControl1;
        private System.Windows.Forms.Button button1;
    }
}