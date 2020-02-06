using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VinceToolbox.UserControl
{
    public partial class ListBoxOfIndexedString : ListBox
    {
        //=============================================================================================================
        private class IndexedString
        {
            public string m_name = "";
            public int m_index = -1;

            public IndexedString(int _index, string _name)
            {
                m_name = _name;
                m_index = _index;
            }
            
            public override string ToString()
            {
                return m_name.ToString();
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public void add( int _index, string _value )
        {
            Items.Add(new IndexedString(_index, _value));
        }

        //-------------------------------------------------------------------------------------------------------------
        public int getSelectedStringIndex()
        {
            int index = SelectedIndex;
            if (index == -1) return -1;
            IndexedString indexedString = Items[index] as IndexedString;
            if (indexedString == null) return -1;
            return indexedString.m_index;
        }

        
    }
}
