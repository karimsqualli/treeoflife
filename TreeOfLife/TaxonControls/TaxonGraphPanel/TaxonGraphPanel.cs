using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TreeOfLife.Controls;

namespace TreeOfLife
{
    public partial class TaxonGraphPanel : TaxonControl
    {
        //-------------------------------------------------------------------
        InertiaMove _InertiaMove = null;
        public GraphOptions Options { get; set; } = new GraphOptions();

        //-------------------------------------------------------------------
        public override void OnOptionChanged()
        {
            base.OnOptionChanged();
            BackColor = _UsedColors.BackColor;
            Invalidate();
        }

        //-------------------------------------------------------------------
        public override void OnRefreshGraph()
        {
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public TaxonGraphPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            
            if (TaxonUtils.MyConfig == null) return;
            UsedColors = TaxonUtils.MyConfig.Options.FullTreeColor;
            PaintInit();
            Enabled = false;
            _InertiaMove = new InertiaMove(this);
            _EditionToolInfo = new TaxonGraphEditionTool(this);
            TaxonUtils.CurrentFilters.OnFiltersChanged += OnFiltersChanged;
            
        }

        //---------------------------------------------------------------------------------
        public override string ToString() 
        {
            if (Root == null) return "Graph";
            string result = TaxonUtils.MyConfig.TaxonFileName;
            if (Description != null)
                result += " (" + Description + ")";
            return result; 
        }
        public string Description = null;
        
        //-------------------------------------------------------------------
        public override TaxonTreeNode Root
        {
            set
            {
                _Root = value;
                Enabled = _Root != null;
                Selected = null;
                OnFiltersChanged(null, EventArgs.Empty);
                RefreshGraph();
            }
        }

        //---------------------------------------------------------------------------------
        public override void OnTaxonChanged(object sender, TaxonTreeNode _taxon)
        {
            RefreshGraph();
        }

        //---------------------------------------------------------------------------------
        public override void OnAvailableImagesChanged()
        {
            Invalidate();
        }

        //-------------------------------------------------------------------
        public void OnFiltersChanged(object sender, EventArgs e)
        {
            if (DesignMode) return;
            UsedColors = TaxonUtils.CurrentFilters.IsEmpty ? TaxonUtils.MyConfig.Options.FullTreeColor : TaxonUtils.MyConfig.Options.PartialTreeColor;
            Description = TaxonUtils.CurrentFilters.ToString();
            Options.DisplayClassikRankRule = false;
        }

        //-------------------------------------------------------------------
        public override bool CanBeMoved { get { return false; } }
        public override bool CanBeClosed { get { return false; } }

        //-------------------------------------------------------------------
        public event EventHandler OnSelectedChanged;
        public event EventHandler OnReselect;

        TaxonTreeNode _Selected = null;
        public TaxonTreeNode Selected
        {
            get { return _Selected; }
            set
            {
                if (_Selected == value)
                {
                    OnReselect?.Invoke(this, new EventArgs());
                    return;
                }
                _Selected = value;
                OnSelectedChanged?.Invoke(this, new EventArgs());
                Invalidate();
            }
        }

        public TaxonTreeNode SelectedInOriginal
        {
            get { return Selected?.GetOriginal(); }
        }

        //-------------------------------------------------------------------
        TaxonGraphEditionTool _EditionToolInfo = null;

        //-------------------------------------------------------------------
        public event EventHandler OnGraphBelowChanged;
        TaxonTreeNode _BelowMouse = null;
        public TaxonTreeNode BelowMouse
        {
            get { return _BelowMouse; }
            set
            {
                if (_BelowMouse == value) return;
                _BelowMouse = value;
                OnGraphBelowChanged?.Invoke(this, new EventArgs());
                Invalidate();
            }
        }

        public TaxonTreeNode BelowMouseNoShortcut = null;

        //-------------------------------------------------------------------
        public void RefreshGraph()
        {
            if (Root == null) return;
            Root.Visible = true;

            TaxonTreeNode.LayoutParams layoutParams = new TaxonTreeNode.LayoutParams
            {
                columnInter = 0,
                width = Options.ZoomedWidth,
                height = Options.ZoomedHeight,
                classicRankColumn = Options.DisplayClassikRankRule ? ClassicRankEnumExt.ClassicRankColumns : null
            };

            // special params for other modes
            if (Options.DisplayMode == GraphOptions.DisplayModeEnum.Lines)
                layoutParams.columnInter = Options.ZoomedColumnWidth;

            /* Removing CircleDisplayMode
            if (Options.DisplayMode == GraphOptions.DisplayModeEnum.Circle) 
                layoutParams.classicRankColumn = null;
                */

            Root.Computelayout(0, 0, layoutParams);

            /* Removing CircleDisplayMode
            if (Options.DisplayMode == GraphOptions.DisplayModeEnum.Circle)
                _CircularParams = Root.computelayoutCircular(0, 0, layoutParams);
            else */
                _CircularParams = null;

            Invalidate();
            OnGraphRefreshed?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnGraphRefreshed = null;

        //-------------------------------------------------------------------
        TaxonTreeNode.CircularParams _CircularParams = null;

        //-------------------------------------------------------------------
        Rectangle _PaintRectangle;
        public Rectangle PaintRectangle
        {
            get { return _PaintRectangle; }
            set
            {
                if (_PaintRectangle == value)
                    return;
                _PaintRectangle = value;
                if (OnPaintRectangleChanged != null)
                {
                    OnPaintRectangleChangedArgs e = new OnPaintRectangleChangedArgs { R = _PaintRectangle };
                    OnPaintRectangleChanged(this, e);
                }
            }
        }

        public class OnPaintRectangleChangedArgs : EventArgs
        {
            public Rectangle R;
        }

        public delegate void OnPaintRectangleChangedEventHandler(object sender, OnPaintRectangleChangedArgs e);

        public event OnPaintRectangleChangedEventHandler OnPaintRectangleChanged = null;

        //-------------------------------------------------------------------
        GraphColors _UsedColors = null;
        public GraphColors UsedColors
        {
            get { return _UsedColors; }
            set
            {
                _UsedColors = value;
                BackColor = _UsedColors?.BackColor ?? Color.Transparent;
                Invalidate();
            }
        }

        //=========================================================================================
        // PICKING
        //
        
        //-------------------------------------------------------------------
        public static TaxonTreeNode PickTaxonRectangularStatic(TaxonTreeNode _in, int X, int Y)
        {
            if (X < _in.R.Left) return null;
            if (Y < _in.R.Top) return null;
            if (Y > _in.R.Bottom) return null;

            if (_in.R.Contains(X, Y))
                return _in;
        
            foreach (TaxonTreeNode child in _in.Children)
            {
                TaxonTreeNode result = PickTaxonRectangularStatic(child, X, Y);
                if (result != null) return result;
            }
            return null;
        }


        //-------------------------------------------------------------------
        public PickingResult PickTaxonRectangular(TaxonTreeNode _in, int X, int Y)
        {
            if (X < _in.R.Left) return null;
            if (Y < _in.R.Top) return null;
            if (Y > _in.R.Bottom) return null;

            if (_in.R.Contains(X, Y))
            {
                if (ShortcutModeKeyOn() && ShortcutModeDatas.TryGetValue(_in, out ShortcutData data))
                {
                    if (data.R.Contains(X, Y)) return new PickingResult() { BelowMouse = _in, BelowMouseIncludingShortcut = _in };
                    foreach (KeyValuePair<TaxonTreeNode, Rectangle> tuple in data.Siblings)
                        if (tuple.Value.Contains(X, Y)) return new PickingResult() { BelowMouse = _in, BelowMouseIncludingShortcut = tuple.Key };
                    //return null;
                }
                return new PickingResult() { BelowMouse = _in, BelowMouseIncludingShortcut = _in };
            }

            foreach (TaxonTreeNode child in _in.Children)
            {
                PickingResult result = PickTaxonRectangular(child, X, Y);
                if (result != null) return result;
            }
            return null;
        }

        //-------------------------------------------------------------------
        TaxonTreeNode PickTaxonCircular(TaxonTreeNode _in, int X, int Y)
        {
            if (_in.CircularInfo != null)
            {
                if (_in.CircularInfo.InternalPoint)
                {
                    int delta = (X - (int) _in.CircularInfo.X);
                    if (delta > -5 && delta < 5)
                    {
                        delta = (Y - (int)_in.CircularInfo.Y);
                        if (delta > -5 && delta < 5)
                            return _in;
                    }
                }
                else
                {
                    double t = (X - _CircularParams.CenterX) * _in.CircularInfo.Dx + (Y - _CircularParams.CenterY) * _in.CircularInfo.Dy;
                    if (t >= _CircularParams.Radius && t <= _CircularParams.Radius + _CircularParams.TaxonWidth)
                    {
                        t = (X - _CircularParams.CenterX) * _in.CircularInfo.Nx + (Y - _CircularParams.CenterY) * _in.CircularInfo.Ny;
                        if (Math.Abs(t) < _CircularParams.TaxonHeightO2)
                            return _in;
                    }
                    return null;
                }
            }

            foreach (TaxonTreeNode child in _in.Children)
            {
                TaxonTreeNode result = PickTaxonCircular(child, X, Y);
                if (result != null) return result;
            }
            return null;
        }

        //-------------------------------------------------------------------
        public class PickingResult
        {
            public TaxonTreeNode BelowMouseIncludingShortcut;
            public TaxonTreeNode BelowMouse;

            public TaxonTreeNode GetMain() { return BelowMouseIncludingShortcut == BelowMouse ? BelowMouse : null; }
        }

        //-------------------------------------------------------------------
        public PickingResult PickTaxon(TaxonTreeNode _in, int X, int Y)
        {
            /* Removing CircleDisplayMode
            if (Options.DisplayMode == GraphOptions.DisplayModeEnum.Circle)
                return PickTaxonCircular(_in, X, Y);
                */

            return PickTaxonRectangular(_in, X, Y);
        }

        //-------------------------------------------------------------------
        PickingResult PickTaxon(TaxonTreeNode _in, Point _pt)
        {
            return PickTaxon(_in, _pt.X, _pt.Y);
        }

        //=========================================================================================
        // MOUSE / MOVE
        //

        //-------------------------------------------------------------------
        Size GraphSize()
        {
            /* Removing CircleDisplayMode
            if (Options.DisplayMode == GraphOptions.DisplayModeEnum.Circle)
                return new Size( (int) _CircularParams.Width, (int) _CircularParams.Height);
                */

            return new Size(Root.WidthWithChildren, Root.HeightWithChildren);
        }

        //-------------------------------------------------------------------
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (Root == null) return;

            if (ModifierKeys.HasFlag(Keys.Control))
            {
                Options.Zoom *= Math.Sign(e.Delta) > 0 ? 1.1f : 0.9f;
                EditionToolUpdate(null);
                ShortcutModeClear();
                RefreshGraph();
                if (Selected != null) Goto(Selected);
                return;
            }

            PickingResult pickResult = PickTaxon(Root, ClientToGraph( e.Location));
            TaxonTreeNode pick = pickResult?.GetMain();

            if (pick != null)
            {
                if (e.Delta > 0)
                {
                    pick.ExpandExtended();
                    RefreshGraph();
                }
                else
                {
                    pick.CollapseExtended();
                    RefreshGraph();
                    if (pick.R.Bottom < e.Location.Y - Origin.Y)
                        Origin.Y = e.Location.Y + 10 - pick.R.Bottom;
                }
            }
        }

        //-------------------------------------------------------------------
        Point Origin; // coordinates in Client space of origin of graph
        bool mouseDown = false;
        bool move = false;
        int mouseX, mouseY;
        int moveX, moveY;

        //-------------------------------------------------------------------
        Point ClientToGraph(Point _client)
        {
            Point pt = new Point(_client.X - Origin.X, _client.Y - Origin.Y);
            return pt;
        }

        //-------------------------------------------------------------------
        Point GraphToClient(Point _graph)
        {
            Point pt = new Point(_graph.X + Origin.X, _graph.Y + Origin.Y);
            return pt;
        }

        //-------------------------------------------------------------------
        Rectangle GraphToClient( Rectangle _graph)
        {
            Rectangle R = new Rectangle(_graph.X + Origin.X, _graph.Y + Origin.Y, _graph.Width, _graph.Height);
            return R;
        }

        //-------------------------------------------------------------------
        void ConstraintGraphInClient()
        {
            Size size = GraphSize();
            Size addSize = new Size( Width - Options.ZoomedWidth, Height - Options.ZoomedHeight);
            Rectangle R = Rectangle.FromLTRB(-addSize.Width, -addSize.Height, size.Width + addSize.Width, size.Height + addSize.Height);
            R = GraphToClient(R);
            
            if (R.Left > 0) Origin.X -= R.Left;
            else if (R.Right < Width) Origin.X += Width - R.Right;

            if (R.Top > 0) Origin.Y -= R.Top;
            else if (R.Bottom < Height) Origin.Y += Height - R.Bottom;
        }

        //-------------------------------------------------------------------
        void OffsetOrigin(int _dx, int _dy)
        {
            Origin.X += _dx;
            Origin.Y += _dy;
        }

        //-------------------------------------------------------------------
        void SetOrigin(int _x, int _y)
        {
            Origin.X = _x;
            Origin.Y = _y;
        }

        //-------------------------------------------------------------------
        public void Offset(int _dx, int _dy)
        {
            OffsetOrigin(_dx, _dy);
            ConstraintGraphInClient();
            Invalidate();
        }

        //-------------------------------------------------------------------
        public void Goto(TaxonTreeNode _taxon)
        {
            if (_taxon == null) return;
            if (!_taxon.Visible) return;

            // get taxon center;
            Point pt;
            /* Removing CircleDisplayMode
            if (Options.DisplayMode == GraphOptions.DisplayModeEnum.Circle)
                pt = new Point((int)_taxon.CircularInfo.X, (int) _taxon.CircularInfo.Y);
            else*/
                pt = new Point(_taxon.R.Left + Options.ZoomedWidth / 2, _taxon.R.Top + _taxon.R.Height / 2);
            // in Client space
            pt = GraphToClient(pt);
            // compute delta to move pt to client middle (Width/2, Height/2);
            // pt.X + deltaX = Width / 2
            // pt.Y + deltaY = Height / 2
            OffsetOrigin(Width / 2 - pt.X, Height / 2 - pt.Y);
            ConstraintGraphInClient();
            Invalidate();
        }

        //-------------------------------------------------------------------
        public void MoveTo(TaxonTreeNode _taxon, Rectangle _to)
        {
            if (_taxon == null) return;
            if (!_taxon.Visible) return;

            Point pt1 = new Point(_to.Left + _to.Width / 2, _to.Top + _to.Height / 2);
            pt1 = GraphToClient(pt1);
            Point pt;
            pt = new Point(_taxon.R.Left + Options.ZoomedWidth / 2, _taxon.R.Top + _taxon.R.Height / 2);
            pt = GraphToClient(pt);
            OffsetOrigin(pt1.X - pt.X, pt1.Y - pt.Y);
            ConstraintGraphInClient();
            Invalidate();
        }

        //-------------------------------------------------------------------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Root == null) return;

            if (e.Button == MouseButtons.Right)
            {
                TaxonTreeNode pick = PickTaxon(Root, ClientToGraph(e.Location))?.GetMain();
                if (pick != Selected) Selected = pick;
                return;
            }

            mouseDown = true;
            mouseX = e.Location.X;
            mouseY = e.Location.Y;
            moveX = Origin.X;
            moveY = Origin.Y;

            _InertiaMove.Stop();
        }

        //-------------------------------------------------------------------
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Root == null) return;

            if (mouseDown)
            {
                if (Math.Max(Math.Abs(e.Location.X - mouseX), Math.Abs(e.Location.Y - mouseY)) > 3)
                {
                    mouseDown = false;
                    move = true;
                    _InertiaMove.StartSamples(e.Location);
                }
                return;
            }

            if (move)
            {
                _InertiaMove.AddSample(e.Location);
                int dx = e.Location.X - mouseX;
                int dy = e.Location.Y - mouseY;
                mouseX = e.Location.X;
                mouseY = e.Location.Y;

                Offset(dx, dy);

                if (mouseY > ClientRectangle.Bottom)
                {
                    _InertiaMove.OffsetRef(0, -ClientRectangle.Height);
                    mouseY -= ClientRectangle.Height;
                    Cursor.Position = PointToScreen(new Point(mouseX, mouseY));
                }
                else if (mouseY < ClientRectangle.Top)
                {
                    _InertiaMove.OffsetRef(0, ClientRectangle.Height);
                    mouseY += ClientRectangle.Height;
                    Cursor.Position = PointToScreen(new Point(mouseX, mouseY));
                }
                if (mouseX > ClientRectangle.Right)
                {
                    _InertiaMove.OffsetRef(-ClientRectangle.Width, 0);
                    mouseX -= ClientRectangle.Width;
                    Cursor.Position = PointToScreen(new Point(mouseX, mouseY));
                }
                else if (mouseX < ClientRectangle.Left)
                {
                    _InertiaMove.OffsetRef(ClientRectangle.Width, 0);
                    mouseX += ClientRectangle.Width;
                    Cursor.Position = PointToScreen(new Point(mouseX, mouseY));
                }
                return;
            }

            PickingResult pickResult = PickTaxon(Root, ClientToGraph(e.Location));
            BelowMouse = pickResult?.BelowMouseIncludingShortcut;
            BelowMouseNoShortcut = pickResult?.BelowMouse;
            mouseX = e.Location.X;
            mouseY = e.Location.Y;
            Invalidate();
        }

        //-------------------------------------------------------------------
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (move)
            {
                _InertiaMove.EndSamples();
                move = false;
                return;
            }
            mouseDown = false;
            if (e.Button == MouseButtons.Left)
            {
                if (!_EditionToolInfo.DoAction())
                {
                    if (BelowMouse == null)
                        TaxonUtils.GotoTaxon(Selected);
                    else
                        Selected = BelowMouse;
                }
            }
        }

        //-------------------------------------------------------------------
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (Selected != null && ShortcutModeKeyOn() )
            {
                foreach( ShortcutData data in ShortcutModeDatas.Values)
                {
                    if (data.Siblings.TryGetValue(Selected, out Rectangle R))
                    {
                        ShortcutModeClear();
                        TaxonUtils.MoveTaxonTo(Selected, R);
                        return;
                    }
                }
            }
            if (BelowMouse == null)
            {
                TaxonTreeNode taxon = TaxonUtils.Root;
                TaxonUtils.SelectTaxon(taxon);
                TaxonUtils.GotoTaxon(taxon);
            }
        }

        //-------------------------------------------------------------------
        private void TaxonGraph_MouseEnter(object sender, EventArgs e)
        {
            Focus();
        }

        //-------------------------------------------------------------------
        private void TaxonGraph_MouseLeave(object sender, EventArgs e)
        {
            BelowMouse = null;
        }

        //-------------------------------------------------------------------
        private void TaxonGraph_KeyUp(object sender, KeyEventArgs e )
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey)
            {
                if (EditionToolUpdate(Selected))
                    Invalidate();
            }
        }

        //-------------------------------------------------------------------
        private void TaxonGraph_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bool ctrlKey = ModifierKeys.HasFlag(Keys.Control);
            bool shiftKey = ModifierKeys.HasFlag(Keys.Shift);

            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                case Keys.ShiftKey:
                    if (ctrlKey && shiftKey && EditionToolUpdate(Selected))
                        Invalidate();
                    break;

                case Keys.Left:
                    if (Selected != null && Selected.Father != null)
                    {
                        Selected = Selected.Father;
                        Goto(Selected);
                    }
                    break;

                case Keys.Right:
                    if (Selected != null && Selected.Children != null && Selected.Children.Count > 0)
                    {
                        Selected.Expand();
                        RefreshGraph();
                        Selected = Selected.Children[0];
                        Goto(Selected);
                    }
                    break;

                case Keys.Up:
                case Keys.Down:
                    if (Selected != null)
                    {
                        if (ModifierKeys == Keys.Control)
                        {
                            if (e.KeyCode == Keys.Up && Selected.CanMoveUp())
                            {
                                Selected.MoveUp();
                                Selected.Father.Expand();
                                EditionToolUpdate(null);
                                RefreshGraph();
                            }
                            if (e.KeyCode == Keys.Down && Selected.CanMoveDown())
                            {
                                Selected.MoveDown();
                                Selected.Father.Expand();
                                EditionToolUpdate(null);
                                RefreshGraph();
                            }
                        }
                        else
                        {
                            int X = Selected.R.X + Options.ZoomedWidth / 2;
                            int Y = (e.KeyCode == Keys.Up) ? Selected.R.Y - 5 : Selected.R.Bottom + 5;
                            TaxonTreeNode test = PickTaxon(Root, X, Y)?.GetMain();
                            if (test != null)
                            {
                                Selected = test;
                                Goto(Selected);
                            }
                        }
                    }
                    break;
                case Keys.PageDown:
                    TaxonUtils.ParseHistoryPrevious();
                    break;
                case Keys.PageUp:
                    TaxonUtils.ParseHistoryNext();
                    break;
                case Keys.C:
                    if (Selected != null)
                    {
                        if (ctrlKey && shiftKey)
                            Clipboard.SetText(Selected.GetHierarchicalName());
                        else if (ctrlKey)
                            CopyToClipboard(Selected);
                    }
                    break;
                case Keys.X:
                    if (Selected != null && !Selected.IsFiltered() && ctrlKey)
                    {
                        CopyToClipboard(Selected);
                        Selected.Father.Children.Remove(Selected);
                        RefreshGraph();
                    }
                    break;
                case Keys.V:
                    if (ctrlKey)
                    {
                        PasteClipboard(Selected);
                        RefreshGraph();
                    }
                    break;
            }
        }

        //=========================================================================================
        // COPY / PASTE
        //

        //-------------------------------------------------------------------
        void CopyToClipboard(TaxonTreeNode _node)
        {
            if (_node == null) return;

            DataObject data_object = new DataObject();
            // Add the data in various formats.
            System.IO.MemoryStream ms = _node.SaveXMLInMemory();
            if (ms != null)
                data_object.SetData("TaxonTreeNodeXmlMemoryStream", ms);
            data_object.SetData(DataFormats.Text, _node.GetHierarchicalName());
            Clipboard.SetDataObject(data_object);
        }

        bool CanPasteClipboard(TaxonTreeNode _to)
        {
            if (Selected == null || Selected.IsFiltered()) return false;
            return Clipboard.ContainsData("TaxonTreeNodeXmlMemoryStream");
        }

        void PasteClipboard(TaxonTreeNode _to )
        { 
            if (Clipboard.ContainsData("TaxonTreeNodeXmlMemoryStream"))
            {
                object o = Clipboard.GetData("TaxonTreeNodeXmlMemoryStream");
                if (o is System.IO.MemoryStream ms)
                {
                    TaxonTreeNode node = TaxonTreeNode.LoadXMLFromMemory(ms);
                    if (node != null)
                        ImportNode(_to, node);
                }
            }
            else if (Clipboard.ContainsData(DataFormats.Text))
            {
                string name = Clipboard.GetText();
                if (name == null) return;
                TaxonTreeNode node = Root.FindTaxonByFullName(name);
                if (node != null)
                {
                    TaxonUtils.GotoTaxon(node);
                    TaxonUtils.SelectTaxon(node);
                }
            }
        }
    }
}
