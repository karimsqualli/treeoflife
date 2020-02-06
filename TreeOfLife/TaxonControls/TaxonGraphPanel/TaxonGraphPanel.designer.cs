namespace TreeOfLife
{
    partial class TaxonGraphPanel
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
            PaintDispose();
            if (_InertiaMove != null) { _InertiaMove.Dispose(); _InertiaMove = null; }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAscendantToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseTaxonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandTaxonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAdvanced = new System.Windows.Forms.ToolStripSeparator();
            this.resetViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GraphContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setAsNewRootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreRootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoRootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GraphContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse all";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.CollapseAllToolStripMenuItem_Click);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.expandAllToolStripMenuItem.Text = "Expand all";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.ExpandAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // selectAscendantToolStripMenuItem
            // 
            this.selectAscendantToolStripMenuItem.Name = "selectAscendantToolStripMenuItem";
            this.selectAscendantToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectAscendantToolStripMenuItem.Text = "Select ascendant";
            // 
            // collapseTaxonToolStripMenuItem
            // 
            this.collapseTaxonToolStripMenuItem.Name = "collapseTaxonToolStripMenuItem";
            this.collapseTaxonToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.collapseTaxonToolStripMenuItem.Text = "Collapse taxon";
            this.collapseTaxonToolStripMenuItem.Click += new System.EventHandler(this.CollapseTaxonToolStripMenuItem_Click);
            // 
            // expandTaxonToolStripMenuItem
            // 
            this.expandTaxonToolStripMenuItem.Name = "expandTaxonToolStripMenuItem";
            this.expandTaxonToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.expandTaxonToolStripMenuItem.Text = "Expand taxon";
            this.expandTaxonToolStripMenuItem.Click += new System.EventHandler(this.ExpandTaxonToolStripMenuItem_Click);
            // 
            // favoritesToolStripMenuItem
            // 
            this.favoritesToolStripMenuItem.Name = "favoritesToolStripMenuItem";
            this.favoritesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.favoritesToolStripMenuItem.Text = "Favorites";
            this.favoritesToolStripMenuItem.Click += new System.EventHandler(this.FavoritesToolStripMenuItem_Click);
            // 
            // toolStripMenuItemAdvanced
            // 
            this.toolStripMenuItemAdvanced.Name = "toolStripMenuItemAdvanced";
            this.toolStripMenuItemAdvanced.Size = new System.Drawing.Size(177, 6);
            this.toolStripMenuItemAdvanced.Visible = false;
            // 
            // resetViewToolStripMenuItem
            // 
            this.resetViewToolStripMenuItem.Name = "resetViewToolStripMenuItem";
            this.resetViewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.resetViewToolStripMenuItem.Text = "Reset View";
            this.resetViewToolStripMenuItem.Click += new System.EventHandler(this.ResetViewToolStripMenuItem_Click);
            // 
            // GraphContextMenu
            // 
            this.GraphContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAsNewRootToolStripMenuItem,
            this.restoreRootToolStripMenuItem,
            this.resetViewToolStripMenuItem,
            this.gotoRootToolStripMenuItem,
            this.gotoSelectedToolStripMenuItem,
            this.collapseAllToolStripMenuItem,
            this.expandAllToolStripMenuItem,
            this.toolStripMenuItem1,
            this.selectAscendantToolStripMenuItem,
            this.collapseTaxonToolStripMenuItem,
            this.expandTaxonToolStripMenuItem,
            this.favoritesToolStripMenuItem,
            this.toolStripMenuItemAdvanced});
            this.GraphContextMenu.Name = "GraphContextMenu";
            this.GraphContextMenu.Size = new System.Drawing.Size(181, 280);
            this.GraphContextMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.GraphContextMenu_Closed);
            this.GraphContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.GraphContextMenu_Opening);
            // 
            // setAsNewRootToolStripMenuItem
            // 
            this.setAsNewRootToolStripMenuItem.Name = "setAsNewRootToolStripMenuItem";
            this.setAsNewRootToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.setAsNewRootToolStripMenuItem.Text = "Set as new root";
            this.setAsNewRootToolStripMenuItem.Click += new System.EventHandler(this.SetAsNewRootToolStripMenuItem_Click);
            // 
            // restoreRootToolStripMenuItem
            // 
            this.restoreRootToolStripMenuItem.Name = "restoreRootToolStripMenuItem";
            this.restoreRootToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.restoreRootToolStripMenuItem.Text = "Restore root";
            this.restoreRootToolStripMenuItem.Click += new System.EventHandler(this.RestoreRootToolStripMenuItem_Click);
            // 
            // gotoRootToolStripMenuItem
            // 
            this.gotoRootToolStripMenuItem.Name = "gotoRootToolStripMenuItem";
            this.gotoRootToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gotoRootToolStripMenuItem.Text = "Goto Root";
            this.gotoRootToolStripMenuItem.Click += new System.EventHandler(this.gotoRootToolStripMenuItem_Click);
            // 
            // gotoSelectedToolStripMenuItem
            // 
            this.gotoSelectedToolStripMenuItem.Name = "gotoSelectedToolStripMenuItem";
            this.gotoSelectedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gotoSelectedToolStripMenuItem.Text = "Goto Selected";
            this.gotoSelectedToolStripMenuItem.Click += new System.EventHandler(this.gotoSelectedToolStripMenuItem_Click);
            // 
            // TaxonGraphPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(94)))), ((int)(((byte)(111)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContextMenuStrip = this.GraphContextMenu;
            this.Name = "TaxonGraphPanel";
            this.Size = new System.Drawing.Size(148, 148);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TaxonGraph_KeyUp);
            this.MouseEnter += new System.EventHandler(this.TaxonGraph_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.TaxonGraph_MouseLeave);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TaxonGraph_PreviewKeyDown);
            this.GraphContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectAscendantToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseTaxonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandTaxonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem favoritesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItemAdvanced;
        private System.Windows.Forms.ToolStripMenuItem resetViewToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip GraphContextMenu;
        private System.Windows.Forms.ToolStripMenuItem setAsNewRootToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreRootToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoRootToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoSelectedToolStripMenuItem;
    }
}
