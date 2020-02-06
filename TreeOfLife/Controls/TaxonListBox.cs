using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TreeOfLife.Controls
{
    public class TaxonListBox : ListBox
    {
        //--------------------------------------------------------------------------------------
        public TaxonListBox() : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        private bool _CanBeSorted = false;
        public bool CanBeSorted
        {
            get { return _CanBeSorted; }
            set { _CanBeSorted = value; }
        }

        //--------------------------------------------------------------------------------------
        void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenuStrip.Items.Clear();
            ToolStripMenuItem menuItem;

            TaxonTreeNode taxon = GetSelected();
            if (taxon != null)
            {
                menuItem = new ToolStripMenuItem(Localization.Manager.Get("_GotoTaxon", "Goto {0}", taxon.Desc.RefMainName), null, new System.EventHandler(OnGoto));
                ContextMenuStrip.Items.Add(menuItem);
                menuItem = new ToolStripMenuItem(Localization.Manager.Get("_SelectTaxon", "Select {0}", taxon.Desc.RefMainName), null, new System.EventHandler(OnSelect));
                ContextMenuStrip.Items.Add(menuItem);
                menuItem = new ToolStripMenuItem(Localization.Manager.Get("_SelectTaxonAndSetAsNewRoot", "Select and set as new root {0}", taxon.Desc.RefMainName), null, new System.EventHandler(OnSelectAndSetAsNewRoot ));
                ContextMenuStrip.Items.Add(menuItem);

                menuItem = TaxonUtils.FavoritesMenuItem(taxon);
                if (menuItem != null)
                {
                    ContextMenuStrip.Items.Add(new ToolStripSeparator());
                    ContextMenuStrip.Items.Add(menuItem);
                }
            }

            if (CanBeSorted)
            {
                List<TaxonTreeNode> list = DataSource as List<TaxonTreeNode>;
                if (list.Count > 1)
                {
                    if (ContextMenuStrip.Items.Count > 0)
                        ContextMenuStrip.Items.Add(new ToolStripSeparator());
                    menuItem = new ToolStripMenuItem(Localization.Manager.Get("_Sort", "Sort"));
                    menuItem.DropDownItems.Add(Localization.Manager.Get("_SortAlpha", "Alpha"), null, new System.EventHandler(OnSort));
                    menuItem.DropDownItems.Add(Localization.Manager.Get("_SortReverseAlpha", "Reverse Alpha"), null, new System.EventHandler(OnSort));
                    menuItem.DropDownItems.Add(Localization.Manager.Get("_SortTree","Tree"), null, new System.EventHandler(OnSort));
                    menuItem.DropDownItems.Add(Localization.Manager.Get("_SortReverseTree","Reverse Tree"), null, new System.EventHandler(OnSort));
                    ContextMenuStrip.Items.Add(menuItem);
                }
            }

            e.Cancel = ContextMenuStrip.Items.Count == 0;
        }

        //--------------------------------------------------------------------------------------
        private void OnGoto(object sender, EventArgs e) { TaxonUtils.GotoTaxon(GetSelected()); }
        private void OnSelect(object sender, EventArgs e) { TaxonUtils.GotoTaxon(GetSelected()); TaxonUtils.SelectTaxon(GetSelected()); }
        private void OnSelectAndSetAsNewRoot(object sender, EventArgs e)
        {
            TaxonTreeNode selected = GetSelected();
            TaxonUtils.CleanSubRoots();
            TaxonUtils.PushSubRoot(selected);
            TaxonUtils.GotoTaxon(selected);
            TaxonUtils.SelectTaxon(selected);
        }
        private void OnRemoveFavorites(object sender, EventArgs e) { TaxonUtils.FavoritesRemove(GetSelected()); }
        private void OnAddFavorites(object sender, EventArgs e) { TaxonUtils.FavoritesAdd(GetSelected()); }

        private void OnSort(object sender, EventArgs e)
        {
            if (!(DataSource is List<TaxonTreeNode> list)) return;
            if (!(sender is ToolStripMenuItem item)) return;

            string order = item.Text;
            if (order.Contains("Alpha"))
                list.Sort(TaxonTreeNode.CompareOnlyName);
            else
                list.Sort();

            if (order.Contains("Inverse"))
                list.Reverse();

            DataSource = null;
            DataSource = list;
        }


        //--------------------------------------------------------------------------------------
        public TaxonTreeNode GetAt(int _index)
        {
            if (_index < 0 || _index >= Items.Count) return null;
            if (Items[_index] is TaxonTreeNode)
                return Items[_index] as TaxonTreeNode;
            if (Items[_index] is TaxonTreeNodeNamed)
                return (Items[_index] as TaxonTreeNodeNamed).TaxonTreeNode;
            return null;
        }

        //--------------------------------------------------------------------------------------
        TaxonTreeNode GetAt(int _index, ref string name)
        {
            if (_index < 0 || _index >= Items.Count) return null;
            if (Items[_index] is TaxonTreeNode)
            {
                name = (Items[_index] as TaxonTreeNode).Desc.RefMainName;
                return Items[_index] as TaxonTreeNode;
            }
            if (Items[_index] is TaxonTreeNodeNamed)
            {
                name = (Items[_index] as TaxonTreeNodeNamed).Name;
                return (Items[_index] as TaxonTreeNodeNamed).TaxonTreeNode;
            }
            return null;
        }

        //--------------------------------------------------------------------------------------
        TaxonTreeNode GetSelected()
        {
            if (SelectedItem is TaxonTreeNode) return (SelectedItem as TaxonTreeNode).GetOriginal();
            if (SelectedItem is TaxonTreeNodeNamed) return (SelectedItem as TaxonTreeNodeNamed).TaxonTreeNode.GetOriginal();
            return null;
        }

        //--------------------------------------------------------------------------------------
        int _MouseIndex = -1;
        int MouseIndex
        {
            get { return _MouseIndex; }
            set
            {
                if (_MouseIndex == value) return;
                _MouseIndex = value;
                Invalidate();
            }
        }

        //--------------------------------------------------------------------------------------
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                OnSelect(this, new EventArgs());
                return;
            }
            base.OnKeyDown(e);
        }

        //--------------------------------------------------------------------------------------
        public static Brush BrushNormal = Brushes.LightYellow;
        public static Brush BrushSelection = Brushes.Gold;
        public static Brush BrushHover = Brushes.Khaki;

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            string name = "";
            TaxonTreeNode taxon = GetAt(e.Index, ref name);
            if (taxon == null)
            {
                base.OnDrawItem(e);
                return;
            }

            //Brush b = (e.Index & 1) == 0 ? Brushes.PapayaWhip : Brushes.PeachPuff;
            Brush b = BrushNormal;
            if ((e.State & DrawItemState.Selected) != 0) b = BrushSelection;
            else if (e.Index == MouseIndex) b = BrushHover;

            e.Graphics.FillRectangle(b, e.Bounds);
            e.Graphics.DrawString(name, Font, Brushes.Black, e.Bounds);

            Image image = TaxonImages.Manager.GetSmallImage(taxon);
            if (image != null)
            {
                Rectangle imageRect = e.Bounds;
                imageRect.X = imageRect.Right - imageRect.Height;
                imageRect.Width = imageRect.Height;
                e.Graphics.DrawImage(image, imageRect);
            }
        }

        //--------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            Region iRegion = new Region(e.ClipRectangle);
            e.Graphics.FillRegion(new SolidBrush(this.BackColor), iRegion);
            if (this.Items.Count > 0)
            {
                for (int i = 0; i < this.Items.Count; ++i)
                {
                    System.Drawing.Rectangle irect = this.GetItemRectangle(i);
                    if (e.ClipRectangle.IntersectsWith(irect))
                    {
                        if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)
                        || (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))
                        || (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))
                        {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
                                irect, i,
                                DrawItemState.Selected, this.ForeColor,
                                this.BackColor));
                        }
                        else
                        {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
                                irect, i,
                                DrawItemState.Default, this.ForeColor,
                                this.BackColor));
                        }
                        iRegion.Complement(irect);
                    }
                }
            }
            base.OnPaint(e);
        }

        //--------------------------------------------------------------------------------------
        protected override void OnMouseMove(MouseEventArgs e)
        {
            MouseIndex = IndexFromPoint(e.Location);
        }

        //--------------------------------------------------------------------------------------
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseIndex = -1;
        }

        //--------------------------------------------------------------------------------------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
                SelectedIndex = IndexFromPoint(e.Location);
        }

        //--------------------------------------------------------------------------------------
        public enum MouseDoubleClickModeEnum
        {
            DoNothing,
            GotoTaxon,
            SelectTaxon
        }

        //--------------------------------------------------------------------------------------
        private MouseDoubleClickModeEnum _MouseDoubleClickMode = MouseDoubleClickModeEnum.SelectTaxon;
        public MouseDoubleClickModeEnum MouseDoubleClickMode
        {
            get { return _MouseDoubleClickMode; }
            set { _MouseDoubleClickMode = value; }
        }

        //--------------------------------------------------------------------------------------
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (MouseDoubleClickMode == MouseDoubleClickModeEnum.DoNothing) return;
            if (GetSelected() == null) return;

            TaxonUtils.GotoTaxon(GetSelected());
            if (MouseDoubleClickMode == MouseDoubleClickModeEnum.SelectTaxon)
                TaxonUtils.SelectTaxon(GetSelected());
        }

        //--------------------------------------------------------------------------------------
        public int IndexOf(TaxonTreeNode _taxon)
        {
            return Items.IndexOf(_taxon);
        }
    }
}
