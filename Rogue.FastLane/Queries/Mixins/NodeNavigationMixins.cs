using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using System.Collections.Generic;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Items;

namespace Rogue.FastLane.Queries.Mixins
{
    public static class NodeNavigationMixins
    {
        #region to be erased
        
        /// <summary>
        /// Retrieves the referenceNode next to a valuenode, and give it back a set of coordinates, to be used as offsets, when iterating
        /// </summary>
        /// <typeparam name="TItem">The type of the Item</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <param name="compareKeys"></param>
        /// <param name="offsets"></param>
        /// <param name="lvlCount"></param>
        /// <param name="lvlIndex"></param>
        /// <returns></returns>
        [Obsolete]
        public static ReferenceNode<TItem, TKey> FirstRefByUniqueKey<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ref OneTimeValue[] offsets, ReferenceNode<TItem, TKey> node = null)
        {
            var offs = (offsets =
                new OneTimeValue[self.State.LevelCount]);

            var refNode = GetRefByUniqueKey(
                self,
                (lvlIndex, index, n) =>
                    offs[lvlIndex] = new OneTimeValue() { Value = index < 0 ? ~index : index }, node);

            offsets = offs;

            return refNode;
        }

        #endregion

        public static ReferenceNode<TItem, TKey> GetRefByUniqueKey<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, Action<int, int, ReferenceNode<TItem, TKey>> getCoordinates, ReferenceNode<TItem, TKey> node = null, int lvlIndex = 0)
        {
            node = node ?? self.Root;

            int index = node.Values != null ?
                node.Values.BinarySearch(n => self.CompareKeys(self.Key, self.SelectKey(n.Value))) :
                node.References.BinarySearch(n => self.CompareKeys(self.Key, n.Key));

            getCoordinates(++lvlIndex, index, node);

            if (node.Values != null) { return node; }

            var found =
                node.References[index < 0 ? ~index : index];

            return found != null ?
                GetRefByUniqueKey(self, getCoordinates, found, lvlIndex) :
                null;
        }

        /// <summary>
        /// Retrieves the referenceNode next to a valuenode, and give it back a set of coordinates, to be used as offsets, when iterating
        /// </summary>
        /// <typeparam name="TItem">The type of the Item</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <param name="compareKeys"></param>
        /// <param name="offsets"></param>
        /// <param name="lvlCount"></param>
        /// <param name="lvlIndex"></param>
        /// <returns></returns>
        public static ReferenceNode<TItem, TKey> FirstRefByUniqueKey<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ref Coordinates[] absoluteCoordinates, ReferenceNode<TItem, TKey> node = null)
        {
            var coordinates =
                new Coordinates[self.State.LevelCount];

            coordinates[0] =
                new Coordinates();

            int lastIndex = 0;

            var refNode = GetRefByUniqueKey(
                self,
                (lvlIndex, index, parentNode) =>
                {
                    if (coordinates.Length <= lvlIndex) { return; }

                    var overallLength = ((INode[])
                        parentNode.References ?? parentNode.Values).Length + (lastIndex * self.State.MaxLengthPerNode);
                    
                    lastIndex =
                        (lastIndex * self.State.MaxLengthPerNode) + (index < 0 ? ~index : index);

                    coordinates[lvlIndex] = 
                        new Coordinates()
                        {
                            Length = overallLength,
                            Index = index < 0 ? ~index : index,
                            OverallIndex = lastIndex
                        };
                },
                node);

            absoluteCoordinates = coordinates;

            return refNode;
        }

        public static int GetValueIndexByUniqueKey<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ref Coordinates[] absoluteCoordinates, ref ReferenceNode<TItem, TKey> closestRef)
        {
            absoluteCoordinates = 
                new Coordinates[self.State.LevelCount];

            closestRef = FirstRefByUniqueKey(self, ref absoluteCoordinates);

            if (closestRef == null) { return -1; }

            return closestRef.Values
                .BinarySearch(n =>
                    self.CompareKeys(self.Key, self.SelectKey(n.Value)));
        }

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

            var found =
                node.References[index < 0 ? ~index : index];

            return found != null ? FirstRefByUniqueKey(self, found) : null;
        }

        public static ReferenceNode<TItem, TKey> GetLastRefNode<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node = null)
        {
            return (node ?? (node = self.Root)).Values == null ?
                node.References == null ? node :
                GetLastRefNode(self, node.References[node.References.Length - 1]) :
                node;
        }
    }
}

