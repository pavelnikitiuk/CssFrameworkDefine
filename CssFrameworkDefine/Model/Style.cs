using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace CssFrameworkDefine.Model
{
    
    class Style
    {
        public string Name { get; set; }
        public BitMask Properties { get; set; }
        public override bool Equals(object obj)
        {
            return (Equals(obj as Style));
        }
        public bool Equals(Style other)
        {
            if (other == null)
                return false;
            if (String.Compare(Name, other.Name) == 0 && Properties.Equals(other.Properties))
                return true;
            return false;
        }
        public override int GetHashCode()
        {

            return  Properties.GetHashCode() ^ Name.GetHashCode();
        }
    }
}


