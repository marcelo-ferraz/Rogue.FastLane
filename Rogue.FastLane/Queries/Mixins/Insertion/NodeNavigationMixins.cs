using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using System.Collections.Generic;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Items;

namespace Rogue.FastLane.Queries.Mixins.Insertion
{
    public static class NodeNavigationMixins
    {
        private static int GetIndexWithinBoundaries<TItem, TKey>(ReferenceNode<TItem, TKey> node, int index)
        {
            index = index < 0 ? ~index : index;

            if (node == null) { return index; }

            var length =
                ((INode[])node.References ?? node.Values).Length;

            return index < length ? index : length - 1; ;
        }
        
        private static bool Try2Set2RightNode<TItem, TKey>(UniqueKeyQuery<TItem, TKey> self, Coordinates[] coordinates, int lvlIndex)
        {
            //First level passed, finish
            if (lvlIndex < 0)
            { return false; }

            //There's no need for passing to the node in the right 
            if (coordinates[lvlIndex].Index < self.State.MaxLengthPerNode)
            { return false; }

            var index = coordinates[lvlIndex].Index;

            //set the index within the boundaries
            index = index >= self.State.MaxLengthPerNode ?
               self.State.MaxLengthPerNode % index:
               index;

            coordinates[lvlIndex].Index = index;

            coordinates[lvlIndex].OverallIndex =
                ((coordinates[lvlIndex - 1].OverallIndex + 1) * self.State.MaxLengthPerNode) + coordinates[lvlIndex].Index;

            coordinates[lvlIndex - 1].OverallIndex++;
            coordinates[lvlIndex - 1].Index++;

            Try2Set2RightNode(self, coordinates, lvlIndex - 1);

            return true;
        }

        private static void SetOverallIndexes<TItem, TKey>(UniqueKeyQuery<TItem, TKey> self, Coordinates[] coordinates)
        {
            int lastOverallIndex = 0;

            for (int i = 0; i < coordinates.Length; i++)
            {
                //putting the index inseide the boundaries of the array it is within
                var index = 
                    coordinates[i].Index;
                var toAdd = 0;
                if(index >= self.State.MaxLengthPerNode)
                {
                    index = self.State.MaxLengthPerNode % index;
                    toAdd = 1;
                }

                coordinates[i].OverallIndex =
                    ((lastOverallIndex + toAdd) * self.State.MaxLengthPerNode) + index;

                lastOverallIndex = 
                    coordinates[i].OverallIndex;
            }

            for (int i = coordinates.Length -1; i > 1; i--)
            {
                var index =
                    coordinates[i].Index;

                if (index < self.State.MaxLengthPerNode)
                { continue; }

                if (coordinates[i - 1].Index + 1 < coordinates[i].Length)
                {
                    coordinates[i - 1].OverallIndex++;
                    coordinates[i - 1].Index++;
                }

                
                coordinates[i].Index = index;
            }
        }

        public static ReferenceNode<TItem, TKey> GetRefByUniqueKey<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, Action<int, int, ReferenceNode<TItem, TKey>> getCoordinates, ReferenceNode<TItem, TKey> node = null, int lvlIndex = 0)
        {
            node = node ?? self.Root;

            int index = node.Values != null ?
                node.Values.BinarySearch(n => self.CompareKeys(self.Key, self.SelectKey(n.Value))) :
                node.References.BinarySearch(n => self.CompareKeys(self.Key, n.Key));

            getCoordinates(++lvlIndex, index, node);

            if (node.Values != null) { return node; }

            //TODO: Refator to understand fully why does only when inserting the 8 in the tests, it asks for a third reference (father) node
            //POG: 
            index = GetIndexWithinBoundaries(node, index);
            //POG: 
            var found =
                node.References[index];

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

            var refNode = GetRefByUniqueKey(
                self,
                (lvlIndex, index, parentNode) =>
                {
                    if (coordinates.Length <= lvlIndex) { return; }

                    
                    coordinates[lvlIndex] =
                        new Coordinates()
                        {
                            Length = parentNode.Length,
                            Index = index < 0 ? ~index : index,
                            OverallLength = self.State.Levels[lvlIndex].TotalUsed,
                        };
                },
                node);
            SetOverallIndexes(self, coordinates);
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
    }
}

