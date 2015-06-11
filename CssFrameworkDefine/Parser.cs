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
                BaseUrl = "http://" + value.Split('/')[2] + '/';
            }
        }
        /// <summary>
        /// Base url website
        /// </summary>
        private string BaseUrl { get; set; }
        /// <summary>
        /// Class CssFrameworkIdentity which will be searched
        /// </summary>
        public CssFrameworkIdentity SearchinFramework { get; set; }
        /// <summary>
        /// HtmlDokument website
        /// </summary>
        private HtmlDocument siteHtml;
        
        /// <summary>
        /// Path to original css
        /// </summary>
        public string CssPath { get; set; }

        public Parser()
        {
            //SearchinFramework.AllClassCount = 0;

            //SearchinFramework.UsesClassesCount = 0;
            siteHtml = new HtmlDocument();

            
        }
        /// <summary>
        /// Start parse
        /// </summary>
        public void Parse()
        {
            //AllClassCount = 0;
            SearchinFramework.matchesCss = new List<string>();
            
            //UsesClassesCount = 0;
            if (Url == null)
                throw new UrlNotFounException("You must specify the url");
            siteHtml.LoadHtml(Html.Load(Url));
            CheckLinks();
            CheckClasses();
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
            var stylesheet = parsingCss.Parse(cssText);
            //Parse original css
            if (SearchinFramework.Paths[0] == null || !File.Exists(SearchinFramework.Paths[0]))
                throw new PathNotFounException("Can not find file!");
            var original = File.ReadAllText(SearchinFramework.Paths[0]);
            var stylesheetOriginal = parsingCss.Parse(original);
            //Compare two css
            SearchinFramework.matchesCss.AddRange(CssComparer.Compare(stylesheetOriginal, stylesheet));
            SearchinFramework.AllClassCount += stylesheet.StyleRules.Count;
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
            foreach (HtmlNode link in siteHtml.DocumentNode.SelectNodes("//link[@href]"))
            {

                var href = link.Attributes.FirstOrDefault(x => x.Name == "href"); //take href
                if (href == null || link.Attributes.FirstOrDefault(x => x.Name == "rel").Value != "stylesheet")
                    continue;

                string hrefValue = href.Value;
                if (hrefValue.Contains("//") && !hrefValue.Contains("http://"))
                    hrefValue = "http:" + hrefValue;
                else
                if (!hrefValue.Contains("http://"))
                    hrefValue = BaseUrl + hrefValue;
                
                ParseCss(hrefValue);
                SearchinFramework.FindLinks.Add(hrefValue);
            }
            return 0;
        }
    }
}
