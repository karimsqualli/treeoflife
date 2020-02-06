 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TreeOfLife.Controls
{
    public partial class TaxonImageControl
    {
        //---------------------------------------------------------------------------------
        static VignetteDisplayParams.ModeEnum _ImageDisplayMode = VignetteDisplayParams.ModeEnum.Brut;
        static public VignetteDisplayParams.ModeEnum ImageDisplayMode
        {
            get { return _ImageDisplayMode; }
            set
            {
                _ImageDisplayMode = value;
                TaxonUtils.Invalidate();
            }
        }

        static int _LegendHeight = 70;
        static public int LegendHeight
        {
            get { return _LegendHeight; }
            set 
            {
                _LegendHeight = value;
                TaxonUtils.Invalidate();
            }
        }

        static float _MaxLegendHeightRatio = 0.2f;
        static public float MaxLegendHeightRatio
        {
            get { return _MaxLegendHeightRatio; }
            set
            {
                _MaxLegendHeightRatio = value;
                TaxonUtils.Invalidate();
            }
        }

        static int _LegendMinWidth = 60;
        static public int LegendMinWidth
        {
            get { return _LegendMinWidth; }
            set
            {
                _LegendMinWidth = value;
                TaxonUtils.Invalidate();
            }
        }

        static int _MinFontSize = 6;
        static public int MinFontSize
        {
            get { return _MinFontSize; }
            set
            {
                _MinFontSize = value;
                TaxonUtils.Invalidate();
            }
        }

        static int _MaxFontSize = 24;
        static public int MaxFontSize
        {
            get { return _MaxFontSize; }
            set
            {
                _MaxFontSize = value;
                TaxonUtils.Invalidate();
            }
        }

        static float _WidthOverFontSize = 24;
        static public float WidthOverFontSize
        {
            get { return _WidthOverFontSize; }
            set
            {
                _WidthOverFontSize = value;
                TaxonUtils.Invalidate();
            }
        }
    }
}
