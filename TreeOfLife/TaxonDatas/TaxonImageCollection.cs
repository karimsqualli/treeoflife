using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace TreeOfLife
{
    //=========================================================================================
    // String extension to get stable hash code
    //
    public static class StringExtensionMethods
    {
        public static int GetStableHashCode(this string str)
        {
            unchecked
            {
                int hash1 = 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }

    //=========================================================================================
    public class ImageLink
    {
        [XmlAttribute]
        public string Key { get; set; }
        [XmlAttribute]
        public string Link { get; set; }
    }

    public class ImagesLinks : List<ImageLink>
    {
        public ImagesLinks() { }
        [XmlIgnore]
        public string Filename;
        [XmlIgnore]
        public int Id;
    }


    //=========================================================================================
    // Image collection
    //class TaxonImageCollection
    public class ImageCollection
    {
        //-----------------------------------------------------------------------------------------
        public string Name = "";
        public string Path = "";
        public int Id = 0;
        public string Desc = "";
        public string Location = "";

        private bool _UseIt = true;
        public bool UseIt
        {
            get { return _UseIt; }
            set { _UseIt = value; }
        }

        public bool IsDefault = false;

        //-----------------------------------------------------------------------------------------
        public ImageCollection() { }

        //-----------------------------------------------------------------------------------------
        public ImageCollection(string _fatherPath, string _name)
        {
            Name = _name;
            Path = _fatherPath;
            Id = _name.GetStableHashCode();
            UseIt = true;
            LoadInfos();
            ConvertCsvs();
            LoadLinks();
            LoadDistantReferences();
        }

        public object TAXON_NAME_KEY { get; } = "taxon";
        public object TAXON_IMAGE_KEY { get; } = "images";
        public object IMAGE_ID_KEY { get; } = "id";
        public object IMAGE_FILE_KEY { get; } = "file";

        private void LoadDistantReferences()
        {
            if (! IsDistant())
            {
                return;
            }
            try
            {
                using (WebClient client = new WebClient())
                {
                    string collection = string.Empty;

                    collection = client.DownloadString(Location);
                    
                    JArray taxons = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(collection);

                    foreach (JObject taxon in taxons)
                    {
                        string taxonName = (string)taxon[TAXON_NAME_KEY];
                        foreach (JObject image in taxon[TAXON_IMAGE_KEY])
                        {
                            int id = (int)image[IMAGE_ID_KEY];
                            string path = (string)image[IMAGE_FILE_KEY];
                            _DistantReferences.Add(new DistantReference(id, taxonName, path));
                        }
                    }
                }

            }
            catch (WebException e)
            {
                Loggers.WriteError(LogTags.Image, "Unable to fetch image collection from :\n" + Location + "\n" + e.Message);
            }
        }

        public int NumberOfDistantReferences()
        {
            return _DistantReferences.Count;
        }

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
            string filename = System.IO.Path.Combine(Path, "_infos.xml");
            TreeOfLife.Helpers.FileSystem.EnsureReadOnlyFlagIsOff(filename);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ImageCollection));
                using (TextWriter writer = new StreamWriter(filename))
                {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception e)
            {
                Loggers.WriteWarning(LogTags.Collection, "Exception while saving file : \n    " + filename + "\n" + e.Message);
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
                    XmlSerializer deserializer = new XmlSerializer(typeof(ImageCollection));
                    using (TextReader reader = new StreamReader(filename))
                    {
                        ImageCollection obj = deserializer.Deserialize(reader) as ImageCollection;
                        
                        if (obj != null && obj.Id == Id)
                        {
                            UseIt = obj.UseIt;
                            IsDefault = obj.IsDefault;
                            Desc = obj.Desc;
                            Location = obj.Location;
                        }
                    }
                }
                catch (Exception e)
                {
                    Loggers.WriteError(LogTags.Collection, "Exception while loading file : \n    " + filename + "\n" + e.Message);
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        [XmlIgnore]
        public Dictionary<int, ImagesLinks> AllLinks { get { return _AllLinks; } }
        Dictionary<int, ImagesLinks> _AllLinks = new Dictionary<int, ImagesLinks>();

        [XmlIgnore]
        public List<DistantReference> DistantReferences { get { return _DistantReferences; } }
        List<DistantReference> _DistantReferences = new List<DistantReference>();


        void LoadLinks()
        {
            _AllLinks.Clear();
            string[] xmlFiles = Directory.GetFiles(Path, "*.xml");
            foreach (string xml in xmlFiles)
            {
                if (System.IO.Path.GetFileName(xml).ToLower().StartsWith("_infos"))
                    continue;

                XmlSerializer deserializer = new XmlSerializer(typeof(ImagesLinks));
                TextReader reader = new StreamReader(xml);
                try
                {
                    object obj = deserializer.Deserialize(reader);
                    reader.Close();
                    ImagesLinks list = obj as ImagesLinks;
                    list.Filename = xml;
                    list.Id = list.Filename.GetStableHashCode();
                    _AllLinks[list.Id] = list;
                }
                catch { }
            }
        }

        //-----------------------------------------------------------------------------------------
        public int NumberOfLinks()
        {
            int result = 0;
            foreach (var pair in _AllLinks)
                result += pair.Value.Count;
            return result;
        }

        //-----------------------------------------------------------------------------------------
        void ConvertCsvs()
        { 
            string[] csvFiles = Directory.GetFiles(Path, "*.csv");
            foreach (string csv in csvFiles)
            {
                string xml = System.IO.Path.ChangeExtension(csv, ".xml");
                if (File.Exists(xml))
                {
                    if (File.GetLastWriteTime(xml).CompareTo(File.GetLastWriteTime(csv)) >= 0)
                        continue;
                    File.Delete(xml);
                }
                ConvertCsvToXml(csv);
            }
        }

        //-----------------------------------------------------------------------------------------
        void ConvertCsvToXml(string _path)
        {
            ImagesLinks datas = new ImagesLinks();

            using (StreamReader tr = new StreamReader(_path))
            {
                string line = tr.ReadLine()?.Trim();
                //while ((line = tr.ReadLine()) != null)
                while ( line != null)
                {
                    string nextLine = tr.ReadLine()?.Trim();
                    while (nextLine != null && nextLine.StartsWith("http"))
                    {
                        line = line + nextLine;
                        nextLine = tr.ReadLine()?.Trim();
                    }

                    string[] parts = line.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[0]) ) continue;
                    string name = parts[0].Trim(" \t\"".ToCharArray());

                    for (int i = 1; i < parts.Length; i++)
                    {
                        string url = parts[i].Trim(" \t\"".ToCharArray());
                        if (!string.IsNullOrEmpty(url) && url.Substring(0,4).ToLower() == "http")
                            datas.Add(new ImageLink() { Key = name, Link = url });
                    }

                    /*if (parts[1].ToLower().StartsWith("http:"))
                        parts[1] = parts[1].Remove(0, 5);
                    if (parts[1].StartsWith("\""))
                    {
                        parts[1] = parts[1].Remove(0, 1);
                        int index = parts[1].IndexOf("\"");
                        if (index != -1) parts[1] = parts[1].Remove(index);
                    }
                    datas.Add(new ImageLink() { Key = parts[0], Link = parts[1] });*/

                    line = nextLine;
                }
            }

            string xml = System.IO.Path.ChangeExtension(_path, ".xml");
            XmlSerializer serializer = new XmlSerializer(typeof(ImagesLinks));
            using (TextWriter writer = new StreamWriter(xml))
            {
                serializer.Serialize(writer, datas);
            }
        }

        //==========================================================================================
        // Static method

        //-----------------------------------------------------------------------------------------
        public static Dictionary<int, ImageCollection> BuildDictionary(string _folder)
        {
            try
            {
                Dictionary<int, ImageCollection> result = new Dictionary<int, ImageCollection>();
                if (!Directory.Exists(_folder))
                    return result;
                string[] listSub = Directory.GetDirectories(_folder);
                foreach (string s in listSub)
                {
                    Console.WriteLine(s);
                    ImageCollection collection = new ImageCollection(s, System.IO.Path.GetFileName(s));
                    if (result.ContainsKey(collection.Id))
                        Loggers.WriteError(LogTags.Image, "Collection with same ID, should not happen !!");
                    result[collection.Id] = collection;
                }
                return result;
            }
            catch (Exception) { }
            return null;
        }

        //-----------------------------------------------------------------------------------------
        public static void SaveInfos(Dictionary<int, ImageCollection> _collections)
        {
            if (_collections == null) return;
            foreach (ImageCollection collection in _collections.Values)
                collection.SaveInfos();
        }

        internal string getDistantImageLink(string _taxonName, int _index)
        {
            if (! IsDistant())
            {
                return null;
            }

            string link = Location + "/";
            link += _taxonName + "/";
            link += _index;

            return link;
        }
    }

    public class DistantReference
    {
        public int Index;
        public string TaxonName;
        public string ImageFile;

        public DistantReference(int _index, string _taxonName, string _imageFile)
        {
            Index = _index;
            TaxonName = _taxonName;
            ImageFile = _imageFile;
        }
    }
}
