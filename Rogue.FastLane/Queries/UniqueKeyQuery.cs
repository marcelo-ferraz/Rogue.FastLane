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
        public UniqueKeyQuery()
        {
            Root = new ReferenceNode<TItem, TKey>() { Values = new ValueNode<TItem>[0], References = new ReferenceNode<TItem,TKey>[0] };
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


        public override void AfterAdd(ValueNode<TItem> node, UniqueKeyQueryState state)
        {
            Key =
                SelectKey(node.Value);

            OneTimeValue[] offsets = null;

            State = state;

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
                    if(this.NeedsAugmentation(closestRef, 1))
                    {
                        this.AugmentValueCount(Root, 1);
                    }

                    Insert(node, offsets);
                }
        }

        private void Insert(ValueNode<TItem> node, OneTimeValue[] offsets)
        {
            ValueNode<TItem> nextFirstNode = node;
            var offset = offsets[offsets.Length - 1];
            foreach (var childNode in this.IntoLowestRefs(Root, offsets))
            {
                var finishIndex = offset.Value;
                for (var i = childNode.Values.Length; i > finishIndex; i--)
                {
                    if ((i - 1) < childNode.Values.Length)
                    { childNode.Values[i + 1] = childNode.Values[i]; }
                    else
                    { nextFirstNode = childNode.Values[i]; }
                }

                childNode.Values[finishIndex] = node;
                childNode.Key = SelectKey(
                    childNode.Values[childNode.Values.Length - 1].Value);

                node = nextFirstNode;
            }
        }

        public override void AfterRemove(ValueNode<TItem> item, UniqueKeyQueryState state)
        {
            throw new NotImplementedException();
        }
    }
}