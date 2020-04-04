using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TreeOfLife
{
    //=========================================================================================
    // Comments collection
    public class CommentsCollection
    {
        //-----------------------------------------------------------------------------------------
        public string Name = "";
        public string Path = "";
        public string Desc = "";
        public int PriorityOrder = -1;
        public string Location = "";

        [XmlIgnore]
        public Dictionary<string, string> DistantReferences = new Dictionary<string, string>();

        [XmlIgnore]
        public bool UseIt
        {
            get { return PriorityOrder != -1; }
            set { PriorityOrder = (value) ? int.MaxValue : -1; }
        }

        //-----------------------------------------------------------------------------------------
        public CommentsCollection() { }

        //-----------------------------------------------------------------------------------------
        public CommentsCollection(string _fatherPath, string _name)
        {
            Name = _name;
            Path = _fatherPath;
            PriorityOrder = -1;
            LoadInfos();
            Console.WriteLine("FOO : " + Name + " " + Location);
            LoadDistantReference();
        }

        private void LoadDistantReference()
        {
            if (!IsDistant())
            {
                return;
            }

            using (WebClient client = new WebClient())
            {
                string collection = string.Empty;

                collection = client.DownloadString(Location);

                JArray entries = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(collection);

                foreach (JObject entry in entries)
                {
                    string taxonName = (string) entry["taxon"];
                    string path = (string)entry["file"];

                    DistantReferences.Add(taxonName, path);
                    Console.WriteLine(taxonName + " " + path);
                }
            }
        }
                //-----------------------------------------------------------------------------------------
        public bool IsDistant()
        {
            return Location != "";
        }

        //-----------------------------------------------------------------------------------------
        public override string ToString()
        {
            return Name;
        }

        //-----------------------------------------------------------------------------------------
        // Save  infos     
        public void SaveInfos()
        {
            string filename = System.IO.Path.Combine( Path,"_infos.xml");
            try
            {
                TreeOfLife.Helpers.FileSystem.EnsureReadOnlyFlagIsOff(filename);
                XmlSerializer serializer = new XmlSerializer(typeof(CommentsCollection));
                using (TextWriter writer = new StreamWriter(filename))
                {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception e)
            {
                Loggers.WriteError(LogTags.Comment, "Exception while saving file : \n    " + filename + "\n" + e.Message);
            }
        }

        //-----------------------------------------------------------------------------------------
        // load infos
        public void LoadInfos()
        {
            string filename = System.IO.Path.Combine(Path, "_infos.xml");
            if (File.Exists(filename))
            {
                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(CommentsCollection));
                    using (TextReader reader = new StreamReader(filename))
                    {
                        if (deserializer.Deserialize(reader) is CommentsCollection obj && obj.Name == Name)
                        {
                            PriorityOrder = obj.PriorityOrder;
                            Desc = obj.Desc;
                            Location = obj.Location;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Exception while loading config file : \n    " + filename + "\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Loggers.WriteError(LogTags.Comment, "Exception while loading file : \n    " + filename + "\n" + e.Message);
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        public static List<CommentsCollection> BuildList(string _folder)
        {
            try
            {
                List<CommentsCollection> result = new List<CommentsCollection>();
                
                List<string> listSub = new List<string>();
                if (Directory.Exists(_folder))
                    listSub.AddRange( Directory.GetDirectories(_folder).ToList() );

                //if (Directory.Exists(TaxonUtils.GetCommentDirectory()))
                //    listSub.Add(TaxonUtils.GetCommentDirectory());

                foreach (string s in listSub)
                {
                    string subFolder = s.Split(System.IO.Path.DirectorySeparatorChar).Last();
                    if (subFolder.StartsWith("_")) continue;
                    CommentsCollection collection = new CommentsCollection(s, System.IO.Path.GetFileName(s));
                    result.Add(collection);
                }

                SortList(result);
                return result;
            }
            catch (Exception) { }
            return null;
        }

        //-----------------------------------------------------------------------------------------
        public static void SortList(List<CommentsCollection> _list)
        {
            Comparison<CommentsCollection> c = delegate(CommentsCollection x, CommentsCollection y)
            { 
                if (x.PriorityOrder == -1 && y.PriorityOrder == -1) return x.Name.CompareTo(y.Name);
                if (x.PriorityOrder == -1) return 1;
                if (y.PriorityOrder == -1) return -1;
                return x.PriorityOrder.CompareTo(y.PriorityOrder);
            };
            _list.Sort( c );
            UpdatePriorityOrder(_list);
        }

        //-----------------------------------------------------------------------------------------
        public static void UpdatePriorityOrder(List<CommentsCollection> _list)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].PriorityOrder == -1) break;
                _list[i].PriorityOrder = i;
            }
        }

        //-----------------------------------------------------------------------------------------
        public static List<CommentsCollection> Available(List<CommentsCollection> _all)
        {
            List<CommentsCollection> list = new List<CommentsCollection>();
            SortList(_all);
            foreach (CommentsCollection c in _all )
            {
                if (c.PriorityOrder == -1) break;
                list.Add( c );
            }
            return list;
        }

        //-----------------------------------------------------------------------------------------
        public static void SaveInfos(List<CommentsCollection> _collections)
        {
            if (_collections == null) return;
            foreach (CommentsCollection collection in _collections)
                collection.SaveInfos();
        }

        internal int NumberOfDistantReferences()
        {
            return DistantReferences.Count;
        }
    }
}

