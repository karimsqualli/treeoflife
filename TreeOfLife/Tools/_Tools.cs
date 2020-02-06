using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using TreeOfLife.Properties;

namespace TreeOfLife.Tools
{
    //=============================================================================================
    // Interface for Tool (Form or UserControl)
    //
    public interface ITool
    {
        //-------------------------------------------------------------------
        bool CanActivate();
        void Activate();
    }

    //=============================================================================================
    public class ToolInfo
    {
        public Type Type = null;
        public string Description = "";
        public string DisplayName = "";
        public string Category = "";
        public bool PresentInUserMode = false;
        public bool PresentInAdminMode = true;
        public Image Icon = null;

        private string _SortName = null;
        public string SortName
        {
            set { _SortName = value; }
            get { return Category + "|" + (_SortName ?? DisplayName); }
        }
    }

    //=============================================================================================
    // Interface for Tool (Form or UserControl)
    //
    public class FactoryOfTool : List<ToolInfo>
    {
        //-------------------------------------------------------------------
        // constructor is private => only one class of this type in Singleton
        private FactoryOfTool() { SearchAllTools(); }
        static FactoryOfTool Singleton = new FactoryOfTool();

        //-------------------------------------------------------------------        
        public void SearchAllTools()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes())
            {
                if (type == typeof(ITool))
                    continue;

                if (typeof(ITool).IsAssignableFrom(type))
                {
                    ToolInfo info = new ToolInfo();
                    info.Type = type;
                    //info.LocId = type.ToString();
                    info.DisplayName = type.ToString(); // Localization.Manager.Get(info.LocId);

                    object[] attributes = type.GetCustomAttributes(true);
                    for (int i = 0; i < attributes.Length; i++)
                    {
                        if (attributes[i] is DescriptionAttribute)
                            info.Description = (((DescriptionAttribute)attributes[i]).Description);

                        if (attributes[i] is DisplayNameAttribute)
                            info.DisplayName = (((DisplayNameAttribute)attributes[i]).DisplayName);

                        if (attributes[i] is SortNameAttribute)
                            info.SortName = (((SortNameAttribute)attributes[i]).SortName);

                        if (attributes[i] is Controls.IconAttribute)
                            info.Icon = Resources.ResourceManager.GetObject(((Controls.IconAttribute)attributes[i]).IconName) as Image;

                        if (attributes[i] is CategoryAttribute)
                            info.Category = (((CategoryAttribute)attributes[i]).Category);

                        if (attributes[i] is PresentInModeAttribute)
                        {
                            info.PresentInUserMode = (((PresentInModeAttribute)attributes[i]).User);
                            info.PresentInAdminMode = (((PresentInModeAttribute)attributes[i]).Admin);
                        }
                    }

                    if (info.DisplayName == "")
                        info.DisplayName = type.Name;

                    this.Add(info);
                }
            }

            this.Sort((x, y) => x.SortName.CompareTo(y.SortName));

            foreach (ToolInfo info in this)
                Console.WriteLine("Found Tool: " + info.DisplayName);
        }

        public ToolStripMenuItem GetSubMenu(string _category, ToolStripMenuItem _in)
        {
            foreach (ToolStripMenuItem item in _in.DropDownItems)
            {
                if (item.Name == _category)
                    return item;
            }

            ToolStripMenuItem newItem = new ToolStripMenuItem();
            newItem.Name = _category;
            newItem.Text = _category;
            newItem.DropDownOpening += new System.EventHandler(this.DropDownOpening);
            
            _in.DropDownItems.Add(newItem);
            return newItem;
        }

        public static ToolStripMenuItem BuildMenus(bool _userMode) { return Singleton.InternalBuildMenus(_userMode); }
        ToolStripMenuItem InternalBuildMenus( bool _userMode)
        {
            ToolStripMenuItem result = new ToolStripMenuItem();

            foreach (ToolInfo info in this)
            {
                try
                {
                    if (_userMode && !info.PresentInUserMode)
                        continue;
                    if (!_userMode && !info.PresentInAdminMode)
                        continue;

                    string category = info.Category;
                    if (String.IsNullOrEmpty(category)) category = "Tools";

                    string[] splitCategory = category.Split(new Char[] { '/', '\\', '|' });
                    ToolStripMenuItem currentMenu = result;
                    for (int index = 0; index < splitCategory.Count(); index++)
                        currentMenu = GetSubMenu(splitCategory[index], currentMenu);

                    ToolStripMenuItem newItem = new ToolStripMenuItem();
                    ITool tool = info.Type.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ITool;
                    newItem.Tag = tool;
                    newItem.Name = info.Type.FullName;
                    newItem.Text = info.DisplayName;
                    newItem.Click += new System.EventHandler(MenuItemClick);
                    currentMenu.DropDownItems.Add(newItem);
                    //currentMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
                }
                catch( Exception e )
                {
                    Loggers.WriteError( LogTags.UI, "error while creating menu for command: " + info.Type.FullName + "\n    " + e.Message );
                }
            }

            return result;
        }

        private void DropDownOpening(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            if (menu == null) return;

            foreach (ToolStripMenuItem entry in menu.DropDownItems)
            {
                if (entry.Tag is ITool)
                {
                    entry.Enabled = (entry.Tag as ITool).CanActivate();
                }
            }
        }

        private void MenuItemClick(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            if (menu == null) return;
            if (menu.Tag is ITool)
                (menu.Tag as ITool).Activate();
        }

    }

    //=============================================================================================
    // new attribute to define comportment of tool in mode
    //
    [AttributeUsage(AttributeTargets.Class)]
    public class PresentInModeAttribute : System.Attribute
    {
        //-------------------------------------------------------------------
        public readonly bool User;
        public readonly bool Admin;

        public PresentInModeAttribute(bool _user, bool _admin)
        {
            this.User = _user;
            this.Admin = _admin;
        }
    }

    //=============================================================================================
    // new attribute to define string used to sort
    //
    [AttributeUsage(AttributeTargets.Class)]
    public class SortNameAttribute : System.Attribute
    {
        //-------------------------------------------------------------------
        public readonly string SortName;
        public SortNameAttribute(string _value)
        {
            this.SortName = _value;
        }
    }
}
