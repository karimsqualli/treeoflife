﻿namespace TreeOfLife
{
    partial class TaxonListBoxFloating
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
            this.components = new System.ComponentModel.Container();
            this.taxonListBox = new TreeOfLife.Controls.TaxonListBox();
            this.SuspendLayout();
            // 
            // taxonListBox
            // 
            this.taxonListBox.CanBeSorted = false;
            this.taxonListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taxonListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.taxonListBox.FormattingEnabled = true;
            this.taxonListBox.Location = new System.Drawing.Point(0, 0);
            this.taxonListBox.MouseDoubleClickMode = TreeOfLife.Controls.TaxonListBox.MouseDoubleClickModeEnum.SelectTaxon;
            this.taxonListBox.Name = "taxonListBox";
            this.taxonListBox.Size = new System.Drawing.Size(339, 450);
            this.taxonListBox.TabIndex = 0;
            // 
            // TaxonListBoxFloating
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 450);
            this.ControlBox = false;
            this.Controls.Add(this.taxonListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TaxonListBoxFloating";
            this.Text = "TaxonListBoxFloating";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TaxonListBox taxonListBox;
    }
}