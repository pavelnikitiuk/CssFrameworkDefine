using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CssFrameworkDefine
{
    public static class CssComparer
    {
        public static void Compare(ExCSS.StyleSheet original, ExCSS.StyleSheet compared)
        {
            int count = 0;
            foreach(var comparedClass in compared.StyleRules)
            {
                foreach(var originalClass in original.StyleRules.Where(x=> String.Compare(x.Value, comparedClass.Value) == 0))
                {
                    if (String.Compare(originalClass.ToString(),comparedClass.ToString()) == 0)
                        count++;
                }
            }
        }
    }
}
