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
            OriginalCssFramework toAdd = new OriginalCssFramework { Name = name };
            foreach (var path in paths)
            {
                Load(path);
            }
        }
        public void AddFramework(string name, out Stopwatch time, params string[] paths)
        {
            time = new Stopwatch();
            time.Start();
            AddFramework(name, paths);
            time.Stop();
        }

      
           

        private Dictionary<string,BitArray> Load(string path)
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
                foreach(var propery in rule.Declarations.Properties)
                {
                    if(CssProperties.ContainsKey(propery.Name))
                        mask.Set(CssProperties[propery.Name], true);
                }
                var key = rule.Selector.ToString();
                if (!dictonary.ContainsKey(key))
                //    dictonary[key] = mask.Or(dictonary[key]);
                //else
                    dictonary.Add(key, mask);
            }
            return dictonary;
        }
    }
}
