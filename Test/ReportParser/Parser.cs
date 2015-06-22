using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Test.ReportParser
{
    public class Parser
    {
        public List<Model> Report { get; set; }
        public Parser()
        {
            Report = new List<Model>();
        }
        public void Parse(string path)
        {
            foreach(var line in File.ReadLines(path))
            {
                var texts = line.Split(' ');
                if(texts.Length <=1)
                    break;
                Model p = new Model();
                p.Path = texts[1];
                p.Name = p.Path.Split('\\').Last();
                p.Count = Convert.ToInt32( texts[texts.Length-2]);
                p.Framework = texts[3];
                p.Time = Convert.ToInt32( texts[5]);
                Report.Add(p);
            }
        }
    }
}
