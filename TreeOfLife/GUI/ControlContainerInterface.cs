using System.Collections.Generic;
using System.Windows.Forms;

namespace TreeOfLife.GUI
{
    //=============================================================================================
    // Interface that TaxonControlContainer have to implement
    // 
    public interface ControlContainerInterface
    {
        Control GetControl();

        ControlContainerInterface Split(DockStyle _dock);
        //void Add(Controls.TaxonControl _control, bool _visible = true);
        //void Remove(Controls.TaxonControl _control, bool _closeIfEmpty);
        //void GetAll(List<Controls.TaxonControl> _list);
        //void SetFocus(Controls.ITaxonControl _control);
        void Add(GuiControlInterface _control, bool _visible = true);
        void Remove(GuiControlInterface _control, bool _closeIfEmpty);
        void GetAll(List<GuiControlInterface> _list);
        void SetFocus(GuiControlInterface _control);
        void DetachAll();
    }

    public interface GuiControlInterface
    {
        //-------------------------------------------------------------------
        bool CanBeMoved { get; }

        //-------------------------------------------------------------------
        ControlContainerInterface OwnerContainer { get; set; }

        //-------------------------------------------------------------------
        Localization.UserControl Control { get; }
    }

}
