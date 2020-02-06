using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Arkive
{
    class ArkiveImages
    {
        public class DiffResult
        {
            public Dictionary<string, List<NetworkFile>> Missings = new Dictionary<string, List<NetworkFile>>();
            public Dictionary<string, List<string>> OldLocals = new Dictionary<string, List<string>>();
            public int MissingImages = 0;
            public int OldImages = 0;
            public void Count()
            {
                MissingImages = 0;
                foreach (var list in Missings.Values) MissingImages += list.Count;
                OldImages = 0;
                foreach (var list in OldLocals.Values) OldImages += list.Count;
            }
        }

        static public DiffResult ComputeDiff(Dictionary<string, List<NetworkFile>> _urls, Dictionary<string, List<string>> _locals )
        {
            DiffResult result = new DiffResult();

            foreach (var pair in _locals)
                result.OldLocals[pair.Key] = new List<string>(pair.Value);

            foreach (KeyValuePair<string, List<NetworkFile>> pair in _urls)
            {
                string species = pair.Key.ToLower();
                if (!_locals.ContainsKey(species))
                    result.Missings[pair.Key] = pair.Value;
                else
                {
                    List<string> localImages = new List<string>(_locals[species]);
                    foreach (NetworkFile netImage in pair.Value)
                    {
                        string imageName = netImage.Name.ToLower();
                        if (localImages.Contains(imageName))
                        {
                            localImages.Remove(imageName);
                            continue;
                        }
                        if (!result.Missings.ContainsKey(pair.Key))
                            result.Missings[pair.Key] = new List<NetworkFile>();
                        result.Missings[pair.Key].Add(netImage);
                    }
                    if (localImages.Count == 0)
                        result.OldLocals.Remove(species);
                    else
                        result.OldLocals[species] = localImages;
                }
            }
            result.Count();
            return result;
        }

        static public void RemoveOld( DiffResult _diff )
        {
            foreach (KeyValuePair<string, List<string>> pair in _diff.OldLocals)
            {
                foreach (string image in pair.Value)
                {
                    string fullfilename = Helpers.GetImagePath(pair.Key, image);
                    if (File.Exists(fullfilename))
                        File.Delete(fullfilename);
                }
            }
        }

        static public void GetMissings( DiffResult _diff )
        {
            WebClient webClient = new WebClient();

            List<Tuple<string, NetworkFile>> list = new List<Tuple<string, NetworkFile>>();
            foreach (KeyValuePair<string, List<NetworkFile>> pair in _diff.Missings)
            {
                if (pair.Value == null || pair.Value.Count == 0)
                    continue;
                foreach (NetworkFile url in pair.Value)
                    list.Add(new Tuple<string, NetworkFile>(pair.Key, url));
                if (list.Count > 10) break;
            }

            bool tryOnceMore = true;
            bool medium = false;
            while (list.Count > 0)
            {
                List<Tuple<string, NetworkFile>> failed = new List<Tuple<string, NetworkFile>>();
                int imageCount = 0;
                //Parallel.ForEach(list, new ParallelOptions { MaxDegreeOfParallelism = 4 }, currentTuple =>
                foreach (Tuple<string, NetworkFile> currentTuple in list)
                {
                    string folderRoot = Helpers.CreateSpeciesPath(currentTuple.Item1);
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
                            Helpers.Log("GetSpeciesImages can't read these images");
                            foreach (Tuple<string, NetworkFile> tuple in failed)
                                Helpers.Log("    " + tuple.Item1 + " - " + tuple.Item2.URL + " => " + tuple.Item2.Name);
                            failed.Clear();
                        }
                    }
                    else
                    {
                        tryOnceMore = failed.Count != list.Count;
                        Helpers.Log("GetSpeciesImages can't read " + failed.Count + " images, loop again");
                    }
                }
                list = failed;
            }
        }
    }
}
