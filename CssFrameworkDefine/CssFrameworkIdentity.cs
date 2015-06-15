using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ExCSS;
using System.IO;
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
        public List<string> MatchesCss;
        


        /// <summary>
        /// The number of classes to be used this framework
        /// </summary>
        public int UsesClassesCount { get; set; }

        public int AllClassCount { get; set; }

        public List<string> Paths { get; set; }

        

        public List<StyleRule> Stylesheet { get; set; }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="name">Name of framework</param>
        /// <param name="paths">Path to original file</param>
        public CssFrameworkIdentity(string name, List<string> paths)
        {
            FrameworkName = name;
            ExCSS.Parser parser = new ExCSS.Parser();
            Paths = new List<string>();
            Stylesheet = new List<StyleRule>();
            foreach (var path in paths)
            {
                Paths.Add(path);
                if (path == null || !File.Exists(path))
                    throw new FileNotFoundException();
                var style = parser.Parse(File.ReadAllText(path));
                Stylesheet.AddRange(style.StyleRules);
            }
            Stylesheet = Stylesheet.OrderBy(x => x.Value).ToList();
        }
    }
}
