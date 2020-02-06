using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VinceToolbox.UserControl.Graph
{
    //=========================================================================================
    //
    // Space graph map : as in win dir stat 
    //
    //=========================================================================================
    partial class SpaceGraphMap
    {
        //-----------------------------------------------------------------------------------------
        // Tree map square style.
        //
        enum STYLE
        {
            SimpleStyle,		// simple but uninteresting style
            KDirStatStyle,		// Children are placed out in rows. Similar to the style used by KDirStat.
            SequoiaViewStyle	// The 'classical' as described in at http://www.win.tue.nl/~vanwijk/.
        };

        //-----------------------------------------------------------------------------------------
        // Collection of all tree map options.
        //
        class Options
        {
            public STYLE Style { get; set; }
        
            public bool Grid { get; set;}
            public Color GridColor { get; set; }

            public Options()
            {
                Style = STYLE.KDirStatStyle;
                Grid = false;
                GridColor = Color.Black;
            }

            public void set ( Options _other )
            {
                Style = _other.Style;
                Grid = _other.Grid;
                GridColor = _other.GridColor;
            }

            Color[] m_colors = new Color[] 
            {
                Color.FromArgb(0, 0, 255),
                Color.FromArgb(255, 0, 0),
                Color.FromArgb(0, 255, 0),
                Color.FromArgb(0, 255, 255),
                Color.FromArgb(255, 0, 255),
                Color.FromArgb(255, 255, 0),
                Color.FromArgb(150, 150, 255),
                Color.FromArgb(255, 150, 150),
                Color.FromArgb(150, 255, 150),
                Color.FromArgb(150, 255, 255),
                Color.FromArgb(255, 150, 255),
                Color.FromArgb(255, 255, 150),
                Color.FromArgb(255, 255, 255)
            };

            const double PALETTE_BRIGHTNESS = 0.6;
        };

        Options GetDefaultOptions() { return new Options(); }
        Options m_options = new Options();	// Current options
        Options CurrentOptions
        {
            get { return m_options; }
            set { m_options.set(value); }
        }

        //-----------------------------------------------------------------------------------------
        public SpaceGraphMap() {}

        //-----------------------------------------------------------------------------------------
        public void draw(Graphics _graphics, Rectangle R, SpaceGraphItem root)
        {
            if (R.IsEmpty) return;

            if (m_options.Grid)
                _graphics.FillRectangle( new SolidBrush(CurrentOptions.GridColor), R);
            else
            {
                _graphics.DrawLine( Pens.DarkGray, new Point(R.Right - 1, R.Top) , new Point(R.Right - 1, R.Bottom));
                _graphics.DrawLine( Pens.DarkGray, new Point(R.Left, R.Bottom - 1) , new Point(R.Right, R.Bottom - 1));
            }
            R.Width--;
            R.Height--;

            if (R.IsEmpty) return;

            if (root.TmiGetSize() > 0)
                RecurseDrawGraph(_graphics, root, R, true, 0);
            else
                _graphics.FillRectangle( Brushes.Black, R);
        }

        //-----------------------------------------------------------------------------------------
        void DrawTreemapDoubleBuffered(Graphics _graphics, Rectangle R, SpaceGraphItem root)
        {
            if (R.IsEmpty) return;
        
            Bitmap bmp = new Bitmap(R.Width, R.Height, _graphics);
            
            Rectangle bmpRect = R;
            bmpRect.X = bmpRect.Y = 0;
            Graphics bmpGraphics = Graphics.FromImage(bmp);
            draw(bmpGraphics, bmpRect, root);

            _graphics.DrawImage(bmp, new Point(R.X, R.Y));
        }

        //-----------------------------------------------------------------------------------------
        public SpaceGraphItem FindItemByPoint(SpaceGraphItem item, Point point)
        {
            Rectangle R = item.TmiGetRectangle();

            if (!R.Contains(point))
                return null;

            SpaceGraphItem result = null;
            int gridWidth = m_options.Grid ? 1: 0;
            if (R.Width <= gridWidth || R.Height <= gridWidth)
                result = item;
            else if (item.TmiIsLeaf())
                result = item;
            else
            {
                for (int i=0; i < item.TmiGetChildrenCount(); i++)
                {
                    SpaceGraphItem child = item.TmiGetChild(i);

                    if (child.TmiGetRectangle().Contains(point))
                    {
                        result = FindItemByPoint(child, point);
                        break;
                    }
                }
            }

            if (result == null)
                result = item;

            return result;
        }

        //-----------------------------------------------------------------------------------------
        void DrawColorPreview(Graphics _graphics, Rectangle R, Color color)
        {
            RenderRectangle( _graphics, R, color);
            if (m_options.Grid)
            {
                Pen gridPen = new Pen(m_options.GridColor);
                _graphics.DrawRectangle( gridPen, R );
            }
        }

        //-----------------------------------------------------------------------------------------
        void RecurseDrawGraph(Graphics _graphics, SpaceGraphItem item, Rectangle R, bool asroot, UInt32 flags)
        {
            //if (m_callback != NULL) m_callback->TreemapDrawingCallback();
            item.TmiSetRectangle(R);
            if (R.IsEmpty) return;
        
            int gridWidth = m_options.Grid ? 1 : 0;
            if (item.TmiIsLeaf())
                RenderLeaf(_graphics, item);
            else
                DrawChildren(_graphics, item, flags);
        }

        //-----------------------------------------------------------------------------------------
        void DrawChildren(Graphics _graphics, SpaceGraphItem parent, UInt32 flags)
        {
            switch (m_options.Style)
            {
            case STYLE.KDirStatStyle:       KDirStat_DrawChildren(_graphics, parent, flags); break;
            case STYLE.SequoiaViewStyle:    SequoiaView_DrawChildren(_graphics, parent, flags); break;
            case STYLE.SimpleStyle:         Simple_DrawChildren(_graphics, parent, flags); break;
            }
        }

        //-----------------------------------------------------------------------------------------   
        void RenderLeaf(Graphics _graphics, SpaceGraphItem item)
        {
            Rectangle R = item.TmiGetRectangle();
            if (m_options.Grid)
            {
                R.Inflate(-1,-1);
                if (R.IsEmpty) return;
            }
            RenderRectangle( _graphics, R, item.TmiGetGraphColor());
        }

        //-----------------------------------------------------------------------------------------   
        void RenderRectangle(Graphics _graphics, Rectangle rc, Color color)
        {
            _graphics.FillRectangle(new SolidBrush(color), rc);

            Point topLeft = new Point( rc.Left, rc.Top);
            Point topRight = new Point( rc.Right - 1, rc.Top);
            Point bottomLeft = new Point( rc.Left, rc.Bottom - 1 );
            Point bottomRight = new Point( rc.Right - 1, rc.Bottom - 1);
            
            Color light = ColorExtended.ModifyBrightness(color, 1.6);
            Pen lightPen = new Pen( light, 1 );
            _graphics.DrawLine(lightPen, bottomLeft, topLeft);
            _graphics.DrawLine(lightPen, topLeft, topRight);

            Color dark = ColorExtended.ModifyBrightness(color, 0.4);
            Pen darkPen = new Pen(dark, 1);
            _graphics.DrawLine(darkPen, topRight, bottomRight);
            _graphics.DrawLine(darkPen, bottomRight, bottomLeft);
        }
    }
}
