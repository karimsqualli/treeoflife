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
    static class Helpers
    {
        static public MainViewModel VM { get; set; }

        //=================================================================================================================================
        // Get page content
        //
        static public string GetPageContent(string _url)
        {
            try
            {
                WebRequest request = WebRequest.Create(_url);
                request.Timeout = 2000;
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                return reader.ReadToEnd();
            }
            catch
            {
                Log("Error GetPageContent(" + _url + ")");
                return null;
            }
        }

        //=================================================================================================================================
        // Image name from url
        //
        static public string GetImageNameFromUrl(string _url)
        {
            return _url.Substring(_url.LastIndexOf('/') + 1);
        }

        //=================================================================================================================================
        // Get species photos name
        //
        static Regex regexPhotoUrlInSpeciesContent = new Regex("<div class=\"thumb\">[^<]*<div class=\"wrapper\">[^<]*<div class=\"inner\">[^<]*<a href=\"([^\"]*)", RegexOptions.Singleline | RegexOptions.Compiled);
        static Regex regexLargeImageUrlInImagePageContent = new Regex("<link rel=\"image_src\" href=\"([^\"]*)", RegexOptions.Compiled);
        static Regex regexAllSrc = new Regex("src=\"([^\"]*)", RegexOptions.Compiled);
        static public List<NetworkFile> GetSpeciesPhotosOnlyName(string _species)
        {
            string url = "http://www.arkive.org" + _species + "photos.html";
            string urlContent = GetPageContent(url);
            if (urlContent == null)
                return null;

            //string folderRoot = CreateSpeciesPath(_species);

            List<NetworkFile> results = new List<NetworkFile>();

            MatchCollection matches = regexAllSrc.Matches(urlContent);
            foreach (Match match in matches)
            {
                string urlSrc = match.Groups[1].Value;
                if (!urlSrc.Contains("/Presentation.Small/"))
                    continue;
                urlSrc = urlSrc.Replace("/Presentation.Small/", "/Presentation.Large/");
                results.Add(new NetworkFile(urlSrc));
            }

            return results;
        }

        //=================================================================================================================================
        // compute folder for species
        //
        static public string CreateSpeciesPath(string _species)
        {
            string species = _species.Trim(" /".ToCharArray());
            string folderName = VM.OutputFolder + Path.DirectorySeparatorChar + species.Replace("/", "\\");
            Directory.CreateDirectory(folderName);
            return folderName;
        }

        static public string GetImagePath(string _species, string _imageName)
        {
            string species = _species.Trim(" /".ToCharArray());
            string result = VM.OutputFolder + Path.DirectorySeparatorChar + species.Replace("/", "\\");
            result += Path.DirectorySeparatorChar + _imageName;
            return result;
        }

        //=================================================================================================================================
        // Log
        //

        static public void ClearLog()
        {
            lock (VM.LogFilename)
            {
                File.Delete(VM.LogFilename);
            }
        }

        static public void Log(string _text)
        {
            lock (VM.LogFilename)
            {
                using (StreamWriter writer = new StreamWriter(VM.LogFilename, true))
                {
                    writer.WriteLine(_text);
                }
            }
        }

        //=================================================================================================================================
        // Log information (crossing information in species list and image dictionary
        //
        static public void LogDicoResult(List<string> _species, Dictionary<string, List<NetworkFile>> _networkImages, Dictionary<string, List<string>> _images)
        {
            int countSpeciesNotInDico = 0;
            int countSpeciesNotInList = 0;
            int countSpeciesWithoutImages = 0;
            int countSpeciesWithImages = 0;
            int countImages = 0;

            foreach (string species in _species)
            {
                if (_images != null)
                {
                    if (_images.ContainsKey(species))
                    {
                        int images = _images[species].Count;
                        if (images == 0)
                        {
                            countSpeciesWithoutImages++;
                            Console.WriteLine("without images: " + species);
                        }
                        else
                            countSpeciesWithImages++;
                        countImages += images;
                    }
                    else
                        countSpeciesNotInDico++;
                }
                else if (_networkImages != null)
                {
                    if (_networkImages.ContainsKey(species))
                    {
                        int images = _networkImages[species].Count;
                        if (images == 0)
                        {
                            countSpeciesWithoutImages++;
                            Console.WriteLine("without images: " + species);
                        }
                        else
                            countSpeciesWithImages++;
                        countImages += images;
                    }
                    else
                        countSpeciesNotInDico++;
                }
            }
            if (_images != null)
            {
                foreach (string key in _images.Keys)
                {
                    if (!_species.Contains(key))
                        countSpeciesNotInList++;
                }
            }
            else if (_networkImages != null)
            {
                foreach (string key in _networkImages.Keys)
                {
                    if (!_species.Contains(key))
                        countSpeciesNotInList++;
                }
            }

            Log("GetSpeciesImages done");
            Log("    " + countSpeciesNotInDico + " species not found in image dictionary");
            Log("    " + countSpeciesNotInList + " species not found in list (but in image dictionary)");
            Log("    " + countSpeciesWithoutImages + " species with no images");
            Log("    " + countSpeciesWithImages + " species with images");
            Log("    " + countImages + " total images found");
        }

    }
}
