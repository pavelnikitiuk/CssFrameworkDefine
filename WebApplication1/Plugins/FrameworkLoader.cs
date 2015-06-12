using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using CssFrameworkDefine;

namespace WebApplication1.Plugins
{
    public class FrameworkLoader
    {
        public static List<CssFrameworkIdentity> Load(string path)
        {
            List<CssFrameworkIdentity> frameworks = new List<CssFrameworkIdentity>();
            foreach (var file in Directory.GetFileSystemEntries(path))
                frameworks.Add(new CssFrameworkIdentity(file.Split('\\').Last(),Directory.GetFiles(file).ToList()));
            return frameworks;
        }


    }
}