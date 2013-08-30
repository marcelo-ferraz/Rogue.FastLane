using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Infrastructure.Mixins;

namespace Rogue.FastLane._Fastlane2.Infrastructure
{
    public class Calculate
    {
        private const int _maxSize = 80000; // correct max size, before becomes large object 85000 bytes;

        private static int SingleItemSize4<T>() 
        {
            var type = typeof(T);

            return type.IsClass || type.IsInterface || type.IsPointer || type == typeof(object) || type == typeof(string) ?
                IntPtr.Size :
                type == typeof(bool) ? 1 :
                type == typeof(byte) || type == typeof(sbyte) ? 1 :
                type == typeof(short) || type == typeof(ushort) ? 2 :
                type == typeof(int) || type == typeof(uint) ? 4 :
                type == typeof(long) || type == typeof(ulong) ? 8 :
                type == typeof(float) ? 4 :
                type == typeof(Single) ? 8 :
                type == typeof(double) ? 8 :
                type == typeof(decimal) ? 16 :

                type == typeof(DateTime) ? typeof(DateTime).Size() :
                ObjectMixins.SizeOf<T>();
        }

        public static int MaxLengthOfArray<T>()
        {
            var initialHeadSize =
                typeof(T).IsPrimitive ? 4 + 8 :
                8 + 8;

            return (int)Math.Floor(
                (double)(_maxSize - initialHeadSize) / Calculate.SingleItemSize4<T>());
        }
    }
}