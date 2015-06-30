using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CssFrameworkDefine.Model;
using ExCSS;
using System.Collections;
using System.Diagnostics;


namespace CssFrameworkDefine
{
    public class Definer
    {
        /// <summary>
        /// Dictionary of css properties
        /// </summary>
        private Dictionary<string, int> CssProperties;
        /// <summary>
        /// list of original css frameworks
        /// </summary>
        private Dictionary<Style, List<FrameworkName>> originalFrameworks;
        /// <summary>
        /// Threshold value for detect framewotk
        /// </summary>
        public float ThresholdValue {get; set;}
        /// <summary>
        /// Dictonary of name and rule count in framework
        /// </summary>
        private Dictionary<string, List<int>> frameworksNameCollection { get; set; }
        /// <summary>
        /// Css Parser
        /// </summary>
        private Parser parser;
        /// <summary>
        /// Name of result framework and % of matches
        /// </summary>
        public KeyValuePair<string, float> MostSuitableFramework 
        {
            get
            {
                return lastСheck.FirstOrDefault(x => x.Value > ThresholdValue);
            }
        }
        /// <summary>
        /// Dictonary of last check
        /// </summary>
        private Dictionary<string, float> lastСheck;

        public Definer()
        {

            frameworksNameCollection = new Dictionary<string, List<int>>();

            CssProperties = new Dictionary<string, int>();

            originalFrameworks = new Dictionary<Style, List<FrameworkName>>();

            parser = new Parser();

            //Add all properties in Dictionary
            foreach (var property in ConstantsProperties.CssProperties)
            {
                CssProperties.Add(property.Name, property.Id);
            }
        }
        /// <summary>
        /// Add original framework
        /// </summary>
        /// <param name="name">Name of Framework</param>
        /// <param name="paths">Array of framework </param>
        public void AddFramework(string name, string css)
        {
            var dictionary = Load(css);

            if (frameworksNameCollection.ContainsKey(name))
                frameworksNameCollection[name].Add(dictionary.Count);
            else frameworksNameCollection.Add(name, new List<int> { dictionary.Count });
            //Framework contains for name of all framework and number of file

            foreach (var style in dictionary)
            {
                var toAdd = new Style { Properties = style.Value, Name = style.Key };
                if (originalFrameworks.ContainsKey(toAdd))
                    originalFrameworks[toAdd].Add(new FrameworkName { Name = name, Number = frameworksNameCollection[name].Count });
                else
                    originalFrameworks.Add(toAdd, new List<FrameworkName> { new FrameworkName { Name = name, Number = frameworksNameCollection[name].Count } });
            }
        }

        /// <summary>
        /// Start define framework
        /// </summary>
        /// <param name="css">css file</param>
        /// <param name="t">time of work</param>
        /// <returns>Dictionary key - the name of the framework, the value - the number of matches found</returns>
        public Dictionary<string, float> Define(string css, out Stopwatch t)
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
        public Dictionary<string, float> Define(string css)
        {
            if (ThresholdValue == 0)
                ThresholdValue = 70;
            Dictionary<string, int[]> searchResult = new Dictionary<string, int[]>();
            IList<ExCSS.StyleRule> stylesheet;
            try
            {
                stylesheet = parser.Parse(css).StyleRules;
            }
            catch
            {
                throw new CssDefineException(Properties.Resources.Exception);
            }
            var styleRules = Load(stylesheet);
            var style = new Style();
            foreach (var rule in styleRules)
            {
                style.Name = rule.Key;
                style.Properties = rule.Value;
                if (!originalFrameworks.ContainsKey(style))
                    continue;
                foreach (var name in originalFrameworks[style])
                {
                    if (searchResult.ContainsKey(name.Name))
                        searchResult[name.Name][name.Number-1]++;
                    else
                    {
                        searchResult.Add(name.Name, new int[frameworksNameCollection[name.Name].Count]);
                        searchResult[name.Name][name.Number-1]++;
                    }
                }
            }
            var a = GetAnswer(searchResult);
            lastСheck = new Dictionary<string, float>(a);
            return a;
        }


      

        private Dictionary<string,float> GetAnswer(Dictionary<string,int[]> matchesResult)
        {
            Dictionary<string, float> result = new Dictionary<string, float>();

            foreach (var name in frameworksNameCollection.Keys)
            {
                result.Add(name, 0);
            }
            foreach (var name in matchesResult.Keys)
            {
                var frameworkname = frameworksNameCollection[name];
                var sresult = matchesResult[name];
                for (int i = 0; i < frameworkname.Count; i++)
                    result[name] += (float)sresult[i] / frameworkname[i];
                result[name] /= (float)frameworkname.Count / 100;
            }
            
            return result;
        }

        private Dictionary<string, BitMask> Load(string css)
        {
            IList<StyleRule> style;
            try
            {
                style = parser.Parse(css).StyleRules;
            }
            catch { throw new CssDefineException(Properties.Resources.Exception); }
            
            return Load(style);
        }

        private Dictionary<string, BitMask> Load(IList<ExCSS.StyleRule> rules)
        {
            if (rules.Any(x => x.Selector == null))
                throw new CssDefineException("CssDefineException");
            Style toAdd = new Style();

            var Dictionary = new Dictionary<string, BitMask>();
            foreach (var rule in rules)
            {
                var mask = new BitMask();

                foreach (var propery in rule.Declarations.Properties)
                    if (CssProperties.ContainsKey(propery.Name))
                        mask[CssProperties[propery.Name]] = true;
                var key = rule.Selector.ToString();
                if (Dictionary.ContainsKey(key))
                    Dictionary[key] |= mask;
                else
                    Dictionary.Add(key, mask);
            }
            //return Dictionary selector- properties mask
            return Dictionary;
        }

    }
}
