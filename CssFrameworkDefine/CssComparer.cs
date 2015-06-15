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
        private static bool CompareProperties(IEnumerable<ExCSS.Property> originalArray, IEnumerable<ExCSS.Property> comparedArray)
        {
            if (originalArray.Count() != comparedArray.Count())
                return false;
            var original = originalArray.GetEnumerator();
            foreach(var compared in comparedArray)
            {
                original.MoveNext();
                original.Current.Important = compared.Important;
                if (String.Compare(original.Current.ToString(), compared.ToString()) != 0)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Compare two ExCSS.StylesRule and if are equal return true
        /// </summary>
        /// <param name="originalRule">First ExCSS.StyleRule</param>
        /// <param name="comparedRule">Second ExCSS.StyleRule</param>
        /// <returns>Are ExCSS.StyleRule equals</returns>
        public static List<string> Compare(IList<ExCSS.StyleRule> originalRule, IList<ExCSS.StyleRule> comparedRule)
        {
            List<string> matches = new List<string>();
            int i = 0; int j = 0;
            while (i < originalRule.Count && j < comparedRule.Count)
            {
                int k = String.Compare(originalRule[i].Value, comparedRule[j].Value);
                if (k == 0)
                {

                    if(CompareProperties(originalRule[i].Declarations.Properties,comparedRule[j].Declarations.Properties))
                        matches.Add(originalRule[i].Value);
                    i++; j++;
                }
                else
                    if (k > 0)
                        j++;
                    else
                        i++;
            }
            return matches;
        }
    }
}
