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
        /// Class names are the same as the implementation
        /// </summary>
        public List<string> matchesCss;
        /// <summary>
        /// Results links from SearchLinks
        /// </summary>
        public List<string> FindLinks { get; set; }
        /// <summary>
        /// Result classes from class search
        /// </summary>
        public List<HtmlNode> FindClasses { get; set; }
        /// <summary>
        /// Result of serch in css
        /// </summary>
        public int FindMatchesInCss {get;  set;}
        /// <summary>
        /// The number of classes to be used this framework
        /// </summary>
        public int UsesClassesCount { get; set; }
        public int AllClassCount { get; set; }

        public List<string> Paths { get; set; }

        public CssFrameworkIdentity()
        {
            FindClasses = new List<HtmlNode>();
            FindLinks = new List<string>();
        }





    }
}
