using System;
using System.IO;
using System.Windows;

namespace VinceToolbox
{
    public class FileWatcher : IDisposable
    {
        //-----------------------------------------------------------------------------------------
        string m_path;
        string m_filter;
        string m_error = "";
        FileSystemWatcher m_watcher = null;

        //-----------------------------------------------------------------------------------------
        FileSystemEventHandler m_onCreated;
        FileSystemEventHandler m_onChanged;
        FileSystemEventHandler m_onDeleted;
        RenamedEventHandler m_onRenamed;

        //-----------------------------------------------------------------------------------------
        public FileWatcher(string _path, string _filter = "*.*")
        {
            m_path = _path;
            m_filter = _filter;
        }

        //-----------------------------------------------------------------------------------------
        public void Dispose()
        {
            if (m_watcher != null) { m_watcher.Dispose(); m_watcher = null; }
        }

        //-----------------------------------------------------------------------------------------
        public void setEventHandler(FileSystemEventHandler _onChanged = null, FileSystemEventHandler _onCreated = null, FileSystemEventHandler _onDeleted = null,  RenamedEventHandler _onRenamed = null)
        {
            m_onCreated = _onCreated;
            m_onChanged = _onChanged;
            m_onDeleted = _onDeleted;
            m_onRenamed = _onRenamed;
        }

        //-----------------------------------------------------------------------------------------
        public bool start()
        {
            if (m_watcher != null) return true;

            m_error = "";
            try
            {
                Directory.CreateDirectory(m_path);

                m_watcher = new FileSystemWatcher();
                m_watcher.Path = m_path;
                m_watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                m_watcher.Filter = m_filter;

                // Add event handlers.
                if (m_onChanged != null) m_watcher.Changed += m_onChanged;
                if (m_onCreated != null) m_watcher.Created += m_onCreated;
                if (m_onDeleted != null) m_watcher.Deleted += m_onDeleted;
                if (m_onRenamed != null) m_watcher.Renamed += m_onRenamed;

                // Begin watching.
                m_watcher.EnableRaisingEvents = true;

                return true;
            }
            catch (System.Exception ex)
            {
                m_error = string.Format("Error creating watcher on {0}\n\nException:\n{1}\n", m_path, ex.Message);
                return false;
            }
            
        }
    }
}
