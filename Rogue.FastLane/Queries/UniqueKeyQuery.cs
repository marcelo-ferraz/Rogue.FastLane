using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Collections.Items.Mixins;
using System.Threading.Tasks;
using Rogue.FastLane.Queries.States;
using Rogue.FastLane.Collections;

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
                //this.GetValueIndexByUniqueKey(ref closestRef);

            resizeValueCount(this);

            //valueIndex =
            //    this.GetValueIndexByUniqueKey(ref coordinates, ref closestRef);

            //If is found, does not update anything, it was already made
            if (valueIndex >= 0) { return; }
            
            valueIndex = ~valueIndex;

            this.MoveAllAside(coordinates, node);

            if (valueIndex < closestRef.Values.Length)
            {
                closestRef.Values[valueIndex] = node;
            }
            else
            {
                closestRef = closestRef.Next(coordinates);
                (closestRef.Values ??
                    (closestRef.Values = new ValueNode<TItem>[1]))[0] = node;
            }
            
            // corrigir os max keys corretos em toda a arvore. 
            /*
             * 
             */

            var @enum = 
                new LowestReferencesEnumerable<TItem, TKey>();

            foreach(var refNode in @enum.LastNLowestRefs(Root))
            {
                var highestKey = 
                    SelectKey(refNode.Values[refNode.Values.Length - 1].Value);

                ChangeKeyIfHigher(refNode, highestKey);
            }
        }

        private void ChangeKeyIfHigher(ReferenceNode<TItem, TKey> refNode, TKey key)
        {
            if (refNode == null) { return; }

            refNode.Key = key;
            ChangeKeyIfHigher(refNode.Parent, key);            
        }

        public override void Remove(ValueNode<TItem> item) { }
    }
}