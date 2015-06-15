using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CssFrameworkDefine
{
    internal static class CssComparer
    {
        public static bool CompareStringArray(IOrderedEnumerable<ExCSS.Property> originalArray, IOrderedEnumerable<ExCSS.Property> comparedArray)
        {
            if (originalArray.Count() != comparedArray.Count())
                return false;
            var original = originalArray.GetEnumerator();
            original.MoveNext();
            foreach(var compared in comparedArray)
            {
                original.Current.Important = compared.Important;
                if (String.Compare(original.Current.ToString(), compared.ToString()) != 0)
                    return false;
                original.MoveNext();
            }
            return true;
            
        }
        public static List<string> Compare(IList<ExCSS.StyleRule> originalRule, IList<ExCSS.StyleRule> comparedRule)
        {
            Stopwatch t = new Stopwatch();
            t.Start();
            List<string> matches = new List<string>();
            int i = 0; int j = 0;

            while (i < originalRule.Count && j < comparedRule.Count)
            {
                int k = String.Compare(originalRule[i].Value, comparedRule[j].Value);
                if (k == 0)
                {
                    var original = originalRule[i].Declarations.Properties.OrderBy(x => x.Name);
                    var compared = comparedRule[j].Declarations.Properties.OrderBy(x => x.Name);
                    if(CompareStringArray(original,compared))
                        matches.Add(originalRule[i].Value);
                    i++; j++;
                }
                else
                    if (k > 0)
                        j++;
                    else
                        i++;
            }
            t.Stop();

            return matches;
        }
    }
}
