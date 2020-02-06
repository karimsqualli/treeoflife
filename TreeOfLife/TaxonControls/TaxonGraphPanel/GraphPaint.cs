using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TreeOfLife
{
    public partial class TaxonGraphPanel
    {
        //-------------------------------------------------------------------
        void PaintInit()
        {
            graphTreePen = new Pen(Color.White, 3);
            PaintCircleInit();
        }

        //-------------------------------------------------------------------
        void PaintDispose()
        {
            if (graphTreePen != null) graphTreePen.Dispose();
            graphTreePen = null;
            PaintCircleDispose();
        }

        //-------------------------------------------------------------------
        int mousePaintX, mousePaintY;

        //-------------------------------------------------------------------
        enum InternalColorEnum { espece, expanded, collapsed };

        const int YInvalid = int.MinValue;
        const int YFarUp = int.MinValue + 1;
        const int YFarDown = int.MinValue + 2;

        Pen graphTreePen = null;
        
        //-------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode) return;
            mousePaintX = mouseX;
            if (mousePaintX < 0) mousePaintX = 0;
            else if (mousePaintX > Width) mousePaintX = Width;
            mousePaintX -= Origin.X;
            mousePaintY = mouseY - Origin.Y;
            DoPaint(e.Graphics);

            PaintRectangle = new Rectangle(-Origin.X, -Origin.Y, Width, Height);
            _InertiaMove.DrawDebug(e.Graphics, Font, ClientRectangle);

            //Rectangle R = new Rectangle(Origin, GraphSize());
            //e.Graphics.DrawRectangle(Pens.Aqua, R);
        }

        //-------------------------------------------------------------------
        protected void DoPaint(Graphics _graphics)
        {
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            if (Root != null  && !Root.R.IsEmpty)
            {
                EditionToolUpdate(Selected);
                ShortcutModeUpdate();
                _graphics.TranslateTransform(Origin.X, Origin.Y);

                switch (Options.DisplayMode)
                {
                    case GraphOptions.DisplayModeEnum.Grid:
                        PaintTaxonRecursive(_graphics, Root, Options.GrayUnselectedInBoxMode);
                        break;

                    case GraphOptions.DisplayModeEnum.Lines:

                        _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                        PaintTaxonRecursiveTree(_graphics, Root, true);
                        _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        if (Options.GrayUnselectedInBoxMode)
                        {
                            Region clipRegion = _graphics.Clip;
                            if (Selected != null)
                            {
                                _graphics.ExcludeClip(Selected.RectangleWithChildren);
                            }
                            _graphics.FillRectangle(GraphResources.BrushTransparentBlack, -Origin.X, -Origin.Y, Width, Height);
                            _graphics.Clip = clipRegion;
                        }

                        EditionToolPaint(_graphics);
                        break;

                        /* Removing CircleDisplayMode
                        case GraphOptions.DisplayModeEnum.Circle:
                            PaintTaxonRecursiveCircle(_graphics, Root);
                            break; */
                }

                _graphics.ResetTransform();

                if (Options.DisplayClassikRankRule)
                    PaintClassicRankRule(_graphics);
            }
        }

        //-------------------------------------------------------------------
        // draw image (_image is not null ... have to be tested before calling method)
        // value returned is the amount of pixel added at the right of full rect to display image.
        Rectangle? PaintTaxon_ComputeImageRect(ref Rectangle _textRect, ref Rectangle _fullRect)
        {
            Rectangle imageR = _fullRect;
            imageR.Width = imageR.Height;

            if (Options.KeepsImageAtNamesRight)
            {
                imageR.X = _textRect.Right + 8;
                if (imageR.X > _fullRect.Right)
                    imageR.X = _fullRect.Right;
            }
            else
            {
                imageR.X = _fullRect.Right;
                if (imageR.Right > Width - Origin.X)
                {
                    imageR.X = Width - Origin.X - imageR.Width;
                    if (imageR.X < _textRect.Right)
                        return null;
                }
            }

            imageR.Width -= 3;
            imageR.Y += 2;
            imageR.Height -= 3;
            return imageR;
        }

        //-------------------------------------------------------------------
        // draw image (_image is not null ... have to be tested before calling method)
        // value returned is the amount of pixel added at the right of full rect to display image.
        int PaintTaxon_Image( Graphics g, Image _image, ref Rectangle _textRect, ref Rectangle _fullRect )
        {
            Rectangle imageR = _fullRect;
            imageR.Width = imageR.Height;

            if (Options.KeepsImageAtNamesRight)
            {
                imageR.X = _textRect.Right + 8;
                if (imageR.X > _fullRect.Right) 
                    imageR.X = _fullRect.Right;
            }
            else
            {
                imageR.X = _fullRect.Right;
                if (imageR.Right > Width - Origin.X)
                {
                    imageR.X = Width - Origin.X - imageR.Width;
                    if (imageR.X < _textRect.Right)
                        return 0;
                }
            }

            g.DrawImage(_image, imageR);
            return (imageR.Right  > _fullRect.Right) ? imageR.Right - _fullRect.Right : 0;
        }

        //-------------------------------------------------------------------
        void PaintTaxonRecursive(Graphics g, TaxonTreeNode _taxon, bool _grayed = true)
        {
            Rectangle R = _taxon.R;
            
            // optim
            if (R.Bottom < -Origin.Y) return;
            if (R.Top > Height - Origin.Y) return;
            if (R.Left > Width - Origin.X) return;
            
            InternalColorEnum internalColor = InternalColorEnum.espece;
            bool alignRight = false;
            Image image = null;

            // stretch species end leafs taxon (species with no sub species or sub species)
            if (_taxon.IsEndLeaf)
            {
                R.Width = Root.WidthWithChildren - R.Left;
                _taxon.R.Width = R.Width;
                alignRight = R.Width > Options.ZoomedWidth;
                image = TaxonImages.Manager.GetImage(_taxon, R.Height);
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
                    PaintTaxonRecursive(g, child, _grayed && (_taxon != Selected));
                }
            }

            if (R.Right < -Origin.X) return;

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

            Rectangle RLeftMargin = R;
            if (_taxon.RLeftMargin != 0)
            {
                R.X += _taxon.RLeftMargin;
                R.Width -= _taxon.RLeftMargin;

                g.FillRectangle(brushBack, RLeftMargin);
                Brush brushBlack = new SolidBrush(Color.FromArgb(100, Color.Black));
                g.FillRectangle(brushBlack, RLeftMargin);
            }

            g.FillRectangle(brushBack, R);
            ControlPaint.DrawBorder3D(g, R, Border3DStyle.Sunken);

            Rectangle textR = R;
            textR.Inflate(-2, -2);

            Font currentFont = Font;

            SizeF sizeText = g.MeasureString(_taxon.Desc.RefMainName, currentFont);
            if (sizeText.Height > textR.Height)
                return;

            // reduce eventually font
            if (sizeText.Width > textR.Width)
            {
                float ratio = (float)sizeText.Width / (float)textR.Width;

                bool applyRatio = true;
                if (ratio > 1.2f)
                {
                    sizeText = g.MeasureString(_taxon.Desc.RefMainName, currentFont, textR.Width);
                    if (sizeText.Height <= textR.Height)
                        applyRatio = false;
                    else
                    {
                        float ratioH = (float)sizeText.Height / (float)textR.Height;
                        if (ratioH < 1.2f) ratio = ratioH;
                        else ratio = 1.2f;
                    }
                }
                if (applyRatio)
                {
                    currentFont = new System.Drawing.Font(currentFont.FontFamily, currentFont.Size / ratio);
                    alignRight = false;
                    sizeText = g.MeasureString(_taxon.Desc.RefMainName, currentFont, textR.Width);
                }
            }

            int height = (int)sizeText.Height;
            if (height < textR.Height)
            {
                int halfHeight = height / 2;
                if (mousePaintY >= textR.Bottom - halfHeight)
                    textR.Y = textR.Bottom - height;
                else if (mousePaintY > textR.Top + halfHeight)
                    textR.Y = mousePaintY - halfHeight;
                textR.Height = height;
            }

            if (alignRight)
            {
                int width = (int)sizeText.Width;
                if (width < textR.Width)
                {
                    int right = textR.Right;

                    if (mousePaintX >= textR.Right)
                        textR.X = textR.Right - width;
                    else if (mousePaintX > textR.Left + width)
                        textR.X = mousePaintX - width;

                    if (textR.X < Origin.X)
                    {
                        textR.X = Origin.X;
                        if (textR.X + width > right)
                            textR.X = right - width;
                    }

                    textR.Width = width;
                }
            }

            textR.Inflate(2, 2);
            g.DrawString(_taxon.Desc.RefMainName, currentFont, brushText, textR, GraphResources.CenteredText);

            if (image != null)
                _taxon.R.Width += PaintTaxon_Image(g, image, ref textR, ref R);

            if (_grayed)
                g.FillRectangle(GraphResources.BrushTransparentBlack, _taxon.R); 
        }

        //-------------------------------------------------------------------
        bool ShortcutModeKeyOn()
        {
            return ModifierKeys == Keys.Control;
        }

        struct ShortcutData
        {
            public Rectangle R;
            public Dictionary<TaxonTreeNode, Rectangle> Siblings;
        }

        Dictionary<TaxonTreeNode, ShortcutData> ShortcutModeDatas = new Dictionary<TaxonTreeNode, ShortcutData>();
        bool ShortcutModeRect( TaxonTreeNode _node, ref Rectangle _R )
        {
            if (ShortcutModeDatas.TryGetValue(_node, out ShortcutData data))
            {
                _R = data.R;
                data.Siblings.Clear();
                return true;
            }
            ShortcutModeDatas[_node] = new ShortcutData() { R = _R, Siblings = new Dictionary<TaxonTreeNode, Rectangle>() };
            return false;
        }

        void ShortcutModeAddSibling( TaxonTreeNode _node, TaxonTreeNode _sibling, Rectangle R)
        {
            if (ShortcutModeDatas.TryGetValue(_node, out ShortcutData data))
                data.Siblings[_sibling] = R;
        }

        void ShortcutModeUpdate()
        {
            if (!ShortcutModeKeyOn())
                ShortcutModeDatas.Clear();
        }

        void ShortcutModeClear()
        {
            ShortcutModeDatas.Clear();
        }


        //-------------------------------------------------------------------
        int PaintTaxonRecursiveTree(Graphics g, TaxonTreeNode _taxon, bool _grayed = true )
        {
            Rectangle R = _taxon.R;

            // optim
            if (R.Bottom < -Origin.Y) return YFarUp;
            if (R.Top > Height - Origin.Y) return YFarDown;
            if (R.Left > Width + Options.ZoomedColumnWidth - Origin.X) return YInvalid;

            InternalColorEnum internalColor = InternalColorEnum.espece;
            bool alignRight = false;
            Image image = null;
            List<int> childrenY = new List<int>();
            bool HasChildrenUp = false;
            bool HasChildrenDown = false;

            bool imageUnder = false;
            Rectangle imageR = new Rectangle();
            int IndexSiblingUp = -1;
            int IndexSiblingDown = -1;

            // stretch species end leafs taxon (species with no sub species or sub species)
            if (_taxon.IsEndLeaf)
            {
                R.Width = Root.WidthWithChildren - R.Left;
                _taxon.R.Width = R.Width;
                alignRight = R.Width > Options.ZoomedWidth;
                image = TaxonImages.Manager.GetImage(_taxon, R.Height);
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
                    int childY = PaintTaxonRecursiveTree(g, child, _grayed && (_taxon != Selected));
                    if (childY == YFarUp)
                        HasChildrenUp = true;
                    else if (childY == YFarDown)
                        HasChildrenDown = true;
                    else if (childY != YInvalid && !childrenY.Contains(childY))
                        childrenY.Add(childY);
                }

                if (_taxon.Desc.ClassicRank == ClassicRankEnum.Espece)
                {
                    if (internalColor == InternalColorEnum.collapsed)
                    {
                        R.Width = Root.WidthWithChildren - R.Left;
                        _taxon.R.Width = R.Width;
                        alignRight = R.Width > Options.ZoomedWidth;
                        image = TaxonImages.Manager.GetImage(_taxon, R.Height);
                    }
                    else
                    {
                        image = TaxonImages.Manager.GetImage(_taxon, R.Height);
                        if (image != null)
                        {
                            imageUnder = true;
                            imageR.Width = Math.Min(R.Height - Options.ZoomedHeight, R.Width);
                            imageR.Height = imageR.Width;
                            imageR.Y = R.Top + Options.ZoomedHeight;
                            imageR.X = R.Left + (R.Width - imageR.Width) / 2;
                        }
                    }
                }

                if (internalColor != InternalColorEnum.collapsed && _taxon.Father != null && _taxon == BelowMouseNoShortcut)
                {
                    // new: draw siblings ??
                    if (R.Top < -Origin.Y)
                        IndexSiblingUp = _taxon.Father.Children.IndexOf(_taxon) - 1;
                    if (R.Bottom > Height - Origin.Y)
                        IndexSiblingDown = _taxon.Father.Children.IndexOf(_taxon) + 1; 
                }
            }

            if (R.Right < - Origin.X - Options.ZoomedColumnWidth) return int.MaxValue;

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

            Rectangle RLeftMargin = R;
            if (_taxon.RLeftMargin != 0)
            {
                R.X += _taxon.RLeftMargin;
                R.Width -= _taxon.RLeftMargin;

                g.FillRectangle(brushBack, RLeftMargin);
                Brush brushBlack = new SolidBrush(Color.FromArgb(100, Color.Black));
                g.FillRectangle(brushBlack, RLeftMargin);
            }

            Rectangle textR = R;
            textR.Inflate(-4, -4);
            textR.X += 6;
            textR.Width -= 6;

            Font currentFont = Font;
            SizeF sizeText = g.MeasureString(_taxon.Desc.RefMainName, currentFont);

            // reduce eventually font
            if (sizeText.Width > textR.Width)
            {
                float ratio = (float)sizeText.Width / (float)textR.Width;
                if (ratio > 1.2f) ratio = 1.2f;

                currentFont = new Font(currentFont.FontFamily, currentFont.Size / ratio);

                alignRight = false;
                sizeText = g.MeasureString(_taxon.Desc.RefMainName, currentFont, textR.Width);
            }

            int height = Options.ZoomedHeight;
            if (imageUnder) textR.Height -= imageR.Height;
            
            if (height < textR.Height)
            {
                int halfHeight = height / 2;
                if (mousePaintY >= textR.Bottom - halfHeight)
                    textR.Y = textR.Bottom - height;
                else if (mousePaintY > textR.Top + halfHeight)
                    textR.Y = mousePaintY - halfHeight;
                textR.Height = height;
            }

            if (alignRight)
            {
                int width = (int)sizeText.Width;
                if (width < textR.Width)
                {
                    int right = textR.Right;

                    if (mousePaintX >= textR.Right)
                        textR.X = textR.Right - width;
                    else if (mousePaintX > textR.Left + width)
                        textR.X = mousePaintX - width;

                    if (textR.X < -Origin.X + 4)
                    {
                        textR.X = -Origin.X + 4;
                        if (textR.X + width > right)
                            textR.X = right - width;
                    }

                    textR.Width = width;
                }
            }

            int y0 = textR.Top + textR.Height / 2;

            Pen linkPen = _grayed ? UsedColors.LinksPen : UsedColors.SelectedLinksPen;
            if (_taxon.RLeftMargin != 0)
                g.DrawLine(linkPen, RLeftMargin.Left, y0, R.Left, y0);

            Rectangle BackR = R;
            BackR.Y = textR.Y;
            BackR.Height = textR.Height;
            
            if (BackR.Height < Options.ZoomedHeight)
            {
                BackR.Y -= (Options.ZoomedHeight - BackR.Height) / 2;
                if (BackR.Height < Options.ZoomedHeight)
                    BackR.Height = Options.ZoomedHeight;
            }
            if (BackR.Y < R.Y) BackR.Y = R.Y;
            if (BackR.Bottom > R.Bottom) BackR.Y -= BackR.Bottom - R.Bottom;
            BackR.Inflate(0, -1);

            if (Options.DisplayBackTransparentInLineMode)
            {
                if (brushBack is SolidBrush)
                {
                    Color c = (brushBack as SolidBrush).Color;
                    Brush b = new SolidBrush(Color.FromArgb(50, c));
                    R.Inflate(Options.ZoomedColumnWidth / 2, 0);
                    g.FillRectangle(b, R);
                    g.DrawRectangle(new Pen(brushBack), R);
                    R.Inflate(-Options.ZoomedColumnWidth / 2, 0);
                }
            }

            if (ShortcutModeKeyOn())
            {
                if (ShortcutModeRect(_taxon, ref BackR))
                {
                    //textR = BackR;
                    //textR.Inflate(-4, -4);
                    textR.Y = BackR.Y;
                    textR.Height = BackR.Height;
                    textR.Inflate(0, -4);
                    y0 = textR.Top + textR.Height / 2;
                }
            }

            else if (_taxon == _EditionToolInfo.Taxon && EditionToolKeyOn() )
            {
                _EditionToolInfo.Init(ref BackR, ref textR);
                BackR = _EditionToolInfo.BackRect;
                //textR = BackR;
                //textR.Inflate(-4, -4);
                textR.Y = BackR.Y;
                textR.Height = BackR.Height;
                textR.Inflate(0, -4);
                y0 = textR.Top + textR.Height / 2;
            }

            textR.Inflate(4, 4);
            Rectangle? imageRect = null;
            if (image != null)
            {
                if (imageUnder)
                {
                    imageR.Y = BackR.Bottom;
                    g.DrawImage(image, imageR);
                }
                else
                    imageRect = PaintTaxon_ComputeImageRect(ref textR, ref BackR);
            }

            if (imageRect != null)
            {
                if (imageRect.Value.Right + 2 > BackR.Right)
                {
                    int newW = imageRect.Value.Right + 1 - BackR.X;
                    _taxon.R.Width += newW - BackR.Width;
                    BackR.Width = newW;
                }
                g.FillRectangle(brushBack, BackR);
                PaintTaxonRedListCategory(g, _taxon, BackR);
                g.DrawString(_taxon.Desc.RefMainName, currentFont, brushText, textR, GraphResources.LeftText);
                g.DrawImage(image, imageRect.Value);
            }
            else
            {
                g.FillRectangle(brushBack, BackR);
                PaintTaxonRedListCategory(g, _taxon, BackR);
                g.DrawString(_taxon.Desc.RefMainName, currentFont, brushText, textR, GraphResources.LeftText);

                if (IndexSiblingUp != -1)
                    PaintTaxonSiblingsUp(g, _taxon, IndexSiblingUp, ref R, BackR );
                if (IndexSiblingDown != -1)
                    PaintTaxonSiblingsDown(g, _taxon, IndexSiblingDown, ref R, BackR);
            }

            if (childrenY.Count > 0 || HasChildrenUp || HasChildrenDown)
                PaintLinks( g, linkPen, R, y0, childrenY, HasChildrenUp, HasChildrenDown);

            return y0;
        }

        //-------------------------------------------------------------------
        void PaintLinks(Graphics g, Pen linkPen, Rectangle R, int y0, List<int> childrenY, bool HasChildrenUp, bool HasChildrenDown)
        {
            int W = Options.ZoomedColumnWidth;
            int w0 = (int)(W * Options.LineSoftRatio / 2);
            if (w0 < 1) w0 = 1;
            int w0double = 2 * w0;

            int xr = R.Right;
            int xm = R.Right + W / 2;
            int xl = R.Right + W;

            int xmr = xm - w0;
            int xml = xm + w0;

            int y1;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Rectangle littleR;
            childrenY.Sort();
            g.DrawLine(linkPen, xr, y0, xmr + 1, y0);

            // take care of out of limits case
            // up
            if (HasChildrenUp)
            {
                littleR = Rectangle.FromLTRB(xm - w0double, y0 - w0double, xm, y0);
                g.DrawArc(linkPen, littleR, 0, 90);
                g.DrawLine(linkPen, xm, y0 - w0 + 1, xm, Origin.Y);
            }
            // down
            if (HasChildrenDown)
            {
                littleR = Rectangle.FromLTRB(xm - w0double, y0, xm, y0 + w0double);
                g.DrawArc(linkPen, littleR, 270, 90);
                g.DrawLine(linkPen, xm, y0 + w0 - 1, xm, Height - Origin.Y);
            }

            // parse children

            if (childrenY.Count > 0)
            {
                int indexCur = 0;

                if (!HasChildrenUp)
                {
                    y1 = childrenY[0];
                    if (y0 - y1 >= w0double)
                    {
                        if (w0double > 0)
                        {
                            littleR = Rectangle.FromLTRB(xm - w0double, y0 - w0double, xm, y0);
                            g.DrawArc(linkPen, littleR, 0, 90);
                        }
                        g.DrawLine(linkPen, xm, y0 - w0 + 1, xm, childrenY[0] + w0 - 1);
                    }
                }

                while (indexCur < childrenY.Count)
                {
                    y1 = childrenY[indexCur];
                    if (y0 - y1 < w0double)
                        break;
                    indexCur++;

                    if (w0double > 0)
                    {
                        littleR = Rectangle.FromLTRB(xm, y1, xm + w0double, y1 + w0double);
                        g.DrawArc(linkPen, littleR, 180, 90);
                    }
                    g.DrawLine(linkPen, xml - 1, y1, xl - 1, y1);
                }

                // draw in front
                while (indexCur < childrenY.Count)
                {
                    y1 = childrenY[indexCur];
                    if (y1 - y0 >= w0double)
                        break;
                    indexCur++;
                    g.DrawBezier(linkPen, xr, y0, xml, y0, xmr, y1, xl - 1, y1);
                }

                // draw down
                if (indexCur < childrenY.Count)
                {
                    if (!HasChildrenDown)
                    {
                        y1 = childrenY[childrenY.Count - 1];
                        if (w0double > 0)
                        {
                            littleR = Rectangle.FromLTRB(xm - w0double, y0, xm, y0 + w0double);
                            g.DrawArc(linkPen, littleR, 270, 90);
                        }
                        g.DrawLine(linkPen, xm, y0 + w0 - 1, xm - 1, y1 - w0 + 1);
                    }

                    while (indexCur < childrenY.Count)
                    {
                        y1 = childrenY[indexCur];

                        indexCur++;
                        if (w0double > 0)
                        {
                            littleR = Rectangle.FromLTRB(xm, y1 - w0double, xm + w0double, y1);
                            g.DrawArc(linkPen, littleR, 90, 90);
                        }
                        g.DrawLine(linkPen, xml - 1, y1, xl - 1, y1);
                    }
                }
            }
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
        }

        //-------------------------------------------------------------------
        void PaintTaxonRedListCategory(Graphics g, TaxonTreeNode _taxon, Rectangle R)
        {
            // display redlist category flags -- debug purpose
            /*
            R.Width = 6;
            for (int i = (int) RedListCategoryEnum.NotEvaluated; i <= (int) RedListCategoryEnum.Extinct; i++ )
            {
                if ( ((1 << i) & _taxon.RedListCategoryFlags) != 0)
                    g.FillRectangle(RedListCategoryExt.GetBackBrush((RedListCategoryEnum) i), R);
                R.X += 6;
            }
            /**/
            //
            if (_taxon.Desc.ClassicRank == ClassicRankEnum.Espece || _taxon.Desc.ClassicRank == ClassicRankEnum.SousEspece)
            {
                if (_taxon.Desc.RedListCategory != RedListCategoryEnum.NotEvaluated)
                {
                    R.Width = 6;
                    g.FillRectangle(RedListCategoryExt.GetBackBrush(_taxon.Desc.RedListCategory), R);
                }
            }
        }

        //-------------------------------------------------------------------
        void PaintTaxonName(Graphics g, TaxonTreeNode _taxon, ref Rectangle R, Brush brushText)
        {
            Rectangle textR = R;
            textR.Inflate(-4, -4);

            Font currentFont = Font;
            SizeF sizeText = g.MeasureString(_taxon.Desc.RefMainName, currentFont);

            // reduce eventually font
            if (sizeText.Width > textR.Width)
            {
                float ratio = (float)sizeText.Width / (float)textR.Width;
                if (ratio > 1.2f) ratio = 1.2f;

                currentFont = new Font(currentFont.FontFamily, currentFont.Size / ratio);
                sizeText = g.MeasureString(_taxon.Desc.RefMainName, currentFont, textR.Width);
            }

            textR.Inflate(4, 4);
            g.DrawString(_taxon.Desc.RefMainName, currentFont, brushText, textR, GraphResources.CenteredText);

        }

        //-------------------------------------------------------------------
        void PaintTaxonSiblingsUp(Graphics g, TaxonTreeNode _node, int _siblingIndex, ref Rectangle _totalR, Rectangle _taxonR)
        {
            Pen pen = Pens.DarkGray;

            _taxonR.Inflate(-1, -1);
            while (_siblingIndex >= 0)
            {
                _taxonR.Y -= _taxonR.Height + 4;
                if (_taxonR.Bottom < -Origin.Y) break;
                int x = _taxonR.Left - 8;
                int y = _taxonR.Top + 10;
                g.DrawLine(pen, x, y, x + 3, y - 3);
                g.DrawLine(pen, x + 3, y - 3, x + 6, y);
                y -= 3;
                g.DrawLine(pen, x, y, x + 3, y - 3);
                g.DrawLine(pen, x + 3, y - 3, x + 6, y);
                g.DrawRectangle(pen, _taxonR);
                TaxonTreeNode _sibling = _node.Father.Children[_siblingIndex];
                Brush brush = _sibling == BelowMouse ? Brushes.LightGray : Brushes.DarkGray;
                PaintTaxonName(g, _node.Father.Children[_siblingIndex], ref _taxonR, brush);
                ShortcutModeAddSibling(_node, _sibling, _taxonR);
                _siblingIndex--;
            }
        }

        //-------------------------------------------------------------------
        void PaintTaxonSiblingsDown(Graphics g, TaxonTreeNode _node, int _siblingIndex, ref Rectangle _totalR, Rectangle _taxonR)
        {
            Pen pen = Pens.DarkGray;
            int last = _node.Father.Children.Count - 1;
            _taxonR.Inflate(-1, -1);
            while (_siblingIndex <= last)
            {
                _taxonR.Y += _taxonR.Height + 4;
                if (_taxonR.Bottom < -Origin.Y) break;
                int x = _taxonR.Left - 8;
                int y = _taxonR.Bottom - 10;
                g.DrawLine(pen, x, y, x + 3, y + 3);
                g.DrawLine(pen, x + 3, y + 3, x + 6, y);
                y -= 3;
                g.DrawLine(pen, x, y, x + 3, y + 3);
                g.DrawLine(pen, x + 3, y + 3, x + 6, y);
                g.DrawRectangle(pen, _taxonR);

                TaxonTreeNode _sibling = _node.Father.Children[_siblingIndex];
                Brush brush = _sibling == BelowMouse ? Brushes.LightGray : Brushes.DarkGray;
                PaintTaxonName(g, _node.Father.Children[_siblingIndex], ref _taxonR, brush );
                ShortcutModeAddSibling(_node, _sibling, _taxonR);
                _siblingIndex++;
            }
        }

        //-------------------------------------------------------------------
        void PaintClassicRankRule(Graphics g)
        {
            List<int> columns = ClassicRankEnumExt.ClassicRankColumns;

            Rectangle R = new Rectangle(0, 0, ClientRectangle.Width, 30);
            g.FillRectangle(Brushes.Silver, R);

            int W = Options.ZoomedWidth + Options.ZoomedColumnWidth;
            g.TranslateTransform(Origin.X, 0);
            foreach (ClassicRankEnum rank in Enum.GetValues(typeof(ClassicRankEnum)))
            {
                int column = columns[(int)rank];
                if (column == -1) continue;
                R = new Rectangle(column * W - Options.ZoomedColumnWidth / 2, 0, W, 30);
                g.FillRectangle((column & 1) == 1 ? Brushes.LightGray : Brushes.Silver, R);
                g.DrawString(VinceToolbox.Helpers.enumHelper.GetEnumDescription(rank), Font, Brushes.Black, R, GraphResources.CenteredText);

                R.Y = R.Bottom;
                R.Height = ClientRectangle.Height - R.Height;
                Brush brush = (column & 1) == 1 ? GraphResources.BrushTransparentDimGray : GraphResources.BrushTransparentGray;
                g.FillRectangle(brush, R);
            }
            g.ResetTransform();
        }

        //-------------------------------------------------------------------
        // simple paint : used by navigator for example
        // todo : stop using main graph members, alloc them
        public class SimpleGraphParams
        {
            public SimpleGraphParams(TaxonTreeNode _root)
            {
                Root = _root;
                TotalWidth = _root.WidthWithChildren;
                ColumnInter = TaxonUtils.MainGraph.Graph.Options.ColumnInter;
                Colors = TaxonUtils.MyConfig.Options.FullTreeColor;
                Font = TaxonUtils.MainGraph.Font;
                ShowTaxonNames = TaxonUtils.MyConfig.Options.NavigatorShowTaxonNames;
                ShowTaxonNamesMinHeight = TaxonUtils.MyConfig.Options.NavigatorShowTaxonNamesMinHeight;
                ShowTaxonNamesMinWidth = TaxonUtils.MyConfig.Options.NavigatorShowTaxonNamesMinWidth;
                ShowTaxonNamesMaxNameWidth = TaxonUtils.MyConfig.Options.NavigatorShowTaxonNamesMaxNameWidth;
                SetLinkSize(3.0f, 3.0f);
                SetLeftMargin(0);
                SetScaleTaxonToGraph(1.0f, 1.0f);
                SetScaleGraphToScreen(1.0f, 1.0f);
            }

            public SimpleGraphParams SetLinkSize(float w, float h)
            {
                LinkSize = new SizeF(w, h);
                LinkSizeBy2 = new SizeF(w / 2, h / 2);
                ReduceTaxonRect = h < 8;
                return this;
            }

            public SimpleGraphParams SetLeftMargin(float w)
            {
                LeftMargin = w;
                return this;
            }

            public SimpleGraphParams SetScaleTaxonToGraph(float w, float h)
            {
                ScaleTaxonToGraph.Width = w;
                ScaleTaxonToGraph.Height = h;
                ScaleTaxonToScreen.Width = w * ScaleGraphToScreen.Width;
                ScaleTaxonToScreen.Height = w * ScaleGraphToScreen.Height;

                if (TaxonUtils.MainGraph.Graph.Options.TaxonWidth * ScaleTaxonToGraph.Width < ShowTaxonNamesMinWidth)
                    ShowTaxonNames = false;

                return this;
            }

            public SimpleGraphParams SetScaleGraphToScreen(float w, float h)
            {
                ScaleGraphToScreen.Width = w;
                ScaleGraphToScreen.Height = h;
                ScaleScreenToGraph.Width = 1 / w;
                ScaleScreenToGraph.Height = 1 / h;
                ScaleTaxonToScreen.Width = w * ScaleTaxonToGraph.Width;
                ScaleTaxonToScreen.Height = h * ScaleTaxonToGraph.Height;
                return this;
            }

            public TaxonTreeNode Root;
            public int TotalWidth;
            public int ColumnInter;
            public GraphColors Colors;
            public float LeftMargin;
            public SizeF LinkSize;
            public SizeF LinkSizeBy2;
            public bool ReduceTaxonRect;
            public Font Font;
            public bool ShowTaxonNames;
            public int ShowTaxonNamesMinHeight;
            public int ShowTaxonNamesMinWidth;
            public int ShowTaxonNamesMaxNameWidth;

            public SizeF ScaleTaxonToGraph;
            public SizeF ScaleGraphToScreen;
            public SizeF ScaleScreenToGraph;
            public SizeF ScaleTaxonToScreen;

            public class RectAndName
            {
                public TaxonTreeNode Taxon;
                public string Name;
                public RectangleF RectF;
                public bool Displayed;
                public Rectangle Rect;
                public bool Center;
            }
            public List<RectAndName> SecondPassNames = new List<RectAndName>();
        }

        //-------------------------------------------------------------------
        public static void PaintTaxonRecursiveSimple( Graphics g, SimpleGraphParams paintParams, TaxonTreeNode _taxon, bool _treeAspect = true )
        {
            if (_treeAspect)
            {
                g.TranslateTransform(paintParams.LeftMargin, 0);
                g.ScaleTransform(paintParams.ScaleTaxonToGraph.Width, paintParams.ScaleTaxonToGraph.Height);
                PaintTaxonRecursiveSimpleLines(g, paintParams, _taxon);
                g.ResetTransform();

                g.ScaleTransform(paintParams.ScaleScreenToGraph.Width, paintParams.ScaleScreenToGraph.Height);
                //PaintTaxonRecursiveSimpleNames(g, paintParams);
                PaintTaxonRecursiveSimpleNamesNoOverlap(g, paintParams);
                g.ResetTransform();
            }
            else
            {
                g.TranslateTransform(paintParams.LeftMargin, 0);
                g.ScaleTransform(paintParams.ScaleTaxonToGraph.Width, paintParams.ScaleTaxonToGraph.Height);
                PaintTaxonRecursiveSimpleGrid(g, paintParams, _taxon);
                g.ResetTransform();
            }

        }

        //-------------------------------------------------------------------
        public static void PaintTaxonRecursiveSimpleNames(Graphics g, SimpleGraphParams paintParams)
        {
            int offset = (int)(paintParams.LeftMargin / paintParams.ScaleScreenToGraph.Width);
            foreach (SimpleGraphParams.RectAndName rn in paintParams.SecondPassNames)
            {
                Rectangle R = new Rectangle()
                {
                    X = (int)(rn.RectF.X * paintParams.ScaleTaxonToScreen.Width) + 2 + offset,
                    Width = (int)(rn.RectF.Width * paintParams.ScaleTaxonToScreen.Width),
                    Y = (int)(rn.RectF.Y * paintParams.ScaleTaxonToScreen.Height - (rn.Center ? 7 : 14)),
                    Height = 14
                };

                SizeF textSize = g.MeasureString(rn.Name, paintParams.Font);
                textSize.Width += 4;
                if (textSize.Width > R.Width)
                {
                    int width = Math.Min((int)textSize.Width, paintParams.ShowTaxonNamesMaxNameWidth);
                    int right = R.Right;
                    R.X = Math.Max(right - width, 0);
                    R.Width = right - R.X;
                }

                g.DrawString(rn.Name, paintParams.Font, Brushes.White, R, GraphResources.NavigatorText);
            }
        }

        //-------------------------------------------------------------------
        public static void PaintTaxonRecursiveSimpleNamesNoOverlap(Graphics g, SimpleGraphParams paintParams)
        {
            Region antiOverlapRegion = new Region();
            antiOverlapRegion.MakeEmpty();
            int offset = (int)(paintParams.LeftMargin / paintParams.ScaleScreenToGraph.Width);
            for (int i = paintParams.SecondPassNames.Count - 1; i >= 0; i--)
            {
                SimpleGraphParams.RectAndName rn = paintParams.SecondPassNames[i];

                Rectangle R = new Rectangle()
                {
                    X = (int)(rn.RectF.X * paintParams.ScaleTaxonToScreen.Width) + 2 + offset,
                    Width = (int)(rn.RectF.Width * paintParams.ScaleTaxonToScreen.Width),
                    Y = Math.Max(0, (int)(rn.RectF.Y * paintParams.ScaleTaxonToScreen.Height - (rn.Center ? 7 :14))),
                    Height = 14
                };

                SizeF textSize = g.MeasureString(rn.Name, paintParams.Font);
                textSize.Width += 4;
                if (textSize.Width > R.Width)
                {
                    int width = Math.Min((int)textSize.Width, paintParams.ShowTaxonNamesMaxNameWidth);
                    int right = R.Right;
                    R.X = Math.Max(right - width, 0);
                    R.Width = right - R.X;
                }

                if (antiOverlapRegion.IsVisible(R))
                {
                    continue;
                }
                else
                {
                    antiOverlapRegion.Union(R);
                }

                g.DrawString(rn.Name, paintParams.Font, Brushes.White, R, GraphResources.NavigatorText);
                rn.Rect = R;
                rn.Displayed = true;
            }
        }

        //-------------------------------------------------------------------
        static void PaintTaxonRecursiveSimpleGrid(Graphics g, SimpleGraphParams _params, TaxonTreeNode _taxon)
        {
            Rectangle R = _taxon.R;

            InternalColorEnum internalColor = InternalColorEnum.espece;

            // stretch species end leafs taxon (species with no sub species or sub species)
            if (_taxon.IsEndLeaf)
            {
                R.Width = _params.TotalWidth - R.Left;
                _taxon.R.Width = R.Width;
            }
            else
            {
                //brush = Brushes.SteelBlue;
                internalColor = InternalColorEnum.expanded;
                foreach (TaxonTreeNode child in _taxon.Children)
                {
                    if (!child.Visible)
                    {
                        internalColor = InternalColorEnum.collapsed;
                        continue;
                    }
                    PaintTaxonRecursiveSimpleGrid(g, _params, child);
                }
                R.Width += _params.ColumnInter;
            }

            Brush brushBack;
            if (_taxon.Highlight)
                brushBack = _params.Colors.HighlightedBrush;
            else
            {
                if (internalColor == InternalColorEnum.espece)
                    brushBack = Brushes.Azure;
                else
                {
                    if (_taxon.Desc.IsExtinct)
                        brushBack = (internalColor == InternalColorEnum.collapsed) ? _params.Colors.ExtinctCollapsedBrush : _params.Colors.ExtinctExpandedBrush;
                    else
                        brushBack = (internalColor == InternalColorEnum.collapsed) ? _params.Colors.CollapsedBrush : _params.Colors.ExpandedBrush;
                }
            }

            Rectangle RLeftMargin = R;
            if (_taxon.RLeftMargin != 0)
            {
                R.X += _taxon.RLeftMargin;
                R.Width -= _taxon.RLeftMargin;
            }

            g.FillRectangle(brushBack, R);
        }

        //-------------------------------------------------------------------
        static float PaintTaxonRecursiveSimpleLines(Graphics g, SimpleGraphParams _params, TaxonTreeNode _taxon )
        {
            Rectangle R = _taxon.R;

            InternalColorEnum internalColor = InternalColorEnum.espece;

            // stretch species end leafs taxon (species with no sub species or sub species)
            bool display = true;
            float xChild = 0;
            float yMin = float.MaxValue, yMax = float.MinValue;
            if (_taxon.IsEndLeaf)
            {
                R.Width = _params.TotalWidth - R.Left;
                _taxon.R.Width = R.Width;
            }
            else
            {
                //brush = Brushes.SteelBlue;
                internalColor = InternalColorEnum.expanded;
                foreach (TaxonTreeNode child in _taxon.Children)
                {
                    if (!child.Visible)
                    {
                        internalColor = InternalColorEnum.collapsed;
                        continue;
                    }
                    float y = PaintTaxonRecursiveSimpleLines(g, _params, child);
                    xChild = child.R.Left;
                    yMin = Math.Min(y, yMin);
                    yMax = Math.Max(y, yMax);
                    display = false;
                }
            }

            if (!display)
            {
                float x0 = R.X;
                float x1 = xChild;

                float y = (yMin + yMax) * 0.5f;
                g.FillRectangle(Brushes.Black, x0, y - _params.LinkSizeBy2.Height, x1 - x0, _params.LinkSize.Height);

                if (yMax > yMin)
                    g.FillRectangle(Brushes.Black, x1 - _params.LinkSizeBy2.Width, yMin, _params.LinkSize.Width, yMax - yMin);

                if (_params.ShowTaxonNames)
                {
                    if (R.Height * _params.ScaleTaxonToGraph.Height > _params.ShowTaxonNamesMinHeight && !_taxon.Desc.IsUnnamed)
                    {
                        _params.SecondPassNames.Add(new SimpleGraphParams.RectAndName()
                        {
                            Taxon = _taxon,
                            Name = _taxon.Desc.RefMainName,
                            RectF = RectangleF.FromLTRB(x0, y, x1, y),
                            Center = false
                        });
                    }
                }
                return y;
            }

            Brush brushBack;
            if (_taxon.Highlight)
                brushBack = _params.Colors.HighlightedBrush;
            else
            {
                if (internalColor == InternalColorEnum.espece)
                {
                    if (_taxon.Desc.IsExtinct)
                        brushBack = Brushes.Black;
                    else
                        brushBack = Brushes.Azure;
                }
                else
                {
                    if (_taxon.Desc.IsExtinct)
                        brushBack = (internalColor == InternalColorEnum.collapsed) ? _params.Colors.ExtinctCollapsedBrush : _params.Colors.ExtinctExpandedBrush;
                    else
                        brushBack = (internalColor == InternalColorEnum.collapsed) ? _params.Colors.CollapsedBrush : _params.Colors.ExpandedBrush;
                }
            }

            Rectangle RLeftMargin = R;
            if (_taxon.RLeftMargin != 0)
            {
                R.X += _taxon.RLeftMargin;
                R.Width -= _taxon.RLeftMargin;
            }

            if (_params.ShowTaxonNames && _taxon.Desc.ClassicRank != ClassicRankEnum.Espece && _taxon.Desc.ClassicRank!= ClassicRankEnum.SousEspece)
            {
                if (R.Height * _params.ScaleTaxonToGraph.Height >= _params.ShowTaxonNamesMinHeight && !_taxon.Desc.IsUnnamed)
                {
                    float y = (R.Top + R.Bottom) * 0.5f;
                    _params.SecondPassNames.Add(new SimpleGraphParams.RectAndName()
                    {
                        Taxon = _taxon,
                        Name = _taxon.Desc.RefMainName,
                        RectF = RectangleF.FromLTRB(R.Left, y, R.Right, y),
                        Center = true
                    });
                }
            }

            if (_params.ReduceTaxonRect)
            {
                R.Y += 1;
                R.Height -= 2;
            }
            g.FillRectangle(brushBack, R);

            return R.Y + R.Height / 2;
        }

    }
}
