 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Controls
{
    public partial class TaxonMultiImageSoundControl : UserControl
    {
        //---------------------------------------------------------------------------------
        public TaxonMultiImageSoundControl()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();

        }

        //=========================================================================================
        // Options
        //

        public bool AllowTips { get; set; } = true;

        //---------------------------------------------------------------------------------
        public List<MyListItem> OutputItems { get; private set; } = new List<MyListItem>();

        //---------------------------------------------------------------------------------
        public int ImageNumber
        {
            get => OutputItems.Count;
            set
            {
                if (value == OutputItems.Count)
                    return;

                // create new
                while (OutputItems.Count < value)
                    OutputItems.Add(new MyListItem() { ImageDisplayParams = TaxonUtils.MyConfig.Options.OneTaxonAllImagesVignetteDisplayParams } );
                // remove if too many
                while (OutputItems.Count > value)
                    OutputItems.RemoveAt(OutputItems.Count - 1);
               
                // resize to set position
                TaxonMultiImageSoundControl_Resize(this, null);
            }
        }

        //---------------------------------------------------------------------------------
        public void ClearAll()
        {
            for (int index = 0; index < ImageNumber; index++)
                OutputItems[index].Image = null;
            ListTaxons = null;
            ListTaxonsFamily = null;
            Items = null;
            ConvertToItems = false;
            Invalidate();
        }

        //---------------------------------------------------------------------------------
        public void SetTaxonList(List<TaxonTreeNode> _taxons, VignetteDisplayParams _params = null)
        {
            if (_taxons.Count != ImageNumber) return;
            ConvertToItems = false;
            Items = null;
            ListTaxons = null;
            ListTaxonsFamily = null;

            Dictionary<TaxonTreeNode, VignetteData> oldItemsData = new Dictionary<TaxonTreeNode, VignetteData>();
            OutputItems.ForEach(item =>
            {
                if (item.Image != null && item.ImageData != null && item.Image.Node != null)
                    oldItemsData[item.Image.Node] = item.ImageData;
                item.Image = null;
                item.ImageData = null;
                if (_params != null) item.ImageDisplayParams = _params;

            });

            for (int i = 0; i < ImageNumber; i++)
            {
                OutputItems[i].Image = new TaxonAndImage(_taxons[i], null, 0);
                if (_taxons[i] != null && oldItemsData.TryGetValue(_taxons[i], out VignetteData ctrlData))
                {
                    OutputItems[i].ImageData = ctrlData;
                    oldItemsData.Remove(_taxons[i]);
                }
            }
            Invalidate();
        }

        //---------------------------------------------------------------------------------
        public List<TaxonAndImage> GetListFromUniqueTaxon(TaxonTreeNode _taxon, List<TaxonAndImage> _old)
        {
            if (_old != null && _old.Count > 0 && _old[0].Node == _taxon)
                return _old;

            List<TaxonImageDesc> images = TaxonImages.Manager.GetListMainImages(_taxon.Desc);
            List<TaxonAndImage> list = new List<TaxonAndImage>();
            if (images != null)
                for (int i = 0; i < images.Count; i++)
                    list.Add(new TaxonAndImage(_taxon, images, i) { CanNavigate = false, AllowSound = false });
            return list;
        }

        //---------------------------------------------------------------------------------
        public void SetListFromUniqueTaxon(TaxonTreeNode _taxon, int _scrollTo)
        {
            if (ImageNumber == 0) return;
            _ListTaxons = null;
            _ListTaxonsFamily = GetListFromUniqueTaxon(_taxon, _ListTaxonsFamily);
            List<MyListItem> oldItems = Items;
            Items = null;
            ConvertToItems = true;
            ScrollMode_Update(false, oldItems);
            Invalidate();
            //todo??
            //if (_scrollTo != -1)
            //  ScrollTo(_scrollTo, Rectangle.Empty);
        }

        //---------------------------------------------------------------------------------
        public void SetLists( List<TaxonTreeNode> _list, TaxonTreeNode _family)
        {
            _ListTaxons = _list.Select(taxon => new TaxonAndImage(taxon, null, 0)).ToList();
            _ListTaxonsFamily = _family != null ? GetListFromUniqueTaxon(_family, _ListTaxonsFamily) : null;
            List<MyListItem> oldItems = Items;
            Items = null;
            ConvertToItems = true;
            ScrollMode_Update(false, oldItems);
            Invalidate();
        }

        //---------------------------------------------------------------------------------
        public void SetOldList(List<TaxonTreeNode> _list)
        {
            Dictionary<TaxonTreeNode, TaxonAndImage> dico = new Dictionary<TaxonTreeNode, TaxonAndImage>();
            if (_ListTaxons != null)
                _ListTaxons.ForEach(tai => {
                    if (tai.Node != null && tai.Images == null && tai.ImageIndex == 0)
                        dico[tai.Node] = tai;
                });

            _ListTaxons = _list.Select(taxon => {
                if (dico.TryGetValue(taxon, out TaxonAndImage tai))
                {
                    dico.Remove(taxon);
                    return tai;
                }
                return new TaxonAndImage(taxon, null, 0); 
            }).ToList();

            _ListTaxonsFamily = null;
            Items = null;
            ConvertToItems = false;
            ScrollMode_Update(false);
            Invalidate();
        }

        //---------------------------------------------------------------------------------
        public TaxonTreeNode GetTaxon(int _index)
        {
            if (_index < 0 || _index >= ImageNumber) return null;
            return OutputItems[_index].Image.Node;
        }

        //---------------------------------------------------------------------------------
        public void GetTaxons(List<TaxonTreeNode> _list)
        {
            Dictionary<TaxonTreeNode, bool> dico = new Dictionary<TaxonTreeNode, bool>();
            for (int i = 0; i < ImageNumber; i++)
            {
                TaxonTreeNode taxon = OutputItems[i].Image?.Node;
                if (taxon != null)
                {
                    if (dico.ContainsKey(taxon)) continue;
                    _list.Add(taxon);
                    dico[taxon] = true;
                }
            }
        }

        //---------------------------------------------------------------------------------
        public int GetTaxonCurrentImageIndex( TaxonTreeNode node )
        {
            if (node == null) return -1;
            for (int i = 0; i < ImageNumber; i++)
            {
                if (OutputItems[i].ImageData != null && OutputItems[i].Image?.Node == node)
                    return OutputItems[i].ImageData.CurrentImageIndex;
            }
            return -1;
        }

        //---------------------------------------------------------------------------------
        public class TaxonAndImage
        {
            public TaxonAndImage(TaxonTreeNode _node, List<TaxonImageDesc> _images, int _index )
            {
                Node = _node;
                Images = _images;
                ImageIndex = _index;
            }
            public readonly TaxonTreeNode Node;
            public readonly List<TaxonImageDesc> Images;
            public readonly int ImageIndex;
            public bool CanNavigate = true;
            public bool AllowSound = true;
        }

        //---------------------------------------------------------------------------------
        private List<TaxonAndImage> _ListTaxons = null;
        public List<TaxonAndImage> ListTaxons
        {
            get { return _ListTaxons; }
            set
            {
                if (_ListTaxons == value) return;
                _ListTaxons = value;
                ScrollMode_Update(false);
            }
        }

        //---------------------------------------------------------------------------------
        private List<TaxonAndImage> _ListTaxonsFamily = null;
        public List<TaxonAndImage> ListTaxonsFamily
        {
            get => _ListTaxonsFamily;
            set
            {
                if (_ListTaxonsFamily == value) return;
                _ListTaxonsFamily = value;
                ScrollMode_Update(false);
            }
        }

        //---------------------------------------------------------------------------------
        private bool _ScrollVisible = false;
        public bool ScrollVisible
        {
            get => _ScrollVisible; 
            private set
            {
                _ScrollVisible = value;
                vScrollBar1.Visible = value;
            }
        }

        //-------------------------------------------------------------------
        public void ScrollMode_Update(bool _keepFirstVisibleItem, List<MyListItem> _oldItems = null)
        {
            ScrollVisible = ListTaxons != null || ListTaxonsFamily != null;
            NoValueChanged = true;
            vScrollBar1.Value = 0;
            vScrollBar1.Tag = 0;
            Items = null;

            if (ConvertToItems)
            {
                BuildItems(_oldItems);
                TaxonMultiImageSoundControl_ResizeWithItems(this, null);
                ScrollMode_UpdateItems();
            }
            else
            {
                TaxonMultiImageSoundControl_Resize(this, null);
                ScrollMode_UpdateOutputImages();
            }
            NoValueChanged = false;
        }

        //-------------------------------------------------------------------
        bool NoValueChanged = false;
        private void VScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if (NoValueChanged) return;
            ScrollMode_UpdateOutputImages();
        }

        //-------------------------------------------------------------------
        public void ScrollTo( int _index, Rectangle? _bounds )
        {
            if (Items != null && _bounds != null)
                vScrollBar1.Value = Math.Max(vScrollBar1.Minimum, Math.Min(vScrollBar1.Maximum, _bounds.Value.Top));
            else
            {
                if (vScrollBar1.Tag == null) return;
                int div = (int)vScrollBar1.Tag;
                vScrollBar1.Value = div == 0 ? 0 : (int)Math.Floor((double)_index / div);
            }
        }

        //-------------------------------------------------------------------
        public void OnMouseWheel( int _delta)
        {
            if (ScrollVisible)
                vScrollBar1.Value = Math.Max( 0, Math.Min(vScrollBar1.Value + vScrollBar1.SmallChange * Math.Sign(-_delta), vScrollBar1.Maximum - vScrollBar1.LargeChange + 1));
        }

        //-------------------------------------------------------------------
        public void ScrollMode_UpdateOutputImages()
        {
            if (ScrollMode_UpdateItems())
                return;

            if (ListTaxons == null || ImageNumber == 0)
                return;
            int offset = vScrollBar1.Value * (int)vScrollBar1.Tag;
            int count = ListTaxons.Count;

            Dictionary<TaxonAndImage, VignetteData> oldItemsData = new Dictionary<TaxonAndImage, VignetteData>();
            OutputItems.ForEach( item => {
                if (item.Image != null && item.ImageData != null)
                    oldItemsData[item.Image] = item.ImageData;
                item.Image = null;
                item.ImageData = null;
            });

            for (int i = 0; i < ImageNumber; i++)
            {
                int index = i + offset;
                TaxonAndImage data = index < ListTaxons.Count ? ListTaxons[index] : null;

                OutputItems[i].Image = data;
                if (data != null )
                {
                    OutputItems[i].Image = data;
                    if (oldItemsData.TryGetValue(data, out VignetteData ctrlData))
                    {
                        OutputItems[i].ImageData = ctrlData;
                        oldItemsData.Remove(data);
                    }
                }
            }
            Invalidate();
        }

        //-------------------------------------------------------------------
        public class OnClickImageEventArgs : EventArgs 
        { 
            public int Index;
            public int ImageIndex;
            public TaxonTreeNode Taxon;
            public MyListItem Item;
        }
        public delegate void OnClickImageEventHandler(object sender, OnClickImageEventArgs e);

        //---------------------------------------------------------------------------------
        public event OnClickImageEventHandler OnClickImage = null;

        //---------------------------------------------------------------------------------
        public event OnClickImageEventHandler OnDoubleClickImage = null;

        //---------------------------------------------------------------------------------
        public event EventHandler OnEnterImage = null;
        
        //---------------------------------------------------------------------------------
        public event EventHandler OnLeaveImage = null; 

        //-------------------------------------------------------------------
        private void TaxonMultiImageSoundControl_Resize(object sender, EventArgs e)
        {
            if (TaxonMultiImageSoundControl_ResizeWithItems(sender, e))
                return;

            if (OutputItems == null || OutputItems.Count == 0) return;

            int W = Width;
            int vScrollBar1Value = 0;
            if (ScrollVisible)
            {
                W -= vScrollBar1.Width;
                if (vScrollBar1.Enabled && vScrollBar1.Value != 0 && vScrollBar1.Tag != null)
                    vScrollBar1Value = vScrollBar1.Value * (int)vScrollBar1.Tag;
            }

            if (!Grid.Compute(W, Height, ImageNumber))
                return;

            if (ScrollVisible)
            {
                vScrollBar1.Enabled = ListTaxons.Count > ImageNumber;

                if (vScrollBar1.Enabled)
                {
                    vScrollBar1.Minimum = 0;
                    vScrollBar1.Maximum = ListTaxons.Count / Grid.Nx;
                    if (vScrollBar1.Maximum * Grid.Nx >= ListTaxons.Count) vScrollBar1.Maximum--;
                    vScrollBar1.SmallChange = 1;
                    vScrollBar1.LargeChange = Grid.Ny;
                    vScrollBar1.Tag = Grid.Nx;
                    vScrollBar1.Value = (int)Math.Floor((double)vScrollBar1Value / Grid.Nx);
                    if (vScrollBar1.Value >= vScrollBar1.Maximum) vScrollBar1.Value = vScrollBar1.Maximum - 1;
                }
                else
                    vScrollBar1.Value = 0;
            }
            

            /*int x = 0;
            int y = 0;
            int decalX = 0;*/

            Rectangle R = Rectangle.Empty;
            for (int i = 0; i < ImageNumber; i++)
            {
                //Rectangle R = Rectangle.FromLTRB(Grid.X[x] + decalX, Grid.Y[y], Grid.X[x + 1] + decalX, Grid.Y[y + 1]);
                Grid.FillRect(i, ref R);
                //_OutputImages[i].Bounds = R;
                OutputItems[i].Bounds = R;

                /*if (++x < Grid.Nx) continue;
                x = 0;
                int propositionLeft = ImageNumber - i - 1;
                if (propositionLeft < Grid.Nx) decalX = Grid.X[Grid.Nx - propositionLeft] / 2;
                y++;*/
            }
            Invalidate();
        }

        

        //=================================================================================
        // Items
        //

        public class MyListItem
        {
            public TaxonAndImage Image = null;
            public string Separator = null;
            public Rectangle Bounds;
            public VignetteData ImageData = null;
            public VignetteDisplayParams ImageDisplayParams = null;
        }

        bool ConvertToItems = false;
        List<MyListItem> Items = null;

        //-------------------------------------------------------------------
        void BuildItems(List<MyListItem> _oldItems)
        {
            Dictionary<TaxonAndImage, MyListItem> dico = new Dictionary<TaxonAndImage, MyListItem>();
            if (_oldItems != null)
                _oldItems.ForEach(i => { if (i.Image != null) dico[i.Image] = i; });
            Items = new List<MyListItem>();
            if (ListTaxons != null && ListTaxons.Count > 0)
            {
                Items.Add(new MyListItem() { Separator = "SubSpecies" });
                foreach (TaxonAndImage tai in ListTaxons)
                    if (dico.TryGetValue(tai, out MyListItem item))
                        Items.Add(item);
                    else
                        Items.Add(new MyListItem() {
                            Image = tai,
                            ImageDisplayParams = TaxonUtils.MyConfig.Options.OneTaxonAllImagesVignetteDisplayParams
                        });
            }

            if (ListTaxonsFamily != null && ListTaxonsFamily.Count > 0)
            {
                if (Items.Count > 0)
                    Items.Add(new MyListItem() { Separator = "Species" });
                foreach (TaxonAndImage tai in ListTaxonsFamily)
                    if (dico.TryGetValue(tai, out MyListItem item))
                        Items.Add(item);
                    else
                        Items.Add(new MyListItem() {
                            Image = tai,
                            ImageDisplayParams = TaxonUtils.MyConfig.Options.OneTaxonOneImageVignetteDisplayParams
                        });
            }
        }

        //-------------------------------------------------------------------
        public bool ScrollMode_UpdateItems()
        {
            if (Items == null) return false;
            Invalidate();
            return true;
        }

        //-------------------------------------------------------------------
        private bool TaxonMultiImageSoundControl_ResizeWithItems(object sender, EventArgs e)
        {
            if (Items == null) return false;

            int W = Width;
            int vScrollBar1Value = vScrollBar1.Value;
            int vScrollBarMaximum = vScrollBar1.Maximum;
            if (ScrollVisible)
                W -= vScrollBar1.Width;

            if (!Grid.Compute(W, Height, ImageNumber))
                return false;

            int x = 0;
            int posY = 0;
            Rectangle R = Rectangle.Empty;
            bool firstGridLine = true;

            foreach (MyListItem item in Items)
            {
                if (item.Separator != null)
                {
                    if (x != 0)
                    {
                        x = 0;
                        posY += Grid.HyAbove;
                    }
                    item.Bounds = Rectangle.FromLTRB(0, posY, W, posY + 16);
                    posY += 16;
                    firstGridLine = true;
                }
                else
                {
                    if (firstGridLine)
                    {
                        firstGridLine = false;
                        posY += Grid.LineThickness;
                    }
                    Grid.FillRectHorizontal(x, ref item.Bounds);
                    item.Bounds.Y = posY;
                    item.Bounds.Height = Grid.HyAbove - Grid.LineThickness;

                    x++;
                    if (x == Grid.Nx)
                    {
                        x = 0;
                        posY += Grid.HyAbove;
                    }
                }
            }
            if (x != 0) posY += Grid.HyAbove; // move after last image

            if (ScrollVisible)
            {
                vScrollBar1.Enabled = posY > Grid.H;

                if (vScrollBar1.Enabled)
                {
                    vScrollBar1.Minimum = 0;
                    vScrollBar1.Maximum = posY;
                    vScrollBar1.LargeChange = Grid.H;
                    vScrollBar1.SmallChange = Grid.Hy / 2;
                    vScrollBar1.Tag = Grid.Nx;
                    if (vScrollBar1Value > 0)
                        vScrollBar1.Value = (int)(((float)vScrollBar1Value / (float)vScrollBarMaximum) * vScrollBar1.Maximum);
                    if (vScrollBar1.Value >= vScrollBar1.Maximum - Grid.H + 1) vScrollBar1.Value = vScrollBar1.Maximum - Grid.H + 1;
                }
                else
                    vScrollBar1.Value = 0;
            }

            Invalidate();
            return true;
        }

        //=================================================================================
        // Paint event 
        //

        public bool GetItemsInfos( out List<MyListItem> items, out int offset )
        {
            items = null;
            offset = 0;

            if (Items != null)
            {
                items = Items;
                offset = vScrollBar1.Value;
                return true;
            }

            if (OutputItems != null)
            {
                items = OutputItems;
                offset = 0;
                return true;
            }

            return false;
        }

        //-------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!GetItemsInfos(out List<MyListItem> items, out int offset))
            {
                base.OnPaint(e);
                return;
            }

            Point mousePos = PointToClient(Cursor.Position);

            foreach (MyListItem item in items)
            {
                if (item.Bounds.Y >= offset + Height) break;
                if (item.Bounds.Bottom <= offset) continue;

                TaxonAndImage data = item.Image;
                if (data != null && data.Node != null)
                {
                    if (item.ImageData == null)
                    {
                        item.ImageData = new VignetteData();
                        if (data.Images == null)
                            item.ImageData.CurrentTaxon = data.Node;
                        else
                            item.ImageData.SetTaxonExt(data.Node, data.Images, data.ImageIndex, data.CanNavigate, data.AllowSound);
                        item.ImageData.OnCurrentImageChanged += (o, evt) => Invalidate();
                    }

                    Rectangle Bounds = item.Bounds;
                    Bounds.Y -= offset;
                    TaxonImageControl.StaticPaint(e.Graphics, Bounds, mousePos, item.ImageDisplayParams, item.ImageData);
                }

                if (item.Separator != null)
                {
                    Rectangle Bounds = item.Bounds;
                    Bounds.Y -= offset;
                    Font font = new Font(TaxonImageControl.FontFamily, 10, FontStyle.Bold);
                    e.Graphics.DrawString(item.Separator, font, Brushes.Black, Bounds, new StringFormat()
                    {
                        LineAlignment = StringAlignment.Center
                    });
                }
            }
        }

        //=================================================================================
        // Mouse event (used when Items system)
        //

        //---------------------------------------------------------------------------------
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!GetItemsInfos(out List<MyListItem> items, out int offset))
                return;

            int X = e.Location.X;
            int Y = e.Location.Y + offset;

            foreach (MyListItem item in items)
            {
                if (item.Bounds.Y >= Y) break;
                if (item.Bounds.Bottom <= Y) continue;

                if (X >= item.Bounds.X && X <= item.Bounds.Right)
                {
                    VignetteData data = item.ImageData;
                    if (data != null)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            if (TaxonImageControl.StaticClickButtons(e.Location, data))
                                return;
                            if (data.SoundPlayerData != null)
                            {
                                if (data.SoundPlayerData.OnMouseClick(e))
                                    return;
                            }
                            OnClickImageEventArgs args = new OnClickImageEventArgs()
                            {
                                Taxon = data.CurrentTaxon,
                                ImageIndex = data.CurrentImageIndex,
                                Item = item,
                                Index = 0 // todo differently was index in list??
                            };
                            OnClickImage?.Invoke(this, args);
                        }
                        else if (e.Button == MouseButtons.Right)
                        {
                            if (data.SoundPlayerData != null)
                            {
                                if (data.SoundPlayerData.OnMouseClick(e))
                                    return;
                            }
                            new TaxonContextualMenu(data.CurrentTaxon)
                                    { ImageDesc = data.GetImageDesc() }.Show(this, e.Location);
                        }
                    }
                    break;
                }
            }
        }

        //---------------------------------------------------------------------------------
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (OnDoubleClickImage == null) return;
            if (e.Button != MouseButtons.Left) return;

            if (!GetItemsInfos(out List<MyListItem> items, out int offset))
                return;

            int X = e.Location.X;
            int Y = e.Location.Y + offset;

            foreach (MyListItem item in items)
            {
                if (item.Bounds.Y >= Y) break;
                if (item.Bounds.Bottom <= Y) continue;

                if (X >= item.Bounds.X && X <= item.Bounds.Right)
                {
                    VignetteData data = item.ImageData;
                    if (data != null)
                    {
                        if (!TaxonImageControl.StaticClickButtons(e.Location, data))
                        {
                            OnClickImageEventArgs args = new OnClickImageEventArgs()
                            {
                                Taxon = data.CurrentTaxon,
                                ImageIndex = data.CurrentImageIndex,
                                Item = item,
                                Index = 0 // todo differently was index in list??
                            };
                            OnDoubleClickImage(this, args);
                            return;
                        }
                    }
                    break;
                }
            }
        }

        //---------------------------------------------------------------------------------
        MyListItem ItemBelowMouse = null;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!GetItemsInfos(out List<MyListItem> items, out int offset))
                return;

            int X = e.Location.X;
            int Y = e.Location.Y + offset;

            foreach (MyListItem item in items)
            {
                if (item.Bounds.Y >= Y) break;
                if (item.Bounds.Bottom <= Y) continue;

                if (X >= item.Bounds.X && X <= item.Bounds.Right)
                {
                    if (ItemBelowMouse != item)
                    {
                        if (ItemBelowMouse != null && ItemBelowMouse.ImageData != null)
                        {
                            ItemBelowMouse.ImageData.OnMouseLeave();
                            OnLeaveImage?.Invoke(ItemBelowMouse.ImageData.CurrentTaxon, EventArgs.Empty);
                        }
                        ItemBelowMouse = item;
                        if (ItemBelowMouse.ImageData != null)
                        {
                            ItemBelowMouse.ImageData.OnMouseEnter();
                            OnEnterImage?.Invoke(ItemBelowMouse.ImageData.CurrentTaxon, EventArgs.Empty);
                        }
                    }
                    else if (ItemBelowMouse != null && ItemBelowMouse.ImageData != null)
                    {
                        ItemBelowMouse.ImageData.OnMouseMove(e);
                    }

                    // tips
                    if (AllowTips && ItemBelowMouse.ImageData != null)
                        TipManager.SetTaxon(ItemBelowMouse.ImageData.CurrentTaxon, ItemBelowMouse.ImageData.GetImageDesc());
                    else
                        TipManager.SetTaxon(null, null);
                    return;
                }
            }

            if (ItemBelowMouse != null && ItemBelowMouse.ImageData != null)
            {
                ItemBelowMouse.ImageData.OnMouseLeave();
                OnLeaveImage?.Invoke(ItemBelowMouse.ImageData.CurrentTaxon, EventArgs.Empty);
            }
            ItemBelowMouse = null;

            // tips
            TipManager.SetTaxon(null, null);
        }

        //---------------------------------------------------------------------------------
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (ItemBelowMouse != null && ItemBelowMouse.ImageData != null)
                OnLeaveImage?.Invoke(ItemBelowMouse.ImageData.CurrentTaxon, EventArgs.Empty);
            ItemBelowMouse = null;
            // tips
            TipManager.SetTaxon(null, null);
        }

        //=================================================================================
        // GRID
        //

        //---------------------------------------------------------------------------------
        class GridParameters
        {
            public bool Valid = false;

            public int W;
            public int H;
            public int ImageNumber;
            public int LineThickness;

            public int Nx;
            public int Wx;

            public int Ny;
            public int Hy;
            public int HyAbove;

            void ComputeNxNy()
            {
                double ratio = Math.Sqrt(ImageNumber * (double)W / (double)H);
                Nx = (int)(ratio + 0.5);
                if (Nx > ImageNumber) Nx = ImageNumber;
                Nx = Math.Max(Nx, 1);
                Ny = 1;
                while (Ny * Nx < ImageNumber) Ny++;
            }

            int[] ColumnX;
            void ComputeColumns()
            {
                Wx = W / Nx;
                ColumnX = new int[Nx * 2];
                int left = W - Wx * Nx;
                for (int i = (Nx - left) / 2; left != 0; left--)
                    ColumnX[i * 2 + 1] = 1;
                ColumnX[0] = LineThickness;
                for (int i = 1; i < Nx; i++)
                    ColumnX[i * 2] = ColumnX[(i - 1) * 2] + ColumnX[(i - 1) * 2 + 1] + Wx;
                for (int i = 0; i < Nx; i++)
                    ColumnX[i * 2 + 1] += Wx - LineThickness;
            }

            int[] RowY;
            void ComputeRows()
            {
                Hy = H / Ny;
                RowY = new int[Ny * 2];
                int left = H - Hy * Ny;
                HyAbove = Hy + (left > 0 ? 1 : 0);
                for (int i = (Ny - left) / 2; left != 0; left--)
                    RowY[i * 2 + 1] = 1;
                RowY[0] = LineThickness;
                for (int i = 1; i < Ny; i++)
                    RowY[i * 2] = RowY[(i - 1) * 2] + RowY[(i - 1) * 2 + 1] + Hy;
                for (int i = 0; i < Ny; i++)
                    RowY[i * 2 + 1] += Hy - LineThickness;
            }

            public bool Compute(int _w, int _h, int _number)
            {
                Valid = false;
                ImageNumber = _number;
                LineThickness = TaxonUtils.MyConfig.Options.GridLineThickness;
                W = _w - LineThickness;
                H = _h - LineThickness;
                if (W <= 0 || H <= 0 || ImageNumber == 0) return false;

                ComputeNxNy();
                ComputeColumns();
                ComputeRows();
                return true;
            }

            public bool FillRect(int _index, ref Rectangle _rect)
            {
                if (_index < 0 || _index >= ImageNumber) return false;
                int row = Math.DivRem(_index, Nx, out int column);
                _rect.X = ColumnX[column * 2];
                _rect.Width = ColumnX[column * 2 + 1];
                _rect.Y = RowY[row * 2];
                _rect.Height = RowY[row * 2 + 1];
                return true;
            }

            public bool FillRectHorizontal(int _index, ref Rectangle _rect)
            {
                if (_index < 0 || _index >= Nx) return false;
                _rect.X = ColumnX[_index * 2];
                _rect.Width = ColumnX[_index * 2 + 1];
                return true;
            }
        }
        GridParameters Grid = new GridParameters();

        //-------------------------------------------------------------------
        public int NextNumberToFillRectangle(int _current)
        {
            int W = Width - ((ScrollVisible) ? vScrollBar1.Width : 0);
            int H = Height;
            int Number = _current + 1;

            while (Number < 100)
            {
                double ratio = Math.Sqrt(Number * (double)W / (double)H);
                int Nx = (int)(ratio + 0.5);
                if (Nx > Number) Nx = Number;
                int Ny = 1;
                while (Ny * Nx < Number) Ny++;
                if (Nx * Ny == Number)
                    return Number;
                Number++;
            }
            return 100;
        }

        //-------------------------------------------------------------------
        public int PreviousNumberToFillRectangle(int _current)
        {
            if (_current == 0) return 1;

            int W = Width - ((ScrollVisible) ? vScrollBar1.Width : 0);
            int H = Height;
            int Number = _current - 1;

            while (Number > 1)
            {
                double ratio = Math.Sqrt(Number * (double)W / (double)H);
                int Nx = (int)(ratio + 0.5);
                if (Nx > Number) Nx = Number;
                int Ny = 1;
                while (Ny * Nx < Number) Ny++;
                if (Nx * Ny == Number)
                    return Number;
                Number--;
            }
            return 1;
        }
    }
}
