using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CssFrameworkDefine.Model;
using ExCSS;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace CssFrameworkDefine
{
    public class Definer
    {
        /// <summary>
        /// Dictonary of css properties
        /// </summary>
        private Dictionary<string, int> CssProperties;
        /// <summary>
        /// list of original css frameworks
        /// </summary>
        private List<OriginalCssFramework> originalFrameworks;
        /// <summary>
        /// Css Parser
        /// </summary>
        private Parser parser;
        /// <summary>
        /// Name of result framework
        /// </summary>
        public string MostSuitableFramework { get; private set; }

        public Definer()
        {
            CssProperties = new Dictionary<string, int>();
            //Add all properties in dictonary
            foreach (var property in ConstantsProperties.CssProperties)
            {
                CssProperties.Add(property.Name, property.Id);
            }

            parser = new Parser();

            originalFrameworks = new List<OriginalCssFramework>();
        }
        /// <summary>
        /// Add original framework
        /// </summary>
        /// <param name="name">Name of Framework</param>
        /// <param name="paths">Array of paths in framework </param>
        public void AddFramework(string name, params string[] paths)
        {
            OriginalCssFramework toAdd = new OriginalCssFramework { Name = name, Styles = new List<Dictionary<string, bool[]>>() };
            foreach (var path in paths)
            {
                toAdd.Styles.Add(Load(path));
            }
            originalFrameworks.Add(toAdd);
        }
        /// <summary>
        /// Add original framework
        /// </summary>
        /// <param name="name">Name of Framework</param>
        /// <param name="paths">Array of paths in framework </param>
        /// <param name="time">Time of work</param>
        public void AddFramework(string name, out Stopwatch time, params string[] paths)
        {
            time = new Stopwatch();
            time.Start();
            AddFramework(name, paths);
            time.Stop();
        }


        private List<string> DetectFramework(string selector, bool[] property)
        {
            List<string> frameworkNames = new List<string>();
            foreach (var framework in originalFrameworks)
            {
                foreach (var style in framework.Styles)
                {
                    if (style.ContainsKey(selector))
                    {
                        if (style[selector].SequenceEqual(property))
                        {
                            frameworkNames.Add(framework.Name);
                            break;
                        }
                    }

                }
            }

            return frameworkNames;
        }

        /// <summary>
        /// Start define framework
        /// </summary>
        /// <param name="css">css file</param>
        /// <param name="t">time of work</param>
        /// <returns>Dictionary key - the name of the framework, the value - the number of matches found</returns>
        public Dictionary<string, int> Define(string css, out Stopwatch t)
        {
            t = new Stopwatch();
            t.Start();
            var ret = Define(css);
            t.Stop();
            return ret;
        }
        /// <summary>
        /// Start define framework
        /// </summary>
        /// <param name="css">css file</param>
        /// <returns>Dictionary key - the name of the framework, the value - the number of matches found</returns>
        public Dictionary<string, int> Define(string css)
        {
            Dictionary<string, int> results = new Dictionary<string, int>();

            //Add all original framework
            foreach (var framework in originalFrameworks)
            {
                results.Add(framework.Name, 0);
            }

            IList<ExCSS.StyleRule> stylesheet;
            try
            {
                stylesheet = parser.Parse(css).StyleRules;
            }
            catch
            {
                throw;
            }
            if (stylesheet.Any(x => x.Selector == null))
                throw new CssDefineException("CssDefineException");

            var styleRules = Load(stylesheet);

            foreach (var rule in styleRules)
            {

                var frameworksName = DetectFramework(rule.Key, rule.Value);
                foreach (var name in frameworksName)
                {
                    results[name]++;
                }
            }
            MostSuitableFramework = results.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            return results;
        }

        
        private Dictionary<string, bool[]> Load(string path)
        {
            if (path == null)
                throw new ArgumentNullException("Path can not be null");
            if (!File.Exists(path))
                throw new FileNotFoundException("This file is not exists");

            return Load(parser.Parse(File.ReadAllText(path)).StyleRules);
        }

        private Dictionary<string, bool[]> Load(IList<ExCSS.StyleRule> rules)
        {
            var dictonary = new Dictionary<string, bool[]>();
            foreach (var rule in rules)
            {
                var mask = new bool[375];
                foreach (var propery in rule.Declarations.Properties)
                {
                    if (CssProperties.ContainsKey(propery.Name))
                        mask[CssProperties[propery.Name]] = true;
                }


                var key = rule.Selector.ToString();
                if (dictonary.ContainsKey(key))
                    for (int i = 0; i < mask.Length; i++)
                    {
                        dictonary[key][i] = mask[i] || dictonary[key][i];
                    }
                else
                    dictonary.Add(key, mask);
            }
            //return dictonary selector- properties mask
            return dictonary;
        }
    }
}
