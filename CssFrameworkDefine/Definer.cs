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
        private Dictionary<string, int> CssProperties;

        private List<OriginalCssFramework> originalFrameworks;

        private Parser parser;

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

        public void AddFramework(string name, params string[] paths)
        {
            OriginalCssFramework toAdd = new OriginalCssFramework { Name = name, Styles = new List<Dictionary<string, BitArray>>() };
            foreach (var path in paths)
            {
                toAdd.Styles.Add(Load(path));
            }
            originalFrameworks.Add(toAdd);
        }
        public void AddFramework(string name, out Stopwatch time, params string[] paths)
        {
            time = new Stopwatch();
            time.Start();
            AddFramework(name, paths);
            time.Stop();
        }


        private List<string> DetectFramework(string selector, BitArray property)
        {
            List<string> frameworkNames = new List<string>();
            foreach (var framework in originalFrameworks)
            {
                foreach (var style in framework.Styles)
                {
                    if (style.ContainsKey(selector))
                    {
                        if (CompareBitArrays(style[selector], property))
                        {
                            frameworkNames.Add(framework.Name);
                            break;
                        }
                    }

                }
            }

            return frameworkNames;
        }

        private static bool CompareBitArrays(BitArray first, BitArray second)
        {
            if (first.Length != second.Length)
                return false;
            for (int i = 0; i < first.Length; i++)
                if (first[i] != second[i])
                    return false;
            return true;
        }

        public Dictionary<string, int> Define(string css, out Stopwatch t)
        {
            t = new Stopwatch();
            t.Start();
            var ret = Define(css);
            t.Stop();
            return ret;
        }

        public Dictionary<string, int> Define(string css)
        {
            Dictionary<string, int> results = new Dictionary<string, int>();


            foreach (var framework in originalFrameworks)
            {
                results.Add(framework.Name, 0);
            }
            try
            {

                var stylesheet = parser.Parse(css).StyleRules;

                if (stylesheet.Any(x => x.Selector == null))
                    return null;

                var styleRules = ListToDictonary(stylesheet);

                foreach (var rule in styleRules)
                {
                    var frameworksName = DetectFramework(rule.Key, rule.Value);
                    foreach (var name in frameworksName)
                    {
                        results[name]++;
                    }
                }
                MostSuitableFramework = results.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                if (results[MostSuitableFramework] < 3)
                    MostSuitableFramework = "Unknown Framework";

                return results;
            }
            catch { return null; }
        }


        private Dictionary<string, BitArray> Load(string path)
        {
            if (path == null)
                throw new ArgumentNullException("Path can not be null");
            if (!File.Exists(path))
                throw new FileNotFoundException("This file is not exists");

            return ListToDictonary(parser.Parse(File.ReadAllText(path)).StyleRules);
        }

        private Dictionary<string, BitArray> ListToDictonary(IList<ExCSS.StyleRule> rules)
        {
            var dictonary = new Dictionary<string, BitArray>();
            foreach (var rule in rules)
            {
                var mask = new BitArray(375);
                foreach (var propery in rule.Declarations.Properties)
                {
                    if (CssProperties.ContainsKey(propery.Name))
                        mask.Set(CssProperties[propery.Name], true);
                }
            

                    var key = rule.Selector.ToString();
                    if (dictonary.ContainsKey(key))
                        dictonary[key] = mask.Or(dictonary[key]);
                    else
                        dictonary.Add(key, mask);
                }
               
            
            return dictonary;
        }
    }
}
