using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TreeOfLife.Localization
{
    public class Language
    {
        [XmlAttribute]
        public string Iso;
        [XmlAttribute]
        public string Name;
        [XmlIgnore]
        public bool Exists;

        public string GetFilename() { return GetFilename(Iso); }
        public static string GetFilename( string _iso ) {  return TaxonUtils.GetConfigFileName("language_" + _iso); }

        public override string ToString()
        {
            return Name + " (" + Iso + ")";
        }
    }

    public class Languages
    {
        Dictionary<string, Language> _Dico = new Dictionary<string, Language>();

        public void Add( string _iso, bool _exists = true )
        {
            if (_Dico.ContainsKey(_iso)) return;
            _Dico[_iso] = new Language() { Iso = _iso, Name = _iso, Exists = _exists };
        }

        public bool Exists( string _iso )
        {
            return _Dico.ContainsKey(_iso);
        }

        public Language Get( string _iso, bool _createIfInexistent )
        {
            if (!Exists(_iso)) Add(_iso, false);
            return _Dico[_iso];
        }

        public List<Language> GetAvailableLanguages()
        {
            //return _Dico.Where(p => p.Value.Exists).Select(p => p.Value).ToList();
            return _Dico.Values.ToList();
        }


        public string GetDefault()
        {
            if (Exists("en")) return "en";
            foreach (var pair in _Dico) return pair.Key;
            return "en";
        }

        public void Save()
        {
            string filename = TaxonUtils.GetConfigFileName("language_desc");

            try
            {
                string filepath = Path.GetDirectoryName(filename);
                if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);

                List<Language> list = new List<Language>();
                foreach (var pair in _Dico) list.Add(pair.Value);
                list.Sort((x, y) => { return x.Iso.CompareTo(y.Iso); });
                XmlSerializer serializer = new XmlSerializer(typeof(List<Language>));
                using (TextWriter writer = new StreamWriter(filename))
                {
                    serializer.Serialize(writer, list);
                }
            }
            catch (Exception e)
            {
                Loggers.WriteError(LogTags.Congif, "Exception while saving language file : \n    " + filename + "\n" + e.Message);
            }
        }

        public void Load()
        {
            string filename = TaxonUtils.GetConfigFileName("language_desc");
            if (File.Exists(filename))
            {
                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<Language>));
                    TextReader reader = new StreamReader(filename);
                    object obj = deserializer.Deserialize(reader);
                    reader.Close();
                    List<Language> list = obj as List<Language>;
                    foreach (Language lang in list)
                    {
                        if (!_Dico.ContainsKey( lang.Iso))
                            Add(lang.Iso, false);
                        _Dico[lang.Iso].Name = lang.Name;
                    }
                }
                catch (Exception e)
                {
                    Loggers.WriteError(LogTags.Congif, "Exception while loading language file : \n    " + filename + "\n" + e.Message);
                }
            }
        }
    }
}
