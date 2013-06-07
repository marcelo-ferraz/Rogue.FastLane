using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.Mixins;
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
                this.FirstValueIndexByUniqueKey(ref coordinates, ref closestRef);

            //talvez nao seja necessário realizar o update, ja que o mesmo ja acontece antes, com outro ponteiro, na estrutura real
            if (valueIndex >= 0) { return; }

            if (this.NeedsAugmentation(closestRef, 1))
            {
                this.AugmentValueCount(Root, 1);
            }

            MoveAll(coordinates);

            closestRef.Values[~valueIndex] = node;
        }

        private void MoveAll(Coordinates[] coordinateSet)
        {
            ReferenceNode<TItem, TKey> previousRef = null;
            var coordinates = coordinateSet[coordinateSet.Length - 1];

            this.ForEachValuedNode(coordinateSet, 
                (@ref, i) => { 
                    
                    if (i < 1) 
                    { previousRef = @ref; } 
                    
                    else if (previousRef == null)
                    { @ref.Values[i] = @ref.Values[i - 1]; }
                    
                    else
                    { previousRef.Values[i] = @ref.Values[i]; }
                });
        }

        public override void AfterRemove(ValueNode<TItem> item, UniqueKeyQueryState state)
        {
            throw new NotImplementedException();
        }
    }
}