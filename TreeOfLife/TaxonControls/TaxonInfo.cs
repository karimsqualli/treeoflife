using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TreeOfLife.Controls;
using TreeOfLife.GUI;

namespace TreeOfLife
{
    [Description("Display information about selected taxon")]
    [DisplayName("Infos")]
    [Controls.IconAttribute("TaxonInfo")]
    public partial class TaxonInfo : Controls.TaxonControl
    {
        //---------------------------------------------------------------------------------
        public TaxonInfo()
        {
            InitializeComponent();
            UpdateMultiImagesNumber();
            UpdateCheckBoxesImages();
            
            Pictures_Clear();
            Comment_Clear();

            Comments.AddTitleForEmpty = TaxonComments.ShowEmpty;
            TaxonComments.OnShowEmptyChanged += TaxonComments_OnShowEmptyChanged;
            CheckAndLinkOptions(TaxonUtils.MyConfig.Options);
        }

        //---------------------------------------------------------------------------------
        ~TaxonInfo()
        {
            TaxonComments.OnShowEmptyChanged -= TaxonComments_OnShowEmptyChanged;
            UnlinkOptions(TaxonUtils.MyConfig.Options);
           
        }

        //---------------------------------------------------------------------------------
        public void CheckAndLinkOptions( Options _opt )
        {
            VignetteDisplayParams dp = _opt.OneTaxonAllImagesVignetteDisplayParams;
            if (dp.DisplayMode == VignetteDisplayParams.ModeEnum.Undefined )
            {
                dp.DisplayMode = VignetteDisplayParams.ModeEnum.LegendBorder;
                dp.InitColors();
            }
            

            dp = _opt.OneTaxonOneImageVignetteDisplayParams;
            if (dp.DisplayMode == VignetteDisplayParams.ModeEnum.Undefined)
            {
                dp.DisplayMode = VignetteDisplayParams.ModeEnum.Brut;
                dp.InitColors();
            }

            dp = _opt.TitleVignetteDisplayParams;
            if (dp.DisplayMode == VignetteDisplayParams.ModeEnum.Undefined)
            {
                dp.DisplayMode = VignetteDisplayParams.ModeEnum.LegendBorder;
                dp.InitColors();
                dp.DisplayImage = false;
                //dp.LegendAliveBackColor = Color.LightSlateGray;
                dp.LegendAliveBackColor = Theme.Current.Control_Background;
                //dp.LegendAliveForeColor = Color.White;
                dp.LegendAliveForeColor = Theme.Current.Control_Forecolor;
                //dp.LegendAliveSecondaryForeColor = Color.White;
                dp.LegendAliveSecondaryForeColor = Theme.Current.Control_Forecolor;
            }
            
            TaxonBigVignette.DisplayParams = TaxonUtils.MyConfig.Options.TitleVignetteDisplayParams;
            TaxonBigVignette.DisplayParams.SoundPlayerDisplayParams = new VinceSoundPlayer.PlayerSmallDisplayParams() {
                ColorDisabled = 0.7f,
                ColorEnabled = 0.9f,
                ColorHovered = 1
            };

            _opt.OneTaxonAllImagesVignetteDisplayParams.OnChanged += InvalidateMulti;
            _opt.OneTaxonOneImageVignetteDisplayParams.OnChanged += InvalidateMulti;
            _opt.TitleVignetteDisplayParams.OnChanged += InvalidateTitle;
        }

        public void UnlinkOptions( Options _opt )
        {
            _opt.OneTaxonAllImagesVignetteDisplayParams.OnChanged -= InvalidateMulti;
            _opt.OneTaxonOneImageVignetteDisplayParams.OnChanged -= InvalidateMulti;
            _opt.TitleVignetteDisplayParams.OnChanged -= InvalidateTitle;
        }

        void InvalidateTitle(object sender, EventArgs e) { TaxonBigVignette.Invalidate(); }

        void InvalidateMulti(object sender, EventArgs e) { _MultiImages.Invalidate(); }


        //---------------------------------------------------------------------------------
        protected override void ApplyTheme()
        {
            base.ApplyTheme();
            splitContainer1.Panel1.BackColor = Theme.Current.Control_Background;
            SpecificCharactersTitle.BackColor = Theme.Current.Control_Background;
            SpecificCharactersTitle.ForeColor = Theme.Current.Control_Forecolor;
            splitContainerSpecificCharacter.BackColor = Theme.Current.Control_Background;
            buttonExpand.BackColor = Theme.Current.Control_Background;
            buttonCollapse.BackColor = Theme.Current.Control_Background;
            DescendantCount.BackColor = Theme.Current.Control_Background;
            DescendantCount.ForeColor = Theme.Current.Control_Forecolor;
        }

        private void splitContainer1_Paint(object sender, PaintEventArgs e)
        {
            Theme.Current.Splitter_PaintThin(e);
        }

        //---------------------------------------------------------------------------------
        void TaxonComments_OnShowEmptyChanged(object sender, EventArgs e)
        {
            Comments.AddTitleForEmpty = TaxonComments.ShowEmpty;
            OnReselectTaxon(_CurrentTaxon);
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Infos"; }

        //---------------------------------------------------------------------------------
        TaxonTreeNode _CurrentTaxon = null;

        public override void OnSelectTaxon(TaxonTreeNode _taxon)
        {
            OnSelectTaxon(_taxon, false, false);
        }

        void OnSelectTaxon(TaxonTreeNode _taxon, bool _keepOldImages, bool _keepComment )
        {
            Control focusedControl = VinceToolbox.winFunctions.GetFocusedControl();

            TaxonTreeNode previousTaxon = null;
            List<TaxonTreeNode> oldTaxons = null;
            int previousImageIndex = -1;
            if (_CurrentTaxon != null && _taxon != null)
            {
                previousTaxon = _CurrentTaxon;
                if (_MultiImages.Visible)
                {
                    if (_keepOldImages) _MultiImages.GetTaxons(oldTaxons = new List<TaxonTreeNode>());
                    previousImageIndex = _MultiImages.GetTaxonCurrentImageIndex(_taxon);
                }
            }

            _CurrentTaxon = _taxon;
            UpdateCheckBoxes();

            if (_taxon != null)
            {
                Pictures_Display(_taxon, previousTaxon, oldTaxons, previousImageIndex);
                if (!_keepComment)
                    Comment_Display(_taxon);
            }
            else
            {
                Pictures_Clear();
                if (!_keepComment)
                    Comment_Clear();
            }

            if (focusedControl != null)
                focusedControl.Focus();
        }

        //---------------------------------------------------------------------------------
        public override void OnReselectTaxon(TaxonTreeNode _taxon)
        {
            OnSelectTaxon(_taxon);
        }

        //---------------------------------------------------------------------------------
        public override void OnAvailableImagesChanged()
        {
            if (_CurrentTaxon == null) return;
            TaxonBigVignette.CurrentTaxon = null;
            _MultiImages.ClearAll();
            Pictures_Display(_CurrentTaxon);
        }

        //---------------------------------------------------------------------------------
        public override void OnOptionChanged()
        {
            UpdateMultiImagesNumber();

            bool displayModeChanged = UpdateCheckBoxesImages();
            if (displayModeChanged)
            {
                OnReselectTaxon(_CurrentTaxon);
                _MultiImages.ScrollMode_Update(false);
            }
        }

        //=================================================================================
        // Options
        //

        //---------------------------------------------------------------------------------
        static int _NumberInMultiImageControl = 4;
        public static int NumberInMultiImageControl
        {
            get { return _NumberInMultiImageControl; }
            set 
            {
                if (value < 1) value = 1;
                if (value > 100) value = 100;
                if (_NumberInMultiImageControl == value)
                    return;

                _NumberInMultiImageControl = value;
                TaxonControlList.OnOptionChanged();
            }
        }

        //---------------------------------------------------------------------------------
        public enum DisplayModeForOthersEnum { Random, AllWithScroll };

        static DisplayModeForOthersEnum _DisplayModeForOthers = DisplayModeForOthersEnum.Random;
        public static DisplayModeForOthersEnum DisplayModeForOthers
        {
            get { return _DisplayModeForOthers; }
            set
            {
                if (_DisplayModeForOthers == value) return;
                _DisplayModeForOthers = value;
                TaxonControlList.OnOptionChanged();
            }
        }

        //=================================================================================
        // mouse events
        //

        //-------------------------------------------------------------------
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (_CurrentTaxon == null) return;
            if (!_MultiImages.Visible) return;
            if (e.Delta == 0) return;
            if (!_MultiImages.Bounds.Contains(e.Location)) return;

            bool ctrlKey = ModifierKeys.HasFlag(Keys.Control);
            if (ctrlKey)
            {
                if (e.Delta < 0)
                    NumberInMultiImageControl = _MultiImages.NextNumberToFillRectangle(NumberInMultiImageControl);
                else
                    NumberInMultiImageControl = _MultiImages.PreviousNumberToFillRectangle(NumberInMultiImageControl);

                _MouseWheelLastTime = DateTime.Now;
                if (_MouseWheelTimer == null)
                {
                    _MouseWheelTimer = new Timer() { Interval = 100 };
                    _MouseWheelTimer.Tick += _MouseWheelTimer_Tick;
                    _MouseWheelTimer.Start();
                }
            }
            else
                _MultiImages.OnMouseWheel(e.Delta);
        }

        Timer _MouseWheelTimer = null;
        DateTime _MouseWheelLastTime;

        void _MouseWheelTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - _MouseWheelLastTime;
            if (ts.TotalMilliseconds > 500)
            {
                if (_MultiImages.ListTaxons != null)
                    _MultiImages.ScrollMode_UpdateOutputImages();
                else
                    OnSelectTaxon(_CurrentTaxon, true, true);
                _MouseWheelTimer.Stop();
                _MouseWheelTimer = null;
            }
        }

        //=================================================================================
        // Pictures multi mode
        //
        bool UpdateMultiImagesNumber()
        {
            if (_NumberInMultiImageControl == _MultiImages.ImageNumber) return false;
            _MultiImages.ImageNumber = _NumberInMultiImageControl;
            return true;
        }

        //---------------------------------------------------------------------------------
        private void _MultiImages_OnEnterImage(object sender, EventArgs e)
        {
            if (sender is TaxonTreeNode ttn)
                Comments.AlternativeTaxon = ttn;
        }

        //---------------------------------------------------------------------------------
        private void _MultiImages_OnLeaveImage(object sender, EventArgs e)
        {
            Comments.AlternativeTaxon = null;
        }

        //---------------------------------------------------------------------------------
        private void _MultiImages_DoubleClick(object sender, Controls.TaxonMultiImageSoundControl.OnClickImageEventArgs e)
        {
            if (!(sender is Controls.TaxonMultiImageSoundControl)) return;
            if (e.Taxon == null) return;
            if (_CurrentTaxon != e.Taxon)
            {
                TaxonUtils.GotoTaxon(e.Taxon);
                TaxonUtils.SelectTaxon(e.Taxon);
                return;
            }
            NumberInMultiImageControl = 1;
            _MultiImages.ScrollTo(e.ImageIndex, e.Item?.Bounds);
        }

        //=================================================================================
        // Picture content
        //

        //---------------------------------------------------------------------------------
        private void Pictures_Clear()
        {
            TaxonBigVignette.Visible = false;
            TaxonBigVignette.Tag = null;
            _MultiImages.Visible = false;
            UpdateCheckBoxes();
        }

        //---------------------------------------------------------------------------------
        bool _CurrentTaxonIsSpecies = false;

        //---------------------------------------------------------------------------------
        private void Pictures_SetSpecies(TaxonTreeNode _species, int _previousImageIndex)
        {
            _CurrentTaxonIsSpecies = true;

            TaxonBigVignette.Visible = true;
            TaxonBigVignette.CurrentTaxon = _species;
            TaxonBigVignette.Tag = _species;

            _MultiImages.SetListFromUniqueTaxon(_species, _previousImageIndex );
            _MultiImages.Visible = true;
        }

        //---------------------------------------------------------------------------------
        private void Picture_SetSpeciesAndSubSpecies(TaxonTreeNode _species, int _previousImageIndex)
        {
            _CurrentTaxonIsSpecies = true;

            TaxonBigVignette.Visible = true;
            TaxonBigVignette.CurrentTaxon = _species;
            TaxonBigVignette.Tag = _species;

            List<TaxonTreeNode> speciesList = new List<TaxonTreeNode>();
            _species.GetAllChildrenWithImageRecursively(speciesList);

            //if (TaxonUtils.CurrentFilters.HasFilter(TaxonFilters.FilterAlive))
            //    speciesList = speciesList.Where(node => node.Desc.IsExtinct).ToList();

            if (speciesList.Contains(_species))
                speciesList.Remove(_species);
            else
                _species = null;

            _MultiImages.SetLists(speciesList, _species);
            _MultiImages.ScrollMode_UpdateOutputImages();
            _MultiImages.Visible = true;
        }
        

        //---------------------------------------------------------------------------------
        private void Pictures_SetFamily(TaxonTreeNode _family, TaxonTreeNode _previousSelectedTaxon = null, List<TaxonTreeNode> _previousDisplayedTaxons = null )
        {
            _CurrentTaxonIsSpecies = false;

            TaxonBigVignette.Visible = true;
            TaxonBigVignette.CurrentTaxon = _family;
            TaxonBigVignette.Tag = _family;

            if (DisplayModeForOthers == DisplayModeForOthersEnum.AllWithScroll)
            {
                List<TaxonTreeNode> speciesList = new List<TaxonTreeNode>();
                _family.GetAllChildrenWithImageRecursively(speciesList);

                //if (TaxonUtils.CurrentFilters.HasFilter(TaxonFilters.FilterAlive))
                //    speciesList = speciesList.Where(node => node.Desc.IsExtinct).ToList();

                _MultiImages.SetOldList(speciesList);
                _MultiImages.ScrollMode_UpdateOutputImages();
            }
            else
            {
                List<TaxonTreeNode> displayedTaxon = null;
                if (_previousSelectedTaxon != null)
                {
                    if (_previousSelectedTaxon == _family)
                        displayedTaxon = _previousDisplayedTaxons;
                    /*else if (_previousSelectedTaxon.Desc.HasImage && _previousSelectedTaxon.IsChildOf(_family))
                        displayedTaxon = new List<TaxonTreeNode>() { _previousSelectedTaxon };*/
                }

                _MultiImages.ListTaxons = null;
                int newChildrenNeeded = _NumberInMultiImageControl;
                if (displayedTaxon != null)
                    newChildrenNeeded -= displayedTaxon.Count;

                List<TaxonTreeNode> newChildren = null;
                if (newChildrenNeeded > 0)
                {
                    // build list of taxon with images
                    List<TaxonTreeNode> speciesList = new List<TaxonTreeNode>();

                    _family.GetAllChildrenWithImageRecursively(speciesList);
                    //if (TaxonUtils.CurrentFilters.HasFilter(TaxonFilters.FilterAlive))
                    //    speciesList = speciesList.Where(node => node.Desc.IsExtinct).ToList();

                    if (displayedTaxon != null)
                    {
                        foreach (TaxonTreeNode node in displayedTaxon)
                            speciesList.Remove(node);
                    }

                    // get random taxons, sort and fill it with null if needed
                    newChildren = TaxonUtils.RandomTaxon(speciesList, newChildrenNeeded);
                    //newChildren.Sort(new Comparison<TaxonTreeNode>(TaxonTreeNode.Compare));
                }

                if (displayedTaxon != null)
                {
                    if (newChildren != null) displayedTaxon.AddRange(newChildren);
                    newChildren = displayedTaxon;
                }
                newChildren.Sort(new Comparison<TaxonTreeNode>(TaxonTreeNode.Compare));
                while (newChildren.Count < _NumberInMultiImageControl) newChildren.Add(null);

                _MultiImages.SetTaxonList(newChildren);
            }
            _MultiImages.Visible = true;
        }

        //---------------------------------------------------------------------------------
        // Display Image of taxon
        private void Pictures_Display(TaxonTreeNode _taxon, TaxonTreeNode _previousSelectedTaxon = null, List<TaxonTreeNode> _previousDisplayedTaxons = null, int _previousImageIndex = -1 )
        {
            if (_taxon == null) return;

            if (!_taxon.HasChildren)
                Pictures_SetSpecies(_taxon, _previousImageIndex);    // display taxon image
            else if (_taxon.Desc.ClassicRank == ClassicRankEnum.Espece)
                Picture_SetSpeciesAndSubSpecies(_taxon, _previousImageIndex);
            else
                Pictures_SetFamily(_taxon, _previousSelectedTaxon, _previousDisplayedTaxons);

            UpdateCheckBoxes();

            if (_CurrentTaxonIsSpecies)
                TaxonBigVignette.Width = _MultiImages.Width;
            else
                TaxonBigVignette.Width = _MultiImages.Width - 32;
        }

        //---------------------------------------------------------------------------------
        private void UpdateCheckBoxes()
        {
            bool visible = _CurrentTaxon != null && !_CurrentTaxonIsSpecies;
            checkBoxList.Visible = visible;
            checkBoxRandom.Visible = visible;
        }

        private DisplayModeForOthersEnum? _CurrentCheckBoxDisplayMode = null;
        private bool UpdateCheckBoxesImages()
        {
            if (_CurrentCheckBoxDisplayMode != null && _CurrentCheckBoxDisplayMode.Value == DisplayModeForOthers) return false;
            _CurrentCheckBoxDisplayMode = DisplayModeForOthers;
            bool random = DisplayModeForOthers == DisplayModeForOthersEnum.Random;
            checkBoxList.Checked = !random;
            checkBoxRandom.Checked = random;
            return true;
        }

        private void CheckBoxList_Click(object sender, EventArgs e)
        {
            DisplayModeForOthers = DisplayModeForOthersEnum.AllWithScroll;
            checkBoxList.Checked = true;
        }

        private void CheckBoxRandom_Click(object sender, EventArgs e)
        {
            if (DisplayModeForOthers != DisplayModeForOthersEnum.Random)
                DisplayModeForOthers = DisplayModeForOthersEnum.Random;
            else
                OnReselectTaxon(_CurrentTaxon);
            checkBoxRandom.Checked = true;
        }

        //=================================================================================
        // Comment
        //

        //---------------------------------------------------------------------------------
        private void Comment_Clear()
        {
            splitContainerSpecificCharacter.Panel1Collapsed = true;
            Comments.MainTaxon = null;
            DescendantCount.Text = "";
        }

        //---------------------------------------------------------------------------------
        // display comments
        private void Comment_Display(TaxonTreeNode _taxon)
        {
            //display comment
            splitContainerSpecificCharacter.Panel1Collapsed = true;
            Comments.MainTaxon = _taxon;

            //display some additional information
            if (_taxon == null)
            {
                DescendantCount.Text = "";
            }
            else
            {
                int countSpecies = 0;
                int countSpeciesWithImages = 0;
                int countSubSpecies = 0;
                int countSubSpeciesWithImages = 0;
                _taxon.ParseNodeDesc((n) =>
                    {
                        if (n.ClassicRank == ClassicRankEnum.Espece)
                        {
                            countSpecies++;
                            if (n.HasImage) countSpeciesWithImages++;
                        }
                        else if (n.ClassicRank == ClassicRankEnum.SousEspece)
                        {
                            countSubSpecies++;
                            if (n.HasImage) countSubSpeciesWithImages++;
                        }
                    });


                string format = Localization.Manager.Get("_InfoImageCount", "Species: {0}, {1} with images, Subspecies: {2}, {3} with images");
                DescendantCount.Text = string.Format(format, countSpecies, countSpeciesWithImages, countSubSpecies, countSubSpeciesWithImages);
            }
        }

        //---------------------------------------------------------------------------------
        private void Comments_OnDocumentCompleted(object sender, EventArgs e)
        {
            splitContainerSpecificCharacter.Panel1Collapsed = !Comments.HasCollapsibleDivs;
        }

        //---------------------------------------------------------------------------------
        private void Comments_OnHasCollapsibleDivsChanged(object sender, EventArgs e)
        {
            splitContainerSpecificCharacter.Panel1Collapsed = !Comments.HasCollapsibleDivs;
        }

        //---------------------------------------------------------------------------------
        private void ButtonExpand_Click(object sender, EventArgs e)
        {
            Comments.ExpandAll();
        }

        //---------------------------------------------------------------------------------
        private void ButtonCollapse_Click(object sender, EventArgs e)
        {
            Comments.CollapseAll();
        }

        const int FEATURE_DISABLE_NAVIGATION_SOUNDS = 21;
        const int SET_FEATURE_ON_THREAD = 0x00000001;
        const int SET_FEATURE_ON_PROCESS = 0x00000002;
        const int SET_FEATURE_IN_REGISTRY = 0x00000004;
        const int SET_FEATURE_ON_THREAD_LOCALMACHINE = 0x00000008;
        const int SET_FEATURE_ON_THREAD_INTRANET = 0x00000010;
        const int SET_FEATURE_ON_THREAD_TRUSTED = 0x00000020;
        const int SET_FEATURE_ON_THREAD_INTERNET = 0x00000040;
        const int SET_FEATURE_ON_THREAD_RESTRICTED = 0x00000080;

        [DllImport("urlmon.dll")]
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        static extern int CoInternetSetFeatureEnabled( int FeatureEntry, [MarshalAs(UnmanagedType.U4)] int dwFlags, bool fEnable);
        static TaxonInfo()
        {
            CoInternetSetFeatureEnabled(FEATURE_DISABLE_NAVIGATION_SOUNDS, SET_FEATURE_ON_PROCESS, true);
        }


    }
}
