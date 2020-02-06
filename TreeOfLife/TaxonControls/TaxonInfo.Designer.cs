namespace TreeOfLife
{
    partial class TaxonInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaxonInfo));
            this.TaxonBigVignette = new TreeOfLife.Controls.TaxonImageControl();
            this._MultiImages = new TreeOfLife.Controls.TaxonMultiImageSoundControl();
            this.DescendantCount = new System.Windows.Forms.Label();
            this.Comments = new TreeOfLife.Controls.TaxonMultiCommentControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.checkBoxList = new System.Windows.Forms.CheckBox();
            this.checkBoxRandom = new System.Windows.Forms.CheckBox();
            this.splitContainerSpecificCharacter = new System.Windows.Forms.SplitContainer();
            this.buttonExpand = new System.Windows.Forms.Button();
            this.buttonCollapse = new System.Windows.Forms.Button();
            this.SpecificCharactersTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSpecificCharacter)).BeginInit();
            this.splitContainerSpecificCharacter.Panel1.SuspendLayout();
            this.splitContainerSpecificCharacter.Panel2.SuspendLayout();
            this.splitContainerSpecificCharacter.SuspendLayout();
            this.SuspendLayout();
            // 
            // TaxonBigVignette
            // 
            this.TaxonBigVignette.AllowContextualMenu = true;
            this.TaxonBigVignette.AllowNavigationButtons = true;
            this.TaxonBigVignette.AllowSecondaryImages = true;
            this.TaxonBigVignette.AllowSound = true;
            this.TaxonBigVignette.AllowTips = true;
            this.TaxonBigVignette.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TaxonBigVignette.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(127)))), ((int)(((byte)(149)))));
            this.TaxonBigVignette.CurrentImage = ((System.Drawing.Image)(resources.GetObject("TaxonBigVignette.CurrentImage")));
            this.TaxonBigVignette.CurrentTaxon = null;
            this.TaxonBigVignette.DisplayFrench = true;
            this.TaxonBigVignette.DisplayImage = false;
            this.TaxonBigVignette.DisplayLatin = true;
            this.TaxonBigVignette.DisplayMode = TreeOfLife.Controls.VignetteDisplayParams.ModeEnum.Brut;
            this.TaxonBigVignette.FrenchVisible = true;
            this.TaxonBigVignette.ImageVisible = true;
            this.TaxonBigVignette.LatinVisible = true;
            this.TaxonBigVignette.ListImages = null;
            this.TaxonBigVignette.Location = new System.Drawing.Point(38, 0);
            this.TaxonBigVignette.Name = "TaxonBigVignette";
            this.TaxonBigVignette.Size = new System.Drawing.Size(474, 70);
            this.TaxonBigVignette.TabIndex = 0;
            // 
            // _MultiImages
            // 
            this._MultiImages.AllowTips = true;
            this._MultiImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._MultiImages.BackColor = System.Drawing.Color.Transparent;
            this._MultiImages.ImageNumber = 0;
            this._MultiImages.ListTaxons = null;
            this._MultiImages.ListTaxonsFamily = null;
            this._MultiImages.Location = new System.Drawing.Point(3, 67);
            this._MultiImages.Name = "_MultiImages";
            this._MultiImages.Size = new System.Drawing.Size(512, 442);
            this._MultiImages.TabIndex = 1;
            this._MultiImages.OnDoubleClickImage += new TreeOfLife.Controls.TaxonMultiImageSoundControl.OnClickImageEventHandler(this._MultiImages_DoubleClick);
            this._MultiImages.OnEnterImage += new System.EventHandler(this._MultiImages_OnEnterImage);
            this._MultiImages.OnLeaveImage += new System.EventHandler(this._MultiImages_OnLeaveImage);
            // 
            // DescendantCount
            // 
            this.DescendantCount.BackColor = System.Drawing.Color.LightSlateGray;
            this.DescendantCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DescendantCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DescendantCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DescendantCount.ForeColor = System.Drawing.Color.White;
            this.DescendantCount.Location = new System.Drawing.Point(0, 485);
            this.DescendantCount.Name = "DescendantCount";
            this.DescendantCount.Size = new System.Drawing.Size(512, 25);
            this.DescendantCount.TabIndex = 12;
            this.DescendantCount.Tag = "xxx";
            this.DescendantCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Comments
            // 
            this.Comments.AddFranceMap = true;
            this.Comments.AddTitleForEmpty = true;
            this.Comments.AddTitleForFirstEmpty = true;
            this.Comments.AlternativeTaxon = null;
            this.Comments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Comments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(195)))), ((int)(((byte)(213)))));
            this.Comments.HasCollapsibleDivs = false;
            this.Comments.Location = new System.Drawing.Point(0, 21);
            this.Comments.MainTaxon = null;
            this.Comments.Name = "Comments";
            this.Comments.Recursive = true;
            this.Comments.Size = new System.Drawing.Size(512, 484);
            this.Comments.TabIndex = 13;
            this.Comments.Tag = "xxx";
            this.Comments.OnDocumentCompleted += new System.EventHandler(this.Comments_OnDocumentCompleted);
            this.Comments.OnHasCollapsibleDivsChanged += new System.EventHandler(this.Comments_OnHasCollapsibleDivsChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(127)))), ((int)(((byte)(149)))));
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxList);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxRandom);
            this.splitContainer1.Panel1.Controls.Add(this.TaxonBigVignette);
            this.splitContainer1.Panel1.Controls.Add(this._MultiImages);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainerSpecificCharacter);
            this.splitContainer1.Panel2.Controls.Add(this.DescendantCount);
            this.splitContainer1.Panel2.Controls.Add(this.Comments);
            this.splitContainer1.Size = new System.Drawing.Size(512, 1024);
            this.splitContainer1.SplitterDistance = 512;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 15;
            this.splitContainer1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Paint);
            // 
            // checkBoxList
            // 
            this.checkBoxList.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxList.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxList.BackgroundImage = global::TreeOfLife.Properties.Resources.List_Mode;
            this.checkBoxList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxList.FlatAppearance.BorderSize = 0;
            this.checkBoxList.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(101)))), ((int)(((byte)(113)))));
            this.checkBoxList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.checkBoxList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.checkBoxList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxList.Location = new System.Drawing.Point(7, 7);
            this.checkBoxList.Name = "checkBoxList";
            this.checkBoxList.Size = new System.Drawing.Size(32, 32);
            this.checkBoxList.TabIndex = 3;
            this.checkBoxList.UseVisualStyleBackColor = false;
            this.checkBoxList.Click += new System.EventHandler(this.CheckBoxList_Click);
            // 
            // checkBoxRandom
            // 
            this.checkBoxRandom.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRandom.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxRandom.BackgroundImage = global::TreeOfLife.Properties.Resources.Random_mode;
            this.checkBoxRandom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxRandom.FlatAppearance.BorderSize = 0;
            this.checkBoxRandom.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(101)))), ((int)(((byte)(113)))));
            this.checkBoxRandom.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.checkBoxRandom.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.checkBoxRandom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxRandom.Location = new System.Drawing.Point(7, 39);
            this.checkBoxRandom.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxRandom.Name = "checkBoxRandom";
            this.checkBoxRandom.Size = new System.Drawing.Size(32, 32);
            this.checkBoxRandom.TabIndex = 3;
            this.checkBoxRandom.UseVisualStyleBackColor = false;
            this.checkBoxRandom.Click += new System.EventHandler(this.CheckBoxRandom_Click);
            // 
            // splitContainerSpecificCharacter
            // 
            this.splitContainerSpecificCharacter.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerSpecificCharacter.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainerSpecificCharacter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerSpecificCharacter.IsSplitterFixed = true;
            this.splitContainerSpecificCharacter.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSpecificCharacter.Name = "splitContainerSpecificCharacter";
            // 
            // splitContainerSpecificCharacter.Panel1
            // 
            this.splitContainerSpecificCharacter.Panel1.Controls.Add(this.buttonExpand);
            this.splitContainerSpecificCharacter.Panel1.Controls.Add(this.buttonCollapse);
            // 
            // splitContainerSpecificCharacter.Panel2
            // 
            this.splitContainerSpecificCharacter.Panel2.Controls.Add(this.SpecificCharactersTitle);
            this.splitContainerSpecificCharacter.Size = new System.Drawing.Size(512, 28);
            this.splitContainerSpecificCharacter.SplitterDistance = 57;
            this.splitContainerSpecificCharacter.SplitterWidth = 1;
            this.splitContainerSpecificCharacter.TabIndex = 0;
            // 
            // buttonExpand
            // 
            this.buttonExpand.BackColor = System.Drawing.Color.Transparent;
            this.buttonExpand.BackgroundImage = global::TreeOfLife.Properties.Resources.Expand_comments;
            this.buttonExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonExpand.FlatAppearance.BorderSize = 0;
            this.buttonExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExpand.Location = new System.Drawing.Point(32, 3);
            this.buttonExpand.Name = "buttonExpand";
            this.buttonExpand.Size = new System.Drawing.Size(22, 22);
            this.buttonExpand.TabIndex = 0;
            this.buttonExpand.UseVisualStyleBackColor = false;
            this.buttonExpand.Click += new System.EventHandler(this.ButtonExpand_Click);
            // 
            // buttonCollapse
            // 
            this.buttonCollapse.BackgroundImage = global::TreeOfLife.Properties.Resources.Collapse_comments;
            this.buttonCollapse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonCollapse.FlatAppearance.BorderSize = 0;
            this.buttonCollapse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCollapse.Location = new System.Drawing.Point(3, 3);
            this.buttonCollapse.Name = "buttonCollapse";
            this.buttonCollapse.Size = new System.Drawing.Size(22, 22);
            this.buttonCollapse.TabIndex = 0;
            this.buttonCollapse.UseVisualStyleBackColor = true;
            this.buttonCollapse.Click += new System.EventHandler(this.ButtonCollapse_Click);
            // 
            // SpecificCharactersTitle
            // 
            this.SpecificCharactersTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(170)))), ((int)(((byte)(193)))));
            this.SpecificCharactersTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpecificCharactersTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpecificCharactersTitle.Location = new System.Drawing.Point(0, 0);
            this.SpecificCharactersTitle.Name = "SpecificCharactersTitle";
            this.SpecificCharactersTitle.Size = new System.Drawing.Size(454, 28);
            this.SpecificCharactersTitle.TabIndex = 14;
            this.SpecificCharactersTitle.Text = "Derived characters";
            this.SpecificCharactersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TaxonInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "TaxonInfo";
            this.Size = new System.Drawing.Size(512, 1024);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainerSpecificCharacter.Panel1.ResumeLayout(false);
            this.splitContainerSpecificCharacter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSpecificCharacter)).EndInit();
            this.splitContainerSpecificCharacter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        private Controls.TaxonImageControl TaxonBigVignette;
        private Controls.TaxonMultiImageSoundControl _MultiImages;
        private System.Windows.Forms.Label DescendantCount;
        private TreeOfLife.Controls.TaxonMultiCommentControl Comments;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainerSpecificCharacter;
        private System.Windows.Forms.CheckBox checkBoxList;
        private System.Windows.Forms.CheckBox checkBoxRandom;
        private System.Windows.Forms.Label SpecificCharactersTitle;
        private System.Windows.Forms.Button buttonExpand;
        private System.Windows.Forms.Button buttonCollapse;
    }
}
