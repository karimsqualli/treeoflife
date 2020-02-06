using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arkive
{
    static class ArkiveSpecies
    {
        static List<string> GetLocal( string _file )
        {
            List<string> results = new List<string>();
            if (File.Exists(_file))
            {
                using (StreamReader reader = new StreamReader(_file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        results.Add(line);
                    }
                }
            }
            return results;
        }

        static public List<string> Get(bool _localFile, string _file )
        {
            try
            {
                // read from file if asked and file exists
                if (_localFile)
                    return GetLocal(_file);

                // clear file
                if (File.Exists(_file)) File.Delete(_file);

                // get first page to extract total number of page
                string allSpeciesFirstPage = Helpers.GetPageContent("http://www.arkive.org/explore/species");
                int pageNumber = GetPageNumber(allSpeciesFirstPage);
                Helpers.Log("Found " + pageNumber.ToString() + " pages of species");

                // get species from first page
                List<string> networkResults = GetSpeciesFromPage(allSpeciesFirstPage);

                // parse all other pages
                int pageCount = 1;
                Parallel.For(2, pageNumber + 1, new ParallelOptions { MaxDegreeOfParallelism = 4 },
                    index =>
                    {
                        string urlPage = "http://www.arkive.org/explore/species/all/all/" + index.ToString();
                        string pageContent = Helpers.GetPageContent(urlPage);
                        List<string> results = GetSpeciesFromPage(pageContent);
                        lock (networkResults)
                        {
                            networkResults.AddRange(results);
                        }
                        pageCount++;
                        float percent = (float)pageCount / (float)pageNumber;
                        Console.WriteLine("[" + percent.ToString("p1") + "] " + urlPage);
                    }
                );

                networkResults.Sort();

                // save results in file
                using (StreamWriter writer = new StreamWriter(_file))
                {
                    foreach (string res in networkResults)
                        writer.WriteLine(res);
                }

                return networkResults;
            }
            catch
            {
                return null;
            }
        }

        //=================================================================================================================================
        // Get page number from one species page content
        //
        static Regex regexForPageNumber = new Regex("a href=\\\"\\/explore\\/species\\/all\\/all\\/([0-9]*)\\\"", RegexOptions.Compiled);
        static int GetPageNumber(string _content)
        {
            int pageNumber = -1;
            int number = -1;
            Match match = regexForPageNumber.Match(_content);
            while (match.Success)
            {
                if (int.TryParse(match.Groups[1].Value, out number))
                    if (number > pageNumber)
                        pageNumber = number;
                match = match.NextMatch();
            }
            return pageNumber;
        }

        //=================================================================================================================================
        // Get species reference in a array of species page
        //
        static List<string> GetSpeciesFromPage(string _content)
        {
            List<string> results = new List<string>();

            string refString = "<a href=\"";
            int index = _content.IndexOf("<div class=\"thumb\">");
            while (index != -1)
            {
                int endIndex = _content.IndexOf("</div>", index);
                if (endIndex == -1) break; ;

                index = _content.IndexOf(refString, index);
                if (index == -1 || index > endIndex) break;

                index += refString.Length;
                endIndex = _content.IndexOf("\"", index);
                if (endIndex == -1) break;

                results.Add(_content.Substring(index, endIndex - index));

                index = _content.IndexOf("<div class=\"thumb\">", endIndex);
            }
            return results;
        }


    }
}
