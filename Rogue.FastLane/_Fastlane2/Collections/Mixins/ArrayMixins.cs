using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using Rogue.FastLane._Fastlane2.Infrastructure;
using System.Reflection;

namespace Rogue.FastLane._Fastlane2.Collections.Mixins
{
    public static class ArrayMixins
    {
        internal static TrySZBinarySearchFunction TrySZBinarySearch { get; private set; }

        internal static TrySZIndexOfFunction TrySZIndexOf { get; private set; }

        internal static TrySZLastIndexOfFunction TrySZLastIndexOf { get; private set; }

        internal static TrySZReverseFunction TrySZReverse { get; private set; }

        internal static TrySZSortFunction TrySZSort { get; private set; }

        static ArrayMixins()
        {
            TrySZBinarySearch = 
                Get<TrySZBinarySearchFunction>("TrySZBinarySearch");
            
            TrySZIndexOf = 
                Get<TrySZIndexOfFunction>("TrySZIndexOf");
            
            TrySZLastIndexOf = 
                Get<TrySZLastIndexOfFunction>("TrySZLastIndexOf");
            
            TrySZReverse = 
                Get<TrySZReverseFunction>("TrySZReverse");
            
            TrySZSort = 
                Get<TrySZSortFunction>("TrySZSort");
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        private static T Get<T>(string functionName) 
        {
            var method = typeof(Array)
                .GetMethod(functionName, BindingFlags.NonPublic | BindingFlags.Static);
            
            return (T) (object) Delegate.CreateDelegate(typeof(T), method);
        }


        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal static Array Resize<T>(this Array array, int newSize)
        {
            Type type = array.GetType();
            Array newArray =
                Array.CreateInstance(type.GetElementType(), newSize);

            Array.Copy(array, 0, newArray, 0, Math.Min(newArray.Length, newSize));

            return newArray;
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static T[] Insert<T>(this T[] array, int index, T val)
        {
            int count = 1;
            Array.Copy(array, index, array, index + count, array.Length - (index + count));
            Array.Clear(array, index, count);
            array[index] = val;
            return array;
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static Array Remove(this Array array, int index)
        {
            int count = 1;
            Array.Copy(array, index + count, array, index, array.Length - (index + count));
            Array.Clear(array, array.Length - count, count);
            return array;
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static T[] G<T>(this object self, int size = 0)
        {
            return (T[])Array.CreateInstance(typeof(T), 0);
        }
    }
}
