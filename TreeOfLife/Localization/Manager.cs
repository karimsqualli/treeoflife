using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Linq;

namespace TreeOfLife.Localization
{
    public static class Manager
    {
        static Manager()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            string path = TaxonUtils.GetConfigFilePath();
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                string prefix = Path.GetFileNameWithoutExtension(TaxonUtils.GetConfigFileName("language_")).ToLower();
                foreach ( string f in files )
                {
                    string fname = Path.GetFileNameWithoutExtension(f).ToLower();
                    if (fname.StartsWith(prefix))
                    {
                        fname = fname.Replace(prefix, "");
                        if (fname.Length == 2) Languages.Add(fname);
                    }
                }
            }
            Languages.Load();
            Languages.Save();
            CurrentLanguage = Languages.GetDefault();
            IsDirty = false;
            StartWatcher();
        }

        public static Languages Languages = new Languages();

        public static event EventHandler OnCurrentLanguageChanged = null;

        private static string _CurrentLanguage;
        public static string CurrentLanguage
        {
            get { return _CurrentLanguage; }
            set
            {
                if (CurrentLanguage == value) return;
                if (!Languages.Exists(value)) Languages.Add(value, false);
                _CurrentLanguage = value;
                Load();
                OnCurrentLanguageChanged?.Invoke(null, EventArgs.Empty);
            }
        }


        static Dictionary<string, string> _Dico = new Dictionary<string, string>();

        static public bool IsDirty { get; private set; }

        static private bool _DebugMode = false;
        static public bool DebugMode
        {
            get { return _DebugMode; }
            set
            {
                _DebugMode = value;
                OnCurrentLanguageChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static string Get( string _key, string _value = null)
        {
            if (DebugMode)
            {
                _key = _key.Replace("(*)", "");
                if (_value != null) _value = _value.Replace("(*)", "");
            }

            _Dico.TryGetValue(_key, out string value );
            if (value == null)
            {
                if (_value == null) return null;
                Register(_key, _value);
                value = _value;
            }
            
            if (DebugMode) return "(*)" + value;
            return value;
        }

        public static string Get( string _key, string _value, params object[] args)
        {
            string format = Get(_key, _value);
            return string.Format(format, args);
        }

        public static void Register( string _id, string _name)
        {
            if (_Dico.ContainsKey(_id)) 
                Console.WriteLine(_id + " already in dico = " + _Dico[_id] );
            _Dico[_id] = _name;
            IsDirty = true;
        }

        public static bool IsLocalizable( string _text )
        {
            if (string.IsNullOrWhiteSpace(_text)) return false;
            if (Regex.IsMatch(_text, @"^\d+$")) return false;
            return true;
        }
        
        //=========================================================================================
        // Menus
        //

       //---------------------------------------------------------------------------------
        public static void DoMenu( ToolStrip _menu )
        {
            if (_menu == null) return;
            string key = _menu.Name;
            foreach (var item in _menu.Items)
                DoMenuItem(key, item as ToolStripItem);
        }

        //---------------------------------------------------------------------------------
        public static void DoMenuItem(string _ownerKey, ToolStripItem _menuItem)
        {
            if (_menuItem == null) return;
            if (_menuItem is ToolStripSeparator) return;

            string key = _ownerKey + "." + _menuItem.Name;
            _menuItem.Text = Get(key, _menuItem.Text);

            if (_menuItem is ToolStripDropDownItem)
                foreach (ToolStripItem item in (_menuItem as ToolStripDropDownItem).DropDownItems)
                    DoMenuItem(key, item);
        }

        //=========================================================================================
        // Watcher
        //

        static FileSystemWatcher _Watcher = null;

        private static void StartWatcher()
        {
            _Watcher = new FileSystemWatcher();
            _Watcher.Path = TaxonUtils.GetConfigFilePath();
            _Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _Watcher.Filter = "*.xml";
            _Watcher.IncludeSubdirectories = false;
            _Watcher.Changed += new FileSystemEventHandler(OnChanged);
            _Watcher.EnableRaisingEvents = true;
        }

        public static void StopWatcher()
        {
            _Watcher.EnableRaisingEvents = false;
            _Watcher.Dispose();
            _Watcher = null;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.FullPath.ToLower() == Language.GetFilename(CurrentLanguage).ToLower())
                TaxonUtils.MainWindow.Invoke(new Action(() => OnChangedDelegate(e)));
        }

        static void OnChangedDelegate(FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            Load();
            OnCurrentLanguageChanged?.Invoke(null, EventArgs.Empty);
        }


        //=========================================================================================
        // Save / Load
        //

        //---------------------------------------------------------------------------------
        public static void Save()
        {
            if (CurrentLanguage == null) return;
            if (!IsDirty) return;
            IsDirty = false;
            SaveLanguage(CurrentLanguage, _Dico);
        }
        
        //---------------------------------------------------------------------------------
        public static void SaveLanguage(string _lang, Dictionary<string,string> _dico)
        {
            List<XmlPair> list = new List<XmlPair>();
            foreach (var pair in _dico) list.Add(new XmlPair() { Key = pair.Key, Value = pair.Value });
            list.Sort((x, y) => { return x.Key.CompareTo(y.Key); });
            XmlSerializer serializer = new XmlSerializer(typeof(List<XmlPair>));
            using (TextWriter writer = new StreamWriter(Language.GetFilename(_lang)))
            {
                serializer.Serialize(writer, list);
            }
        }

        //---------------------------------------------------------------------------------
        public static void Load()
        {
            Dictionary<string, string> result = LoadLanguage(CurrentLanguage);
            if (result != null) _Dico = result;
            IsDirty = false;
        }

        //---------------------------------------------------------------------------------
        public static Dictionary<string, string> LoadLanguage( string _lang )
        {
            if (_lang  == null) return null;
            string filename = Language.GetFilename(_lang);
            if (!File.Exists(filename)) return null;
            
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<XmlPair>));
                TextReader reader = new StreamReader(filename);
                object obj = deserializer.Deserialize(reader);
                reader.Close();
                List<XmlPair> list = obj as List<XmlPair>;
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (XmlPair pair in list) result[pair.Key] = pair.Value;
                return result;
            }
            catch (Exception e)
            {
                Loggers.WriteError(LogTags.Congif, "Exception while loading language file : \n    " + filename + "\n" + e.Message);
                return null;
            }
        }

        //=========================================================================================
        // Export / Import
        //

        public static void ExportAllLanguages()
        {
            Dictionary<Language, Dictionary<string, string>> allText = new Dictionary<Language, Dictionary<string, string>>();
            foreach (var lang in Languages.GetAvailableLanguages())
            {
                Dictionary<string, string> result = LoadLanguage(lang.Iso);
                if (result != null) allText[lang] = result;
            }

            // get all keys
            Dictionary<string, bool> allKeys = new Dictionary<string, bool>();
            foreach (var perLang in allText)
            {
                foreach (string key in perLang.Value.Keys)
                    allKeys[key] = true;
            }
            List<string> listKeys = allKeys.Keys.ToList();
            listKeys.Sort();

            string exportFile = Path.Combine(TaxonUtils.GetConfigFilePath(), "AllTexts.csv");
            using (TextWriter writer = new StreamWriter(exportFile))
            {
                string headerLine = "Key;" + string.Join(";", allText.Keys);
                writer.WriteLine(headerLine);

                foreach (string key in listKeys)
                {
                    string line = key;
                    foreach (var dico in allText.Values)
                    {
                        line += ";";
                        if (dico.TryGetValue(key, out string value))
                            line += value;
                    }
                    writer.WriteLine(line);
                }
            }
        }

        public static void ImportAllLanguages()
        {
            Regex isoRegex = new Regex("\\((.*)\\)");
            using (TextReader reader = new StreamReader(Path.Combine(TaxonUtils.GetConfigFilePath(), "AllTexts.csv")))
            {
                // reade head line
                string line = reader.ReadLine();
                if (line == null) return;
                string[] parts = line.Split(';');
                int numColumns = parts.Length;
                if (numColumns <= 1) return;
                if (parts[0].Trim().ToLower() != "key") return;

                List<Tuple<Language, Dictionary<string, string>>> allText = new List<Tuple<Language, Dictionary<string, string>>>();
                for (int i = 1; i < numColumns; i++)
                {
                    Match isoMatch = isoRegex.Match(parts[i]);
                    if (!isoMatch.Success || isoMatch.Groups.Count < 2) return;
                    string iso = isoMatch.Groups[1].Value;
                    Language lang = Languages.Get(iso, true);
                    allText.Add(new Tuple<Language, Dictionary<string, string>>(lang, new Dictionary<string, string>() ));
                }

                Dictionary<string, bool> keyDone = new Dictionary<string, bool>();
                while ( (line = reader.ReadLine()) != null)
                {
                    parts = line.Split(';');
                    if (parts.Length != numColumns) continue;
                    if (keyDone.ContainsKey(parts[0])) continue;
                    keyDone[parts[0]] = true;
                    for (int i = 1; i < numColumns; i++)
                    {
                        if (!String.IsNullOrEmpty(parts[i]))
                            allText[i - 1].Item2[parts[0]] = parts[i];
                    }
                }

                foreach( var data in allText)
                    SaveLanguage(data.Item1.Iso, data.Item2);
            }
        }
    }

    public class XmlPair
    {
        [XmlAttribute]
        public string Key { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
        public override string ToString() { return Key + " = " + Value; }
    }
}
