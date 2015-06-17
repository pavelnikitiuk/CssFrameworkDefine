using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CssFrameworkDefine;
using System.Diagnostics;

namespace Test
{

    class Program
    {
        public static List<CssFrameworkIdentity> Load(string path)
        {
            List<CssFrameworkIdentity> frameworks = new List<CssFrameworkIdentity>();
            foreach (var file in Directory.GetFileSystemEntries(path))
                frameworks.Add(new CssFrameworkIdentity(file.Split('\\').Last(), Directory.GetFiles(file).ToList()));
            return frameworks;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Write path to tests");
            //var str = Console.ReadLine();
            var str = "D:\\tests";

            Definer d = new Definer();
            foreach (var name in Load("D:\\CssFrameworks"))
            {
                d.AddFramework(name.FrameworkName, name.Paths.ToArray());


            }

            Stopwatch timer = new Stopwatch();


            File.WriteAllText("D:\\tests\\result.txt", "");

            foreach (var folder in Directory.EnumerateDirectories(str))
            {
                foreach (var file in Directory.EnumerateFiles(folder))
                {
                    Console.WriteLine("{0}", file);
                    var res = d.Define(File.ReadAllText(file), out timer);
                    if (res != null && res.ContainsKey(d.MostSuitableFramework) && res[d.MostSuitableFramework] > 10)
                        File.AppendAllText("D:\\tests\\result.txt",
                            String.Format("File: {0} use {1} time {2}   matches {3} \n", file, d.MostSuitableFramework, timer.ElapsedMilliseconds,
                            (res != null && res.ContainsKey(d.MostSuitableFramework)) ? res[d.MostSuitableFramework].ToString() : "0"));
                }
            }

        }
    }
}
