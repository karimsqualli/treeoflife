using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife.GUI
{
    public class ControlContainerTabsDesc
    {
        public ControlContainerTabsDesc()
        {
            ChildrenName = null;
            CurrentIndex = -1;
            ShowHeaderWhenOnlyOne = true;
        }

        public bool IsValid()
        {
            if (ChildrenName == null) return false;
            if (ChildrenName.Count == 0) return false;
            if (CurrentIndex == -1) return false;
            return true;
        }

        public List<string> ChildrenName { get; set; }
        public int CurrentIndex { get; set; }
        public bool ShowHeaderWhenOnlyOne { get; set; }

        public static ControlContainerTabsDesc FromTaxonTabControls(ControlContainerTabs _tab)
        {
            ControlContainerTabsDesc desc = new ControlContainerTabsDesc
            {
                ChildrenName = new List<string>()
            };
            foreach (Controls.TaxonControl tuc in _tab.Children)
                desc.ChildrenName.Add(tuc.GetType().FullName);
            desc.CurrentIndex = _tab.Children.IndexOf(_tab.Current);
            desc.ShowHeaderWhenOnlyOne = _tab.ShowHeaderWhenOnlyOne;
            return desc;
        }

        public ControlContainerTabs Rebuild()
        {
            ControlContainerTabs tabControl = new ControlContainerTabs
            {
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Dock = System.Windows.Forms.DockStyle.Fill,
                Current = null,
                TabIndex = 0,
                ShowHeaderWhenOnlyOne = ShowHeaderWhenOnlyOne
            };

            foreach (string name in ChildrenName)
            {
                Type type = FactoryOfTaxonControl.TaxonUserControlTypeFromName( name );
                if (type == null) continue;
                object o = type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                if (!(o is Controls.TaxonControl)) continue;
                tabControl.Add(o as Controls.TaxonControl);
            }

            if (CurrentIndex >= 0 && CurrentIndex < tabControl.Children.Count)
                tabControl.Current = tabControl.Children[CurrentIndex]; 

            return tabControl;
        }
    }
}
