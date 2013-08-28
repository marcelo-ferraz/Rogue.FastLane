using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Rogue.FastLane._Fastlane2.Collections.Mixins
{
    public delegate bool TrySZBinarySearchFunction(Array sourceArray, int sourceIndex, int count, Object value, out int retVal);
    public delegate bool TrySZIndexOfFunction(Array sourceArray, int sourceIndex, int count, Object value, out int retVal);
    public delegate bool TrySZLastIndexOfFunction(Array sourceArray, int sourceIndex, int count, Object value, out int retVal);
    public delegate bool TrySZReverseFunction(Array array, int index, int count);
    public delegate bool TrySZSortFunction(Array keys, Array items, int left, int right);

    public static class SZMixins
    {
        static SZMixins()
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

        private static T Get<T>(string functionName) 
        {
            var method = typeof(Array)
                .GetMethod(functionName, BindingFlags.NonPublic | BindingFlags.Static);
            
            return (T) (object) Delegate.CreateDelegate(typeof(T), method);
        }

        public static TrySZBinarySearchFunction TrySZBinarySearch { get; private set; }
        
        public static TrySZIndexOfFunction TrySZIndexOf  { get; private set; }
        
        public static TrySZLastIndexOfFunction TrySZLastIndexOf { get; private set; }
        
        public static TrySZReverseFunction TrySZReverse { get; private set; }
        
        public static TrySZSortFunction TrySZSort { get; private set; }
    }
}
