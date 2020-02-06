using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ConvertDocXToHtml
{
    public class Config
    {
        public Config() { }



        //=========================================================================================
        // data
        //
        public bool SourceIsFolder = true;
        public string SourceFilename = "";
        public string SourceFolder = "";
        public string TargetFolder = "";

        //=========================================================================================
        // main window placement
        //
        public winFunctions.winPlacement MainWindowPlacement;

        public void MainWindowPlacementToUI( IntPtr _handle )
        {
            if (winFunctions.winPlacementIsValid(MainWindowPlacement))
                winFunctions.winSetPlacement(_handle, MainWindowPlacement);
        }

        public void MainWindowPlacementFromUI(IntPtr _handle )
        {
            MainWindowPlacement = winFunctions.winGetPlacement(_handle);
        }
        
        //=========================================================================================
        // File functions : save / load
        //

        [XmlIgnore]
        public static string Name { get { return "ConvertDocXToHtml.cfg"; } }

        //-----------------------------------------------------------------------------------------
        //méthode Save        
        public void Save()
        {
            BeforeSave();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                using (TextWriter writer = new StreamWriter(Name))
                {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception while saving config file : \n    " + Name + "\n" + e.Message);
            }
        }

        //-----------------------------------------------------------------------------------------
        //méthode load
        public static Config Load()
        {
            if (File.Exists(Name))
            {
                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(Config));
                    TextReader reader = new StreamReader(Name);
                    object obj = deserializer.Deserialize(reader);
                    reader.Close();
                    (obj as Config).AfterLoad();
                    return obj as Config;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Exception while loading config file : \n    " + Name + "\n" + e.Message);
                }
            }
            Config result = new Config();
            result.AfterLoad();
            return result;
        }

        //-----------------------------------------------------------------------------------------
        public void BeforeSave()
        {
        }

        //-----------------------------------------------------------------------------------------
        public void AfterLoad()
        {
        }

    }
}
