using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Collections.Items.Mixins;
using System.Threading.Tasks;

namespace Rogue.FastLane.Queries
{
    public class UniqueKeyQuery<TItem, TKey> : SimpleQuery<TItem, TKey>
    {
        public UniqueKeyQuery()
        {
            Root = new ReferenceNode<TItem, TKey>() { Values = new ValueNode<TItem>[0] };
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
            State = state;

            Coordinates[] coordinates = null;

            ReferenceNode<TItem, TKey> closestRef = null;

            var valueIndex =
                this.GetValueIndexByUniqueKey(ref coordinates, ref closestRef);

            //talvez nao seja necessário realizar o update, ja que o mesmo ja acontece antes, com outro ponteiro, na estrutura real
            if (valueIndex >= 0) { return; }

            this.AugmentValueCount(1);
            
            this.MoveAndInsert(coordinates, node);
            //until this, seems to be accetable
            //now has to re-find, or recalculate the position finding the right reference node
            Insert(node, closestRef, ~valueIndex, coordinates);
        }

        private void Insert(ValueNode<TItem> node, ReferenceNode<TItem, TKey> closestRef, int index, Coordinates[] coordinateSet)
        {
            if (closestRef.Values == null)
            {
                index = 
                    this.GetValueIndexByUniqueKey(ref coordinateSet, ref closestRef);
            }

            if (index < closestRef.Values.Length)
            {
                closestRef.Values[index] = node;
            }
            else 
            {
                (closestRef.Next(coordinateSet).Values ??
                    (closestRef.Next(coordinateSet).Values = new ValueNode<TItem>[1]))[0] = node;
            }
        }


        public override void AfterRemove(ValueNode<TItem> item, UniqueKeyQueryState state)
        {
            throw new NotImplementedException();
        }
    }
}