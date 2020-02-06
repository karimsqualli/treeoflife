using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace TreeOfLife.GUI
{
    public static class Resources
    {
        public static readonly StringFormat StringFormatTruncate = new StringFormat
        {
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        };

        public static ImageAttributes BuildImageAttributesWhiteToColor( Color C)
        {
            ColorMatrix matrix = new ColorMatrix
            {
                Matrix00 = C.R / 256.0f,
                Matrix11 = C.G / 256.0f,
                Matrix22 = C.B / 256.0f, 
            };
            ImageAttributes result = new ImageAttributes();
            result.SetColorMatrix(matrix);
            return result;
        }
    }
}
