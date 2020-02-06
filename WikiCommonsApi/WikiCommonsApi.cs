using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace TestApiPhpCall
{
    public class WCApiCall
    {
        public List<string> Args;

        public WCApiCall() { Args = new List<string>(); }
        public WCApiCall(string arg0) { Args = new List<string>() { arg0 }; }
        public WCApiCall(string arg0, string arg1) { Args = new List<string>() { arg0, arg1 }; }


        static string urlBase = "https://commons.wikimedia.org/w/api.php?";

        public const string argQueryAction = "action=query";
        public const string argFormatJson = "format=json";

        public string Call()
        {
            string url = urlBase + string.Join("&", Args);
            using (var client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }

    //========================================================================
    public class WCApiCallSearch : WCApiCall
    {
        public const string argSearch = "list=prefixsearch";

        public WCApiCallSearch(string _toFind) : base(argQueryAction, argFormatJson)
        {
            Args.Add(argSearch);
            Args.Add("pssearch=" + _toFind.Replace(" ", "+"));
        }

        public List<PageResult> GetResults()
        {
            string raw = Call();
            Result result = new JavaScriptSerializer().Deserialize<Result>(raw);
            return result.query.prefixsearch;
        }


        public class PageResult
        {
            public string ns { get; set; }
            public string title { get; set; }
            public string pageid { get; set; }
        }

        public class Query
        {
            public List<PageResult> prefixsearch { get; set; }
        }

        public class Result
        {
            public string batchcomplete { get; set; }
            public Query query { get; set; }
        }
    }

    //========================================================================
    public class WPApiCallPageImages : WCApiCall
    {

        public WPApiCallPageImages(string _pageId) : base(argQueryAction, argFormatJson)
        {
            Args.Add("prop=images");
            Args.Add("imlimit=500");
            Args.Add("pageids=" + _pageId);
            _PageId = _pageId;
        }

        readonly string _PageId;

        public List<Image> GetResults()
        {
            string raw = Call();

            JObject j = JObject.Parse(raw);
            if (j == null) return null;
            JToken token = j.GetValue("query");
            token = token?.SelectToken("pages")?.SelectToken(_PageId);
            if (token == null) return null;
            string s = token.ToString();


            Page result = new JavaScriptSerializer().Deserialize<Page>(s);
            return result.images.ToList();
        }

        public class Page
        {
            public int pageid { get; set; }
            public int ns { get; set; }
            public string title { get; set; }
            public Image[] images { get; set; }
        }

        public class Image
        {
            public int ns { get; set; }
            public string title { get; set; }

            public override string ToString() { return title; }
        }
    }
}
