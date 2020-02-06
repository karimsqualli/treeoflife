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
    //      Sequoia View method to arrange bloc
    //
    //=========================================================================================

    partial class SpaceGraphMap
    {
        //-----------------------------------------------------------------------------------------
        void SequoiaView_DrawChildren(Graphics _graphics, SpaceGraphItem parent, UInt32 flags)
        {
            // Rest rectangle to fill
            Rectangle remaining = parent.TmiGetRectangle();

            // Size of rest rectangle
            UInt32 remainingSize = parent.TmiGetSize();

            // Scale factor
            double sizePerSquarePixel = (double)parent.TmiGetSize() / remaining.Width / remaining.Height;

            // First child for next row
            int head = 0;

            // At least one child left
            while (head < parent.TmiGetChildrenCount())
            {
                // How we divide the remaining rectangle 
                bool horizontal = (remaining.Width >= remaining.Height);

                // Height of the new row
                int height = horizontal ? remaining.Height : remaining.Width;

                // Square of height in size scale for ratio formula
                double hh = (height * height) * sizePerSquarePixel;

                // Row will be made up of child(rowBegin)...child(rowEnd - 1)
                int rowBegin = head;
                int rowEnd = head;

                // Worst ratio so far
                double worst = double.MaxValue;

                // Maximum size of children in row
                UInt32 rmax = parent.TmiGetChild(rowBegin).TmiGetSize();

                // Sum of sizes of children in row
                UInt32 sum = 0;

                // This condition will hold at least once.
                while (rowEnd < parent.TmiGetChildrenCount())
                {
                    // We check a virtual row made up of child(rowBegin)...child(rowEnd) here.

                    // Minimum size of child in virtual row
                    UInt32 rmin = parent.TmiGetChild(rowEnd).TmiGetSize();

                    // If sizes of the rest of the children is zero, we add all of them
                    if (rmin == 0)
                    {
                        rowEnd = parent.TmiGetChildrenCount();
                        break;
                    }

                    // Calculate the worst ratio in virtual row.
                    // Formula taken from the "Squarified Tree maps" paper.
                    // (http://http://www.win.tue.nl/~vanwijk/)

                    double ss = ((double)sum + rmin) * ((double)sum + rmin);
                    double ratio1 = hh * rmax / ss;
                    double ratio2 = ss / hh / rmin;

                    double nextWorst = Math.Max(ratio1, ratio2);

                    // Will the ratio get worse?
                    if (nextWorst > worst)
                    {
                        // Yes. Don't take the virtual row, but the
                        // real row (child(rowBegin)..child(rowEnd - 1))
                        // made so far.
                        break;
                    }

                    // Here we have decided to add child(rowEnd) to the row.
                    sum += rmin;
                    rowEnd++;

                    worst = nextWorst;
                }

                // Row will be made up of child(rowBegin)...child(rowEnd - 1).
                // sum is the size of the row.

                // Width of row
                int width = (horizontal ? remaining.Width : remaining.Height);

                if (sum < remainingSize)
                    width = (int)((double)sum / remainingSize * width);
                // else: use up the whole width
                // width may be 0 here.

                // Build the rectangles of children.
                Rectangle rc = remaining;
                double fBegin;
                if (horizontal)
                {
                    rc.Width = width;
                    fBegin = remaining.Top;
                }
                else
                {
                    rc.Height = width;
                    fBegin = remaining.Left;
                }

                // Now put the children into their places
                for (int i = rowBegin; i < rowEnd; i++)
                {
                    int begin = (int)fBegin;
                    double fraction = (double)(parent.TmiGetChild(i).TmiGetSize()) / sum;
                    double fEnd = fBegin + fraction * height;
                    int end = (int)fEnd;

                    bool lastChild = (i == rowEnd - 1 || parent.TmiGetChild(i + 1).TmiGetSize() == 0);

                    if (lastChild)
                    {
                        // Use up the whole height
                        end = (horizontal ? remaining.Top + height : remaining.Left + height);
                    }

                    if (horizontal)
                    {
                        rc.Y = begin;
                        rc.Height = end - begin;
                    }
                    else
                    {
                        rc.X = begin;
                        rc.Width = end - begin;
                    }


                    RecurseDrawGraph(_graphics, parent.TmiGetChild(i), rc, false, 0);

                    if (lastChild)
                        break;

                    fBegin = fEnd;
                }

                // Put the next row into the rest of the rectangle
                if (horizontal)
                {
                    remaining.X += width;
                    remaining.Width -= width;
                }
                else
                {
                    remaining.Y += width;
                    remaining.Height -= width;
                }

                remainingSize -= sum;

                head += (rowEnd - rowBegin);

                if (remaining.Width <= 0 || remaining.Height <= 0)
                {
                    if (head < parent.TmiGetChildrenCount())
                        parent.TmiGetChild(head).TmiSetRectangle(Rectangle.FromLTRB(-1, -1, -1, -1));

                    break;
                }
            }

        }
    }
}
