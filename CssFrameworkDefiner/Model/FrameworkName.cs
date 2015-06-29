using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CssFrameworkDefine.Model
{
    class FrameworkName
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public override bool Equals(object obj)
        {
            var other = obj as FrameworkName;
            if (other == null)
                return false;
            return (String.Compare(Name, other.Name) == 0 && other.Number == Number);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Number.GetHashCode();
        }
    }
}
