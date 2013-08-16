using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;

namespace Rogue.FastLane.Collections.Mixins
{
    public static class ListMixins
        {
            [TargetedPatchingOptOut("")]
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

            [TargetedPatchingOptOut("")]
            public static int BinarySearch<T>(this IList<T> self, Func<T, int> compare)
            {
                int i = 0;
                int num = i + self.Count - 1;

                while (i <= num)
                {
                    int num2 = i + (num - i >> 1);
                    int num3 = compare(self[num2]);
                    if (num3 == 0)
                    {
                        return num2;
                    }
                    if (num3 < 0)
                    {
                        i = num2 + 1;
                    }
                    else
                    {
                        num = num2 - 1;
                    }
                }
                return ~i;
            }

            [TargetedPatchingOptOut("")]
            public static int BinarySearch<T>(this IList<T> self, T item)
                where T : IComparable
            {
                return self.BinarySearch<T>(i => item.CompareTo(i));
            }

            [TargetedPatchingOptOut("")]
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
            [TargetedPatchingOptOut("")]
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
            [TargetedPatchingOptOut("")]
            public static T BinaryGet<T>(this IList<T> self, Func<T, int> comparison)
            {
                int index =
                    BinarySearch(self, comparison);

                return index < 0 ? default(T) : self[index];
            }
        }
    }

