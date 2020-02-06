using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace TreeOfLife
{
    public class TaxonSearchAsyncCompletedArgs : EventArgs
    {
        public TaxonSearchAsyncCompletedArgs(string _searchString, List<TaxonTreeNodeNamed> _results)
        {
            SearchString = _searchString;
            Results = _results;
        }
        public string SearchString { get; set; }
        public List<TaxonTreeNodeNamed> Results { get; set; }
    }

    public class TaxonSearchAsync
    {
        public event EventHandler<TaxonSearchAsyncCompletedArgs> OnSearchCompleted = null;

        readonly object _SearchLock = new object();
        Thread _SearchingThread = null;
        string _SearchString = null;
        TaxonTreeNode _Root; 

        public void Search(TaxonTreeNode _root, string _text)
        {
            lock (_SearchLock)
            {
                //Console.WriteLine("Search: " + _text);
                if (_SearchingThread != null)
                {
                    if (_text == _SearchString && _root == _Root)
                    {
                        //Console.WriteLine("Search: same search in progress");
                        return;
                    }

                    //Console.WriteLine("Search: aborting previous search");
                    _SearchingThread.Abort();
                    //Console.WriteLine("Search: waiting end of thread");
                    while (_SearchingThread.IsAlive) ;
                    //Console.WriteLine("Search: reset data");
                    _SearchingThread = null;
                    _SearchString = null;
                    _Root = null;
                }

                if (_text.Length < 2)
                {
                    //Console.WriteLine("Search: too small text, no search");
                    SearchCompleted(null);
                }
                else
                {
                    _SearchString = _text;
                    _Root = _root;
                    _SearchingThread = new Thread(Search);

                    _SearchingThread.Start();
                    //Console.WriteLine("Search: Starting thread...");

                    // Loop until worker thread activates.
                    while (!_SearchingThread.IsAlive) ;
                    //Console.WriteLine("Search: thread left alone...");
                }
            }
        }

        private void Search()
        {
            try
            {
                //Console.WriteLine("Search thread (start)");

                List<TaxonTreeNodeNamed> results = new List<TaxonTreeNodeNamed>();

                string normalized = RemoveDiacritics(_SearchString);
                Regex searchExp = new Regex(normalized, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                if (normalized.Contains('^') || normalized.Contains('$'))
                {
                    SearchTaxonsSeparateSynonyms(_Root, results, searchExp);
                    results.Sort();
                }
                else
                {
                    SearchTaxons(_Root, results, searchExp);

                    searchExp = new Regex("^" + normalized, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    List<TaxonTreeNodeNamed> resultsStartWith = new List<TaxonTreeNodeNamed>();
                    List<TaxonTreeNodeNamed> resultsOther = new List<TaxonTreeNodeNamed>();

                    foreach (TaxonTreeNodeNamed node in results)
                    {
                        if (searchExp.IsMatch(node.Name))
                            resultsStartWith.Add(node);
                        else
                            resultsOther.Add(node);
                    }
                    resultsStartWith.Sort();
                    resultsOther.Sort();
                    results = resultsStartWith;
                    results.AddRange(resultsOther);
                }

                //Console.WriteLine("Search thread (end)");
                SearchCompleted(results);
            }
            catch (Exception e)
            {
                //Console.WriteLine("Search thread (abort)");
                Console.WriteLine("Exception : " + e.Message);
            }
        }

        //-------------------------------------------------------------------
        void SearchTaxons(TaxonTreeNode _taxon, List<TaxonTreeNodeNamed> _list, Regex searchExp)
        {
            bool added = false;
            if (TaxonUtils.MyConfig.Options.LangageLatin)
            {
                Match match = searchExp.Match(_taxon.Desc.RefAllNames);
                if (match.Success)
                {
                    added = true;
                    if (match.Index < _taxon.Desc.RefMultiName.Main.Length)
                        _list.Add(new TaxonTreeNodeNamed(_taxon.Desc.RefMainName, _taxon));
                    else
                        _list.Add(new TaxonTreeNodeNamed(_taxon.Desc.RefMultiName.GetSynonymAtCharIndex(match.Index) + " (" + _taxon.Desc.RefMainName + ")", _taxon));
                }
            }

            if (!added && TaxonUtils.MyConfig.Options.LangageFrench && _taxon.Desc.HasFrenchName)
            {
                Match match = searchExp.Match(_taxon.Desc.FrenchSearchMultiName.Full);
                if (match.Success)
                {
                    if (match.Index < _taxon.Desc.FrenchMainName.Length)
                        _list.Add(new TaxonTreeNodeNamed(_taxon.Desc.FrenchMainName, _taxon));
                    else
                        _list.Add(new TaxonTreeNodeNamed(_taxon.Desc.FrenchMultiName.GetSynonymAtCharIndex(match.Index) + " (" + _taxon.Desc.FrenchMainName + ")", _taxon));
                }
            }

            foreach (TaxonTreeNode child in _taxon.Children)
                SearchTaxons(child, _list, searchExp);
        }

        //-------------------------------------------------------------------
        void SearchTaxonsSeparateSynonyms(TaxonTreeNode _taxon, List<TaxonTreeNodeNamed> _list, Regex searchExp)
        {
            if (TaxonUtils.MyConfig.Options.LangageLatin)
            {
                if (searchExp.IsMatch(_taxon.Desc.RefMultiName.Main))
                    _list.Add(new TaxonTreeNodeNamed(_taxon.Desc.RefMainName, _taxon));
                else
                {
                    string[] synonyms = _taxon.Desc.RefMultiName.GetSynonymsArray();
                    if (synonyms != null)
                    {
                        foreach (string syn in synonyms)
                        {
                            if (searchExp.IsMatch(syn))
                            {
                                _list.Add(new TaxonTreeNodeNamed(syn + " (" + _taxon.Desc.RefMainName + ")", _taxon));
                                break;
                            }
                        }
                    }
                }
            }

            if (TaxonUtils.MyConfig.Options.LangageFrench && _taxon.Desc.HasFrenchName)
            {
                if (searchExp.IsMatch(_taxon.Desc.FrenchSearchMultiName.Main))
                    _list.Add(new TaxonTreeNodeNamed(_taxon.Desc.FrenchSearchMultiName.Main + " (" + _taxon.Desc.RefMainName + ")", _taxon));
                else
                {
                    string[] synonyms = _taxon.Desc.FrenchSearchMultiName.GetSynonymsArray();
                    if (synonyms != null)
                    {
                        for (int i = 0; i < synonyms.Length; i++)
                        {
                            if (searchExp.IsMatch(synonyms[i]))
                            {
                                if (_taxon.Desc.FrenchSearchMultiName == _taxon.Desc.FrenchMultiName)
                                    _list.Add(new TaxonTreeNodeNamed(synonyms[i] + " (" + _taxon.Desc.RefMainName + ")", _taxon));
                                else
                                {
                                    string realSynonyms = _taxon.Desc.FrenchMultiName.GetSynonymAtIndex(i);
                                    if (realSynonyms != null)
                                    {
                                        _list.Add(new TaxonTreeNodeNamed(realSynonyms + " (" + _taxon.Desc.FrenchMainName + ")", _taxon));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (TaxonTreeNode child in _taxon.Children)
                SearchTaxonsSeparateSynonyms(child, _list, searchExp);
        }

        //-------------------------------------------------------------------
        private void SearchCompleted(List<TaxonTreeNodeNamed> _results)
        {
            lock (_SearchLock)
            {
                //Console.WriteLine("Search completed");

                if (_SearchingThread != null)
                    _SearchingThread = null;
                OnSearchCompleted?.Invoke(this, new TaxonSearchAsyncCompletedArgs(_SearchString, _results));

                //Console.WriteLine("Seach completed end");
            }
        }

        //-------------------------------------------------------------------
        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
