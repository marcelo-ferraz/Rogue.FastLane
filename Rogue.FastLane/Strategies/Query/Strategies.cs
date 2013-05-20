using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Collections.Mixins;

namespace Rogue.FastLane.Strategies.Query
{
	public static class QueryStrategies
	{
        private static NodeFetchStrategy _fetchStrategy;
        private static AugmentStrategy _augmentStrategy;
        static QueryStrategies()
        {
            _augmentStrategy = new AugmentStrategy(
                _fetchStrategy = new NodeFetchStrategy());
        }

		
		/// <summary>
		/// Augments the level count.
		/// </summary>
		/// <param name='root'>
        /// The Root of the query.
		/// </param>
		/// <param name='state'>
		/// the state of the selector tree.
		/// </param>
		/// <param name='itemAmmountToSum'>
		/// Item ammount to sum to the tree.
		/// </param>
        public static void AugmentLevelCount<TItem, TKey>(ReferenceNode<TItem, TKey> root, UniqueKeyQueryState state, int itemAmmountToSum)
        {
            _augmentStrategy.AugmentLevelCount(root, state, itemAmmountToSum);
        }

        /// <summary>
        /// Augments the value count on the tree structure.
        /// </summary>
        /// <param name='root'>
        /// The Root of the query.
        /// </param>
        /// <param name='state'>
        /// the state of the selector tree.
        /// </param>
        /// <param name='itemAmmountToSum'>
        /// Item ammount to sum to the tree.
        /// </param>
        public static void AugmentValueCount<TItem, TKey>(ReferenceNode<TItem, TKey> root, UniqueKeyQueryState state, int itemAmmountToSum)
		{
            _augmentStrategy.AugmentValueCount(root, state, itemAmmountToSum);
		}
	}
}

