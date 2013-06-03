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

            Coordinates[] coordinates = null;

            State = state;

            var closestRef =
                this.FirstRefByUniqueKey(ref coordinates);

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

                    Insert(node, coordinates);
                }
        }

        private void Insert(ValueNode<TItem> node, Pair[] coordinateSet)
        {
            ValueNode<TItem> nextFirstNode = node;
            var coordinates = coordinateSet[coordinateSet.Length - 1];

            this.ForEachValuedNode(coordinateSet, 
                (@ref, i) => { 
                    
                    /*
                     * Como resolver o caso de acabarem os valores abaixo do reference node?
                     * preciso manter a referencia do primeiro item e passar como ultimo do reference node anterior
                     */
                    
                    var values = 
                        @ref.Values;

                    if (i > 0) { values[i] = values[i - 1]; }
                    else {
 
                    }

                });

            foreach (var childNode in this.IntoLowestRefsReverse(Root, coordinateSet))
            {
                var finishIndex = 0;//coordinates.Value;
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