using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife
{
    class TaxonIds
    {
        public const UInt32 FirstTolID = 0x80000000; // normally unused, real first id = FirstTolID + 1
        public static bool IsTolID(UInt32 _id) { return _id >= FirstTolID; }
        public static string IdDesc(UInt32 _id)
        {
            if (_id == 0) return "";
            if (_id >= FirstTolID) return "TOL:" + (_id - FirstTolID).ToString();
            return "OTT:" + _id.ToString();
        }

        public List<TaxonDesc> NoIds = new List<TaxonDesc>();

        public Dictionary<UInt32, TaxonDesc> Ids = new Dictionary<UInt32, TaxonDesc>();
        public UInt32 MinId = UInt32.MaxValue;
        public UInt32 MaxId = 0;

        public Dictionary<UInt32, TaxonDesc> TolIds = new Dictionary<UInt32, TaxonDesc>();
        public UInt32 MinTolId = UInt32.MaxValue;
        public UInt32 MaxTolId = 0;
        
        public List<Tuple<TaxonDesc, TaxonDesc>> Duplicates = new List<Tuple<TaxonDesc, TaxonDesc>>();
        public List<Tuple<UInt32, UInt32>> Ranges = null;

        private void BuildRanges(UInt32 _min, UInt32 _max, Dictionary<UInt32, TaxonDesc> _dico, List<Tuple<UInt32, UInt32>> _ranges )
        {
            if (_min > _max) return;

            UInt32 current = _min;
            UInt32 start = 0;
            UInt32 end = 0;

            while (current <= _max + 1)
            {
                if (_dico.ContainsKey(current))
                {
                    if (start == 0) start = current;
                }
                else
                {
                    if (start != 0)
                    {
                        end = current - 1;
                        _ranges.Add(new Tuple<uint, uint>(start, end));
                        start = 0;
                    }
                }
                current++;
            }

            uint Count = 0;
            foreach (Tuple<uint, uint> tuple in _ranges)
                Count += tuple.Item2 - tuple.Item1 + 1;

            if (Count != _dico.Count)
            {
                Console.WriteLine("error in BuildRanges");
            }
        }

        private void BuildRanges()
        {
            Ranges = new List<Tuple<UInt32, UInt32>>();
            BuildRanges(MinId, MaxId, Ids, Ranges);
            BuildRanges(MinTolId, MaxTolId, TolIds, Ranges);
        }

        public static TaxonIds Compute(TaxonTreeNode _node)
        {
            TaxonIds Data = new TaxonIds();
            _node.ParseNodeDesc(CheckIds, Data);
            Data.BuildRanges();
            return Data;
        }

        private static void CheckIds(TaxonDesc _desc, object _data )
        {
            if (_desc.OTTID == 0)
            {
                (_data as TaxonIds).NoIds.Add(_desc);
                return;
            }

            TaxonIds Data = _data as TaxonIds;
            if (_desc.OTTID >= FirstTolID)
            {
                if (Data.TolIds.ContainsKey(_desc.OTTID))
                {
                    Data.Duplicates.Add(new Tuple<TaxonDesc, TaxonDesc>(Data.TolIds[_desc.OTTID], _desc));
                    return;
                }

                Data.TolIds[_desc.OTTID] = _desc;
                if (_desc.OTTID > Data.MaxTolId) Data.MaxTolId = _desc.OTTID;
                if (_desc.OTTID < Data.MinTolId) Data.MinTolId = _desc.OTTID;
            }
            else
            {
                if (Data.Ids.ContainsKey(_desc.OTTID))
                {
                    Data.Duplicates.Add(new Tuple<TaxonDesc, TaxonDesc>(Data.Ids[_desc.OTTID], _desc));
                    return;
                }

                Data.Ids[_desc.OTTID] = _desc;
                if (_desc.OTTID > Data.MaxId) Data.MaxId = _desc.OTTID;
                if (_desc.OTTID < Data.MinId) Data.MinId = _desc.OTTID;
            }
        }

        public static TaxonIds ComputeTol(TaxonTreeNode _node)
        {
            TaxonIds Data = new TaxonIds();
            _node.ParseNodeDesc(CheckTolIds, Data);
            Data.BuildRanges();
            return Data;
        }

        private static void CheckTolIds(TaxonDesc _desc, object _data)
        {
            if (_desc.OTTID >= FirstTolID)
            {
                TaxonIds Data = _data as TaxonIds;

                if (Data.TolIds.ContainsKey(_desc.OTTID))
                {
                    Data.Duplicates.Add(new Tuple<TaxonDesc, TaxonDesc>(Data.TolIds[_desc.OTTID], _desc));
                    return;
                }

                Data.TolIds[_desc.OTTID] = _desc;
                if (_desc.OTTID > Data.MaxTolId) Data.MaxTolId = _desc.OTTID;
                if (_desc.OTTID < Data.MinTolId) Data.MinTolId = _desc.OTTID;
            }
        }

        public static UInt32 GetUnusedTolId(TaxonTreeNode _node)
        {
            TaxonIds Data = new TaxonIds() { MaxTolId = FirstTolID };
            _node.ParseNodeDesc(LastTolIds, Data);
            return Data.MaxTolId + 1;
        }

        private static void LastTolIds(TaxonDesc _desc, object _data)
        {
            if (_desc.OTTID >= FirstTolID)
            {
                TaxonIds Data = _data as TaxonIds;
                if (_desc.OTTID > Data.MaxTolId) Data.MaxTolId = _desc.OTTID;
            }
        }

    }

    
}
