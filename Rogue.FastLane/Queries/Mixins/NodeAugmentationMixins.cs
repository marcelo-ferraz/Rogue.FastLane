using System;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Queries.States;
using Rogue.FastLane.Items;

namespace Rogue.FastLane.Queries.Mixins
{
	public static class NodeAugmentationMixins
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
                        Key = root.Key
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

        public static void AugmentValueCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, int itemAmmountToSum)
        {
            var lastRefNode =
                self.GetLastRefNode(self.Root);

            //if needs to change to the next ref node
            if(lastRefNode.Length >= self.State.MaxLengthPerNode)
            { self.TryResizeReferencesByOne(lastRefNode); }
            else
            { TryResizeValues(self, lastRefNode, itemAmmountToSum); }
        }

        private static void TryResizeReferencesByOne<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node)
        {
            #region
            //if (node.Parent != null && lvlIndex == 1)
            //{
            //    node = node.Root();
            //}

            //if (lvlIndex + 1 == self.State.LevelCount)
            //{
            //    node.Values = new ValueNode<TItem>[1];
            //    return;
            //}

            //int lastIndex = 0;

            //if (node.References != null)
            //{
            //    if (node.References.Length > self.State.MaxLengthPerNode)
            //    { throw new NotSupportedException("This query has more items than can it bear."); }

            //    node.References = node.References.Resize(
            //        node.References.Length + 1);

            //    lastIndex =
            //        node.References.Length - 1;
            //}
            //else
            //{ node.References = new ReferenceNode<TItem, TKey>[1]; }

            //node.References[lastIndex] =
            //    new ReferenceNode<TItem, TKey>()
            //    {
            //        Parent = node
            //    };

            //self.TryResizeReferencesByOne(node.References[lastIndex], lvlIndex + 1);
            #endregion

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
                                new ReferenceNode<TItem,TKey>[1];
                            
                            node.References[node.Length - 1] =
                                new ReferenceNode<TItem, TKey> { Parent = node };
                            
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

        private static void TryResizeValues<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node, int toSum)
        {
            if (node.Values.Length >= self.State.MaxLengthPerNode)
            {
                node = self.GetLastRefNode(self.Root);
            }

            node.Values = node.Values
                .Resize(node.Values.Length + toSum);
        }
        
        public static bool Needs2AugmentLevelCount<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, int itemAmmountToSum)
        {            
            //if there is not enough room for this new item
            return self.State.Last.TotalOfSpaces < self.State.Last.TotalUsed;
        }
	}
}

