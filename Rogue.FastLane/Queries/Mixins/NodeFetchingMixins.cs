using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using System.Collections.Generic;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Mixins;

namespace Rogue.FastLane.Queries.Mixins
{
	public static class NodeFetchingMixins
	{
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
            var offs = (offsets ?? (offsets = new OneTimeValue[self.State.Levels.Length - 1]));
            
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
            var coordinates = (absoluteCoordinates?? 
                (absoluteCoordinates= new Coordinates[self.State.Levels.Length + 1/*- 1*/]));

            int lastIndex = 0;

            var refNode = GetRefByUniqueKey(
                self,
                (lvlIndex, index, n) => {
                    coordinates[lvlIndex] = new Coordinates()
                    {
                        Length = (n.Parent != null ? n.Parent.References.Length : 0),
                        Index = index < 0 ? ~index : index,
                        FullFlatPosition = (index < 0 ? ~index : index) + (lvlIndex * lastIndex)
                    };
                    lastIndex = index;
                }, 
                    node);

            absoluteCoordinates = coordinates;

            return refNode;
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


        /// <summary>
        /// Gets the reference node that holds the value. This value is gotten by the following algorithm:  valueFlatIndex/(state.Length/state.OptimumLenghtPerSegment ^ levelIndex)
        /// </summary>
        /// <returns>
        /// The reference node.
        /// </returns>
        /// <param name='node'>
        /// The Node that holds the persued one.
        /// </param>
        /// <param name='valueFlatIndex'>
        /// The flat index of the Value.
        /// <remarks>The index keeps in mind that the final list is all just the same list</remarks>
        /// </param>
        /// <param name='offsets'>
        /// the selectors State.
        /// </param>
        /// <param name='levelIndex'>
        /// the current Level index.
        /// </param>
        [Obsolete("No real use for it until now, probably will be replaced")]
        public static ReferenceNode<TItem, TKey> GetLastRefNodeByItsValueFlatIndex<TItem, TKey>(ReferenceNode<TItem, TKey> node, int valueFlatIndex, UniqueKeyQueryState state, ref OneTimeValue[] offsets, int levelIndex = 0, int itemCountToTheLeft = 0)
        {
            levelIndex++;
            itemCountToTheLeft *= state.OptimumLenghtPerSegment;
            //through This value is got by the following algorithm:  valueFlatIndex/(state.Length/state.OptimumLenghtPerSegment ^ levelIndex)
            int currentIndex = (int)Math.Floor(
                (double)(valueFlatIndex / (state.Length / Math.Pow(state.OptimumLenghtPerSegment, levelIndex))) - itemCountToTheLeft) - 1;

            itemCountToTheLeft = currentIndex;
            
            (offsets ??
                (offsets = new OneTimeValue[state.Levels.Length]))[levelIndex - 1] = new OneTimeValue { Value = currentIndex };
            
            if (node.Values != null) { return node; }
            
            return GetLastRefNodeByItsValueFlatIndex(
                node.References[currentIndex],
                valueFlatIndex,
                state,
                ref offsets,
                levelIndex,
                itemCountToTheLeft);
        }
        
        public static ReferenceNode<TItem, TKey> GetLastRefNode<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> node = null)
        {
            return (node ?? (node = self.Root)).Values == null ?
                GetLastRefNode(self, node.References[node.References.Length -1]) : 
                node;
        }

        public static IEnumerable<ReferenceNode<TItem, TKey>> IntoLowestRefs<TItem, TKey>(
            this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> root, OneTimeValue[] offsets)
        {
            var iterator = 
                new LowestReferencesEnumerable<TItem, TKey>();

            return iterator.FromHereOn(root, offsets);
        }
        
        //public static IEnumerable<ReferenceNode<TItem, TKey>> IntoLowestRefsReverse<TItem, TKey>(
        //    this UniqueKeyQuery<TItem, TKey> self, ReferenceNode<TItem, TKey> root, OneTimeValue[] offsets)
        //{
        //    var iterator =
        //        new LowestReverseReferencesEnumerable<TItem, TKey>();

        //    return iterator.UpToHere(root, offsets);
        //}
	}
}

