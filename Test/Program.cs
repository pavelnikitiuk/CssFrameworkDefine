using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CssFrameworkDefine;
using System.Diagnostics;
using ExCSS;

namespace Test
{

    class Program
    {
        public static Dictionary<string, string[]> Load(string path)
        {
            Dictionary<string, string[]> frameworks = new Dictionary<string, string[]>();
            foreach (var file in Directory.GetFileSystemEntries(path))
                frameworks.Add(file.Split('\\').Last(), Directory.GetFiles(file));
            return frameworks;
        }


        public static void Start(string testsPath, string frameworksPath, string pattern = "*.css")
        {
            var stream1 = File.Create("Table.csv");
            stream1.Dispose();
            File.AppendAllText("Table.csv", "Table,");

            Definer d = new Definer();
            //Add all frameworks
            foreach (var name in Load(frameworksPath))
            {
                int i = 0;
                Parser p = new Parser();
                foreach (var file in name.Value)
                    d.AddFramework(name.Key, File.ReadAllText(file));
                
                File.AppendAllText("Table.csv", String.Format("{0},", name.Key));
            }

            Stopwatch timer = new Stopwatch();
            timer.Start();

            var stream = File.Create(Properties.Resources.Result);
            stream.Dispose();

            
                foreach (var file in Directory.EnumerateFiles(testsPath,"*.txt",SearchOption.AllDirectories))
                {
                    Console.WriteLine(file);
                    try
                    {
                        var result = d.Define(File.ReadAllText(file));
                        var framework = result.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                        KeyValuePair<string, double> answer;
                        if(result[framework] == 0)
                            answer = new KeyValuePair<string, double>("Unknown Framework", 0);
                        else
                            answer = new KeyValuePair<string, double>(framework, result[framework]);
                        File.AppendAllText("result.txt", String.Format("File {0} use {1} % {2}\n", file, answer.Key, answer.Value.ToString("F2")));
                    }
                    catch
                    {
                        File.AppendAllText("result.txt", "It does not css");
                    }
                }
            
            
            timer.Stop();
            File.AppendAllText(Properties.Resources.Result, String.Format("Tottal time - {0}", timer.ElapsedMilliseconds.ToString()));
        }


        static void Main(string[] args)
        {
            var path = "D:\\projects\\Tests\\CssDefiner\\AllTests";
            var frameworkPath = "D:\\CssFrameworks";

            Start(path, frameworkPath);
        }
    }
}
