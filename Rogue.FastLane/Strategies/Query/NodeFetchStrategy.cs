using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using System.Collections.Generic;
using Rogue.FastLane.Collections;

namespace Rogue.FastLane.Strategies.Query
{
	public class NodeFetchStrategy
	{       

        /// <summary>
        /// Gets the reference node that holds the value. This value is got by the following algorithm:  valueFlatIndex/(state.Length/state.OptimumLenghtPerSegment ^ levelIndex)
        /// </summary>
        /// <returns>
        /// The reference node.
        /// </returns>
        /// <param name='node'>
        /// The Node that holds the persued one.
        /// </param>
        /// <param name='valueFlatIndex'>
        /// The flat index of the Value.
        /// <remarks>The index keeps in mind that the final list is all just the same list</remarks>
        /// </param>
        /// <param name='state'>
        /// the selectors State.
        /// </param>
        /// <param name='levelIndex'>
        /// the current Level index.
        /// </param>
        public ReferenceNode<TItem, TKey> GetLastRefNodeByItsValueFlatIndex<TItem, TKey>(ReferenceNode<TItem, TKey> node, int valueFlatIndex, UniqueKeyQueryState state, int levelIndex)
        {
            if (node.Values != null) { return node; }

            int thisLevelsIndex = (int)Math.Floor(
                (double)(valueFlatIndex / (state.Length / Math.Pow(state.OptimumLenghtPerSegment, levelIndex))));

            return GetLastRefNodeByItsValueFlatIndex(
                node.References[thisLevelsIndex],
                valueFlatIndex,
                state,
                ++levelIndex);
        }

        public IEnumerable<ReferenceNode<TItem, TKey>> IterateTroughLowestReferences<TItem, TKey>(ReferenceNode<TItem, TKey> root, UniqueKeyQueryState state, int levelIndex)
        {
            return new LowestReferencesEnumerable<TItem, TKey>(root);
        }
	}
}

