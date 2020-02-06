namespace TreeOfLife.GUI
{
    partial class FormContainer
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
            this.SuspendLayout();
            // 
            // FormContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormContainer";
            this.ShowInTaskbar = false;
            this.Text = "FormContainer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormContainer_FormClosed);
            this.ResizeBegin += new System.EventHandler(this.FormContainer_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.FormContainer_ResizeEnd);
            this.ResumeLayout(false);
        }

        #endregion

    }
}