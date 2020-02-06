using System;
using System.Drawing;
using System.Windows.Forms;
using TreeOfLife.GUI;

namespace TreeOfLife.Controls
{
    //=============================================================================================
    // TaxonUserControl : user control that register to mainform to receive messages via interface
    //
    public class TaxonControl : Localization.UserControl, ITaxonControl
    {
        //-------------------------------------------------------------------
        public TaxonControl()
        {
            if (DesignMode) return;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            TaxonControlList.RegisterTaxonControl(this);
        }

        public Localization.UserControl Control { get => this; }

        //-------------------------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            TaxonControlList.InitTaxonControlOnLoad(this);
            ApplyTheme();
            base.OnLoad(e);
            if (ParentForm != null)
                ParentForm.FormClosed += ParentForm_FormClosed;

            SetRoot(TaxonUtils.Root);
            if (!(this is TaxonGraphPanel) && TaxonUtils.MainGraph != null)
                OnSelectTaxon(TaxonUtils.MainGraph.Selected);
        }

        //-------------------------------------------------------------------
        protected virtual void OnClose()
        {
            TaxonControlList.UnregisterTaxonControl(this);
        }

        //-------------------------------------------------------------------
        protected virtual void ApplyTheme()
        {
            BackColor = Theme.Current.Control_Background;

            foreach( Control ctrl in Controls)
            {
                if (ctrl is MenuStrip menuStrip)
                    Theme.Current.Menu_ApplyTheme(menuStrip);
            }
        }

        //-------------------------------------------------------------------
        void ParentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ParentForm == sender)
                OnClose();
        }

        //-------------------------------------------------------------------
        protected TaxonTreeNode _Root = null;
        public virtual TaxonTreeNode Root
        {
            get { return _Root; }
            set { _Root = value; }
        }

        //-------------------------------------------------------------------
        public virtual void SetRoot(TaxonTreeNode _taxon) { Root = _taxon; }
        public virtual void OnSelectTaxon(TaxonTreeNode _taxon) { }
        public virtual void OnReselectTaxon(TaxonTreeNode _taxon) { }
        public virtual void OnBelowChanged(TaxonTreeNode _taxon) { }
        public virtual void OnRefreshAll() { }
        public virtual void OnRefreshGraph() { }
        public virtual void OnViewRectangleChanged(Rectangle R) { }
        public virtual void OnTaxonChanged(object sender, TaxonTreeNode _taxon) { }
        
        public virtual void OnOptionChanged() { }

        public virtual void OnAvailableImagesChanged() { }

        public GUI.ControlContainerInterface OwnerContainer { get; set; }

        public virtual bool CanBeMoved { get { return true; } }
        public virtual bool CanBeClosed { get { return true; } }
    }

    //=============================================================================================
    // new attribute to set an icon for taxon control
    // a déplacer dans Vincetoolbox
    //
    [AttributeUsage(AttributeTargets.All)]
    public class IconAttribute : Attribute
    {
        //-------------------------------------------------------------------
        public readonly string IconName;
        public IconAttribute(string _name)
        {
            this.IconName = _name;
        }
    }
}
    
