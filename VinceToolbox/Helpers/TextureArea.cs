using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
//using System.Windows.Input;

namespace VinceToolbox
{

    public class TextureArea : IDisposable
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        //-----------------------------------------------------------------------------------------
        // members
        //
        Image m_texture = null;
        Color m_backColor = Color.Black;
        bool m_bilinearFilter = true;

        Rectangle m_winRect = new Rectangle();          // window rect to display atlas
        Rectangle m_winTextureRect = new Rectangle();   // window rect to display texture (keeping texture ratio)
        RectangleF m_textureRect = new RectangleF();    // rect of texture to be displayed
        RectangleF m_capturedTextureRect;
        System.Drawing.Point m_captureStart;

        RectangleF m_allTextureRectRatio = new RectangleF();
        Rectangle m_allTextureRect = new Rectangle();
        Rectangle m_capturedAllTextureRect;

        //-----------------------------------------------------------------------------------------
        Bitmap m_TextureCache = null;
        Rectangle m_TextureCacheTextureRect = Rectangle.FromLTRB(0, 0, 0, 0);
        Rectangle m_TextureCacheWindowRect = Rectangle.FromLTRB(0, 0, 0, 0);
        Rectangle m_TextureCacheRect = Rectangle.FromLTRB(0, 0, 0, 0);

        
        //-----------------------------------------------------------------------------------------
        // init
        //
        public void init()
        {
            m_textureRect = new RectangleF(0, 0, 1.0f, 1.0f);
            m_allTextureRectRatio = new RectangleF(0, 0, 1.0f, 1.0f);
        }

        //-----------------------------------------------------------------------------------------
        public void Dispose()
        {
            if (m_texture != null) { m_texture.Dispose(); m_texture = null; }
            if (m_TextureCache != null) { m_TextureCache.Dispose(); m_TextureCache = null; }
        }


        //-----------------------------------------------------------------------------------------
        // accessors
        //
        public void setTexture(Image _texture) 
        { 
            m_texture = _texture;
            if (m_TextureCache != null)
            {
                m_TextureCache.Dispose();
                m_TextureCache = null;
            }
            setWinRect(m_winRect);
        }

        public Image getTextureClone() { return m_texture.Clone() as Image; }
        public void setBackColor(Color _backColor) { m_backColor = _backColor; }
        public void setBilinearFilter(bool _bilinearFilter) { m_bilinearFilter = _bilinearFilter; }

        public void setWinRect(Rectangle _rectangle)
        {
            m_winRect = _rectangle;
            m_winTextureRect = _rectangle;

            if (m_texture == null) return;

            int y = (m_winTextureRect.Width * m_texture.Height) / m_texture.Width;
            if (y < m_winTextureRect.Height)
            {
                m_winTextureRect.Y = m_winTextureRect.Y + (m_winTextureRect.Height - y) / 2;
                m_winTextureRect.Height = y;
            }
            else
            {
                int x = (m_winTextureRect.Height * m_texture.Width) / m_texture.Height;
                m_winTextureRect.X = m_winTextureRect.X + (m_winTextureRect.Width - x) / 2;
                m_winTextureRect.Width = x;
            }

            m_allTextureRectRatio.X = (float)(m_winTextureRect.Left - m_winRect.Left) / (float)m_winRect.Width;
            m_allTextureRectRatio.Width = (float)m_winTextureRect.Width / (float)m_winRect.Width;
            m_allTextureRectRatio.Y = (float)(m_winTextureRect.Top - m_winRect.Top) / (float)m_winRect.Height;
            m_allTextureRectRatio.Height = (float)m_winTextureRect.Height / (float)m_winRect.Height;
        }

        public Rectangle getWinRect() { return m_winRect; }
        public Rectangle getWinTextureRect() { return m_winTextureRect; }
        public void setSubRect(RectangleF _rectangleF) { m_textureRect = _rectangleF; }
        public RectangleF getSubRect() { return m_textureRect; }
        public int getTextureWidth() { return m_texture != null ? m_texture.Width : 0; }
        public int getTextureHeight() { return m_texture != null ? m_texture.Height : 0; }

        bool m_methodOld = false;

        //-----------------------------------------------------------------------------------------
        // clip painting region
        //
        public void clip(ref Graphics _graphics)
        {
            if (m_methodOld)
                _graphics.Clip = new Region(getWinTextureRect());
            else
                _graphics.Clip = new Region(getWinRect());
        }

        //-----------------------------------------------------------------------------------------
        // paint
        //
        public void paintOld(Graphics _graphics)
        {
            // display background
            Region region = new Region(getWinRect());
            region.Exclude(getWinTextureRect());
            _graphics.Clip = region;
            _graphics.FillRectangle(new SolidBrush(Color.Black), getWinRect());
            _graphics.Clip = new Region(getWinTextureRect());

            // display image
            if (m_texture != null)
            {
                Rectangle displayRectangle = new Rectangle();

                try
                {
                    RectangleF subRect = getSubRect();
                    displayRectangle.X = (int)(getSubRect().X * (m_texture.Width));
                    displayRectangle.Y = (int)(getSubRect().Y * (m_texture.Height));
                    displayRectangle.Width = (int)(getSubRect().Width * (m_texture.Width));
                    displayRectangle.Height = (int)(getSubRect().Height * (m_texture.Height));
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                

                Rectangle windowRect = getWinTextureRect();

                if (m_TextureCache == null || m_TextureCacheTextureRect != displayRectangle || m_TextureCacheWindowRect != windowRect)
                {
                    if (m_TextureCacheWindowRect != windowRect || m_TextureCache == null)
                    {
                        if (m_TextureCache != null) m_TextureCache.Dispose();
                        m_TextureCache = null;
                        if (windowRect.Width > 0 && windowRect.Height > 0)
                        {
                            m_TextureCache = new Bitmap(m_texture, windowRect.Width, windowRect.Height);
                            m_TextureCacheWindowRect = windowRect;
                        }
                    }
                    if (m_TextureCache != null)
                    {
                        Graphics g = Graphics.FromImage(m_TextureCache);
                        g.Clear(m_backColor);

                        if (m_bilinearFilter)
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                        else
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                        m_TextureCacheRect.Width = windowRect.Width;
                        m_TextureCacheRect.Height = windowRect.Height;
                        g.CompositingMode = CompositingMode.SourceOver;
                        g.DrawImage(m_texture, m_TextureCacheRect, displayRectangle, GraphicsUnit.Pixel);
                        g.Dispose();
                        m_TextureCacheTextureRect = displayRectangle;
                    }
                }
                if (m_TextureCache != null)
                {
                    _graphics.CompositingMode = CompositingMode.SourceCopy;
                    _graphics.DrawImage(m_TextureCache, windowRect.Left, windowRect.Top);
                    _graphics.CompositingMode = CompositingMode.SourceOver;
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        public void computeDisplayRect()
        {
            m_allTextureRect.X = (int)(m_allTextureRectRatio.X * m_winRect.Width + m_winRect.Left);
            m_allTextureRect.Width = (int)(m_allTextureRectRatio.Width * m_winRect.Width);
            m_allTextureRect.Y = (int)(m_allTextureRectRatio.Y * m_winRect.Height + m_winRect.Top);
            m_allTextureRect.Height = (int)(m_allTextureRectRatio.Height * m_winRect.Height);

            m_winTextureRect = m_allTextureRect;
            m_winTextureRect.Intersect(m_winRect);

            m_textureRect.X = (float)(m_winTextureRect.X - m_allTextureRect.X) / (float)m_allTextureRect.Width;
            m_textureRect.Width = (float)m_winTextureRect.Width / (float)m_allTextureRect.Width;
            m_textureRect.Y = (float)(m_winTextureRect.Y - m_allTextureRect.Y) / (float)m_allTextureRect.Height;
            m_textureRect.Height = (float)m_winTextureRect.Height / (float)m_allTextureRect.Height;
        }

        //-----------------------------------------------------------------------------------------
        public void computeAllTextureRect()
        {
            m_allTextureRect.Width = (int)(m_winTextureRect.Width / m_textureRect.Width);
            m_allTextureRect.Height = (int)(m_winTextureRect.Height / m_textureRect.Height);
            m_allTextureRect.X = (int)(m_winTextureRect.X - m_textureRect.X * m_allTextureRect.Width);
            m_allTextureRect.Y = (int)(m_winTextureRect.Y - m_textureRect.Y * m_allTextureRect.Height);
            computeAllTextureRectRatio();
        }

        //-----------------------------------------------------------------------------------------
        public void computeAllTextureRectRatio()
        {
            m_allTextureRectRatio.X = (float)(m_allTextureRect.Left - m_winRect.Left) / (float)m_winRect.Width;
            m_allTextureRectRatio.Width = (float)m_allTextureRect.Width / (float)m_winRect.Width;
            m_allTextureRectRatio.Y = (float)(m_allTextureRect.Top - m_winRect.Top) / (float)m_winRect.Height;
            m_allTextureRectRatio.Height = (float)m_allTextureRect.Height / (float)m_winRect.Height;
        }

        //-----------------------------------------------------------------------------------------
        // paint
        //
        public void paintNew(Graphics _graphics)
        {
            // display image
            if (m_texture == null)
            {
                _graphics.FillRectangle(new SolidBrush(Color.Black), getWinRect());
                return;
            }

            computeDisplayRect();
            if (m_winTextureRect.IsEmpty)
            {
                _graphics.FillRectangle(new SolidBrush(Color.Black), getWinRect());
                return;
            }

            paintOld(_graphics);
        }

        //-----------------------------------------------------------------------------------------
        // paint
        //
        public void paint(Graphics _graphics)
        {
            lock (this)
            {
                if (m_methodOld)
                    paintOld(_graphics);
                else
                    paintNew(_graphics);
            }
        }

        //-----------------------------------------------------------------------------------------
        public void paintGrid(Graphics _graphics, System.Drawing.Size _grid, Color _gridColor)
        {
            Region oldRegion = _graphics.Clip;
            clip(ref _graphics);
            Pen penGrid = new Pen(_gridColor, 2);
            System.Drawing.Point point0 = new System.Drawing.Point(), point1 = new System.Drawing.Point();

            point0.Y = y_atlas2win(0);
            point1.Y = y_atlas2win(1);
            for (int i = 0; i <= _grid.Width; i++)
            {
                float x = (float)i / (float)_grid.Width;
                point0.X = point1.X = x_atlas2win(x);
                _graphics.DrawLine(penGrid, point0, point1);
            }

            point0.X = x_atlas2win(0);
            point1.X = x_atlas2win(1);
            for (int j = 0; j <= _grid.Height; j++)
            {
                float y = (float)j / (float)_grid.Height;
                point0.Y = point1.Y = y_atlas2win(y);
                _graphics.DrawLine(penGrid, point0, point1);
            }
            _graphics.Clip = oldRegion;
        }

        //-----------------------------------------------------------------------------------------
        // picking
        //
        public bool isWinPointInside(System.Drawing.Point _point) { return m_winTextureRect.Contains(_point); }

        //-----------------------------------------------------------------------------------------
        // edit sub rect
        //
        public void subRectZoom(System.Drawing.Point _pos, bool _out)
        {
            float xwinratio = (float)(_pos.X - m_winTextureRect.Left) / (float)m_winTextureRect.Width;
            float ywinratio = (float)(_pos.Y - m_winTextureRect.Top) / (float)m_winTextureRect.Height;

            float xtextureratioBefore = m_textureRect.Left + xwinratio * (m_textureRect.Width);
            float ytextureratioBefore = m_textureRect.Top + ywinratio * (m_textureRect.Height);

            float w = (m_textureRect.Width * 0.05f) * (_out ? 1 : -1);
            float h = (m_textureRect.Height * 0.05f) * (_out ? 1 : -1);
            m_textureRect.Inflate(w, h);

            float xtextureratioAfter = m_textureRect.Left + xwinratio * (m_textureRect.Width);
            float ytextureratioAfter = m_textureRect.Top + ywinratio * (m_textureRect.Height);

            m_textureRect.X += xtextureratioBefore - xtextureratioAfter;
            m_textureRect.Y += ytextureratioBefore - ytextureratioAfter;

            computeAllTextureRect();
        }

        public void subRectStartPan(System.Drawing.Point _pos)
        {
            m_captureStart = _pos;
            m_capturedTextureRect = getSubRect();
            m_capturedAllTextureRect = m_allTextureRect;
        }

        public void subRectPan(System.Drawing.Point _pos)
        {
            if (m_methodOld)
            {
                float ratiox = m_capturedTextureRect.Width / (float)m_winTextureRect.Width;
                m_textureRect.X = m_capturedTextureRect.X + (m_captureStart.X - _pos.X) * ratiox;
                float ratioy = m_capturedTextureRect.Height / (float)m_winTextureRect.Height;
                m_textureRect.Y = m_capturedTextureRect.Y + (m_captureStart.Y - _pos.Y) * ratioy;
                computeAllTextureRect();
            }
            else
            {
                m_allTextureRect.X = m_capturedAllTextureRect.X - (m_captureStart.X - _pos.X);
                m_allTextureRect.Y = m_capturedAllTextureRect.Y - (m_captureStart.Y - _pos.Y);
                computeAllTextureRectRatio();
            }
        }

        //-----------------------------------------------------------------------------------------
        // mouse function
        //
        bool m_capturePan = false;

        public bool mouseDown(System.Drawing.Point _pos)
        {
            if (!isWinPointInside(_pos)) return false;
            if (GetKeyState(32 /*VK_SPACE*/ ) >= 0) return false;

            m_capturePan = true;
            subRectStartPan(_pos);
            return true;
        }

        public void mouseMove(System.Drawing.Point _pos)
        {
            if (!m_capturePan) return;
            subRectPan(_pos);
        }

        public void mouseUp(System.Drawing.Point _pos)
        {
            m_capturePan = false;
        }

        public bool mouseWheel(System.Drawing.Point _pos, bool _out)
        {
            if (!isWinPointInside(_pos)) return false;
            if (GetKeyState(32 /*VK_SPACE*/ ) >= 0) return false;

            subRectZoom(_pos, _out);
            return true;
        }


        public bool mouseSetCursor(System.Drawing.Point _pos, ref Cursor _cursor)
        {
            if (!isWinPointInside(_pos)) return false;
            if (GetKeyState(32 /*VK_SPACE*/ ) >= 0) return false;

            _cursor = Cursors.Hand;
            return true;
        }

        //-----------------------------------------------------------------------------------------
        // transformation function
        //
        public int x_atlas2win(float _xAtlas)
        {
//             int xTexture = (int)(_xAtlas * m_texture.Width + 0.5);
//             float xTextureDisplayed = ((float)xTexture - m_TextureCacheTextureRect.Left) / (float)m_TextureCacheTextureRect.Width;
//             int xTextureWindow = (int)m_TextureCacheWindowRect.Left + (int)(m_TextureCacheWindowRect.Width * xTextureDisplayed);

            int xTextureWindow = (int)(_xAtlas * m_allTextureRect.Width + m_allTextureRect.Left + 0.5);
            return xTextureWindow;
        }
        public int y_atlas2win(float _yAtlas)
        {
//             int yTexture = (int)(_yAtlas * m_texture.Height + 0.5);
//             float yTextureDisplayed = ((float)yTexture - m_TextureCacheTextureRect.Top) / (float)m_TextureCacheTextureRect.Height;
//             int yTextureWindow = (int)m_TextureCacheWindowRect.Top + (int)(m_TextureCacheWindowRect.Height * yTextureDisplayed);
            int yTextureWindow = (int)(_yAtlas * m_allTextureRect.Height + m_allTextureRect.Top + 0.5);
            return yTextureWindow;
        }
        public System.Drawing.Point point_atlas2win(PointF _pos)
        {
            return new System.Drawing.Point(x_atlas2win(_pos.X), y_atlas2win(_pos.Y));
        }

        public Rectangle rect_atlas2win(RectangleF _rect)
        {
            int x0 = x_atlas2win(_rect.X);
            int y0 = y_atlas2win(_rect.Y);
            int x1 = x_atlas2win(_rect.Right);
            int y1 = y_atlas2win(_rect.Bottom);
            return Rectangle.FromLTRB(x0, y0, x1, y1);
        }

        public float x_win2atlas(int _xWin)
        {
            float x = _xWin - m_winTextureRect.Left;
            x /= (float)m_winTextureRect.Width;
            return x * m_textureRect.Width + m_textureRect.Left;
        }

        public float y_win2atlas(int _yWin)
        {
            float y = _yWin - m_winTextureRect.Top;
            y /= (float)m_winTextureRect.Height;
            return y * m_textureRect.Height + m_textureRect.Top;
        }

        public PointF point_win2atlas(System.Drawing.Point _pos)
        {
            return new PointF(x_win2atlas(_pos.X), y_win2atlas(_pos.Y));
        }

        public RectangleF rect_win2atlas(Rectangle _rect)
        {
            float x0 = x_win2atlas(_rect.X);
            float y0 = y_win2atlas(_rect.Y);
            float x1 = x_win2atlas(_rect.Right);
            float y1 = y_win2atlas(_rect.Bottom);
            return RectangleF.FromLTRB(x0, y0, x1, y1);
        }
    }

}