using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;

namespace Rogue.FastLane.Queries.Mixins
{
    public static class NodeNavigationMixins
    {
        public static int GetValueIndexByUniqueKey<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ref ReferenceNode<TItem, TKey> closestRef)
        {
            closestRef = 
                FirstRefByUniqueKey(self);

            if (closestRef == null) { return -1; }

            return closestRef.Values
                .BinarySearch(n =>
                    self.CompareKeys(self.Key, self.SelectKey(n.Value)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <param name="compareKeys"></param>
        /// <returns></returns>
        public static ReferenceNode<TItem, TKey> FirstRefByUniqueKey<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node = null)
        {
            if ((node ?? (node = self.Root)).Values != null) { return node; }

            int index = node
                .References.BinarySearch(k => self.CompareKeys(self.Key, k.Key));

            index = 
                index < 0 ? ~index : index;

            return index < node.References.Length ? 
                FirstRefByUniqueKey(self, node.References[index]) : 
                null;
        }

        public static ReferenceNode<TItem, TKey> GetLastRefNode<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node = null)
        {
            if (node == null) { node = self.Root; }

            return node.Values == null ?
                node.References == null ? node :
                GetLastRefNode(self, node.References[node.References.Length - 1]) :
                node;
        }

        public static ReferenceNode<TItem, TKey> Root<TItem, TKey>(this ReferenceNode<TItem, TKey> node)
        {
            return node.Parent != null ? node.Parent.Root() : node;
        }
    }
}

