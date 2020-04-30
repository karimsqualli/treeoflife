using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TreeOfLife.Controls;
using TreeOfLife.GUI;

namespace TreeOfLife
{
    [Description("Display Graph v1.0")]
    [IconAttribute("TaxonGraph")]
    public partial class TaxonGraph : TaxonControl
    {
        public TaxonGraph()
        {
            InitializeComponent();
            InitFilterButton();
            UpdateFilterButtonState();
            TaxonUtils.CurrentFilters.OnFiltersChanged += OnFiltersChanged;
            TaxonUtils.UserFilter.OnListChanged += OnListChanged;
            //TaxonUtils.UserFilter.OnListModifiedChanged += OnListModifiedChanged;

            TaxonSearchAsync = new TaxonSearchAsync();
            TaxonSearchAsync.OnSearchCompleted += OnSearchCompleted;

        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return Graph?.ToString(); }

        public string Description
        {
            get => Graph.Description;
            set => Graph.Description = value;
        }

        //-------------------------------------------------------------------
        public TaxonGraphPanel Graph { get => taxonGraph1; }

        //-------------------------------------------------------------------
        public override void OnOptionChanged()
        {
            Graph.OnOptionChanged();
        }

        //---------------------------------------------------------------------------------
        protected override void ApplyTheme()
        {
            TopPanel.BackColor = Theme.Current.Control_Background;
        }

        //-------------------------------------------------------------------
        public override void OnRefreshGraph()
        {
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public void RefreshGraph()
        {
            Graph.RefreshGraph();
        }

        //-------------------------------------------------------------------
        public void ResetView()
        {
            Graph.ResetView();
        }

        //-------------------------------------------------------------------
        public TaxonTreeNode Selected
        {
            get { return Graph.Selected; }
            set { Graph.Selected = value; }
        }

        //-------------------------------------------------------------------
        public void Goto(TaxonTreeNode _taxon)
        {
            Graph.Goto(_taxon);
        }

        //-------------------------------------------------------------------
        public void MoveTo(TaxonTreeNode _taxon, Rectangle _to)
        {
            Graph.MoveTo(_taxon, _to);
        }

        //-------------------------------------------------------------------
        public void Offset(int _dx, int _dy)
        {
            Graph.Offset(_dx, _dy);
        }

        //=============================================================================
        // Top panel
        //

        //---------------------------------------------------------------------------------
        private void TopPanelResize(object sender, EventArgs e)
        {
            int width = TopPanel.Width;
            int x = checkBoxEX.Right;

            if (width - x < 100)
            {
                labelSepAfterIUCN.Visible = false;
                buttonTag.Visible = false;
                checkBoxTag.Visible = false;
                labelSepAfterTag.Visible = false;
                FinderIcon.Visible = false;
                FinderTextBox.Visible = false;
            }
            else if (width - x < 200 )
            {
                buttonTag.Visible = false;
                checkBoxTag.Visible = false;
                labelSepAfterTag.Visible = false;

                labelSepAfterIUCN.Visible = true;
                FinderIcon.Visible = true;
                FinderTextBox.Visible = true;

                labelSepAfterIUCN.Left = x + 8;
                labelSepAfterIUCN.Width = 1;
                FinderIcon.Left = x + 16;
                FinderTextBox.Left = x + 34;
                FinderTextBox.Width = width - x - 38;
            }
            else
            {
                buttonTag.Visible = true;
                checkBoxTag.Visible = TaxonUtils.UserFilter.List.Count > 0;
                labelSepAfterTag.Visible = true;

                labelSepAfterIUCN.Visible = true;
                FinderIcon.Visible = true;
                FinderTextBox.Visible = true;

                int w = Math.Min(200, (width - x) / 2);
                labelSepAfterIUCN.Left = x + 8;
                labelSepAfterIUCN.Width = 1;
                buttonTag.Left = x + 13;
                checkBoxTag.Left = x + 34;
                checkBoxTag.Width = w - 34;
                checkBoxTag.Top = FinderTextBox.Top;
                checkBoxTag.Height = FinderTextBox.Height;

                x += w;
                labelSepAfterTag.Left = x + 8;
                labelSepAfterTag.Width = 1;
                FinderIcon.Left = x + 16;
                FinderTextBox.Left = x + 34;
                FinderTextBox.Width = w - 38;
            }
        }

        //=============================================================================
        // Filters
        //

        //---------------------------------------------------------------------------------
        public void OnFiltersChanged(object sender, EventArgs e)
        {
            UpdateFilterButtonState();
            checkBoxTag.Checked = TaxonUtils.UserFilter.FilterIsActive;
        }

        //---------------------------------------------------------------------------------
        public void OnListChanged(object sender, EventArgs e)
        {
            checkBoxTag.Text = TaxonUtils.UserFilter.CurrentName;
            checkBoxTag.Visible = TaxonUtils.UserFilter.List.Count > 0;
            checkBoxTag.Checked = TaxonUtils.UserFilter.FilterIsActive;
        }

        //-------------------------------------------------------------------
        private void ButtonTag_Click(object sender, EventArgs e)
        {
            ContextMenu menu = new ContextMenu();
            menu.MenuItems.Add("Load", OnLoadTag);
            menu.MenuItems.Add("Close", OnCloseTag);
            menu.Show(this, PointToClient(Cursor.Position));
        }

        private void OnLoadTag( object sender, EventArgs e)
        {
            TaxonUtils.UserFilter.BrowseAndOpen();
        }

        private void OnCloseTag(object sender, EventArgs e)
        {
            TaxonUtils.UserFilter.Clear();
        }

        //---------------------------------------------------------------------------------
        /*public void OnListModifiedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }*/

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
            checkBoxTag.Checked = !TaxonUtils.CurrentFilters.HasFilter<TaxonFilterTag>();

            ushort hiddenCategory = 0;
            if (TaxonUtils.CurrentFilters.GetFilter<TaxonFilterRedListCategory>() is TaxonFilterRedListCategory filterRL)
                hiddenCategory = filterRL.HiddenCategory;
            foreach (var cb in RedListCategoryCheckBoxes)
            {
                ushort flag = (ushort)(1 << (int)cb.Value);
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
                name = name + " \"" + RedListCategoryExt.GetName((RedListCategoryEnum)(sender as CheckBox).Tag) + "\"";
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

        //---------------------------------------------------------------------------------
        private void CheckBoxTag_Click(object sender, EventArgs e)
        {
            if (TaxonUtils.CurrentFilters.GetFilter<TaxonFilterTag>() != null)
                TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.RemoveFilter<TaxonFilterTag>());
            else
            {
                TaxonFilterTag filter = new TaxonFilterTag(TaxonUtils.UserFilter.List);
                TaxonUtils.UpdateCurrentFilters(() => TaxonUtils.CurrentFilters.AddFilter(filter));
            }
        }

        //---------------------------------------------------------------------------------
        private void CheckBoxTag_Paint(object sender, PaintEventArgs e)
        {
            Theme.Current.FlatCheckBox_Paint(checkBoxTag, e.Graphics, e.ClipRectangle);
        }

        //=============================================================================
        // Finder
        //
        TaxonSearchAsync TaxonSearchAsync;
        TaxonListBoxFloating TaxonSearchResultForm;

        //-------------------------------------------------------------------
        private void FinderTextBox_TextChanged(object sender, EventArgs e)
        {
            if (FinderTextBox.Text.Length > 2)
                TaxonSearchAsync.Search(_Root, FinderTextBox.Text);
        }

        //-------------------------------------------------------------------
        private void FinderTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                if (FinderTextBox.Text.Length > 2)
                    TaxonSearchAsync.Search(_Root, FinderTextBox.Text);
                if (TaxonSearchResultForm != null && TaxonSearchResultForm.TaxonListBox.Items.Count > 0)
                {
                    TaxonTreeNode taxon = TaxonSearchResultForm.TaxonListBox.GetAt(0);
                    if (taxon != null)
                    {
                        TaxonUtils.GotoTaxon(taxon);
                        TaxonUtils.SelectTaxon(taxon);
                        TaxonSearchResultForm.Hide();
                    }
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                if (TaxonSearchResultForm.TaxonListBox.Items.Count > 0)
                {
                    e.Handled = true;
                    TaxonSearchResultForm.TaxonListBox.Focus();
                    if (TaxonSearchResultForm.TaxonListBox.SelectedIndex == -1)
                        TaxonSearchResultForm.TaxonListBox.SelectedIndex = 0;
                }
            }
        }

        //-------------------------------------------------------------------
        private void FinderTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                e.Handled = true;
        }

        //-------------------------------------------------------------------
        private void OnSearchCompleted(object sender, TaxonSearchAsyncCompletedArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                if (TaxonSearchResultForm == null)
                {
                    TaxonSearchResultForm = new TaxonListBoxFloating();
                    TaxonSearchResultForm.TaxonListBox.PreviewKeyDown += ListBoxResult_PreviewKeyDown;
                    TaxonSearchResultForm_Show();
                }
                TaxonSearchResultForm.SetContent(e.Results);
            }));
        }

        //-------------------------------------------------------------------
        private void TaxonSearchResultForm_Show()
        {
            if (TaxonSearchResultForm == null) return;
            if (TaxonSearchResultForm.Visible) return;
            TaxonSearchResultForm.Show();
            TaxonSearchResultForm.TopMost = true;
            Point pos = PointToScreen(new Point(FinderTextBox.Left, FinderTextBox.Bottom));
            TaxonSearchResultForm.Left = pos.X;
            TaxonSearchResultForm.Top = pos.Y;
            FinderTextBox.Focus();
        }

        //-------------------------------------------------------------------
        private void FinderTextBox_Enter(object sender, EventArgs e)
        {
            if (TaxonSearchResultForm != null)
                TaxonSearchResultForm_Show();
        }
        
        //-------------------------------------------------------------------
        private void FinderTextBox_Leave(object sender, EventArgs e)
        {
            if (TaxonSearchResultForm != null)
                TaxonSearchResultForm.Hide();
        }

        //-------------------------------------------------------------------
        private void ListBoxResult_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (TaxonSearchResultForm.TaxonListBox.Items.Count == 0 || TaxonSearchResultForm.TaxonListBox.SelectedIndex == 0)
                {
                    FinderTextBox.Focus();
                    TaxonSearchResultForm.TaxonListBox.SelectedIndex = -1;
                }
            }
        }
    }
}
