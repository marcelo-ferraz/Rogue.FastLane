using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace Rogue.FastLane.Collections.Mixins
{
    public unsafe static class ByteArrayMixins
    {
        [TargetedPatchingOptOut("")]
        public static int CompareTo(this byte[] self, byte[] other)
        {
            //if (self.Length < other.Length) { return -1; }

            //if (self.Length  > other.Length) { return +1; }

            int comparison = 0;

            for (int i = 0; i < self.Length && i < other.Length; i++)
            {
                if ((comparison = self[i].CompareTo(other[i])) != 0)
                { return comparison; }
            }
            return comparison;
        }

        [TargetedPatchingOptOut("")]
        public unsafe static int UnsafeCompareTo(this byte[] self, byte[] other)
        {
            //if (self.Length < other.Length) { return -1; }

            //if (self.Length > other.Length) { return +1; }

            fixed (byte* selfPtr = self, otherPtr = other)
            {
                int n = self.Length;

                byte* ptr1 = selfPtr;
                byte* ptr2 = otherPtr;

                while (n-- > 0)
                {
                    int comparison;

                    if ((comparison = (*ptr1++).CompareTo(*ptr2++)) != 0)
                    {
                        return comparison;
                    }
                }
            }
            return 0;
        }
    }
}
