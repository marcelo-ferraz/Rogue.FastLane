using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.Mixins;

namespace Rogue.FastLane.Queries
{
    public class UniqueKeyQuery<TItem, TKey> : SimpleQuery<TItem, TKey>
    {
        protected internal UniqueKeyQueryState State;

        protected virtual ReferenceNode<TItem, TKey> FirstReference(TKey key, ReferenceNode<TItem, TKey> node, ref ValueOneTime[] offsets)
        {
            return this.FirstRefByUniqueKey(ref offsets);
        }

        public override void AfterAdd(ValueNode<TItem> node, UniqueKeyQueryState state)
        {
            Key =
                SelectKey(node.Value);

            ValueOneTime[] offsets = null;

            var closestRef =
                this.FirstRefByUniqueKey(ref offsets);

            var valueIndex = closestRef.Values.BinarySearch(
                val =>
                    CompareKeys(Key, SelectKey(val.Value)));

            //talvez nao seja necessário realizar o update, ja que o mesmo ja acontece antes, com outro ponteiro, na estrutura real
            if (valueIndex >= 0)
            { closestRef.Values[valueIndex] = node; }
            else
                if (valueIndex < 0)
                {
                    this.AugmentValueCount(Root, 1);

                    Insert(node, offsets);
                }
        }

        private void Insert(ValueNode<TItem> node, ValueOneTime[] offsets)
        {
            ValueNode<TItem> nextFirstNode = node;
            var offset = offsets[offsets.Length - 1];
            foreach (var childNode in this.IterateTroughLowestReferences(Root, offsets))
            {
                var startAt = offset.Value;
                for (var i = startAt; i < childNode.Values.Length; i++)
                {
                    if ((i + 1) < childNode.Values.Length)
                    { childNode.Values[i + 1] = childNode.Values[i]; }
                    else
                    { nextFirstNode = childNode.Values[i]; }
                }
                childNode.Values[startAt] = nextFirstNode;
            }
        }

        public override void AfterRemove(ValueNode<TItem> item, UniqueKeyQueryState state)
        {
            throw new NotImplementedException();
        }
    }
}