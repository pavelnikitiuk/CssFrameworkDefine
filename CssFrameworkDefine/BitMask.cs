using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CssFrameworkDefine
{
    public class BitMask
    {
        private ulong[] mask;


        public bool this[int index]
        {
            get
            {
                if (index < 0 || index > 374)
                    throw new ArgumentException();
                byte num = (byte)(index / 64);
                byte offset = (byte)(index - num * 64);
                 return (mask[num] & ((ulong)1 << offset )) != 0;
            }
            set
            {
                if (index < 0 || index > 374)
                    throw new ArgumentException();

                byte num = (byte)(index / 64);
                byte offset = (byte)(index - num * 64);

                mask[num] ^= (ulong)1 << offset;
            }

        }

        public override bool Equals(object obj)
        {
            var other = obj as BitMask;

            if(other == null)
                return false;

            for(int i = 0; i < mask.Length; i++)
            {
                if (mask[i] != other.mask[i])
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int code = 0;
            for (int i = 0; i < mask.Length; i++)
            {
                code ^= mask[i].GetHashCode();
            }
            return code;
        }

        public BitMask()
        {
            mask = new ulong[6];
        }
    }


}
