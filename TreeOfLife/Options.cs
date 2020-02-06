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
    public class Options
    {
        //=========================================================================================
        // Update functions
        //
        public void AfterLoad()
        {
            if (EditTreeColor == null)
                EditTreeColor = GraphColors.EditTreeDefault();
            EditTreeColor.Check(GraphColors.EditTreeDefault());

            if (FullTreeColor == null)
                FullTreeColor = GraphColors.FullTreeDefault();
            FullTreeColor.Check(GraphColors.FullTreeDefault());

            if (PartialTreeColor == null)
                PartialTreeColor = GraphColors.PartialTreeDefault();
            PartialTreeColor.Check(GraphColors.PartialTreeDefault());

            if (NavigatorTreeColor == null)
                NavigatorTreeColor = GraphColors.FullTreeDefault();
            NavigatorTreeColor.Check(GraphColors.FullTreeDefault());

            if (NavigatorSelectedRectangleBrush == null)
                NavigatorSelectedRectangleColor = Color.FromArgb(100, Color.Green);

            OneTaxonAllImagesVignetteDisplayParams.AfterLoad();
            OneTaxonOneImageVignetteDisplayParams.AfterLoad();
            TitleVignetteDisplayParams.AfterLoad();
        }

        //=========================================================================================
        // General options
        //
        [Category("General"), DisplayName("Language"), Description("Interface language, change it in Display menu")]
        public string GuiLanguage
        {
            get { return Localization.Manager.CurrentLanguage; }
            set { Localization.Manager.CurrentLanguage = value; }
        }

        [Category("General"), DisplayName("Localization Debug Mode"), Description("prefix all localized string with a mark")]
        public bool GuiLanguageDebugMode
        {
            get { return Localization.Manager.DebugMode; }
            set { Localization.Manager.DebugMode= value; }
        }

        //=========================================================================================
        // navigator options
        //

        [Category("Navigator"), DisplayName("Colors for tree"), Description("Color used to draw graph of navigato")]
        public GraphColors NavigatorTreeColor { get; set; } = null;

        [XmlIgnore, Browsable(false)]
        public System.Drawing.Brush NavigatorSelectedRectangleBrush = null;
        private System.Drawing.Color _NavigatorSelectedRectangleColor;
        [Category("Navigator"), DisplayName("Selected rectangle color"), Description("Color of rectangle that hilight the selected taxon and its descendance")]
        [XmlIgnore]
        public System.Drawing.Color NavigatorSelectedRectangleColor
        {
            get { return _NavigatorSelectedRectangleColor; }
            set
            {
                _NavigatorSelectedRectangleColor = value;
                if (NavigatorSelectedRectangleBrush != null) NavigatorSelectedRectangleBrush.Dispose();
                NavigatorSelectedRectangleBrush = new SolidBrush(_NavigatorSelectedRectangleColor);
            }
        }
        [XmlElement("NavigatorSelectedRectangleColor")]
        [Browsable(false)]
        public string NavigatorSelectedRectangleColor_XML
        {
            get { return String.Format("{0};{1};{2};{3}", NavigatorSelectedRectangleColor.A, NavigatorSelectedRectangleColor.R, NavigatorSelectedRectangleColor.G, NavigatorSelectedRectangleColor.B); }
            set
            {
                string[] parts = value.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 4) return;
                int[] comp = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    if (!int.TryParse(parts[i], out comp[i])) return;
                    comp[i] = Math.Min(Math.Max(comp[i], 0), 255);
                }
                NavigatorSelectedRectangleColor = Color.FromArgb(comp[0], comp[1], comp[2], comp[3]);
            }
        }

        [Category("Navigator"), DisplayName("Show taxon names"), Description("Show some names in navigator view")]
        public bool NavigatorShowTaxonNames
        {
            get => _NavigatorShowTaxonNames;
            set
            {
                if (_NavigatorShowTaxonNames == value) return;
                _NavigatorShowTaxonNames = value;
                TaxonControlList.OnRefreshGraph();
            }
        }
        private bool _NavigatorShowTaxonNames = false;

        [Category("Navigator"), DisplayName("Min height to show names"), Description("When taxon rect height is less than option value, name is not displayed")]
        public int NavigatorShowTaxonNamesMinHeight
        {
            get => _NavigatorShowTaxonNamesMinHeight; 
            set
            {
                if (_NavigatorShowTaxonNamesMinHeight == value) return;
                _NavigatorShowTaxonNamesMinHeight = value;
                TaxonControlList.OnRefreshGraph();
            }
        }
        private int _NavigatorShowTaxonNamesMinHeight = 30;

        [Category("Navigator"), DisplayName("Min width to show names"), Description("When taxon rect width is less than option value, name is not displayed")]
        public int NavigatorShowTaxonNamesMinWidth
        {
            get => _NavigatorShowTaxonNamesMinWidth; 
            set
            {
                if (_NavigatorShowTaxonNamesMinWidth == value) return;
                _NavigatorShowTaxonNamesMinWidth = value;
                TaxonControlList.OnRefreshGraph();
            }
        }
        private int _NavigatorShowTaxonNamesMinWidth = 0;

        [Category("Navigator"), DisplayName("Max width of names"), Description("When taxon name is displayed, max width of displayed name")]
        public int NavigatorShowTaxonNamesMaxNameWidth
        {
            get =>_NavigatorShowTaxonNamesMaxTextWidth; 
            set
            {
                if (_NavigatorShowTaxonNamesMaxTextWidth == value) return;
                _NavigatorShowTaxonNamesMaxTextWidth = value;
                TaxonControlList.OnRefreshGraph();
            }
        }
        private int _NavigatorShowTaxonNamesMaxTextWidth = 100;

        [Category("Navigator"), DisplayName("Left margin"), Description("Add a margin to the left")]
        public int NavigatorLeftMargin
        {
            get =>_NavigatorLeftMargin; 
            set
            {
                if (_NavigatorLeftMargin == value) return;
                _NavigatorLeftMargin = value;
                TaxonControlList.OnRefreshGraph();
            }
        }
        private int _NavigatorLeftMargin = 0;

        //=========================================================================================
        // Image options
        //

        [Category("Image"), DisplayName("Legend minimum width"), Description("Minimum width of legend, below, legend will not be displayed")]
        public int LegendMinWidth
        {
            get { return TreeOfLife.Controls.TaxonImageControl.LegendMinWidth; }
            set { TreeOfLife.Controls.TaxonImageControl.LegendMinWidth = value; }
        }

        [Category("Image"), DisplayName("Legend height"), Description("Height of legend, final height may be change to fit with Max legend height ratio")]
        public int LegendHeight
        {
            get { return TreeOfLife.Controls.TaxonImageControl.LegendHeight; }
            set { TreeOfLife.Controls.TaxonImageControl.LegendHeight = value; }
        }

        [Category("Image"), DisplayName("Max legend height ratio"), Description("To limit legend height when image height is too small, final legend height will be minimized to Image height multiply by value")]
        public float MaxLegendHeightRatio
        {
            get { return TreeOfLife.Controls.TaxonImageControl.MaxLegendHeightRatio; }
            set { TreeOfLife.Controls.TaxonImageControl.MaxLegendHeightRatio = value; }
        }

        [Category("Image"), DisplayName("Min font size"), Description("Minimum font size for legend display")]
        public int MinFontSize
        {
            get { return TreeOfLife.Controls.TaxonImageControl.MinFontSize; }
            set { TreeOfLife.Controls.TaxonImageControl.MinFontSize = value; }
        }

        [Category("Image"), DisplayName("Max font size"), Description("Maximum font size for legend display")]
        public int MaxFontSize
        {
            get { return TreeOfLife.Controls.TaxonImageControl.MaxFontSize; }
            set { TreeOfLife.Controls.TaxonImageControl.MaxFontSize = value; }
        }

        [Category("Image"), DisplayName("Width over font size"), Description("Ratio used to determine initial font size. Font size = Width / value")]
        public float WidthOverFontSize
        {
            get { return TreeOfLife.Controls.TaxonImageControl.WidthOverFontSize; }
            set { TreeOfLife.Controls.TaxonImageControl.WidthOverFontSize = value; }
        }

        //=========================================================================================
        // Graph
        //

        [Category("Graph"), DisplayName("Graph options"), Description("Some graph options gathered together")]
        public GraphOptions GraphOptions
        {
            get => TaxonUtils.MainGraph?.Graph.Options;
            set
            {
                if (TaxonUtils.MainGraph != null)
                    TaxonUtils.MainGraph.Graph.Options = value;
                TaxonControlList.OnOptionChanged();
            }
        }

        /*private bool _UsePartialTreeColor = false;
        [Category("Graph"), DisplayName("Use partial tree colors"), Description("Use partial tree colors for full tree graph")]
        public bool UsePartialTreeColor
        {
            get => _UsePartialTreeColor;
            set 
            {
                if (_UsePartialTreeColor == value) return;
                _UsePartialTreeColor = value;
                TaxonControlList.OnOptionChanged();
            }
        }*/

        [Category("Graph"), DisplayName("Colors for full tree"), Description("Color used to draw graph of full taxon tree")]
        public GraphColors FullTreeColor { get; set; } = null;

        [Category("Graph"), DisplayName("Colors for partial tree"), Description("Color used to draw graph of partial taxon tree")]
        public GraphColors PartialTreeColor { get; set; } = null;

        [Category("Graph"), DisplayName("Colors for tree in special modes"), Description("Color used to draw graph in miscelleanous special mode")]
        public GraphColors EditTreeColor { get; set; } = null;

        [Category("Graph"), DisplayName("Inertia movement"), Description("Activate inertia when moving graph")]
        public bool InertiaActive { get; set; } = true;

        [Category("Graph"), DisplayName("Inertia debug"), Description("Display debug informaton about inertia")]
        public bool InertiaDebug { get; set; } = false;

        //=========================================================================================
        // Finder options
        //

        [Category("Finder"), DisplayName("Latin"), Description("Search string in latin names")]
        public bool LangageLatin { get; set; } = true;
        
        [Category("Finder"), DisplayName("French"), Description("Search string in french names")]
        public bool LangageFrench { get; set; } = true;

        //=========================================================================================
        // Tag options
        //

        [Browsable(false)]
        public string TagBatchImportSourceFolder { get; set; } = "";

        [Browsable(false)]
        public string TagBatchImportDestinationFolder { get; set; } = "";

        [Browsable(false)]
        public string TagBatchImportOverwrite { get; set; } = "";

        //=========================================================================================
        // Infos options
        //

        [Category("Infos"), DisplayName("Number of image"), Description("Set number of images displayed in multi image control.")]
        public int NumberInMultiImageControl
        {
            get { return TaxonInfo.NumberInMultiImageControl; }
            set { TaxonInfo.NumberInMultiImageControl = value; }
        }

        [Category("Infos"), DisplayName("Display mode for others"), Description("Display mode for image of taxon other than species")]
        public TaxonInfo.DisplayModeForOthersEnum DisplayModeForOthers
        {
            get { return TaxonInfo.DisplayModeForOthers; }
            set { TaxonInfo.DisplayModeForOthers = value; }
        }

        [Category("Infos"), DisplayName("Vignette with all images"), Description("Display mode for vignette used for taxon with all images")]
        public VignetteDisplayParams OneTaxonAllImagesVignetteDisplayParams { get; set; } = new VignetteDisplayParams("Options");

        [Category("Infos"), DisplayName("Vignette for one image"), Description("Display mode for vignette used for one image of taxon")]
        public VignetteDisplayParams OneTaxonOneImageVignetteDisplayParams { get; set; } = new VignetteDisplayParams("Options");

        [Category("Infos"), DisplayName("Vignette for title"), Description("Display mode for vignette used for title")]
        public VignetteDisplayParams TitleVignetteDisplayParams { get; set; } = new VignetteDisplayParams("Options");

        [Category("Infos"), DisplayName("Grid line thickness"), Description("Thickness of line between to image in list")]
        public int GridLineThickness { get; set; } = 0;


        //=========================================================================================
        // Comment options
        //
        [Category("Comments"), DisplayName("Show empty"), Description("Show taxon without comment.")]
        public bool ShowEmpty
        {
            get { return TaxonComments.ShowEmpty; }
            set { TaxonComments.ShowEmpty = value; }
        }

        [Category("Comments"), DisplayName("Watch changes"), Description("Reload comment when file changes.")]
        public bool WatchChanges
        {
            get { return TaxonComments.WatchComment; }
            set { TaxonComments.WatchComment = value; }
        }
        
    }
}
