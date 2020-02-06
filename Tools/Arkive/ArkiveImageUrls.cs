using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkive
{
    class NetworkFile
    {
        public NetworkFile(string _url)
        {
            URL = _url;
            Name = Helpers.GetImageNameFromUrl(_url);
        }
        public string URL;
        public string Name;
    }

    class ArkiveImageUrls
    {
        static Dictionary<string, List<NetworkFile>> GetLocal(List<string> _speciesUrl, string _file )
        {
            Dictionary<string, List<NetworkFile>> results = new Dictionary<string, List<NetworkFile>>();
            if (File.Exists(_file))
            {
                using (StreamReader reader = new StreamReader(_file))
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
                                Helpers.Log(_file + " contains multiple species " + line);
                            else
                                results[line] = new List<NetworkFile>();
                        }
                        else if (line.StartsWith("http:"))
                        {
                            if (currentSpecies == null)
                                Helpers.Log(_file + " image url outside species " + line);
                            else
                                results[currentSpecies].Add(new NetworkFile(line));
                        }
                        else
                            Helpers.Log(_file + " image url outside species " + line);
                    }
                }

                foreach (string res in _speciesUrl)
                {
                    if (!results.ContainsKey(res))
                        Helpers.Log("Error GetImagesUrl, no key for " + res);
                }
            }
            return results;
        }

        static public Dictionary<string, List<NetworkFile>> Get(List<string> _speciesUrl, bool _localFile, string _file)
        {
            if (_localFile)
                return GetLocal(_speciesUrl, _file);

            Dictionary<string, List<NetworkFile>> results = new Dictionary<string, List<NetworkFile>>();

            try
            {
                if (File.Exists(_file)) File.Delete(_file);

                List<string> speciesUrl = new List<string>(_speciesUrl);
                bool tryOnceMore = true;

                while (speciesUrl.Count > 0)
                {
                    List<string> failedSpeciesUrl = new List<string>();
                    int speciesCount = 0;
                    Parallel.ForEach(speciesUrl, new ParallelOptions { MaxDegreeOfParallelism = 16 }, currentSpeciesUrl =>
                    {
                        List<NetworkFile> images = Helpers.GetSpeciesPhotosOnlyName(currentSpeciesUrl);

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
                            Helpers.Log("GetSpeciesImages can't read these urls");
                            foreach (string url in failedSpeciesUrl)
                                Helpers.Log("    " + url);
                            failedSpeciesUrl.Clear();
                        }
                        else
                        {
                            tryOnceMore = failedSpeciesUrl.Count != speciesUrl.Count;
                            Helpers.Log("GetSpeciesImages can't read " + failedSpeciesUrl.Count + "urls, loop again");
                        }
                    }
                    speciesUrl = failedSpeciesUrl;
                }

                using (StreamWriter writer = new StreamWriter(_file))
                {
                    foreach (string res in _speciesUrl)
                    {
                        if (!results.ContainsKey(res))
                        {
                            Helpers.Log("Error GetImages, no key for " + res);
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

        static void Normalize(string species, List<NetworkFile> _list)
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
            Helpers.Log(species + " some image with same name, have to rename");

            foreach (KeyValuePair<string, int> pair in nameDico)
            {
                if (pair.Value == 1) continue;

                int count = 0;
                foreach (NetworkFile image in _list)
                {
                    if (image.Name.ToLower() != pair.Key)
                        continue;
                    string ext = Path.GetExtension(image.Name);
                    image.Name = Path.GetFileNameWithoutExtension(image.Name) + "_" + count.ToString() + ext;
                    count++;
                }
            }
        }

        static public void Normalize(Dictionary<string, List<NetworkFile>> _images)
        {
            foreach (KeyValuePair<string, List<NetworkFile>> pair in _images)
                Normalize(pair.Key, pair.Value);
        }

        static public void GenerateCsv(Dictionary<string, List<NetworkFile>> _images, string _file)
        {
            if (File.Exists(_file)) File.Delete(_file);

            using (StreamWriter writer = new StreamWriter(_file))
            {
                foreach (var pair in _images)
                {
                    foreach (NetworkFile link in pair.Value)
                    {
                        string[] parts = pair.Key.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 2)
                            Helpers.Log("bad key : " + pair.Key);
                        else
                            writer.WriteLine(parts[1].Replace("-", " ") + ";" + link.URL);
                    }
                }
            }
        }
    }
}
