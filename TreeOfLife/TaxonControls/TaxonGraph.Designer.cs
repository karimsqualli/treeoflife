namespace TreeOfLife
{
    partial class TaxonGraph
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

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            TreeOfLife.GraphOptions graphOptions2 = new TreeOfLife.GraphOptions();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.labelSepAfterTag = new TreeOfLife.GUI.LabelVerticalLine();
            this.labelSepAfterIUCN = new TreeOfLife.GUI.LabelVerticalLine();
            this.buttonTag = new System.Windows.Forms.Button();
            this.FinderTextBox = new System.Windows.Forms.TextBox();
            this.FinderIcon = new System.Windows.Forms.PictureBox();
            this.checkBoxEN = new TreeOfLife.GUI.CheckBoxNoPadding();
            this.checkBoxTag = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayUnnamed = new System.Windows.Forms.CheckBox();
            this.checkBoxVU = new TreeOfLife.GUI.CheckBoxNoPadding();
            this.checkBoxLC = new TreeOfLife.GUI.CheckBoxNoPadding();
            this.checkBoxEX = new TreeOfLife.GUI.CheckBoxNoPadding();
            this.checkBoxNE = new TreeOfLife.GUI.CheckBoxNoPadding();
            this.checkBoxDD = new TreeOfLife.GUI.CheckBoxNoPadding();
            this.checkBoxEW = new TreeOfLife.GUI.CheckBoxNoPadding();
            this.checkBoxCR = new TreeOfLife.GUI.CheckBoxNoPadding();
            this.checkBoxNT = new TreeOfLife.GUI.CheckBoxNoPadding();
            this.taxonGraph1 = new TreeOfLife.TaxonGraphPanel();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FinderIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.LightSlateGray;
            this.TopPanel.Controls.Add(this.labelSepAfterTag);
            this.TopPanel.Controls.Add(this.labelSepAfterIUCN);
            this.TopPanel.Controls.Add(this.buttonTag);
            this.TopPanel.Controls.Add(this.FinderTextBox);
            this.TopPanel.Controls.Add(this.FinderIcon);
            this.TopPanel.Controls.Add(this.checkBoxEN);
            this.TopPanel.Controls.Add(this.checkBoxTag);
            this.TopPanel.Controls.Add(this.checkBoxDisplayUnnamed);
            this.TopPanel.Controls.Add(this.checkBoxVU);
            this.TopPanel.Controls.Add(this.checkBoxLC);
            this.TopPanel.Controls.Add(this.checkBoxEX);
            this.TopPanel.Controls.Add(this.checkBoxNE);
            this.TopPanel.Controls.Add(this.checkBoxDD);
            this.TopPanel.Controls.Add(this.checkBoxEW);
            this.TopPanel.Controls.Add(this.checkBoxCR);
            this.TopPanel.Controls.Add(this.checkBoxNT);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(697, 29);
            this.TopPanel.TabIndex = 0;
            this.TopPanel.Resize += new System.EventHandler(this.TopPanelResize);
            // 
            // labelSepAfterTag
            // 
            this.labelSepAfterTag.Location = new System.Drawing.Point(514, 3);
            this.labelSepAfterTag.Name = "labelSepAfterTag";
            this.labelSepAfterTag.Size = new System.Drawing.Size(4, 22);
            this.labelSepAfterTag.TabIndex = 2;
            // 
            // labelSepAfterIUCN
            // 
            this.labelSepAfterIUCN.Location = new System.Drawing.Point(350, 3);
            this.labelSepAfterIUCN.Name = "labelSepAfterIUCN";
            this.labelSepAfterIUCN.Size = new System.Drawing.Size(4, 22);
            this.labelSepAfterIUCN.TabIndex = 2;
            // 
            // buttonTag
            // 
            this.buttonTag.FlatAppearance.BorderSize = 0;
            this.buttonTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTag.Image = global::TreeOfLife.Properties.Resources.TaxonTags;
            this.buttonTag.Location = new System.Drawing.Point(392, 6);
            this.buttonTag.Name = "buttonTag";
            this.buttonTag.Size = new System.Drawing.Size(16, 16);
            this.buttonTag.TabIndex = 2;
            this.buttonTag.UseVisualStyleBackColor = true;
            this.buttonTag.Click += new System.EventHandler(this.ButtonTag_Click);
            // 
            // FinderTextBox
            // 
            this.FinderTextBox.AcceptsReturn = true;
            this.FinderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FinderTextBox.Location = new System.Drawing.Point(542, 4);
            this.FinderTextBox.Name = "FinderTextBox";
            this.FinderTextBox.Size = new System.Drawing.Size(152, 20);
            this.FinderTextBox.TabIndex = 34;
            this.FinderTextBox.TextChanged += new System.EventHandler(this.FinderTextBox_TextChanged);
            this.FinderTextBox.Enter += new System.EventHandler(this.FinderTextBox_Enter);
            this.FinderTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FinderTextBox_KeyDown);
            this.FinderTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FinderTextBox_KeyUp);
            this.FinderTextBox.Leave += new System.EventHandler(this.FinderTextBox_Leave);
            // 
            // FinderIcon
            // 
            this.FinderIcon.Image = global::TreeOfLife.Properties.Resources.TaxonFinder;
            this.FinderIcon.Location = new System.Drawing.Point(524, 8);
            this.FinderIcon.Name = "FinderIcon";
            this.FinderIcon.Size = new System.Drawing.Size(12, 12);
            this.FinderIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FinderIcon.TabIndex = 33;
            this.FinderIcon.TabStop = false;
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
            this.checkBoxEN.Location = new System.Drawing.Point(236, 2);
            this.checkBoxEN.Name = "checkBoxEN";
            this.checkBoxEN.Size = new System.Drawing.Size(28, 24);
            this.checkBoxEN.TabIndex = 24;
            this.checkBoxEN.Text = "NE";
            this.checkBoxEN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEN.UseVisualStyleBackColor = false;
            this.checkBoxEN.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxEN.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // checkBoxTag
            // 
            this.checkBoxTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxTag.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxTag.FlatAppearance.BorderSize = 0;
            this.checkBoxTag.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(91)))), ((int)(((byte)(103)))));
            this.checkBoxTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxTag.ForeColor = System.Drawing.Color.White;
            this.checkBoxTag.Location = new System.Drawing.Point(414, 4);
            this.checkBoxTag.Name = "checkBoxTag";
            this.checkBoxTag.Size = new System.Drawing.Size(41, 20);
            this.checkBoxTag.TabIndex = 9;
            this.checkBoxTag.Text = "Tag";
            this.checkBoxTag.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxTag.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.checkBoxTag.UseVisualStyleBackColor = true;
            this.checkBoxTag.Visible = false;
            this.checkBoxTag.Click += new System.EventHandler(this.CheckBoxTag_Click);
            this.checkBoxTag.Paint += new System.Windows.Forms.PaintEventHandler(this.CheckBoxTag_Paint);
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
            this.checkBoxDisplayUnnamed.Location = new System.Drawing.Point(3, 2);
            this.checkBoxDisplayUnnamed.Name = "checkBoxDisplayUnnamed";
            this.checkBoxDisplayUnnamed.Size = new System.Drawing.Size(90, 24);
            this.checkBoxDisplayUnnamed.TabIndex = 9;
            this.checkBoxDisplayUnnamed.Text = "Unnamed";
            this.checkBoxDisplayUnnamed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxDisplayUnnamed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.checkBoxDisplayUnnamed.UseVisualStyleBackColor = true;
            this.checkBoxDisplayUnnamed.Click += new System.EventHandler(this.CheckBoxDisplayUnnamed_Click);
            this.checkBoxDisplayUnnamed.Paint += new System.Windows.Forms.PaintEventHandler(this.CheckBoxDisplayUnnamed_Paint);
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
            this.checkBoxVU.Location = new System.Drawing.Point(208, 2);
            this.checkBoxVU.Name = "checkBoxVU";
            this.checkBoxVU.Size = new System.Drawing.Size(28, 24);
            this.checkBoxVU.TabIndex = 25;
            this.checkBoxVU.Text = "NE";
            this.checkBoxVU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxVU.UseVisualStyleBackColor = false;
            this.checkBoxVU.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxVU.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
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
            this.checkBoxLC.Location = new System.Drawing.Point(152, 2);
            this.checkBoxLC.Name = "checkBoxLC";
            this.checkBoxLC.Size = new System.Drawing.Size(28, 24);
            this.checkBoxLC.TabIndex = 27;
            this.checkBoxLC.Text = "NE";
            this.checkBoxLC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxLC.UseVisualStyleBackColor = false;
            this.checkBoxLC.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxLC.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
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
            this.checkBoxEX.Location = new System.Drawing.Point(320, 2);
            this.checkBoxEX.Name = "checkBoxEX";
            this.checkBoxEX.Size = new System.Drawing.Size(28, 24);
            this.checkBoxEX.TabIndex = 26;
            this.checkBoxEX.Text = "NE";
            this.checkBoxEX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEX.UseVisualStyleBackColor = false;
            this.checkBoxEX.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxEX.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
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
            this.checkBoxNE.Location = new System.Drawing.Point(96, 2);
            this.checkBoxNE.Name = "checkBoxNE";
            this.checkBoxNE.Size = new System.Drawing.Size(28, 24);
            this.checkBoxNE.TabIndex = 32;
            this.checkBoxNE.Text = "NE";
            this.checkBoxNE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxNE.UseVisualStyleBackColor = false;
            this.checkBoxNE.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxNE.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
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
            this.checkBoxDD.Location = new System.Drawing.Point(124, 2);
            this.checkBoxDD.Name = "checkBoxDD";
            this.checkBoxDD.Size = new System.Drawing.Size(28, 24);
            this.checkBoxDD.TabIndex = 31;
            this.checkBoxDD.Text = "NE";
            this.checkBoxDD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxDD.UseVisualStyleBackColor = false;
            this.checkBoxDD.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxDD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
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
            this.checkBoxEW.Location = new System.Drawing.Point(292, 2);
            this.checkBoxEW.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxEW.Name = "checkBoxEW";
            this.checkBoxEW.Size = new System.Drawing.Size(28, 24);
            this.checkBoxEW.TabIndex = 28;
            this.checkBoxEW.Text = "EW";
            this.checkBoxEW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEW.UseVisualStyleBackColor = false;
            this.checkBoxEW.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxEW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
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
            this.checkBoxCR.Location = new System.Drawing.Point(264, 2);
            this.checkBoxCR.Name = "checkBoxCR";
            this.checkBoxCR.Size = new System.Drawing.Size(28, 24);
            this.checkBoxCR.TabIndex = 30;
            this.checkBoxCR.Text = "NE";
            this.checkBoxCR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCR.UseVisualStyleBackColor = false;
            this.checkBoxCR.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxCR.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
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
            this.checkBoxNT.Location = new System.Drawing.Point(180, 2);
            this.checkBoxNT.Name = "checkBoxNT";
            this.checkBoxNT.Size = new System.Drawing.Size(28, 24);
            this.checkBoxNT.TabIndex = 29;
            this.checkBoxNT.Text = "NE";
            this.checkBoxNT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxNT.UseVisualStyleBackColor = false;
            this.checkBoxNT.Click += new System.EventHandler(this.CheckBoxRedListCategoryFilter_Click);
            this.checkBoxNT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckBoxRedListCategoryFilter_MouseUp);
            // 
            // taxonGraph1
            // 
            this.taxonGraph1.BackColor = System.Drawing.Color.LightSlateGray;
            this.taxonGraph1.BelowMouse = null;
            this.taxonGraph1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.taxonGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taxonGraph1.Location = new System.Drawing.Point(0, 29);
            this.taxonGraph1.Name = "taxonGraph1";
            graphOptions2.ColumnInter = 30;
            graphOptions2.DisplayBackTransparentInLineMode = false;
            graphOptions2.DisplayClassikRankRule = false;
            graphOptions2.DisplayMode = TreeOfLife.GraphOptions.DisplayModeEnum.Lines;
            graphOptions2.GrayUnselectedInBoxMode = false;
            graphOptions2.KeepsImageAtNamesRight = false;
            graphOptions2.LineSoftRatio = 0.6F;
            graphOptions2.TaxonHeight = 30;
            graphOptions2.TaxonWidth = 100;
            graphOptions2.Zoom = 1F;
            graphOptions2.ZoomedColumnWidth = 30;
            graphOptions2.ZoomedHeight = 30;
            graphOptions2.ZoomedWidth = 100;
            this.taxonGraph1.Options = graphOptions2;
            this.taxonGraph1.OwnerContainer = null;
            this.taxonGraph1.PaintRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.taxonGraph1.Root = null;
            this.taxonGraph1.Selected = null;
            this.taxonGraph1.Size = new System.Drawing.Size(697, 337);
            this.taxonGraph1.TabIndex = 1;
            this.taxonGraph1.UsedColors = null;
            // 
            // TaxonGraph
            // 
            this.Controls.Add(this.taxonGraph1);
            this.Controls.Add(this.TopPanel);
            this.Name = "TaxonGraph";
            this.Size = new System.Drawing.Size(697, 366);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FinderIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TopPanel;
        private TaxonGraphPanel taxonGraph1;
        private System.Windows.Forms.CheckBox checkBoxDisplayUnnamed;
        private GUI.CheckBoxNoPadding checkBoxEN;
        private GUI.CheckBoxNoPadding checkBoxVU;
        private GUI.CheckBoxNoPadding checkBoxLC;
        private GUI.CheckBoxNoPadding checkBoxEX;
        private GUI.CheckBoxNoPadding checkBoxNE;
        private GUI.CheckBoxNoPadding checkBoxDD;
        private GUI.CheckBoxNoPadding checkBoxEW;
        private GUI.CheckBoxNoPadding checkBoxCR;
        private GUI.CheckBoxNoPadding checkBoxNT;
        private System.Windows.Forms.PictureBox FinderIcon;
        private System.Windows.Forms.TextBox FinderTextBox;
        private System.Windows.Forms.CheckBox checkBoxTag;
        private System.Windows.Forms.Button buttonTag;
        private GUI.LabelVerticalLine labelSepAfterIUCN;
        private GUI.LabelVerticalLine labelSepAfterTag;
    }
}
