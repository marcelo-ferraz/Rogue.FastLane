using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Infrastructure.Positioning;
using System.Collections.Generic;

namespace Rogue.FastLane.Queries.Mixins
{
    public static class QueryMovingMixins
    {
        /// <summary>
        /// Moves all the references to values one position to the right, starting from a set of coordinates
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="coordinateSet"></param>
        /// <returns></returns>
        public static Stack<ReferenceNode<TItem, TKey>> MoveAll2TheRight<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, Coordinates[] coordinateSet)
        {
            ReferenceNode<TItem, TKey> previousRef = null;

            return self.ForEachValuedNodeReverse(coordinateSet,
                (@ref, i) =>
                {
                    if (i < 1)
                    { previousRef = @ref; }
                    else {
                        if (previousRef != null)
                        {
                            previousRef.Values[0] = @ref.Values[@ref.Values.Length - 1];
                            previousRef = null;                            
                        }
                        
                        @ref.Values[i] = @ref.Values[i - 1]; 
                    }
                });
        }

        /// <summary>
        /// Moves all the references to values one position to the left, starting from a set of coordinates
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="coordinateSet"></param>
        /// <returns></returns>
        public static Stack<ReferenceNode<TItem, TKey>> MoveAll2TheLeft<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, Coordinates[] coordinateSet)
        {
            ReferenceNode<TItem, TKey> previousRef = null;
            
            return self.ForEachValuedNode(coordinateSet,
                (@ref, i) =>
                {
                    if (@ref.Key.Equals(1089) || (@ref.Key.Equals(1088) && i == 32))
                    {
                        System.Diagnostics.Debugger.Break();
                    }

                    if (i == (@ref.Length - 1) && i > 1)
                    { previousRef = @ref; }
                    else
                    {
                        if (previousRef != null)
                        {
                            previousRef.Values[previousRef.Length - 1] = @ref.Values[0];
                            previousRef = null;
                        }

                        if (i != (@ref.Length - 1))
                        {
                            @ref.Values[i] = @ref.Values[i + 1];
                        }
                    }
                });
        }
    }
}