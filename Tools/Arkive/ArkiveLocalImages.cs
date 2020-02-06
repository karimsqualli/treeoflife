using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkive
{
    static class ArkiveLocalImages
    {
        static public Dictionary<string, List<string>> Get(string _folder)
        {
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();

            String[] allfiles = System.IO.Directory.GetFiles(_folder, "*.jpg", System.IO.SearchOption.AllDirectories);
            int prefixLength = _folder.Length;
            foreach (string file in allfiles)
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
