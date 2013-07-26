using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Infrastructure.Positioning;
using Rogue.FastLane.Queries.States.Mixins;

namespace Rogue.FastLane.Queries.Mixins
{
    public static class QuerySearchMixins
    {
        /// <summary>
        /// Creates the set 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="node"></param>
        /// <param name="coordinateSet"></param>
        /// <param name="lvlIndex"></param>
        /// <param name="lastOverallIndex"></param>
        /// <param name="found"></param>
        /// <param name="rawIndex"></param>
        /// <returns></returns>
        internal static Coordinates[] GetCoordinates<TItem, TKey>(UniqueKeyQuery<TItem, TKey> self, ref ReferenceNode<TItem, TKey> node, Coordinates[] coordinateSet, int lvlIndex, int lastOverallIndex, INode found, int rawIndex)
        {
            var index = rawIndex < 0 ? ~rawIndex : rawIndex;

            var coord = 
                coordinateSet[lvlIndex];
            
            coord.Length = node.Length;
            coord.Index = index;
            coord.OverallLength = self.State.Levels[lvlIndex].TotalUsed;
            coord.OverallIndex = (lastOverallIndex * self.State.MaxLengthPerNode) + index;

            lastOverallIndex = coord.OverallIndex;

            if (found is ReferenceNode<TItem, TKey>)
            {
                node = (ReferenceNode<TItem, TKey>)found;

                return self.Locate(
                    ref node, coordinateSet, lvlIndex + 1, lastOverallIndex);
            }

            return coordinateSet;
        }
        
        /// <summary>
        /// Searches whithn the node hierarchy, using the binary search method
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        internal static int BinarySearch<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node)
        {
            if (node.Values != null)
            {
                return node.Values.BinarySearch(n =>
                    n != null ?
                        self.CompareKeys(self.Key, self.SelectKey(n.Value)) : 0);
            }
            return node.References.BinarySearch(
                n =>
                    n != null ? self.CompareKeys(self.Key, n.Key) : 0);
        }

        /// <summary>
        /// Locates and maps the node by its unique key
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="node"></param>
        /// <param name="coordinateSet"></param>
        /// <param name="lvlIndex"></param>
        /// <param name="lastOverallIndex"></param>
        /// <returns></returns>
        internal static Coordinates[] Locate<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ref ReferenceNode<TItem, TKey> node, Coordinates[] coordinateSet, int lvlIndex = 1, int lastOverallIndex = 0)
        {
            node = node ?? self.Root;

            var keyComparison =
                self.CompareKeys(node.Key, self.Key);

            //the key in the node is lower than the new key
            if (keyComparison < 0 && node.References != null)
            {
                /* set the rest of the coordinates to the right
                 * if the node has the maximum length, go to the parent and move one to the right
                 * all in the first position
                 * return coordinates set
                 */

                var found = node[node.Length - 1];

                var index = node.Length - 1;

                return GetCoordinates<TItem, TKey>(
                    self, ref node, coordinateSet, lvlIndex, lastOverallIndex, found, index);
            }
            //the key in the node is higher than the new key
            else //if (rootComparison > 0) 
            {
                /*
                 * if it is a reference one, search inside the child node, 
                 * else search inside the values and return the coordinates set
                 */
                var rawIndex =
                    self.BinarySearch(node);

                var found = node[rawIndex < 0 ? ~rawIndex : rawIndex];

                return GetCoordinates<TItem, TKey>(
                    self, ref node, coordinateSet, lvlIndex, lastOverallIndex, found, rawIndex);
            }
        }

        /// <summary>
        /// Locates and maps the node by its unique key
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="node"></param>
        /// <param name="ephemeral"></param>
        /// <returns></returns>
        public static Coordinates[] Locate<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ref ReferenceNode<TItem, TKey> node, bool ephemeral = false)
        {
            var coordinates =
                self.State.AcquireMap(ephemeral);

            return self.Locate(ref node, coordinates);
        }

        /// <summary>
        /// Retrieves the first reference node by its unique key
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
        
        /// <summary>
        /// Retrieves the last reference node from the root. The last in depth and in index
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ReferenceNode<TItem, TKey> LastRefNode<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node = null)
        {
            if (node == null) { node = self.Root; }

            return node.Values == null ?
                node.References == null ? node :
                LastRefNode(self, node.References[node.References.Length - 1]) :
                node;
        }
    }
}

