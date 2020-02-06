namespace TreeOfLife
{
    partial class TaxonTags
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
            this.components = new System.ComponentModel.Container();
            this.taxonListBox = new TreeOfLife.Controls.TaxonListBox();
            this.TagsMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSelectedTaxonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAllSpeciesInSelectedTaxonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.autoaddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxDisplayUnnamed = new System.Windows.Forms.CheckBox();
            this.labelTag = new System.Windows.Forms.Label();
            this.buttonSaveCurrentFilter = new System.Windows.Forms.Button();
            this.checkBoxEN = new System.Windows.Forms.CheckBox();
            this.checkBoxVU = new System.Windows.Forms.CheckBox();
            this.checkBoxEX = new System.Windows.Forms.CheckBox();
            this.checkBoxLC = new System.Windows.Forms.CheckBox();
            this.checkBoxEW = new System.Windows.Forms.CheckBox();
            this.checkBoxNT = new System.Windows.Forms.CheckBox();
            this.checkBoxCR = new System.Windows.Forms.CheckBox();
            this.checkBoxDD = new System.Windows.Forms.CheckBox();
            this.checkBoxNE = new System.Windows.Forms.CheckBox();
            this.TagsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // taxonListBox
            // 
            this.taxonListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taxonListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.taxonListBox.CanBeSorted = true;
            this.taxonListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.taxonListBox.FormattingEnabled = true;
            this.taxonListBox.IntegralHeight = false;
            this.taxonListBox.Location = new System.Drawing.Point(3, 50);
            this.taxonListBox.MouseDoubleClickMode = TreeOfLife.Controls.TaxonListBox.MouseDoubleClickModeEnum.SelectTaxon;
            this.taxonListBox.Name = "taxonListBox";
            this.taxonListBox.Size = new System.Drawing.Size(297, 320);
            this.taxonListBox.TabIndex = 0;
            // 
            // TagsMenu
            // 
            this.TagsMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagsMenu.AutoSize = false;
            this.TagsMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.TagsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.filterToolStripMenuItem});
            this.TagsMenu.Location = new System.Drawing.Point(1, 0);
            this.TagsMenu.Name = "TagsMenu";
            this.TagsMenu.Size = new System.Drawing.Size(299, 24);
            this.TagsMenu.TabIndex = 4;
            this.TagsMenu.Text = "TagsMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.batchImportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.FileToolStripMenuItem_DropDownOpening);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.saveAsToolStripMenuItem.Text = "Save as ...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.importToolStripMenuItem.Text = "Import ...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.ImportToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.exportToolStripMenuItem.Text = "Export ...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.ExportToolStripMenuItem_Click);
            // 
            // batchImportToolStripMenuItem
            // 
            this.batchImportToolStripMenuItem.Name = "batchImportToolStripMenuItem";
            this.batchImportToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.batchImportToolStripMenuItem.Text = "Batch import ...";
            this.batchImportToolStripMenuItem.Click += new System.EventHandler(this.BatchImportToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSelectedTaxonToolStripMenuItem,
            this.addAllSpeciesInSelectedTaxonToolStripMenuItem,
            this.toolStripMenuItem3,
            this.autoaddToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.DropDownOpening += new System.EventHandler(this.EditToolStripMenuItem_DropDownOpening);
            // 
            // addSelectedTaxonToolStripMenuItem
            // 
            this.addSelectedTaxonToolStripMenuItem.Name = "addSelectedTaxonToolStripMenuItem";
            this.addSelectedTaxonToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.addSelectedTaxonToolStripMenuItem.Text = "Add selected taxon";
            this.addSelectedTaxonToolStripMenuItem.Click += new System.EventHandler(this.AddSelectedTaxonToolStripMenuItem_Click);
            // 
            // addAllSpeciesInSelectedTaxonToolStripMenuItem
            // 
            this.addAllSpeciesInSelectedTaxonToolStripMenuItem.Name = "addAllSpeciesInSelectedTaxonToolStripMenuItem";
            this.addAllSpeciesInSelectedTaxonToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.addAllSpeciesInSelectedTaxonToolStripMenuItem.Text = "Add all species in selected taxon";
            this.addAllSpeciesInSelectedTaxonToolStripMenuItem.Click += new System.EventHandler(this.AddAllSpeciesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(240, 6);
            // 
            // autoaddToolStripMenuItem
            // 
            this.autoaddToolStripMenuItem.Name = "autoaddToolStripMenuItem";
            this.autoaddToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.autoaddToolStripMenuItem.Text = "Auto-add taxon on selection";
            this.autoaddToolStripMenuItem.Click += new System.EventHandler(this.AutoaddToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.ClearToolStripMenuItem_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onToolStripMenuItem,
            this.offToolStripMenuItem});
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.filterToolStripMenuItem.Text = "Filter";
            this.filterToolStripMenuItem.DropDownOpening += new System.EventHandler(this.FilterToolStripMenuItem_DropDownOpening);
            // 
            // onToolStripMenuItem
            // 
            this.onToolStripMenuItem.Name = "onToolStripMenuItem";
            this.onToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.onToolStripMenuItem.Text = "On";
            this.onToolStripMenuItem.Click += new System.EventHandler(this.OnToolStripMenuItem_Click);
            // 
            // offToolStripMenuItem
            // 
            this.offToolStripMenuItem.Name = "offToolStripMenuItem";
            this.offToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.offToolStripMenuItem.Text = "Off";
            this.offToolStripMenuItem.Click += new System.EventHandler(this.OffToolStripMenuItem_Click);
            // 
            // checkBoxDisplayUnnamed
            // 
            this.checkBoxDisplayUnnamed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxDisplayUnnamed.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxDisplayUnnamed.FlatAppearance.BorderSize = 0;
            this.checkBoxDisplayUnnamed.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(91)))), ((int)(((byte)(103)))));
            this.checkBoxDisplayUnnamed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxDisplayUnnamed.ForeColor = System.Drawing.Color.White;
            this.checkBoxDisplayUnnamed.Image = global::TreeOfLife.Properties.Resources.filter_unnamed;
            this.checkBoxDisplayUnnamed.Location = new System.Drawing.Point(4, 316);
            this.checkBoxDisplayUnnamed.Name = "checkBoxDisplayUnnamed";
            this.checkBoxDisplayUnnamed.Size = new System.Drawing.Size(90, 24);
            this.checkBoxDisplayUnnamed.TabIndex = 8;
            this.checkBoxDisplayUnnamed.Text = "Unnamed";
            this.checkBoxDisplayUnnamed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxDisplayUnnamed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.checkBoxDisplayUnnamed.UseVisualStyleBackColor = true;
            this.checkBoxDisplayUnnamed.Visible = false;
            this.checkBoxDisplayUnnamed.Click += new System.EventHandler(this.CheckBoxDisplayUnnamed_Click);
            this.checkBoxDisplayUnnamed.Paint += new System.Windows.Forms.PaintEventHandler(this.CheckBoxDisplayUnnamed_Paint);
            // 
            // labelTag
            // 
            this.labelTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTag.BackColor = System.Drawing.SystemColors.Window;
            this.labelTag.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTag.Location = new System.Drawing.Point(3, 25);
            this.labelTag.Name = "labelTag";
            this.labelTag.Size = new System.Drawing.Size(272, 20);
            this.labelTag.TabIndex = 9;
            this.labelTag.Text = "label1";
            this.labelTag.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSaveCurrentFilter
            // 
            this.buttonSaveCurrentFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveCurrentFilter.BackgroundImage = global::TreeOfLife.Properties.Resources.save_12x12_white;
            this.buttonSaveCurrentFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSaveCurrentFilter.FlatAppearance.BorderSize = 0;
            this.buttonSaveCurrentFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveCurrentFilter.Location = new System.Drawing.Point(281, 28);
            this.buttonSaveCurrentFilter.Name = "buttonSaveCurrentFilter";
            this.buttonSaveCurrentFilter.Size = new System.Drawing.Size(14, 14);
            this.buttonSaveCurrentFilter.TabIndex = 5;
            this.buttonSaveCurrentFilter.UseVisualStyleBackColor = true;
            this.buttonSaveCurrentFilter.Click += new System.EventHandler(this.ButtonSaveCurrentFilter_Click);
            // 
            // checkBoxEN
            // 
            this.checkBoxEN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxEN.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEN.Checked = true;
            this.checkBoxEN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEN.FlatAppearance.BorderSize = 0;
            this.checkBoxEN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEN.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEN.Location = new System.Drawing.Point(164, 346);
            this.checkBoxEN.Name = "checkBoxEN";
            this.checkBoxEN.Size = new System.Drawing.Size(32, 24);
            this.checkBoxEN.TabIndex = 15;
            this.checkBoxEN.Text = "NE";
            this.checkBoxEN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEN.UseVisualStyleBackColor = false;
            this.checkBoxEN.Visible = false;
            this.checkBoxEN.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxEN.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // checkBoxVU
            // 
            this.checkBoxVU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxVU.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxVU.Checked = true;
            this.checkBoxVU.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxVU.FlatAppearance.BorderSize = 0;
            this.checkBoxVU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxVU.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxVU.Location = new System.Drawing.Point(132, 346);
            this.checkBoxVU.Name = "checkBoxVU";
            this.checkBoxVU.Size = new System.Drawing.Size(32, 24);
            this.checkBoxVU.TabIndex = 16;
            this.checkBoxVU.Text = "NE";
            this.checkBoxVU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxVU.UseVisualStyleBackColor = false;
            this.checkBoxVU.Visible = false;
            this.checkBoxVU.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxVU.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // checkBoxEX
            // 
            this.checkBoxEX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxEX.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEX.Checked = true;
            this.checkBoxEX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEX.FlatAppearance.BorderSize = 0;
            this.checkBoxEX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEX.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEX.Location = new System.Drawing.Point(260, 346);
            this.checkBoxEX.Name = "checkBoxEX";
            this.checkBoxEX.Size = new System.Drawing.Size(32, 24);
            this.checkBoxEX.TabIndex = 17;
            this.checkBoxEX.Text = "NE";
            this.checkBoxEX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEX.UseVisualStyleBackColor = false;
            this.checkBoxEX.Visible = false;
            this.checkBoxEX.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxEX.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // checkBoxLC
            // 
            this.checkBoxLC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxLC.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxLC.Checked = true;
            this.checkBoxLC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLC.FlatAppearance.BorderSize = 0;
            this.checkBoxLC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxLC.Location = new System.Drawing.Point(68, 346);
            this.checkBoxLC.Name = "checkBoxLC";
            this.checkBoxLC.Size = new System.Drawing.Size(32, 24);
            this.checkBoxLC.TabIndex = 18;
            this.checkBoxLC.Text = "NE";
            this.checkBoxLC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxLC.UseVisualStyleBackColor = false;
            this.checkBoxLC.Visible = false;
            this.checkBoxLC.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxLC.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // checkBoxEW
            // 
            this.checkBoxEW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxEW.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEW.Checked = true;
            this.checkBoxEW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEW.FlatAppearance.BorderSize = 0;
            this.checkBoxEW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEW.Location = new System.Drawing.Point(228, 346);
            this.checkBoxEW.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxEW.Name = "checkBoxEW";
            this.checkBoxEW.Size = new System.Drawing.Size(32, 24);
            this.checkBoxEW.TabIndex = 19;
            this.checkBoxEW.Text = "EW";
            this.checkBoxEW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEW.UseVisualStyleBackColor = false;
            this.checkBoxEW.Visible = false;
            this.checkBoxEW.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxEW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // checkBoxNT
            // 
            this.checkBoxNT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxNT.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxNT.Checked = true;
            this.checkBoxNT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNT.FlatAppearance.BorderSize = 0;
            this.checkBoxNT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxNT.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxNT.Location = new System.Drawing.Point(100, 346);
            this.checkBoxNT.Name = "checkBoxNT";
            this.checkBoxNT.Size = new System.Drawing.Size(32, 24);
            this.checkBoxNT.TabIndex = 20;
            this.checkBoxNT.Text = "NE";
            this.checkBoxNT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxNT.UseVisualStyleBackColor = false;
            this.checkBoxNT.Visible = false;
            this.checkBoxNT.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxNT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // checkBoxCR
            // 
            this.checkBoxCR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxCR.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCR.Checked = true;
            this.checkBoxCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCR.FlatAppearance.BorderSize = 0;
            this.checkBoxCR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCR.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCR.Location = new System.Drawing.Point(196, 346);
            this.checkBoxCR.Name = "checkBoxCR";
            this.checkBoxCR.Size = new System.Drawing.Size(32, 24);
            this.checkBoxCR.TabIndex = 21;
            this.checkBoxCR.Text = "NE";
            this.checkBoxCR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCR.UseVisualStyleBackColor = false;
            this.checkBoxCR.Visible = false;
            this.checkBoxCR.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxCR.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // checkBoxDD
            // 
            this.checkBoxDD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxDD.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxDD.Checked = true;
            this.checkBoxDD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDD.FlatAppearance.BorderSize = 0;
            this.checkBoxDD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxDD.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxDD.Location = new System.Drawing.Point(36, 346);
            this.checkBoxDD.Name = "checkBoxDD";
            this.checkBoxDD.Size = new System.Drawing.Size(32, 24);
            this.checkBoxDD.TabIndex = 22;
            this.checkBoxDD.Text = "NE";
            this.checkBoxDD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxDD.UseVisualStyleBackColor = false;
            this.checkBoxDD.Visible = false;
            this.checkBoxDD.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxDD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // checkBoxNE
            // 
            this.checkBoxNE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxNE.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxNE.Checked = true;
            this.checkBoxNE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNE.FlatAppearance.BorderSize = 0;
            this.checkBoxNE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxNE.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxNE.Location = new System.Drawing.Point(4, 346);
            this.checkBoxNE.Name = "checkBoxNE";
            this.checkBoxNE.Size = new System.Drawing.Size(32, 24);
            this.checkBoxNE.TabIndex = 23;
            this.checkBoxNE.Text = "NE";
            this.checkBoxNE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxNE.UseVisualStyleBackColor = false;
            this.checkBoxNE.Visible = false;
            this.checkBoxNE.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxNE.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // TaxonTags
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxEN);
            this.Controls.Add(this.checkBoxVU);
            this.Controls.Add(this.checkBoxEX);
            this.Controls.Add(this.checkBoxLC);
            this.Controls.Add(this.checkBoxEW);
            this.Controls.Add(this.checkBoxNT);
            this.Controls.Add(this.checkBoxCR);
            this.Controls.Add(this.checkBoxDD);
            this.Controls.Add(this.checkBoxNE);
            this.Controls.Add(this.labelTag);
            this.Controls.Add(this.checkBoxDisplayUnnamed);
            this.Controls.Add(this.buttonSaveCurrentFilter);
            this.Controls.Add(this.TagsMenu);
            this.Controls.Add(this.taxonListBox);
            this.MinimumSize = new System.Drawing.Size(100, 300);
            this.Name = "TaxonTags";
            this.Size = new System.Drawing.Size(300, 374);
            this.TagsMenu.ResumeLayout(false);
            this.TagsMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.MenuStrip TagsMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private Controls.TaxonListBox taxonListBox;
        private System.Windows.Forms.ToolStripMenuItem addSelectedTaxonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoaddToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addAllSpeciesInSelectedTaxonToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.CheckBox checkBoxDisplayUnnamed;
        private System.Windows.Forms.Label labelTag;
        private System.Windows.Forms.Button buttonSaveCurrentFilter;
        private System.Windows.Forms.CheckBox checkBoxEN;
        private System.Windows.Forms.CheckBox checkBoxVU;
        private System.Windows.Forms.CheckBox checkBoxEX;
        private System.Windows.Forms.CheckBox checkBoxLC;
        private System.Windows.Forms.CheckBox checkBoxEW;
        private System.Windows.Forms.CheckBox checkBoxNT;
        private System.Windows.Forms.CheckBox checkBoxCR;
        private System.Windows.Forms.CheckBox checkBoxDD;
        private System.Windows.Forms.CheckBox checkBoxNE;
        private System.Windows.Forms.ToolStripMenuItem batchImportToolStripMenuItem;
    }
}
