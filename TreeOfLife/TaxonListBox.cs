using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TreeOfLife
{
    public class TaxonListBox : ListBox
    {
        //--------------------------------------------------------------------------------------
        public TaxonListBox()
            : base()
        {
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        //--------------------------------------------------------------------------------------
        void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SelectedItem == null || !(SelectedItem is Taxon))
            {
                e.Cancel = true;
                return;
            }

            Taxon taxon = SelectedItem as Taxon;

            ToolStripMenuItem menuItem;
            ContextMenuStrip.Items.Clear();
            menuItem = new ToolStripMenuItem("Goto " + taxon.DisplayName, null, new System.EventHandler(onGoto));
            ContextMenuStrip.Items.Add(menuItem);
            menuItem = new ToolStripMenuItem("Select " + taxon.DisplayName, null, new System.EventHandler(onSelect));
            ContextMenuStrip.Items.Add(menuItem);
            menuItem = new ToolStripMenuItem("Add " + taxon.DisplayName + " to favorite", null, new System.EventHandler(onAddFavorite));
            ContextMenuStrip.Items.Add(menuItem);
            e.Cancel = false;
        }

        //--------------------------------------------------------------------------------------
        private void onGoto(object sender, EventArgs e)
        {
            TaxonUtils.GotoTaxon(SelectedItem as Taxon);
        }

        //--------------------------------------------------------------------------------------
        private void onSelect(object sender, EventArgs e)
        {
            TaxonUtils.GotoTaxon(SelectedItem as Taxon);
        }

        //--------------------------------------------------------------------------------------
        private void onAddFavorite(object sender, EventArgs e)
        {
            TaxonUtils.FavoritesAdd(SelectedItem as Taxon);
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
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Taxon taxon = null;
            if (e.Index >= 0 && e.Index < Items.Count) 
                taxon = Items[e.Index] as Taxon;
            if (taxon == null)
            {
                base.OnDrawItem(e);
                return;
            }

            //Brush b = (e.Index & 1) == 0 ? Brushes.PapayaWhip : Brushes.PeachPuff;
            Brush b = Brushes.LightYellow;
            if ((e.State & DrawItemState.Selected) != 0) b = Brushes.Gold;
            else if (e.Index == MouseIndex) b = Brushes.Khaki;

            e.Graphics.FillRectangle(b, e.Bounds);
            e.Graphics.DrawString(taxon.DisplayName, Font, Brushes.Black, e.Bounds);

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
            if (SelectedItem == null) return;
            if (!(SelectedItem is Taxon)) return;
            
            TaxonUtils.GotoTaxon(SelectedItem as Taxon);
            if (MouseDoubleClickMode == MouseDoubleClickModeEnum.SelectTaxon)
                TaxonUtils.SelectTaxon(SelectedItem as Taxon);
        }

        //--------------------------------------------------------------------------------------
        public int IndexOf(Taxon _taxon)
        {
            return Items.IndexOf(_taxon);
        }
    }
}
