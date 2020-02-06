using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace TreeOfLife
{
    public class TaxonDesc : IComparable
    {
        //---------------------------------------------------------------------------------
        [XmlAttribute]
        public string Name
        {
            get { return RefMultiName == null ? "" : RefAllNames; }
            set { RefMultiName = new Helpers.MultiName(value??""); }
        }

        [XmlIgnore]
        public Helpers.MultiName RefMultiName;

        [XmlIgnore]
        public string RefAllNames { get { return RefMultiName.Full; } }

        [XmlIgnore]
        public string RefMainName { get { return RefMultiName.Main; } }

        [XmlIgnore]
        public bool IsUnnamed { get { return RefMultiName.IsUnnamed(); } }

        //---------------------------------------------------------------------------------
        // todo : change with a flag
        [XmlIgnore]
        public bool IsExtinct
        {
            get
            {
                if (RedListCategory == RedListCategoryEnum.Extinct) return true;
                //if ((Flags & (uint)FlagsEnum.ExtinctInherited) != 0) return true;
                return false;
            }
        }

        [XmlAttribute]
        public uint Flags { get; set; } = 0;

        public bool HasFlag(FlagsEnum _flag)
        {
            return (Flags & ((uint)_flag)) != 0;
        }

        public void SetFlag(FlagsEnum _flag)
        {
            Flags |= (uint)_flag;
        }

        public void UnsetFlag(FlagsEnum _flag)
        {
            Flags &= ~((uint)_flag);
        }

        public void SetFlagValue(FlagsEnum _flag, bool _value)
        {
            if (_value)
                Flags |= (uint)_flag;
            else
                Flags &= ~((uint)_flag);
        }

        //---------------------------------------------------------------------------------
        public List<TaxonImageDesc> Images = null;

        [XmlIgnore]
        public bool HasImage = false;

        [XmlIgnore]
        public List<TaxonImageDesc> AvailableImages = null;

        public void UpdateAvailableImages()
        {
            AvailableImages = null;
            HasImage = false;
            if (Images != null)
            {
                if (Images.Count == 0)
                    Images = null;
                else
                {
                    foreach (TaxonImageDesc image in Images)
                    {
                        if (TaxonImages.Manager.CollectionIsAvailable(image.CollectionId))
                        {
                            if (AvailableImages == null)
                                AvailableImages = new List<TaxonImageDesc>();
                            AvailableImages.Add(image);
                            HasImage = true;
                        }
                    }
                }
            }
        }

        List<TaxonImageDesc> ListImageDesc( int collectionId, bool _secondary = false)
        {
            List<TaxonImageDesc> result = new List<TaxonImageDesc>();
            foreach (TaxonImageDesc desc in Images)
            {
                if (desc.CollectionId != collectionId) continue;
                if (_secondary != desc.Secondary) continue;
                result.Add(desc);
            }
            result.Sort((x, y) => { return x.Index.CompareTo(y.Index); });
            return result;
        }

        void ReindexAndReorderImages( List<TaxonImageDesc> _list)
        {
            if (_list == null || _list.Count == 0) return;
            int insertIndex = int.MaxValue;
            _list.ForEach(i => { int index = Images.IndexOf(i); insertIndex = Math.Min(insertIndex, index); Images.RemoveAt(index); i.StartUseTempName(this); });
            int newIndex = _list[0].Secondary  ? 0 : - 1;
            _list.ForEach(i => { i.EndUseTempName_ChangeIndex(newIndex++); });
            _list.Reverse();
            _list.ForEach(i => { Images.Insert(insertIndex, i); });
            UpdateAvailableImages();
            TaxonImages.Manager.Clear();
            Controls.TaxonControlList.OnAvailableImagesChanged();
            Controls.TaxonControlList.OnReselectTaxon();
        }

        public bool CanChangeIndexToStart(TaxonImageDesc _imageDesc)
        {
            if (_imageDesc == null) return false;
            List<TaxonImageDesc> list = ListImageDesc(_imageDesc.CollectionId, _imageDesc.Secondary );
            return (list.IndexOf(_imageDesc) > 0);
        }
        public void ChangeIndexToStart(TaxonImageDesc _imageDesc)
        {
            if (_imageDesc == null) return;
            List<TaxonImageDesc> list = ListImageDesc(_imageDesc.CollectionId, _imageDesc.Secondary);
            int index = list.IndexOf(_imageDesc);
            if (index == 0) return;
            list.RemoveAt(index);
            list.Insert(0, _imageDesc);
            ReindexAndReorderImages(list);
        }

        public void ChangeIndexToNext(TaxonImageDesc _imageDesc)
        {
            if (_imageDesc == null) return;
            List<TaxonImageDesc> list = ListImageDesc(_imageDesc.CollectionId, _imageDesc.Secondary);
            int index = list.IndexOf(_imageDesc);
            if (index == 0) return;
            list.RemoveAt(index);
            list.Insert(index - 1, _imageDesc);
            ReindexAndReorderImages(list);
        }
        public void ChangeIndexToPrevious(TaxonImageDesc _imageDesc)
        {
            if (_imageDesc == null) return;
            List<TaxonImageDesc> list = ListImageDesc(_imageDesc.CollectionId, _imageDesc.Secondary);
            int index = list.IndexOf(_imageDesc);
            if (index == -1 || index == list.Count - 1) return;
            list.RemoveAt(index);
            list.Insert(index + 1, _imageDesc);
            ReindexAndReorderImages(list);
        }

        public bool CanChangeIndexToEnd(TaxonImageDesc _imageDesc)
        {
            if (_imageDesc == null) return false;
            List<TaxonImageDesc> list = ListImageDesc(_imageDesc.CollectionId, _imageDesc.Secondary);
            int index = list.IndexOf(_imageDesc);
            return (index != -1 && index < list.Count - 1);
        }

        public void ChangeIndexToEnd(TaxonImageDesc _imageDesc)
        {
            if (_imageDesc == null) return;
            List<TaxonImageDesc> list = ListImageDesc(_imageDesc.CollectionId, _imageDesc.Secondary);
            int index = list.IndexOf(_imageDesc);
            if (index == -1 || index == list.Count - 1) return;
            list.RemoveAt(index);
            list.Add(_imageDesc);
            ReindexAndReorderImages(list);
        }

        public void SetImageAsPrimary(TaxonImageDesc _imageDesc)
        {
            SetImageAsSecondary(_imageDesc, false);
        }

        public void SetImageAsSecondary(TaxonImageDesc _imageDesc, bool _secondary = true)
        {
            if (_imageDesc == null) return;
            if (_imageDesc.Secondary == _secondary) return;

            List<TaxonImageDesc> listFrom = ListImageDesc(_imageDesc.CollectionId, _imageDesc.Secondary);
            List<TaxonImageDesc> listTo = ListImageDesc(_imageDesc.CollectionId, !_imageDesc.Secondary);
            int index = listFrom.IndexOf(_imageDesc);
            if (index == -1) return;
            _imageDesc.StartUseTempName(this);
            listFrom.Remove(_imageDesc);
            _imageDesc.Secondary = _secondary;
            listTo.Add(_imageDesc);
            ReindexAndReorderImages(listFrom);
            ReindexAndReorderImages(listTo);
        }

        public void DeleteImage(TaxonImageDesc _imageDesc, bool _rename)
        {
            if (_imageDesc.IsALink) return;
            string path = _imageDesc.GetPath(this);
            if (File.Exists(path))
                File.Delete(path);
            Images.Remove(_imageDesc);

            if (!_rename) return;
            foreach (TaxonImageDesc desc in Images)
            {
                if (desc.CollectionId != _imageDesc.CollectionId) continue;
                if (desc.Secondary != _imageDesc.Secondary) continue;
                if (desc.Index <= _imageDesc.Index) continue;
                desc.ChangeIndexAndRenameFile(this, desc.Index - 1);
            }
            UpdateAvailableImages();
            TaxonImages.Manager.Clear();
            Controls.TaxonControlList.OnAvailableImagesChanged();
            Controls.TaxonControlList.OnReselectTaxon();
        }


        //---------------------------------------------------------------------------------
        [XmlIgnore]
        public string SoundName
        {
            get { return RefMultiName.Main + ".wma"; }
        }
        [XmlAttribute]
        public bool HasSound { get; set; } = false;

        //---------------------------------------------------------------------------------
        // french name
        [XmlAttribute]
        public string FrenchName
        {
            get { return FrenchMultiName == null ? "" : FrenchMultiName.Full; }
            set
            {
                if (value == "" || value == "Non nommé" )
                {
                    FrenchMultiName = null;
                    _FrenchSearchMultiName = null;
                    return;
                }
                FrenchMultiName = new Helpers.MultiName(value);
                string forSearch = RemoveAccents(FrenchMultiName.Full);
                if (FrenchMultiName.Full != forSearch)
                    _FrenchSearchMultiName = new Helpers.MultiName(forSearch);
            }
        }

        [XmlIgnore]
        public Helpers.MultiName FrenchMultiName;

        [XmlIgnore]
        public string FrenchAllNames { get { return FrenchMultiName == null ? "" : FrenchMultiName.Full; } }

        [XmlIgnore]
        public string FrenchMainName { get { return FrenchMultiName == null ? "" : FrenchMultiName.Main; } }

        [XmlIgnore]
        public bool HasFrenchName { get { return FrenchMultiName != null && FrenchMultiName.Full != ""; } }


        private static string RemoveAccents(string s)
        {
            Encoding destEncoding = Encoding.GetEncoding("iso-8859-8");
            return destEncoding.GetString( Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(s)));
        }

        [XmlIgnore]
        Helpers.MultiName _FrenchSearchMultiName = null;
        public Helpers.MultiName FrenchSearchMultiName { get { return _FrenchSearchMultiName ?? FrenchMultiName; } }

        [XmlAttribute]
        public ClassicRankEnum ClassicRank { get; set; } = ClassicRankEnum.None;

        [XmlAttribute]
        public uint Age { get; set; } = 0;

        [XmlAttribute]
        public float RelativeGeneticDistance { get; set; } = 0;

        [XmlAttribute]
        public uint OTTID { get; set; } = 0;

        [XmlAttribute]
        public RedListCategoryEnum RedListCategory { get; set; } = RedListCategoryEnum.NotEvaluated;

        //---------------------------------------------------------------------------------
        public override string ToString() { return "TaxonDesc: " + RefMultiName.Main; }

        //=========================================================================================
        // Constructors
        //

        public TaxonDesc(string _name)
        {
            if (string.IsNullOrEmpty(_name))
                RefMultiName = new Helpers.MultiName("");
            else
                RefMultiName = new Helpers.MultiName(_name.Substring(0, 1).ToUpper() + _name.Substring(1));
        }

        public TaxonDesc() { }

        //=========================================================================================
        // comparison interface
        //
        public int CompareTo(object obj)
        {
            if (!(obj is TaxonDesc)) return 0;
            return RefMultiName.CompareTo((obj as TaxonDesc).RefMultiName);
        }

        //=========================================================================================
        // Save / Load
        //
        public void SaveBin(BinaryWriter _bw)
        {
            _bw.Write(RefAllNames);
            _bw.Write(Flags);
            _bw.Write(HasSound);
            _bw.Write(FrenchAllNames);
            _bw.Write((uint) ClassicRank);
            _bw.Write(Age);
            _bw.Write(RelativeGeneticDistance);
            _bw.Write(OTTID);
            _bw.Write((byte)RedListCategory);

            if (Images == null)
                _bw.Write(0);
            else
            {
                _bw.Write(Images.Count);
                foreach (TaxonImageDesc image in Images)
                {
                    _bw.Write(image.CollectionId);
                    _bw.Write(image.Secondary);
                    _bw.Write(image.Index);
                    _bw.Write(image.LinksId);
                }
            }
        }

        public static TaxonDesc LoadBin(BinaryReader _br, uint _version )
        {
            if (_version == 1)
            {
                TaxonDesc taxon = new TaxonDesc
                {
                    Name = _br.ReadString(),
                    HasSound = _br.ReadBoolean(),
                    FrenchName = _br.ReadString(),
                    ClassicRank = (ClassicRankEnum)_br.ReadUInt32(),
                    Age = _br.ReadUInt32(),
                    RelativeGeneticDistance = _br.ReadSingle(),
                    OTTID = _br.ReadUInt32()
                };

                uint imageNumber = _br.ReadUInt32();
                if (imageNumber != 0)
                {
                    taxon.Images = new List<TaxonImageDesc>((int)imageNumber);
                    for (uint i = 0; i < imageNumber; i++)
                    {
                        TaxonImageDesc image = new TaxonImageDesc
                        {
                            CollectionId = _br.ReadInt32(),
                            Secondary = _br.ReadBoolean(),
                            Index = _br.ReadInt32()
                        };
                        taxon.Images.Add(image);
                    }
                }

                return taxon;
            }

            if (_version == 2)
            {
                TaxonDesc taxon = new TaxonDesc
                {
                    Name = _br.ReadString(),
                    Flags = _br.ReadUInt32(),
                    HasSound = _br.ReadBoolean(),
                    FrenchName = _br.ReadString(),
                    ClassicRank = (ClassicRankEnum)_br.ReadUInt32(),
                    Age = _br.ReadUInt32(),
                    RelativeGeneticDistance = _br.ReadSingle(),
                    OTTID = _br.ReadUInt32()
                };

                uint imageNumber = _br.ReadUInt32();
                if (imageNumber != 0)
                {
                    taxon.Images = new List<TaxonImageDesc>((int)imageNumber);
                    for (uint i = 0; i < imageNumber; i++)
                    {
                        TaxonImageDesc image = new TaxonImageDesc
                        {
                            CollectionId = _br.ReadInt32(),
                            Secondary = _br.ReadBoolean(),
                            Index = _br.ReadInt32()
                        };
                        taxon.Images.Add(image);
                    }
                }

                return taxon;
            }

            if (_version == 3)
            {
                TaxonDesc taxon = new TaxonDesc
                {
                    Name = _br.ReadString()
                };

                if (_br.ReadUInt32() != 0)
                    /*taxon.AlternativeNames = */_br.ReadString();

                taxon.Flags = _br.ReadUInt32();
                taxon.HasSound = _br.ReadBoolean();
                taxon.FrenchName = _br.ReadString();
                taxon.ClassicRank = (ClassicRankEnum)_br.ReadUInt32();
                taxon.Age = _br.ReadUInt32();
                taxon.RelativeGeneticDistance = _br.ReadSingle();
                taxon.OTTID = _br.ReadUInt32();

                uint imageNumber = _br.ReadUInt32();
                if (imageNumber != 0)
                {
                    taxon.Images = new List<TaxonImageDesc>((int)imageNumber);
                    for (uint i = 0; i < imageNumber; i++)
                    {
                        TaxonImageDesc image = new TaxonImageDesc
                        {
                            CollectionId = _br.ReadInt32(),
                            Secondary = _br.ReadBoolean(),
                            Index = _br.ReadInt32()
                        };
                        taxon.Images.Add(image);
                    }
                }

                return taxon;
            }

            if (_version == 4)
            {
                TaxonDesc taxon = new TaxonDesc
                {
                    Name = _br.ReadString(),
                    Flags = _br.ReadUInt32(),
                    HasSound = _br.ReadBoolean(),
                    FrenchName = _br.ReadString(),
                    ClassicRank = (ClassicRankEnum)_br.ReadUInt32(),
                    Age = _br.ReadUInt32(),
                    RelativeGeneticDistance = _br.ReadSingle(),
                    OTTID = _br.ReadUInt32()
                };

                uint imageNumber = _br.ReadUInt32();
                if (imageNumber != 0)
                {
                    taxon.Images = new List<TaxonImageDesc>((int)imageNumber);
                    for (uint i = 0; i < imageNumber; i++)
                    {
                        TaxonImageDesc image = new TaxonImageDesc
                        {
                            CollectionId = _br.ReadInt32(),
                            Secondary = _br.ReadBoolean(),
                            Index = _br.ReadInt32()
                        };
                        taxon.Images.Add(image);
                    }
                }

                return taxon;
            }

            if (_version == 5)
            {
                TaxonDesc taxon = new TaxonDesc
                {
                    Name = _br.ReadString(),
                    Flags = _br.ReadUInt32(),
                    HasSound = _br.ReadBoolean(),
                    FrenchName = _br.ReadString(),
                    ClassicRank = (ClassicRankEnum)_br.ReadUInt32(),
                    Age = _br.ReadUInt32(),
                    RelativeGeneticDistance = _br.ReadSingle(),
                    OTTID = _br.ReadUInt32()
                };

                uint imageNumber = _br.ReadUInt32();
                if (imageNumber != 0)
                {
                    taxon.Images = new List<TaxonImageDesc>((int)imageNumber);
                    for (uint i = 0; i < imageNumber; i++)
                    {
                        TaxonImageDesc image = new TaxonImageDesc
                        {
                            CollectionId = _br.ReadInt32(),
                            Secondary = _br.ReadBoolean(),
                            Index = _br.ReadInt32(),
                            LinksId = _br.ReadInt32()
                        };
                        taxon.Images.Add(image);
                    }
                }

                return taxon;
            }

            if (_version == 6)
            {
                TaxonDesc taxon = new TaxonDesc
                {
                    Name = _br.ReadString(),
                    Flags = _br.ReadUInt32(),
                    HasSound = _br.ReadBoolean(),
                    FrenchName = _br.ReadString(),
                    ClassicRank = (ClassicRankEnum)_br.ReadUInt32(),
                    Age = _br.ReadUInt32(),
                    RelativeGeneticDistance = _br.ReadSingle(),
                    OTTID = _br.ReadUInt32(),
                    RedListCategory = (RedListCategoryEnum)_br.ReadByte()
                };

                uint imageNumber = _br.ReadUInt32();
                if (imageNumber != 0)
                {
                    taxon.Images = new List<TaxonImageDesc>((int)imageNumber);
                    for (uint i = 0; i < imageNumber; i++)
                    {
                        TaxonImageDesc image = new TaxonImageDesc
                        {
                            CollectionId = _br.ReadInt32(),
                            Secondary = _br.ReadBoolean(),
                            Index = _br.ReadInt32(),
                            LinksId = _br.ReadInt32()
                        };
                        taxon.Images.Add(image);
                    }
                }

                return taxon;
            }


            return null;
        }
    }
}
