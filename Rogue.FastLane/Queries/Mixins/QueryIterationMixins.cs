using System;
using Rogue.FastLane.Collections.Items;
using System.Collections.Generic;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Infrastructure.Positioning;

namespace Rogue.FastLane.Queries.Mixins
{
    public static class QueryIterationMixins
	{
        /// <summary>
        /// Iterates throughtout the lowest reference nodes, the ones closer to the values
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static IEnumerable<ReferenceNode<TItem, TKey>> IntoLowestRefs<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, Coordinates[] coordinates)
        {
            var iterator = 
                new LowRefsEnumerable<TItem, TKey>();

            return iterator.FromHereOn(self.Root, coordinates);
        }

        /// <summary>
        /// Iterates throughtout the lowest reference nodes, the ones closer to the values, in reverse
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="root"></param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static IEnumerable<ReferenceNode<TItem, TKey>> IntoLowestRefsReverse<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> root, Coordinates[] coordinates)
        {
            var iterator =
                new LowRefsReverseEnumerable<TItem, TKey>();

            return iterator.UpToHere(root, coordinates);
        }

        /// <summary>
        /// Loops throughtout all the value nodes, starting from a set of coordinates, in reverse
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="coordinates"></param>
        /// <param name="inEach"></param>
        /// <returns></returns>
        public static Stack<ReferenceNode<TItem, TKey>> ForEachValuedNodeReverse<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self,
            Coordinates[] coordinates, Action<ReferenceNode<TItem, TKey>, int> inEach)
        {
            var queue = 
                new Stack<ReferenceNode<TItem, TKey>>();
            
            var offset =
                coordinates[coordinates.Length - 1];
            
            ReferenceNode<TItem, TKey> @ref = null;
            
            var iterator =
                IntoLowestRefsReverse(self, self.Root, coordinates).GetEnumerator();

            int reverseIndex = offset.OverallLength;

            while (iterator.MoveNext())
            {
                queue.Push(@ref = iterator.Current);
                for (int i = @ref.Values.Length - 1; i > -1 && offset.OverallIndex < reverseIndex; i--)
                {
                    reverseIndex--;
                    inEach(@ref, i);
                }
            }

            return queue;
        }

        /// <summary>
        /// Loops throughtout all the value nodes, starting from a set of coordinates
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="coordinateSet"></param>
        /// <param name="inEach"></param>
        /// <returns></returns>
        public static Stack<ReferenceNode<TItem, TKey>> ForEachValuedNode<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self,
            Coordinates[] coordinateSet, Action<ReferenceNode<TItem, TKey>, int> inEach)
        {
            var queue =
                new Stack<ReferenceNode<TItem, TKey>>();

            var coordinates =
                coordinateSet[coordinateSet.Length - 1];

            ReferenceNode<TItem, TKey> @ref = null;

            var iterator =
                IntoLowestRefs(self, coordinateSet).GetEnumerator();

            int overallIndex = 
                coordinates.OverallIndex;

            while (iterator.MoveNext())
            {
                queue.Push(@ref = iterator.Current);

                for (int i = coordinates.Index; i < @ref.Length && overallIndex < coordinates.OverallLength; i++)
                {
                    overallIndex++;
                    inEach(@ref, i);
                }
            }

            return queue;
        }
	}
}

