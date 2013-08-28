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
using Rogue.FastLane._Fastlane2.Collections.Mixins;

namespace Rogue.FastLane.Queries
{
    public class FlatUniqueKeyKeyQuery<TItem, TKey> : IQuery<TItem>, ICrudQuery<TItem>
    {
        protected TKey[] Keys;

        protected ValueNode<TItem>[] Items;

        public virtual Func<TItem, TKey> SelectKey { get; set; }
        
        public FlatUniqueKeyKeyQuery()
        {

        }

        public void Add(ValueNode<TItem> item)
        {
            int index;
            int length = 
                this.Keys.Length;
            
            var key = 
                SelectKey(item.Value);

            if (!SZMixins.TrySZBinarySearch(this.Keys, 0, length, key, out index)) 
            { 
                index = ~index;

                this.Items.Resize(++length);
                this.Keys.Resize(length);

                for (int i = index; i < length; i++) 
                {
                    Keys[i] = Keys[i-1];
                    Items[i] = Items[i -1];
                }
            }

            Keys[index] = key;
            Items[index] = item;
        }

        public void Remove(ValueNode<TItem> item)
        {
            int index;
            int length =
                this.Keys.Length;

            var key =
                SelectKey(item.Value);

            if (!SZMixins.TrySZBinarySearch(this.Keys, 0, length, key, out index))
            { return; }


            for (int i = index; i < length; i++)
            {
                Keys[i-1] = Keys[i];
                Items[i-1] = Items[i];
            }
            
            this.Items.Resize(--length);
            this.Keys.Resize(length);
        }

        public string Name { get; set; }

        public ValueNode<TItem> First()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TItem> Enumerate()
        {
            throw new NotImplementedException();
        }

        public void AugmentQueryValueCount(int qtd)
        {
            throw new NotImplementedException();
        }

        public void AbridgeQueryValueCount(int qtd)
        {
            throw new NotImplementedException();
        }

        public void AugmentQueryLevelCount(int qtd)
        {
            throw new NotImplementedException();
        }

        public void AbridgeQueryLevelCount(int qtd)
        {
            throw new NotImplementedException();
        }
    }
}