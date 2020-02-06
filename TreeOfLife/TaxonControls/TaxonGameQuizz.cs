using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TreeOfLife.Controls;

namespace TreeOfLife
{

    [Description("A quizz game")]
    [DisplayName("Quizz")]
    [Controls.IconAttribute("TaxonGameQuizz")]
    public partial class TaxonGameQuizz : Controls.TaxonControl
    {
        //---------------------------------------------------------------------------------
        public TaxonGameQuizz()
        {
            InitializeComponent();
            panelStart.Dock = DockStyle.Fill;
            panelGame2.Dock = DockStyle.Fill;
            panelEnd.Dock = DockStyle.Fill;
            Root = TaxonUtils.Root;

            taxonMISC.OnClickImage += new Controls.TaxonMultiImageSoundControl.OnClickImageEventHandler(this.OutputImage_Click);
        }

        //---------------------------------------------------------------------------------
        public override TaxonTreeNode Root
        {
            set 
            { 
                _TaxonRef = value;
                if (_TaxonRef == null)
                    State = StateEnum.NoRoot;
                else 
                {
                    radioButtonRoot.Text = _TaxonRef.Desc.RefMainName;
                    if (_State == StateEnum.NoRoot || _State == StateEnum.Invalid)
                        State = StateEnum.Start;
                }
            }
        }
        
        //---------------------------------------------------------------------------------
        public override void OnSelectTaxon(TaxonTreeNode _taxon)
        {
            base.OnSelectTaxon(_taxon);
            if (_State == StateEnum.Start)
            {
                UpdateRadioButtonSelected();
                if (radioButtonSelected.Checked)
                {
                    _TaxonRef = _taxon == null ? TaxonUtils.Root : _taxon;
                    ComputeInputOutput();
                    DataToStartUI();
                }
            }
        }

        //---------------------------------------------------------------------------------
        public override void OnAvailableImagesChanged()
        {
            base.OnAvailableImagesChanged();
            if (_State == StateEnum.Start)
            {
                ComputeInputOutput();
                DataToStartUI();
            }
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Quizz"; }

        //---------------------------------------------------------------------------------
        enum StateEnum
        {
            Invalid, 
            NoRoot,
            Start, 
            InGame,
            End
        }
        StateEnum _State = StateEnum.Invalid;
        StateEnum State
        {
            get { return _State; }
            set
            {
                EndState();
                if (_State == value) return;
                _State = value;
                UpdatePanel();
                StartState();
            }
        }

        //---------------------------------------------------------------------------------
        void UpdatePanel()
        {
            panelStart.Visible = _State == StateEnum.Start;
            panelGame2.Visible = _State == StateEnum.InGame;
            panelEnd.Visible = _State == StateEnum.End;
        }

        //---------------------------------------------------------------------------------
        void StartState()
        {
            switch (_State)
            {
                case StateEnum.Start:   StartState_init(); break;
                case StateEnum.InGame: InGame_init(); break;
            }
        }

        //---------------------------------------------------------------------------------
        void EndState()
        {
            switch (_State)
            {
                case StateEnum.Start: StartState_end(); break;
                case StateEnum.InGame: InGame_end(); break;
            }
        }

        

        //---------------------------------------------------------------------------------
        enum DataEnum
        {
            Image = 0x1,
            Sound = 0x2,
            Latin = 0x4,
            French = 0x8
        }
        TaxonTreeNode _TaxonRef = null;
        int _Input = (int) DataEnum.Latin;
        int _Output = (int) DataEnum.Image;
        int _NumQuestions = 20;
        int _NumPropositions = 4;

        //=========================================================================================
        // Start Panel
        //

        //---------------------------------------------------------------------------------
        void StartState_init()
        {
            checkboxInputImage.Checked = (_Input & (int)DataEnum.Image) != 0;
            checkboxInputSound.Checked = (_Input & (int)DataEnum.Sound) != 0;
            checkboxInputLatin.Checked = (_Input & (int)DataEnum.Latin) != 0;
            checkboxInputFrench.Checked = (_Input & (int)DataEnum.French) != 0;
            checkboxOutputImage.Checked = (_Output & (int)DataEnum.Image) != 0;
            checkboxOutputSound.Checked = (_Output & (int)DataEnum.Sound) != 0;
            checkboxOutputLatin.Checked = (_Output & (int)DataEnum.Latin) != 0;
            checkboxOutputFrench.Checked = (_Output & (int)DataEnum.French) != 0;
            StartState_update();
        }

        void StartState_update()
        {
            _Input = 0;
            if (checkboxInputImage.Checked) _Input += (int)DataEnum.Image;
            if (checkboxInputSound.Checked) _Input += (int)DataEnum.Sound;
            if (checkboxInputLatin.Checked) _Input += (int)DataEnum.Latin;
            if (checkboxInputFrench.Checked) _Input += (int)DataEnum.French;

            _Output = 0;
            if (checkboxOutputImage.Checked) _Output += (int)DataEnum.Image;
            if (checkboxOutputSound.Checked) _Output += (int)DataEnum.Sound;
            if (checkboxOutputLatin.Checked) _Output += (int)DataEnum.Latin;
            if (checkboxOutputFrench.Checked) _Output += (int)DataEnum.French;

            ComputeInputOutput();
            DataToStartUI();
        }

        void StartState_end()
        {
            _TaxonInput = TaxonUtils.RandomTaxon(_TaxonInput, _NumQuestions);
        }

        //---------------------------------------------------------------------------------
        bool _SuspendEvents = false;
        void DataToStartUI()
        {
            _SuspendEvents = true;
            textBoxNumQuestions.Text = _NumQuestions.ToString();

            checkboxInputImage.Checked = (_Input & (int)DataEnum.Image) != 0;
            checkboxInputSound.Checked = (_Input & (int)DataEnum.Sound) != 0;
            checkboxInputFrench.Checked = (_Input & (int)DataEnum.French) != 0;
            checkboxInputLatin.Checked = (_Input & (int)DataEnum.Latin) != 0;

            checkboxOutputImage.Checked = (_Output & (int)DataEnum.Image) != 0;
            checkboxOutputSound.Checked = (_Output & (int)DataEnum.Sound) != 0;
            checkboxOutputFrench.Checked = (_Output & (int)DataEnum.French) != 0;
            checkboxOutputLatin.Checked = (_Output & (int)DataEnum.Latin) != 0;

            labelPropositionsNumber.Text = _NumPropositions.ToString();
            trackBarPropositions.Value = _NumPropositions;

            labelAvailableTaxon.Text = _TaxonInput.Count.ToString();

            radioButtonRoot.Checked = (_TaxonRef == TaxonUtils.Root);
            UpdateRadioButtonSelected();

            bool startEnable = true;
            if (_NumQuestions < 1 || _NumQuestions > 999) startEnable = false;
            if (_Input == _Output) startEnable = false;
            if (_NumPropositions < 2 || _NumPropositions > 8) startEnable = false;
            if (_TaxonInput.Count < _NumQuestions) startEnable = false;
            if (_TaxonInput.Count < _NumPropositions) startEnable = false;
            buttonStart.Enabled = startEnable;
            _SuspendEvents = false;
        }

        //---------------------------------------------------------------------------------
        void UpdateRadioButtonSelected()
        {
            radioButtonSelected.Enabled = TaxonUtils.SelectedTaxon() != null;
            radioButtonSelected.Text = TaxonUtils.SelectedTaxon() == null ? "no taxon selected" : TaxonUtils.SelectedTaxon().Desc.RefMainName;
            radioButtonSelected.Checked = !radioButtonRoot.Checked;
        }

        //---------------------------------------------------------------------------------
        void ComputeInputOutput()
        {
            if (_Output == 0 || _Input == 0)
            {
                _TaxonInput = new List<TaxonTreeNode>();
                _TaxonOutput = new List<TaxonTreeNode>();
                return;
            }

            List<TaxonTreeNode> Species = new List<TaxonTreeNode>();
            _TaxonRef.GetAllChildrenRecursively(Species, ClassicRankEnum.Espece);
            _TaxonRef.GetAllChildrenRecursively(Species, ClassicRankEnum.SousEspece);

            // new presentation:take only with images
            _TaxonOutput = new List<TaxonTreeNode>();
            foreach (TaxonTreeNode taxon in Species)
                if (taxon.Desc.HasImage) _TaxonOutput.Add(taxon);
            Species = _TaxonOutput;

            

            if ((_Output & (int)DataEnum.Image) != 0)
            {
                _TaxonOutput = new List<TaxonTreeNode>();
                foreach (TaxonTreeNode taxon in Species)
                    if (taxon.Desc.HasImage) _TaxonOutput.Add(taxon);
                Species = _TaxonOutput;
            }
            if ((_Output & (int)DataEnum.Sound) != 0 || (_Input & (int)DataEnum.Sound) != 0 )
            {
                _TaxonOutput = new List<TaxonTreeNode>();
                foreach (TaxonTreeNode taxon in Species)
                    if (taxon.Desc.HasSound) _TaxonOutput.Add(taxon);
                Species = _TaxonOutput;
            }
            if ((_Output & (int)DataEnum.French) != 0)
            {
                _TaxonOutput = new List<TaxonTreeNode>();
                foreach (TaxonTreeNode taxon in Species)
                    if (taxon.Desc.HasFrenchName) _TaxonOutput.Add(taxon);
                Species = _TaxonOutput;
            }
            _TaxonOutput = Species;


            if ((_Input & (int)DataEnum.Image) != 0)
            {
                _TaxonInput = new List<TaxonTreeNode>();
                foreach (TaxonTreeNode taxon in Species)
                    if (taxon.Desc.HasImage) _TaxonInput.Add(taxon);
                Species = _TaxonInput;
            }
            if ((_Input & (int)DataEnum.Sound) != 0)
            {
                _TaxonInput = new List<TaxonTreeNode>();
                foreach (TaxonTreeNode taxon in Species)
                    if (taxon.Desc.HasSound) _TaxonInput.Add(taxon);
                Species = _TaxonInput;
                // temp : select only output in image with sound
                /*List<TaxonTreeNode> oldOutput = _TaxonOutput;
                _TaxonOutput = new List<TaxonTreeNode>();
                foreach (TaxonTreeNode node in oldOutput)
                    if (_TaxonInput.Contains(node))
                        _TaxonOutput.Add(node);*/
            }
            if ((_Input & (int)DataEnum.French) != 0)
            {
                _TaxonInput = new List<TaxonTreeNode>();
                foreach (TaxonTreeNode taxon in Species)
                    if (taxon.Desc.HasFrenchName) _TaxonInput.Add(taxon);
                Species = _TaxonInput;
            }
            _TaxonInput = Species;
        }

        //---------------------------------------------------------------------------------
        private void textBoxNumQuestions_Leave(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            int result;
            if (int.TryParse(textBoxNumQuestions.Text, out result))
                _NumQuestions= result;
            DataToStartUI();
        }

        //---------------------------------------------------------------------------------
        private void textBoxNumQuestions_KeyDown(object sender, KeyEventArgs e)
        {
            if (_SuspendEvents) return;
            if (e.KeyCode == Keys.Return)
            {
                buttonStart.Focus();
                e.Handled = true;
            }
        }

        //---------------------------------------------------------------------------------
        private void checkboxInputImage_Click(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            if (checkboxInputImage.Checked)
                checkboxOutputImage.Checked = false;
            StartState_update();
        }
        private void checkboxInputSound_Click(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            if (checkboxInputSound.Checked)
                checkboxOutputSound.Checked = false;
            StartState_update();
        }
        private void checkboxInputLatin_Click(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            if (checkboxInputLatin.Checked)
                checkboxOutputLatin.Checked = false;
            StartState_update();
        }
        private void checkboxInputFrench_Click(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            if (checkboxInputFrench.Checked)
                checkboxOutputFrench.Checked = false;
            StartState_update();
        }
        
        //---------------------------------------------------------------------------------
        private void checkboxOutputImage_Click(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            if (checkboxOutputImage.Checked)
                checkboxInputImage.Checked = false;
            StartState_update();
        }
        private void checkboxOutputSound_Click(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            if (checkboxOutputSound.Checked)
                checkboxInputSound.Checked = false;
            StartState_update();
        }
        private void checkboxOutputLatin_Click(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            if (checkboxOutputLatin.Checked)
                checkboxInputLatin.Checked = false;
            StartState_update();
        }
        private void checkboxOutputFrench_Click(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            if (checkboxOutputFrench.Checked)
                checkboxInputFrench.Checked = false;
            StartState_update();
        }

        //---------------------------------------------------------------------------------
        private void trackBarPropositions_ValueChanged(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            _NumPropositions = trackBarPropositions.Value;
            DataToStartUI();
        }

        //---------------------------------------------------------------------------------
        private void radioButtonRoot_CheckedChanged(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            _TaxonRef = radioButtonRoot.Checked ? TaxonUtils.Root : TaxonUtils.SelectedTaxon();
            StartState_update();
        }

        private void radioButtonSelected_CheckedChanged(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            _TaxonRef = radioButtonRoot.Checked ? TaxonUtils.Root : TaxonUtils.SelectedTaxon();
            StartState_update();
        } 

        //---------------------------------------------------------------------------------
        private void buttonStart_Click(object sender, EventArgs e)
        {
            State = StateEnum.InGame;
        }

        //=========================================================================================
        // Game Panel
        //

        List<TaxonTreeNode> _TaxonInput;
        List<TaxonTreeNode> _TaxonOutput;
        TaxonTreeNode _CurrentInput;
        List<TaxonTreeNode> _CurrentOutput;
        TaxonTreeNode _SelectedOutput;
        int _CurrentQuestion = 0;
        int _QuestionAnswered = 0;
        int _Score;

        bool _QuestionPhase = true;

        //---------------------------------------------------------------------------------
        void InGame_init()
        {
            _CurrentQuestion = 0;
            _QuestionAnswered = 0;
            _Score = 0;

            splitContainer2.SplitterDistance = 350;

            taxonMISC.ImageNumber = _NumPropositions;
            taxonMISC.AllowTips = false;

            taxonISC.Width = 400;
            taxonISC.Height = 300;
            taxonISC.DisplayMode = VignetteDisplayParams.ModeEnum.LegendBorder;
            taxonISC.DisplayLatin = true;
            taxonISC.LatinVisible = (_Input & (int)DataEnum.Latin) != 0;
            taxonISC.DisplayFrench = true;
            taxonISC.FrenchVisible = (_Input & (int)DataEnum.French) != 0;
            taxonISC.DisplayImage = true;
            taxonISC.ImageVisible = (_Input & (int)DataEnum.Image) != 0;
            taxonISC.AllowSound = (_Input & (int)DataEnum.Sound) != 0;
            taxonISC.AllowTips = false;
            taxonISC.AllowContextualMenu = false;

            PrepareQuestion();
        }

        void InGame_end()
        {
            StopAllSounds();
        }

        //---------------------------------------------------------------------------------
        VignetteDisplayParams QuizzAnswerVignetteDisplayParams;
        VignetteDisplayParams QuizzSelectedAnswerVignetteDisplayParams;
        VignetteDisplayParams QuizzWrongAnswerVignetteDisplayParams;
        VignetteDisplayParams QuizzGoodAnswerVignetteDisplayParams;

        //---------------------------------------------------------------------------------
        void PrepareQuestion()
        {
            StopAllSounds();
            _QuestionPhase = true;

            _CurrentInput = TaxonUtils.RandomTaxon(_TaxonInput, 1)[0];
            _TaxonInput.Remove(_CurrentInput);
            if (_TaxonOutput.Count > _NumQuestions + _NumPropositions)
                _TaxonOutput.Remove(_CurrentInput);
            _CurrentQuestion = _NumQuestions - _TaxonInput.Count();
            
            _CurrentOutput = TaxonUtils.RandomTaxon(_TaxonOutput, _NumPropositions - 1);
            int index = TaxonUtils.random.Next(_CurrentOutput.Count + 1);
            if (index == _CurrentOutput.Count)
                _CurrentOutput.Add(_CurrentInput);
            else
                _CurrentOutput.Insert(index, _CurrentInput);

            _SelectedOutput = null;

            /*taxonMISC.AllowSound = (_Output & (int)DataEnum.Sound) != 0;
            taxonMISC.BorderColor = SystemColors.Control;*/

            QuizzAnswerVignetteDisplayParams = new VignetteDisplayParams(TaxonUtils.MyConfig.Options.OneTaxonAllImagesVignetteDisplayParams)
            {
                DisplayMode = VignetteDisplayParams.ModeEnum.LegendBorder,
                LatinVisible = (_Output & (int)DataEnum.Latin) != 0,
                FrenchVisible = (_Output & (int)DataEnum.French) != 0,
                ImageVisible = (_Output & (int)DataEnum.Image) != 0,
                FitInRectAboveThisSize = 0,
                BorderSize = 5,
                BorderColor = Color.White,
            };

            QuizzSelectedAnswerVignetteDisplayParams = new VignetteDisplayParams(TaxonUtils.MyConfig.Options.OneTaxonAllImagesVignetteDisplayParams)
            {
                DisplayMode = VignetteDisplayParams.ModeEnum.LegendBorder,
                LatinVisible = (_Output & (int)DataEnum.Latin) != 0,
                FrenchVisible = (_Output & (int)DataEnum.French) != 0,
                ImageVisible = (_Output & (int)DataEnum.Image) != 0,
                FitInRectAboveThisSize = 0,
                BorderSize = 5,
                BorderColor = Color.Green
            };

            QuizzWrongAnswerVignetteDisplayParams = new VignetteDisplayParams(TaxonUtils.MyConfig.Options.OneTaxonAllImagesVignetteDisplayParams)
            {
                DisplayMode = VignetteDisplayParams.ModeEnum.LegendBorder,
                FitInRectAboveThisSize = 0,
                LatinVisible = true,
                FrenchVisible = true,
                ImageVisible = true,
                BorderSize = 5,
                BorderColor = Color.Red
            };
            QuizzWrongAnswerVignetteDisplayParams.OnPainted += PaintRedCross;

            QuizzGoodAnswerVignetteDisplayParams = new VignetteDisplayParams(TaxonUtils.MyConfig.Options.OneTaxonAllImagesVignetteDisplayParams)
            {
                DisplayMode = VignetteDisplayParams.ModeEnum.LegendBorder,
                FitInRectAboveThisSize = 0,
                LatinVisible = true,
                FrenchVisible = true,
                ImageVisible = true,
                BorderSize = 5,
                BorderColor = Color.Green
            };

            taxonMISC.SetTaxonList(_CurrentOutput, QuizzAnswerVignetteDisplayParams);
            taxonISC.CurrentTaxon = _CurrentInput;

            buttonValidate.Visible = false;
            buttonContinue1.Visible = false;
            labelQuestion1.Text = "Question " + _CurrentQuestion.ToString() + " / " + _NumQuestions.ToString();
            labelScore2.Text = _Score.ToString();
        }

        //---------------------------------------------------------------------------------
        private void OutputImage_Click(object sender, Controls.TaxonMultiImageSoundControl.OnClickImageEventArgs e)
        {
            if (!(sender is Controls.TaxonMultiImageSoundControl)) return;
            if (!_QuestionPhase) return;
            if (e.Item == null || e.Item.ImageDisplayParams == null) return;

            // reset display params
            bool clickOnSelected = e.Item.ImageDisplayParams == QuizzSelectedAnswerVignetteDisplayParams;
            if (taxonMISC.GetItemsInfos(out List<TaxonMultiImageSoundControl.MyListItem> items, out int offset))
                items.ForEach(i => i.ImageDisplayParams = QuizzAnswerVignetteDisplayParams);

            if (clickOnSelected)
            {
                _SelectedOutput = null;
                buttonValidate.Visible = false;
            }
            else
            {
                e.Item.ImageDisplayParams = QuizzSelectedAnswerVignetteDisplayParams;
                _SelectedOutput = e.Item.Image.Node;
                buttonValidate.Visible = true;
                buttonValidate.Left = e.Item.Bounds.Left + (e.Item.Bounds.Width - buttonValidate.Width) / 2;
                buttonValidate.Top = e.Item.Bounds.Top + (e.Item.Bounds.Height - buttonValidate.Height) / 2;
            }
            taxonMISC.Invalidate();
        }

        //---------------------------------------------------------------------------------
        private void buttonQuit_Click(object sender, EventArgs e)
        {
            State = StateEnum.Start;
        }

        //---------------------------------------------------------------------------------
        private void buttonValidate_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _NumPropositions; i++)
            {
                if (taxonMISC.GetTaxon(i) == _CurrentInput)
                    taxonMISC.OutputItems[i].ImageDisplayParams = QuizzGoodAnswerVignetteDisplayParams;
                else
                    taxonMISC.OutputItems[i].ImageDisplayParams = QuizzWrongAnswerVignetteDisplayParams;
            }
            taxonMISC.Invalidate();

            _QuestionAnswered++;
            if (_SelectedOutput == _CurrentInput)
                _Score++;

            _QuestionPhase = false;
            labelScore2.Text = _Score.ToString();
            buttonValidate.Visible = false;
            buttonContinue1.Visible = true;
        }

        //---------------------------------------------------------------------------------
        private void buttonContinue_Click(object sender, EventArgs e)
        {
            if (_TaxonInput.Count == 0)
            {
                _State = StateEnum.End;
                string format = Localization.Manager.Get("_QuizzScore", "Your score is {0} / {1}");
                labelEndScore.Text = string.Format(format, _Score, _NumQuestions);
                UpdatePanel();
                return;
            }

            PrepareQuestion();
        }

        //---------------------------------------------------------------------------------
        void StopAllSounds()
        {
            // todo stop all sounds //
        }

        //---------------------------------------------------------------------------------
        static Pen redPen = new Pen(Color.FromArgb(50, Color.Red), 20);
        void PaintRedCross( object _sender, VignetteDisplayParams.OnPaintedArgs _args )
        {
            Rectangle R = _args.Bounds;
            _args.Graphics.DrawLine(redPen, new Point(R.Left, R.Top), new Point(R.Right, R.Bottom));
            _args.Graphics.DrawLine(redPen, new Point(R.Right, R.Top), new Point(R.Left, R.Bottom));
        }


        //=========================================================================================
        // End Panel
        //

        //---------------------------------------------------------------------------------
        private void buttonEnd_Click(object sender, EventArgs e)
        {
            State = StateEnum.Start;
        }
    }
}
