using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TreeOfLife.GUI
{
    public static class Draw
    {
        //-------------------------------------------------------------------
        public class IconAndTextParams
        {
            public int IconSize = 12;
            public int Margin = 10;
            public int BetweenIconAndText = 2;
            public Brush TextBrush = Brushes.White;
            public ImageAttributes ImageAttributes = null;
        }

        //-------------------------------------------------------------------
        static public void IconAndText(Graphics G, Rectangle R, string _text, Font _font, Image _image, IconAndTextParams _params)
        {
            if (R.Width < 20) return;

            Rectangle imageR = R;
            imageR.Y += (imageR.Height - _params.IconSize) / 2;
            imageR.Height = _params.IconSize;
            imageR.Width = _params.IconSize;

            SizeF textSize = G.MeasureString(_text, _font);
            textSize.Width++;
            int widthForText = R.Width - (_params.IconSize + _params.Margin * 2 + _params.BetweenIconAndText);

            if (R.Width < 40 || String.IsNullOrEmpty(_text) || widthForText < 20)
            {
                imageR.X += (R.Width - _params.IconSize) / 2;
                G.DrawImage(_image, imageR, 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel, _params.ImageAttributes);
            }
            else
            {
                imageR.X += _params.Margin;
                if (widthForText > (int)textSize.Width)
                {
                    imageR.X += (widthForText - (int)textSize.Width) / 2;
                    widthForText = (int)textSize.Width;
                }
                G.DrawImage(_image, imageR, 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel, _params.ImageAttributes);

                R.Width = widthForText;
                R.X = imageR.Right + _params.BetweenIconAndText;
                R.Y += 1 + (R.Height - (int)textSize.Height) / 2;
                R.Height = (int)textSize.Height;
                G.DrawString(_text, _font, _params.TextBrush, R, Resources.StringFormatTruncate );
            }
        }

        //-------------------------------------------------------------------
        static public void Text(Graphics G, Rectangle R, string _text, Font _font, IconAndTextParams _params)
        {
            if (R.Width < 20) return;

            SizeF textSize = G.MeasureString(_text, _font);
            textSize.Width++;
            int widthForText = Math.Min(R.Width - _params.Margin * 2, (int)textSize.Width);

            R.Width = widthForText;
            R.X += _params.Margin;
            R.Y += 1 + (R.Height - (int)textSize.Height) / 2;
            R.Height = (int)textSize.Height;
            G.DrawString(_text, _font, _params.TextBrush, R, Resources.StringFormatTruncate);
        }


    }
}
