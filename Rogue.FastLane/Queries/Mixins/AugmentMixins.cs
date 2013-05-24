using System;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Queries.Mixins;

namespace Rogue.FastLane.Queries.Mixins
{
	public static class AugmentMixins
	{
        /// <summary>
        /// Augments the level count.
        /// </summary>
        /// <param name='root'>
        /// The Root of the selector.
        /// </param>
        /// <param name='state'>
        /// the state of the selector tree.
        /// </param>
        /// <param name='itemAmmountToSum'>
        /// Item ammount to sum to the tree.
        /// </param>
        public static void AugmentLevelCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> root, UniqueKeyQueryState state, int itemAmmountToSum)
        {
            int newLength =
                state.Length + itemAmmountToSum;

            var spacesCount =
                Math.Pow(state.OptimumLenghtPerSegment, state.Levels.Length);

            //if there is enough room for this new item, return
            if (spacesCount >= newLength) { return; }

            //increases one level
            //if there is only one level bellow the root,
            if (root.Values != null)
            {
                //Increase one level, and send them to the first node of this new level
                root.References =
                    new ReferenceNode<TItem, TKey>[1];

                root.References[0].Values =
                    root.Values;

                root.Values = null;
            }
            else
            {
                //Increase one level, and send them to the first node of this new level
                var refs =
                    root.References;

                root.References =
                    new ReferenceNode<TItem, TKey>[1];

                root.References[0].References =
                    root.References;

                refs = null;
            }
        }

        public static void AugmentValueCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> root, int itemAmmountToSum)
        {
            var state = self.State;

            double newLength =
                state.Length + itemAmmountToSum;

            double spacesCount =
                Math.Pow(state.OptimumLenghtPerSegment, state.Levels.Length);

            //if there is not enough room for this new item
            if (spacesCount < newLength)
            { AugmentLevelCount(self, root, state, itemAmmountToSum); }

            //get the one who references the value array that can be changed
            int holderIndex = (int)
                Math.Ceiling((double)newLength / spacesCount);
            
            var nodeFound =
                self.GetLastRefNode(root);

            nodeFound.Values =
                nodeFound.Values.Resize(nodeFound.Values.Length + itemAmmountToSum);
        }
	}
}

