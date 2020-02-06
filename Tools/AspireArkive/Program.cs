using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AspireArkive
{
    class Program
    {
        static string OutputPath = "E:\\Perso\\Arkive";
        static string LogFilename = OutputPath + "\\arkive.log";
        static string SpeciesFilename = OutputPath + "\\species.txt";
        static string ImagesFilename = OutputPath + "\\photos.txt";

        class NetworkFile
        {
            public NetworkFile( string _url )
            {
                URL = _url;
                Name = GetImageNameFromUrl(_url);
            }
            public string URL;
            public string Name;
        }

        static Dictionary<string, List<NetworkFile>> _NetworkImages = new Dictionary<string, List<NetworkFile>>();
        static Dictionary<string, List<string>> _LocalImages = new Dictionary<string, List<string>>();
        static void Main(string[] args)
        {
            //--------------------------------------------------------------------------------------------
            // Clear data
            //
            if (File.Exists(LogFilename)) File.Delete(LogFilename);

            // Get all species
            List<string> speciesUrl = GetAllSpecies(false);
            if (speciesUrl == null || speciesUrl.Count == 0)
            {
                Log("Error getting species, abort");
                return;
            }
            Log("Get species done : " + speciesUrl.Count);


            // Get all images
            _NetworkImages = GetSpeciesImages(speciesUrl, false);
            if (_NetworkImages == null || _NetworkImages.Count == 0)
            {
                Log("Error getting species images, abort");
                return;
            }
            NormalizeListImage(_NetworkImages);
            LogDictionaryResult(speciesUrl, _NetworkImages, null);
            

            // Get all local images
            _LocalImages = GetLocalImages(OutputPath);
            LogDictionaryResult(speciesUrl, null, _LocalImages);

            // look for missing local images, and images to remove
            Dictionary<string, List<NetworkFile>> missings = new Dictionary<string, List<NetworkFile>>();

            foreach ( KeyValuePair<string, List<NetworkFile>> pair in _NetworkImages)
            {
                string species = pair.Key.ToLower();
                if (!_LocalImages.ContainsKey(species))
                    missings[pair.Key] = pair.Value;
                else
                {
                    List<string> localImages = _LocalImages[species];
                    foreach(NetworkFile netImage in pair.Value)
                    {
                        string imageName = netImage.Name.ToLower();
                        if (localImages.Contains(imageName))
                        {
                            localImages.Remove(imageName);
                            continue;
                        }
                        if (!missings.ContainsKey(pair.Key))
                            missings[pair.Key] = new List<NetworkFile>();
                        missings[pair.Key].Add(netImage);
                    }
                    if (localImages.Count == 0)
                        _LocalImages.Remove(species);
                }
            }

            // remove old images (what's left in _LocalImages)
            foreach (KeyValuePair<string, List<string>> pair in _LocalImages)
            {
                foreach (string image in pair.Value)
                {
                    string fullfilename = GetImagePath(pair.Key, image);
                    if (File.Exists(fullfilename))
                        File.Delete(fullfilename);
                }
            }

            GetImages(missings);

            // get photos for each species
            /*
            foreach (string res in speciesUrl)
            {
                Console.WriteLine(res);
                GetSpeciesPhotos(res);
            }
            */

            Console.WriteLine("Done. <push key to exit>");
            Console.ReadKey();
        }

        static void Log(string _text)
        {
            lock (LogFilename)
            {
                /*using (StreamWriter writer = new StreamWriter(LogFilename, true))
                {
                    writer.WriteLine(_text);
                }*/
            }
        }

        static string GetPageContent(string _url)
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
        // Get species
        //

        static List<string> GetAllSpecies( bool _fromNetwork )
        {
            try
            {
                // read from file if asked and file exists
                if (!_fromNetwork && File.Exists(SpeciesFilename))
                {
                    List<string> results = new List<string>();
                    using (StreamReader reader = new StreamReader(SpeciesFilename))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            results.Add(line);
                        }
                    }
                    return results;
                }

                // clear file
                if (File.Exists(SpeciesFilename)) File.Delete(SpeciesFilename);

                // get first page to extract total number of page
                string allSpeciesFirstPage = GetPageContent("http://www.arkive.org/explore/species");
                int pageNumber = GetPageNumber(allSpeciesFirstPage);
                Log("Found " + pageNumber.ToString() + " pages of species");

                // get species from first page
                List<string> networkResults = GetSpeciesFromPage(allSpeciesFirstPage);

                // parse all other pages
                int pageCount = 1;
                Parallel.For(2, pageNumber + 1, new ParallelOptions { MaxDegreeOfParallelism = 4 }, 
                    index =>
                    {
                        string urlPage = "http://www.arkive.org/explore/species/all/all/" + index.ToString();
                        string pageContent = GetPageContent(urlPage);
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
                using (StreamWriter writer = new StreamWriter(SpeciesFilename))
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
        static int GetPageNumber( string _content )
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
        static List<string> GetSpeciesFromPage(string _content )
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

       

        //=================================================================================================================================
        // Get images
        //

        static Dictionary<string, List<NetworkFile>> GetSpeciesImages( List<string> _speciesUrl, bool _fromNetwork)
        {
            Dictionary<string, List<NetworkFile>> results = new Dictionary<string, List<NetworkFile>>();

            try
            {
                // read from file if asked and file exists
                if (!_fromNetwork && File.Exists(ImagesFilename))
                {
                    using (StreamReader reader = new StreamReader(ImagesFilename))
                    {
                        string currentSpecies = null;
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            line = line.Trim();
                            if (line.StartsWith("/"))
                            {
                                currentSpecies = line;
                                if (results.ContainsKey(line))
                                    Log(ImagesFilename + " contains multiple species " + line);
                                else
                                    results[line] = new List<NetworkFile>();
                            }
                            else if (line.StartsWith("http:"))
                            {
                                if (currentSpecies == null)
                                    Log(ImagesFilename + " image url outside species " + line);
                                else
                                    results[currentSpecies].Add(new NetworkFile(line));
                            }
                            else
                                Log(ImagesFilename + " image url outside species " + line);
                        }
                    }

                    foreach (string res in _speciesUrl)
                    {
                        if (!results.ContainsKey(res))
                            Log("Error GetImages, no key for " + res);
                    }

                    return results;
                }

                if (File.Exists(ImagesFilename)) File.Delete(ImagesFilename);

                List<string> speciesUrl = new List<string>(_speciesUrl);
                bool tryOnceMore = true;

                while (speciesUrl.Count > 0)
                {
                    List<string> failedSpeciesUrl = new List<string>();
                    int speciesCount = 0;
                    Parallel.ForEach(speciesUrl, new ParallelOptions { MaxDegreeOfParallelism = 16 }, currentSpeciesUrl =>
                    {
                        List<NetworkFile> images = GetSpeciesPhotosOnlyName(currentSpeciesUrl);

                        speciesCount++;
                        float percent = (float)speciesCount / (float)speciesUrl.Count;
                        if (images != null && images.Count > 0)
                        {
                            lock (results)
                            {
                                results[currentSpeciesUrl] = images;
                            }
                            Console.WriteLine("[" + percent.ToString("p1") + "] " + currentSpeciesUrl + " (" + images.Count + " images)");
                        }
                        else
                        {
                            lock (failedSpeciesUrl)
                            {
                                failedSpeciesUrl.Add(currentSpeciesUrl);
                            }
                            Console.WriteLine("[" + percent.ToString("p1") + "] " + currentSpeciesUrl + " (failed)");
                        }
                    }
                    );

                    if (failedSpeciesUrl.Count != 0)
                    {
                        if (failedSpeciesUrl.Count == speciesUrl.Count && !tryOnceMore)
                        {
                            Log("GetSpeciesImages can't read these urls");
                            foreach (string url in failedSpeciesUrl)
                                Log("    " + url);
                            failedSpeciesUrl.Clear();
                        }
                        else
                        {
                            tryOnceMore = failedSpeciesUrl.Count != speciesUrl.Count;
                            Log("GetSpeciesImages can't read " + failedSpeciesUrl.Count + "urls, loop again");
                        }
                    }
                    speciesUrl = failedSpeciesUrl;
                }

                using (StreamWriter writer = new StreamWriter(ImagesFilename))
                {
                    foreach (string res in _speciesUrl)
                    {
                        if (!results.ContainsKey(res))
                        {
                            Log("Error GetImages, no key for " + res);
                            continue;
                        }
                        writer.WriteLine(res);
                        List<NetworkFile> photos = results[res];
                        foreach (NetworkFile url in photos)
                            writer.WriteLine("    " + url.URL);
                    }
                }

                return results;
            }
            catch
            {
                return null;
            }
        }

        //=================================================================================================================================
        // Get species photos
        //

        static void NormalizeListImage(List<NetworkFile> _list)
        {
            _list.Sort((x, y) => (x.URL.CompareTo(y.URL)));

            bool renameNeeded = false;
            Dictionary<string, int> nameDico = new Dictionary<string, int>();
            foreach (NetworkFile image in _list)
            {
                string name = image.Name.ToLower();
                if (!nameDico.ContainsKey(name))
                    nameDico[name] = 1;
                else
                {
                    renameNeeded = true;
                    nameDico[name]++;
                }
            }

            if (!renameNeeded) return;

            foreach (KeyValuePair<string,int> pair in nameDico)
            {
                if (pair.Value == 1) continue;

                int count = 0;
                foreach( NetworkFile image in _list)
                {
                    if (image.Name.ToLower() != pair.Key)
                        continue;
                    string ext = Path.GetExtension(image.Name);
                    image.Name = Path.GetFileNameWithoutExtension(image.Name) + "_" + count.ToString() + ext;
                    count++;
                }
            }
        }

       static void NormalizeListImage(Dictionary<string, List<NetworkFile>> _images )
        {
            foreach (KeyValuePair<string, List<NetworkFile>> pair in _images)
                NormalizeListImage(pair.Value);
        }

        static WebClient webClient = new WebClient();
        static void GetSpeciesPhotos(string _species)
        {
            List<NetworkFile> urls = GetSpeciesPhotosOnlyName(_species);
            NormalizeListImage(urls);
            string folderRoot = CreateSpeciesPath(_species);
            foreach (NetworkFile urlImage in urls)
            {
                try
                {
                    webClient.DownloadFile(urlImage.URL, folderRoot + "\\" + urlImage.Name);
                }
                catch
                {
                    Log("Error in GetSpeciesPhotos(" + _species + "): cannot download file " + urlImage.URL);
                }
            }
        }

        static void GetImages(Dictionary<string, List<NetworkFile>> _urlImagePerSpecies )
        {
            List<Tuple<string, NetworkFile>> list= new List<Tuple<string, NetworkFile>>();
            foreach (KeyValuePair<string, List<NetworkFile>> pair in _urlImagePerSpecies)
            {
                if (pair.Value == null || pair.Value.Count == 0)
                    continue;
                foreach (NetworkFile url in pair.Value)
                    list.Add(new Tuple<string, NetworkFile>(pair.Key, url));
            }

            bool tryOnceMore = true;
            bool medium = false;
            while (list.Count > 0)
            {
                List<Tuple<string, NetworkFile>> failed = new List<Tuple<string, NetworkFile>>();
                int imageCount = 0;
                //Parallel.ForEach(list, new ParallelOptions { MaxDegreeOfParallelism = 4 }, currentTuple =>
                foreach(Tuple<string, NetworkFile> currentTuple in list)
                {
                    string folderRoot = CreateSpeciesPath(currentTuple.Item1);
                    string imageName = currentTuple.Item2.Name;
                    string url = currentTuple.Item2.URL;
                    if (medium)
                    {
                        imageName = imageName.Replace(".jpg", "_medium.jpg");
                        url = url.Replace("/Presentation.Large/", "/Presentation.Medium/");
                    }

                    string result = "";
                    try
                    {
                        webClient.DownloadFile(url, folderRoot + "\\" + imageName);
                        result = " success";
                    }
                    catch
                    {
                        failed.Add(currentTuple);
                        result = " failed";
                    }

                    imageCount++;
                    float percent = (float)imageCount / (float)list.Count;
                    Console.WriteLine("[" + percent.ToString("p1") + "] " + folderRoot + "\\" + imageName + result);
                } //);
                    
                if (failed.Count != 0)
                {
                    if (failed.Count == failed.Count && !tryOnceMore)
                    {
                        if (!medium)
                        {
                            medium = true;
                            tryOnceMore = true;
                        }
                        else
                        {
                            Log("GetSpeciesImages can't read these images");
                            foreach (Tuple<string, NetworkFile> tuple in failed)
                                Log("    " + tuple.Item1 + " - " + tuple.Item2.URL + " => " + tuple.Item2.Name);
                            failed.Clear();
                        }
                    }
                    else
                    {
                        tryOnceMore = failed.Count != list.Count;
                        Log("GetSpeciesImages can't read " + failed.Count + " images, loop again");
                    }
                }
                list = failed;
            }
        }

        //=================================================================================================================================
        // Get species photos name
        //
        static Regex regexPhotoUrlInSpeciesContent = new Regex("<div class=\"thumb\">[^<]*<div class=\"wrapper\">[^<]*<div class=\"inner\">[^<]*<a href=\"([^\"]*)", RegexOptions.Singleline | RegexOptions.Compiled);
        static Regex regexLargeImageUrlInImagePageContent = new Regex("<link rel=\"image_src\" href=\"([^\"]*)", RegexOptions.Compiled);
        static Regex regexAllSrc = new Regex("src=\"([^\"]*)", RegexOptions.Compiled);
        static List<NetworkFile> GetSpeciesPhotosOnlyName(string _species)
        {
            string url = "http://www.arkive.org" + _species + "photos.html";
            string urlContent = GetPageContent(url);
            if (urlContent == null)
                return null;

            string folderRoot = CreateSpeciesPath(_species);

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
        // Log information (crossing information in species list and image dictionary
        //
        static void LogDictionaryResult( List<string> _species, Dictionary<string, List<NetworkFile>> _networkImages, Dictionary<string, List<string>> _images)
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

        //=================================================================================================================================
        // Path and files
        //

        static string GetImagePath( string _species, string _imageName )
        {
            string species = _species.Trim(" /".ToCharArray());
            string result = OutputPath + Path.DirectorySeparatorChar + species.Replace("/", "\\");
            result += Path.DirectorySeparatorChar + _imageName;
            return result;
        }

        static string CreateSpeciesPath(string _species)
        {
            string species = _species.Trim(" /".ToCharArray());
            string folderName = OutputPath + Path.DirectorySeparatorChar + species.Replace("/", "\\");
            Directory.CreateDirectory(folderName);
            return folderName;
        }

        static string GetImageNameFromUrl(string _url)
        {
            return _url.Substring(_url.LastIndexOf('/') + 1);
        }



        static Dictionary<string, List<string>> GetLocalImages( string _folder )
        {
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();

            String[] allfiles = System.IO.Directory.GetFiles(_folder, "*.jpg", System.IO.SearchOption.AllDirectories);
            int prefixLength = _folder.Length;
            foreach( string file in allfiles)
            {
                string species = file.Substring(prefixLength).Replace('\\', '/').ToLower();
                int indexSep = species.LastIndexOf('/');
                if (indexSep == -1) continue;
                string imageName = species.Substring(indexSep + 1);
                species = species.Remove(indexSep + 1);

                if (!results.ContainsKey(species))
                    results[species] = new List<string>();
                results[species].Add(imageName);
            }
            return results;
        }
    }
}
