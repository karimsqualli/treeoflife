using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace TreeOfLife.GUI
{
    public partial class FormContainer : Form
    {
        //---------------------------------------------------------------------------------
        private FormContainer()
        {
            InitializeComponent();
            //TopMost = true;
        }

        //---------------------------------------------------------------------------------
        public FormContainer(ControlContainerTabs _tabs) : this()
        {
            SetTabsContainer(_tabs);
            UpdateName();
        }

        //---------------------------------------------------------------------------------
        public FormContainer(GuiControlInterface _itf) : this()
        {
            SetTabsContainer();
            ControlContainerTabs tabControl = GetTabsContainer();
            if (tabControl == null) return;
            if (_itf == null || _itf.Control == null) return;
            tabControl.Add(_itf);
            UpdateName();

            if (_itf.Control.MinimumSize.Width != 0 && _itf.Control.MinimumSize.Height != 0)
            {
                int addX = Size.Width - ClientRectangle.Width;
                int addY = Size.Height - ClientRectangle.Height;
                MinimumSize = new Size(_itf.Control.MinimumSize.Width + addX, _itf.Control.MinimumSize.Height + addY);
            }
        }

        //---------------------------------------------------------------------------------
        public FormContainer(ControlContainerSplitter _split) : this()
        {
            SetSplitterContainer(_split);
            UpdateName();
        }

        //---------------------------------------------------------------------------------
        private void FormContainer_FormClosed(object sender, FormClosedEventArgs e)
        {
            List<GuiControlInterface> all = GetAllControl();
            foreach (TreeOfLife.Controls.TaxonControl tc in all)
                TreeOfLife.Controls.TaxonControlList.UnregisterTaxonControl(tc);
        }

        //---------------------------------------------------------------------------------
        public void UpdateName()
        {
            ControlContainerTabs tab = GetTabsContainer();
            if (tab != null && tab.Current != null) 
            {
                Text = tab.Current.ToString();
                return;
            }

            Text = "Floating tool";
        }

        //---------------------------------------------------------------------------------
        public static void UpdateName(Control _control)
        {
            Control current = _control;
            while (current != null)
            {
                if (current is FormContainer)
                    break;
                current = current.Parent;
            }
            if (current == null) return;
            (current as FormContainer).UpdateName();
        }

        //---------------------------------------------------------------------------------
        public static FormContainer GetFormContainer(ControlContainerTabs _container)
        {
            Control current = _container;
            while (current != null)
            {
                if (current is FormContainer)
                    return current as FormContainer;
                current = current.Parent;
            }
            return null;
        }

        //---------------------------------------------------------------------------------
        public void SetTabsContainer(ControlContainerTabs _tabs = null )
        {
            if (_tabs == null)
            {
                _tabs = new ControlContainerTabs
                {
                    ShowHeaderWhenOnlyOne = false
                };
            }
            _tabs.OnCurrentTabChanged += OnCurrentTabChanged;

            Controls.Clear();
            Controls.Add(_tabs);
        }

        //---------------------------------------------------------------------------------
        public ControlContainerTabs GetTabsContainer() 
        {
            if (Controls.Count != 1) return null;
            return Controls[0] as ControlContainerTabs;
        }

        //---------------------------------------------------------------------------------
        public void OnCurrentTabChanged(object sender, EventArgs e)
        {
            UpdateName();
        }

        //---------------------------------------------------------------------------------
        public void SetSplitterContainer(ControlContainerSplitter _splitter = null)
        {
            if (_splitter == null)
                _splitter = new ControlContainerSplitter();
            Controls.Clear();
            Controls.Add(_splitter);
        }

        //---------------------------------------------------------------------------------
        public ControlContainerSplitter GetSplitterContainer()
        {
            if (Controls.Count != 1) return null;
            return Controls[0] as ControlContainerSplitter;
        }

        //---------------------------------------------------------------------------------
        public ControlContainerInterface GetControlContainer()
        {
            if (Controls.Count != 1) return null;
            return Controls[0] as ControlContainerInterface;
        }

        //---------------------------------------------------------------------------------
        bool _UserMove = false;
        protected override void OnMove(EventArgs e)
        {
            if (!_UserMove) return;
            if (IsDisposed) return;
            ControlContainerInterface interf = ControlContainerInterfaceList.getUnderMouse(null);
            ControlContainerInterfaceList.showAnchors(interf);
            ControlContainerInterfaceList.updateAnchors();
            base.OnMove(e);
        }

        //---------------------------------------------------------------------------------
        private void FormContainer_ResizeBegin(object sender, EventArgs e)
        {
            _UserMove = true;
        }

        //---------------------------------------------------------------------------------
        private void FormContainer_ResizeEnd(object sender, EventArgs e)
        {
            _UserMove = false;
            DockStyle dock = ControlContainerInterfaceList.pickAnchor(out ControlContainerInterface container);
            ControlContainerInterfaceList.hideAnchors();

            if (container == null || dock == DockStyle.None) return;

            if (dock != DockStyle.Fill)
                container = container.Split(dock);

            List<GuiControlInterface> allControls = GetAllControl();
            foreach(GuiControlInterface tc in allControls)
                container.Add(tc);
            FormContainer.UpdateName(container.GetControl());

            Close();
        }

        //---------------------------------------------------------------------------------
        private List<GuiControlInterface> GetAllControl()
        {
            List<GuiControlInterface> allControls = new List<GuiControlInterface>();

            ControlContainerInterface container = GetControlContainer();
            if (container != null)
            {
                container.GetAll(allControls);
                container.DetachAll();
            }

            return allControls;
        }

        
    }
}
