using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VinceToolbox.UserControl.Graph
{
    partial class SpaceGraphMap
    {
        //=========================================================================================
        //
        // Space graph map:
        //      simple arrangment method: No squarification. Children are arranged 
        //      alternately horizontally and vertically.
        //
        //=========================================================================================

        //-----------------------------------------------------------------------------------------
        void Simple_DrawChildren(Graphics _graphics, SpaceGraphItem parent, UInt32 flags)
        {
            //throw (new NotImplementedException());

            Rectangle rc= parent.TmiGetRectangle();

            bool horizontal = (flags == 0);

            int width = horizontal ? rc.Width : rc.Height;
            if (width < 0) return;
            

            double fBegin = horizontal ? rc.Left : rc.Top;
            int veryEnd = horizontal ? rc.Right : rc.Bottom;
            int i=0;
            for ( ; i < parent.TmiGetChildrenCount(); i++)
            {
                double fraction = (double)(parent.TmiGetChild(i).TmiGetSize()) / parent.TmiGetSize();

                double fEnd = fBegin + fraction * width;

                bool lastChild = (i == parent.TmiGetChildrenCount() - 1 || parent.TmiGetChild(i + 1).TmiGetSize() == 0);

                if (lastChild)
                    fEnd = veryEnd;

                int begin= (int)fBegin;
                int end= (int)fEnd;

                Rectangle rcChild = new Rectangle();
                if (horizontal)
                {
                    rcChild.X = begin;
                    rcChild.Width = end - begin;
                    rcChild.Y = rc.Top;
                    rcChild.Height = rc.Height;
                }
                else
                {
                    rcChild.Y = begin;
                    rcChild.Height = end - begin;
                    rcChild.X = rc.Left;
                    rcChild.Width = rc.Width;
                }

                RecurseDrawGraph(_graphics, parent.TmiGetChild(i), rcChild, false, (uint)(flags == 0 ? 1 : 0));

                if (lastChild)
                {
                    i++;
                    break;
                }

                fBegin= fEnd;
            }
            if (i < parent.TmiGetChildrenCount())
                parent.TmiGetChild(i).TmiSetRectangle(Rectangle.FromLTRB(-1, -1, -1, -1));
        }
    }
}
