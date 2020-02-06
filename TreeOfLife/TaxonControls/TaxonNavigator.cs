using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife
{
    [Description("Navigator")]
    [DisplayName("Navigator")]
    [Controls.IconAttribute("TaxonNavigator")]
    public partial class TaxonNavigator : Controls.TaxonControl
    {
        //-------------------------------------------------------------------
        public TaxonNavigator()
        {
            BorderPen = new Pen(Color.White, width: 1) { DashPattern = new[] { 5f, 5f } };
            SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint | System.Windows.Forms.ControlStyles.UserPaint | System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            OnOptionChanged();
        }

        Pen BorderPen;

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Navigator"; }

        //---------------------------------------------------------------------------------
        public override void OnOptionChanged()
        {
            base.OnOptionChanged();
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public override void OnRefreshGraph()
        {
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public override TaxonTreeNode Root
        {
            set
            {
                if (_Root == value) return;
                _Root = value;
                _Graph = null;
                RefreshGraph();
            }
        }

        //-------------------------------------------------------------------
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public override void OnRefreshAll()
        {
            RefreshGraph();
        }

        //-------------------------------------------------------------------
        public override void OnViewRectangleChanged(Rectangle R)
        {
            SetSubRectangle(R);
        }

        //-------------------------------------------------------------------
        public override void OnSelectTaxon(TaxonTreeNode _taxon)
        {
            Invalidate();
        }

        //-------------------------------------------------------------------
        Bitmap _Graph = null;
        public Bitmap Graph
        {
            get { return _Graph; }
            private set
            {
                if (_Graph == value) return;
                if (_Graph != null)
                {
                    _Graph.Dispose();
                    _Graph = null;
                }
                _Graph = value;
            }
        }

        //todo passer en double
        float _WidthRatio = 1;
        float _HeightRatio = 1;
        float _FinalWidthRatio = 1;
        float _FinalHeightRatio = 1;

        //-------------------------------------------------------------------
        bool _TreeAspect { get { return TaxonUtils.MainGraph.Graph.Options.DisplayMode == GraphOptions.DisplayModeEnum.Lines; } }

        List<TaxonGraphPanel.SimpleGraphParams.RectAndName> _PickableNames;
        public TaxonGraphPanel.SimpleGraphParams.RectAndName NameHovered
        {
            get => _NameHovered;
            private set
            {
                if (_NameHovered == value) return;
                _NameHovered = value;
                Invalidate();
            }
        }
        TaxonGraphPanel.SimpleGraphParams.RectAndName _NameHovered;

        //-------------------------------------------------------------------
        public void RefreshGraph()
        {
            _PickableNames = null;
            NameHovered = null;
            if (_Root == null) return;

            int width, height;
            width = _Root.WidthWithChildren;
            height = _Root.HeightWithChildren;
            
            if (width <= 0 || height <= 0) return;

            _WidthRatio = 1;
            _HeightRatio = 1;

            bool treeAspect = _TreeAspect;
            float wlink = 3.0f;
            float hlink = 3.0f;

            // new display
            if (treeAspect)
            {
                if (width > 1024)
                {
                    _WidthRatio = 1024 / (float)width;
                    width = 1024;
                    wlink /= _WidthRatio;
                }

                if (height > 2048)
                {
                    _HeightRatio = 2048 / (float)height;
                    height = 2048;
                    hlink /= _HeightRatio;
                }
            }
            else
            { 
                _WidthRatio = 5.0f / (float)_Root.R.Width;
                width = (int)(width * _WidthRatio);

                if (height > 2048)
                {
                    _HeightRatio = 2048 / (float)height;
                    height = 2048;
                }
            }

            int leftMargin = TaxonUtils.MyConfig.Options.NavigatorLeftMargin;
            float BitmapToScreenHeightRatio = 1.0f;
            if (height > Height) BitmapToScreenHeightRatio = (float)Height / (float)height;
            float BitmapToScreenWidthRatio = 1.0f;
            if (width > Width) BitmapToScreenWidthRatio = (float) Width / (float)width;

            TaxonGraphPanel.SimpleGraphParams paintParams = 
                new TaxonGraphPanel.SimpleGraphParams(_Root)
                        .SetScaleTaxonToGraph(_WidthRatio, _HeightRatio)
                        .SetLinkSize(wlink, hlink)
                        .SetLeftMargin(leftMargin)
                        .SetScaleGraphToScreen(BitmapToScreenWidthRatio, BitmapToScreenHeightRatio);
            paintParams.Colors = TaxonUtils.MyConfig.Options.NavigatorTreeColor;
            BackColor = paintParams.Colors.BackColor;

            Graph = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(Graph);
            TaxonGraphPanel.PaintTaxonRecursiveSimple(g, paintParams, _Root, treeAspect);
            _PickableNames = paintParams.SecondPassNames;
            g.Dispose();
            Invalidate();
        }

        //-------------------------------------------------------------------
        private Rectangle _SubRectangle = Rectangle.FromLTRB(0, 0, 0, 0);
        public void SetSubRectangle(Rectangle _rectangle)
        {
            _SubRectangle = _rectangle;
            Invalidate();
        }

        //-------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_Graph == null) return;
            Rectangle R = ClientRectangle;
            if (R.Width > Graph.Width) R.Width = Graph.Width;
            if (R.Height > Graph.Height) R.Height = Graph.Height;
            e.Graphics.DrawImage(_Graph, R);

            if (NameHovered != null)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.White)), NameHovered.Rect);
            }

            _FinalHeightRatio = (_HeightRatio * R.Height) / Graph.Height;
            _FinalWidthRatio = (_WidthRatio * R.Width) / Graph.Width;
            e.Graphics.ScaleTransform(_FinalWidthRatio, _FinalHeightRatio);

            TaxonTreeNode taxon = TaxonUtils.SelectedTaxon();
            if (taxon != null)
            {
                if (_TreeAspect && !taxon.IsEndLeaf)
                {
                    R = taxon.R;
                    R.Width = Root.WidthWithChildren - R.Left;
                    e.Graphics.FillRectangle(TaxonUtils.MyConfig.Options.NavigatorSelectedRectangleBrush, R);
                }
                else
                {
                    R = taxon.R;
                    Brush brush = Brushes.Gold;
                    if (TaxonUtils.MainGraph != null)
                        brush = TaxonUtils.MainGraph.Graph.UsedColors.SelectedBrush;
                    if (taxon.IsEndLeaf)
                        R.Width = Root.WidthWithChildren - R.Left;
                    e.Graphics.FillRectangle(brush, R);
                }
            }

            e.Graphics.ResetTransform();
            R = _SubRectangle;
            R.X = (int)(R.X * _FinalWidthRatio);
            R.Width = (int)(R.Width * _FinalWidthRatio);
            R.Y = (int)(R.Y * _FinalHeightRatio);
            R.Height = (int)(R.Height * _FinalHeightRatio);
            if (R.Height < 10)
                R.Inflate(0, (10 - R.Height) / 2);
            e.Graphics.DrawRectangle(BorderPen, R);
        }

        //-------------------------------------------------------------------
        private void TaxonNavigator_Paint(object sender, PaintEventArgs e)
        {
            if (_Graph == null) return;
            e.Graphics.DrawImage(_Graph, ClientRectangle);
        }

        //-------------------------------------------------------------------
        bool move = false;
        int  mouseX, mouseY;

        //-------------------------------------------------------------------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Root == null) return;

            if ( (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Capture = true;
                move = true;
                mouseX = e.Location.X;
                mouseY = e.Location.Y;

                if (NameHovered != null)
                {
                    int centerX = _SubRectangle.Left + _SubRectangle.Width / 2;
                    int centerY = _SubRectangle.Top + _SubRectangle.Height / 2;
                    OnMoveRectangleArgs moveEvent = new OnMoveRectangleArgs
                    {
                        X = centerX - (int) (_NameHovered.RectF.Left + _NameHovered.RectF.Width / 2),
                        Y = centerY - (int) _NameHovered.RectF.Y 
                    };
                    OnMoveRectangle(this, moveEvent);
                }
                else
                {
                    Rectangle R = Rectangle.FromLTRB((int)(_SubRectangle.Left * _FinalWidthRatio), (int)(_SubRectangle.Top * _FinalHeightRatio), (int)(_SubRectangle.Right * _FinalWidthRatio), (int)(_SubRectangle.Bottom * _FinalHeightRatio));
                    R.Inflate(3, 3);
                    if (!R.Contains(e.Location))
                    {
                        int centerX = _SubRectangle.Left + _SubRectangle.Width / 2;
                        int centerY = _SubRectangle.Top + _SubRectangle.Height / 2;

                        OnMoveRectangleArgs moveEvent = new OnMoveRectangleArgs
                        {
                            X = centerX - (int)(mouseX / _FinalWidthRatio),
                            Y = centerY - (int)(mouseY / _FinalHeightRatio)
                        };
                        OnMoveRectangle(this, moveEvent);
                    }
                }
                
            }
        }

        //-------------------------------------------------------------------
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Root == null) return;
            if (!move || OnMoveRectangle == null)
            {
                TaxonTreeNode taxon = null;
                if (_PickableNames != null)
                {
                    foreach (var pn in _PickableNames)
                    {
                        if (pn.Displayed && pn.Rect.Contains(e.Location))
                        {
                            NameHovered = pn;
                            taxon = pn.Taxon;
                            break;
                        }
                    }
                }
                if (taxon == null)
                {
                    NameHovered = null;
                    taxon = TaxonGraphPanel.PickTaxonRectangularStatic(Root, (int)(e.Location.X / _FinalWidthRatio), (int)(e.Location.Y / _FinalHeightRatio));
                }
                TipManager.SetTaxon(taxon, null);
                return;
            }

            OnMoveRectangleArgs moveEvent = new OnMoveRectangleArgs
            {
                X = (int)((mouseX - e.Location.X) / _FinalWidthRatio),
                Y = (int)((mouseY - e.Location.Y) / _FinalHeightRatio)
            };
            mouseX = e.Location.X;
            mouseY = e.Location.Y;
            OnMoveRectangle(this, moveEvent);
        }

        //-------------------------------------------------------------------
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Capture = false;
            move = false;
        }

        //--------------------------------------------------------------------------------------
        protected override void OnMouseLeave(EventArgs e)
        {
            TipManager.SetTaxon(null, null);
            base.OnMouseLeave(e);
        }

        //-------------------------------------------------------------------
        public class OnMoveRectangleArgs : EventArgs
        {
            public int X, Y;
        }

        public delegate void OnMoveRectangleEventHandler(object sender, OnMoveRectangleArgs e);

        public event OnMoveRectangleEventHandler OnMoveRectangle = null;
    }
}
