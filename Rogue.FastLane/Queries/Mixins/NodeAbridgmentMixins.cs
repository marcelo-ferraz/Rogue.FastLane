using System;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Queries.States;
using Rogue.FastLane.Items;
using System.Collections.Generic;

namespace Rogue.FastLane.Queries.Mixins
{
    public static class NodeabridgmentMixins
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
        public static void AbridgeLevelCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, int itemAmmountToSum)
        {
            var root = self.Root;

            if (root.Values != null)
            { throw new NotImplementedException(); }

            var refs = new List<ReferenceNode<TItem, TKey>>();

            foreach (var subRef in root.GetSubReferences())
            {
                subRef.Parent = root;
                refs.Add(subRef);
            }

            root.References = refs.ToArray();
        }

        public static void AbridgeValueCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, int itemAmmountToSum)
        {
            var lastRefNode =
                self.GetLastRefNode(self.Root);

            if (lastRefNode.Length < 2)
            { TryEraseNode(lastRefNode.Parent); }
            else
            {
                lastRefNode.Values = lastRefNode
                    .Values.Resize(lastRefNode.Length - 1);
            }
        }

        private static void TryEraseNode<TItem, TKey>(ReferenceNode<TItem, TKey> node)
        {
            if (node == null)
            { throw new NotSupportedException("Collection is too shallow to be dimished."); }

            if (node.Length < 2)
            { TryEraseNode(node.Parent); }
            else
            {
                node.References = node
                    .References.Resize(node.Length - 1);
            }
        }

        private static IEnumerable<ReferenceNode<TItem, TKey>> GetSubReferences<TItem, TKey>(this ReferenceNode<TItem, TKey> node)
        {
            for (int i = 0; i < node.Length; i++)
            {
                var child = 
                    node.References[i];
               
                for (int j = 0; j < child.Length; j++)
                {
                    yield return child.References[j];
                }
            }
        }

        public static bool CanDiminishOneLevel<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self)
        {
            return self.State.Levels[2].TotalUsed <= self.State.Levels[1].TotalOfSpaces;
        }
    }
}

