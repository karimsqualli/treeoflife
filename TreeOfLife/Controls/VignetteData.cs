using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Controls
{
    public class VignetteData : IDisposable
    {
        //---------------------------------------------------------------------------------
        public VignetteData()
        {
            CurrentImage = TaxonImageControl.ImageWait;
        }

        //---------------------------------------------------------------------------------
        public void Dispose()
        {
            CurrentImage = null;
        }

        //---------------------------------------------------------------------------------
        private bool _AllowSecondaryImages = false;
        public bool AllowSecondaryImages
        {
            get { return _AllowSecondaryImages; }
            set
            {
                if (_AllowSecondaryImages == value) return;
                _AllowSecondaryImages = value;
                OnCurrentImageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //---------------------------------------------------------------------------------
        private bool _AllowNavigationButtons = true;
        public bool AllowNavigationButtons
        {
            get { return _AllowNavigationButtons; }
            set
            {
                if (_AllowNavigationButtons == value) return;
                _AllowNavigationButtons = value;
                OnCurrentImageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //---------------------------------------------------------------------------------
        private bool _AllowSound = true;
        public bool AllowSound
        {
            get => _AllowSound;
            set
            {
                if (_AllowSound == value) return;
                _AllowSound = value;
                UpdateSoundControl();
                OnCurrentImageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //---------------------------------------------------------------------------------
        private bool _AllowTips = true;
        public bool AllowTips
        {
            get { return _AllowTips; }
            set
            {
                if (_AllowTips == value) return;
                _AllowTips = value;
                TipManager.SetTaxon(null, null);
            }
        }

        //---------------------------------------------------------------------------------
        private bool _AllowContextualMenu = true;
        public bool AllowContextualMenu
        {
            get { return _AllowContextualMenu; }
            set
            {
                if (_AllowContextualMenu == value) return;
                _AllowContextualMenu = value;
            }
        }

        //---------------------------------------------------------------------------------
        TaxonTreeNode _CurrentTaxon = null;
        public TaxonTreeNode CurrentTaxon
        {
            get { return _CurrentTaxon; }
            set
            {
                if (_CurrentTaxon == value) return;
                _CurrentTaxon = value;
                UpdateSoundControl();
                if (_CurrentTaxon == null)
                {
                    ListImages = null;
                    CurrentImage = null;
                }
                else
                {
                    if (AllowSecondaryImages)
                        ListImages = TaxonImages.Manager.GetListImages(_CurrentTaxon.Desc);
                    else
                        ListImages = TaxonImages.Manager.GetListMainImages(_CurrentTaxon.Desc);
                    CurrentImageIndex = 0;
                    ReloadImage();
                }
                OnCurrentImageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //---------------------------------------------------------------------------------
        public void SetTaxonExt(TaxonTreeNode _taxon, List<TaxonImageDesc> _list, int _index, bool _canNavigate)
        {
            _AllowNavigationButtons = _canNavigate;
            if (_CurrentTaxon == _taxon && _index == CurrentImageIndex) return;
            _CurrentTaxon = _taxon;
            ListImages = _list;
            CurrentImageIndex = _index;
            ReloadImage();
        }

        //---------------------------------------------------------------------------------
        public void SetTaxonExt(TaxonTreeNode _taxon, List<TaxonImageDesc> _list, int _index, bool _canNavigate, bool _allowSound)
        {
            AllowNavigationButtons = _canNavigate;
            AllowSound = _allowSound;
            if (_CurrentTaxon == _taxon && _index == CurrentImageIndex) return;
            _CurrentTaxon = _taxon;
            ListImages = _list;
            CurrentImageIndex = _index;
            UpdateSoundControl();
            ReloadImage();
        }

        //---------------------------------------------------------------------------------
        Image _CurrentImage = null;
        public Image CurrentImage
        {
            get { return _CurrentImage; }
            set
            {
                if (value == null) value = TaxonImageControl.ImageWait;
                if (_CurrentImage == value) return;
                if (_CurrentImage != null && !CurrentImageIsSystem)
                    _CurrentImage.Dispose();
                _CurrentImage = value;
                OnCurrentImageChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler OnCurrentImageChanged = null;

        //---------------------------------------------------------------------------------
        public bool CurrentImageIsSystem { get => (_CurrentImage == TaxonImageControl.ImageNone) || (_CurrentImage == TaxonImageControl.ImageWait); }

        //---------------------------------------------------------------------------------
        public int CurrentImageIndex
        {
            get { return _CurrentImageIndex; }
            set
            {
                if (ListImages == null || ListImages.Count == 0)
                    value = -1;
                else if (value < 0)
                    value = 0;
                else if (value >= ListImages.Count)
                    value = ListImages.Count - 1;

                if (_CurrentImageIndex == value) return;
                _CurrentImageIndex = value;
                ReloadImage();
                OnCurrentImageIndexChanged?.Invoke(this, null);
            }
        }
        int _CurrentImageIndex = 0;
        public event EventHandler OnCurrentImageIndexChanged = null;

        //---------------------------------------------------------------------------------
        public List<TaxonImageDesc> ListImages { get; set; } = null;

        //---------------------------------------------------------------------------------
        public TaxonImageDesc GetImageDesc()
        {
            if (ListImages == null) return null;
            if (_CurrentImageIndex < 0 || _CurrentImageIndex >= ListImages.Count) return null;
            return ListImages[_CurrentImageIndex];
        }

        //---------------------------------------------------------------------------------
        public void SetImageDesc(TaxonImageDesc _imageDesc)
        {
            if (ListImages == null) return;
            int index = ListImages.IndexOf(_imageDesc);
            if (index != -1 && _CurrentImageIndex != index)
            {
                CurrentImageIndex = index;
                ReloadImage();
            }
        }

        //---------------------------------------------------------------------------------
        public string GetImagePath()
        {
            if (ListImages == null) return null;
            if (_CurrentImageIndex < 0 || _CurrentImageIndex >= ListImages.Count) return null;
            return ListImages[_CurrentImageIndex].GetPath(_CurrentTaxon.Desc);
        }

        //---------------------------------------------------------------------------------
        bool ImageRequestDone = false;
        public Image GetCurrentImageToDraw()
        {
            if (!ImageRequestDone)
            {
                ImageRequestDone = true;
                TaxonImages.Manager.RegisterDelayedGetFullImage(this, _CurrentTaxon, GetImageDesc(), OnImageLoaded);
            }
            return _CurrentImage;
        }

        //---------------------------------------------------------------------------------
        public void ReloadImage()
        {
            CurrentImage = TaxonImageControl.ImageWait;
            if (ListImages == null || ListImages.Count == 0)
            {
                CurrentImage = null;
                return;
            }

            TaxonImages.Manager.RegisterDelayedGetFullImage(this, _CurrentTaxon, GetImageDesc(), OnImageLoaded);
        }

        //---------------------------------------------------------------------------------
        void OnImageLoaded(TaxonTreeNode _taxon, Image _image)
        {
            if (_taxon != _CurrentTaxon) return;
            if (_image == null)
            {
                CurrentImage = TaxonImageControl.ImageNone;
            }
            CurrentImage = _image;
        }

        //---------------------------------------------------------------------------------
        public void ResetButtons()
        {
            ButtonNextEnable = false;
            ButtonPreviousEnable = false;
        }

        public void EnableButtonNext(Rectangle R)
        {
            ButtonNextEnable = true;
            ButtonNextBounds = R;
        }

        public void EnableButtonPrevious(Rectangle R)
        {
            ButtonPreviousEnable = true;
            ButtonPreviousBounds = R;
        }


        public bool ButtonNextEnable = false;
        public bool ButtonNextHovered = false;
        public Rectangle ButtonNextBounds;
        public bool ButtonPreviousEnable = false;
        public bool ButtonPreviousHovered = false;
        public Rectangle ButtonPreviousBounds;

        public void SetButtonHovered( bool _next, bool _previous )
        {
            if (_next == ButtonNextHovered && _previous == ButtonPreviousHovered)
                return;
            ButtonNextHovered = _next;
            ButtonPreviousHovered = _previous;
            OnCurrentImageChanged?.Invoke(this, EventArgs.Empty);
        }

        //=================================================================================
        // Sounds
        //

        //---------------------------------------------------------------------------------
        public VinceSoundPlayer.PlayerSmallData SoundPlayerData = null;

        //---------------------------------------------------------------------------------
        public void UpdateSoundControl()
        {
            if (AllowSound && CurrentTaxon != null && CurrentTaxon.Desc.HasSound)
            {
                if (SoundPlayerData == null)
                {
                    SoundPlayerData = new VinceSoundPlayer.PlayerSmallData()
                    {
                        DisplayMode = VinceSoundPlayer.PlayerSmall.DisplayModeEnum.PlayPause
                    };
                    SoundPlayerData.AllowOpen = false;
                    SoundPlayerData.DisplayMode = VinceSoundPlayer.PlayerSmall.DisplayModeEnum.PlayPauseStop;
                    SoundPlayerData.OnChanged += (o,e) => OnCurrentImageChanged?.Invoke(this, EventArgs.Empty);
                }

                //string soundPath = TaxonUtils.GetSoundFullPath(CurrentTaxon);
                string soundPath = null;

                if (soundPath == null || ! File.Exists(soundPath))
                {
                    soundPath = TOLData.DownloadSound(CurrentTaxon);
                }

                SoundPlayerData.File = soundPath;
            }
            else
            {
                if (SoundPlayerData != null)
                {
                    SoundPlayerData = null;
                }
            }
        }

        //=============================================================================
        // Mouse event
        //
        public void OnMouseMove(MouseEventArgs e)
        {
            bool hoverNext = ButtonNextEnable && ButtonNextBounds.Contains(e.Location);
            bool hoverPrevious = ButtonPreviousEnable && ButtonPreviousBounds.Contains(e.Location);
            SetButtonHovered(hoverNext, hoverPrevious);
            SoundPlayerData?.OnMouseMove(e);
        }

        public void OnMouseLeave()
        {
            SetButtonHovered(false, false);
            SoundPlayerData?.OnMouseLeave();
            // warning some leave are not sent
            // ButtonPrevNextCollapsed = true;
            // OnCurrentImageChanged?.Invoke(this, EventArgs.Empty);
        }

        public void OnMouseEnter()
        {
            // ButtonPrevNextCollapsed = false;
            // OnCurrentImageChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
