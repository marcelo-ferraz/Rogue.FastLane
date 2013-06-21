using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Collections.Items.Mixins;
using System.Threading.Tasks;
using Rogue.FastLane.Queries.States;

namespace Rogue.FastLane.Queries
{
    public class UniqueKeyQuery<TItem, TKey> : SimpleQuery<TItem, TKey>
    {
        public UniqueKeyQuery()
        {
            Root = 
                new ReferenceNode<TItem, TKey>() 
                { Values = new ValueNode<TItem>[0] };

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

        public override void AbridgeQueryValueCount(int qtd)
        {
            throw new NotImplementedException();
        }

        public override void AbridgeQueryLevelCount(int qtd)
        {
            throw new NotImplementedException();
        }

        public override void AugmentQueryValueCount(int qtd)
        {            
            this.AugmentValueCount(qtd);
        }

        public override void AugmentQueryLevelCount(int qtd)
        {
            this.AugmentLevelCount(qtd);
        }

        public override void Add(ValueNode<TItem> node, Action<IQuery<TItem>> resizeValueCount)
        {
            Key = SelectKey(node.Value);

            Coordinates[] coordinates = null;
            ReferenceNode<TItem, TKey> closestRef = null;

            var valueIndex =
                this.GetValueIndexByUniqueKey(ref coordinates, ref closestRef);

            //If is found, does not update anything, it was already made
            if (valueIndex >= 0) { return; }
            
            valueIndex = 
                ~valueIndex;
            
            resizeValueCount(this);

            this.MoveAllAside(coordinates, node);

            if (valueIndex < closestRef.Values.Length)
            {
                closestRef.Values[valueIndex] = node;
            }
            else
            {
                (closestRef.Next(coordinates).Values ??
                    (closestRef.Next(coordinates).Values = new ValueNode<TItem>[1]))[0] = node;
            }
        }

        public override void Remove(ValueNode<TItem> item) { }
    }
}