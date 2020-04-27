using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.IO.Compression;
using TreeOfLife.TaxonDialog;

namespace TreeOfLife
{
    partial class TaxonTreeNode
    {
        //=========================================================================================
        // XML save / load
        //

        //méthode Save        
        public void SaveXML(string _fileName)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TaxonTreeNode));
                using (TextWriter writer = new StreamWriter(_fileName))
                {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception e)
            {
                Loggers.WriteError(LogTags.Data, "Exception while saving " + _fileName + "\n" + e.Message);
            }
        }

        //méthode load
        public static TaxonTreeNode LoadXML(string _fileName)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(TaxonTreeNode));
                TextReader reader = new StreamReader(_fileName);
                object obj = deserializer.Deserialize(reader);
                reader.Close();
                (obj as TaxonTreeNode).UpdateFather();
                (obj as TaxonTreeNode).Visible = true;
                return obj as TaxonTreeNode;
            }
            catch (Exception e)
            {
                string message = "Exception while loading " + _fileName + " : \n\n";
                message += e.Message;
                if (e.InnerException != null)
                    message += "\n" + e.InnerException.Message;
                Loggers.WriteError(LogTags.Data, message);
            }
            return null;
        }

        //=========================================================================================
        // XML save in memory
        //

        public MemoryStream SaveXMLInMemory()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TaxonTreeNode));
                MemoryStream writer = new MemoryStream();
                serializer.Serialize(writer, this);
                writer.Position = 0;
                return writer;
            }
            catch (Exception e)
            {
                Loggers.WriteError(LogTags.Data, "Exception while saving in memory\n" + e.Message);
                return null;
            }
        }

        public static TaxonTreeNode LoadXMLFromMemory(MemoryStream ms)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(TaxonTreeNode));
                object obj = deserializer.Deserialize(ms);
                (obj as TaxonTreeNode).UpdateFather();
                (obj as TaxonTreeNode).Visible = true;
                return obj as TaxonTreeNode;
            }
            catch (Exception e)
            {
                string message = "Exception while loading from memory : \n\n";
                message += e.Message;
                if (e.InnerException != null)
                    message += "\n" + e.InnerException.Message;
                Loggers.WriteError(LogTags.Data, message);
            }
            return null;
        }


        //=========================================================================================
        // binary save / load
        //
        private readonly int SaveBinVersion = 6;

        class SaveBinProgress
        {
            public SaveBinProgress(ProgressItem _piSave )
            {
                PISave = _piSave;
                _Count = 0;
            }

            public readonly ProgressItem PISave;
            int _Count;

            public void Inc()
            {
                if (_Count++ % 1000 == 0)
                    PISave.Update(_Count);
            }
        }

        public void SaveBin(string _fileName)
        {
            TaxonUtils.LockMainWindow();
            try
            {
                using (ProgressDialog progressDlg = new ProgressDialog())
                {
                    progressDlg.StartPosition = FormStartPosition.CenterScreen;
                    progressDlg.Show();

                    ProgressItem piSave = progressDlg.Add("Saving ...", null, 0, Count());
                    using (FileStream fs = File.Create(_fileName, 65536, FileOptions.None))
                    {
                        using (DeflateStream deflateStream = new DeflateStream(fs, CompressionMode.Compress))
                        {
                            using (BinaryWriter w = new BinaryWriter(deflateStream))
                            //using (BinaryWriter w = new BinaryWriter(fs))
                            {
                                w.Write(SaveBinVersion);
                                SaveBin(w, new SaveBinProgress(piSave));
                                w.Close();
                                deflateStream.Close();
                                fs.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Loggers.WriteError(LogTags.Data, "Exception while saving config file : \n    " + _fileName + "\n" + e.Message);
            }
            finally
            {
                TaxonUtils.UnlockMainWindow();
            }
        }

        void SaveBin(BinaryWriter _bw, SaveBinProgress _progress)
        {
            Desc.SaveBin(_bw);
            _progress.Inc();
            _bw.Write(Children.Count);
            foreach (TaxonTreeNode node in Children)
                node.SaveBin(_bw, _progress);
        }

        static TaxonTreeNode LoadBin(string _fileName)
        {
            TaxonTreeNode result = null;

            _LoadCanceled = false;
            LoadCounterInit();

            try
            {
                using (FileStream fs = File.Open(_fileName, FileMode.Open, FileAccess.Read))
                {
                    using (DeflateStream deflateStream = new DeflateStream(fs, CompressionMode.Decompress))
                    {
                        using (BinaryReader r = new BinaryReader(deflateStream))
                        {
                            uint version = r.ReadUInt32();
                            result = LoadBin(r, version);
                            r.Close();
                            deflateStream.Close();
                            fs.Close();
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        static TaxonTreeNode LoadBin(BinaryReader _r, uint _version)
        {
            LoadCounterInc();
            if (_LoadCanceled) return null;
            TaxonTreeNode node = new TaxonTreeNode() { Desc = TaxonDesc.LoadBin(_r, _version) };
            int count = _r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                TaxonTreeNode child = LoadBin(_r, _version);
                if (child == null) return null;
                node.AddChild(child);
            }
            return node;
        }

        //=========================================================================================
        // Newick load
        //

        static TaxonTreeNode ParseNewick(StreamReader sr)
        {
            TaxonTreeNode node = new TaxonTreeNode( new TaxonDesc());
            string name = "";
            
            char ch = (char)sr.Read();
            if (ch == '(')
            {
                while (ch != ')')
                {
                    TaxonTreeNode child = ParseNewick(sr);
                    node.AddChild(child);
                    ch = (char)sr.Read();
                }
            }

            if (ch != ')')
                name += ch;

            ch = (char) sr.Peek();
            while (ch != ',' && ch != ';' && ch != ')')
            {
                sr.Read();
                name += ch;
                ch = (char) sr.Peek();
            }

            name = name.Replace("'", "");
            int index = name.LastIndexOf("_ott");
            if (index != -1)
            {
                string ottString = name.Substring(index + 4);

                UInt32 id;
                if (UInt32.TryParse(ottString, out id))
                {
                    node.Desc.OTTID = id;
                    name = name.Remove(index);
                }
            }

            node.Desc.RefMultiName = new Helpers.MultiName(name.Replace('_', ' '));
            return node;
        }

        static TaxonTreeNode LoadNewick(string _filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(_filename))
                {
                    return ParseNewick(sr);
                }
            }
            catch
            {
                return null;
            }
        }

        //=========================================================================================
        // Jac File load
        //
        static TaxonTreeNode LoadJacFile(string _filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(_filename))
                {
                    TaxonTreeNode root = new TaxonTreeNode( new TaxonDesc("__ignore__") );
                    TaxonTreeNode currentNode = root;
                    int currentTab = 0;
                    int currentLine = 0;

                    string line;
                    // read header line
                    while ((line = sr.ReadLine()) != null)
                    {
                        currentLine++;
                        int tab = 0;
                        while (line.StartsWith("\t")) { tab++; line = line.Substring(1); }
                        if (tab > currentTab)
                        {
                            if (tab - currentTab > 1 || currentNode.Children.Count == 0)
                            {
                                Loggers.WriteError(LogTags.Data, _filename + "(" + currentLine.ToString() + ") too many tabs");
                                return null;
                            }
                            currentTab = tab;
                            currentNode = currentNode.Children[currentNode.Children.Count - 1];
                        }
                        else if (tab < currentTab)
                        {
                            while (currentTab != tab)
                            {
                                if (currentNode == root)
                                {
                                    Loggers.WriteError(LogTags.Data, _filename + "(" + currentLine.ToString() + ") tabulation error");
                                    return null;
                                }
                                currentTab--;
                                currentNode = currentNode.Father;
                            }
                        }

                        string[] parts = line.Split('>');
                        TaxonTreeNode newNode = new TaxonTreeNode(new TaxonDesc()
                        {
                            RefMultiName = new Helpers.MultiName(parts[0]),
                            FrenchMultiName = parts.Length < 2 || String.IsNullOrWhiteSpace(parts[1]) ? null : new Helpers.MultiName(parts[1]),
                            ClassicRank = parts.Length < 3 || String.IsNullOrWhiteSpace(parts[2]) ? ClassicRankEnum.None : ClassicRankEnumExt.FromString(parts[2]),
                            Flags = (parts.Length < 4 || String.IsNullOrWhiteSpace(parts[3])) ? 0 : FlagsEnumExt.FromString(parts[3]),
                            RedListCategory = (parts.Length < 5 || String.IsNullOrWhiteSpace(parts[4])) ? RedListCategoryEnum.NotEvaluated : RedListCategoryExt.FromString(parts[4]),
                        }
                        );
                        currentNode.AddChild(newNode);
                    }

                    return root;
                }
            }
            catch( Exception e )
            {
                Loggers.WriteError(LogTags.Data, "Exception raised while reading file " + _filename + "\n   " + e.Message );
                return null;
            }
        }

        //=========================================================================================
        // txt file
        //
        static TaxonTreeNode LoadTxt(string _filename)
        {
            try
            {
                int ch = 0;
                using (StreamReader sr = new StreamReader(_filename))
                {
                    ch = (int)sr.Read();
                    while (ch != -1)
                    {
                        if (ch == '(') break;
                        if (ch == (int)Keys.Return || ch == '>' ) break;
                        ch = (int)sr.Read();
                    }
                }
                if (ch == '(') return LoadNewick(_filename);
                if (ch == (int)Keys.Return || ch == '>' ) return LoadJacFile(_filename);
                Loggers.WriteError(LogTags.Data, "Cannot recognize file forma : \n    " + _filename);
                return null;
            }
            catch
            {
                return null;
            }
        }

        //=========================================================================================
        // generic function to load
        //
        public static TaxonTreeNode Load(string _fileName)
        {
            if (_fileName == string.Empty) return null;
            DateTime ts_Start = DateTime.Now;
            if (!File.Exists(_fileName)) return null;
            Cursor.Current = Cursors.WaitCursor;
            string extension = Path.GetExtension(_fileName).ToLower();

            if (extension == ".tol")
                FormAbout.SetSplashScreenMessage(".. Loading data ...", false, new Action(() => CancelLoad()));
            else
                FormAbout.SetSplashScreenMessage(".. Loading data ...");

            TaxonTreeNode result = null;
            if (extension == ".xml") result = LoadXML(_fileName);
            if (extension == ".tol") result = LoadBin(_fileName);
            if (extension == ".txt") result = LoadTxt(_fileName);
            Loggers.WriteInformation(LogTags.Data, string.Format("Loading {0} took {1} ms ", _fileName, (int)((DateTime.Now - ts_Start).TotalMilliseconds)));
            if (result != null)
                result.AfterLoad();
            Cursor.Current = Cursors.Default;
            return result;
        }

        static bool _LoadCanceled = false;
        public static void CancelLoad() { _LoadCanceled = true; }
        public static bool LoadHasBeenCanceled() { return _LoadCanceled; }

        static uint _LoadCounter = 0;
        public static void LoadCounterInit() { _LoadCounter = 0; }
        public static void LoadCounterInc()
        {
            _LoadCounter++;
            if ((_LoadCounter % 10000) == 0)
            {
                FormAbout.UpdateSplashScreenMessage(".. Loading data ... " + _LoadCounter.ToString() + " taxons loaded" );
            }
        }

        public void AfterLoad()
        {
            UpdateAvailableImages();
            UpdateRedListCategoryFlags();
        }

        //=========================================================================================
        // Import OTT
        //
        enum ColumnType { uid, parent_uid, name, rank, sourceinfo, uniqname, flags, count }

        public static void AddRank( List<string> _list, string _rank )
        {
            if (_list.Contains(_rank)) return;
            _list.Add(_rank);
        }

        public static void AddSourceInfos(List<string> _list, string _infos )
        {
            _infos = _infos.Trim().ToLower();
            string[] split = _infos.Split(new Char[] { ',' });
            foreach (string source in split)
            {
                string temp = source;
                int index = temp.IndexOf(':');
                if (index != -1) temp = temp.Remove(index);
                if (!_list.Contains(temp))
                    _list.Add(temp);
            }
        }

        public static void AddFlags(List<string> _list, string _flags)
        {
            _flags = _flags.Trim().ToLower();
            string[] split = _flags.Split(new Char[] { ',' });
            foreach (string flag in split)
            {
                if (!_list.Contains(flag))
                    _list.Add(flag);
            }
        }

        public static TaxonTreeNode ImportOTT(string _folder)
        {
            if (!Directory.Exists( _folder )) return null;
            string fileTaxonomy = _folder + Path.DirectorySeparatorChar +"taxonomy.tsv";
            if (!File.Exists(fileTaxonomy)) return null;

            TaxonTreeNode root = null;

            using (StreamReader sr = new StreamReader(fileTaxonomy))
            {
                string line;
                // read header line
                if ((line = sr.ReadLine()) == null)
                    return null;

                List<string> columsHeader = line.Split(new Char[] { '|' }).ToList();
                if (columsHeader.Count < 3) return null;
                int[] ColumnIndex = Enumerable.Repeat(-1, (int) ColumnType.count).ToArray();
                List<string> unknownHeader = new List<string>();
                for (int i = 0; i < columsHeader.Count; i++ )
                {
                    string header = columsHeader[i].Trim().ToLower();
                    if (header == "uid") ColumnIndex[(int)ColumnType.uid] = i;
                    else if (header == "parent_uid") ColumnIndex[(int)ColumnType.parent_uid] = i;
                    else if (header == "name") ColumnIndex[(int)ColumnType.name] = i;
                    else if (header == "rank") ColumnIndex[(int)ColumnType.rank] = i;
                    else if (header == "sourceinfo") ColumnIndex[(int)ColumnType.sourceinfo] = i;
                    else if (header == "uniqname") ColumnIndex[(int)ColumnType.uniqname] = i;
                    else if (header == "flags") ColumnIndex[(int)ColumnType.flags] = i;
                    else unknownHeader.Add(header);
                }

                if (ColumnIndex[(int)ColumnType.uid] == -1) return null;
                if (ColumnIndex[(int)ColumnType.parent_uid] == -1) return null;
                if (ColumnIndex[(int)ColumnType.name] == -1) return null;
                if (ColumnIndex[(int)ColumnType.rank] == -1) return null;

                
                Dictionary<uint, TaxonTreeNode> dico = new Dictionary<uint, TaxonTreeNode>();
                List<string> listRanks = new List<string>();
                List<string> listSourceInfo= new List<string>();
                List<string> listFlags= new List<string>();
                Dictionary<uint, string>  dicoUniqueName = new Dictionary<uint,string>();

                bool first = true;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] columns = line.Split(new Char[] { '|' });
                    if (columns.Length != columsHeader.Count) continue;

                    uint id;
                    if (!uint.TryParse(columns[ColumnIndex[(int)ColumnType.uid]].Trim(), out id))
                        continue;

                    TaxonTreeNode node = new TaxonTreeNode(new TaxonDesc(columns[ColumnIndex[(int)ColumnType.name]].Trim()));
                    node.Desc.OTTID = id;
                    dico[ id ] = node;

                    if (!first)
                    {
                        uint parent_uid;
                        if (!uint.TryParse(columns[ColumnIndex[(int)ColumnType.parent_uid]].Trim(), out parent_uid))
                            continue;

                        if (!dico.ContainsKey(parent_uid))
                            continue;
                        TaxonTreeNode parentNode = dico[ parent_uid ];
                        parentNode.AddChild( node );
                    }
                    else
                    {
                        first = false;
                        root = node;
                    }

                    string rank = columns[ColumnIndex[(int)ColumnType.rank]].Trim().ToLower();
                    if (rank != "")
                    {
                        AddRank(listRanks, rank);
                        node.Desc.ClassicRank = fromOttString(rank);
                    }
                    
                    AddSourceInfos(listSourceInfo, columns[ColumnIndex[(int)ColumnType.sourceinfo]]);
                    AddFlags(listFlags, columns[ColumnIndex[(int)ColumnType.flags]]);
                }

                listRanks.Sort();
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(_folder + "/ranks.txt"))
                {
                    foreach (string rank in listRanks)
                        file.WriteLine(rank);
                }

                listSourceInfo.Sort();
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(_folder + "/sources.txt"))
                {
                    foreach (string source in listSourceInfo)
                        file.WriteLine(source);
                }

                listFlags.Sort();
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(_folder + "/flags.txt"))
                {
                    foreach (string flag in listFlags)
                        file.WriteLine(flag);
                }
            }

            ConvertOttString = null;
            return root;
        }
        
        static Dictionary<string, ClassicRankEnum> ConvertOttString = null;
        static ClassicRankEnum fromOttString(string _rank)
        {
            if (ConvertOttString == null)
            {
                ConvertOttString = new Dictionary<string, ClassicRankEnum>();
                ConvertOttString["domain"] = ClassicRankEnum.Domaine;
                ConvertOttString["family"] = ClassicRankEnum.Famille;
                ConvertOttString["forma"] = ClassicRankEnum.None;
                ConvertOttString["genus"] = ClassicRankEnum.Genre;
                ConvertOttString["infraclass"] = ClassicRankEnum.InfraClasse;
                ConvertOttString["infrakingdom"] = ClassicRankEnum.InfraRegne;
                ConvertOttString["infraorder"] = ClassicRankEnum.InfraOrdre;
                ConvertOttString["infraphylum"] = ClassicRankEnum.None;
                ConvertOttString["infraspecificname"] = ClassicRankEnum.None;
                ConvertOttString["kingdom"] = ClassicRankEnum.Regne;
                ConvertOttString["natio"] = ClassicRankEnum.None;
                ConvertOttString["no rank"] = ClassicRankEnum.None;
                ConvertOttString["no rank - terminal"] = ClassicRankEnum.None;
                ConvertOttString["order"] = ClassicRankEnum.Ordre;
                ConvertOttString["parvorder"] = ClassicRankEnum.None;
                ConvertOttString["phylum"] = ClassicRankEnum.Embranchement;
                ConvertOttString["section"] = ClassicRankEnum.Section;
                ConvertOttString["species"] = ClassicRankEnum.Espece;
                ConvertOttString["species group"] = ClassicRankEnum.Espece;
                ConvertOttString["species subgroup"] = ClassicRankEnum.Espece;
                ConvertOttString["subclass"] = ClassicRankEnum.SousClasse;
                ConvertOttString["subdivision"] = ClassicRankEnum.SousEmbranchement;
                ConvertOttString["subfamily"] = ClassicRankEnum.SousFamille;
                ConvertOttString["subform"] = ClassicRankEnum.SousEspece;
                ConvertOttString["subgenus"] = ClassicRankEnum.SousGenre;
                ConvertOttString["subkingdom"] = ClassicRankEnum.SousRègne;
                ConvertOttString["suborder"] = ClassicRankEnum.SousOrdre;
                ConvertOttString["subphylum"] = ClassicRankEnum.SousEmbranchement;
                ConvertOttString["subsection"] = ClassicRankEnum.None;
                ConvertOttString["subspecies"] = ClassicRankEnum.SousEspece;
                ConvertOttString["subtribe"] = ClassicRankEnum.SousTribu;
                ConvertOttString["subvariety"] = ClassicRankEnum.SousEspece;
                ConvertOttString["superclass"] = ClassicRankEnum.SuperClasse;
                ConvertOttString["superfamily"] = ClassicRankEnum.SuperFamille;
                ConvertOttString["superkingdom"] = ClassicRankEnum.SuperRegne;
                ConvertOttString["superorder"] = ClassicRankEnum.SuperOrdre;
                ConvertOttString["superphylum"] = ClassicRankEnum.SuperEmbranchement;
                ConvertOttString["supertribe"] = ClassicRankEnum.None;
                ConvertOttString["tribe"] = ClassicRankEnum.Tribu;
                ConvertOttString["varietas"] = ClassicRankEnum.SousEspece;
                ConvertOttString["variety"] = ClassicRankEnum.SousEspece;
            }
            if (ConvertOttString.ContainsKey(_rank)) return ConvertOttString[_rank];
            return ClassicRankEnum.None;
        }
    }
}
