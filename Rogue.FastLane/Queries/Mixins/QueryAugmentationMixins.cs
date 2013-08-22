using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Items;

namespace Rogue.FastLane.Queries.Mixins
{
	public static class QueryAugmentationMixins
    {
        /// <summary>
        /// Tries to resize the references by one
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <param name="self"></param>
        /// <param name="node">the node that will be increased</param>
        private static void TryResizeReferencesByOne<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node)
        {
            var lvlIndex = self.State.LevelCount - 2;

            while (node != null)
            {
                if (node.Length < self.State.MaxLengthPerNode)
                {
                    for (int i = lvlIndex; i < self.State.LevelCount; i++)
                    {
                        if (i != (self.State.LevelCount - 2))
                        {
                            node.References = node.References != null ?
                                node.References.Resize(node.Length + 1) :
                                new ReferenceNode<TItem, TKey>[1];

                            node.References[node.Length - 1] =
                                new ReferenceNode<TItem, TKey> { Parent = node, Comparer = self.KeyComparer };

                            node = node.References[node.Length - 1];
                        }
                        else
                        {
                            node.Values = node.Values != null ?
                                node.Values.Resize(node.Length + 1) :
                                new ValueNode<TItem>[1];
                            return;
                        }
                    }
                }
                lvlIndex--;
                node = node.Parent;
            }
        }

        /// <summary>
        /// Tries to resize the node value count
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <param name="self"></param>
        /// <param name="node"></param>
        /// <param name="toSum">quantity to sum</param>
        private static void TryResizeValues<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node, int toSum)
        {
            if (node.Values.Length >= self.State.MaxLengthPerNode)
            {
                node = self.LastRefNode(self.Root);
            }

            node.Values = node.Values
                .Resize(node.Values.Length + toSum);
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
        public static void AugmentLevelCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, int itemAmmountToSum)
        {
            var root = self.Root;

            //increases one level
            //if there is only one level bellow the root,
            if (root.Values != null)
            {
                //Increase one level, and send them to the first node of this new level
                root.References = new []
                { 
                    new ReferenceNode<TItem, TKey> 
                    {
                        Values = root.Values,
                        Parent = root,
                        Key = root.Key, 
                        Comparer = self.KeyComparer
                    } 
                };

                root.Values = null;
            }
            else
            {
                //Increase one level, and send them to the first node of this new level
                var refs =
                    root.References;

                root.References = new[]  
                { 
                    new ReferenceNode<TItem, TKey>
                    {
                        References = refs,
                        Key = root.Key,
                        Parent = root, 
                        Comparer = self.KeyComparer
                    } 
                };

                for (int i = 0; i < refs.Length; i++) 
                {
                    refs[i].Parent =
                        root.References[0];
                }

                refs = null;
            }
        }

        /// <summary>
        /// Adds to the value count
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <param name="self"></param>
        /// <param name="sumQtd"></param>
        public static void AugmentValueCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, int sumQtd)
        {
            var lastRefNode =
                self.LastRefNode(self.Root);

            //if needs to change to the next ref node
            if(lastRefNode.Length >= self.State.MaxLengthPerNode)
            { self.TryResizeReferencesByOne(lastRefNode); }
            else
            { TryResizeValues(self, lastRefNode, sumQtd); }
        }
	}
}

