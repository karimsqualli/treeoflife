using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TreeOfLife
{
    public class GraphResources
    {
        static GraphResources()
        {
            CenteredText = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            NavigatorText = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };

            LeftText = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near,
            };

            BrushTransparentDimGray = new SolidBrush(Color.FromArgb(50, Color.DimGray));
            BrushTransparentGray = new SolidBrush(Color.FromArgb(50, Color.Gray));
            BrushTransparentBlack = new SolidBrush(Color.FromArgb(70, Color.Black));
        }

        public static StringFormat CenteredText { get; private set; }
        public static StringFormat NavigatorText { get; private set; }
        public static StringFormat LeftText { get; private set; }

        public static Brush BrushTransparentDimGray { get; private set; }
        public static Brush BrushTransparentGray { get; private set; }
        public static Brush BrushTransparentBlack { get; private set; }
    }
}
