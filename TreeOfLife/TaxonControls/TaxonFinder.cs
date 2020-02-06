using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
namespace TreeOfLife
{
    //=============================================================================================
    // Finder
    //
    [Description("Search taxons with regular expressions")]
    [DisplayName("Finder")]
    [Controls.IconAttribute("TaxonFinder")]
    public partial class TaxonFinder : Controls.TaxonControl
    {
        //-------------------------------------------------------------------
        public TaxonFinder()
        {
            InitializeComponent();
            TaxonSearchAsync = new TaxonSearchAsync();
            TaxonSearchAsync.OnSearchCompleted += OnSearchCompleted;
        }

        //---------------------------------------------------------------------------------
        public override string ToString() { return "Finder"; }

        //-------------------------------------------------------------------
        void UpdateLayout()
        {
            textBoxSearch.Enabled = _Root != null;
            listBoxResult.Enabled = _Root != null;
        }

        //-------------------------------------------------------------------
        TaxonSearchAsync TaxonSearchAsync = new TaxonSearchAsync();

        //===================================================================
        // Text box
        //

        //-------------------------------------------------------------------
        private void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.Length < 2)
                listBoxResult.DataSource = null;
            else
                TaxonSearchAsync.Search(_Root, textBoxSearch.Text);
            UpdateLayout();
        }

        //-------------------------------------------------------------------
        private void TextBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                if (textBoxSearch.Text.Length < 2)
                    TaxonSearchAsync.Search(_Root, textBoxSearch.Text); 
                if (listBoxResult.Items.Count > 0)
                {
                    TaxonTreeNode taxon = listBoxResult.GetAt(0);
                    if (taxon != null)
                    {
                        TaxonUtils.GotoTaxon(taxon);
                        TaxonUtils.SelectTaxon(taxon);
                    }
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                if (listBoxResult.Items.Count > 0)
                {
                    e.Handled = true;
                    listBoxResult.Focus();
                    if (listBoxResult.SelectedIndex == -1)
                        listBoxResult.SelectedIndex = 0;
                }
            }
        }

        //-------------------------------------------------------------------
        private void TextBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                e.Handled = true;
        }

        //-------------------------------------------------------------------
        /*void SearchTaxons(TaxonTreeNode _taxon, List<TaxonTreeNodeNamed> _list, Regex searchExp)
        {
            if (TaxonUtils.MyConfig.Options.LangageLatin)
            {
                Match match = searchExp.Match(_taxon.Desc.RefAllNames);
                if (match.Success)
                {
                    if (match.Index < _taxon.Desc.RefMultiName.Main.Length)
                        _list.Add(new TaxonTreeNodeNamed(_taxon.Desc.RefMainName, _taxon));
                    else
                        _list.Add(new TaxonTreeNodeNamed(_taxon.Desc.RefMultiName.GetSynonymAtCharIndex( match.Index)+ " (" + _taxon.Desc.RefMainName + ")", _taxon));
                }
            }

            if (TaxonUtils.MyConfig.Options.LangageFrench && _taxon.Desc.HasFrenchName)
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
        }*/

        //-------------------------------------------------------------------
        /*void SearchTaxonsSeparateSynonyms(TaxonTreeNode _taxon, List<TaxonTreeNodeNamed> _list, Regex searchExp)
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
        }*/

        //-------------------------------------------------------------------
        /*static string RemoveDiacritics(string text)
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
        }*/

        //===================================================================
        // Searching thread
        //
        /*object _SearchLock = new object();
        Thread _SearchingThread = null;
        string _SearchString = null;

        private void Search(string _text)
        {
            lock (_SearchLock)
            {
                //Console.WriteLine("Search: " + _text);
                if (_SearchingThread != null)
                {
                    if (_text == _SearchString)
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
                }

                if (_text.Length < 2)
                {
                    //Console.WriteLine("Search: too small text, no search");
                    SearchCompleted(null);
                }
                else
                {
                    _SearchString = _text;
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
                    SearchTaxonsSeparateSynonyms(_Root, results, searchExp);
                else
                    SearchTaxons(_Root, results, searchExp);

                results.Sort();

                //Console.WriteLine("Search thread (end)");
                BeginInvoke((Action)(() => SearchCompleted(results)));
            }
            catch (Exception e)
            {
                //Console.WriteLine("Search thread (abort)");
                Console.WriteLine("Exception : " + e.Message);
            }
        }

        private void SearchCompleted(List<TaxonTreeNodeNamed> _results)
        {
            lock (_SearchLock)
            {
                //Console.WriteLine("Search completed");

                if (_SearchingThread != null)
                    _SearchingThread = null;

                listBoxResult.SuspendLayout();
                listBoxResult.DataSource = null;
                if (_results != null)
                    listBoxResult.DataSource = _results;
                listBoxResult.SelectedIndex = -1;
                listBoxResult.ResumeLayout();
            
                //Console.WriteLine("Seach completed end");
            }
        }
        */

        private void OnSearchCompleted(object sender, TaxonSearchAsyncCompletedArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                listBoxResult.SuspendLayout();
                listBoxResult.DataSource = null;
                if (e.Results != null)
                    listBoxResult.DataSource = e.Results;
                listBoxResult.SelectedIndex = -1;
                listBoxResult.ResumeLayout();
            }));
        }
        


        //===================================================================
        // List box
        //

        //-------------------------------------------------------------------
        private void ListBoxResult_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (listBoxResult.Items.Count == 0 || listBoxResult.SelectedIndex == 0)
                {
                    textBoxSearch.Focus();
                    listBoxResult.SelectedIndex = -1;
                }
            }
        }

        //-------------------------------------------------------------------
        bool _LockEvent = false;
        private void ListBoxResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_LockEvent) return;
            if (!listBoxResult.Focused && listBoxResult.SelectedIndex != -1)
            {
                _LockEvent = true;
                listBoxResult.SelectedIndex = -1;
                _LockEvent = false;
            }
        }
    }
}
