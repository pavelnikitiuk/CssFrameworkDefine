using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace CssFrameworkDefine
{
    public class CssLoader
    {

        private HtmlDocument document;
        private string scheme;
        private string baseUrl;

        public CssLoader(string url)
        {
            if (!Regex.IsMatch(url, @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$"))
                throw new ArgumentException("You must specify the url");

            scheme = url.Contains("https") ? "https:" : "http:";
            baseUrl = scheme + "//" + url.Split('/')[2] + '/';
            document = new HtmlDocument();
            document.LoadHtml(Load(url));
        }

        public static string Load(string url)
        {
            using (var myWebClient = new WebClient())
            {
                myWebClient.Headers["User-Agent"] = "MOZILLA/5.0 (WINDOWS NT 6.1; WOW64) APPLEWEBKIT/537.1 (KHTML, LIKE GECKO) CHROME/21.0.1180.75 SAFARI/537.1";
                return myWebClient.DownloadString(url);
            }
        }

        public IEnumerable<string> LoadLinks()
        {
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//link[@rel='stylesheet']"))
            {
                var href = link.Attributes.FirstOrDefault(x => x.Name == "href"); //take href
                if (href == null)
                    continue;
                yield return Load(MakeUrl(href.Value));
            }
        }
        private string MakeUrl(string str)
        {
            if (str.Contains("//") && !str.Contains("http"))
                str = scheme + str;
            else
                if (!str.Contains("http"))
                    str = baseUrl + str;
            return str;
        }
    }
}
