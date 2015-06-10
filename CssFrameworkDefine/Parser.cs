using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace CssFrameworkDefine
{
    public class Parser
    {
        public string Url { get; private set; }
        public string BaseUrl { get; private set; }
        public List<CssFrameworkIdentity> SearchinFramework { get; set; }
        public string Css { get; set; }
        private HtmlDocument siteHtml;
        public Parser(string url)
        {
            Url = url;
            BaseUrl = "http://" + url.Split('/')[2];
            siteHtml = new HtmlDocument();
            SearchinFramework = new List<CssFrameworkIdentity>();
            var s = Html.Load(url);
            siteHtml.LoadHtml(s);
        }
        public int Parse()
        {
            foreach (var x in SearchinFramework)
            {
                CheckClasses(x);
                CheckLinks(x);
            }
            return 0;
        }
        private void ParseCss(CssFrameworkIdentity css, string url)
        {
            if (!url.Contains("http://"))
                url = BaseUrl + url.Substring(url.IndexOf('/'));
            string cssText;
            try
            {
                cssText = Html.Load(url);
            }
            catch
            {
                return;
            }
            foreach (var searchCss in css.NameOfClassInCss)
            {
                css.FindMatchesInCss += new Regex(searchCss).Matches(cssText).Count;
            }

        }
        private void CheckClasses(CssFrameworkIdentity classes)
        {
            foreach (HtmlNode div in siteHtml.DocumentNode.SelectNodes("//div").Where(x => x.Attributes.Contains("class")))
            {
                string s = classes.SearchDivClasses[0];
                foreach (var fClass in classes.SearchDivClasses)
                {
                    if (Regex.IsMatch(div.Attributes.FirstOrDefault(x => x.Name == "class").Value, fClass))
                        classes.FindClasses.Add(div);
                }
            }
        }
        //Check links 
        private int CheckLinks(CssFrameworkIdentity classes)
        {
            foreach (HtmlNode link in siteHtml.DocumentNode.SelectNodes("//link[@href]"))
            {
                var href = link.Attributes.FirstOrDefault(x => x.Name == "href"); //take href
                if (href == null)
                    continue;
                var stylesheet = link.Attributes.FirstOrDefault(x => x.Name == "rel");
                if (stylesheet != null && stylesheet.Value == "stylesheet")
                    ParseCss(classes, href.Value);
                foreach (var fLink in classes.SearchLinks)
                {
                    if (href.Value.Contains(fLink))
                        classes.FindLinks.Add(link);
                }
            }
            return 0;
        }
    }
}
