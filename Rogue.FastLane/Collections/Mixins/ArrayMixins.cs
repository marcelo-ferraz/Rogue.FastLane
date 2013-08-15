using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace Rogue.FastLane.Collections.Mixins
{
    public static class ArrayMixins
    {
        [TargetedPatchingOptOut("")]
        //, MethodImpl(MethodImplOptions.Unmanaged)]
        public static T[] Resize<T>(this T[] self, int newSize)
        {             
            var array = self;
            Array.Resize(ref array, newSize);
            return array;
        }
    }
}
