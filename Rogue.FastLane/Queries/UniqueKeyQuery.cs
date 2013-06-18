using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Collections.Items.Mixins;
using System.Threading.Tasks;
using Rogue.FastLane.Queries.State;

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

        public override void AbridgeQueryValueCount(UniqueKeyQueryState newState)
        {
            throw new NotImplementedException();
        }

        public override void AbridgeQueryLevelCount(UniqueKeyQueryState newState)
        {
            throw new NotImplementedException();
        }

        public override void AugmentQueryValueCount(UniqueKeyQueryState newState)
        {
            this.AugmentValueCount(newState.LevelCount - State.LevelCount);
        }
        
        public override void AugmentQueryLevelCount(UniqueKeyQueryState newState)
        {
            this.AugmentLevelCount(newState.LevelCount - State.LevelCount);
        }

        public override void Add(ValueNode<TItem> node)
        {
            Coordinates[] coordinates = null;
            ReferenceNode<TItem, TKey> closestRef = null;

            var valueIndex =
                this.GetValueIndexByUniqueKey(ref coordinates, ref closestRef);

            //If is found, does not update anything, it was already made
            if (valueIndex >= 0) { return; }

            this.MoveAndInsert(coordinates, node);

            //until this, seems to be accetable
            //now has to re-find, or recalculate the position finding the right reference node
            Insert(node, closestRef, ~valueIndex, coordinates);

            if (closestRef.Values == null)
            {
                valueIndex =
                    this.GetValueIndexByUniqueKey(ref coordinates, ref closestRef);
            }

            valueIndex = 
                ~valueIndex;
            
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
    }
}