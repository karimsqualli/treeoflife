namespace TreeOfLife
{
    partial class TaxonGameHangman
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaxonGameHangman));
            this.panelStart = new System.Windows.Forms.Panel();
            this.labelInput = new System.Windows.Forms.Label();
            this.radioButtonFrench = new System.Windows.Forms.RadioButton();
            this.buttonStart = new System.Windows.Forms.Button();
            this.radioButtonLatin = new System.Windows.Forms.RadioButton();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelGame = new System.Windows.Forms.Panel();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.labelTitle2 = new System.Windows.Forms.Label();
            this.splitContainerQuestion = new System.Windows.Forms.SplitContainer();
            this.splitContainerImages = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.taxonImageControlInput = new TreeOfLife.Controls.TaxonImageControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelState = new System.Windows.Forms.Label();
            this.panelStart.SuspendLayout();
            this.panelGame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerQuestion)).BeginInit();
            this.splitContainerQuestion.Panel1.SuspendLayout();
            this.splitContainerQuestion.Panel2.SuspendLayout();
            this.splitContainerQuestion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImages)).BeginInit();
            this.splitContainerImages.Panel1.SuspendLayout();
            this.splitContainerImages.Panel2.SuspendLayout();
            this.splitContainerImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStart
            // 
            this.panelStart.Controls.Add(this.labelInput);
            this.panelStart.Controls.Add(this.radioButtonFrench);
            this.panelStart.Controls.Add(this.buttonStart);
            this.panelStart.Controls.Add(this.radioButtonLatin);
            this.panelStart.Controls.Add(this.labelTitle);
            this.panelStart.Location = new System.Drawing.Point(0, 0);
            this.panelStart.Name = "panelStart";
            this.panelStart.Size = new System.Drawing.Size(152, 363);
            this.panelStart.TabIndex = 0;
            // 
            // labelInput
            // 
            this.labelInput.AutoSize = true;
            this.labelInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelInput.Location = new System.Drawing.Point(7, 34);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(86, 15);
            this.labelInput.TabIndex = 1;
            this.labelInput.Text = "Choose langage";
            // 
            // radioButtonFrench
            // 
            this.radioButtonFrench.AutoSize = true;
            this.radioButtonFrench.Location = new System.Drawing.Point(45, 78);
            this.radioButtonFrench.Name = "radioButtonFrench";
            this.radioButtonFrench.Size = new System.Drawing.Size(58, 17);
            this.radioButtonFrench.TabIndex = 8;
            this.radioButtonFrench.TabStop = true;
            this.radioButtonFrench.Text = "French";
            this.radioButtonFrench.UseVisualStyleBackColor = true;
            this.radioButtonFrench.CheckedChanged += new System.EventHandler(this.radioButtonInputFrench_CheckedChanged);
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(7, 316);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(143, 39);
            this.buttonStart.TabIndex = 7;
            this.buttonStart.Text = "Start !!";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // radioButtonLatin
            // 
            this.radioButtonLatin.AutoSize = true;
            this.radioButtonLatin.Location = new System.Drawing.Point(45, 61);
            this.radioButtonLatin.Name = "radioButtonLatin";
            this.radioButtonLatin.Size = new System.Drawing.Size(48, 17);
            this.radioButtonLatin.TabIndex = 7;
            this.radioButtonLatin.TabStop = true;
            this.radioButtonLatin.Text = "Latin";
            this.radioButtonLatin.UseVisualStyleBackColor = true;
            this.radioButtonLatin.CheckedChanged += new System.EventHandler(this.radioButtonInputLatin_CheckedChanged);
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelTitle.Location = new System.Drawing.Point(3, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(149, 23);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "TAXON HANGMAN";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelGame
            // 
            this.panelGame.Controls.Add(this.buttonQuit);
            this.panelGame.Controls.Add(this.labelTitle2);
            this.panelGame.Controls.Add(this.splitContainerQuestion);
            this.panelGame.Location = new System.Drawing.Point(158, 3);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(253, 360);
            this.panelGame.TabIndex = 0;
            // 
            // buttonQuit
            // 
            this.buttonQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonQuit.Location = new System.Drawing.Point(214, 334);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(36, 23);
            this.buttonQuit.TabIndex = 10;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // labelTitle2
            // 
            this.labelTitle2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitle2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTitle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelTitle2.Location = new System.Drawing.Point(0, 0);
            this.labelTitle2.Name = "labelTitle2";
            this.labelTitle2.Size = new System.Drawing.Size(253, 23);
            this.labelTitle2.TabIndex = 8;
            this.labelTitle2.Text = "Hangman";
            this.labelTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainerQuestion
            // 
            this.splitContainerQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerQuestion.Location = new System.Drawing.Point(6, 31);
            this.splitContainerQuestion.Name = "splitContainerQuestion";
            this.splitContainerQuestion.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerQuestion.Panel1
            // 
            this.splitContainerQuestion.Panel1.BackColor = System.Drawing.Color.Pink;
            this.splitContainerQuestion.Panel1.Controls.Add(this.splitContainerImages);
            // 
            // splitContainerQuestion.Panel2
            // 
            this.splitContainerQuestion.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainerQuestion.Size = new System.Drawing.Size(244, 297);
            this.splitContainerQuestion.SplitterDistance = 210;
            this.splitContainerQuestion.TabIndex = 9;
            // 
            // splitContainerImages
            // 
            this.splitContainerImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerImages.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerImages.IsSplitterFixed = true;
            this.splitContainerImages.Location = new System.Drawing.Point(0, 0);
            this.splitContainerImages.Name = "splitContainerImages";
            // 
            // splitContainerImages.Panel1
            // 
            this.splitContainerImages.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainerImages.Panel2
            // 
            this.splitContainerImages.Panel2.Controls.Add(this.taxonImageControlInput);
            this.splitContainerImages.Size = new System.Drawing.Size(244, 210);
            this.splitContainerImages.SplitterDistance = 180;
            this.splitContainerImages.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman11;
            this.pictureBox1.InitialImage = global::TreeOfLife.Properties.Resources.hangman11;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(180, 210);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // taxonImageControlInput
            // 
            this.taxonImageControlInput.CurrentImage = ((System.Drawing.Image)(resources.GetObject("taxonImageControlInput.CurrentImage")));
            this.taxonImageControlInput.CurrentTaxon = null;
            this.taxonImageControlInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taxonImageControlInput.Location = new System.Drawing.Point(0, 0);
            this.taxonImageControlInput.Name = "taxonImageControlInput";
            this.taxonImageControlInput.Size = new System.Drawing.Size(60, 210);
            this.taxonImageControlInput.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.labelState);
            this.splitContainer1.Size = new System.Drawing.Size(244, 83);
            this.splitContainer1.SplitterDistance = 54;
            this.splitContainer1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(244, 54);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelState
            // 
            this.labelState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelState.Location = new System.Drawing.Point(0, 0);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(244, 25);
            this.labelState.TabIndex = 1;
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TaxonGameHangman
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.panelGame);
            this.Controls.Add(this.panelStart);
            this.Name = "TaxonGameHangman";
            this.Size = new System.Drawing.Size(509, 366);
            this.panelStart.ResumeLayout(false);
            this.panelStart.PerformLayout();
            this.panelGame.ResumeLayout(false);
            this.splitContainerQuestion.Panel1.ResumeLayout(false);
            this.splitContainerQuestion.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerQuestion)).EndInit();
            this.splitContainerQuestion.ResumeLayout(false);
            this.splitContainerImages.Panel1.ResumeLayout(false);
            this.splitContainerImages.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImages)).EndInit();
            this.splitContainerImages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelStart;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelGame;
        private System.Windows.Forms.Label labelInput;
        private System.Windows.Forms.RadioButton radioButtonFrench;
        private System.Windows.Forms.RadioButton radioButtonLatin;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelTitle2;
        private System.Windows.Forms.SplitContainer splitContainerQuestion;
        private Controls.TaxonImageControl taxonImageControlInput;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.SplitContainer splitContainerImages;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label labelState;
    }
}
