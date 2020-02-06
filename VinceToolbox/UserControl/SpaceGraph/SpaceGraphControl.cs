using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VinceToolbox.UserControl.Graph
{
    //=========================================================================================
    //
    // Space graph Control:
    //      draw a space graph tree from give space graph data
    //
    //=========================================================================================

    public partial class SpaceGraphControl : System.Windows.Forms.UserControl
    {
        //-----------------------------------------------------------------------------------------
        public SpaceGraphControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);

            createData();
            createDrawingData();
        }

        //-----------------------------------------------------------------------------------------
        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg == 0x14) return;
            base.OnNotifyMessage(m);
        }

        //-----------------------------------------------------------------------------------------
        // treemap : class that arragne space graph data 
        SpaceGraphMap m_treemap = new SpaceGraphMap();
        
        // False if no treemap displayed
        private bool m_showTreemap = true;				    
        public bool ShowTreemap { get { return m_showTreemap; } set { m_showTreemap = value; } }

        //-----------------------------------------------------------------------------------------
        public  bool    IsDrawn { get { return m_bitmap != null;} }
        private Bitmap  m_bitmap = null;				        
        Bitmap          m_dimmed = null;				        // Dimmed view. Used during refresh to avoid the ooops-effect.

        //-----------------------------------------------------------------------------------------
        Brush m_hilightBrush = null;
        Brush m_selectBrush = null;
        private void createDrawingData()
        {
            m_selectBrush = new SolidBrush( Color.FromArgb(80,255,0,0));
            m_hilightBrush = new SolidBrush(Color.FromArgb(80, 0, 255, 255));
        }

        //-----------------------------------------------------------------------------------------
        private SpaceGraphData m_data = null;
        [Browsable(false)]
        public SpaceGraphData Data { get { return m_data; } }

        public void createData()
        {
            m_data = new SpaceGraphData();
            m_data.OnRootChanged = onRootChanged;
            m_data.OnHoverItemChanged = onHilightChanged;
            m_data.OnSelectedItemsChanged = onHilightChanged;
        }
        
        public SpaceGraphItem Root 
        {
            get { return m_data.Root; }
            set { m_data.Root = value; }
        }

        //-----------------------------------------------------------------------------------------
        void onRootChanged(SpaceGraphData _data)
        {
            Inactivate();
        }

        //-----------------------------------------------------------------------------------------
        void onHilightChanged(SpaceGraphData _data)
        {
            Invalidate();
        }

        //-----------------------------------------------------------------------------------------
        void Inactivate()
        {
            if (m_bitmap == null) return;
            if (m_dimmed != null) m_dimmed.Dispose();
            m_dimmed = m_bitmap;
            m_bitmap = null;
            Invalidate();
        }

        //-----------------------------------------------------------------------------------------
        void DrawEmpty() { DrawEmpty(CreateGraphics()); }
        //-----------------------------------------------------------------------------------------
        void DrawEmpty( Graphics _graphics )
        {
            Inactivate();

            if (m_dimmed == null)
            {
                _graphics.FillRectangle( Brushes.Gray, ClientRectangle);
                return;
            }

            _graphics.DrawImage( m_dimmed, 0, 0);
            _graphics.FillRectangle( new SolidBrush( Color.FromArgb(128,0,0,0)), ClientRectangle);
                
            if (ClientRectangle.Width > m_dimmed.Width)
            {
                Rectangle R = ClientRectangle;
                R.X = R.Left + m_dimmed.Width;
                R.Width -= m_dimmed.Width;
                _graphics.FillRectangle( Brushes.Gray, R);
            }

            if (ClientRectangle.Height > m_dimmed.Height)
            {
                Rectangle R = ClientRectangle;
                R.Y = R.Top + m_dimmed.Height;
                R.Height -= m_dimmed.Height;
                _graphics.FillRectangle(Brushes.Gray, R);
            }
        }

        //-----------------------------------------------------------------------------------------
        protected override void OnSizeChanged(EventArgs e)
        {
            Inactivate();
        }

        //-----------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs paintEvnt)
        {
            if (!ShowTreemap || m_data == null || !m_data.IsReadyForDrawing)
            {
                DrawEmpty( paintEvnt.Graphics);
                return;
            }

            Rectangle R = ClientRectangle;

            if (!IsDrawn)
            {
                UseWaitCursor = true;
                m_bitmap = new Bitmap( R.Width, R.Height, paintEvnt.Graphics );
                Graphics graphics = Graphics.FromImage(m_bitmap);
                m_treemap.draw( graphics, R, Root );
                UseWaitCursor = false;
            }

            if (m_bitmap == null)
            {
                DrawEmpty(paintEvnt.Graphics);
                return;
            }

            paintEvnt.Graphics.DrawImage( m_bitmap, 0, 0);
            DrawHighlights(paintEvnt.Graphics);
        }

        //-----------------------------------------------------------------------------------------
        void RenderHighlightRectangle(Graphics _graphics, Rectangle rc, Brush _brush)
        {
            _graphics.FillRectangle(_brush, rc);
        }

        //-----------------------------------------------------------------------------------------
        void DrawHighlights(Graphics _graphics)
        {
            if (m_data.HoverItem != null)
                RenderHighlightRectangle(_graphics, m_data.HoverItem.TmiGetRectangle(), m_hilightBrush);
            if (m_data.m_selectedItems != null)
            {
                foreach (SpaceGraphItem item in m_data.m_selectedItems)
                    RenderHighlightRectangle(_graphics, item.TmiGetRectangle(), m_selectBrush);
            }
        }

        //-----------------------------------------------------------------------------------------
        private void SpaceGraphControl_MouseLeave(object sender, EventArgs e)
        {
            m_data.HoverItem = null;
        }

        //-----------------------------------------------------------------------------------------
        private void SpaceGraphControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = PointToClient(Cursor.Position);
            m_data.HoverItem = m_treemap.FindItemByPoint(Root, position);
        }
    }
}
