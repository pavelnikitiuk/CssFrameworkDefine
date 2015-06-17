using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace CssFrameworkDefine.Model
{
    class OriginalCssFramework
    {
        public string Name { get; set; }

        public List<Dictionary<string, BitArray>> Styles { get; set; }

    }

    class Style
    {
        public List<ExCSS.StyleRule> StyleRule { get; set; }

        public Dictionary<string, BitArray> Selelectors { get; set; }
    }
}


