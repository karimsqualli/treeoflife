using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using TreeOfLife.GUI;

namespace TreeOfLife
{
    [Description("Edition of tags")]
    [DisplayName("Tags")]
    [Controls.IconAttribute("TaxonTags")]
    public partial class TaxonTags : Controls.TaxonControl
    {
        //---------------------------------------------------------------------------------
        public TaxonTags()
        {
            TaxonUtils.CurrentFilters.OnFiltersChanged += OnFiltersChanged;
            TaxonUtils.OnOriginalRootChanged += OnOriginalRootChanged;
            TaxonUtils.UserFilter.OnListChanged += OnListChanged;
            TaxonUtils.UserFilter.OnListModifiedChanged += OnListModifiedChanged;
            InitializeComponent();
            InitFilterButton();
            UpdateFilterButtonState();
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Tags"; }

        bool AutoAddSelectedTaxon = false;
        TaxonFilterListInFile Filter { get => TaxonUtils.UserFilter; }

        //---------------------------------------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            taxonListBox.DataSource = Filter.List;
            UpdateUI();
        }

        //---------------------------------------------------------------------------------
        protected override void OnClose()
        {
            TaxonUtils.CurrentFilters.OnFiltersChanged -= OnFiltersChanged;
            TaxonUtils.OnOriginalRootChanged -= OnOriginalRootChanged;
            TaxonUtils.UserFilter.OnListChanged -= OnListChanged;
            TaxonUtils.UserFilter.OnListModifiedChanged -= OnListModifiedChanged;
            base.OnClose();
        }

        //---------------------------------------------------------------------------------
        public override void OnSelectTaxon(TaxonTreeNode _taxon)
        {
            _taxon = _taxon?.GetOriginal();
            if (AutoAddSelectedTaxon)
                Filter.AddToList(_taxon);

            if (Filter.List.Contains(_taxon))
                taxonListBox.SelectedItem = _taxon;
            else
                taxonListBox.SelectedItem = null;
        }

        //---------------------------------------------------------------------------------
        public override void OnTaxonChanged(object sender, TaxonTreeNode _taxon)
        {
            RefreshList();
        }

        //---------------------------------------------------------------------------------
        public void OnFiltersChanged(object sender, EventArgs e)
        {
            UpdateFilterButtonState();
        }

        //---------------------------------------------------------------------------------
        public void OnOriginalRootChanged(object sender, EventArgs e)
        {
            Filter.SetCurrentFilter(null, false, true);
        }

        //---------------------------------------------------------------------------------
        public void OnListChanged(object sender, EventArgs e)
        {
            RefreshList();
            UpdateUI();
        }

        //---------------------------------------------------------------------------------
        public void OnListModifiedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        //=================================================================================
        // Misc UI functions
        //

        private void RefreshList()
        {
            taxonListBox.DataSource = null;
            taxonListBox.DataSource = Filter.List;
        }

        private void UpdateUI()
        {
            buttonSaveCurrentFilter.Enabled = Filter.CanSaveCurrentOrAs;
            labelTag.Text = Filter.CurrentFile == null ? Localization.Manager.Get( "_NoAssociatedFiles", "<no associated file>") : Filter.CurrentName;
        }

        //=================================================================================
        // Menu command
        //

        //---------------------------------------------------------------------------------
        private void FileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            closeToolStripMenuItem.Enabled = Filter.CurrentFile != null;
            saveToolStripMenuItem.Enabled = Filter.CanSaveCurrentFile;
            saveAsToolStripMenuItem.Enabled = Filter.CanSaveAs;
            exportToolStripMenuItem.Enabled = (Filter.List.Count > 0);
            importToolStripMenuItem.Enabled = TaxonUtils.OriginalRoot != null;
            batchImportToolStripMenuItem.Enabled = TaxonUtils.OriginalRoot != null;
        }

        //---------------------------------------------------------------------------------
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonUtils.UserFilter.BrowseAndOpen();
        }

        //---------------------------------------------------------------------------------
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter.Clear();
        }

        //---------------------------------------------------------------------------------
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter.SaveCurrentFile();
        }

        //---------------------------------------------------------------------------------
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter.SaveAs();
        }

        //---------------------------------------------------------------------------------
        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Filter.IgnoreUnsavedModification())
                return;

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt",
                Multiselect = false,
                AddExtension = true
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (!File.Exists(ofd.FileName)) return;

                TaxonList.ImportFileResult result = TaxonList.ImportFile(ofd.FileName, TaxonUtils.OriginalRoot );

                string message = "Import " + Path.GetFileName(ofd.FileName) + ": \n";
                message += String.Format("    taxons found: {0}\n", result.TaxonsFound);
                message += String.Format("    taxons not found: {0}\n", result.TaxonNotFound);
                message += String.Format("for more details, look at " + result.LogFilename + " file");

                Loggers.WriteInformation(LogTags.Tags, message);

            Filter.SetCurrentFilter(Path.ChangeExtension(ofd.FileName, ".lot"), false, true);
            Filter.SetList(result.List);
        }

        //---------------------------------------------------------------------------------
        private void BatchImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TagBatchImportDialog dlg = new TagBatchImportDialog { StartPosition = FormStartPosition.CenterScreen };
            dlg.ShowDialog();
        }


        //---------------------------------------------------------------------------------
        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Filter.List == null || Filter.List.Count == 0) return;

            string filename = "TagsExport.txt";
            if (!string.IsNullOrWhiteSpace(Filter.CurrentFile))
            {
                filename = Path.GetDirectoryName(Filter.CurrentFile);
                filename += Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(Filter.CurrentFile);
                filename += "_export.txt";
            }
            TaxonDialog.ExportDialog dlg = new TaxonDialog.ExportDialog(Filter.List, filename);
            dlg.ShowDialog();
        }

        //---------------------------------------------------------------------------------
        private void EditToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            autoaddToolStripMenuItem.Checked = AutoAddSelectedTaxon;
            addSelectedTaxonToolStripMenuItem.Enabled = TaxonUtils.SelectedTaxon() != null;
            deleteToolStripMenuItem.Enabled = taxonListBox != null;
            clearToolStripMenuItem.Enabled = Filter.List.Count > 0;
        }

        //---------------------------------------------------------------------------------
        private void AddSelectedTaxonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter.AddToList(TaxonUtils.SelectedTaxon());
        }

        //---------------------------------------------------------------------------------
        private void AddAllSpeciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<TaxonTreeNode> Species = new List<TaxonTreeNode>();
            TaxonUtils.SelectedTaxon().GetAllChildrenRecursively(Species, ClassicRankEnum.Espece);

            if (Species.Count > 100)
            {
                string message = "this operation will add " + Species.Count + " taxon in list\n\nProceed ?";
                DialogResult result = MessageBox.Show(message, "Add taxons in tag", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
            }

            Filter.AddToList(Species);
        }

        //---------------------------------------------------------------------------------
        private void AutoaddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoAddSelectedTaxon = !AutoAddSelectedTaxon;
        }

        //---------------------------------------------------------------------------------
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter.RemoveFromList(taxonListBox.SelectedItem as TaxonTreeNode);
        }

        //---------------------------------------------------------------------------------
        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Filter.IgnoreUnsavedModification())
                Filter.ClearList(true);
        }

        //---------------------------------------------------------------------------------
        private void FilterToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            bool filterOn = Filter.FilterIsActive;
            onToolStripMenuItem.Enabled = Filter.FilterIsActivable;
            onToolStripMenuItem.Checked = filterOn;
            offToolStripMenuItem.Enabled = filterOn;
            offToolStripMenuItem.Checked = !filterOn;
        }

        //---------------------------------------------------------------------------------
        private void OnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TaxonUtils.CurrentFilters.GetFilter<TaxonFilterTag>() != null) return;
            TaxonFilterTag filter = new TaxonFilterTag(Filter.List);
            TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.AddFilter(filter));
        }

        //---------------------------------------------------------------------------------
        private void OffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.RemoveFilter<TaxonFilterTag>());
        }

        //=================================================================================
        // some other UI interaction
        //
        private void ButtonSaveCurrentFilter_Click(object sender, EventArgs e)
        {
            Filter.SaveCurrentOrAs();
        }

        

        //---------------------------------------------------------------------------------
        Dictionary<CheckBox, RedListCategoryEnum> RedListCategoryCheckBoxes;
        private void InitFilterButton()
        {
            RedListCategoryCheckBoxes = new Dictionary<CheckBox, RedListCategoryEnum>
            {
                [checkBoxNE] = RedListCategoryEnum.NotEvaluated,
                [checkBoxDD] = RedListCategoryEnum.DataDeficient,
                [checkBoxLC] = RedListCategoryEnum.LeastConcern,
                [checkBoxNT] = RedListCategoryEnum.NearThreatened,
                [checkBoxVU] = RedListCategoryEnum.Vulnerable,
                [checkBoxEN] = RedListCategoryEnum.Endangered,
                [checkBoxCR] = RedListCategoryEnum.CriticallyEndangered,
                [checkBoxEW] = RedListCategoryEnum.ExtinctInTheWild,
                [checkBoxEX] = RedListCategoryEnum.Extinct
            };
            foreach (var cb in RedListCategoryCheckBoxes)
                RedListCategoryExt.SetupToggleButton(cb.Value, cb.Key);
        }

        //---------------------------------------------------------------------------------
        private void UpdateFilterButtonState()
        {
            checkBoxDisplayUnnamed.Checked = !TaxonUtils.CurrentFilters.HasFilter<TaxonFilterUnnamed>();

            ushort hiddenCategory = 0;
            if (TaxonUtils.CurrentFilters.GetFilter<TaxonFilterRedListCategory>() is TaxonFilterRedListCategory filterRL)
                hiddenCategory = filterRL.HiddenCategory;
            foreach (var cb in RedListCategoryCheckBoxes)
            {
                ushort flag = (ushort) (1 << (int) cb.Value);
                cb.Key.Checked = (flag & hiddenCategory) == 0;
                RedListCategoryExt.SetupToggleButton(cb.Value, cb.Key);
            }
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxDisplayUnnamed_Click(object sender, EventArgs e)
        {
            string filterName = Localization.Manager.Get("_FilterName_HideUnnamed", "Hide unnamed");
            if (TaxonUtils.CurrentFilters.GetFilter<TaxonFilterUnnamed>() != null)
                TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.RemoveFilter<TaxonFilterUnnamed>());
            else
                TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.AddFilter<TaxonFilterUnnamed>());
            UpdateFilterButtonState();
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxDisplayUnnamed_Paint(object sender, PaintEventArgs e)
        {
            Theme.Current.FlatCheckBox_Paint(checkBoxDisplayUnnamed, e.Graphics, e.ClipRectangle);
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxRedListCategoryFilter_Click(object sender, EventArgs e)
        {
            ushort showFlag = 0;
            ushort hideFlag = 0;
            foreach (var cb in RedListCategoryCheckBoxes)
            {
                if (cb.Key.Checked)
                    showFlag |= (ushort)(1 << (int)cb.Value);
                else
                    hideFlag |= (ushort)(1 << (int)cb.Value);
            }

            if (showFlag == 0)
                hideFlag -= 1;

            if (hideFlag == 0)
                TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.RemoveFilter<TaxonFilterRedListCategory>());
            else
            {
                if (TaxonUtils.CurrentFilters.GetFilter<TaxonFilterRedListCategory>() is TaxonFilterRedListCategory filter)
                {
                    filter.HiddenCategory = hideFlag;
                    TaxonUtils.UpdateCurrentFilters(() => { });
                }
                else
                {
                    filter = new TaxonFilterRedListCategory { HiddenCategory = hideFlag };
                    TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.AddFilter(filter));
                }
            }
            UpdateFilterButtonState();
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxRedListCategoryFilter_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();

                string name = Localization.Manager.Get("_FilterRedList_Only", "Only");
                name = name + " \"" + RedListCategoryExt.GetName((RedListCategoryEnum) (sender as CheckBox).Tag) + "\"";
                menu.MenuItems.Add(name, CheckBoxRedListCategoryFilter_CommandOnly);
                menu.MenuItems[0].Tag = (sender as CheckBox).Tag;

                name = Localization.Manager.Get("_FilterRedList_ActivateAll", "Activate all");
                menu.MenuItems.Add(name, CheckBoxRedListCategoryFilter_CommandActivateAll);

                name = Localization.Manager.Get("_FilterRedList_DeactivateAll", "Deactivate all");
                menu.MenuItems.Add(name, CheckBoxRedListCategoryFilter_CommandDeactivateAll);

                name = Localization.Manager.Get("_FilterRedList_Invert", "Invert");
                menu.MenuItems.Add(name, CheckBoxRedListCategoryFilter_CommandInvert);

                menu.Show(this, this.PointToClient(MousePosition));
            }
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxRedListCategoryFilter_CommandOnly(object sender, EventArgs e)
        {
            ushort hideFlags = 0;
            for (int i = (int)RedListCategoryEnum.NotEvaluated; i <= (int)RedListCategoryEnum.Extinct; i++)
                if (i != (int)(RedListCategoryEnum)(sender as MenuItem).Tag)
                    hideFlags |= (ushort) (1 << i);

            if (TaxonUtils.CurrentFilters.GetFilter<TaxonFilterRedListCategory>() is TaxonFilterRedListCategory filter)
            {
                filter.HiddenCategory = hideFlags;
                TaxonUtils.UpdateCurrentFilters(() => { });
            }
            else
            {
                filter = new TaxonFilterRedListCategory { HiddenCategory = hideFlags };
                TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.AddFilter(filter));
            }
            UpdateFilterButtonState();
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxRedListCategoryFilter_CommandActivateAll(object sender, EventArgs e)
        {
            TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.RemoveFilter<TaxonFilterRedListCategory>());
            UpdateFilterButtonState();
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxRedListCategoryFilter_CommandDeactivateAll(object sender, EventArgs e)
        {
            ushort hideFlags = 0;
            for (int i = (int)RedListCategoryEnum.NotEvaluated; i <= (int)RedListCategoryEnum.Extinct; i++)
                hideFlags |= (ushort)(1 << i);
            if (TaxonUtils.CurrentFilters.GetFilter<TaxonFilterRedListCategory>() is TaxonFilterRedListCategory filter)
            {
                filter.HiddenCategory = hideFlags;
                TaxonUtils.UpdateCurrentFilters(() => { });
            }
            else
            {
                filter = new TaxonFilterRedListCategory { HiddenCategory = hideFlags };
                TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.AddFilter(filter));
            }
            UpdateFilterButtonState();
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxRedListCategoryFilter_CommandInvert(object sender, EventArgs e)
        {
            ushort hideFlag = 0;
            TaxonFilterRedListCategory filter = TaxonUtils.CurrentFilters.GetFilter<TaxonFilterRedListCategory>() as TaxonFilterRedListCategory;
            if (filter != null)
                hideFlag = filter.HiddenCategory;
            ushort newHideFlag = 0;
            for (int i = (int)RedListCategoryEnum.NotEvaluated; i <= (int)RedListCategoryEnum.Extinct; i++)
                if ((hideFlag & (1 << i)) == 0)
                    newHideFlag |= (ushort)(1 << i);
            if (filter != null)
            {
                filter.HiddenCategory = newHideFlag;
                TaxonUtils.UpdateCurrentFilters(() => { });
            }
            else
            {
                filter = new TaxonFilterRedListCategory { HiddenCategory = newHideFlag };
                TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.AddFilter(filter));
            }
            UpdateFilterButtonState();
        }

        
    }


}
