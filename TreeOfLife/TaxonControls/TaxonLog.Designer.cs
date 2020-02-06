namespace TreeOfLife
{
    partial class TaxonLog
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.logListBox1 = new TreeOfLife.LogListBox();
            this.SuspendLayout();
            // 
            // logListBox1
            // 
            this.logListBox1.BackColor = System.Drawing.Color.DimGray;
            this.logListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.logListBox1.FormattingEnabled = true;
            this.logListBox1.Location = new System.Drawing.Point(0, 0);
            this.logListBox1.Name = "logListBox1";
            this.logListBox1.Size = new System.Drawing.Size(150, 150);
            this.logListBox1.TabIndex = 0;
            // 
            // TaxonLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.logListBox1);
            this.Name = "TaxonLog";
            this.ResumeLayout(false);

        }

        #endregion

        private LogListBox logListBox1;

    }
}
