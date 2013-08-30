using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane._Fastlane2.Infrastructure
{
    public class Calculate
    {
        private static const int MAXSIZE = 80000; // correct max size, before becomes large object 87040;

        public static int MaxSizeOfArray<T>()
        {
            var type = typeof(T);

            var sizeOfT =
                type.IsClass || type.IsInterface || type.IsPointer || type == typeof(object) || type == typeof(string) ?
                IntPtr.Size :
                type == typeof(bool) ? 1 :
                type == typeof(byte) ? 1 :
                type == typeof(short) ? 2 :
                type == typeof(int) ? 4 :
                type == typeof(long) ? 8 :
                type == typeof(float) ? 4 :
                type == typeof(double) ? 8 :
                type == typeof(decimal) ? 16 :
                int.MinValue;

            return (int)Math.Floor((double)(MAXSIZE - 8) / sizeOfT);
        }
    }
}