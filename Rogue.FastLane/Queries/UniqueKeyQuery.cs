using System;

using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Items.Mixins;
using Rogue.FastLane.Collections.Mixins;

using Rogue.FastLane.Items;

using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Queries.States;

namespace Rogue.FastLane.Queries
{
    public class UniqueKeyQuery<TItem, TKey> : SimpleQuery<TItem, TKey>
    {
        public UniqueKeyQuery()
        {
            Root =
                new ReferenceNode<TItem, TKey>() { Values = new ValueNode<TItem>[0] };

            State =
                new UniqueKeyQueryState();
        }

        protected internal UniqueKeyQueryState State;

        public override ValueNode<TItem> First()
        {
            var found =
                this.FirstRefByUniqueKey();

            return found.Values.BinaryGet(
                    node =>
                        CompareKeys(Key, SelectKey(node.Value)));
        }

        public ValueNode<TItem> First(TKey key)
        {
            this.Key = key;
            return First();
        }

        public override void AbridgeQueryValueCount(int qtd)
        {
            this.AbridgeValueCount(qtd);
        }

        public override void AbridgeQueryLevelCount(int qtd)
        {
            this.AbridgeLevelCount(qtd);
        }

        public override void AugmentQueryValueCount(int qtd)
        {
            this.AugmentValueCount(qtd);
        }

        public override void AugmentQueryLevelCount(int qtd)
        {
            this.AugmentLevelCount(qtd);
        }

        public override void Add(ValueNode<TItem> node)
        {
            Key = SelectKey(node.Value);

            ReferenceNode<TItem, TKey> closestRef = null;

            var coordinates =
                this.Locate(ref closestRef);

            var valueCoordinates =
                coordinates[coordinates.Length - 1];

            var affected =
                this.MoveAll2TheRight(coordinates);

            closestRef.
                Values[valueCoordinates.Index] = node;


            /*TODO: this stands as part of the solution, whereas the solution must righten all the affected keys, 
             * keeping in mind that the alteration must obey the rule of the most valuable, 
             * or just the key of the last child.
             */

            //while (affected.Count > 0)
            //{
            //    var @ref = affected.Pop();
            //    @ref.Key =
            //        SelectKey(@ref.Values[@ref.Values.Length - 1].Value);
            //    ChangeKey2LastKey(@ref.Parent);
            //}


            StraightUpKeys();
        }

        private void StraightUpKeys()
        {
            var @enum =
                new LowestReferencesEnumerable<TItem, TKey>();

            foreach (var refNode in @enum.LastNLowestRefs(Root))
            {
                var highestKey =
                    SelectKey(refNode.Values[refNode.Values.Length - 1].Value);

                ChangeKey2LastKey(refNode, highestKey);
            }
        }

        private void ChangeKey2LastKey(ReferenceNode<TItem, TKey> refNode, TKey key)
        {
            if (refNode == null) { return; }

            refNode.Key = key;
            ChangeKey2LastKey(refNode.Parent, key);
        }

        private void ChangeKey2LastKey(ReferenceNode<TItem, TKey> refNode)
        {
            if (refNode == null) { return; }

            var lastIndex =
                refNode.Length - 1;

            refNode.Key =
                refNode.References[lastIndex].Key;

            ChangeKey2LastKey(refNode.Parent);
        }

        public override void Remove(ValueNode<TItem> item)
        {
            Key = this.SelectKey(item.Value);

            ReferenceNode<TItem, TKey> closestRef = null;

            var coordinates =
                this.Locate(ref closestRef, ephemeral: true);
            
            this.MoveAll2TheLeft(coordinates);

            StraightUpKeys();
        }
    }
}