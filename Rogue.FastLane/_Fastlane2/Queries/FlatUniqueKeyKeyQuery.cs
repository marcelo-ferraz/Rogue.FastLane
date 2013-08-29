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
        public TKey[] Keys { get; set; }

        public ValueNode<TItem>[] Items { get; set; }

        public State<TItem, TKey> Next { get; set; }
    }

    public class IndexNState<TItem, TKey>
    {
        public int Index { get; set; }
        public State<TItem, TKey> State { get; set; }
    }

    public class FlatUniqueKeyKeyQuery<TItem, TKey> : IQuery<TItem>
    {
        protected State<TItem, TKey> CurrentState;

        public virtual Func<TItem, TKey> SelectKey { get; set; }

        public FlatUniqueKeyKeyQuery()
        {
            CurrentState =
                new State<TItem, TKey>();
        }

        protected IndexNState<TItem, TKey> GetByKey(ValueNode<TItem> item)
        {
            int index;
            int length;
            var state = CurrentState;
            var key =
                SelectKey(item.Value);
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



        public void Add(ValueNode<TItem> item)
        {
            var result =
                GetByKey(item);

            if (result.Index < 0)
            {
                var index = ~result.Index;

                var state =
                    GetLastState(this.CurrentState);

                var length =
                    state.Keys.Length;

                state.Items.Resize(++length);
                state.Keys.Resize(length);

                for (int i = index; i < length; i++)
                {
                    CurrentState.Keys[i] = CurrentState.Keys[i - 1];
                    CurrentState.Items[i] = CurrentState.Items[i - 1];
                }
            }

            CurrentState.Keys[index] = key;
            CurrentState.Items[index] = item;
        }

        public void Remove(ValueNode<TItem> item)
        {
            int index;
            int length =
                this.CurrentState.Keys.Length;

            var key =
                SelectKey(item.Value);

            if (!SZFunctions.TrySZBinarySearch(this.CurrentState.Keys, 0, length, key, out index))
            { return; }


            for (int i = index; i < length; i++)
            {
                CurrentState.Keys[i - 1] = CurrentState.Keys[i];
                CurrentState.Items[i - 1] = CurrentState.Items[i];
            }

            this.CurrentState.Items.Resize(--length);
            this.CurrentState.Keys.Resize(length);
        }

        public string Name { get; set; }

    }
}