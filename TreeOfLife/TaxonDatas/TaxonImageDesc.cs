using System;
using System.IO;
using System.Xml.Serialization;

namespace TreeOfLife
{
    //=========================================================================================
    // one Image descriptor
    public class TaxonImageDesc
    {
        [XmlAttribute]
        public int CollectionId;
        [XmlAttribute]
        public bool Secondary = false;
        [XmlAttribute]
        public int LinksId = 0;
        [XmlAttribute]
        public int Index = 0;

        [XmlIgnore]
        public bool IsALink { get { return LinksId != 0; } }

        public override string ToString()
        {
            return TaxonImages.Manager.CollectionName(CollectionId) + (IsALink ? " (l) " : "") + (Secondary ? " (s) " : " ") + Index.ToString();
        }

        public string GetName( TaxonDesc _taxon )
        {
            string name = _taxon.RefMultiName.Main;
            if (Secondary)
            {
                name += "_s";
                if (Index != -1) name += Index.ToString();
            }
            else if (Index != -1) name += "_" + Index.ToString();
            name += ".jpg";
            return name;
        }

        public string GetPath(TaxonDesc _taxon)
        {
            if (IsALink) return null;
            return TaxonImages.Manager.CollectionPath(CollectionId) + "\\" + GetName(_taxon);
        }

        public string GetLink()
        {
            if (!IsALink) return null;
            ImageCollection collection = TaxonImages.Manager.Collection(CollectionId);
            if (collection == null) return null;
            collection.AllLinks.TryGetValue(LinksId, out ImagesLinks links);
            if (links != null && links.Count > Index)
                return links[Index].Link;
            return null;
        }

        public string GetImageCacheFile()
        {
            return Path.Combine(TaxonUtils.GetImageCachePath(), "link_" + CollectionId.ToString() + "_" + LinksId.ToString() + "-" + Index.ToString() + ".jpg");
        }

        public string GetTempName(TaxonDesc _taxon)
        {
            string name = GetName(_taxon);
            return name.Substring(0, name.Length - 4) + "_temp.jpg";
        }
        public string GetTempPath(TaxonDesc _taxon)
        {
            return TaxonImages.Manager.CollectionPath(CollectionId) + "\\" + GetTempName(_taxon);
        }

        TaxonDesc _TaxonForTempName = null;
        string _TempPath = null;
        public void StartUseTempName(TaxonDesc _taxon)
        {
            if (_TaxonForTempName != null) return;
            if (_taxon == null) return;
            if (IsALink) return;
            _TaxonForTempName = _taxon;
            _TempPath = GetTempPath(_taxon);
            File.Move(GetPath(_taxon), _TempPath );
        }

        public void EndUseTempName_ChangeIndex(int _newIndex)
        {
            if (IsALink) return;
            Index = _newIndex;
            File.Move(_TempPath, GetPath(_TaxonForTempName));
            _TaxonForTempName = null;
        }

        public void EndUseTempName_ChangeCategoryAndIndex(bool _secondary, int _newIndex)
        {
            Secondary = _secondary;
            EndUseTempName_ChangeIndex(_newIndex);
        }

        public string GetCollectionName()
        {
            return TaxonImages.Manager.CollectionName(CollectionId);
        }

        public ImageCollection GetCollection()
        {
            return TaxonImages.Manager.Collection(CollectionId);
        }

        public bool Exists(TaxonDesc _taxon)
        {
            if (IsALink) return false;
            return File.Exists(GetPath(_taxon));
        }

        public void FillForNewFile(TaxonDesc _taxon)
        {
            Index = 0;
            while (Exists(_taxon))
                Index++;
        }

        public bool ChangeIndexAndRenameFile(TaxonDesc _taxon, int _newIndex )
        {
            if (IsALink) return false;
            string oldPath = GetPath(_taxon);
            int saveIndex = Index;
            Index = _newIndex;
            string newPath = GetPath(_taxon);
            Index = saveIndex;

            if (File.Exists(newPath))
            {
                Loggers.WriteError(LogTags.Image, "Can't reindex image " + oldPath + ",\n    file " + newPath + " already exists");
                return false;
            }
            try
            {
                File.Move(oldPath, newPath);
            }
            catch (Exception e )
            {
                Loggers.WriteError(LogTags.Image, "Error renaming " + oldPath + " to " + newPath + ":\n" + e.Message);
                return false;
            }
            Index = _newIndex;
            return true;
        }
    }
}
