using System;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Queries.Mixins;

namespace Rogue.FastLane.Queries.Mixins
{
	public static class AugmentationMixins
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
        public static void AugmentLevelCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, int itemAmmountToSum)
        {
            if (!self.Needs2AugmentLevelCount(itemAmmountToSum)) { return; }

            var root = self.Root;

            //increases one level
            //if there is only one level bellow the root,
            if (root.Values != null)
            {
                //Increase one level, and send them to the first node of this new level
                root.References = new []
                { new ReferenceNode<TItem, TKey>() };

                root.References[0].Values =
                    root.Values;
                root.References[0].Parent = root;

                root.Values = null;
            }
            else
            {
                //Increase one level, and send them to the first node of this new level
                var refs =
                    root.References;

                root.References = new[]  
                { new ReferenceNode<TItem, TKey>() };

                root.References[0].References = refs;
                root.References[0].Parent = root;

                for (int i = 0; i < refs.Length; i++) 
                {
                    refs[i].Parent =
                        root.References[0];
                }

                refs = null;
            }
        }

        public static void AugmentValueCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, int itemAmmountToSum)
        {
            //if there is not enough room for this new item
            if (self.Needs2AugmentLevelCount(itemAmmountToSum))
            { 
                self.AugmentLevelCount(itemAmmountToSum); 
                
            }

            var nodeFound =
                self.GetLastRefNode(self.Root);

            //if needs to change to the next ref node
            TryResizeReferences(nodeFound.Parent, self.State, 1);

            TryResizeValues(self, nodeFound, itemAmmountToSum);
        }

        private static void TryResizeReferences<TItem, TKey>(ReferenceNode<TItem, TKey> node, UniqueKeyQueryState state, int toSum)
        {
            if (node == null) { return; }
            if (node.References == null) { return; }

            TryResizeReferences(node.Parent, state, toSum);

            if (node.References.Length > state.MaxLengthPerNode)
            {
                node.References.Resize(
                    node.References.Length + 1);

                node = node.References[
                    node.References.Length - 1];
            }
        }

        private static void TryResizeValues<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node, int toSum)
        {
            if (node.Values.Length > self.State.MaxLengthPerNode)
            {
                node = self.GetLastRefNode(self.Root);
            }

            node.Values = node.Values
                .Resize(node.Values.Length + toSum);
        }
        
        public static bool Needs2AugmentLevelCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, int itemAmmountToSum)
        {
            double totalOfSpacesCount =
                Math.Pow(self.State.MaxLengthPerNode, self.State.Levels.Length - 1);

            //if there is not enough room for this new item
            return totalOfSpacesCount < self.State.Length;
        }
	}
}

