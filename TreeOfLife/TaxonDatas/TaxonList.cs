using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TreeOfLife
{
    public class TaxonList
    {
        private static readonly int _CurrentVersion = 1;
        [XmlIgnore]
        public string FileName { get; set; } = null;

        [XmlIgnore]
        public string Name
        {
            get 
            { 
                if (String.IsNullOrWhiteSpace(FileName)) return null;
                try { return System.IO.Path.GetFileNameWithoutExtension(FileName); } catch { }
                return null;
            }
        }
        [XmlAttribute]
        public bool HasFile { get; set; } = false;
        [XmlAttribute]
        public int Version { get; set; } = _CurrentVersion;

        public List<string> TaxonFullNames { get; set; } = new List<string>();

        public List<TaxonTreeNode> ToTaxonTreeNodeList(TaxonTreeNode _root) 
        {
            if (_root == null) return null;

            List<TaxonTreeNode> nodes = new List<TaxonTreeNode>();
            foreach (string fullname in TaxonFullNames)
            {
                TaxonTreeNode taxon = _root.FindTaxonByFullName(fullname);
                if (taxon != null) nodes.Add(taxon);
            }
            return nodes; 
        }

        public void FromTaxonTreeNodeList(List<TaxonTreeNode> _nodes)
        {
            TaxonFullNames.Clear();
            foreach (TaxonTreeNode taxon in _nodes)
                TaxonFullNames.Add(taxon.GetHierarchicalName());
        }

        public static TaxonList Load( string _filename ) 
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(TaxonList));
                TextReader reader = new StreamReader(_filename);
                object obj = deserializer.Deserialize(reader);
                reader.Close();
                if (obj is TaxonList)
                {
                    TaxonList tl = obj as TaxonList;
                    tl.FileName = _filename;
                    tl.HasFile = true;
                    return tl;
                }
            }
            catch (Exception e)
            {
                string message = "Exception while loading " + _filename + " : \n\n";
                message += e.Message;
                if (e.InnerException != null)
                    message += "\n" + e.InnerException.Message;
                Loggers.WriteError(LogTags.Data, message);
            }
            return null; 
        }

        public bool Save(bool chooseFile = false, FileFilterIndexEnum _preferredFileExt = FileFilterIndexEnum.Xml )
        {
            if (!HasFile)
            {
                string message = "Cannot save that list in a file, it has not the HasFile flag set !";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (chooseFile || String.IsNullOrWhiteSpace(FileName))
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    FileName = FileName,
                    InitialDirectory = GetFolder(),
                    FilterIndex = 2 * (int)_preferredFileExt,
                    Filter = FileFilters,
                    AddExtension = true
                };
                if (sfd.ShowDialog() != DialogResult.OK)
                    return false;
                FileName = sfd.FileName;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TaxonList));
                using (TextWriter writer = new StreamWriter(FileName))
                {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception e)
            {
                Loggers.WriteError(LogTags.Data, "Exception while saving file : \n    " + FileName + "\n" + e.Message);
                return false;
            }
            return true;
        }

        public static string FileFilters = "xml files (*.xml)|*.xml|List of taxons files (*.lot)|*.lot";
        public enum FileFilterIndexEnum { Xml = 0, ListOfTaxons = 1};
        public static FileFilterIndexEnum FilterIndexFromFile(string _file, FileFilterIndexEnum _prefered = FileFilterIndexEnum.Xml)
        {
            if (String.IsNullOrEmpty(_file)) return _prefered;
            string ext = System.IO.Path.GetExtension(_file).ToLower();
            if (ext == ".xml") return FileFilterIndexEnum.Xml;
            if (ext == ".tol") return FileFilterIndexEnum.ListOfTaxons;
            return _prefered;
        }

        public static string GetFolder()
        {
            string folder = Path.Combine( TaxonUtils.GetTaxonPath(), "ListOfTaxons");
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
            return folder;
        }

        //---------------------------------------------------------------------------------
        public class ImportFileResult
        {
            public List<TaxonTreeNode> List;
            public int TaxonsFound = 0;
            public int TaxonNotFound = 0;
            public string LogFilename = null;
        }

        public static ImportFileResult ImportFile(string _filename, TaxonTreeNode _root)
        {
            return ImportFile(_filename, new TaxonSearch(_root, true, true));
        }

        public static ImportFileResult ImportFile(string _filename, TaxonSearch _searchTool, bool _logFoundTaxon = true)
        {
            ImportFileResult result = new ImportFileResult();

            List<string> notFound = new List<string>();
            List<Tuple<string, TaxonTreeNode>> found = new List<Tuple<string, TaxonTreeNode>>();

            using (StreamReader file = new StreamReader(_filename))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    TaxonTreeNode node = _searchTool.FindOne(line);
                    if (node == null)
                        notFound.Add(line);
                    else
                        found.Add(new Tuple<string, TaxonTreeNode>(line, node));
                }
            }

            result.LogFilename = _filename.Replace(".txt", "") + "_import.log";
            using (StreamWriter log = new StreamWriter(result.LogFilename))
            {
                log.WriteLine("Import " + _filename + " results");
                log.WriteLine("");
                log.WriteLine(found.Count + " taxons found");
                log.WriteLine(notFound.Count + " taxons not found");
                log.WriteLine("");
                log.WriteLine("Not found:");
                foreach (string name in notFound)
                    log.WriteLine("    " + name);
                log.WriteLine("");
                if (_logFoundTaxon)
                {
                    log.WriteLine("Found:");
                    foreach (Tuple<string, TaxonTreeNode> tuple in found)
                        log.WriteLine("    " + tuple.Item1 + " ==> " + tuple.Item2.GetHierarchicalName());
                }
                log.WriteLine("");
            }

            result.TaxonsFound = found.Count;
            result.TaxonNotFound = notFound.Count;
            Dictionary<TaxonTreeNode, bool> dico = new Dictionary<TaxonTreeNode, bool>();
            foreach (Tuple<string, TaxonTreeNode> tuple in found)
            {
                if (dico.ContainsKey(tuple.Item2)) continue;
                dico[tuple.Item2] = true;
            }
            result.List = dico.Keys.ToList();
            return result;
        }
    }
}
