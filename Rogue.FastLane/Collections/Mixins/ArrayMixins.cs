using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Collections.Mixins
{
    public static class ArrayMixins
    {
        public static T[] Resize<T>(this T[] self, int newSize)
        { 
            var array = self;
            Array.Resize(ref array, newSize);
            return array;
        }
    }
}
