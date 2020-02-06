namespace TreeOfLife
{
    partial class TaxonGameQuizz
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaxonGameQuizz));
            this.panelStart = new System.Windows.Forms.Panel();
            this.radioButtonSelected = new System.Windows.Forms.RadioButton();
            this.radioButtonRoot = new System.Windows.Forms.RadioButton();
            this.buttonStart = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelAvailableTaxon = new System.Windows.Forms.Label();
            this.labelInput = new System.Windows.Forms.Label();
            this.checkboxInputSound = new System.Windows.Forms.CheckBox();
            this.checkboxInputFrench = new System.Windows.Forms.CheckBox();
            this.checkboxInputImage = new System.Windows.Forms.CheckBox();
            this.checkboxInputLatin = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelPropositionsNumber = new System.Windows.Forms.Label();
            this.trackBarPropositions = new System.Windows.Forms.TrackBar();
            this.labelOutput = new System.Windows.Forms.Label();
            this.checkboxOutputSound = new System.Windows.Forms.CheckBox();
            this.checkboxOutputFrench = new System.Windows.Forms.CheckBox();
            this.labelPropositions = new System.Windows.Forms.Label();
            this.checkboxOutputImage = new System.Windows.Forms.CheckBox();
            this.checkboxOutputLatin = new System.Windows.Forms.CheckBox();
            this.textBoxNumQuestions = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelNbQuestions = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelEnd = new System.Windows.Forms.Panel();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.labelEndScore = new System.Windows.Forms.Label();
            this.panelGame2 = new System.Windows.Forms.Panel();
            this.buttonContinue1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.taxonISC = new TreeOfLife.Controls.TaxonImageControl();
            this.labelQuestion2 = new System.Windows.Forms.Label();
            this.labelScore2 = new System.Windows.Forms.Label();
            this.labelQuestion1 = new System.Windows.Forms.Label();
            this.buttonValidate = new System.Windows.Forms.Button();
            this.taxonMISC = new TreeOfLife.Controls.TaxonMultiImageSoundControl();
            this.panelStart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPropositions)).BeginInit();
            this.panelEnd.SuspendLayout();
            this.panelGame2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStart
            // 
            this.panelStart.Controls.Add(this.radioButtonSelected);
            this.panelStart.Controls.Add(this.radioButtonRoot);
            this.panelStart.Controls.Add(this.buttonStart);
            this.panelStart.Controls.Add(this.splitContainer1);
            this.panelStart.Controls.Add(this.textBoxNumQuestions);
            this.panelStart.Controls.Add(this.label1);
            this.panelStart.Controls.Add(this.labelNbQuestions);
            this.panelStart.Controls.Add(this.labelTitle);
            this.panelStart.Location = new System.Drawing.Point(0, 0);
            this.panelStart.Name = "panelStart";
            this.panelStart.Size = new System.Drawing.Size(300, 323);
            this.panelStart.TabIndex = 0;
            // 
            // radioButtonSelected
            // 
            this.radioButtonSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonSelected.Location = new System.Drawing.Point(132, 228);
            this.radioButtonSelected.Name = "radioButtonSelected";
            this.radioButtonSelected.Size = new System.Drawing.Size(168, 16);
            this.radioButtonSelected.TabIndex = 9;
            this.radioButtonSelected.TabStop = true;
            this.radioButtonSelected.UseVisualStyleBackColor = true;
            this.radioButtonSelected.CheckedChanged += new System.EventHandler(this.radioButtonSelected_CheckedChanged);
            // 
            // radioButtonRoot
            // 
            this.radioButtonRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonRoot.Location = new System.Drawing.Point(132, 211);
            this.radioButtonRoot.Name = "radioButtonRoot";
            this.radioButtonRoot.Size = new System.Drawing.Size(165, 16);
            this.radioButtonRoot.TabIndex = 8;
            this.radioButtonRoot.TabStop = true;
            this.radioButtonRoot.Text = "Taxon root";
            this.radioButtonRoot.UseVisualStyleBackColor = true;
            this.radioButtonRoot.CheckedChanged += new System.EventHandler(this.radioButtonRoot_CheckedChanged);
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(6, 280);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(291, 39);
            this.buttonStart.TabIndex = 7;
            this.buttonStart.Text = "Start !!";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 60);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labelAvailableTaxon);
            this.splitContainer1.Panel1.Controls.Add(this.labelInput);
            this.splitContainer1.Panel1.Controls.Add(this.checkboxInputSound);
            this.splitContainer1.Panel1.Controls.Add(this.checkboxInputFrench);
            this.splitContainer1.Panel1.Controls.Add(this.checkboxInputImage);
            this.splitContainer1.Panel1.Controls.Add(this.checkboxInputLatin);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.labelPropositionsNumber);
            this.splitContainer1.Panel2.Controls.Add(this.trackBarPropositions);
            this.splitContainer1.Panel2.Controls.Add(this.labelOutput);
            this.splitContainer1.Panel2.Controls.Add(this.checkboxOutputSound);
            this.splitContainer1.Panel2.Controls.Add(this.checkboxOutputFrench);
            this.splitContainer1.Panel2.Controls.Add(this.labelPropositions);
            this.splitContainer1.Panel2.Controls.Add(this.checkboxOutputImage);
            this.splitContainer1.Panel2.Controls.Add(this.checkboxOutputLatin);
            this.splitContainer1.Size = new System.Drawing.Size(300, 145);
            this.splitContainer1.SplitterDistance = 150;
            this.splitContainer1.TabIndex = 6;
            // 
            // labelAvailableTaxon
            // 
            this.labelAvailableTaxon.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.labelAvailableTaxon.Location = new System.Drawing.Point(112, 117);
            this.labelAvailableTaxon.Name = "labelAvailableTaxon";
            this.labelAvailableTaxon.Size = new System.Drawing.Size(52, 19);
            this.labelAvailableTaxon.TabIndex = 1;
            this.labelAvailableTaxon.Text = "4";
            this.labelAvailableTaxon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInput
            // 
            this.labelInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInput.AutoSize = true;
            this.labelInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelInput.Location = new System.Drawing.Point(86, 1);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(51, 15);
            this.labelInput.TabIndex = 1;
            this.labelInput.Text = "Question";
            // 
            // checkboxInputSound
            // 
            this.checkboxInputSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkboxInputSound.AutoSize = true;
            this.checkboxInputSound.Location = new System.Drawing.Point(80, 40);
            this.checkboxInputSound.Name = "checkboxInputSound";
            this.checkboxInputSound.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkboxInputSound.Size = new System.Drawing.Size(57, 17);
            this.checkboxInputSound.TabIndex = 5;
            this.checkboxInputSound.Text = "Sound";
            this.checkboxInputSound.UseVisualStyleBackColor = true;
            this.checkboxInputSound.Click += new System.EventHandler(this.checkboxInputSound_Click);
            // 
            // checkboxInputFrench
            // 
            this.checkboxInputFrench.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkboxInputFrench.AutoSize = true;
            this.checkboxInputFrench.Location = new System.Drawing.Point(78, 70);
            this.checkboxInputFrench.Name = "checkboxInputFrench";
            this.checkboxInputFrench.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkboxInputFrench.Size = new System.Drawing.Size(59, 17);
            this.checkboxInputFrench.TabIndex = 5;
            this.checkboxInputFrench.Text = "French";
            this.checkboxInputFrench.UseVisualStyleBackColor = true;
            this.checkboxInputFrench.Click += new System.EventHandler(this.checkboxInputFrench_Click);
            // 
            // checkboxInputImage
            // 
            this.checkboxInputImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkboxInputImage.AutoSize = true;
            this.checkboxInputImage.Location = new System.Drawing.Point(82, 25);
            this.checkboxInputImage.Name = "checkboxInputImage";
            this.checkboxInputImage.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkboxInputImage.Size = new System.Drawing.Size(55, 17);
            this.checkboxInputImage.TabIndex = 3;
            this.checkboxInputImage.Text = "Image";
            this.checkboxInputImage.UseVisualStyleBackColor = true;
            this.checkboxInputImage.Click += new System.EventHandler(this.checkboxInputImage_Click);
            // 
            // checkboxInputLatin
            // 
            this.checkboxInputLatin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkboxInputLatin.AutoSize = true;
            this.checkboxInputLatin.Location = new System.Drawing.Point(88, 55);
            this.checkboxInputLatin.Name = "checkboxInputLatin";
            this.checkboxInputLatin.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkboxInputLatin.Size = new System.Drawing.Size(49, 17);
            this.checkboxInputLatin.TabIndex = 4;
            this.checkboxInputLatin.Text = "Latin";
            this.checkboxInputLatin.UseVisualStyleBackColor = true;
            this.checkboxInputLatin.Click += new System.EventHandler(this.checkboxInputLatin_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(3, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Avalaible taxon:";
            // 
            // labelPropositionsNumber
            // 
            this.labelPropositionsNumber.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.labelPropositionsNumber.Location = new System.Drawing.Point(82, 94);
            this.labelPropositionsNumber.Name = "labelPropositionsNumber";
            this.labelPropositionsNumber.Size = new System.Drawing.Size(33, 19);
            this.labelPropositionsNumber.TabIndex = 1;
            this.labelPropositionsNumber.Text = "4";
            this.labelPropositionsNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarPropositions
            // 
            this.trackBarPropositions.AutoSize = false;
            this.trackBarPropositions.Location = new System.Drawing.Point(10, 113);
            this.trackBarPropositions.Maximum = 8;
            this.trackBarPropositions.Minimum = 2;
            this.trackBarPropositions.Name = "trackBarPropositions";
            this.trackBarPropositions.Size = new System.Drawing.Size(122, 29);
            this.trackBarPropositions.TabIndex = 9;
            this.trackBarPropositions.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarPropositions.Value = 4;
            this.trackBarPropositions.ValueChanged += new System.EventHandler(this.trackBarPropositions_ValueChanged);
            // 
            // labelOutput
            // 
            this.labelOutput.AutoSize = true;
            this.labelOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelOutput.Location = new System.Drawing.Point(10, 1);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(49, 15);
            this.labelOutput.TabIndex = 1;
            this.labelOutput.Text = "Answers";
            // 
            // checkboxOutputSound
            // 
            this.checkboxOutputSound.AutoSize = true;
            this.checkboxOutputSound.Location = new System.Drawing.Point(10, 40);
            this.checkboxOutputSound.Name = "checkboxOutputSound";
            this.checkboxOutputSound.Size = new System.Drawing.Size(57, 17);
            this.checkboxOutputSound.TabIndex = 8;
            this.checkboxOutputSound.Text = "Sound";
            this.checkboxOutputSound.UseVisualStyleBackColor = true;
            this.checkboxOutputSound.Click += new System.EventHandler(this.checkboxOutputSound_Click);
            // 
            // checkboxOutputFrench
            // 
            this.checkboxOutputFrench.AutoSize = true;
            this.checkboxOutputFrench.Location = new System.Drawing.Point(10, 70);
            this.checkboxOutputFrench.Name = "checkboxOutputFrench";
            this.checkboxOutputFrench.Size = new System.Drawing.Size(59, 17);
            this.checkboxOutputFrench.TabIndex = 8;
            this.checkboxOutputFrench.Text = "French";
            this.checkboxOutputFrench.UseVisualStyleBackColor = true;
            this.checkboxOutputFrench.Click += new System.EventHandler(this.checkboxOutputFrench_Click);
            // 
            // labelPropositions
            // 
            this.labelPropositions.AutoSize = true;
            this.labelPropositions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPropositions.Location = new System.Drawing.Point(10, 96);
            this.labelPropositions.Name = "labelPropositions";
            this.labelPropositions.Size = new System.Drawing.Size(66, 15);
            this.labelPropositions.TabIndex = 1;
            this.labelPropositions.Text = "Propositions";
            // 
            // checkboxOutputImage
            // 
            this.checkboxOutputImage.AutoSize = true;
            this.checkboxOutputImage.Location = new System.Drawing.Point(10, 25);
            this.checkboxOutputImage.Name = "checkboxOutputImage";
            this.checkboxOutputImage.Size = new System.Drawing.Size(55, 17);
            this.checkboxOutputImage.TabIndex = 6;
            this.checkboxOutputImage.Text = "Image";
            this.checkboxOutputImage.UseVisualStyleBackColor = true;
            this.checkboxOutputImage.Click += new System.EventHandler(this.checkboxOutputImage_Click);
            // 
            // checkboxOutputLatin
            // 
            this.checkboxOutputLatin.AutoSize = true;
            this.checkboxOutputLatin.Location = new System.Drawing.Point(10, 55);
            this.checkboxOutputLatin.Name = "checkboxOutputLatin";
            this.checkboxOutputLatin.Size = new System.Drawing.Size(49, 17);
            this.checkboxOutputLatin.TabIndex = 7;
            this.checkboxOutputLatin.Text = "Latin";
            this.checkboxOutputLatin.UseVisualStyleBackColor = true;
            this.checkboxOutputLatin.Click += new System.EventHandler(this.checkboxOutputLatin_Click);
            // 
            // textBoxNumQuestions
            // 
            this.textBoxNumQuestions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNumQuestions.Location = new System.Drawing.Point(115, 34);
            this.textBoxNumQuestions.Name = "textBoxNumQuestions";
            this.textBoxNumQuestions.Size = new System.Drawing.Size(182, 20);
            this.textBoxNumQuestions.TabIndex = 2;
            this.textBoxNumQuestions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxNumQuestions_KeyDown);
            this.textBoxNumQuestions.Leave += new System.EventHandler(this.textBoxNumQuestions_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(6, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Take taxon included in:";
            // 
            // labelNbQuestions
            // 
            this.labelNbQuestions.AutoSize = true;
            this.labelNbQuestions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelNbQuestions.Location = new System.Drawing.Point(3, 37);
            this.labelNbQuestions.Name = "labelNbQuestions";
            this.labelNbQuestions.Size = new System.Drawing.Size(106, 15);
            this.labelNbQuestions.TabIndex = 1;
            this.labelNbQuestions.Text = "Number of questions";
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
            this.labelTitle.Size = new System.Drawing.Size(297, 23);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "TAXON QUIZZ";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelEnd
            // 
            this.panelEnd.BackgroundImage = global::TreeOfLife.Properties.Resources.new_quizz_background;
            this.panelEnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelEnd.Controls.Add(this.buttonEnd);
            this.panelEnd.Controls.Add(this.labelEndScore);
            this.panelEnd.Location = new System.Drawing.Point(7, 340);
            this.panelEnd.Name = "panelEnd";
            this.panelEnd.Size = new System.Drawing.Size(279, 263);
            this.panelEnd.TabIndex = 0;
            // 
            // buttonEnd
            // 
            this.buttonEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEnd.Location = new System.Drawing.Point(6, 237);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(269, 23);
            this.buttonEnd.TabIndex = 1;
            this.buttonEnd.Text = "End !";
            this.buttonEnd.UseVisualStyleBackColor = true;
            this.buttonEnd.Click += new System.EventHandler(this.buttonEnd_Click);
            // 
            // labelEndScore
            // 
            this.labelEndScore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEndScore.BackColor = System.Drawing.Color.Transparent;
            this.labelEndScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEndScore.Location = new System.Drawing.Point(6, 31);
            this.labelEndScore.Name = "labelEndScore";
            this.labelEndScore.Size = new System.Drawing.Size(269, 69);
            this.labelEndScore.TabIndex = 0;
            this.labelEndScore.Text = "Your score is";
            this.labelEndScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelGame2
            // 
            this.panelGame2.Controls.Add(this.buttonContinue1);
            this.panelGame2.Controls.Add(this.button2);
            this.panelGame2.Controls.Add(this.splitContainer2);
            this.panelGame2.Location = new System.Drawing.Point(326, 332);
            this.panelGame2.Name = "panelGame2";
            this.panelGame2.Size = new System.Drawing.Size(317, 323);
            this.panelGame2.TabIndex = 0;
            // 
            // buttonContinue1
            // 
            this.buttonContinue1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonContinue1.AutoSize = true;
            this.buttonContinue1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonContinue1.Location = new System.Drawing.Point(7, 294);
            this.buttonContinue1.Name = "buttonContinue1";
            this.buttonContinue1.Size = new System.Drawing.Size(77, 26);
            this.buttonContinue1.TabIndex = 1;
            this.buttonContinue1.Text = "Next  >>";
            this.buttonContinue1.UseVisualStyleBackColor = true;
            this.buttonContinue1.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.AutoSize = true;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(278, 297);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Quit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.splitContainer2.Panel1.Controls.Add(this.taxonISC);
            this.splitContainer2.Panel1.Controls.Add(this.labelQuestion2);
            this.splitContainer2.Panel1.Controls.Add(this.labelScore2);
            this.splitContainer2.Panel1.Controls.Add(this.labelQuestion1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.buttonValidate);
            this.splitContainer2.Panel2.Controls.Add(this.taxonMISC);
            this.splitContainer2.Size = new System.Drawing.Size(311, 288);
            this.splitContainer2.SplitterDistance = 202;
            this.splitContainer2.TabIndex = 9;
            // 
            // taxonISC
            // 
            this.taxonISC.AllowContextualMenu = true;
            this.taxonISC.AllowNavigationButtons = true;
            this.taxonISC.AllowSecondaryImages = false;
            this.taxonISC.AllowSound = true;
            this.taxonISC.AllowTips = true;
            this.taxonISC.CurrentImage = ((System.Drawing.Image)(resources.GetObject("taxonISC.CurrentImage")));
            this.taxonISC.CurrentTaxon = null;
            this.taxonISC.DisplayFrench = true;
            this.taxonISC.DisplayImage = true;
            this.taxonISC.DisplayLatin = true;
            this.taxonISC.FrenchVisible = true;
            this.taxonISC.ImageVisible = true;
            this.taxonISC.LatinVisible = true;
            this.taxonISC.Location = new System.Drawing.Point(26, 46);
            this.taxonISC.Name = "taxonISC";
            this.taxonISC.Size = new System.Drawing.Size(200, 150);
            this.taxonISC.TabIndex = 9;
            // 
            // labelQuestion2
            // 
            this.labelQuestion2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelQuestion2.BackColor = System.Drawing.Color.Transparent;
            this.labelQuestion2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuestion2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.labelQuestion2.Location = new System.Drawing.Point(3, 24);
            this.labelQuestion2.Name = "labelQuestion2";
            this.labelQuestion2.Size = new System.Drawing.Size(193, 19);
            this.labelQuestion2.TabIndex = 8;
            this.labelQuestion2.Text = "Qu\'est-ce qui correspond à:";
            // 
            // labelScore2
            // 
            this.labelScore2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelScore2.BackColor = System.Drawing.Color.Transparent;
            this.labelScore2.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScore2.ForeColor = System.Drawing.Color.Navy;
            this.labelScore2.Location = new System.Drawing.Point(220, 5);
            this.labelScore2.Name = "labelScore2";
            this.labelScore2.Size = new System.Drawing.Size(88, 59);
            this.labelScore2.TabIndex = 8;
            this.labelScore2.Text = "00";
            this.labelScore2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelQuestion1
            // 
            this.labelQuestion1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelQuestion1.BackColor = System.Drawing.Color.Transparent;
            this.labelQuestion1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuestion1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelQuestion1.Location = new System.Drawing.Point(3, 5);
            this.labelQuestion1.Name = "labelQuestion1";
            this.labelQuestion1.Size = new System.Drawing.Size(211, 19);
            this.labelQuestion1.TabIndex = 8;
            this.labelQuestion1.Text = "Question";
            // 
            // buttonValidate
            // 
            this.buttonValidate.AutoSize = true;
            this.buttonValidate.Location = new System.Drawing.Point(152, 39);
            this.buttonValidate.Name = "buttonValidate";
            this.buttonValidate.Size = new System.Drawing.Size(75, 23);
            this.buttonValidate.TabIndex = 11;
            this.buttonValidate.Text = "Validate";
            this.buttonValidate.UseVisualStyleBackColor = true;
            this.buttonValidate.Visible = false;
            this.buttonValidate.Click += new System.EventHandler(this.buttonValidate_Click);
            // 
            // taxonMISC
            // 
            this.taxonMISC.AllowTips = true;
            this.taxonMISC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taxonMISC.ImageNumber = 0;
            this.taxonMISC.ListTaxons = null;
            this.taxonMISC.ListTaxonsFamily = null;
            this.taxonMISC.Location = new System.Drawing.Point(0, 0);
            this.taxonMISC.Name = "taxonMISC";
            this.taxonMISC.Size = new System.Drawing.Size(311, 82);
            this.taxonMISC.TabIndex = 0;
            // 
            // TaxonGameQuizz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.panelEnd);
            this.Controls.Add(this.panelGame2);
            this.Controls.Add(this.panelStart);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "TaxonGameQuizz";
            this.Size = new System.Drawing.Size(731, 692);
            this.panelStart.ResumeLayout(false);
            this.panelStart.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPropositions)).EndInit();
            this.panelEnd.ResumeLayout(false);
            this.panelGame2.ResumeLayout(false);
            this.panelGame2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelStart;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelEnd;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox checkboxInputFrench;
        private System.Windows.Forms.CheckBox checkboxInputLatin;
        private System.Windows.Forms.CheckBox checkboxInputImage;
        private System.Windows.Forms.TextBox textBoxNumQuestions;
        private System.Windows.Forms.Label labelInput;
        private System.Windows.Forms.Label labelNbQuestions;
        private System.Windows.Forms.Label labelPropositionsNumber;
        private System.Windows.Forms.TrackBar trackBarPropositions;
        private System.Windows.Forms.Label labelOutput;
        private System.Windows.Forms.CheckBox checkboxOutputFrench;
        private System.Windows.Forms.Label labelPropositions;
        private System.Windows.Forms.CheckBox checkboxOutputImage;
        private System.Windows.Forms.CheckBox checkboxOutputLatin;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Label labelEndScore;
        private System.Windows.Forms.RadioButton radioButtonSelected;
        private System.Windows.Forms.RadioButton radioButtonRoot;
        private System.Windows.Forms.Label labelAvailableTaxon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkboxInputSound;
        private System.Windows.Forms.CheckBox checkboxOutputSound;
        private System.Windows.Forms.Panel panelGame2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label labelQuestion2;
        private System.Windows.Forms.Label labelQuestion1;
        private Controls.TaxonMultiImageSoundControl taxonMISC;
        private Controls.TaxonImageControl taxonISC;
        private System.Windows.Forms.Label labelScore2;
        private System.Windows.Forms.Button buttonValidate;
        private System.Windows.Forms.Button buttonContinue1;
    }
}
