using System;

using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Items.Mixins;
using Rogue.FastLane.Collections.Mixins;

using Rogue.FastLane.Items;

using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Queries.States;
using System.Runtime;
using System.Collections.Generic;

namespace Rogue.FastLane.Queries
{
    public class UniqueKeyQuery<TItem, TKey> : SimpleQuery<TItem, TKey>, IUniqueKeyQuery<TItem>, ICrudQuery<TItem>
    {
        public UniqueKeyQuery()
        {
            Root =
                new ReferenceNode<TItem, TKey>()
                {
                    Values = new ValueNode<TItem>[0],
                    Comparer = this.KeyComparer
                };

            State =
                new UniqueKeyQueryState();
        }

        public UniqueKeyQueryState State { get; set; }

        [TargetedPatchingOptOut("")]
        public override ValueNode<TItem> First()
        {
            var found =
                this.FirstRefByUniqueKey();

            return found.Values.BinaryGet(
                    node =>
                        KeyComparer(Key, SelectKey(node.Value)));
        }

        public ValueNode<TItem> Get(TKey key)
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
            
            ReferenceNode<TItem, TKey> @ref = null;

            while (affected.Count > 0)
            {
                try
                {
                    @ref = affected.Pop();
                    @ref.Key =
                        SelectKey(@ref.Values[@ref.Values.Length - 1].Value);
                    ChangeKey2LastKey(@ref.Parent);

                }
                catch (NullReferenceException ex)
                {
                    /*Minor fix, before evaluating the calculus precision.
                     * On tests, it fails after 5 million.
                     * 
                     * values failed:
                     * - 5044805
                     */
                    if (@ref == null) { throw ex; }
                    @ref.Values =
                        @ref.Values.Resize(@ref.Values.Length - 1);
                    ChangeKey2LastKey(@ref.Parent);
                }
            }
        }

        private void StraightUpKeys()
        {
            var @enum =
                new LowRefsEnumerable<TItem, TKey>();

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

        public void Remove(TKey key)
        {
            Key = key;
            ReferenceNode<TItem, TKey> closestRef = null;

            var coordinates =
                this.Locate(ref closestRef, ephemeral: true);

            this.MoveAll2TheLeft(coordinates);

            StraightUpKeys();
        }

        public override void Remove(ValueNode<TItem> item)
        {
            Remove(this.SelectKey(item.Value));
        }

        public override IEnumerable<TItem> Enumerate()
        {
            var iterator = 
                new LowRefsEnumerable<TItem, TKey>();

            foreach (var item in iterator.AllFrom(Root))
            { 
                var len = item.Values.Length;
                for (var i = 0; i < len; i++)
                {
                    yield return item.Values[i].Value;
                }
            }
        }
    }
}