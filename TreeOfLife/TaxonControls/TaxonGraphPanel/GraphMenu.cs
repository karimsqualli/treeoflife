using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TreeOfLife.TaxonDialog;

namespace TreeOfLife
{
    public partial class TaxonGraphPanel
    {
        //=========================================================================================
        // Normal menu
        //
        TaxonTreeNode _MenuTaxonTreeNode = null;

        //-------------------------------------------------------------------
        private void GraphContextMenu_Opening(object sender, CancelEventArgs e)
        {
            // remove previously added menu
            int index = GraphContextMenu.Items.IndexOf(toolStripMenuItemAdvanced) + 1;
            while (GraphContextMenu.Items.Count > index)
                GraphContextMenu.Items.RemoveAt(index);
            Localization.Manager.DoMenu(GraphContextMenu);

            _MenuTaxonTreeNode = BelowMouse;

            bool fullMenu = true;
            ToolStripMenuItem advanced = BuildAdvancedMenu();
            if (ModifierKeys == Keys.Shift && advanced != null)
                fullMenu = false;

            foreach (ToolStripItem item in GraphContextMenu.Items)
                item.Visible = fullMenu;

            if (fullMenu)
            {
                GraphContextMenu.Enabled = (Root != null);
                
                collapseAllToolStripMenuItem.Visible = false;
                expandAllToolStripMenuItem.Visible = false;

                setAsNewRootToolStripMenuItem.Visible = TaxonUtils.CanPushSubRoot(_MenuTaxonTreeNode);

                bool gotoSelected = TaxonUtils.SelectedTaxon() != null && TaxonUtils.SelectedTaxon() != _MenuTaxonTreeNode;
                gotoSelectedToolStripMenuItem.Visible = gotoSelected;
                if (gotoSelected)
                    gotoSelectedToolStripMenuItem.Text = Localization.Manager.Get("_GotoTaxon", "Goto {0}", TaxonUtils.SelectedTaxon().Desc.RefMainName);

                restoreRootToolStripMenuItem.Visible = TaxonUtils.HasSubRoots();
                toolStripMenuItem1.Visible = _MenuTaxonTreeNode != null;
                selectAscendantToolStripMenuItem.Visible = _MenuTaxonTreeNode != null;
                collapseTaxonToolStripMenuItem.Visible = _MenuTaxonTreeNode != null;
                expandTaxonToolStripMenuItem.Visible = _MenuTaxonTreeNode != null;
                favoritesToolStripMenuItem.Visible = _MenuTaxonTreeNode != null;

                if (_MenuTaxonTreeNode != null)
                {
                    selectAscendantToolStripMenuItem.Text = Localization.Manager.Get("_SelectAscendant", "Select ascendant");
                    List<TaxonTreeNode> Ascendants = new List<TaxonTreeNode>();
                    _MenuTaxonTreeNode.GetAllParents(Ascendants, false, false, false);
                    if (Ascendants.Count == 0)
                        selectAscendantToolStripMenuItem.Enabled = false;
                    else
                    {
                        selectAscendantToolStripMenuItem.Enabled = true;
                        selectAscendantToolStripMenuItem.DropDownItems.Clear();
                        foreach (TaxonTreeNode node in Ascendants)
                        {
                            ToolStripMenuItem subMenuItem = new ToolStripMenuItem(node.Desc.RefMainName, null, new System.EventHandler(OnSelect)) { Tag = node };
                            selectAscendantToolStripMenuItem.DropDownItems.Add(subMenuItem);
                        }
                    }

                    collapseTaxonToolStripMenuItem.Text = Localization.Manager.Get("_CollapseTaxon", "Collapse {0}", _MenuTaxonTreeNode.Desc.RefMainName);
                    collapseTaxonToolStripMenuItem.Enabled = _MenuTaxonTreeNode.HasAllChildVisible;
                    expandTaxonToolStripMenuItem.Text = Localization.Manager.Get("_ExpandTaxon", "Expand {0}", _MenuTaxonTreeNode.Desc.RefMainName);
                    string favoritesMenuItemText = TaxonUtils.FavoritesMenuItemText(_MenuTaxonTreeNode);
                    favoritesToolStripMenuItem.Visible = favoritesMenuItemText != null;
                    if (favoritesMenuItemText != null) favoritesToolStripMenuItem.Text = favoritesMenuItemText;
                }
            }

            if (advanced == null)
                toolStripMenuItemAdvanced.Visible = false;
            else if (fullMenu)
            {
                toolStripMenuItemAdvanced.Visible = true;
                GraphContextMenu.Items.Add(advanced);
            }
            else
            {
                while(advanced.DropDownItems.Count > 0)
                    GraphContextMenu.Items.Add(advanced.DropDownItems[0]);
            }
        }

        //-------------------------------------------------------------------
        private void GraphContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            // remove previously added menu
            int index = GraphContextMenu.Items.IndexOf(toolStripMenuItemAdvanced) + 1;
            while (GraphContextMenu.Items.Count > index)
                GraphContextMenu.Items.RemoveAt(index);
        }

        //-------------------------------------------------------------------
        private void OnSelect(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem)) return;
            if (!((sender as ToolStripMenuItem).Tag is TaxonTreeNode node)) return;
            TaxonUtils.GotoTaxon(node);
            TaxonUtils.SelectTaxon(node);
        }


        //-------------------------------------------------------------------
        private void CollapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Root == null) return;
            Root.CollapseAll();
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        private void ExpandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Root == null) return;
            Root.ExpandAll();
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        private void CollapseTaxonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            _MenuTaxonTreeNode.CollapseAll();
            if (_MenuTaxonTreeNode.IsFiltered())
                _MenuTaxonTreeNode.GetOriginal()?.CollapseAll();
            RefreshGraph();
            Goto(_MenuTaxonTreeNode);
        }

        //-------------------------------------------------------------------
        private void ExpandTaxonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            _MenuTaxonTreeNode.ExpandAll();
            if (_MenuTaxonTreeNode.IsFiltered())
                _MenuTaxonTreeNode.GetOriginal()?.ExpandAll();
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        private void FavoritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            if (TaxonUtils.Favorites.Contains(_MenuTaxonTreeNode.GetOriginal()))
                TaxonUtils.FavoritesRemove(_MenuTaxonTreeNode);
            else
                TaxonUtils.FavoritesAdd(_MenuTaxonTreeNode);
        }

        //-------------------------------------------------------------------
        public void ResetView()
        {
            Origin = new Point(0, 0);
            RefreshGraph();
        }

        private void ResetViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetView();
        }

        //-------------------------------------------------------------------
        private void gotoRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonUtils.GotoTaxon(TaxonUtils.Root); 
        }

        //-------------------------------------------------------------------
        private void gotoSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonUtils.GotoTaxon(TaxonUtils.SelectedTaxon());
        }

        //-------------------------------------------------------------------
        private void SetAsNewRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonUtils.PushSubRoot( _MenuTaxonTreeNode );
        }

        //-------------------------------------------------------------------
        private void RestoreRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxonUtils.CleanSubRoots();
        }

        //=========================================================================================
        // Advanced menu
        //
        readonly bool MenuAdminCreateHtml = false;

        ToolStripMenuItem BuildAdvancedMenu()
        {
            if (_MenuTaxonTreeNode == null) return null;
            if (SystemConfig.IsInUserMode) return null;

            ToolStripMenuItem advanced = new ToolStripMenuItem("Advanced") { Name = "Advanced" };
            int countLastSeparator = 0;

            if (Root.IsFiltered())
            {
                // Import / export
                advanced.DropDownItems.Add(new ToolStripMenuItem("Export", null, MenuExport_Click) { Name = "Export" });
                advanced.DropDownItems.Add(new ToolStripMenuItem("Export full", null, MenuExportFull_Click) { Name = "ExportFull" });
            }
            else
            {
                bool isRoot = _MenuTaxonTreeNode == Root;

                if (MenuAdminCreateHtml)
                {
                    if (TaxonComments.CommentFile(_MenuTaxonTreeNode) == null)
                        advanced.DropDownItems.Add(new ToolStripMenuItem("create HTML comment", null, MenuCreateComment_Click) { Name = "CreateHTMLComment" });
                }

                // Edition
                {
                    if (advanced.DropDownItems.Count > countLastSeparator)
                        advanced.DropDownItems.Add(new ToolStripSeparator());
                    countLastSeparator = advanced.DropDownItems.Count;

                    if (!isRoot)
                    {
                        advanced.DropDownItems.Add(new ToolStripMenuItem("Delete", null, MenuDelete_Click) { Name = "Delete" });
                        if (_MenuTaxonTreeNode.HasChildren)
                            advanced.DropDownItems.Add(new ToolStripMenuItem("Delete but keep children (moved in father)", null, MenuDeleteKeepChildren_Click) { Name = "DeleteKeepChildren" });
                    }
                    if (_MenuTaxonTreeNode.HasChildren)
                    {
                        advanced.DropDownItems.Add(new ToolStripMenuItem("Remove all children", null, MenuRemoveChildren_Click) { Name = "RemoveAllChild" });
                        advanced.DropDownItems.Add(new ToolStripMenuItem("Sort all children", null, MenuSortChildren_Click) { Name = "SortAllChildren" });
                    }

                    if (advanced.DropDownItems.Count > countLastSeparator)
                        advanced.DropDownItems.Add(new ToolStripSeparator());
                    countLastSeparator = advanced.DropDownItems.Count;

                    if (!isRoot && _MenuTaxonTreeNode.Father.Children.Count > 1)
                    {
                        int index = _MenuTaxonTreeNode.Father.Children.IndexOf(_MenuTaxonTreeNode);
                        if (index > 0)
                        {
                            advanced.DropDownItems.Add(new ToolStripMenuItem("Move top", null, MenuMoveTop_Click) { Name = "MoveTop" });
                            advanced.DropDownItems.Add(new ToolStripMenuItem("Move up", null, MenuMoveUp_Click) { Name = "MoveUp" });
                        }
                        if (index < _MenuTaxonTreeNode.Father.Children.Count - 1)
                        {
                            advanced.DropDownItems.Add(new ToolStripMenuItem("Move down", null, MenuMoveDown_Click) { Name = "MoveDown" });
                            advanced.DropDownItems.Add(new ToolStripMenuItem("Move bottom", null, MenuMoveBottom_Click) { Name = "MoveBottom" });
                        }
                    }

                    if (advanced.DropDownItems.Count > countLastSeparator)
                        advanced.DropDownItems.Add(new ToolStripSeparator());
                    countLastSeparator = advanced.DropDownItems.Count;

                    if (!isRoot)
                    {
                        advanced.DropDownItems.Add(new ToolStripMenuItem("Add Father", null, MenuAddFather_Click) { Name = "AddFather" });
                        advanced.DropDownItems.Add(new ToolStripMenuItem("Add Father (for all siblings)", null, MenuAddFatherAll_Click) { Name = "AddFatherAllSiblings" });
                        advanced.DropDownItems.Add(new ToolStripMenuItem("Add sibling above", null, MenuAddSiblingAbove_Click) { Name = "AddSiblingAbove" });
                        advanced.DropDownItems.Add(new ToolStripMenuItem("Add sibling below", null, MenuAddSiblingBelow_Click) { Name = "AddSiblingBelow" });
                    }
                    advanced.DropDownItems.Add(new ToolStripMenuItem("Add child", null, MenuAddChild_Click) { Name = "AddChild" });

                    /*ToolStripMenuItem flags = new ToolStripMenuItem("Flags") { Name = "Flags" };
                    flags.DropDownItems.Add(new ToolStripMenuItem("Add extinct (taxon and sub tree)", null, MenuAddEctinctFlag_Click) { Name = "AddExtinct" });
                    flags.DropDownItems.Add(new ToolStripMenuItem("Remove extinct (taxon and sub tree)", null, MenuRemoveEctinctFlag_Click) { Name = "RemoveExtinct" });
                    advanced.DropDownItems.Add(new ToolStripSeparator());
                    advanced.DropDownItems.Add(flags);*/
                }

                // Copy / paste
                {
                    if (advanced.DropDownItems.Count > 0)
                        advanced.DropDownItems.Add(new ToolStripSeparator());
                    advanced.DropDownItems.Add(new ToolStripMenuItem("Copy", null, MenuCopy_Click) { Name="Copy", ShortcutKeys = Keys.Control | Keys.C});
                    advanced.DropDownItems.Add(new ToolStripMenuItem("Cut", null, MenuCut_Click) { Name="Cut", Enabled = _MenuTaxonTreeNode.Father != null, ShortcutKeys = Keys.Control | Keys.X });
                    advanced.DropDownItems.Add(new ToolStripMenuItem("Paste", null, MenuPaste_Click) { Name = "Paste", Enabled = CanPasteClipboard(_MenuTaxonTreeNode), ShortcutKeys = Keys.Control | Keys.V });
                }

                // Import / export
                {
                    if (advanced.DropDownItems.Count > 0)
                        advanced.DropDownItems.Add(new ToolStripSeparator());
                    advanced.DropDownItems.Add(new ToolStripMenuItem("Import", null, MenuImport_Click) { Name = "Import" });
                    advanced.DropDownItems.Add(new ToolStripMenuItem("Export", null, MenuExport_Click) { Name = "Export" });
                    advanced.DropDownItems.Add(new ToolStripMenuItem("Export full", null, MenuExportFull_Click) { Name = "ExportFull" });
                }
            }

            if (advanced.DropDownItems.Count == 0)
                return null;
            Localization.Manager.DoMenuItem("ContextMenu.Graph", advanced);
            return advanced;
        }

        //-------------------------------------------------------------------
        public void MenuCreateComment_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            TaxonComments.CommentFileCreate(_MenuTaxonTreeNode);
        }

        //-------------------------------------------------------------------
        public void MenuCopy_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            CopyToClipboard(_MenuTaxonTreeNode);
        }

        //-------------------------------------------------------------------
        public void MenuCut_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            CopyToClipboard(_MenuTaxonTreeNode);
            _MenuTaxonTreeNode.Father.Children.Remove(Selected);
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public void MenuPaste_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            PasteClipboard(_MenuTaxonTreeNode);
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public void MenuImport_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            string filename = TaxonUtils.MyConfig.TaxonFileName;
            TaxonTreeNode importNode = TaxonUtils.Import(ref filename);
            if (importNode == null) return;
            ImportNode(_MenuTaxonTreeNode, importNode);
        }

        public void ImportNode(TaxonTreeNode _where, TaxonTreeNode _what)
        { 
            if (_what.Desc.Name == "__ignore__" || _where.Desc.RefMultiName.ItsOneOfMyNames(_what.Desc.RefMultiName.Main))
            {
                foreach (TaxonTreeNode node in _what.Children)
                    _where.AddChild(node);
            }
            else
                _where.AddChild(_what);

            _where.SortChildren();
            _where.Expand();
            TaxonUtils.OriginalRoot.UpdateRedListCategoryFlags();
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public void MenuExport_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            TaxonUtils.Save(_MenuTaxonTreeNode, true);
        }

        //-------------------------------------------------------------------
        public void MenuExportFull_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            if (TaxonUtils.MyConfig.GenerateNewDatabaseConfig == null)
                TaxonUtils.MyConfig.GenerateNewDatabaseConfig = new GenerateNewDatabaseConfig();
            GenerateNewDatabaseDialog dlg = new GenerateNewDatabaseDialog(_MenuTaxonTreeNode, TaxonUtils.MyConfig.GenerateNewDatabaseConfig)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            dlg.ShowDialog();
        }

        //-------------------------------------------------------------------
        public void MenuDelete_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            _MenuTaxonTreeNode.Father.Children.Remove(_MenuTaxonTreeNode);
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public void MenuDeleteKeepChildren_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            foreach (TaxonTreeNode child in _MenuTaxonTreeNode.Children)
                _MenuTaxonTreeNode.AddSiblingBefore(child);
            _MenuTaxonTreeNode.Children.Clear();
            _MenuTaxonTreeNode.Father.Children.Remove(_MenuTaxonTreeNode);
            RefreshGraph();
        }
        
        //-------------------------------------------------------------------
        public void MenuRemoveChildren_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            _MenuTaxonTreeNode.Children.Clear();
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public void MenuSortChildren_Click(object sender, EventArgs e)
        {
            if (_MenuTaxonTreeNode == null) return;
            _MenuTaxonTreeNode.Children.Sort((x,y) => x.Desc.RefMainName.CompareTo(y.Desc.RefMainName));
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public void MoveTopTaxon(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return;
            _taxon.MoveTop();
            _taxon.Father.Expand();
            RefreshGraph();
        }
        public void MenuMoveTop_Click(object sender, EventArgs e) { MoveTopTaxon(_MenuTaxonTreeNode); }

        //-------------------------------------------------------------------
        public void MoveUpTaxon( TaxonTreeNode _taxon )
        {
            if (_taxon == null) return;
            _taxon.MoveUp();
            _taxon.Father.Expand();
            RefreshGraph();
        }
        public void MenuMoveUp_Click(object sender, EventArgs e){MoveUpTaxon(_MenuTaxonTreeNode);}

        //-------------------------------------------------------------------
        public void MoveBottomTaxon(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return;
            _taxon.MoveBottom();
            _taxon.Father.Expand();
            RefreshGraph();
        }
        public void MenuMoveBottom_Click(object sender, EventArgs e) { MoveBottomTaxon(_MenuTaxonTreeNode); }

        //-------------------------------------------------------------------
        public void MoveDownTaxon(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return;
            _taxon.MoveDown();
            _taxon.Father.Expand();
            RefreshGraph();
        }
        public void MenuMoveDown_Click(object sender, EventArgs e) { MoveDownTaxon(_MenuTaxonTreeNode); }

        //-------------------------------------------------------------------
        public void AddFather(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return;
            TaxonDialog.NewTaxon dlg = new TaxonDialog.NewTaxon
            {
                TopMost = true,
                CheckNameUsage = true
            };
            dlg.ShowDialog();
            if (dlg.DialogResult != DialogResult.OK) return;

            TaxonDesc newTaxon = new TaxonDesc(dlg.TaxonName);
            TaxonTreeNode newNode = new TaxonTreeNode(newTaxon);

            TaxonTreeNode OldFather = _taxon.Father;
            OldFather.ReplaceChild( _taxon, newNode);
            newNode.AddChild(_taxon);
            OldFather.Expand();
            newNode.Expand();
            RefreshGraph();
        }
        public void MenuAddFather_Click(object sender, EventArgs e) { AddFather(_MenuTaxonTreeNode); }

        //-------------------------------------------------------------------
        public void AddFatherAll(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return;
            TaxonDialog.NewTaxon dlg = new TaxonDialog.NewTaxon
            {
                TopMost = true,
                CheckNameUsage = true
            };
            dlg.ShowDialog();
            if (dlg.DialogResult != DialogResult.OK) return;

            TaxonDesc newTaxon = new TaxonDesc(dlg.TaxonName);
            TaxonTreeNode newNode = new TaxonTreeNode(newTaxon);

            TaxonTreeNode OldFather = _taxon.Father;
            foreach (TaxonTreeNode child in OldFather.Children)
                newNode.AddChild(child);

            OldFather.Children.Clear();
            OldFather.AddChild(newNode);

            OldFather.Expand();
            newNode.Expand();
            RefreshGraph();
        }
        public void MenuAddFatherAll_Click(object sender, EventArgs e) { AddFatherAll(_MenuTaxonTreeNode); }

        //-------------------------------------------------------------------
        public void AddSiblingAbove(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return;
            string firstName = null;
            if (_taxon.Desc.ClassicRank == ClassicRankEnum.Espece)
            {
                string[] parts = _taxon.Desc.RefMainName.Split(' ');
                if (parts.Length >= 2) firstName = parts[0];
            }
            NewTaxon dlg = new TaxonDialog.NewTaxon(firstName)
            {
                TopMost = true,
                CheckNameUsage = true
            };
            dlg.ShowDialog();
            if (dlg.DialogResult != DialogResult.OK) return;

            TaxonDesc newTaxon = new TaxonDesc(dlg.TaxonName)
            {
                ClassicRank = _taxon.Desc.ClassicRank
            };

            TaxonTreeNode newNode = new TaxonTreeNode(newTaxon);
            _taxon.AddSiblingBefore(newNode);
            _taxon.Father.Expand();
            RefreshGraph();
        }
        public void MenuAddSiblingAbove_Click(object sender, EventArgs e) { AddSiblingAbove(_MenuTaxonTreeNode); }


        //-------------------------------------------------------------------
        public void AddSiblingBelow(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return;
            string firstName = null;
            if (_taxon.Desc.ClassicRank == ClassicRankEnum.Espece)
            {
                string[] parts = _taxon.Desc.RefMainName.Split(' ');
                if (parts.Length >= 2) firstName = parts[0];
            }
            TaxonDialog.NewTaxon dlg = new TaxonDialog.NewTaxon(firstName)
            {
                TopMost = true,
                CheckNameUsage = true
            };
            dlg.ShowDialog();
            if (dlg.DialogResult != DialogResult.OK) return;

            TaxonDesc newTaxon = new TaxonDesc(dlg.TaxonName)
            {
                ClassicRank = _taxon.Desc.ClassicRank
            };

            TaxonTreeNode newNode = new TaxonTreeNode(newTaxon);
            _taxon.AddSiblingAfter(newNode);
            _taxon.Father.Expand();
            RefreshGraph();
        }
        public void MenuAddSiblingBelow_Click(object sender, EventArgs e) { AddSiblingBelow(_MenuTaxonTreeNode); }

        //-------------------------------------------------------------------
        public void AddChild(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return;

            string firstName = null;
            if (_taxon.Desc.ClassicRank == ClassicRankEnum.Genre)
                firstName = _taxon.Desc.RefMainName;
            TaxonDialog.NewTaxon dlg = new TaxonDialog.NewTaxon(firstName)
            {
                TopMost = true,
                CheckNameUsage = true
            };
            dlg.ShowDialog();
            if (dlg.DialogResult != DialogResult.OK) return;

            TaxonDesc newTaxon = new TaxonDesc(dlg.TaxonName);
            if (_taxon.Desc.ClassicRank == ClassicRankEnum.Genre)
                newTaxon.ClassicRank = ClassicRankEnum.Espece;
            else if (_taxon.Desc.ClassicRank == ClassicRankEnum.Espece)
                newTaxon.ClassicRank = ClassicRankEnum.SousEspece;

            TaxonTreeNode newNode = new TaxonTreeNode(newTaxon);
            _taxon.AddChild(newNode);
            _taxon.SortChildren();
            _taxon.Expand();
            RefreshGraph();
        }
        public void MenuAddChild_Click(object sender, EventArgs e) { AddChild(_MenuTaxonTreeNode); }
    }
}
