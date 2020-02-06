using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace VinceToolbox
{
    [Serializable()]
    public class FileHistory
    {
        //=========================================================================================
        // Constants
        //=========================================================================================
        
        //! number max of files in history
        private const int c_maxFiles = 15;

        //=========================================================================================
        // sub classes
        //=========================================================================================

        //-----------------------------------------------------------------------------------------
        //! one history file, i.e. a file and a date
        //
        public class Entry
        {
            public string      m_name;
            public DateTime    m_date;
        };

        //=========================================================================================
        // members
        //=========================================================================================

        //! list of history files
        private List<Entry> m_list = new List<Entry>();

        //=========================================================================================
        // functions
        //=========================================================================================

        //-----------------------------------------------------------------------------------------
        //! add one file to history
        //! \params _path string to add
        //
        public void add( string _path, DateTime _date = default(DateTime) )
        {
            string lowerPath = _path.ToLower();
            lowerPath.Replace('\\', '/');
            lowerPath.Replace("//", "/");
            Entry result = m_list.Find(delegate(Entry _find) { return _find.m_name.CompareTo(lowerPath) == 0; });
            if (result != null)
            {
                result.m_date = DateTime.Now;
                return;
            }

            result = new Entry();
            result.m_name = lowerPath;
            result.m_date = (_date == default(DateTime)) ? DateTime.Now : _date;
            
            m_list.Add(result);
            checkFiles();
            checkCount();
            m_list.Sort(delegate(Entry _history1, Entry _history2) { return _history1.m_name.CompareTo(_history2.m_name); });

        }

        //-----------------------------------------------------------------------------------------
        //! get number of history files
        //
        public int getCount() 
        { 
            return m_list.Count();  
        }

        //-----------------------------------------------------------------------------------------
        //! get file name of history at given index
        //! \params _index zero based index of wanted history file
        //
        public string getPath( int _index )
        {
            if (_index < 0 || _index >= m_list.Count()) return null;
            return m_list[_index].m_name;
        }

        //-----------------------------------------------------------------------------------------
        //! get file name of history at given index
        //! \params _index zero based index of wanted history file
        //
        public DateTime getDate(int _index)
        {
            if (_index < 0 || _index >= m_list.Count()) return DateTime.Now;
            return m_list[_index].m_date;
        }

        //-----------------------------------------------------------------------------------------
        //! convert history file list to an history file array (needed for serialization)
        //
        public Entry[] toHistoryFileArray()
        {
            if (getCount() == 0) return null;

            Entry[] history = new Entry[getCount()];
            for (int i = 0; i < getCount(); i++) history[i] = m_list[i];
            return history;
        }

        //-----------------------------------------------------------------------------------------
        //! convert history file array to history file list (needed for de-serialization)
        //! \params _historyFileArray array of history file
        //
        public void fromHistoryFileArray(ref Entry[] _historyFileArray)
        {
            m_list.Clear();
            for (int i = 0; i < _historyFileArray.Count(); i++)
                m_list.Add( _historyFileArray[i] );
            checkFiles();
            checkCount();
        }

        //-----------------------------------------------------------------------------------------
        //! check that files exists, remove invalid file from list
        //
        private void checkFiles()
        {
            for (int i = 0; i < getCount();) 
            {
                if (!File.Exists( m_list[i].m_name ))
                    m_list.RemoveAt( i );
                else i++;
            }
        }

        //-----------------------------------------------------------------------------------------
        //! check that number of history file do not exceed c_maxFiles
        //! remove older element if needed
        //
        private void checkCount()
        {
            while (getCount() > c_maxFiles)
            {
                DateTime olderFileTime = DateTime.Now;
                int olderFileIndex = -1;

                for (int i = 0; i < getCount(); i++)
                {
                    if (m_list[i].m_date >= olderFileTime) continue;
                    olderFileTime = m_list[i].m_date;
                    olderFileIndex = i;
                }

                if (olderFileIndex == -1) break;
                m_list.RemoveAt(olderFileIndex);
            }
        }

        //-----------------------------------------------------------------------------------------
        //! sort file by date
        //
        public void sortByDate()
        {
            m_list.Sort(delegate(Entry h1, Entry h2) { return h2.m_date.CompareTo(h1.m_date); });
        }
    }
}
