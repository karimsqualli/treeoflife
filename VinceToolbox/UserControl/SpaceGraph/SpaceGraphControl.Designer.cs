namespace VinceToolbox.UserControl.Graph
{
    partial class SpaceGraphControl
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SpaceGraphControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SpaceGraphControl";
            this.MouseLeave += new System.EventHandler(this.SpaceGraphControl_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SpaceGraphControl_MouseMove);
            this.ResumeLayout(false);

        }

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

        
        #endregion
    }
}
