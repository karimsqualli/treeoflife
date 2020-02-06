using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VinceToolbox
{

    //=========================================================================================
    //
    // ColorExtended : HSL / RGB
    //
    //=========================================================================================
    public class ColorExtended
    {
        public ColorExtended() { }

        //-----------------------------------------------------------------------------------------
        // gets the brightness of a color 
        //
        public static double GetBrightness(Color c)
        {
            HSL hsl = RGB_to_HSL(c);
            return hsl.L;
        }
        
        //-----------------------------------------------------------------------------------------
        // Sets the absolute brightness of a color 
        //
        public static Color SetBrightness(Color c, double brightness)
        {
            HSL hsl = RGB_to_HSL(c);
            hsl.L = brightness;
            return HSL_to_RGB(hsl);
        }

        //-----------------------------------------------------------------------------------------
        //  Modifies an existing brightness level 
        //
        public static Color ModifyBrightness(Color c, double brightness)
        {
            HSL hsl = RGB_to_HSL(c);
            hsl.L *= brightness;
            return HSL_to_RGB(hsl);
        }

        //-----------------------------------------------------------------------------------------
        //  Sets the absolute saturation level 
        //
        public static Color SetSaturation(Color c, double Saturation)
        {
            HSL hsl = RGB_to_HSL(c);
            hsl.S = Saturation;
            return HSL_to_RGB(hsl);
        }

        //-----------------------------------------------------------------------------------------
        //  Modifies an existing Saturation level 
        //
        public static Color ModifySaturation(Color c, double Saturation)
        {
            HSL hsl = RGB_to_HSL(c);
            hsl.S *= Saturation;
            return HSL_to_RGB(hsl);
        }

        //-----------------------------------------------------------------------------------------
        //  set absolute hue level 
        //
        public static Color SetHue(Color c, double Hue)
        {
            HSL hsl = RGB_to_HSL(c);
            hsl.H = Hue;
            return HSL_to_RGB(hsl);
        }

        //-----------------------------------------------------------------------------------------
        //  Modifies an existing Hue level 
        //
        public static Color ModifyHue(Color c, double Hue)
        {
            HSL hsl = RGB_to_HSL(c);
            hsl.H *= Hue;
            return HSL_to_RGB(hsl);
        }

        //-----------------------------------------------------------------------------------------
        //  Converts a color from HSL to RGB 
        //
        public static Color HSL_to_RGB(HSL hsl)
        {
            int Max, Mid, Min;
            double q;

            Max = Round(hsl.L * 255);
            Min = Round((1.0 - hsl.S) * (hsl.L / 1.0) * 255);
            q = (double)(Max - Min) / 255;

            if (hsl.H >= 0 && hsl.H <= (double)1 / 6)
            {
                Mid = Round(((hsl.H - 0) * q) * 1530 + Min);
                return Color.FromArgb(Max, Mid, Min);
            }
            else if (hsl.H <= (double)1 / 3)
            {
                Mid = Round(-((hsl.H - (double)1 / 6) * q) * 1530 + Max);
                return Color.FromArgb(Mid, Max, Min);
            }
            else if (hsl.H <= 0.5)
            {
                Mid = Round(((hsl.H - (double)1 / 3) * q) * 1530 + Min);
                return Color.FromArgb(Min, Max, Mid);
            }
            else if (hsl.H <= (double)2 / 3)
            {
                Mid = Round(-((hsl.H - 0.5) * q) * 1530 + Max);
                return Color.FromArgb(Min, Mid, Max);
            }
            else if (hsl.H <= (double)5 / 6)
            {
                Mid = Round(((hsl.H - (double)2 / 3) * q) * 1530 + Min);
                return Color.FromArgb(Mid, Min, Max);
            }
            else if (hsl.H <= 1.0)
            {
                Mid = Round(-((hsl.H - (double)5 / 6) * q) * 1530 + Max);
                return Color.FromArgb(Max, Min, Mid);
            }
            else return Color.FromArgb(0, 0, 0);
        }


        //-----------------------------------------------------------------------------------------
        //  Converts a color from RGB to HSL
        //
        public static HSL RGB_to_HSL(Color c)
        {
            HSL hsl = new HSL();

            int Max, Min, Diff, Sum;

            //	Of our RGB values, assign the highest value to Max, and the Smallest to Min
            if (c.R > c.G) { Max = c.R; Min = c.G; }
            else { Max = c.G; Min = c.R; }
            if (c.B > Max) Max = c.B;
            else if (c.B < Min) Min = c.B;

            Diff = Max - Min;
            Sum = Max + Min;

            //	Luminance - a.k.a. Brightness - Adobe photoshop uses the logic that the
            //	site VBspeed regards (regarded) as too primitive = superior decides the 
            //	level of brightness.
            hsl.L = (double)Max / 255;

            //	Saturation
            if (Max == 0) hsl.S = 0;	//	Protecting from the impossible operation of division by zero.
            else hsl.S = (double)Diff / Max;	//	The logic of Adobe Photoshops is this simple.

            //	Hue		R is situated at the angel of 360 eller noll degrees; 
            //			G vid 120 degrees
            //			B vid 240 degrees
            double q;
            if (Diff == 0) q = 0; // Protecting from the impossible operation of division by zero.
            else q = (double)60 / Diff;

            if (Max == c.R)
            {
                if (c.G < c.B) hsl.H = (double)(360 + q * (c.G - c.B)) / 360;
                else hsl.H = (double)(q * (c.G - c.B)) / 360;
            }
            else if (Max == c.G) hsl.H = (double)(120 + q * (c.B - c.R)) / 360;
            else if (Max == c.B) hsl.H = (double)(240 + q * (c.R - c.G)) / 360;
            else hsl.H = 0.0;

            return hsl;
        }

        //-----------------------------------------------------------------------------------------
        //  Custom rounding function
        //
        private static int Round(double val)
        {
            int ret_val = (int)val;

            int temp = (int)(val * 100);

            if ((temp % 100) >= 50)
                ret_val += 1;

            return ret_val;
        }

        //-----------------------------------------------------------------------------------------
        public class HSL
        {
            public HSL() { m_h = 0; m_s = 0; m_l = 0; }
            public HSL(double _h, double _s, double _l ) { m_h = _h; m_s =_s; m_l = _l; }

            double m_h;
            double m_s;
            double m_l;

            public double H
            {
                get { return m_h; }
                set
                {
                    m_h = value;
                    m_h = m_h > 1 ? 1 : m_h < 0 ? 0 : m_h;
                }
            }


            public double S
            {
                get { return m_s; }
                set
                {
                    m_s = value;
                    m_s = m_s > 1 ? 1 : m_s < 0 ? 0 : m_s;
                }
            }


            public double L
            {
                get { return m_l; }
                set
                {
                    m_l = value;
                    m_l = m_l > 1 ? 1 : m_l < 0 ? 0 : m_l;
                }
            }
        }
    }

    //=========================================================================================
    //
    // Graphics extension class
    //
    //=========================================================================================
    public class GraphicsExtension
    {

        private Graphics m_graphics;
        public Graphics Graphics { get { return this.m_graphics; } set { this.m_graphics = value; } }
        public GraphicsExtension(Graphics _graphics) { m_graphics = _graphics; }

        //=========================================================================================
        //
        // Color Bar
        //
        //=========================================================================================

        //-----------------------------------------------------------------------------------------
        public void drawFullColorBar(Rectangle _rect, Color _color)
        {
            Rectangle subRect = _rect;
            subRect.Width = (int)(subRect.Width * 0.75);
            Color c = Color.FromArgb(255, _color);
            m_graphics.FillRectangle(new SolidBrush(c), subRect);

            subRect.X = subRect.Right;
            subRect.Width = _rect.Right - subRect.X;
            c = Color.FromArgb(255, _color.A, _color.A, _color.A);
            m_graphics.FillRectangle(new SolidBrush(c), subRect);
        }

        //-----------------------------------------------------------------------------------------
        public enum ColorSlider
        {
            red,
            green,
            blue,
            alpha,
            hue,
            saturation,
            lightness
        }
        
        //-----------------------------------------------------------------------------------------
        public void drawColorSlider( Rectangle _rect, ColorSlider _type, Color _color )
        {
            float posCursor;

            _rect.Inflate(0,-1);
            if (_type == ColorSlider.red)
            {
                LinearGradientBrush gradient = new LinearGradientBrush(_rect, Color.FromArgb(255, 0, _color.G, _color.B), Color.FromArgb(255, 255, _color.G, _color.B), 0.0f);
                m_graphics.FillRectangle(gradient, _rect);
                posCursor = (float)_color.R / 255.0f;
            }
            else if (_type == ColorSlider.green)
            {
                LinearGradientBrush gradient = new LinearGradientBrush(_rect, Color.FromArgb(255, _color.R, 0, _color.B), Color.FromArgb(255, _color.R, 255, _color.B), 0.0f);
                m_graphics.FillRectangle(gradient, _rect);
                posCursor = (float)_color.G / 255.0f;
            }
            else if (_type == ColorSlider.blue)
            {
                LinearGradientBrush gradient = new LinearGradientBrush(_rect, Color.FromArgb(255, _color.R, _color.G, 0), Color.FromArgb(255, _color.R, _color.G, 255), 0.0f);
                m_graphics.FillRectangle(gradient, _rect);
                posCursor = (float)_color.B / 255.0f;
            }
            else if (_type == ColorSlider.alpha)
            {
                LinearGradientBrush gradient = new LinearGradientBrush(_rect, Color.FromArgb(255, 0, 0, 0), Color.FromArgb(255, 255, 255, 255), 0.0f);
                m_graphics.FillRectangle(gradient, _rect);
                posCursor = (float)_color.A / 255.0f;
            }
            else if (_type == ColorSlider.hue)
            {
                ColorExtended.HSL hsl = ColorExtended.RGB_to_HSL(_color);
                posCursor = (float) hsl.H;
                hsl.L = hsl.S = 1.0;

                double div = 1 / (double)_rect.Width;
                Pen pen = new Pen(Color.Black);

                for (int i = 0; i < _rect.Width ; i++)	
                {
                    //hsl.H = 1.0 - (double)i * div;
                    hsl.H = (double)i * div;
                    pen.Color = ColorExtended.HSL_to_RGB(hsl);
                    m_graphics.DrawLine(pen, _rect.X + i, _rect.Top, _rect.X + i, _rect.Bottom);	
                }
                
            }
            else if (_type == ColorSlider.saturation)
            {
                ColorExtended.HSL hsl = ColorExtended.RGB_to_HSL(_color);
                posCursor = (float) hsl.S;
                double div = 1 / (double)_rect.Width;
                Pen pen = new Pen(Color.Black);

                for (int i = 0; i < _rect.Width ; i++)	
                {
                    //hsl.S = 1.0 - (double)i * div;
                    hsl.S = (double)i * div;
                    pen.Color = ColorExtended.HSL_to_RGB(hsl);
                    m_graphics.DrawLine(pen, _rect.X + i, _rect.Top, _rect.X + i, _rect.Bottom);	
                }
            }
            else if (_type == ColorSlider.lightness)
            {
                ColorExtended.HSL hsl = ColorExtended.RGB_to_HSL(_color);
                posCursor = (float) hsl.L;
                double div = 1 / (double)_rect.Width;
                Pen pen = new Pen(Color.Black);

                for (int i = 0; i < _rect.Width; i++)
                {
                    hsl.L = (double)i * div;
                    //hsl.L = 1.0 - (double)i * div;
                    pen.Color = ColorExtended.HSL_to_RGB(hsl);
                    m_graphics.DrawLine(pen, _rect.X + i, _rect.Top, _rect.X + i, _rect.Bottom);	
                }
            }
            else
                return;
            _rect.Inflate(0,1);

            int x = _rect.X + (int) (_rect.Width * posCursor);
            Rectangle cursorRect = Rectangle.FromLTRB( x-2, _rect.Top, x+2, _rect.Bottom);
            m_graphics.DrawRectangle(Pens.White, cursorRect);
            cursorRect.Inflate(-1, -1);
            m_graphics.DrawRectangle(Pens.Black, cursorRect);
        }

        //=========================================================================================
        //
        // slider
        //
        //=========================================================================================

        //-----------------------------------------------------------------------------------------
        public void drawSlider(Rectangle _rect, float _min, float _max, float _value)
        {
            Rectangle bar = _rect;
            int y = (_rect.Top + _rect.Bottom) / 2;

            bar.X += 5;
            bar.Width -= 10;
            bar.Y = y - 1;
            bar.Height = 3;
            bar.Inflate(1, 1);
            Graphics.FillRectangle(Brushes.Black, bar);
            bar.Inflate(-1, -1);
            Graphics.FillRectangle(Brushes.Gray, bar);

            float ratio = (_value - _min) / (_max - _min);
            int x = bar.Left + (int)(ratio * bar.Width);
            Rectangle cursorRect = Rectangle.FromLTRB(x-5, y-5, x+5, y+5);
            Graphics.FillEllipse(Brushes.Black, cursorRect);
            cursorRect.Inflate(-1, -1);
            Graphics.FillEllipse(Brushes.LightGray, cursorRect);
        }

        //=========================================================================================
        //
        // Rounded Rect
        //
        //=========================================================================================


        //-----------------------------------------------------------------------------------------
        // Fills a Rounded Rectangle with integers.
        //
        public void FillRoundRectangle(System.Drawing.Brush brush, int x, int y, int width, int height, int radius)
        {

            float fx = Convert.ToSingle(x);
            float fy = Convert.ToSingle(y);
            float fwidth = Convert.ToSingle(width);
            float fheight = Convert.ToSingle(height);
            float fradius = Convert.ToSingle(radius);
            this.FillRoundRectangle(brush, fx, fy, fwidth, fheight, fradius);

        }


        //-----------------------------------------------------------------------------------------
        // Fills a Rounded Rectangle (float parameters).
        //
        public void FillRoundRectangle(System.Drawing.Brush brush, float x, float y, float width, float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = this.GetRoundedRect(rectangle, radius);
            this.Graphics.FillPath(brush, path);
        }


        //-----------------------------------------------------------------------------------------
        // Draw a Rounded Rectangle (int parameters).
        //
        public void DrawRoundRectangle(System.Drawing.Pen pen, int x, int y, int width, int height, int radius)
        {
            float fx = Convert.ToSingle(x);
            float fy = Convert.ToSingle(y);
            float fwidth = Convert.ToSingle(width);
            float fheight = Convert.ToSingle(height);
            float fradius = Convert.ToSingle(radius);
            this.DrawRoundRectangle(pen, fx, fy, fwidth, fheight, fradius);
        }

        //-----------------------------------------------------------------------------------------
        // Draw a Rounded Rectangle (float parameters).
        //
        public void DrawRoundRectangle(System.Drawing.Pen pen, float x, float y, float width, float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = this.GetRoundedRect(rectangle, radius);
            this.Graphics.DrawPath(pen, path);
        }

        //-----------------------------------------------------------------------------------------
        // build graphics path for rounded rect
        //
        private GraphicsPath GetRoundedRect(RectangleF baseRect, float radius)
        {
            // if corner radius is less than or equal to zero, return the original rectangle 
            if (radius <= 0.0F)
            {
                GraphicsPath mPath = new GraphicsPath();
                mPath.AddRectangle(baseRect);
                mPath.CloseFigure();
                return mPath;
            }

            // if the corner radius is greater than or equal to half the width, or height (whichever is shorter) 
            // then return a capsule instead of a lozenge 
            if (radius >= (Math.Min(baseRect.Width, baseRect.Height)) / 2.0)
                return GetCapsule(baseRect);

            // create the arc for the rectangle sides and declare a graphics path object for the drawing 
            float diameter = radius * 2.0F;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            // top left arc 
            path.AddArc(arc, 180, 90);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        //-----------------------------------------------------------------------------------------
        // build graphics path for capsule
        //
        private GraphicsPath GetCapsule(RectangleF baseRect)
        {
            float diameter;
            RectangleF arc;
            GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            try
            {
                if (baseRect.Width > baseRect.Height)
                {
                    // return horizontal capsule 
                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    // return vertical capsule 
                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                {
                    // return circle 
                    path.AddEllipse(baseRect);
                }
            }
            catch 
            {
                path.AddEllipse(baseRect);
            }
            finally
            {
                path.CloseFigure();
            }
            return path;
        }
    }
} 
