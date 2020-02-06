using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeOfLife.Controls;

namespace TreeOfLife
{

    [Description("A hangman game")]
    [DisplayName("Hangman")]
    [Controls.IconAttribute("TaxonGameHangman")]
    public partial class TaxonGameHangman : Controls.TaxonControl
    {
        //---------------------------------------------------------------------------------
        public TaxonGameHangman()
        {
            InitializeComponent();
            UpdatePanel();
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Hangman"; }

        //---------------------------------------------------------------------------------
        enum StateEnum
        {
            Start, InGame, End
        }
        StateEnum _State = StateEnum.Start;

        //---------------------------------------------------------------------------------
        void UpdatePanel()
        {
            panelStart.Visible = _State == StateEnum.Start;
            panelStart.Dock = DockStyle.Fill;
            panelGame.Visible = (_State == StateEnum.InGame || _State == StateEnum.End);
            panelGame.Dock = DockStyle.Fill;

            if (_State == StateEnum.Start)
                DataToStartUI();
        }

        //---------------------------------------------------------------------------------
        enum DataEnum { Latin, French }
        DataEnum _Input = DataEnum.Latin;

        //=========================================================================================
        // Start Panel
        //

        //---------------------------------------------------------------------------------
        bool _SuspendEvents = false;
        void DataToStartUI()
        {
            _SuspendEvents = true;
            radioButtonFrench.Checked = _Input == DataEnum.French;
            radioButtonLatin.Checked = _Input == DataEnum.Latin;
            buttonStart.Enabled = true;
            _SuspendEvents = false;
        }

        private void radioButtonInputLatin_CheckedChanged(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            _Input = DataEnum.Latin;
            DataToStartUI();
        }

        private void radioButtonInputFrench_CheckedChanged(object sender, EventArgs e)
        {
            if (_SuspendEvents) return;
            _Input = DataEnum.French;
            DataToStartUI();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            List<TaxonTreeNode> Species = new List<TaxonTreeNode>();
            TaxonUtils.Root.GetAllChildrenRecursively(Species, ClassicRankEnum.Espece);
            TaxonUtils.Root.GetAllChildrenRecursively(Species, ClassicRankEnum.SousEspece);

            _TaxonInput = new List<TaxonTreeNode>();
            foreach (TaxonTreeNode taxon in Species)
                if (taxon.Desc.HasImage) _TaxonInput.Add(taxon);
            
            if (_Input == DataEnum.French)
            {
                Species = _TaxonInput;
                _TaxonInput = new List<TaxonTreeNode>();
                foreach (TaxonTreeNode taxon in Species)
                    if (taxon.Desc.HasFrenchName) _TaxonInput.Add(taxon);
            }

            if (Species.Count == 0)
            {
                MessageBox.Show("No taxon found for this game", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _CurrentInput = TaxonUtils.RandomTaxon(_TaxonInput, 1)[0];
            _State = StateEnum.InGame;
            UpdatePanel();
            InitGame();
        }

        //=========================================================================================
        // Game Panel
        //
        List<TaxonTreeNode> _TaxonInput;
        TaxonTreeNode _CurrentInput;

        string DisplayString = "";
        string SoluceString = "";
        string CleanString = "";

        int LostCount = 0;

        //---------------------------------------------------------------------------------
        void InitGame()
        {
            string name = (_Input == DataEnum.Latin) ? _CurrentInput.Desc.RefMainName : _CurrentInput.Desc.FrenchMultiName.Main;
            
            taxonImageControlInput.DisplayMode = VignetteDisplayParams.ModeEnum.Brut;
            taxonImageControlInput.CurrentTaxon = _CurrentInput;

            labelState.Text = "Guess a letter ...";

            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(name);
            string temp = System.Text.Encoding.UTF8.GetString(tempBytes).ToUpper();
            if (temp.Length != name.Length)
                temp = name.ToUpper();

            SoluceString = "";
            CleanString = "";
            DisplayString = "";
            LostCount = 0;

            bool spacePossible = false;
            for (int i = 0; i < temp.Length; i++)
            {
                bool add = false;
                if (temp[i] == ' ')
                {
                    add = spacePossible;
                    spacePossible = false;
                }
                else if (temp[i] >= 'A' && temp[i] <= 'Z')
                {
                    add = true;
                    spacePossible = true;
                }
                if (add)
                {
                    SoluceString += name[i];
                    CleanString += temp[i];
                    DisplayString += (temp[i] == ' ') ? temp[i] : '-';
                }
            }

            textBox1.Text = DisplayString;
            setImage();
        }

        //---------------------------------------------------------------------------------
        private void setImage()
        {
            switch (LostCount)
            {
                case 0: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman00; break;
                case 1: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman01; break;
                case 2: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman02; break;
                case 3: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman03; break;
                case 4: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman04; break;
                case 5: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman05; break;
                case 6: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman06; break;
                case 7: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman07; break;
                case 8: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman08; break;
                case 9: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman09; break;
                case 10: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman10; break;
                default: pictureBox1.Image = global::TreeOfLife.Properties.Resources.hangman11; break;
            }
            pictureBox1.Refresh();
        }

        //---------------------------------------------------------------------------------
        private void buttonQuit_Click(object sender, EventArgs e)
        {
            _State = StateEnum.Start;
            UpdatePanel();
        }

        //---------------------------------------------------------------------------------
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (_State == StateEnum.InGame)
            {
                if (keyData >= Keys.A && keyData <= Keys.Z)
                {
                    bool win = true;
                    bool good = false;
                    string oldDisplayString = DisplayString;
                    DisplayString = "";
                    for (int i = 0; i < CleanString.Length; i++)
                    {
                        if (CleanString[i] == (char)keyData)
                        {
                            DisplayString += SoluceString[i];
                            good = true;
                        }
                        else
                            DisplayString += oldDisplayString[i];
                        if (DisplayString[i] == '-')
                            win = false;
                    }

                    textBox1.Text = DisplayString;

                    if (win)
                    {
                        _State = StateEnum.End;
                        UpdatePanel();
                        EndGame();
                    }
                    else if (!good)
                    {
                        LostCount++;
                        setImage();
                        if (LostCount == 11)
                        {
                            _State = StateEnum.End;
                            UpdatePanel();
                            EndGame();
                        }
                    }

                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        //=========================================================================================
        // End Panel
        //

        //---------------------------------------------------------------------------------
        private void EndGame()
        {
            labelState.Text = (LostCount == 11) ? "Pendu !!" : "Bien joué !!";
            textBox1.Text = SoluceString;
        }

        //---------------------------------------------------------------------------------
        private void buttonEnd_Click(object sender, EventArgs e)
        {
            _State = StateEnum.Start;
            UpdatePanel();
            return;
        }

        

        
    }
}
