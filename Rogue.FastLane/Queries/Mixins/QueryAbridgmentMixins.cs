using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;
using System;
using System.Collections.Generic;

namespace Rogue.FastLane.Queries.Mixins
{
    public static class QueryAbridgmentMixins
    {
        /// <summary>
        /// Tries to erase the node by resizing it
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <param name="node">The node that will have its value stripped</param>
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

        /// <summary>
        /// Gets all the grandchildren from a node
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <param name="node"></param>
        /// <returns>All the grandchildren from a given node</returns>
        private static IEnumerable<ReferenceNode<TItem, TKey>> GetGrandChildren<TItem, TKey>(this ReferenceNode<TItem, TKey> node)
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

            foreach (var subRef in root.GetGrandChildren())
            {
                subRef.Parent = root;
                refs.Add(subRef);
            }

            root.References = refs.ToArray();
        }

        /// <summary>
        /// Diminishes the value count from a unique key query
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <param name="self">The query</param>
        /// <param name="qtdToSubtract">Quantity to substract</param>
        public static void AbridgeValueCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, int qtdToSubtract)
        {
            var lastRefNode =
                self.LastRefNode(self.Root);

            if (lastRefNode.Length < 2)
            { TryEraseNode(lastRefNode.Parent); }
            else
            {
                lastRefNode.Values = lastRefNode
                    .Values.Resize(lastRefNode.Length - 1);
            }
        }
    }
}

