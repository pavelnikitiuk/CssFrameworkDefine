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
        public static Dictionary<string, string[]> Load(string path)
        {
            Dictionary<string, string[]> frameworks = new Dictionary<string, string[]>();
            foreach (var file in Directory.GetFileSystemEntries(path))
                frameworks.Add(file.Split('\\').Last(), Directory.GetFiles(file));
            return frameworks;
        }


        public static void Start(string testsPath, string frameworksPath, string pattern = "*.txt")
        {


            Definer d = new Definer();
            //Add all frameworks
            foreach (var name in Load(frameworksPath))
                d.AddFramework(name.Key, name.Value);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            var stream = File.Create(Properties.Resources.Result);
            stream.Dispose();
            foreach (var file in Directory.EnumerateFiles(testsPath, pattern, SearchOption.AllDirectories))
            {
                Console.WriteLine("{0}", file);
                try
                {
                    var res = d.Define(File.ReadAllText(file));
                    var resultframework = res[d.MostSuitableFramework];
                    File.AppendAllText(Properties.Resources.Result,
                        String.Format("File: {0} use {1} matches {2}\n",
                        file, resultframework == 0 ? Properties.Resources.UnknownFramework : d.MostSuitableFramework, resultframework));
                }
                catch { File.AppendAllText(Properties.Resources.Result, String.Format("{0} - it does not css\n", file)); }
            }
            timer.Stop();
            File.AppendAllText(Properties.Resources.Result, String.Format("Tottal time - {0}", timer.ElapsedMilliseconds.ToString()));
        }


        static void Main(string[] args)
        {
            string path, frameworkPath;
            Console.WriteLine("Write path to tests floder");
            path = Console.ReadLine();
            Console.WriteLine("Write path to framework floder");
            frameworkPath = Console.ReadLine();
        }
    }
}
