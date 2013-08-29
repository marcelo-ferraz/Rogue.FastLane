using System;

using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Items.Mixins;
using Rogue.FastLane._Fastlane2.Collections.Mixins;

using Rogue.FastLane.Items;

using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Queries.States;
using System.Runtime;
using System.Collections.Generic;
using Rogue.FastLane._Fastlane2.Infrastructure;
using Rogue.FastLane.Queries;

namespace Rogue.FastLane._Fastlane2.Queries
{
    public class State<TItem, TKey>
    {
        public State()
        {
            Keys = this.CreateArray<TKey>();
            Items = this.CreateArray<ValueNode<TItem>>();
        }

        public TKey[] Keys { get; set; }

        public ValueNode<TItem>[] Items { get; set; }

        public State<TItem, TKey> Next { get; set; }
    }

    public class IndexNState<TItem, TKey>
    {
        public int Index { get; set; }
        public State<TItem, TKey> State { get; set; }
    }

    public class FlatUniqueKeyKeyQuery<TItem, TKey> : ICrudQuery<TItem>
    {
        protected State<TItem, TKey> CurrentState;

        public virtual Func<TItem, TKey> SelectKey { get; set; }

        public FlatUniqueKeyKeyQuery()
        {
            CurrentState =
                new State<TItem, TKey>();
        }

        protected IndexNState<TItem, TKey> GetByKey(TKey key)
        {
            int index;
            int length;
            var state = CurrentState;
            do
            {
                length = state.Keys.Length;

                if (ArrayMixins.TrySZBinarySearch(state.Keys, 0, length, key, out index))
                {
                    return
                        new IndexNState<TItem, TKey>
                        {
                            State = state,
                            Index = index
                        };
                }
            }
            while ((state = state.Next) != null);

            return new IndexNState<TItem, TKey>() { Index = index };
        }

        protected State<TItem, TKey> GetLastState(State<TItem, TKey> state)
        {
            while (state.Next != null)
            {
                state = state.Next;
            }
            return state;
        }

        protected State<TItem, TKey> Go2Next(State<TItem, TKey> state)
        {
            if(state.Next != null)
            { return state.Next; }

            state.Next = 
                new State<TItem, TKey>();

            return state;
        }


        public void Add(ValueNode<TItem> item)
        {
            var key =
                SelectKey(item.Value);
            
            var result =
                GetByKey(key);

            if (result.Index < 0)
            {
                var index = ~result.Index;

                //TODO: calcular o valor maximo que a lista pode ter para ocupar menos de 85kb, em int eh algo perto de 21.242
                //                   \/
                var state = index >= 99 ?
                    Go2Next(result.State) :
                    result.State;

                state.Keys.Insert(index, SelectKey(item.Value));
                state.Items.Insert(index, item);
            }
            else
            {
                result.State.Keys[result.Index] = key;
                result.State.Items[result.Index] = item;
            }
        }

        public void Remove(ValueNode<TItem> item)
        {
            var key =
                SelectKey(item.Value);

            var result =
                GetByKey(key);

            // not found
            if (result.Index < 0) { return; }

            result.State.Keys.Remove(result.Index);
            result.State.Items.Remove(result.Index);
        }

        public string Name { get; set; }

    }
}