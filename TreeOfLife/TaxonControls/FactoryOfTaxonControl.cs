using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using TreeOfLife.Properties;
using TreeOfLife.Controls;

namespace TreeOfLife
{
    public class TaxonControlInfo
    {
        public string Description = "";
        public string LocId = null;
        public string InvariantDisplayName = null;
        public Image Icon = null;
        public string DisplayName { get; set; } = null;

        public GUI.GuiControlInterface CurrentTaxonControl
        {
            set
            {
                if (string.IsNullOrEmpty(InvariantDisplayName))
                    DisplayName = value.ToString();
                else
                    DisplayName = Localization.Manager.Get( LocId, InvariantDisplayName);
            }
        }
    }

    public class FactoryOfTaxonControl : Dictionary<Type, TaxonControlInfo>
    {
        //-------------------------------------------------------------------
        // constructor is private => only one class of this type in Singleton
        private FactoryOfTaxonControl() 
        { 
            SearchAllTaxonUserControl();  
        }
        static FactoryOfTaxonControl Singleton = new FactoryOfTaxonControl();

        //-------------------------------------------------------------------        
        public void SearchAllTaxonUserControl()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.BaseType == null) continue;
                if (type.BaseType == typeof(Controls.TaxonControl))
                {
                    TaxonControlInfo info = new TaxonControlInfo();

                    object[] attributes = type.GetCustomAttributes(true);
                    for (int i = 0; i < attributes.Length; i++)
                    {
                        if (attributes[i] is DescriptionAttribute)
                            info.Description = (((DescriptionAttribute)attributes[i]).Description);

                        info.LocId = type.ToString();
                        if (attributes[i] is DisplayNameAttribute)
                            info.InvariantDisplayName = (((DisplayNameAttribute)attributes[i]).DisplayName);

                        if (attributes[i] is Controls.IconAttribute)
                            info.Icon = Resources.ResourceManager.GetObject(((Controls.IconAttribute)attributes[i]).IconName) as Image;
                    }

                    if (info.Icon == null)
                        info.Icon = Resources.ResourceManager.GetObject("TaxonControl") as Image;
                    this[type] = info;
                }
            }

            foreach (KeyValuePair<Type, TaxonControlInfo> pair in this)
                Console.WriteLine("Found UserControl: " + pair.Key.FullName);
        }

        //-------------------------------------------------------------------
        public static TaxonControlInfo TaxonControlInfoFromType(Type _type)
        {
            return Singleton[_type];
        }

        //-------------------------------------------------------------------
        public static Type TaxonUserControlTypeFromName(string _name)
        {
            foreach (KeyValuePair<Type, TaxonControlInfo> pair in Singleton)
            {
                if (pair.Key.FullName == _name) 
                    return pair.Key;
            }
            return null;
        }
    }
}
