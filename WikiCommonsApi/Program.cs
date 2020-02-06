using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestApiPhpCall
{
    class Program
    {
        static void Main(string[] args)
        {
            List<WCApiCallSearch.PageResult> res = new WCApiCallSearch("salamandra salamandra").GetResults();
            WriteToFile(string.Join(", ", res.Select(t => t.pageid)));

            foreach(WCApiCallSearch.PageResult r in res )
            {
                WPApiCallPageImages apicall = new WPApiCallPageImages(r.pageid);
                WriteToFile( apicall.Call());
                List<WPApiCallPageImages.Image> images = apicall.GetResults();
                WriteToFile(string.Join("\n", images));

            }
        }

        static int FileCount = 0;
        static void WriteToFile(string _result )
        {
            string filename = "c:\\temp\\result_" + FileCount.ToString("D2") + ".txt";
            System.IO.File.WriteAllText(filename, _result);
            FileCount++;
        }

    }
}
