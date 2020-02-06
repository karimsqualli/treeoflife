using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeOfLife.Properties;

namespace TreeOfLife.Controls
{
    public partial class TaxonImageControl
    {
        //=================================================================================
        // Constant / Constructor 

        //---------------------------------------------------------------------------------
        public static Image ImageNone { get; private set; } = null;
        public static Image ImageWait { get; private set; } = null;

        public static string FontFamily = "Microsoft Sans Serif";

        public static ImageAttributes ImageAttributesWhiteToBlack;
        public static ImageAttributes ImageAttributesWhiteToGray;

        //---------------------------------------------------------------------------------
        static TaxonImageControl()
        {
            ImageNone = Resources.ResourceManager.GetObject("NoImage") as Image;
            ImageWait = Resources.ResourceManager.GetObject("Wait") as Image;
            ImageAttributesWhiteToGray = GUI.Resources.BuildImageAttributesWhiteToColor(Color.Gray);
            ImageAttributesWhiteToBlack = GUI.Resources.BuildImageAttributesWhiteToColor(Color.Black);
        }

        //=================================================================================
        // Tools
        //

        //---------------------------------------------------------------------------------
        static void SquarifyRectangle(ref Rectangle R)
        {
            if (R.Width > R.Height)
            {
                R.X = R.X + (R.Width - R.Height) / 2;
                R.Width = R.Height;
            }
            else
            {
                R.Y = R.Y + (R.Height - R.Width) / 2;
                R.Height = R.Width;
            }
        }

        //=================================================================================
        // Paint
        //

        //---------------------------------------------------------------------------------
        static void PaintSystemImage(Graphics G, Rectangle R, Image _image)
        {
            SquarifyRectangle(ref R);
            if (R.Width > _image.Width)
            {
                int diff = (R.Width - _image.Width) / 2;
                R.Offset(diff, diff);
                R.Width = R.Height = _image.Width;
            }
            G.DrawImage(_image, R);
        }

        //---------------------------------------------------------------------------------
        static void PaintLegendText(Graphics G, Font _font, Brush _textBrush, string _text, Rectangle R)
        {
            if (_text == "") return;
            StringFormat centeredText = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };
            G.DrawString(_text, _font, _textBrush, R, centeredText);
        }

        //---------------------------------------------------------------------------------
        static void StaticPaintLegend(Graphics G, Rectangle R, out int _height, VignetteDisplayParams _params, VignetteData _data)
        {
            _height = 0;

            int minHeight = (_params.DisplayFrench ? 14 : 0) + (_params.DisplayLatin ? 14 : 0);
            if (R.Height <= minHeight || R.Width <= LegendMinWidth)
                return;

            float _FontSize = R.Width / WidthOverFontSize;
            if (_FontSize > MaxFontSize) _FontSize = MaxFontSize;
            if (_FontSize < MinFontSize) _FontSize = MinFontSize;
            Font fontFirst = new Font(FontFamily, _FontSize, FontStyle.Bold);
            Font fontSecond = new Font(FontFamily, _FontSize, FontStyle.Italic);
            int heightFirst = (int)G.MeasureString("fg", fontFirst).Height;
            int heightSecond = (int)G.MeasureString("fg", fontSecond).Height;

            int heightBetween = 2;
            if (_FontSize != MaxFontSize)
                heightBetween = (int)(0.5f + 2.0f * (_FontSize - MinFontSize) / (MaxFontSize - MinFontSize));

            string firstName = null;
            bool firstVisible = true;
            string secondName = null;
            bool secondVisible = true;
            if (_params.DisplayLatin)
            {
                firstName = _data.CurrentTaxon.Desc.RefMainName;
                firstVisible = _params.LatinVisible;
                if (_params.DisplayFrench)
                {
                    secondName = _data.CurrentTaxon.Desc.FrenchMainName;
                    secondVisible = _params.FrenchVisible;
                }
            }
            else if (_params.DisplayFrench)
            {
                firstName = _data.CurrentTaxon.Desc.FrenchMainName;
                firstVisible = _params.FrenchVisible;
            }

            if (!_params.DisplayEmptyText)
            {
                if (firstName == "")
                {
                    if (secondName == "")
                        return;
                    firstName = secondName;
                    secondName = null;
                }
                if (secondName == "")
                    secondName = null;
            }

            bool adaptFontSize = false;
            float ratio = 1.0f;
            if (secondName != null)
            {
                _height = 2 + heightFirst + heightBetween + heightSecond + 2;
                if (_height > R.Height)
                {
                    ratio = (float) _height / R.Height;
                    if (ratio < 1.2f)
                        adaptFontSize = true;
                    else
                        secondName = null;
                }
            }

            int decalFirstLine = 0;
            int decalAfterFirstLine = 0;
            if (secondName == null)
            {
                _height = heightFirst + 4;
                if (_height > R.Height)
                {
                    ratio = (float) _height / R.Height;
                    adaptFontSize = true;
                }
            }
            else if (secondName == "")
            {
                decalFirstLine = (heightBetween + heightSecond) / 2;
                decalAfterFirstLine = heightBetween + heightSecond - decalFirstLine - 2;
                secondName = null;
            }

            if (adaptFontSize)
            {
                _FontSize /= ratio;
                fontFirst = new Font(FontFamily, _FontSize, FontStyle.Bold);
                heightFirst = (int)G.MeasureString("fg", fontFirst).Height;
                _height = 4 + heightFirst;

                if (secondName != null)
                {
                    fontSecond = new Font(FontFamily, _FontSize, FontStyle.Italic);
                    heightSecond = (int)G.MeasureString("fg", fontSecond).Height;
                    _height += heightBetween + heightSecond;
                }
            }

            // draw background
            _params.GetLegendBrushes(_data.CurrentTaxon.Desc.IsExtinct, out Brush backBrush, out Brush textBrush, out Brush textSecondaryBrush );
            if (_params.DisplayImage) R.Height = _height;
            G.FillRectangle(backBrush, R);

            Rectangle RText = R;
            if (!_params.DisplayImage && RText.Height > _height) RText.Y += (RText.Height - _height) / 2;

            RText.Inflate(-2, 0);
            if (firstName != null)
            {
                RText.Y += 2 + _params.LegendFirstLineOffset + decalFirstLine;
                RText.Height = heightFirst;
                if (firstVisible) PaintLegendText(G, fontFirst, textBrush, firstName, RText);
                RText.Y = RText.Bottom - _params.LegendFirstLineOffset; 
            }

            if (secondName != null)
            {
                RText.Y += heightBetween;
                RText.Height = heightSecond;
                if (secondVisible) PaintLegendText(G, fontSecond, textSecondaryBrush, secondName, RText);
                RText.Y = RText.Bottom;
            }

            _height = (RText.Y + decalAfterFirstLine + 2 ) - R.Y;
        }

        //---------------------------------------------------------------------------------
        public static void StaticPaint(Graphics _graphics, Rectangle R, Point _mousePos, VignetteDisplayParams _params, VignetteData _data)
        {
            Rectangle SavingR = R;
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            // Change display mode if size too small
            VignetteDisplayParams.ModeEnum displayMode = _params.DisplayMode;
            if (_params.DisplayImage && Math.Min(R.Width, R.Height) < 64)
                displayMode = VignetteDisplayParams.ModeEnum.Brut;

            Rectangle buttonsR = R;

            // draw border
            if (_params.BorderSize > 0 || _params.BottomMargin > 0)
            {
                if (_data.CurrentTaxon.Desc.IsExtinct)
                    _graphics.FillRectangle(_params.BorderBrushExtinct, R);
                else
                    _graphics.FillRectangle(_params.BorderBrush, R);

                R.Inflate(-_params.BorderSize, -_params.BorderSize);
                R.Height -= _params.BottomMargin;
                buttonsR.X = R.X;
                buttonsR.Width = R.Width;
            }
            int buttonsHeigth = Math.Max(16, _params.BorderSize + _params.BottomMargin);
            buttonsR.Y = buttonsR.Bottom - buttonsHeigth;
            buttonsR.Height = buttonsHeigth;

            // draw legend
            if (displayMode == VignetteDisplayParams.ModeEnum.LegendBorder && LegendHeight > 0)
            {
                Rectangle legendRectangle = R;
                if (_params.DisplayImage)
                {
                    int legendHeight = LegendHeight;
                    float ratio = MaxLegendHeightRatio;
                    if ((float)legendHeight / (float)R.Height > ratio)
                        legendHeight = (int)(R.Height * ratio);
                    legendRectangle.Height = legendHeight;
                }

                bool paintRedListEllipse = legendRectangle.Height > 12 &&
                                            _params.DisplayRedListCategory &&
                                            _data.CurrentTaxon.Desc.RedListCategory != RedListCategoryEnum.NotEvaluated &&
                                            _data.CurrentTaxon.Desc.RedListCategory != RedListCategoryEnum.Extinct;
                if (paintRedListEllipse)
                {
                    legendRectangle.X += 14;
                    legendRectangle.Width -= 14;
                }

                legendRectangle.Y -= _params.BorderSize;
                StaticPaintLegend(_graphics, legendRectangle, out int height, _params, _data);
                R.Y += height;
                R.Height -= height;

                if (paintRedListEllipse)
                {
                    Rectangle ellipseR = legendRectangle;
                    ellipseR.X -= 14;
                    ellipseR.Width = 14;
                    ellipseR.Height = height;
                    _graphics.FillRectangle(_params.GetLegendBackBrush(_data.CurrentTaxon.Desc.IsExtinct), ellipseR);

                    if (height > 16)
                    {
                        int x = legendRectangle.Left - 14;
                        int y = legendRectangle.Top + (height - 12) / 2;
                        _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        _graphics.FillEllipse(RedListCategoryExt.GetBackBrush(_data.CurrentTaxon.Desc.RedListCategory), x, y, 12, 12);
                        _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    }
                }
            }

            // paint image
            if (_params.DisplayImage)
            {
                _graphics.FillRectangle(_params.ImageBackBrush, R);

                if (_params.ImageVisible)
                {
                    Image image = _data.GetCurrentImageToDraw();

                    R.Inflate(-_params.ImageBorderSize, -_params.ImageBorderSize);

                    if (_data.CurrentImageIsSystem)
                    {
                        PaintSystemImage(_graphics, R, image);
                    }
                    else
                    {
                        float imageWoH = image.Width / (float)image.Height;
                        float rectWoH = R.Width / (float)R.Height;

                        Size rectSize = R.Size;

                        int rectRefValue = Math.Max(R.Width, R.Height);
                        if (rectRefValue >= _params.FitInRectAboveThisSize)
                        {
                            // full fit
                            if (rectWoH >= imageWoH)
                                rectSize.Width = (int)(R.Height * imageWoH);
                            else
                                rectSize.Height = (int)(R.Width / imageWoH);
                        }
                        else if (rectRefValue < _params.CropUnderThisSize)
                        {
                            // full crop
                            if (rectWoH >= imageWoH)
                                rectSize.Height = (int)(R.Width / imageWoH);
                            else
                                rectSize.Width = (int)(R.Height * imageWoH);
                        }
                        else
                        {
                            Size rectCropSize = R.Size;
                            Size rectFitSize = R.Size;
                            if (rectWoH >= imageWoH)
                            {
                                rectFitSize.Width = (int)(R.Height * imageWoH);
                                rectCropSize.Height = (int)(R.Width / imageWoH);
                            }
                            else
                            {
                                rectFitSize.Height = (int)(R.Width / imageWoH);
                                rectCropSize.Width = (int)(R.Height * imageWoH);
                            }

                            float ratio = (float)(rectRefValue - _params.CropUnderThisSize) / (_params.FitInRectAboveThisSize - _params.CropUnderThisSize);
                            rectSize.Width = (int)(ratio * rectFitSize.Width) + (int)((1 - ratio) * rectCropSize.Width);
                            rectSize.Height = (int)(ratio * rectFitSize.Height) + (int)((1 - ratio) * rectCropSize.Height);
                        }

                        Rectangle imageRect = Rectangle.FromLTRB(0, 0, image.Width, image.Height);

                        if (rectSize.Width > R.Width)
                        {
                            float r = (float)R.Width / rectSize.Width;
                            int w = (int)(imageRect.Width * r);
                            imageRect.X = (imageRect.Width - w) / 2;
                            imageRect.Width = w;
                        }
                        else if (R.Width != rectSize.Width)
                        {
                            R.X += (R.Width - rectSize.Width) / 2;
                            R.Width = rectSize.Width;
                        }

                        if (rectSize.Height > R.Height)
                        {
                            float r = (float)R.Height / rectSize.Height;
                            int h = (int)(imageRect.Height * r);
                            imageRect.Y = (imageRect.Height - h) / 2;
                            imageRect.Height = h;
                        }
                        else if (R.Height != rectSize.Height)
                        {
                            R.Y += (R.Height - rectSize.Height) / 2;
                            R.Height = rectSize.Height;
                        }

                        _graphics.DrawImage(image, R, imageRect, GraphicsUnit.Pixel);
                    }

                    // paint next / previous
                    StaticPaintButtons(_graphics, buttonsR, _params, _data);
                }
            }

            if (_data.SoundPlayerData != null)
            {
                //Rectangle SoundRectangle = buttonsR;
                /*SoundRectangle.X += 2;
                SoundRectangle.Width -= 2;
                SoundRectangle.Y = SoundRectangle.Bottom - 20;
                SoundRectangle.Height = 20;*/
                VinceSoundPlayer.PlayerSmall.StaticPaint(_graphics, buttonsR, _mousePos, _data.SoundPlayerData, _params.SoundPlayerDisplayParams );
            }

            _params.EndPaint(_data.CurrentTaxon, SavingR, _graphics);
        }

        static void StaticPaintButtons(Graphics g, Rectangle R, VignetteDisplayParams _params, VignetteData _data)
        {
            _data.ResetButtons();
            // paint next / previous
            if (_data.ListImages == null || _data.ListImages.Count < 2 || !_data.AllowNavigationButtons)
                return;

            //if (Math.Min(R.Width, R.Height) < 48)
            if (R.Height < 16 || R.Width < 48)
                return;

            ImageAttributes hoveredEffect = _data.CurrentTaxon.Desc.IsExtinct ? null : ImageAttributesWhiteToBlack;

            int size = 12;
            Rectangle RArrow = R;
            RArrow.X = R.Right - size - 2;
            RArrow.Y += (R.Height - size) / 2;
            RArrow.Height = size;
            RArrow.Width = size;

            if (_data.CurrentImageIndex < _data.ListImages.Count - 1)
            {
                Image next = Resources.Next_Image;
                ImageAttributes attr = _data.ButtonNextHovered ? hoveredEffect : ImageAttributesWhiteToGray;
                g.DrawImage(next, RArrow, 0,0,next.Width, next.Height, GraphicsUnit.Pixel, attr);
                _data.EnableButtonNext(RArrow);
            }

            RArrow.X -= size;
            if (_data.CurrentImageIndex > 0)
            {
                Image prev = Resources.Previous_Image;
                ImageAttributes attr = _data.ButtonPreviousHovered ? hoveredEffect : ImageAttributesWhiteToGray;
                g.DrawImage(prev, RArrow, 0, 0, prev.Width, prev.Height, GraphicsUnit.Pixel, attr);
                _data.EnableButtonPrevious(RArrow);
            }
        }

        public static bool StaticClickButtons(Point _pt, VignetteData _data)
        {
            if (_data == null || _data.ListImages == null || _data.ListImages.Count < 2) return false;

            if (_data.ButtonNextEnable && _data.ButtonNextBounds.Contains(_pt))
            {
                _data.CurrentImageIndex = (_data.CurrentImageIndex + 1) % _data.ListImages.Count;
                return true;
            }

            if (_data.ButtonPreviousEnable && _data.ButtonPreviousBounds.Contains(_pt))
            {
                _data.CurrentImageIndex = (_data.CurrentImageIndex + _data.ListImages.Count - 1) % _data.ListImages.Count;
                return true;
            }
            return false;
        }

    }
}
