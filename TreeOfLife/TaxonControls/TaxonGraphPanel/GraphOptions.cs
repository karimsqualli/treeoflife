using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TreeOfLife.Controls;

namespace TreeOfLife
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GraphOptions
    {
        [DisplayName("Display classik rank rule"), Description("Set to true to activate classik rank display mode"), Browsable(false)]
        public bool DisplayClassikRankRule { get; set; } = false;

        public enum DisplayModeEnum
        {
            Grid,
            Lines,
            //Circle
        }

        private DisplayModeEnum _DisplayMode = DisplayModeEnum.Lines;
        [DisplayName("Display mode"), Description("Choose mode to draw graph")]
        public DisplayModeEnum DisplayMode
        {
            get => _DisplayMode;
            set
            {
                _DisplayMode = value;
                TaxonControlList.OnRefreshGraph();
            }
        }

        private bool _DisplayBackTransparentInLineMode = false;
        [DisplayName("Display back transparent in line mode"), Description("Set to true to display full taxon in backgroun when in line mode")]
        public bool DisplayBackTransparentInLineMode
        {
            get => _DisplayBackTransparentInLineMode;
            set
            {
                _DisplayBackTransparentInLineMode = value;
                TaxonControlList.OnRefreshGraph();
            }
        }

        private int _ColumnInter = 30;
        [DisplayName("Interval between columns"), Description("Width between to columns to display lines")]
        public int ColumnInter
        {
            get => _ColumnInter;
            set
            {
                _ColumnInter = value;
                UpdateZoomedValues();
                TaxonControlList.OnRefreshGraph();
            }
        }

        private float _LineSoftRatio = 0.6f;
        [DisplayName("Rounded line ratio"), Description("Ratio of column used for rounded line")]
        public float LineSoftRatio
        {
            get => _LineSoftRatio;
            set
            {
                _LineSoftRatio = Math.Max(0, Math.Min(1, value));
                TaxonControlList.OnRefreshGraph();
            }
        }

        private int _TaxonWidth = 100;
        [DisplayName("Taxon width"), Description("Width of a single taxon")]
        public int TaxonWidth
        {
            get => _TaxonWidth;
            set
            {
                _TaxonWidth = value;
                UpdateZoomedValues();
                TaxonControlList.OnRefreshGraph();
            }
        }

        private int _TaxonHeight = 30;
        [DisplayName("Taxon height"), Description("Height of a single taxon")]
        public int TaxonHeight
        {
            get => _TaxonHeight;
            set
            {
                _TaxonHeight = value;
                UpdateZoomedValues();
                TaxonControlList.OnRefreshGraph();
            }
        }

        private bool _KeepsImageAtNamesRight = false;
        [DisplayName("Keeps taxon image at name's right"), Description("When image is displayed, draw it just at right of taxon's name")]
        public bool KeepsImageAtNamesRight
        {
            get => _KeepsImageAtNamesRight;
            set
            {
                _KeepsImageAtNamesRight = value;
                TaxonControlList.OnRefreshGraph();
            }
        }

        private bool _GrayUnselectedInBoxMode = false;
        [DisplayName("Gray unselected boxes "), Description("When in box mode, gray the ones who are not selected and without selected parents")]
        public bool GrayUnselectedInBoxMode
        {
            get => _GrayUnselectedInBoxMode;
            set
            {
                _GrayUnselectedInBoxMode = value;
                TaxonControlList.OnRefreshGraph();
            }
        }

        private float _Zoom = 1.0f;
        [DisplayName("Graph zoom"), Description("Zoom that affect graph display")]
        public float Zoom
        {
            get => _Zoom;
            set
            {
                float min = Math.Max(20.0f / (float) _TaxonWidth, 10.0f / (float) _TaxonHeight);
                _Zoom = Math.Max(min, Math.Min(value, 10));
                UpdateZoomedValues();
                TaxonControlList.OnRefreshGraph();
            }
        }

        //===== Computed Value
        void UpdateZoomedValues()
        {
            ZoomedWidth = (int) (_Zoom * _TaxonWidth);
            ZoomedHeight = (int) (_Zoom * _TaxonHeight);
            ZoomedColumnWidth = (int)(_Zoom * _ColumnInter);
        }
        [XmlIgnore, Browsable(false)]
        public int ZoomedWidth { get; set; }  = 100;
        [XmlIgnore, Browsable(false)]
        public int ZoomedHeight { get; set; } = 30;
        [XmlIgnore, Browsable(false)]
        public int ZoomedColumnWidth { get; set; } = 30;

    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GraphColors : IDisposable
    {
        // ----- background color
        private Color _BackColor;
        [XmlIgnore, DisplayName("Background color"), Description("Color of graph background")]
        public Color BackColor
        {
            get => _BackColor;
            set
            {
                if (_BackColor == value) return;
                _BackColor = value;
                TaxonControlList.OnOptionChanged();
            }
        }
        [XmlElement("BackColor"), Browsable(false)]
        public string BackColor_XML { get { return ColorTranslator.ToHtml(BackColor); } set { BackColor = ColorTranslator.FromHtml(value); } }
        
        // ----- expanded taxon color
        private Color _Expanded;
        [XmlIgnore, Browsable(false)]
        public Brush ExpandedBrush;
        [XmlIgnore, DisplayName("Expanded color"), Description("Color of expanded taxon")]
        public Color Expanded
        {
            get => _Expanded;
            set
            {
                if (_Expanded == value) return;
                _Expanded = value;
                ExpandedBrush = new SolidBrush(_Expanded);
                TaxonControlList.OnRefreshGraph();
            }
        }
        [XmlElement("Expanded"), Browsable(false)]
        public string Expanded_XML { get { return ColorTranslator.ToHtml(Expanded); } set { Expanded = ColorTranslator.FromHtml(value); } }

        // ----- collapsed taxon color
        private Color _Collapsed;
        [XmlIgnore, Browsable(false)]
        public Brush CollapsedBrush;
        [XmlIgnore, DisplayName("Collapsed color"), Description("Color of collapsed taxon")]
        public Color Collapsed
        {
            get => _Collapsed; 
            set
            {
                if (_Collapsed == value) return;
                _Collapsed = value;
                CollapsedBrush = new SolidBrush(_Collapsed);
                TaxonControlList.OnRefreshGraph();
            }
        }
        [XmlElement("Collapsed"), Browsable(false)]
        public string Collapsed_XML { get { return ColorTranslator.ToHtml(Collapsed); } set { Collapsed = ColorTranslator.FromHtml(value); } }

        // ----- expanded but extinct taxon color
        private Color _ExtinctExpanded;
        [XmlIgnore, Browsable(false)]
        public Brush ExtinctExpandedBrush;
        [XmlIgnore, DisplayName("ExtinctExpanded color"), Description("Color of expanded taxon when this taxon is extinct.")]
        public Color ExtinctExpanded
        {
            get => _ExtinctExpanded;
            set 
            {
                if (_ExtinctExpanded == value) return;
                _ExtinctExpanded = value;
                ExtinctExpandedBrush = new SolidBrush(_ExtinctExpanded);
                TaxonControlList.OnRefreshGraph();
            }
        }
        [XmlElement("ExtinctExpanded"), Browsable(false)]
        public string ExtinctExpanded_XML { get { return ColorTranslator.ToHtml(ExtinctExpanded); } set { ExtinctExpanded = ColorTranslator.FromHtml(value); } }

        // ----- expanded but extinct taxon color
        private Color _ExtinctCollapsed;
        [XmlIgnore, Browsable(false)]
        public Brush ExtinctCollapsedBrush;
        [XmlIgnore, DisplayName("ExtinctCollapsed color"), Description("Color of collpased taxon when this taxon is extinct.")]
        public Color ExtinctCollapsed
        {
            get => _ExtinctCollapsed; 
            set
            {
                if (_ExtinctCollapsed == value) return;
                _ExtinctCollapsed = value;
                ExtinctCollapsedBrush = new SolidBrush(_ExtinctCollapsed);
                TaxonControlList.OnRefreshGraph();
            }
        }
        [XmlElement("ExtinctCollapsed"), Browsable(false)]
        public string ExtinctCollapsed_XML { get { return ColorTranslator.ToHtml(ExtinctCollapsed); } set { ExtinctCollapsed = ColorTranslator.FromHtml(value); } }

        // ----- color of select taxon
        private Color _Selected;
        [XmlIgnore, Browsable(false)]
        public Brush SelectedBrush;
        [XmlIgnore, DisplayName("Selected color"), Description("Color of selected taxon")]
        public Color Selected
        {
            get => _Selected; 
            set
            {
                if (_Selected == value) return;
                _Selected = value;
                SelectedBrush = new SolidBrush(_Selected);
                TaxonControlList.OnRefreshGraph();
            }
        }
        [XmlElement("Selected"), Browsable(false)]
        public string Selected_XML { get { return ColorTranslator.ToHtml(Selected); } set { Selected = ColorTranslator.FromHtml(value); } }

        // ----- Color of taxon hovered by mouse cursor
        private Color _Hover;
        [XmlIgnore, Browsable(false)]
        public Brush HoverBrush;
        [XmlIgnore, DisplayName("Hover color"), Description("Color of taxon hovered by mouse cursor")]
        public Color Hover
        {
            get => _Hover;
            set
            {
                if (_Hover == value) return;
                _Hover = value;
                HoverBrush = new SolidBrush(_Hover);
                TaxonControlList.OnRefreshGraph();
            }
        }
        [XmlElement("Hover"), Browsable(false)]
        public string Hover_XML { get { return ColorTranslator.ToHtml(Hover); } set { Hover = ColorTranslator.FromHtml(value); } }

        // ----- Color of hilighted 
        private Color _Highlighted;
        [XmlIgnore, Browsable(false)]
        public Brush HighlightedBrush;
        [XmlIgnore, DisplayName("Highlighted color"), Description("Color of hilighted taxon")]
        public Color Highlighted
        {
            get => _Highlighted; 
            set 
            {
                if (_Highlighted == value) return;
                _Highlighted = value;
                HighlightedBrush = new SolidBrush(_Highlighted);
                TaxonControlList.OnRefreshGraph();
            }
        }
        [XmlElement("Highlighted"), Browsable(false)]
        public string Highlighted_XML { get { return ColorTranslator.ToHtml(Highlighted); } set { Highlighted = ColorTranslator.FromHtml(value); } }

        // ---- color of links
        private Color _Links;
        [XmlIgnore, Browsable(false)]
        public Pen LinksPen;
        [XmlIgnore, DisplayName("Links color"), Description("Color of links")]
        public Color Links
        {
            get => _Links;
            set
            {
                if (_Links == value) return;
                _Links = value;
                LinksPen = new Pen(_Links,3);
                TaxonControlList.OnRefreshGraph();
            }
        }
        [XmlElement("Links"), Browsable(false)]
        public string Links_XML { get { return ColorTranslator.ToHtml(Links); } set { Links = ColorTranslator.FromHtml(value); } }

        // ---- color of selected links
        private Color _SelectedLinks;
        [XmlIgnore, Browsable(false)]
        public Pen SelectedLinksPen;
        [XmlIgnore, DisplayName("Selected Links color"), Description("Color of selected links")]
        public Color SelectedLinks
        {
            get => _SelectedLinks;
            set
            {
                if (_SelectedLinks == value) return;
                _SelectedLinks = value;
                SelectedLinksPen = new Pen(_SelectedLinks,3);
                TaxonControlList.OnRefreshGraph();
            }
        }
        [XmlElement("SelectedLinks"), Browsable(false)]
        public string SelectedLinks_XML { get { return ColorTranslator.ToHtml(SelectedLinks); } set { SelectedLinks = ColorTranslator.FromHtml(value); } }

        public void Check( GraphColors _model )
        {
            if (BackColor == null) { BackColor = _model.BackColor; }
            if (ExpandedBrush == null) { Expanded = _model.Expanded; }
            if (CollapsedBrush == null) { Collapsed = _model.Collapsed; }
            if (ExtinctExpandedBrush == null) { ExtinctExpanded = _model.ExtinctExpanded; }
            if (ExtinctCollapsedBrush == null) { ExtinctCollapsed = _model.ExtinctCollapsed; }
            if (SelectedBrush == null) { Selected = _model.Selected; }
            if (HoverBrush == null) { Hover = _model.Hover; }
            if (HighlightedBrush == null) { Highlighted = _model.Highlighted; }
            if (LinksPen == null) { Links = _model.Links; }
            if (SelectedLinksPen == null) { SelectedLinks = _model.SelectedLinks; }
        }

        static public GraphColors FullTreeDefault()
        {
            GraphColors result = new GraphColors
            {
                BackColor = Color.FromArgb(84, 97, 111),
                Expanded = Color.FromArgb(41, 158, 162),
                Collapsed = Color.FromArgb(110, 195, 198),
                ExtinctExpanded = Color.FromArgb(34, 43, 44),
                ExtinctCollapsed = Color.FromArgb(76, 93, 94),
                Selected = Color.FromArgb(139, 216, 146),
                Hover = Color.FromArgb(92, 188, 154),
                Highlighted = Color.FromArgb(252, 251, 171),
                Links = Color.FromArgb(63, 67, 81),
                SelectedLinks = Color.FromArgb(197, 202, 218)
            };
            return result;
        }

        static public GraphColors PartialTreeDefault()
        {
            GraphColors result = new GraphColors
            {
                BackColor = Color.FromArgb(87, 99, 93),
                Expanded = Color.FromArgb(88, 184, 145),
                Collapsed = Color.FromArgb(162, 207, 183),
                ExtinctExpanded = Color.FromArgb(8, 26, 19),
                ExtinctCollapsed = Color.FromArgb(76, 93, 94),
                Selected = Color.FromArgb(204, 251, 89),
                Hover = Color.FromArgb(154, 212, 111),
                Highlighted = Color.FromArgb(252, 251, 171),
                Links = Color.FromArgb(63, 67, 81),
                SelectedLinks = Color.FromArgb(197, 202, 218)
            };
            return result;
        }

        static public GraphColors EditTreeDefault()
        {
            GraphColors result = new GraphColors
            {
                BackColor = Color.FromArgb(42, 46, 55),
                Expanded = Color.FromArgb(20, 78, 81),
                Collapsed = Color.FromArgb(55, 100, 100),
                ExtinctExpanded = Color.FromArgb(34, 43, 44),
                ExtinctCollapsed = Color.FromArgb(76, 93, 94),
                Selected = Color.FromArgb(139, 216, 146),
                Hover = Color.FromArgb(92, 188, 154),
                Highlighted = Color.FromArgb(252, 251, 171),
                Links = Color.FromArgb(63, 67, 81),
                SelectedLinks = Color.FromArgb(197, 202, 218)
            };
            return result;
        }

        public void Dispose()
        {
            if (ExpandedBrush != null) { ExpandedBrush.Dispose(); ExpandedBrush = null; }
            if (CollapsedBrush != null) { CollapsedBrush.Dispose(); CollapsedBrush = null; }
            if (ExtinctExpandedBrush != null) { ExtinctExpandedBrush.Dispose(); ExtinctExpandedBrush = null; }
            if (ExtinctCollapsedBrush != null) { ExtinctCollapsedBrush.Dispose(); ExtinctCollapsedBrush= null; }
            if (SelectedBrush != null) { SelectedBrush.Dispose(); SelectedBrush = null; }
            if (HoverBrush != null) { HoverBrush.Dispose(); HoverBrush = null; }
            if (HighlightedBrush != null) { HighlightedBrush.Dispose(); HighlightedBrush = null; }
            if (LinksPen != null) { LinksPen.Dispose(); LinksPen = null; }
            if (SelectedLinksPen != null) { SelectedLinksPen.Dispose(); SelectedLinksPen = null; }
        }
    }
}
