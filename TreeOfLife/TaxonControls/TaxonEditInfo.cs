using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TreeOfLife.TaxonDialog;

namespace TreeOfLife
{
    [Description("Edition of taxon content")]
    [DisplayName("Edit")]
    [Controls.IconAttribute("TaxonEditInfo")]
    public partial class TaxonEditInfo : Controls.TaxonControl
    {
        //---------------------------------------------------------------------------------
        public TaxonEditInfo()
        {
            InitializeComponent();
            RedListCategoryInit();
            ShowInfo();
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Edit"; }

        //---------------------------------------------------------------------------------
        public override void OnTaxonChanged(object sender, TaxonTreeNode _taxon)
        {
            if (sender == this) return;
            if (_taxon != Edited) return;
            ShowSimpleInfos();
        }

        //---------------------------------------------------------------------------------
        public override void OnSelectTaxon(TaxonTreeNode _taxon)
        {
            if (checkBoxLock.Checked) return;
            Edited = _taxon;
        }

        //---------------------------------------------------------------------------------
        //const int WM_KEYDOWN = 0x100;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            //Console.WriteLine(ActiveControl.ToString());
            if ((Edited != null) && !(ActiveControl is TextBox))
            {
                if (m.Msg == 0x100 && (Keys)m.WParam == Keys.V && (ModifierKeys & (Keys.Control | Keys.Shift)) != 0)
                {
                    Image bmp = Clipboard.GetImage();
                    if (bmp != null)
                    {
                        AddImage(bmp);
                        return true;
                    }
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

        //---------------------------------------------------------------------------------
        TaxonTreeNode _Edited = null;
        public TaxonTreeNode Edited
        {
            get { return _Edited; }
            set
            {
                if (_Edited == value) return;
                _Edited = value;
                if (_Edited != null)
                {
                    originalName = Edited.Desc.RefAllNames;
                    originalFrenchName = Edited.Desc.FrenchAllNames;
                    originalClassicRank = Edited.Desc.ClassicRank;
                    originalFlags = Edited.Desc.Flags;
                }
                ShowInfo();
            }
        }

        //---------------------------------------------------------------------------------
        public void AddImage(Image _image)
        {
            if (_image == null) return;

            NewImage dlg = new NewImage(Edited, _image);

            // try creating default image (faster create image without UI ... )
            if ((ModifierKeys & Keys.Shift) == 0 && dlg.CreateWithoutUI())
            {
                taxonListImage1.UpdateUI();
            }
            else if (dlg.ShowDialog() == DialogResult.OK)
            {
                taxonListImage1.UpdateUI();
                TaxonImageDesc imageDesc = dlg.GetImageDesc();
                if (imageDesc != null)
                    taxonListImage1.SetImage(imageDesc);
            }
        }

        //---------------------------------------------------------------------------------
        string originalName;
        string originalFrenchName;
        ClassicRankEnum originalClassicRank;
        uint originalFlags;

        //---------------------------------------------------------------------------------
        void EnableUI(bool _show)
        {
            textBoxName.Enabled = _show;
            buttonRefSyn.Enabled = _show;
            textBoxFrenchName.Enabled = _show;
            buttonFrenchSyn.Enabled = _show;
            textBoxClassicRank.Enabled = _show;
            checkBoxFlagExtinctInherited.Visible = _show;
            checkBoxFlagUseID.Visible = _show;
            buttonSave.Enabled = _show;
            buttonReset.Enabled = _show;
            buttonGoogle.Enabled = _show;
            buttonWikiCommons.Enabled = _show;
            buttonOTT.Enabled = _show;
            buttonClassicRank.Enabled = _show;
            taxonListImage1.Enabled = _show;

            splitContainerComments.Visible = false;
        }

        void ShowNothing()
        {
            textBoxName.Text = "";
            textBoxFrenchName.Text = "";
            textBoxClassicRank.Text = "";
            checkBoxFlagExtinctInherited.Checked = false;
            checkBoxFlagUseID.Checked = false;
            taxonListImage1.Taxon = null;
            Comments.MainTaxon = null;
            buttonOTT.Text = "";
            RedListCategoryUpdate();
        }

        void ShowSimpleInfos()
        {
            textBoxName.Text = Edited.Desc.RefAllNames;
            textBoxFrenchName.Text = Edited.Desc.FrenchAllNames;
            textBoxClassicRank.Text = VinceToolbox.Helpers.enumHelper.GetEnumDescription(Edited.Desc.ClassicRank);
            checkBoxFlagExtinctInherited.Checked = Edited.Desc.HasFlag(FlagsEnum.ExtinctInherited);
            checkBoxFlagUseID.Checked = Edited.Desc.HasFlag(FlagsEnum.IncludeIdInFilenames);
            UpdateButtonID();
            RedListCategoryUpdate();
        }

        void ShowInfo()
        {
            if (Edited == null)
            {
                EnableUI(false);
                ShowNothing();
                return;
            }

            EnableUI(true);
            ShowSimpleInfos();
            taxonListImage1.Taxon = Edited;
            Comments.MainTaxon = null;
            Comments.MainTaxon = Edited;
        }

        void UpdateButtonID()
        {
            if (Edited.Desc.OTTID == 0) buttonOTT.Text = "Create Id";
            else buttonOTT.Text = TaxonIds.IdDesc(Edited.Desc.OTTID);
        }

        //---------------------------------------------------------------------------------
        List<CheckBox> RedListCategoryCheckBoxes;
        void RedListCategoryInit()
        {
            RedListCategoryCheckBoxes = new List<CheckBox>();
            checkBoxNE.Tag = RedListCategoryEnum.NotEvaluated;
            RedListCategoryCheckBoxes.Add(checkBoxNE);
            checkBoxDD.Tag = RedListCategoryEnum.DataDeficient;
            RedListCategoryCheckBoxes.Add(checkBoxDD);
            checkBoxLC.Tag = RedListCategoryEnum.LeastConcern;
            RedListCategoryCheckBoxes.Add(checkBoxLC);
            checkBoxNT.Tag = RedListCategoryEnum.NearThreatened;
            RedListCategoryCheckBoxes.Add(checkBoxNT);
            checkBoxVU.Tag = RedListCategoryEnum.Vulnerable;
            RedListCategoryCheckBoxes.Add(checkBoxVU);
            checkBoxEN.Tag = RedListCategoryEnum.Endangered;
            RedListCategoryCheckBoxes.Add(checkBoxEN);
            checkBoxCR.Tag = RedListCategoryEnum.CriticallyEndangered;
            RedListCategoryCheckBoxes.Add(checkBoxCR);
            checkBoxEW.Tag = RedListCategoryEnum.ExtinctInTheWild;
            RedListCategoryCheckBoxes.Add(checkBoxEW);
            checkBoxEX.Tag = RedListCategoryEnum.Extinct;
            RedListCategoryCheckBoxes.Add(checkBoxEX);
            foreach (CheckBox cb in RedListCategoryCheckBoxes)
                RedListCategoryExt.SetupToggleButton((RedListCategoryEnum)cb.Tag, cb);
        }

        void RedListCategoryUpdate()
        {
            foreach (CheckBox cb in RedListCategoryCheckBoxes)
            {
                cb.Checked = Edited == null ? false : ((RedListCategoryEnum)cb.Tag == Edited.Desc.RedListCategory);
                RedListCategoryExt.SetupToggleButton((RedListCategoryEnum)cb.Tag, cb);
            }
        }

        private void RedListCategoryClick(object sender, EventArgs e)
        {
            if (Edited == null) return;
            Edited.Desc.RedListCategory = (RedListCategoryEnum)(sender as CheckBox).Tag;
            RedListCategoryUpdate();
        }

        //---------------------------------------------------------------------------------
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            TaxonUtils.MainWindow.Save();
        }

        //---------------------------------------------------------------------------------
        private void ButtonReset_Click(object sender, EventArgs e)
        {
            Edited.Desc.RefMultiName = new Helpers.MultiName(originalName);
            Edited.Desc.FrenchMultiName = new Helpers.MultiName(originalFrenchName);
            Edited.Desc.ClassicRank = originalClassicRank;
            Edited.Desc.Flags = originalFlags;
            ShowSimpleInfos();
        }

        //---------------------------------------------------------------------------------
        private void ButtonGoogle_Click(object sender, EventArgs e)
        {
            string searchItem = Edited.Desc.RefMultiName.Main;
            searchItem.Replace(" ", "%20");
            Process.Start("http://google.com/search?q=" + searchItem);
        }

        //---------------------------------------------------------------------------------
        private void ButtonWikiCommons_Click(object sender, EventArgs e)
        {
            ContextMenu menu = new ContextMenu();
            menu.MenuItems.Add("Web search", OnSearchOnWikiCommons);
            menu.MenuItems.Add("Get images", OnGetWikiCommonsImages);
            menu.Show(this, this.PointToClient(Cursor.Position));
        }

        private void OnSearchOnWikiCommons(object sender, EventArgs e)
        {
            string searchItem = Edited.Desc.RefMultiName.Main;
            searchItem.Replace(" ", "_");
            System.Diagnostics.Process.Start("https://commons.wikimedia.org/wiki/" + searchItem);
        }

        private void OnGetWikiCommonsImages(object sender, EventArgs e)
        {
            MessageBox.Show("Work in progress ! ");
            /*string searchItem = Edited.Desc.RefMultiName.Main;
            searchItem.Replace(" ", "_");
            string url = "https://commons.wikimedia.org/wiki/" + searchItem;
            try
            {
                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString(url);
                    Console.WriteLine(s);
                }
            }
            catch(System.Exception ex)
            {
                MessageBox.Show("Error while trying to get wiki page:\n  " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
        }


        //---------------------------------------------------------------------------------
        private void ButtonOTT_Click(object sender, EventArgs e)
        {
            if (Edited.Desc.OTTID == 0)
            {
                string message = "Create TOL ID for " + (string.IsNullOrEmpty(Edited.Desc.RefMultiName.Main) ? "that taxon" : Edited.Desc.RefMultiName.Main) + "?";
                if (MessageBox.Show(message, "Create ID", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;
                Edited.Desc.OTTID = TaxonIds.GetUnusedTolId(TaxonUtils.OriginalRoot);
                UpdateButtonID();
            }
            else if (!TaxonIds.IsTolID(Edited.Desc.OTTID))
            {
                string url = "https://tree.opentreeoflife.org/opentree/argus/ottol@" + Edited.Desc.OTTID.ToString();
                Process.Start(url);
            }
        }

        //---------------------------------------------------------------------------------
        private void ButtonClassicRank_Click(object sender, EventArgs e)
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.SuspendLayout();

            ToolStripMenuItem item;
            string currentCategory = "";
            foreach (ClassicRankEnum value in VinceToolbox.Helpers.enumHelper.GetValues<ClassicRankEnum>())
            {
                string category = VinceToolbox.Helpers.enumHelper.GetEnumCategory(value);
                if (currentCategory != "" && currentCategory != category)
                    contextMenuStrip.Items.Add(new ToolStripSeparator());
                currentCategory = category;

                item = new System.Windows.Forms.ToolStripMenuItem(VinceToolbox.Helpers.enumHelper.GetEnumDescription(value), null, OnSetClassicRank);
                item.Tag = value;
                if (value == Edited.Desc.ClassicRank) item.Checked = true;
                contextMenuStrip.Items.Add(item);
            }
            contextMenuStrip.RightToLeft = RightToLeft.Yes;
            contextMenuStrip.ResumeLayout();
            contextMenuStrip.Show(buttonClassicRank, new Point(-contextMenuStrip.Width + buttonClassicRank.Width, buttonClassicRank.Height));
        }

        //---------------------------------------------------------------------------------
        private void OnSetClassicRank(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item)) return;
            if (Edited.Desc.ClassicRank == (ClassicRankEnum)item.Tag) return;
            Edited.Desc.ClassicRank = (ClassicRankEnum)item.Tag;
            ShowSimpleInfos();
            CallOnEditedTaxonChanged();
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxFlagExtinctInherited_CheckedChanged(object sender, EventArgs e)
        {
            if (Edited == null) return;
            if (Edited.Desc.HasFlag(FlagsEnum.ExtinctInherited) == checkBoxFlagExtinctInherited.Checked) return;
            Edited.Desc.SetFlagValue(FlagsEnum.ExtinctInherited, checkBoxFlagExtinctInherited.Checked);
            ShowSimpleInfos();
            taxonListImage1.UpdateUI();
            CallOnEditedTaxonChanged();
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxFlagUseID_CheckedChanged(object sender, EventArgs e)
        {
            if (Edited == null) return;
            if (Edited.Desc.HasFlag(FlagsEnum.IncludeIdInFilenames) == checkBoxFlagUseID.Checked) return;

            if (checkBoxFlagUseID.Checked && Edited.Desc.OTTID == 0)
            {
                string message = "You have to create a new ID first";
                MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                checkBoxFlagUseID.Checked = Edited.Desc.HasFlag(FlagsEnum.IncludeIdInFilenames);
                return;
            }

            bool hasFile = (Edited.Desc.Images != null && Edited.Desc.Images.Count != 0) || (TaxonComments.CommentFile(Edited) != null);
            if (hasFile)
            {
                string message = "Taxon have already some associated files";
                message += "\nchanging that flag may result in loosing these association";
                message += "\n\nContinue anyway ?";
                if (MessageBox.Show(message, "Continue ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    checkBoxFlagUseID.Checked = Edited.Desc.HasFlag(FlagsEnum.IncludeIdInFilenames);
                    return;
                }
            }
            
            Edited.Desc.SetFlagValue(FlagsEnum.IncludeIdInFilenames, checkBoxFlagUseID.Checked);
            TaxonComments.Manager.CleanCommentInMemory(0);
            ShowInfo();
            CallOnEditedTaxonChanged();
        }

        //---------------------------------------------------------------------------------
        private void TextBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                textBoxFrenchName.Focus();
                e.Handled = true;
            }
        }

        //---------------------------------------------------------------------------------
        private void TextBoxName_Leave(object sender, EventArgs e)
        {
            if (Edited.Desc.RefAllNames == textBoxName.Text) return;
            Edited.Desc.RefMultiName = new Helpers.MultiName(textBoxName.Text);
            ShowSimpleInfos();
            CallOnEditedTaxonChanged();
        }

        //---------------------------------------------------------------------------------
        private void ButtonRefSyn_Click(object sender, EventArgs e)
        {
            SynonymsDialog dlg = new SynonymsDialog(Edited.Desc.RefMultiName);
            if (dlg.ShowDialog() == DialogResult.OK)
                Edited.Desc.RefMultiName = dlg.Result;
            ShowSimpleInfos();
        }

        //---------------------------------------------------------------------------------
        private void TextBoxFrenchName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                textBoxClassicRank.Focus();
                e.Handled = true;
            }
        }

        //---------------------------------------------------------------------------------
        private void TextBoxFrenchName_Leave(object sender, EventArgs e)
        {
            if (Edited.Desc.FrenchAllNames == textBoxFrenchName.Text) return;
            Edited.Desc.FrenchMultiName = new Helpers.MultiName(textBoxFrenchName.Text);
            ShowSimpleInfos();
            CallOnEditedTaxonChanged();
        }

        //---------------------------------------------------------------------------------
        private void ButtonFrenchSyn_Click(object sender, EventArgs e)
        {
            SynonymsDialog dlg = new SynonymsDialog(Edited.Desc.FrenchMultiName);
            if (dlg.ShowDialog() == DialogResult.OK)
                Edited.Desc.FrenchMultiName = dlg.Result;
            ShowSimpleInfos();
        }

        //---------------------------------------------------------------------------------
        public class OnTaxonChangedArgs : EventArgs
        {
            public TaxonTreeNode taxon;
        }

        public delegate void OnTaxonChangedEventHandler(object sender, OnTaxonChangedArgs e);
        public event OnTaxonChangedEventHandler OnEditedTaxonChanged = null;

        private void CallOnEditedTaxonChanged()
        {
            if (OnEditedTaxonChanged == null) return;
            OnTaxonChangedArgs e = new OnTaxonChangedArgs() { taxon = Edited };
            OnEditedTaxonChanged(this, e);
        }

        //---------------------------------------------------------------------------------
        private void TaxonEditInfo_DragDrop(object sender, DragEventArgs e)
        {
            AddImage(VinceToolbox.Helpers.DragHelper.GetImage(e));
        }

        //---------------------------------------------------------------------------------
        private void TaxonEditInfo_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (Edited != null)
            {
                if (VinceToolbox.Helpers.DragHelper.HasImage(e))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
        }

        //---------------------------------------------------------------------------------
        private void Comments_OnDocumentCompleted(object sender, EventArgs e)
        {
            if (Edited == null || Comments.MainTaxon == null || Comments.MainTaxon != Edited)
                return;
            
            bool hasContent = Comments.HasContent();
            splitContainerComments.Panel1Collapsed = !hasContent;
            splitContainerComments.Panel2Collapsed = hasContent;
            splitContainerComments.Visible = true;
        }

        //---------------------------------------------------------------------------------
        private void ButtonCreateComment_Click(object sender, EventArgs e)
        {
            if (Edited == null) return;
            if (TaxonComments.CommentFile( Edited ) != null) return;
            TaxonComments.CommentFileCreateResult result = TaxonComments.CommentFileCreate(Edited);
            if (result == TaxonComments.CommentFileCreateResult.Success)
            {
                splitContainerComments.Visible = false;
                Comments.RefreshContent(true);
                return;
            }

            string message  = "Creation failed:\n    ";
            if (result == TaxonComments.CommentFileCreateResult.ExistsAlready)
                message += "a comment file already exists";
            else if (result == TaxonComments.CommentFileCreateResult.NoNameAndID)
                message += "taxon as no name or id, click on ID button to create a new TOL ID";
            else if (result == TaxonComments.CommentFileCreateResult.NoCollection)
                message += "No available collection, edit collections before creating comments";
            else
                message += "unknown reason";
            Loggers.WriteError(LogTags.Comment, message);
        }

        //---------------------------------------------------------------------------------
        /*private void splitContainerData_Panel2_Resize(object sender, EventArgs e)
        {
            int w = splitContainerComments.Panel2.Width;
            int h = splitContainerComments.Panel2.Height;

            if (w < 150)
            {
                buttonCreateComment.Left = 5;
                buttonCreateComment.Width = w - 10;
            }
            else
            {
                buttonCreateComment.Width = 150;
                buttonCreateComment.Left = (w - 150) / 2;
            }

            buttonCreateComment.Top = (h - buttonCreateComment.Height) / 2;
        }*/

    }
}

