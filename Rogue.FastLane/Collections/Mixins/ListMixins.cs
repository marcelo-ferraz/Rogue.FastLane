using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Collections.Mixins
{
    public static class ListMixins
        {
            private static Func<T, int> GetComparison<T>(T item, Func<T, int> comparison)
            {
                if (comparison != null) { return comparison; }

                if (item.Equals(default(T))) { throw new ArgumentException("Item and comparison can not be null."); }

                if (typeof(IComparable<T>).IsAssignableFrom(item.GetType()))
                {
                    return i =>
                        ((IComparable<T>)item).CompareTo(i);
                }

                if (typeof(IComparable).IsAssignableFrom(item.GetType()))
                {
                    return i =>
                        ((IComparable)item).CompareTo(i);
                }

                return i => item.GetHashCode().CompareTo(i.GetHashCode());
            }

            public static int BinarySearch<T>(this IList<T> self, Func<T, int> comparison)
            {
                int low = 0;
                int high = self.Count - 1;
                int midpoint = 0;

                if (self.Count == 1) { return comparison(self[0]) == 0 ? 0 : -1; }

                while (low <= high)
                {
                    midpoint = low + (high - low) / 2;

                    //var item = 
                    //    self[midpoint];

                    //if (object.Equals(item, default(T))) { continue; }

                    int comparisonResult =
                        comparison(self[midpoint]);

                    // check to see if value is equal to item in array
                    if (comparisonResult == 0)
                    {
                        return midpoint;
                    }

                    if (comparisonResult < 0)
                    { high = midpoint - 1; }
                    else
                    { low = midpoint + 1; }
                }

                return -(low + 1);
            }

            public static int BinarySearch<T>(this IList<T> self, T item)
                where T : IComparable
            {
                return self.BinarySearch<T>(i => item.CompareTo(i));
            }

            public static int BinarySearch<T>(this IList<T> self, Func<T, int> comparison = null, T item = default(T))
            {
                return self.BinarySearch<T>(GetComparison<T>(item, comparison));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="self"></param>
            /// <param name="comparison"></param>
            /// <param name="item"></param>
            /// <returns></returns>
            public static T BinaryGet<T>(this IList<T> self, Func<T, int> comparison = null, T item = default(T))
            {
                return self.BinaryGet(GetComparison(item, comparison));
            }

            /// <summary>
            /// Busca binaria que retorna o item pedido, ou seu valor default no lugar.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="self"></param>
            /// <param name="comparison"></param>
            /// <returns></returns>
            public static T BinaryGet<T>(this IList<T> self, Func<T, int> comparison)
            {
                int index =
                    BinarySearch(self, comparison);

                return index < 0 ? default(T) : self[index];
            }
        }
    }

