namespace TreeOfLife
{
    partial class TaxonEditInfo
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
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonRefSyn = new System.Windows.Forms.Button();
            this.labelFrench = new System.Windows.Forms.Label();
            this.textBoxFrenchName = new System.Windows.Forms.TextBox();
            this.buttonFrenchSyn = new System.Windows.Forms.Button();
            this.labelClassikRank = new System.Windows.Forms.Label();
            this.textBoxClassicRank = new System.Windows.Forms.TextBox();
            this.splitContainerData = new System.Windows.Forms.SplitContainer();
            this.taxonListImage1 = new TreeOfLife.Controls.TaxonListImage();
            this.splitContainerComments = new System.Windows.Forms.SplitContainer();
            this.Comments = new TreeOfLife.Controls.TaxonMultiCommentControl();
            this.buttonCreateComment = new System.Windows.Forms.Button();
            this.SpecificCharactersTitle = new System.Windows.Forms.Label();
            this.buttonClassicRank = new System.Windows.Forms.Button();
            this.buttonWikiCommons = new System.Windows.Forms.Button();
            this.checkBoxLock = new System.Windows.Forms.CheckBox();
            this.buttonOTT = new System.Windows.Forms.Button();
            this.buttonGoogle = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelFlags = new System.Windows.Forms.Label();
            this.checkBoxFlagExtinctInherited = new System.Windows.Forms.CheckBox();
            this.checkBoxFlagUseID = new System.Windows.Forms.CheckBox();
            this.labelRedListCategory = new System.Windows.Forms.Label();
            this.checkBoxNE = new System.Windows.Forms.CheckBox();
            this.checkBoxDD = new System.Windows.Forms.CheckBox();
            this.checkBoxLC = new System.Windows.Forms.CheckBox();
            this.checkBoxNT = new System.Windows.Forms.CheckBox();
            this.checkBoxVU = new System.Windows.Forms.CheckBox();
            this.checkBoxEN = new System.Windows.Forms.CheckBox();
            this.checkBoxCR = new System.Windows.Forms.CheckBox();
            this.checkBoxEW = new System.Windows.Forms.CheckBox();
            this.checkBoxEX = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).BeginInit();
            this.splitContainerData.Panel1.SuspendLayout();
            this.splitContainerData.Panel2.SuspendLayout();
            this.splitContainerData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerComments)).BeginInit();
            this.splitContainerComments.Panel1.SuspendLayout();
            this.splitContainerComments.Panel2.SuspendLayout();
            this.splitContainerComments.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(45, 46);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(86, 43);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(319, 20);
            this.textBoxName.TabIndex = 3;
            this.textBoxName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxName_KeyDown);
            this.textBoxName.Leave += new System.EventHandler(this.TextBoxName_Leave);
            // 
            // buttonRefSyn
            // 
            this.buttonRefSyn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefSyn.Location = new System.Drawing.Point(411, 43);
            this.buttonRefSyn.Name = "buttonRefSyn";
            this.buttonRefSyn.Size = new System.Drawing.Size(33, 20);
            this.buttonRefSyn.TabIndex = 10;
            this.buttonRefSyn.Text = "Syn";
            this.buttonRefSyn.UseVisualStyleBackColor = true;
            this.buttonRefSyn.Click += new System.EventHandler(this.ButtonRefSyn_Click);
            // 
            // labelFrench
            // 
            this.labelFrench.AutoSize = true;
            this.labelFrench.Location = new System.Drawing.Point(40, 71);
            this.labelFrench.Name = "labelFrench";
            this.labelFrench.Size = new System.Drawing.Size(40, 13);
            this.labelFrench.TabIndex = 2;
            this.labelFrench.Text = "French";
            // 
            // textBoxFrenchName
            // 
            this.textBoxFrenchName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFrenchName.Location = new System.Drawing.Point(86, 68);
            this.textBoxFrenchName.Name = "textBoxFrenchName";
            this.textBoxFrenchName.Size = new System.Drawing.Size(319, 20);
            this.textBoxFrenchName.TabIndex = 5;
            this.textBoxFrenchName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxFrenchName_KeyDown);
            this.textBoxFrenchName.Leave += new System.EventHandler(this.TextBoxFrenchName_Leave);
            // 
            // buttonFrenchSyn
            // 
            this.buttonFrenchSyn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFrenchSyn.Location = new System.Drawing.Point(411, 68);
            this.buttonFrenchSyn.Name = "buttonFrenchSyn";
            this.buttonFrenchSyn.Size = new System.Drawing.Size(33, 20);
            this.buttonFrenchSyn.TabIndex = 10;
            this.buttonFrenchSyn.Text = "Syn";
            this.buttonFrenchSyn.UseVisualStyleBackColor = true;
            this.buttonFrenchSyn.Click += new System.EventHandler(this.ButtonFrenchSyn_Click);
            // 
            // labelClassikRank
            // 
            this.labelClassikRank.AutoSize = true;
            this.labelClassikRank.Location = new System.Drawing.Point(16, 96);
            this.labelClassikRank.Name = "labelClassikRank";
            this.labelClassikRank.Size = new System.Drawing.Size(64, 13);
            this.labelClassikRank.TabIndex = 2;
            this.labelClassikRank.Text = "Classic rank";
            // 
            // textBoxClassicRank
            // 
            this.textBoxClassicRank.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClassicRank.Location = new System.Drawing.Point(86, 93);
            this.textBoxClassicRank.Name = "textBoxClassicRank";
            this.textBoxClassicRank.ReadOnly = true;
            this.textBoxClassicRank.Size = new System.Drawing.Size(319, 20);
            this.textBoxClassicRank.TabIndex = 5;
            // 
            // splitContainerData
            // 
            this.splitContainerData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerData.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerData.Location = new System.Drawing.Point(3, 166);
            this.splitContainerData.Name = "splitContainerData";
            this.splitContainerData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerData.Panel1
            // 
            this.splitContainerData.Panel1.Controls.Add(this.taxonListImage1);
            // 
            // splitContainerData.Panel2
            // 
            this.splitContainerData.Panel2.Controls.Add(this.splitContainerComments);
            this.splitContainerData.Panel2.Controls.Add(this.SpecificCharactersTitle);
            this.splitContainerData.Size = new System.Drawing.Size(441, 406);
            this.splitContainerData.SplitterDistance = 252;
            this.splitContainerData.TabIndex = 6;
            // 
            // taxonListImage1
            // 
            this.taxonListImage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taxonListImage1.Location = new System.Drawing.Point(0, 0);
            this.taxonListImage1.Name = "taxonListImage1";
            this.taxonListImage1.Size = new System.Drawing.Size(441, 252);
            this.taxonListImage1.TabIndex = 11;
            this.taxonListImage1.Taxon = null;
            // 
            // splitContainerComments
            // 
            this.splitContainerComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerComments.Location = new System.Drawing.Point(0, 24);
            this.splitContainerComments.Name = "splitContainerComments";
            this.splitContainerComments.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerComments.Panel1
            // 
            this.splitContainerComments.Panel1.Controls.Add(this.Comments);
            // 
            // splitContainerComments.Panel2
            // 
            this.splitContainerComments.Panel2.Controls.Add(this.buttonCreateComment);
            this.splitContainerComments.Size = new System.Drawing.Size(438, 126);
            this.splitContainerComments.SplitterDistance = 74;
            this.splitContainerComments.TabIndex = 17;
            // 
            // Comments
            // 
            this.Comments.AddFranceMap = true;
            this.Comments.AddTitleForEmpty = false;
            this.Comments.AddTitleForFirstEmpty = false;
            this.Comments.AlternativeTaxon = null;
            this.Comments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(195)))), ((int)(((byte)(213)))));
            this.Comments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Comments.HasCollapsibleDivs = false;
            this.Comments.Location = new System.Drawing.Point(0, 0);
            this.Comments.MainTaxon = null;
            this.Comments.Name = "Comments";
            this.Comments.Recursive = false;
            this.Comments.Size = new System.Drawing.Size(438, 74);
            this.Comments.TabIndex = 15;
            this.Comments.Tag = "xxx";
            this.Comments.OnDocumentCompleted += new System.EventHandler(this.Comments_OnDocumentCompleted);
            // 
            // buttonCreateComment
            // 
            this.buttonCreateComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateComment.Location = new System.Drawing.Point(16, 12);
            this.buttonCreateComment.Name = "buttonCreateComment";
            this.buttonCreateComment.Size = new System.Drawing.Size(407, 23);
            this.buttonCreateComment.TabIndex = 0;
            this.buttonCreateComment.Text = "Create comments";
            this.buttonCreateComment.UseVisualStyleBackColor = true;
            this.buttonCreateComment.Click += new System.EventHandler(this.ButtonCreateComment_Click);
            // 
            // SpecificCharactersTitle
            // 
            this.SpecificCharactersTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpecificCharactersTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(170)))), ((int)(((byte)(193)))));
            this.SpecificCharactersTitle.Location = new System.Drawing.Point(0, 0);
            this.SpecificCharactersTitle.Name = "SpecificCharactersTitle";
            this.SpecificCharactersTitle.Size = new System.Drawing.Size(441, 21);
            this.SpecificCharactersTitle.TabIndex = 16;
            this.SpecificCharactersTitle.Text = "Derived characters";
            this.SpecificCharactersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonClassicRank
            // 
            this.buttonClassicRank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClassicRank.Location = new System.Drawing.Point(411, 93);
            this.buttonClassicRank.Name = "buttonClassicRank";
            this.buttonClassicRank.Size = new System.Drawing.Size(33, 20);
            this.buttonClassicRank.TabIndex = 10;
            this.buttonClassicRank.Text = "...";
            this.buttonClassicRank.UseVisualStyleBackColor = true;
            this.buttonClassicRank.Click += new System.EventHandler(this.ButtonClassicRank_Click);
            // 
            // buttonWikiCommons
            // 
            this.buttonWikiCommons.Image = global::TreeOfLife.Properties.Resources.WikimediaCommons_32x32;
            this.buttonWikiCommons.Location = new System.Drawing.Point(155, 3);
            this.buttonWikiCommons.Name = "buttonWikiCommons";
            this.buttonWikiCommons.Size = new System.Drawing.Size(32, 32);
            this.buttonWikiCommons.TabIndex = 11;
            this.buttonWikiCommons.UseVisualStyleBackColor = true;
            this.buttonWikiCommons.Click += new System.EventHandler(this.ButtonWikiCommons_Click);
            // 
            // checkBoxLock
            // 
            this.checkBoxLock.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxLock.Image = global::TreeOfLife.Properties.Resources.unlocked;
            this.checkBoxLock.Location = new System.Drawing.Point(41, 3);
            this.checkBoxLock.Name = "checkBoxLock";
            this.checkBoxLock.Size = new System.Drawing.Size(32, 32);
            this.checkBoxLock.TabIndex = 9;
            this.checkBoxLock.UseVisualStyleBackColor = true;
            // 
            // buttonOTT
            // 
            this.buttonOTT.Image = global::TreeOfLife.Properties.Resources.ott;
            this.buttonOTT.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOTT.Location = new System.Drawing.Point(193, 3);
            this.buttonOTT.Name = "buttonOTT";
            this.buttonOTT.Size = new System.Drawing.Size(120, 32);
            this.buttonOTT.TabIndex = 8;
            this.buttonOTT.UseVisualStyleBackColor = true;
            this.buttonOTT.Click += new System.EventHandler(this.ButtonOTT_Click);
            // 
            // buttonGoogle
            // 
            this.buttonGoogle.Image = global::TreeOfLife.Properties.Resources.google;
            this.buttonGoogle.Location = new System.Drawing.Point(117, 3);
            this.buttonGoogle.Name = "buttonGoogle";
            this.buttonGoogle.Size = new System.Drawing.Size(32, 32);
            this.buttonGoogle.TabIndex = 8;
            this.buttonGoogle.UseVisualStyleBackColor = true;
            this.buttonGoogle.Click += new System.EventHandler(this.ButtonGoogle_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.AutoSize = true;
            this.buttonReset.Image = global::TreeOfLife.Properties.Resources.Undo;
            this.buttonReset.Location = new System.Drawing.Point(79, 3);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(32, 32);
            this.buttonReset.TabIndex = 8;
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Image = global::TreeOfLife.Properties.Resources.save;
            this.buttonSave.Location = new System.Drawing.Point(3, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(32, 32);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // labelFlags
            // 
            this.labelFlags.AutoSize = true;
            this.labelFlags.Location = new System.Drawing.Point(48, 121);
            this.labelFlags.Name = "labelFlags";
            this.labelFlags.Size = new System.Drawing.Size(32, 13);
            this.labelFlags.TabIndex = 12;
            this.labelFlags.Text = "Flags";
            // 
            // checkBoxFlagExtinctInherited
            // 
            this.checkBoxFlagExtinctInherited.AutoSize = true;
            this.checkBoxFlagExtinctInherited.Location = new System.Drawing.Point(86, 121);
            this.checkBoxFlagExtinctInherited.Name = "checkBoxFlagExtinctInherited";
            this.checkBoxFlagExtinctInherited.Size = new System.Drawing.Size(130, 17);
            this.checkBoxFlagExtinctInherited.TabIndex = 13;
            this.checkBoxFlagExtinctInherited.Text = "Subdivision for Extinct";
            this.checkBoxFlagExtinctInherited.UseVisualStyleBackColor = true;
            this.checkBoxFlagExtinctInherited.CheckedChanged += new System.EventHandler(this.CheckBoxFlagExtinctInherited_CheckedChanged);
            // 
            // checkBoxFlagUseID
            // 
            this.checkBoxFlagUseID.AutoSize = true;
            this.checkBoxFlagUseID.Location = new System.Drawing.Point(319, 12);
            this.checkBoxFlagUseID.Name = "checkBoxFlagUseID";
            this.checkBoxFlagUseID.Size = new System.Drawing.Size(112, 17);
            this.checkBoxFlagUseID.TabIndex = 13;
            this.checkBoxFlagUseID.Text = "Use ID in filename";
            this.checkBoxFlagUseID.UseVisualStyleBackColor = true;
            this.checkBoxFlagUseID.CheckedChanged += new System.EventHandler(this.CheckBoxFlagUseID_CheckedChanged);
            // 
            // labelRedListCategory
            // 
            this.labelRedListCategory.AutoSize = true;
            this.labelRedListCategory.Location = new System.Drawing.Point(5, 143);
            this.labelRedListCategory.Name = "labelRedListCategory";
            this.labelRedListCategory.Size = new System.Drawing.Size(75, 13);
            this.labelRedListCategory.TabIndex = 12;
            this.labelRedListCategory.Text = "Red list categ.";
            // 
            // checkBoxNE
            // 
            this.checkBoxNE.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxNE.Checked = true;
            this.checkBoxNE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNE.FlatAppearance.BorderSize = 0;
            this.checkBoxNE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxNE.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxNE.Location = new System.Drawing.Point(86, 138);
            this.checkBoxNE.Name = "checkBoxNE";
            this.checkBoxNE.Size = new System.Drawing.Size(32, 24);
            this.checkBoxNE.TabIndex = 14;
            this.checkBoxNE.Text = "NE";
            this.checkBoxNE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxNE.UseVisualStyleBackColor = false;
            this.checkBoxNE.Click += new System.EventHandler(this.RedListCategoryClick);
            // 
            // checkBoxDD
            // 
            this.checkBoxDD.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxDD.Checked = true;
            this.checkBoxDD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDD.FlatAppearance.BorderSize = 0;
            this.checkBoxDD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxDD.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxDD.Location = new System.Drawing.Point(118, 138);
            this.checkBoxDD.Name = "checkBoxDD";
            this.checkBoxDD.Size = new System.Drawing.Size(32, 24);
            this.checkBoxDD.TabIndex = 14;
            this.checkBoxDD.Text = "NE";
            this.checkBoxDD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxDD.UseVisualStyleBackColor = false;
            this.checkBoxDD.Click += new System.EventHandler(this.RedListCategoryClick);
            // 
            // checkBoxLC
            // 
            this.checkBoxLC.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxLC.Checked = true;
            this.checkBoxLC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLC.FlatAppearance.BorderSize = 0;
            this.checkBoxLC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxLC.Location = new System.Drawing.Point(150, 138);
            this.checkBoxLC.Name = "checkBoxLC";
            this.checkBoxLC.Size = new System.Drawing.Size(32, 24);
            this.checkBoxLC.TabIndex = 14;
            this.checkBoxLC.Text = "NE";
            this.checkBoxLC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxLC.UseVisualStyleBackColor = false;
            this.checkBoxLC.Click += new System.EventHandler(this.RedListCategoryClick);
            // 
            // checkBoxNT
            // 
            this.checkBoxNT.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxNT.Checked = true;
            this.checkBoxNT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNT.FlatAppearance.BorderSize = 0;
            this.checkBoxNT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxNT.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxNT.Location = new System.Drawing.Point(182, 138);
            this.checkBoxNT.Name = "checkBoxNT";
            this.checkBoxNT.Size = new System.Drawing.Size(32, 24);
            this.checkBoxNT.TabIndex = 14;
            this.checkBoxNT.Text = "NE";
            this.checkBoxNT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxNT.UseVisualStyleBackColor = false;
            this.checkBoxNT.Click += new System.EventHandler(this.RedListCategoryClick);
            // 
            // checkBoxVU
            // 
            this.checkBoxVU.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxVU.Checked = true;
            this.checkBoxVU.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxVU.FlatAppearance.BorderSize = 0;
            this.checkBoxVU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxVU.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxVU.Location = new System.Drawing.Point(214, 138);
            this.checkBoxVU.Name = "checkBoxVU";
            this.checkBoxVU.Size = new System.Drawing.Size(32, 24);
            this.checkBoxVU.TabIndex = 14;
            this.checkBoxVU.Text = "NE";
            this.checkBoxVU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxVU.UseVisualStyleBackColor = false;
            this.checkBoxVU.Click += new System.EventHandler(this.RedListCategoryClick);
            // 
            // checkBoxEN
            // 
            this.checkBoxEN.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEN.Checked = true;
            this.checkBoxEN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEN.FlatAppearance.BorderSize = 0;
            this.checkBoxEN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEN.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEN.Location = new System.Drawing.Point(246, 138);
            this.checkBoxEN.Name = "checkBoxEN";
            this.checkBoxEN.Size = new System.Drawing.Size(32, 24);
            this.checkBoxEN.TabIndex = 14;
            this.checkBoxEN.Text = "NE";
            this.checkBoxEN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEN.UseVisualStyleBackColor = false;
            this.checkBoxEN.Click += new System.EventHandler(this.RedListCategoryClick);
            // 
            // checkBoxCR
            // 
            this.checkBoxCR.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCR.Checked = true;
            this.checkBoxCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCR.FlatAppearance.BorderSize = 0;
            this.checkBoxCR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCR.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCR.Location = new System.Drawing.Point(278, 138);
            this.checkBoxCR.Name = "checkBoxCR";
            this.checkBoxCR.Size = new System.Drawing.Size(32, 24);
            this.checkBoxCR.TabIndex = 14;
            this.checkBoxCR.Text = "NE";
            this.checkBoxCR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCR.UseVisualStyleBackColor = false;
            this.checkBoxCR.Click += new System.EventHandler(this.RedListCategoryClick);
            // 
            // checkBoxEW
            // 
            this.checkBoxEW.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEW.Checked = true;
            this.checkBoxEW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEW.FlatAppearance.BorderSize = 0;
            this.checkBoxEW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEW.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEW.Location = new System.Drawing.Point(310, 138);
            this.checkBoxEW.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxEW.Name = "checkBoxEW";
            this.checkBoxEW.Size = new System.Drawing.Size(32, 24);
            this.checkBoxEW.TabIndex = 14;
            this.checkBoxEW.Text = "EW";
            this.checkBoxEW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEW.UseVisualStyleBackColor = false;
            this.checkBoxEW.Click += new System.EventHandler(this.RedListCategoryClick);
            // 
            // checkBoxEX
            // 
            this.checkBoxEX.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEX.Checked = true;
            this.checkBoxEX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEX.FlatAppearance.BorderSize = 0;
            this.checkBoxEX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEX.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEX.Location = new System.Drawing.Point(342, 138);
            this.checkBoxEX.Name = "checkBoxEX";
            this.checkBoxEX.Size = new System.Drawing.Size(32, 24);
            this.checkBoxEX.TabIndex = 14;
            this.checkBoxEX.Text = "NE";
            this.checkBoxEX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxEX.UseVisualStyleBackColor = false;
            this.checkBoxEX.Click += new System.EventHandler(this.RedListCategoryClick);
            // 
            // TaxonEditInfo
            // 
            this.AllowDrop = true;
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
            this.Controls.Add(this.checkBoxFlagUseID);
            this.Controls.Add(this.checkBoxFlagExtinctInherited);
            this.Controls.Add(this.labelRedListCategory);
            this.Controls.Add(this.labelFlags);
            this.Controls.Add(this.buttonWikiCommons);
            this.Controls.Add(this.buttonFrenchSyn);
            this.Controls.Add(this.buttonRefSyn);
            this.Controls.Add(this.buttonClassicRank);
            this.Controls.Add(this.checkBoxLock);
            this.Controls.Add(this.buttonOTT);
            this.Controls.Add(this.buttonGoogle);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.splitContainerData);
            this.Controls.Add(this.textBoxClassicRank);
            this.Controls.Add(this.textBoxFrenchName);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelClassikRank);
            this.Controls.Add(this.labelFrench);
            this.Controls.Add(this.labelName);
            this.MinimumSize = new System.Drawing.Size(300, 400);
            this.Name = "TaxonEditInfo";
            this.Size = new System.Drawing.Size(447, 575);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.TaxonEditInfo_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.TaxonEditInfo_DragOver);
            this.splitContainerData.Panel1.ResumeLayout(false);
            this.splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).EndInit();
            this.splitContainerData.ResumeLayout(false);
            this.splitContainerComments.Panel1.ResumeLayout(false);
            this.splitContainerComments.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerComments)).EndInit();
            this.splitContainerComments.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelFrench;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxFrenchName;
        private System.Windows.Forms.Label labelClassikRank;
        private System.Windows.Forms.TextBox textBoxClassicRank;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.Label SpecificCharactersTitle;
        private TreeOfLife.Controls.TaxonMultiCommentControl Comments;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.CheckBox checkBoxLock;
        private System.Windows.Forms.Button buttonGoogle;
        private System.Windows.Forms.Button buttonClassicRank;
        private Controls.TaxonListImage taxonListImage1;
        private System.Windows.Forms.Button buttonOTT;
        private System.Windows.Forms.SplitContainer splitContainerComments;
        private System.Windows.Forms.Button buttonCreateComment;
        private System.Windows.Forms.Button buttonRefSyn;
        private System.Windows.Forms.Button buttonFrenchSyn;
        private System.Windows.Forms.Button buttonWikiCommons;
        private System.Windows.Forms.Label labelFlags;
        private System.Windows.Forms.CheckBox checkBoxFlagExtinctInherited;
        private System.Windows.Forms.CheckBox checkBoxFlagUseID;
        private System.Windows.Forms.Label labelRedListCategory;
        private System.Windows.Forms.CheckBox checkBoxNE;
        private System.Windows.Forms.CheckBox checkBoxDD;
        private System.Windows.Forms.CheckBox checkBoxLC;
        private System.Windows.Forms.CheckBox checkBoxNT;
        private System.Windows.Forms.CheckBox checkBoxVU;
        private System.Windows.Forms.CheckBox checkBoxEN;
        private System.Windows.Forms.CheckBox checkBoxCR;
        private System.Windows.Forms.CheckBox checkBoxEW;
        private System.Windows.Forms.CheckBox checkBoxEX;
    }
}
