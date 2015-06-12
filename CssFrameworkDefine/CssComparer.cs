using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CssFrameworkDefine
{
    internal static class CssComparer
    {
        public static List<string> Compare(IList<ExCSS.StyleRule> originalRule, IList<ExCSS.StyleRule> comparedRule)
        {
            List<string> matches = new List<string>();
            int i = 0; int j = 0;

            while (i < originalRule.Count && j < comparedRule.Count)
            {
                int k = String.Compare(originalRule[i].Value, comparedRule[j].Value);
                if (k == 0)
                {
                    string original = originalRule[i].ToString();
                    string compared = comparedRule[j].ToString();
                    if ((double)LevenshteinDistance(original, compared) / original.Length < 0.7)
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

        public static int LevenshteinDistance(string string1, string string2)
        {
            if (string1 == null)
                throw new ArgumentNullException("string1");
            if (string2 == null)
                throw new ArgumentNullException("string2");
            int diff;
            int[,] m = new int[string1.Length + 1, string2.Length + 1];

            for (int i = 0; i <= string1.Length; i++) m[i, 0] = i;
            for (int j = 0; j <= string2.Length; j++) m[0, j] = j;

            for (int i = 1; i <= string1.Length; i++)
                for (int j = 1; j <= string2.Length; j++)
                {
                    diff = (string1[i - 1] == string2[j - 1]) ? 0 : 1;

                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
                                             m[i, j - 1] + 1),
                                             m[i - 1, j - 1] + diff);
                }

            return m[string1.Length, string2.Length];
        }
    }
}
