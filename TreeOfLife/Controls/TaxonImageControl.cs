using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeOfLife.Properties;

namespace TreeOfLife.Controls
{
    

    public partial class TaxonImageControl : UserControl
    {

        //---------------------------------------------------------------------------------
        public TaxonImageControl()
        {
            SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint | System.Windows.Forms.ControlStyles.UserPaint | System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();

            Data.OnCurrentImageChanged += (o, e) => Invalidate();
            DisplayParams.DisplayMode = ImageDisplayMode;
            DisplayParams.OnChanged += (o, e) => Invalidate();

            CurrentImage = ImageNone;

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        //---------------------------------------------------------------------------------
        protected override void OnResize(EventArgs e)
        {
            Invalidate();
        }

        //---------------------------------------------------------------------------------
        public VignetteData Data { get; private set; } = new VignetteData();

        //---------------------------------------------------------------------------------
        public TaxonTreeNode CurrentTaxon
        {
            get => Data.CurrentTaxon;
            set => Data.CurrentTaxon = value;
        }

        //---------------------------------------------------------------------------------
        public void SetTaxonExt(TaxonTreeNode _taxon, List<TaxonImageDesc> _list, int _index, bool _canNavigate )
        {
            Data.SetTaxonExt(_taxon, _list, _index, _canNavigate);
        }

        //---------------------------------------------------------------------------------
        public List<TaxonImageDesc> ListImages
        {
            get => Data.ListImages;
            set => Data.ListImages = value;
        }

        public int CurrentImageIndex
        {
            get => Data.CurrentImageIndex;
            private set => Data.CurrentImageIndex = value;
        }

        public TaxonImageDesc GetImageDesc()
        {
            return Data.GetImageDesc();
        }

        public void SetImageDesc(TaxonImageDesc _imageDesc)
        {
            Data.SetImageDesc(_imageDesc);
        }

        public string GetImagePath()
        {
            return Data.GetImagePath();
        }

        //---------------------------------------------------------------------------------
        public Image CurrentImage
        {
            get => Data.CurrentImage;
            set => Data.CurrentImage = value;
        }

        //---------------------------------------------------------------------------------
        public VignetteDisplayParams DisplayParams = new VignetteDisplayParams();

        public VignetteDisplayParams.ModeEnum DisplayMode
        {
            get => DisplayParams.DisplayMode;
            set => DisplayParams.DisplayMode = value; 
        }

        public bool DisplayImage
        {
            get => DisplayParams.DisplayImage;
            set => DisplayParams.DisplayImage = value; 
        }

        public bool ImageVisible
        {
            get => DisplayParams.ImageVisible;
            set => DisplayParams.ImageVisible = value;
        }

        public bool DisplayLatin
        {
            get => DisplayParams.DisplayLatin;
            set => DisplayParams.DisplayLatin = value; 
        }

        public bool LatinVisible
        {
            get => DisplayParams.LatinVisible;
            set => DisplayParams.LatinVisible = value; 
        }

        public bool DisplayFrench
        {
            get => DisplayParams.DisplayFrench;
            set => DisplayParams.DisplayFrench = value; 
        }

        public bool FrenchVisible
        {
            get => DisplayParams.FrenchVisible;
            set => DisplayParams.FrenchVisible = value; 
        }

        public bool AllowSecondaryImages
        {
            get { return Data.AllowSecondaryImages; }
            set
            {
                if (Data.AllowSecondaryImages == value) return;
                Data.AllowSecondaryImages = value;
                if (CurrentTaxon != null)
                {
                    TaxonTreeNode save = CurrentTaxon;
                    CurrentTaxon = null;
                    CurrentTaxon = save;
                }
            }
        }

        public bool AllowNavigationButtons
        {
            get => Data.AllowNavigationButtons;
            set => Data.AllowNavigationButtons = value;
        }

        //---------------------------------------------------------------------------------
        public bool AllowSound
        {
            get => Data.AllowSound;
            set => Data.AllowSound = value;
        }

        //---------------------------------------------------------------------------------
        public bool AllowTips
        {
            get => Data.AllowTips;
            set => Data.AllowTips = value;
        }

        //---------------------------------------------------------------------------------
        public bool AllowContextualMenu
        {
            get => Data.AllowContextualMenu;
            set => Data.AllowContextualMenu = value;
        }

        //--------------------------------------------------------------------------------------
        // mouse button
        //
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (ClickButtons(e.Location))
                return;
            if (Data.SoundPlayerData != null && Data.SoundPlayerData.OnMouseClick(e))
                return;
            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Data.OnMouseMove(e);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Data.SoundPlayerData?.OnMouseLeave();
            base.OnMouseLeave(e);
        }

        //--------------------------------------------------------------------------------------
        // code en double : listbox et ici => refacto
        //
        public event CancelEventHandler OnContextMenuOpening = null;

        void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CurrentTaxon == null)
            {
                e.Cancel = true;
                return;
            }

            TaxonContextualMenu tcm = new TaxonContextualMenu(CurrentTaxon) { ImageDesc = GetImageDesc() };
            ContextMenuStrip.Items.Clear();
            if (!tcm.Build(ContextMenuStrip))
                e.Cancel = true;
            else
            {
                e.Cancel = false;
                OnContextMenuOpening?.Invoke(sender, e);
            }
        }

        //=================================================================================
        // Next prev
        //
        
        public bool ClickButtons( Point _pt )
        {
            return StaticClickButtons(_pt, Data);
        }

        //=================================================================================
        // Main Paint
        //

        //---------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (CurrentImage == null || CurrentTaxon == null) return;
            StaticPaint(e.Graphics, ClientRectangle, PointToClient( Cursor.Position), DisplayParams, Data);
        }
    }
}
