namespace TreeOfLife.TaxonDialog
{
    partial class CollectionManagerDialog
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlCollections = new System.Windows.Forms.TabControl();
            this.tabPageImages = new System.Windows.Forms.TabPage();
            this.dataGridViewImages = new System.Windows.Forms.DataGridView();
            this.ColumnImageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnImagesNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnImageUseIt = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnImageDefault = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabPageComments = new System.Windows.Forms.TabPage();
            this.dataGridViewComments = new System.Windows.Forms.DataGridView();
            this.textBoxNew = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelDescription = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAddNew = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.ColumnCommentsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCommentsNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCommentsUseIt = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnCommentsRank = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlCollections.SuspendLayout();
            this.tabPageImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImages)).BeginInit();
            this.tabPageComments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComments)).BeginInit();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControlCollections);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxNew);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSave);
            this.splitContainer1.Panel2.Controls.Add(this.labelDescription);
            this.splitContainer1.Panel2.Controls.Add(this.buttonClose);
            this.splitContainer1.Panel2.Controls.Add(this.buttonDelete);
            this.splitContainer1.Panel2.Controls.Add(this.buttonAddNew);
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(378, 433);
            this.splitContainer1.SplitterDistance = 264;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControlCollections
            // 
            this.tabControlCollections.Controls.Add(this.tabPageImages);
            this.tabControlCollections.Controls.Add(this.tabPageComments);
            this.tabControlCollections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCollections.Location = new System.Drawing.Point(0, 0);
            this.tabControlCollections.Name = "tabControlCollections";
            this.tabControlCollections.SelectedIndex = 0;
            this.tabControlCollections.Size = new System.Drawing.Size(378, 264);
            this.tabControlCollections.TabIndex = 1;
            this.tabControlCollections.SelectedIndexChanged += new System.EventHandler(this.tabControlCollections_SelectedIndexChanged);
            // 
            // tabPageImages
            // 
            this.tabPageImages.Controls.Add(this.dataGridViewImages);
            this.tabPageImages.Location = new System.Drawing.Point(4, 22);
            this.tabPageImages.Name = "tabPageImages";
            this.tabPageImages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImages.Size = new System.Drawing.Size(370, 238);
            this.tabPageImages.TabIndex = 0;
            this.tabPageImages.Text = "Images";
            this.tabPageImages.UseVisualStyleBackColor = true;
            // 
            // dataGridViewImages
            // 
            this.dataGridViewImages.AllowUserToAddRows = false;
            this.dataGridViewImages.AllowUserToDeleteRows = false;
            this.dataGridViewImages.AllowUserToResizeColumns = false;
            this.dataGridViewImages.AllowUserToResizeRows = false;
            this.dataGridViewImages.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewImages.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dataGridViewImages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewImages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnImageName,
            this.ColumnImagesNumber,
            this.ColumnImageUseIt,
            this.ColumnImageDefault});
            this.dataGridViewImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewImages.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewImages.MultiSelect = false;
            this.dataGridViewImages.Name = "dataGridViewImages";
            this.dataGridViewImages.RowHeadersVisible = false;
            this.dataGridViewImages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewImages.ShowCellErrors = false;
            this.dataGridViewImages.ShowCellToolTips = false;
            this.dataGridViewImages.ShowEditingIcon = false;
            this.dataGridViewImages.ShowRowErrors = false;
            this.dataGridViewImages.Size = new System.Drawing.Size(364, 232);
            this.dataGridViewImages.TabIndex = 0;
            this.dataGridViewImages.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellClick);
            this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
            this.dataGridViewImages.SelectionChanged += new System.EventHandler(this.dataGridViewImages_SelectionChanged);
            // 
            // ColumnImageName
            // 
            this.ColumnImageName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnImageName.FillWeight = 50.74626F;
            this.ColumnImageName.HeaderText = "Name";
            this.ColumnImageName.MaxInputLength = 100;
            this.ColumnImageName.Name = "ColumnImageName";
            this.ColumnImageName.ReadOnly = true;
            // 
            // ColumnImagesNumber
            // 
            this.ColumnImagesNumber.Name = "ColumnImagesNumber";
            this.ColumnImagesNumber.HeaderText = "Number";
            this.ColumnImagesNumber.ReadOnly = true;
            // 
            // ColumnImageUseIt
            // 
            this.ColumnImageUseIt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnImageUseIt.HeaderText = "Use it";
            this.ColumnImageUseIt.MinimumWidth = 50;
            this.ColumnImageUseIt.Name = "ColumnImageUseIt";
            this.ColumnImageUseIt.ReadOnly = true;
            this.ColumnImageUseIt.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnImageUseIt.Width = 50;
            // 
            // ColumnImageDefault
            // 
            this.ColumnImageDefault.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnImageDefault.HeaderText = "Default";
            this.ColumnImageDefault.MinimumWidth = 50;
            this.ColumnImageDefault.Name = "ColumnImageDefault";
            this.ColumnImageDefault.ReadOnly = true;
            this.ColumnImageDefault.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnImageDefault.Width = 50;
            // 
            // tabPageComments
            // 
            this.tabPageComments.Controls.Add(this.dataGridViewComments);
            this.tabPageComments.Location = new System.Drawing.Point(4, 22);
            this.tabPageComments.Name = "tabPageComments";
            this.tabPageComments.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageComments.Size = new System.Drawing.Size(370, 238);
            this.tabPageComments.TabIndex = 1;
            this.tabPageComments.Text = "Comments";
            this.tabPageComments.UseVisualStyleBackColor = true;
            // 
            // dataGridViewComments
            // 
            this.dataGridViewComments.AllowUserToAddRows = false;
            this.dataGridViewComments.AllowUserToDeleteRows = false;
            this.dataGridViewComments.AllowUserToResizeColumns = false;
            this.dataGridViewComments.AllowUserToResizeRows = false;
            this.dataGridViewComments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewComments.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dataGridViewComments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewComments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCommentsName,
            this.ColumnCommentsNumber,
            this.ColumnCommentsUseIt,
            this.ColumnCommentsRank});
            this.dataGridViewComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewComments.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewComments.MultiSelect = false;
            this.dataGridViewComments.Name = "dataGridViewComments";
            this.dataGridViewComments.RowHeadersVisible = false;
            this.dataGridViewComments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewComments.ShowCellErrors = false;
            this.dataGridViewComments.ShowCellToolTips = false;
            this.dataGridViewComments.ShowEditingIcon = false;
            this.dataGridViewComments.ShowRowErrors = false;
            this.dataGridViewComments.Size = new System.Drawing.Size(364, 232);
            this.dataGridViewComments.TabIndex = 0;
            this.dataGridViewComments.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewComments_CellClick);
            this.dataGridViewComments.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewComments_CellValueChanged);
            this.dataGridViewComments.SelectionChanged += new System.EventHandler(this.dataGridViewComments_SelectionChanged);
            // 
            // textBoxNew
            // 
            this.textBoxNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNew.Location = new System.Drawing.Point(78, 134);
            this.textBoxNew.Name = "textBoxNew";
            this.textBoxNew.Size = new System.Drawing.Size(178, 20);
            this.textBoxNew.TabIndex = 6;
            this.textBoxNew.TextChanged += new System.EventHandler(this.textBoxNew_TextChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(78, 4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(12, 9);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(60, 13);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(296, 132);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new System.Drawing.Point(261, 4);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(110, 23);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete collection";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAddNew
            // 
            this.buttonAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddNew.Enabled = false;
            this.buttonAddNew.Location = new System.Drawing.Point(3, 132);
            this.buttonAddNew.Name = "buttonAddNew";
            this.buttonAddNew.Size = new System.Drawing.Size(75, 23);
            this.buttonAddNew.TabIndex = 1;
            this.buttonAddNew.Text = "Add new";
            this.buttonAddNew.UseVisualStyleBackColor = true;
            this.buttonAddNew.Click += new System.EventHandler(this.buttonAddNew_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(3, 33);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(368, 93);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // ColumnCommentsName
            // 
            this.ColumnCommentsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnCommentsName.FillWeight = 50.74626F;
            this.ColumnCommentsName.HeaderText = "Name";
            this.ColumnCommentsName.MaxInputLength = 100;
            this.ColumnCommentsName.Name = "ColumnCommentsName";
            this.ColumnCommentsName.ReadOnly = true;
            // 
            // ColumnCommentsNumber
            // 
            this.ColumnCommentsNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnCommentsNumber.HeaderText = "Comments";
            this.ColumnCommentsNumber.MinimumWidth = 80;
            this.ColumnCommentsNumber.Name = "ColumnCommentsNumber";
            this.ColumnCommentsNumber.ReadOnly = true;
            this.ColumnCommentsNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnCommentsNumber.Width = 80;
            // 
            // ColumnCommentsUseIt
            // 
            this.ColumnCommentsUseIt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnCommentsUseIt.HeaderText = "Use it";
            this.ColumnCommentsUseIt.MinimumWidth = 50;
            this.ColumnCommentsUseIt.Name = "ColumnCommentsUseIt";
            this.ColumnCommentsUseIt.ReadOnly = true;
            this.ColumnCommentsUseIt.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnCommentsUseIt.Width = 50;
            // 
            // ColumnCommentsRank
            // 
            this.ColumnCommentsRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnCommentsRank.HeaderText = "Prio";
            this.ColumnCommentsRank.MinimumWidth = 64;
            this.ColumnCommentsRank.Name = "ColumnCommentsRank";
            this.ColumnCommentsRank.ReadOnly = true;
            this.ColumnCommentsRank.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnCommentsRank.Width = 64;
            // 
            // CollectionManagerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 433);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(1000, 600);
            this.MinimumSize = new System.Drawing.Size(350, 250);
            this.Name = "CollectionManagerDialog";
            this.Text = "Collection Manager";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlCollections.ResumeLayout(false);
            this.tabPageImages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImages)).EndInit();
            this.tabPageComments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridViewImages;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAddNew;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnImageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnImagesNumber;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnImageUseIt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnImageDefault;
        private System.Windows.Forms.TabControl tabControlCollections;
        private System.Windows.Forms.TabPage tabPageImages;
        private System.Windows.Forms.TabPage tabPageComments;
        private System.Windows.Forms.DataGridView dataGridViewComments;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommentsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommentsNumber;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnCommentsUseIt;
        private System.Windows.Forms.DataGridViewImageColumn ColumnCommentsRank;
    }
}