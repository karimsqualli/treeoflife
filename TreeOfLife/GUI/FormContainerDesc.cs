using VinceToolbox;

namespace TreeOfLife.GUI
{
    public class FormContainerDesc
    {
        //-------------------------------------------------------------------
        public FormContainerDesc()
        {
            winFunctions.winPlacementSetInvalid(_Placement);
        }

        //-------------------------------------------------------------------
        public bool IsValid()
        {
            if (!winFunctions.winPlacementIsValid(_Placement)) return false;
            if (_TaxonTabControlsDescriptor == null && _SplitterContainerDescriptor == null) return false;
            return true;
        }

        //-------------------------------------------------------------------
        // Datas
        //

        private winFunctions.winPlacement _Placement;
        public winFunctions.winPlacement Placement
        {
            get { return _Placement; }
            set { _Placement = value; }
        }

        GUI.ControlContainerTabsDesc _TaxonTabControlsDescriptor = null;
        public GUI.ControlContainerTabsDesc TaxonTabControlsDescriptor
        {
            get { return _TaxonTabControlsDescriptor; }
            set { _TaxonTabControlsDescriptor = value; }
        }

        GUI.ControlContainerSplitterDesc _SplitterContainerDescriptor = null;
        public GUI.ControlContainerSplitterDesc SplitterContainerDescriptor
        {
            get { return _SplitterContainerDescriptor; }
            set { _SplitterContainerDescriptor = value; }
        }

        //-------------------------------------------------------------------
        // build descriptor from form
        //
        public static FormContainerDesc FromFormContainer(FormContainer _form)
        {
            FormContainerDesc desc = new FormContainerDesc();

            desc.Placement = winFunctions.winGetPlacement(_form.Handle);
            
            if (_form.Controls[0] is GUI.ControlContainerTabs)
                desc._TaxonTabControlsDescriptor = GUI.ControlContainerTabsDesc.FromTaxonTabControls(_form.Controls[0] as GUI.ControlContainerTabs);
            else if (_form.Controls[0] is GUI.ControlContainerSplitter)
                desc.SplitterContainerDescriptor = GUI.ControlContainerSplitterDesc.FromTaxonSplitterContainer(_form.Controls[0] as GUI.ControlContainerSplitter);
            return desc;
        }

        //-------------------------------------------------------------------
        // rebuild form from descriptor
        //
        public FormContainer Rebuild()
        {
            FormContainer form = null;

            if (_SplitterContainerDescriptor != null)
            {
                GUI.ControlContainerSplitter split = _SplitterContainerDescriptor.Rebuild();
                form = new FormContainer(split);
            }
            else if (_TaxonTabControlsDescriptor != null)
            {
                GUI.ControlContainerTabs tabControl = _TaxonTabControlsDescriptor.Rebuild();
                form = new FormContainer(tabControl);
            }

            if (form != null && winFunctions.winPlacementIsValid(Placement))
                winFunctions.winSetPlacement(form.Handle, Placement);

            return form;
        }
    }
}
