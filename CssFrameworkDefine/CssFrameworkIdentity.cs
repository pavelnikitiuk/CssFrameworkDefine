using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
namespace CssFrameworkDefine
{
    public class CssFrameworkIdentity
    {
        /// <summary>
        /// Name of Framework
        /// </summary>
        public string FrameworkName { get; set; }
        /// <summary>
        /// Regex name of link 
        /// </summary>
        public List<string> SearchLinks { get; set; }
        /// <summary>
        /// Regex names of the classes that can be used by the framework
        /// </summary>
        public List<string> SearchDivClasses { get; set; }
        /// <summary>
        /// Regex names of the classes thet can be used by the framework in css
        /// </summary>
        public List<string> NameOfClassInCss { get; set; }
        ///<summary>
        /// Regex names of the classes thet can be used by the framework in css
        /// </summary>
        public List<string> JsContent { get; set; }
        /// <summary>
        /// Results links from SearchLinks
        /// </summary>
        public List<HtmlNode> FindLinks { get; set; }
        /// <summary>
        /// Result classes from class search
        /// </summary>
        public List<HtmlNode> FindClasses { get; set; }
        /// <summary>
        /// Result of serch in css
        /// </summary>
        public int FindMatchesInCss {get;  set;}

        public CssFrameworkIdentity()
        {
            FindClasses = new List<HtmlNode>();
            FindLinks = new List<HtmlNode>();
        }





    }
}
