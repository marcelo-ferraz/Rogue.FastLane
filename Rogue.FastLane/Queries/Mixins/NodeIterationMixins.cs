using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using System.Collections.Generic;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Items;

namespace Rogue.FastLane.Queries.Mixins
{
    public static class NodeIterationMixins
	{
        public static IEnumerable<ReferenceNode<TItem, TKey>> IntoLowestRefs<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> root, Coordinates[] coordinates)
        {
            var iterator = 
                new LowestReferencesEnumerable<TItem, TKey>();

            return iterator.FromHereOn(root, coordinates);
        }

        public static IEnumerable<ReferenceNode<TItem, TKey>> IntoLowestRefsReverse<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> root, Coordinates[] coordinates)
        {
            var iterator =
                new LowestReferencesReverseEnumerable<TItem, TKey>();

            return iterator.UpToHere(root, coordinates);
        }

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


        public static Stack<ReferenceNode<TItem, TKey>> ForEachValuedNode<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self,
            Coordinates[] coordinates, Action<ReferenceNode<TItem, TKey>, int> inEach)
        {
            var queue =
                new Stack<ReferenceNode<TItem, TKey>>();

            var offset =
                coordinates[coordinates.Length - 1];

            ReferenceNode<TItem, TKey> @ref = null;

            var iterator =
                IntoLowestRefs(self, self.Root, coordinates).GetEnumerator();

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
	}
}

