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
        private static int GetOverallIndex<TItem, TKey>(ReferenceNode<TItem, TKey> nodeRef, int coordPos, Coordinates coord)
        {
            if (nodeRef.Parent == null) { return coord.Index; }

            var restCount = 
                //is root
                nodeRef.Parent == null ? 0 :
                
                nodeRef.Values.Length > 0 ?
                //has values
                nodeRef.Parent.Values.Length - 1 * coordPos :
                //has no values
                0;

            return coord.Index + restCount;
        }

        public static ReferenceNode<TItem, TKey> GetRefByUniqueKey<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, Action<int, int, ReferenceNode<TItem, TKey>> getCoordinates, ReferenceNode<TItem, TKey> node = null, int lvlIndex = 0)
        {
            node = node ?? self.Root;

            int index = node.Values != null ?
                node.Values.BinarySearch(n => self.CompareKeys(self.Key, self.SelectKey(n.Value))) :
                node.References.BinarySearch(n => self.CompareKeys(self.Key, n.Key));

            getCoordinates(lvlIndex, index, node);

            if (node.Values != null) { return node; }

            var found =
                node.References[index < 0 ? ~index : index];

            return found != null ?
                GetRefByUniqueKey(self, getCoordinates, node, ++lvlIndex) :
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

            int lastIndex = 0;

            var refNode = GetRefByUniqueKey(
                self,
                (lvlIndex, index, n) =>
                {
                    if (coordinates.Length <= lvlIndex) { return; }
                    var coord = new Coordinates()
                    {
                        Length = n.Parent != null ? n.Parent.References.Length : 0,
                        Index = index < 0 ? ~index : index,
                    };

                    coord.OverallIndex =
                        coord.Index + (lvlIndex * coord.Length);

                    coordinates[lvlIndex] = coord;

                    lastIndex = index;
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

            closestRef = FirstRefByUniqueKey(
                self, ref absoluteCoordinates);

            if (closestRef == null) { return -1; }

            var index = closestRef.Values.BinarySearch(
                val =>
                    self.CompareKeys(self.Key, self.SelectKey(val.Value)));
            
            var coordPos = 
                absoluteCoordinates.Length - 1;

            var coord = 
                new Coordinates() 
                {
                    Index = index < 0 ? ~index : index,
                    Length = closestRef.Values.Length,        
                };

            coord.OverallIndex =  
                GetOverallIndex<TItem, TKey>(closestRef, coordPos, coord);
            
            absoluteCoordinates[coordPos] = coord;
            
            return index;
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
                GetLastRefNode(self, node.References[node.References.Length - 1]) :
                node;
        }
    }
}

