using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.IO;
namespace CssFrameworkDefine
{
    public class Parser
    {
        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                baseUrl = scheme + "//" + value.Split('/')[2] + '/';
            }
        }
        /// <summary>
        /// Base url website
        /// </summary>
        private string baseUrl { get; set; }
        private string scheme;
        /// <summary>
        /// Class CssFrameworkIdentity which will be searched
        /// </summary>
        public CssFrameworkIdentity SearchinFramework { get; set; }
        /// <summary>
        /// HtmlDokument website
        /// </summary>
        private HtmlDocument siteHtml = new HtmlDocument();

        /// <summary>
        /// Path to original css
        /// </summary>
        public string CssPath { get; set; }

        private List<ExCSS.StyleSheet> styles;

        /// <summary>
        /// Start parse
        /// </summary>
        public void Parse()
        {
           
            //AllClassCount = 0;
            SearchinFramework.matchesCss = new List<string>();
            CheckClasses();
            //Compare two css
            foreach (var style in styles)
            {
                var result = CssComparer.Compare(SearchinFramework.Stylesheet, style.StyleRules.OrderBy(x => x.Value).ToList());
                if (result.Count != 0)
                {
                    SearchinFramework.matchesCss.AddRange(result);
                    SearchinFramework.AllClassCount += style.StyleRules.Count;
                }
            }

        }
        /// <summary>
        /// Load Css file and parse him
        /// </summary>
        /// <param name="css">original css to compare</param>
        /// <param name="url">Css url</param>
        private void ParseCss(string url)
        {
            //Load Css 
            string cssText;
            try { cssText = Html.Load(url); }
            catch { return; }
            //Parse load Css
            ExCSS.Parser parsingCss = new ExCSS.Parser();
            styles.Add(parsingCss.Parse(cssText));
        }
        public Parser(string url)
        {
            if (url == null)
                throw new UrlNotFounException("You must specify the url");
            scheme = url.Contains("https") ? "https:" : "http:";
            Url = url;
            siteHtml.LoadHtml(Html.Load(Url));
            styles = new List<ExCSS.StyleSheet>();
            CheckLinks();
        }
        private void CheckClasses()
        {
            foreach (var css in SearchinFramework.matchesCss)
            {
                var useClasses = siteHtml.DocumentNode.SelectNodes(string.Format("//*[contains(@class,'{0}')]", css));
                if (useClasses != null)
                    SearchinFramework.UsesClassesCount += useClasses.Count;
            }
        }
        /// <summary>
        /// Finde links on css and parse it
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        private int CheckLinks()
        {
            foreach (HtmlNode link in siteHtml.DocumentNode.SelectNodes("//link[@rel='stylesheet']"))
            {

                var href = link.Attributes.FirstOrDefault(x => x.Name == "href"); //take href
                if (href == null)
                    continue;

                string hrefValue = href.Value;
                if (hrefValue.Contains("//") && !hrefValue.Contains("http"))
                    hrefValue = scheme + hrefValue;
                else
                    if (!hrefValue.Contains("http"))
                        hrefValue = baseUrl + hrefValue;

                ParseCss(hrefValue);
                // SearchinFramework.FindLinks.Add(hrefValue);
            }
            return 0;
        }
    }
}
