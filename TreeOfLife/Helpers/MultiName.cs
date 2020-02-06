using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife.Helpers
{
    public class MultiName : IComparable
    {
        public static string SeparatorAsString = ";";
        public static char[] SeparatorAsCharArray = {';'};

        public MultiName( string _full)
        {
            Full = _full;
            int index = Full.IndexOf(SeparatorAsString );
            HasSynonym = index != -1;
            Main = HasSynonym ? Full.Substring(0, index) : Full;
        }

        public MultiName( List<string> _all) : this(string.Join(SeparatorAsString, _all))
        {
        }

        public override string ToString() { return Full; }

        public readonly string Full;
        public readonly bool HasSynonym;
        public readonly string Main;

        public bool IsUnnamed()
        {
            if (String.IsNullOrEmpty(Full)) return true;
            // unnamed = 7 chars, non nommé = 9 chars
            if (Main.Length >= 9)
            {
                string start = Main.Substring(0, 9).ToLower();
                if (start == "non nommé") return true;
                if (start.StartsWith("unnamed")) return true;
            }
            else if (Main.Length >= 7)
            {
                if (Main.Substring(0, 7).ToLower() == "unnamed") return true;
            }
            return false;
        }

        public string[] GetAll()
        {
            return Full.Split(SeparatorAsCharArray, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] GetSynonymsArray()
        {
            if (!HasSynonym) return null;
            return Full.Substring(Main.Length).Split(SeparatorAsCharArray, StringSplitOptions.RemoveEmptyEntries);
        }

        public string GetSynonymAtCharIndex(int _index)
        {
            while (_index > 0 && Full[_index] != ';') _index--;
            if (Full[_index] != ';') return null;
            _index++;
            int lastIndex = Full.IndexOf(';', _index);
            if (lastIndex == -1) return Full.Substring(_index);
            return Full.Substring(_index, lastIndex - _index);
        }

        public string GetSynonymAtIndex(int _index)
        {
            if (_index < 0) return null;
            string[] synonyms = GetSynonymsArray();
            if (synonyms == null || synonyms.Length <= _index) return null;
            return synonyms[_index];
        }

        public bool ItsOneOfMyNames(string _name)
        {
            _name = _name.Trim().ToLower();
            return Full.Split(SeparatorAsCharArray).ToList().Where(t => t.Trim().ToLower() == _name).FirstOrDefault() != null;
        }

      
        //=========================================================================================
        // comparison interface
        //
        public int CompareTo(object obj)
        {
            if (obj is MultiName) 
                return Full.CompareTo((obj as MultiName).Full);
            if (obj is string)
                return Full.CompareTo(obj as string);
            return 0;
        }
    }
}
