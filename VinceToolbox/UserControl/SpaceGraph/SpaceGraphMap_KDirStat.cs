using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VinceToolbox.UserControl.Graph
{
    //=========================================================================================
    //
    // Space graph map:
    //      KDir stat method to arrange bloc
    //
    //=========================================================================================

    partial class SpaceGraphMap
    {
        //-----------------------------------------------------------------------------------------
        void KDirStat_DrawChildren(Graphics _graphics, SpaceGraphItem parent, UInt32 flags)
        {
            Rectangle R = parent.TmiGetRectangle();

            List<double> rows = new List<double>();	   
            List<int> childrenPerRow = new List<int>();

            List<double> childWidth = new List<double>(); // Widths of the children (fraction of row width).
            for (int i = 0; i < parent.TmiGetChildrenCount(); i++)
                childWidth.Add(0);

            bool horizontalRows = KDirStat_ArrangeChildren(parent, childWidth, rows, childrenPerRow);

            int width = horizontalRows ? R.Width : R.Height;
            int height = horizontalRows ? R.Height : R.Width;

            int c = 0;
            double top = horizontalRows ? R.Top : R.Left;
            for (int row = 0; row < rows.Count; row++)
            {
                double fBottom = top + rows[row] * height;
                int bottom;
                if (row == rows.Count - 1)
                    bottom = horizontalRows ? R.Bottom : R.Right;
                else
                    bottom = (int)(top + rows[row] * height);

                double left = horizontalRows ? R.Left : R.Top;
                for (int i = 0; i < childrenPerRow[row]; i++, c++)
                {
                    SpaceGraphItem child = parent.TmiGetChild(c);

                    double fRight = left + childWidth[c] * width;
                    int right = (int)fRight;

                    bool lastChild = (i == childrenPerRow[row] - 1 || childWidth[c + 1] == 0);

                    if (lastChild)
                        right = horizontalRows ? R.Right : R.Bottom;

                    Rectangle RChild;
                    if (horizontalRows)
                        RChild = Rectangle.FromLTRB((int)left, (int)top, right, bottom);
                    else
                        RChild = Rectangle.FromLTRB((int)top, (int)left, bottom, right);

                    RecurseDrawGraph(_graphics, child, RChild, false, 0);

                    if (lastChild)
                    {
                        i++;
                        c++;

                        if (i < childrenPerRow[row])
                            parent.TmiGetChild(c).TmiSetRectangle(Rectangle.FromLTRB(-1, -1, -1, -1));

                        c += childrenPerRow[row] - i;
                        break;
                    }

                    left = fRight;
                }
                top = fBottom;
            }
        }


        //-----------------------------------------------------------------------------------------
        // return: whether the rows are horizontal.
        //
        bool KDirStat_ArrangeChildren(SpaceGraphItem parent, List<double> childWidth, List<double> rows, List<int> childrenPerRow)
        {
            if (parent.TmiGetSize() == 0)
            {
                rows.Add(1.0);
                childrenPerRow.Add(parent.TmiGetChildrenCount());
                for (int i = 0; i < parent.TmiGetChildrenCount(); i++)
                    childWidth[i] = 1.0 / parent.TmiGetChildrenCount();
                return true;
            }

            bool horizontalRows = (parent.TmiGetRectangle().Width >= parent.TmiGetRectangle().Height);

            double width = 1.0;
            if (horizontalRows)
            {
                if (parent.TmiGetRectangle().Height > 0)
                    width = (double)parent.TmiGetRectangle().Width / parent.TmiGetRectangle().Height;
            }
            else
            {
                if (parent.TmiGetRectangle().Width > 0)
                    width = (double)parent.TmiGetRectangle().Height / parent.TmiGetRectangle().Width;
            }

            int nextChild = 0;
            while (nextChild < parent.TmiGetChildrenCount())
            {
                int childrenUsed = 0;
                rows.Add(KDirStat_CalcutateNextRow(parent, nextChild, width, ref childrenUsed, childWidth));
                childrenPerRow.Add(childrenUsed);
                nextChild += childrenUsed;
            }

            return horizontalRows;
        }

        //-----------------------------------------------------------------------------------------
        double KDirStat_CalcutateNextRow(SpaceGraphItem parent, int nextChild, double width, ref int childrenUsed, List<double> childWidth)
        {
            double _minProportion = 0.4;

            double mySize = (double)parent.TmiGetSize();

            UInt32 sizeUsed = 0;
            double rowHeight = 0;
            int i;
            for (i = nextChild; i < parent.TmiGetChildrenCount(); i++)
            {
                UInt32 childSize = parent.TmiGetChild(i).TmiGetSize();
                if (childSize == 0)
                    break;

                sizeUsed += childSize;
                double virtualRowHeight = sizeUsed / mySize;

                // Rectangle(mySize)    = width * 1.0
                // Rectangle(childSize) = childWidth * virtualRowHeight
                // Rectangle(childSize) = childSize / mySize * width

                double childwidth = childSize / mySize * width / virtualRowHeight;

                if (childwidth / virtualRowHeight < _minProportion)
                {
                    // For the first child we have:
                    // childWidth / rowHeight
                    // = childSize / mySize * width / rowHeight / rowHeight
                    // = childSize * width / sizeUsed / sizeUsed * mySize
                    // > childSize * mySize / sizeUsed / sizeUsed
                    // > childSize * childSize / childSize / childSize 
                    // = 1 > _minProportion.
                    break;
                }
                rowHeight = virtualRowHeight;
            }


            // Now i-1 is the last child used
            // and rowHeight is the height of the row.

            // We add the rest of the children, if their size is 0.
            while (i < parent.TmiGetChildrenCount() && parent.TmiGetChild(i).TmiGetSize() == 0)
                i++;

            childrenUsed = i - nextChild;

            // Now as we know the rowHeight, we compute the widths of our children.
            if (rowHeight == 0)
            {
                for (i = 0; i < childrenUsed; i++)
                    childWidth[nextChild + i] = 0;
            }
            else
            {
                for (i = 0; i < childrenUsed; i++)
                {
                    // Rectangle(1.0 * 1.0) = mySize
                    double rowSize = mySize * rowHeight;
                    double childSize = (double)parent.TmiGetChild(nextChild + i).TmiGetSize();
                    double cw = childSize / rowSize;
                    childWidth[nextChild + i] = cw;
                }
            }

            return rowHeight;
        }
    }
}
