using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace VinceToolbox
{
    public class xmlFunctions
    {
        //-----------------------------------------------------------------------------------------
        static public string xmlHeader()
        {
            return "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>";
        }

        //-----------------------------------------------------------------------------------------
        static public string loadString(XmlNode _xmlNode, string _attribute, string _default)
        {
            XmlAttribute atr = _xmlNode.Attributes[_attribute];
            return (atr == null) ? _default : atr.Value;
        }

        //-----------------------------------------------------------------------------------------
        static public string loadString(XmlDocument _xml, string _xmlPath, string _attribute, string _default)
        {
            XmlNode node = _xml.SelectSingleNode(_xmlPath);
            if (node == null) return _default;
            XmlAttribute atr = node.Attributes[_attribute];
            return (atr == null) ? _default : atr.Value;
        }

        //-----------------------------------------------------------------------------------------
        static public string loadString(XmlNode _xmlNode, string _xmlPath, string _attribute, string _default)
        {
            XmlNode node = _xmlPath == "" ? _xmlNode : _xmlNode.SelectSingleNode(_xmlPath);
            if (node == null) return _default;
            XmlAttribute atr = node.Attributes[_attribute];
            return (atr == null) ? _default : atr.Value;
        }

        //-----------------------------------------------------------------------------------------
        static public bool loadBool(XmlNode _xmlNode, string _attribute, bool _default)
        {
            return Convert.ToBoolean(loadString(_xmlNode, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public bool loadBool(XmlNode _xmlNode, string _xmlPath, string _attribute, bool _default)
        {
            return Convert.ToBoolean(loadString(_xmlNode, _xmlPath, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public bool loadBool(XmlDocument _xml, string _xmlPath, string _attribute, bool _default)
        {
            return Convert.ToBoolean(loadString(_xml, _xmlPath, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public int loadInt(XmlNode _xmlNode, string _attribute, int _default)
        {
            return Convert.ToInt32(loadString(_xmlNode, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public int loadInt(XmlNode _xmlNode, string _xmlPath, string _attribute, int _default)
        {
            return Convert.ToInt32(loadString(_xmlNode, _xmlPath, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public int loadInt(XmlDocument _xml, string _xmlPath, string _attribute, int _default)
        {
            return Convert.ToInt32(loadString(_xml, _xmlPath, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public float loadFloat(XmlNode _xmlNode, string _attribute, float _default)
        {
            return Convert.ToSingle(loadString(_xmlNode, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public float loadFloat(XmlNode _xmlNode, string _xmlPath, string _attribute, float _default)
        {
            return Convert.ToSingle(loadString(_xmlNode, _xmlPath, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public float loadFloat(XmlDocument _xml, string _xmlPath, string _attribute, float _default)
        {
            return Convert.ToSingle(loadString(_xml, _xmlPath, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public double loadDouble(XmlNode _xmlNode, string _attribute, double _default)
        {
            return Convert.ToDouble(loadString(_xmlNode, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public double loadDouble(XmlNode _xmlNode, string _xmlPath, string _attribute, double _default)
        {
            return Convert.ToDouble(loadString(_xmlNode, _xmlPath, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        static public double loadDouble(XmlDocument _xml, string _xmlPath, string _attribute, double _default)
        {
            return Convert.ToDouble(loadString(_xml, _xmlPath, _attribute, _default.ToString()));
        }

        //-----------------------------------------------------------------------------------------
        public static T load<T>(string _file, out string _error )
        {
            _error = "";

            if (!File.Exists(_file))
                return default(T);

            TextReader textReader = null;
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                textReader = new StreamReader(_file);
                T result = (T)deserializer.Deserialize(textReader);
                textReader.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                if (textReader != null) textReader.Close(); 
                _error = string.Format("Error loading config file:\n\n    {0}\n\nError:\n    {1}", _file, ex.Message);
                return default(T);
            }
        }

        //-----------------------------------------------------------------------------------------
        public static bool save<T>(T _data, string _file, out string _error)
        {
            _error = "";

            try
            {
                TextWriter textWriter = new StreamWriter(_file);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(textWriter, _data);
                textWriter.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                _error = string.Format("Error saving options file:\n\n    {0}\n\nError:\n    {1}", _file, ex.Message);
                return false;
            }
        }
    }
}
