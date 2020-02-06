using System;
using WinForms = System.Windows.Forms;


namespace TreeOfLife.Localization
{
    public class Form : WinForms.Form
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = Manager.Get(ToString()+"_Title", Text);
            UserControl.DoControls(ToString(), Controls);
            Manager.OnCurrentLanguageChanged += OnCurrentLanguageChanged;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Manager.OnCurrentLanguageChanged -= OnCurrentLanguageChanged;
        }

        //-------------------------------------------------------------------
        public void OnCurrentLanguageChanged(object sender, EventArgs e)
        {
            Text = Manager.Get(ToString() + "_Title", Text);
            UserControl.DoControls(ToString(), Controls);
            Invalidate();
        }
    }

    public class Tag
    {
        public bool Ignore { get; set; }
    }


    public class UserControl : WinForms.UserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            if (DesignMode) return;
            base.OnLoad(e);
            DoControls(ToString(), Controls);
            Manager.OnCurrentLanguageChanged += OnCurrentLanguageChanged;
        }

        //-------------------------------------------------------------------
        public void OnCurrentLanguageChanged(object sender, EventArgs e)
        {
            DoControls(ToString(), Controls);
            Invalidate();
        }

        //-------------------------------------------------------------------
        public static void DoControls( string _key, ControlCollection _controls )
        {
            foreach (WinForms.Control control in _controls)
            {
                if (control is WinForms.DataGridView)
                    DoDataGridView(_key, _grid: control as WinForms.DataGridView);
                else if (control is WinForms.ToolStrip)
                    Manager.DoMenu(control as WinForms.ToolStrip);
                else if (control is WinForms.TextBox)
                    continue;
                else
                {
                    if (!(control.Tag is Tag t && t.Ignore))
                    {
                        if (Manager.IsLocalizable(control.Text))
                            control.Text = Manager.Get(_key + "." + control.Name, control.Text);
                    }
                    DoControls(_key, control.Controls);
                }
            }
        }

        //-------------------------------------------------------------------
        public static void DoDataGridView(string _key, WinForms.DataGridView _grid )
        {
            foreach(WinForms.DataGridViewColumn column in _grid.Columns)
                if (Manager.IsLocalizable(column.HeaderText))
                    column.HeaderText= Manager.Get(_key + "." + column.Name, column.HeaderText);
        }
    }
}
