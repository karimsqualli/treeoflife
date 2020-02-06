using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Controls
{
    public class TaxonContextualMenu
    {
        public TaxonContextualMenu( TaxonTreeNode _taxon )
        {
            Taxon = _taxon;
        }

        public readonly TaxonTreeNode Taxon;
        public TaxonImageDesc ImageDesc = null;

        public bool Build(ContextMenuStrip _menu)
        {
            if (Taxon == null)
                return false;

            ToolStripMenuItem menuItem;

            // goto
            menuItem = new ToolStripMenuItem(
                Localization.Manager.Get("_GotoTaxon", "Goto {0}", Taxon.Desc.RefMainName),
                null, new System.EventHandler(OnGoto)) { Tag = Taxon };
            _menu.Items.Add(menuItem);

            // select
            if (TaxonUtils.SelectedTaxon() != Taxon)
            {
                menuItem = new ToolStripMenuItem(
                    Localization.Manager.Get("_SelectTaxon", "Select {0}", Taxon.Desc.RefMainName),
                    null, new System.EventHandler(OnSelect)) { Tag = Taxon };
                _menu.Items.Add(menuItem);
            }

            // ascendants
            List<TaxonTreeNode> Ascendants = new List<TaxonTreeNode>();
            Taxon.GetAllParents(Ascendants, false, false, false);
            if (Ascendants.Count > 0)
            {
                menuItem = new ToolStripMenuItem(Localization.Manager.Get("_SelectAscendant", "Select ascendant"));
                _menu.Items.Add(menuItem);
                foreach (TaxonTreeNode node in Ascendants)
                {
                    ToolStripMenuItem subMenuItem = new ToolStripMenuItem(
                            node.Desc.RefMainName, null, new System.EventHandler(OnSelect)) { Tag = node };
                    menuItem.DropDownItems.Add(subMenuItem);
                }
            }

            // add open image if ImagePath has been setup
            string imagePath = ImageDesc?.GetPath(Taxon.Desc);
            if (imagePath != null)
            {
                _menu.Items.Add(new ToolStripSeparator());

                menuItem = new ToolStripMenuItem(
                    Localization.Manager.Get("_OpenImage", "Open image {0}", imagePath),
                    null, new EventHandler(OnOpenImage)) { Tag = imagePath };
                _menu.Items.Add(menuItem);

                menuItem = new ToolStripMenuItem(
                    Localization.Manager.Get("_LocateImage", "Locate image in explorer"),
                    null, new EventHandler(OnLocateImage)) { Tag = imagePath };
                _menu.Items.Add(menuItem);
            }

            // add favorite menu
            menuItem = TaxonUtils.FavoritesMenuItem(Taxon);
            if (menuItem != null)
            {
                _menu.Items.Add(new ToolStripSeparator());
                _menu.Items.Add(menuItem);
            }

            menuItem = BuildAdvanced();
            if (menuItem != null)
            {
                _menu.Items.Add(new ToolStripSeparator());
                _menu.Items.Add(menuItem);
            }

            return true;
        }

        ToolStripMenuItem BuildAdvanced()
        {
            if (SystemConfig.IsInUserMode) return null;
            if (ImageDesc == null) return null;

            ToolStripMenuItem advanced = new ToolStripMenuItem("Advanced") { Name = "Advanced" };
            ToolStripItemCollection items = advanced.DropDownItems;

            items.Add(new ToolStripMenuItem("Delete this image", null, OnDeleteImage) { Name = "Delete" });
            if (Taxon.Desc.CanChangeIndexToStart(ImageDesc))
            {
                items.Add(new ToolStripMenuItem("Move at start", null, OnMoveImageAtStart) { Name = "MoveAtStart" });
                items.Add(new ToolStripMenuItem("Move up", null, OnMoveImageUp) { Name = "MoveUp" });
            }
            if (Taxon.Desc.CanChangeIndexToEnd(ImageDesc))
            {
                items.Add(new ToolStripMenuItem("Move down", null, OnMoveImageDown) { Name = "MoveDown" });
                items.Add(new ToolStripMenuItem("Move at end", null, OnMoveImageAtEnd) { Name = "MoveAtEnd" });
            }

            if (ImageDesc.Secondary)
                items.Add(new ToolStripMenuItem("Set as Primary", null, OnSetImageAsPrimary) { Name = "SetAsPrimary" });
            else
                items.Add(new ToolStripMenuItem("Set as Secondary", null, OnSetImageAsSecondary) { Name = "SetAsSecondary" });
            Localization.Manager.DoMenuItem("ContextMenu.Image", advanced);
            return advanced;
        }

        public void Show( Control _owner, Point _pt)
        {
            ContextMenuStrip _menu = new ContextMenuStrip();
            if (Build(_menu))
                _menu.Show(_owner, _pt);
        }

        public void Show( Control _owner, Point _pt, ContextMenuStrip _menu )
        {
            if (Build(_menu))
                _menu.Show(_owner, _pt);
        }

        T GetTag<T>( object _menuItem ) where T : class
        {
            return (_menuItem as ToolStripMenuItem)?.Tag as T;
        }

        private void OnGoto(object sender, EventArgs e)
        {
            TaxonUtils.GotoTaxon(GetTag<TaxonTreeNode>(sender));
        }

        private void OnSelect(object sender, EventArgs e)
        {
            TaxonUtils.GotoTaxon(GetTag<TaxonTreeNode>(sender));
            TaxonUtils.SelectTaxon(GetTag<TaxonTreeNode>(sender));
        }

        private static void OnOpenImage(object sender, EventArgs e)
        {
            string path = (string)(sender as ToolStripMenuItem)?.Tag;
            if (path != null) 
                VinceToolbox.exeFunctions.openFile(path);
        }

        private static void OnLocateImage(object sender, EventArgs e)
        {
            string path = (string)(sender as ToolStripMenuItem)?.Tag;
            if (path != null)
                VinceToolbox.exeFunctions.gotoFile(path);
        }

        public void OnDeleteImage(object sender, EventArgs e)
        {
            Taxon.Desc.DeleteImage(ImageDesc, true);
        }

        public void OnMoveImageAtStart(object sender, EventArgs e)
        {
            Taxon.Desc.ChangeIndexToStart(ImageDesc);
        }

        public void OnMoveImageUp(object sender, EventArgs e)
        {
            Taxon.Desc.ChangeIndexToNext(ImageDesc);
        }

        public void OnMoveImageDown(object sender, EventArgs e)
        {
            Taxon.Desc.ChangeIndexToPrevious(ImageDesc);
        }

        public void OnMoveImageAtEnd(object sender, EventArgs e)
        {
            Taxon.Desc.ChangeIndexToEnd(ImageDesc);
        }

        public void OnSetImageAsPrimary(object sender, EventArgs e)
        {
            Taxon.Desc.SetImageAsPrimary(ImageDesc);
        }

        public void OnSetImageAsSecondary(object sender, EventArgs e)
        {
            Taxon.Desc.SetImageAsSecondary(ImageDesc);
        }
    }
}
