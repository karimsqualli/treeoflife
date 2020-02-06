using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using VinceSoundPlayer;

namespace TreeOfLife.Controls
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class VignetteDisplayParams
    {
        //---------------------------------------------------------------------------------
        public VignetteDisplayParams(string callerName)
        {
            // force some values to have valid parameters
            InitColors();
            Version = CurrentVersion;
        }

        public VignetteDisplayParams()
        {
            InitColors();
        }

        public VignetteDisplayParams(VignetteDisplayParams _from)
        {
            if (_from.Version != CurrentVersion) return;
            DisplayMode = _from.DisplayMode;
            DisplayImage = _from.DisplayImage;
            ImageVisible = _from.ImageVisible;
            DisplayEmptyText = _from._DisplayEmptyText;
            DisplayLatin = _from.DisplayLatin;
            LatinVisible = _from._LatinVisible;
            DisplayFrench = _from.DisplayFrench;
            FrenchVisible = _from.FrenchVisible;
            CropUnderThisSize = _from.CropUnderThisSize;
            FitInRectAboveThisSize = _from.FitInRectAboveThisSize;
            BorderSize = _from.BorderSize;
            BorderColor = _from.BorderColor;
            BorderColorExtinct = _from.BorderColorExtinct;
            BottomMargin = _from.BottomMargin;
            LegendFirstLineOffset = _from.LegendFirstLineOffset;
            LegendAliveBackColor = _from.LegendAliveBackColor;
            LegendAliveForeColor = _from.LegendAliveForeColor;
            LegendAliveSecondaryForeColor = _from.LegendAliveSecondaryForeColor;
            LegendExtinctBackColor = _from.LegendExtinctBackColor;
            LegendExtinctForeColor = _from.LegendExtinctForeColor;
            LegendExtinctSecondaryForeColor = _from.LegendExtinctSecondaryForeColor;
            ImageBackColor = _from.ImageBackColor;
            ImageBorderSize = _from.ImageBorderSize;
        }

        //---------------------------------------------------------------------------------
        [XmlAttribute, Browsable(false)]
        public int Version { get; set; } = 0;

        //---------------------------------------------------------------------------------
        public void InitColors()
        {
            BorderColor = Color.White;
            BorderColorExtinct = Color.Black;
            LegendAliveBackColor = Color.White;
            LegendAliveForeColor = Color.Black;
            LegendAliveSecondaryForeColor = Color.Black;
            LegendExtinctBackColor = Color.Black;
            LegendExtinctForeColor = Color.White;
            LegendExtinctSecondaryForeColor = Color.White;
            ImageBackColor = Color.White;
        }

        //---------------------------------------------------------------------------------
        public event EventHandler OnChanged;

        //-------------------------------------------------------------------
        public class OnPaintedArgs : EventArgs
        {
            public Graphics Graphics;
            public Rectangle Bounds;
            public TaxonTreeNode Taxon;
        }
        public delegate void OnPaintedHandler(object sender, OnPaintedArgs e);
        public event OnPaintedHandler OnPainted;
        public void EndPaint(TaxonTreeNode _taxon, Rectangle _bounds, Graphics _graphics)
        {
            OnPainted?.Invoke(this, new OnPaintedArgs() { Bounds = _bounds, Taxon = _taxon, Graphics = _graphics });
        }

        //---------------------------------------------------------------------------------
        public enum ModeEnum
        {
            Undefined,
            Brut,
            LegendBorder,
        }

        public ModeEnum DisplayMode
        {
            get => _DisplayMode;
            set {
                if (_DisplayMode == value) return;
                _DisplayMode = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        ModeEnum _DisplayMode = ModeEnum.Undefined;

        //---------------------------------------------------------------------------------
        public bool DisplayImage
        {
            get => _DisplayImage;
            set
            {
                if (_DisplayImage == value) return;
                _DisplayImage = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        bool _DisplayImage = true;

        //---------------------------------------------------------------------------------
        public bool ImageVisible
        {
            get => _ImageVisible;
            set
            {
                if (_ImageVisible == value) return;
                _ImageVisible = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        bool _ImageVisible = true;

        //---------------------------------------------------------------------------------
        public bool DisplayEmptyText
        {
            get => _DisplayEmptyText;
            set
            {
                if (_DisplayEmptyText == value) return;
                _DisplayEmptyText = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        bool _DisplayEmptyText = true;

        //---------------------------------------------------------------------------------
        public bool DisplayLatin
        {
            get => _DisplayLatin;
            set
            {
                if (_DisplayLatin == value) return;
                _DisplayLatin = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        bool _DisplayLatin = true;

        //---------------------------------------------------------------------------------
        public bool LatinVisible
        {
            get => _LatinVisible;
            set
            {
                if (_LatinVisible == value) return;
                _LatinVisible = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        bool _LatinVisible = true;

        //---------------------------------------------------------------------------------
        public bool DisplayFrench
        {
            get => _DisplayFrench;
            set
            {
                if (_DisplayFrench == value) return;
                _DisplayFrench = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        bool _DisplayFrench = true;

        //---------------------------------------------------------------------------------
        public bool FrenchVisible
        {
            get => _FrenchVisible;
            set
            {
                if (_FrenchVisible == value) return;
                _FrenchVisible = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        bool _FrenchVisible = true;

        //---------------------------------------------------------------------------------
        public int CropUnderThisSize
        {
            get => _CropUnderThisSize;
            set
            {
                if (_CropUnderThisSize == value) return;
                _CropUnderThisSize = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        int _CropUnderThisSize = 200;

        //---------------------------------------------------------------------------------
        public int FitInRectAboveThisSize
        {
            get => _FitInRectAboveThisSize;
            set
            {
                if (_FitInRectAboveThisSize == value) return;
                _FitInRectAboveThisSize = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        int _FitInRectAboveThisSize = 400;

        //---------------------------------------------------------------------------------
        public int BorderSize
        {
            get => _BorderSize;
            set
            {
                if (_BorderSize == value) return;
                _BorderSize = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        int _BorderSize = 0;

        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public Color BorderColor
        {
            get => _BorderColor;
            set
            {
                if (_BorderColor == value) return;
                _BorderColor = Color.FromArgb(255, value);
                BorderBrush = new SolidBrush(_BorderColor);
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        Color _BorderColor;
        [XmlIgnore, Browsable(false)]
        public Brush BorderBrush { get; private set; }
        [XmlElement("BorderColor"), Browsable(false)]
        public string BorderColorXML { get { return ColorTranslator.ToHtml(BorderColor); } set { BorderColor = ColorTranslator.FromHtml(value); } }

        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public Color BorderColorExtinct
        {
            get => _BorderColorExtinct;
            set
            {
                if (_BorderColorExtinct == value) return;
                _BorderColorExtinct = Color.FromArgb(255, value);
                BorderBrushExtinct = new SolidBrush(_BorderColorExtinct);
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        Color _BorderColorExtinct;
        [XmlIgnore, Browsable(false)]
        public Brush BorderBrushExtinct { get; private set; }
        [XmlElement("BorderColorExtinct"), Browsable(false)]
        public string BorderColorExtinctXML { get { return ColorTranslator.ToHtml(BorderColorExtinct); } set { BorderColorExtinct = ColorTranslator.FromHtml(value); } }

        //---------------------------------------------------------------------------------
        public int BottomMargin
        {
            get => _BottomMargin;
            set
            {
                if (_BottomMargin== value) return;
                _BottomMargin = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        int _BottomMargin = 0;

        //---------------------------------------------------------------------------------
        public int LegendFirstLineOffset
        {
            get => _LegendFirstLineOffset;
            set
            {
                if (_LegendFirstLineOffset == value) return;
                _LegendFirstLineOffset = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        int _LegendFirstLineOffset = 0;

        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public Color LegendAliveBackColor
        {
            get => _LegendAliveBackColor;
            set
            {
                if (_LegendAliveBackColor == value) return;
                _LegendAliveBackColor = Color.FromArgb(255, value);
                LegendAliveBackBrush = new SolidBrush(_LegendAliveBackColor);
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        Color _LegendAliveBackColor;
        [XmlIgnore, Browsable(false)]
        public Brush LegendAliveBackBrush { get; private set; }
        [XmlElement("LegendAliveBackColor"), Browsable(false)]
        public string LegendAliveBackColorXML { get { return ColorTranslator.ToHtml(LegendAliveBackColor); } set { LegendAliveBackColor = ColorTranslator.FromHtml(value); } }

        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public Color LegendAliveForeColor
        {
            get => _LegendAliveForeColor;
            set
            {
                if (_LegendAliveForeColor == value) return;
                _LegendAliveForeColor = Color.FromArgb(255,value);
                LegendAliveForeBrush = new SolidBrush(_LegendAliveForeColor);
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        Color _LegendAliveForeColor;
        [XmlIgnore, Browsable(false)]
        public Brush LegendAliveForeBrush { get; private set; }
        [XmlElement("LegendAliveForeColor"), Browsable(false)]
        public string LegendAliveForeColorXML { get { return ColorTranslator.ToHtml(LegendAliveForeColor); } set { LegendAliveForeColor = ColorTranslator.FromHtml(value); } }

        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public Color LegendAliveSecondaryForeColor
        {
            get => _LegendAliveSecondaryForeColor;
            set
            {
                if (_LegendAliveSecondaryForeColor == value) return;
                _LegendAliveSecondaryForeColor = Color.FromArgb(255, value);
                LegendAliveSecondaryForeBrush = new SolidBrush(_LegendAliveSecondaryForeColor);
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        Color _LegendAliveSecondaryForeColor;
        [XmlIgnore, Browsable(false)]
        public Brush LegendAliveSecondaryForeBrush { get; private set; }
        [XmlElement("LegendAliveSecondaryForeColor"), Browsable(false)]
        public string LegendAliveSecondaryForeColorXML { get { return ColorTranslator.ToHtml(LegendAliveSecondaryForeColor); } set { LegendAliveSecondaryForeColor = ColorTranslator.FromHtml(value); } }
        
        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public Color LegendExtinctBackColor
        {
            get => _LegendExtinctBackColor;
            set
            {
                if (_LegendExtinctBackColor == value) return;
                _LegendExtinctBackColor = Color.FromArgb(255, value);
                LegendExtinctBackBrush = new SolidBrush(_LegendExtinctBackColor);
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        Color _LegendExtinctBackColor;
        [XmlIgnore, Browsable(false)]
        public Brush LegendExtinctBackBrush { get; private set; }
        [XmlElement("LegendExtinctBackColor"), Browsable(false)]
        public string LegendExtinctBackColorXML { get { return ColorTranslator.ToHtml(LegendExtinctBackColor); } set { LegendExtinctBackColor = ColorTranslator.FromHtml(value); } }

        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public Color LegendExtinctForeColor
        {
            get => _LegendExtinctForeColor;
            set
            {
                if (_LegendExtinctForeColor == value) return;
                _LegendExtinctForeColor = Color.FromArgb(255, value);
                LegendExtinctForeBrush = new SolidBrush(_LegendExtinctForeColor);
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        Color _LegendExtinctForeColor;
        [XmlIgnore, Browsable(false)]
        public Brush LegendExtinctForeBrush { get; private set; }
        [XmlElement("LegendExtinctForeColor"), Browsable(false)]
        public string LegendExtinctForeColorXML { get { return ColorTranslator.ToHtml(LegendExtinctForeColor); } set { LegendExtinctForeColor = ColorTranslator.FromHtml(value); } }

        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public Color LegendExtinctSecondaryForeColor
        {
            get => _LegendExtinctSecondaryForeColor;
            set
            {
                if (_LegendExtinctSecondaryForeColor == value) return;
                _LegendExtinctSecondaryForeColor = Color.FromArgb(255, value);
                LegendExtinctSecondaryForeBrush = new SolidBrush(_LegendExtinctSecondaryForeColor);
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        Color _LegendExtinctSecondaryForeColor;
        [XmlIgnore, Browsable(false)]
        public Brush LegendExtinctSecondaryForeBrush { get; private set; }
        [XmlElement("LegendExtinctSecondaryForeColor"), Browsable(false)]
        public string LegendExtinctSecondaryForeColorXML { get { return ColorTranslator.ToHtml(LegendExtinctSecondaryForeColor); } set { LegendExtinctSecondaryForeColor = ColorTranslator.FromHtml(value); } }

        //---------------------------------------------------------------------------------
        public void GetLegendBrushes( bool _extinct, out Brush _background, out Brush _forecolor, out Brush _secondaryForecolor )
        {
            if (_extinct)
            {
                _background = LegendExtinctBackBrush;
                _forecolor = LegendExtinctForeBrush;
                _secondaryForecolor = LegendExtinctSecondaryForeBrush ?? LegendExtinctForeBrush;
            }
            else
            {
                _background = LegendAliveBackBrush;
                _forecolor = LegendAliveForeBrush;
                _secondaryForecolor = LegendAliveSecondaryForeBrush ?? LegendAliveForeBrush;
            }
        }

        public Brush GetLegendBackBrush(bool _extinct)
        {
            return _extinct? LegendExtinctBackBrush : LegendAliveBackBrush;
        }

        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public Color ImageBackColor
        {
            get => _ImageBackColor;
            set
            {
                if (_ImageBackColor == value) return;
                _ImageBackColor = Color.FromArgb(255, value);
                ImageBackBrush = new SolidBrush(_ImageBackColor);
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        Color _ImageBackColor;
        [XmlIgnore, Browsable(false)]
        public Brush ImageBackBrush { get; private set; }
        [XmlElement("ImageBackColor"), Browsable(false)]
        public string ImageBackColorXML { get { return ColorTranslator.ToHtml(ImageBackColor); } set { ImageBackColor = ColorTranslator.FromHtml(value); } }

        //---------------------------------------------------------------------------------
        int _ImageBorderSize = 0;
        public int ImageBorderSize
        {
            get => _ImageBorderSize;
            set
            {
                if (_ImageBorderSize == value) return;
                _ImageBorderSize = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //---------------------------------------------------------------------------------
        [XmlIgnore, Browsable(false)]
        public PlayerSmallDisplayParams SoundPlayerDisplayParams { get; set; }

        //---------------------------------------------------------------------------------
        public bool DisplayRedListCategory
        {
            get => _DisplayRedListCategory;
            set
            {
                if (_DisplayRedListCategory == value) return;
                _DisplayRedListCategory = value;
                OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        bool _DisplayRedListCategory = false;


        //=================================================================================
        // Version Management
        //

        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public static int CurrentVersion = 1;

        //---------------------------------------------------------------------------------
        public void AfterLoad()
        {
            if (Version == 0)
            {
                LegendAliveSecondaryForeColor = LegendAliveForeColor;
                LegendExtinctSecondaryForeColor = LegendExtinctForeColor;
                Version++;
            }
            Version = 1;
        }
    }
}
