//#define EnableCircleMode

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TreeOfLife
{
    public partial class TaxonGraphPanel
    {
#if (EnableCircleMode)
        Pen circleTreePen = null;

        //-------------------------------------------------------------------
        void PaintCircleInit()
        {
            circleTreePen = new Pen(Color.Black, 1);
        }

        //-------------------------------------------------------------------
        void PaintCircleDispose()
        {
            if (circleTreePen != null) circleTreePen.Dispose();
            circleTreePen = null;
        }

        //-------------------------------------------------------------------
        void PaintTaxonRecursiveCircle(Graphics g, TaxonTreeNode _taxon, bool _grayed = true)
        {
            InternalColorEnum internalColor = InternalColorEnum.espece;
            Image image = null;

            if (_taxon.CircularInfo == null)
                return;

            // optim
            if (_taxon.CircularInfo.Bounds.Bottom < -Origin.Y) return;
            if (_taxon.CircularInfo.Bounds.Top > Height - Origin.Y) return;
            if (_taxon.CircularInfo.Bounds.Right < -Origin.X) return;
            if (_taxon.CircularInfo.Bounds.Left > Width - Origin.X) return;


            // stretch species end leafs taxon (species with no sub species or sub species)
            if (_taxon.IsEndLeaf)
            {
                image = TaxonImages.Manager.GetImage(_taxon, _taxon.R.Height);
            }
            else
            {
                internalColor = InternalColorEnum.expanded;
                foreach (TaxonTreeNode child in _taxon.Children)
                {
                    if (!child.Visible)
                    {
                        internalColor = InternalColorEnum.collapsed;
                        continue;
                    }
                    PaintTaxonRecursiveCircle(g, child, _grayed && (_taxon != Selected));
                }
            }

            Brush brushBack;
            Brush brushText = Brushes.Black;
            if (_taxon == Selected)
            {
                brushBack = UsedColors.SelectedBrush;
                _grayed = false;
            }
            else if (_taxon == BelowMouse)
                brushBack = UsedColors.HoverBrush;
            else if (_taxon.Highlight)
                brushBack = UsedColors.HighlightedBrush;
            else
            {
                if (internalColor == InternalColorEnum.espece)
                {
                    if (_taxon.Desc.IsExtinct)
                    {
                        brushBack = UsedColors.ExtinctExpandedBrush;
                        brushText = Brushes.White;
                    }
                    else
                        brushBack = Brushes.Azure;
                }
                else
                {
                    if (_taxon.Desc.IsExtinct)
                    {
                        brushBack = (internalColor == InternalColorEnum.collapsed) ? UsedColors.ExtinctCollapsedBrush : UsedColors.ExtinctExpandedBrush;
                        brushText = Brushes.White;
                    }
                    else
                        brushBack = (internalColor == InternalColorEnum.collapsed) ? UsedColors.CollapsedBrush : UsedColors.ExpandedBrush;
                }
            }

            Pen pen = _grayed ? circleTreePen : graphTreePen;

            if (_taxon.CircularInfo.InternalPoint)
            {
                float dx = (float)(_CircularParams.DemiRadiusPart * _taxon.CircularInfo.Dx);
                float dy = (float)(_CircularParams.DemiRadiusPart * _taxon.CircularInfo.Dy);

                g.DrawLine(pen, (float)_taxon.CircularInfo.X, (float)_taxon.CircularInfo.Y, (float)(_taxon.CircularInfo.X + dx), (float)(_taxon.CircularInfo.Y + dy));
                //if (_taxon.Father != null )  
                //    g.DrawLine(pen, (float)_taxon.CircularInfo.X, (float)_taxon.CircularInfo.Y, (float)(_taxon.CircularInfo.X - dx), (float)(_taxon.CircularInfo.Y - dy));
                g.DrawLines(pen, _taxon.CircularInfo.Points);

                foreach (TaxonTreeNode child in _taxon.Children)
                {
                    if (!child.Visible)
                        continue;
                    if (child.CircularInfo.InternalPoint)
                    {
                        dx = (float)(_CircularParams.DemiRadiusPart * child.CircularInfo.Dx);
                        dy = (float)(_CircularParams.DemiRadiusPart * child.CircularInfo.Dy);
                        float dx1 = (float)(5 * child.CircularInfo.Dx);
                        float dy1 = (float)(5 * child.CircularInfo.Dy);
                        g.DrawLine(pen, (float)(child.CircularInfo.X - dx1), (float)(child.CircularInfo.Y - dy1), (float)(child.CircularInfo.X - dx), (float)(child.CircularInfo.Y - dy));
                    }
                    else
                        g.DrawLine(pen, (float)child.CircularInfo.X, (float)child.CircularInfo.Y, (float)(child.CircularInfo.X - child.CircularInfo.LengthBefore * child.CircularInfo.Dx), (float)(child.CircularInfo.Y - child.CircularInfo.LengthBefore * child.CircularInfo.Dy));
                }

                g.FillEllipse(brushBack, (float)_taxon.CircularInfo.X - 5, (float)_taxon.CircularInfo.Y - 5, 10, 10);

                if (_grayed)
                    g.FillEllipse(GraphResources.BrushTransparentBlack, (float)_taxon.CircularInfo.X - 5, (float)_taxon.CircularInfo.Y - 5, 10, 10);

                return;
            }

            //g.DrawLine(pen, (float)_taxon.CircularInfo.X, (float)_taxon.CircularInfo.Y, (float)(_taxon.CircularInfo.X - _taxon.CircularInfo.LengthBefore * _taxon.CircularInfo.Dx), (float)(_taxon.CircularInfo.Y - _taxon.CircularInfo.LengthBefore * _taxon.CircularInfo.Dy));
            g.FillPolygon(brushBack, _taxon.CircularInfo.Points);

            Font currentFont = Font;

            SizeF sizeText = g.MeasureString(_taxon.Desc.RefMainName, currentFont);
            if (sizeText.Height <= _taxon.R.Height)
            {
                System.Drawing.Drawing2D.Matrix M = g.Transform;


                float angle = (float)(_taxon.CircularInfo.Angle / Math.PI * 180.0);
                g.TranslateTransform((float)_taxon.CircularInfo.X, (float)_taxon.CircularInfo.Y);
                g.RotateTransform(angle);
                g.TranslateTransform(10, -sizeText.Height / 2);
                g.DrawString(_taxon.Desc.RefMainName, currentFont, brushText, 0, 0);

                g.Transform = M;
            }

            if (_grayed)
                g.FillPolygon(GraphResources.BrushTransparentBlack, _taxon.CircularInfo.Points);

        }
#else

        void PaintCircleInit() { }
        void PaintCircleDispose() { }

#endif //EnableCircleMode
    }
}
