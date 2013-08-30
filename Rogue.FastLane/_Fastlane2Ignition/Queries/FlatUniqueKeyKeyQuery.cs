using System;
using System.Collections.Generic;
using Rogue._fastlane2.FastLane.Queries;
using Rogue.FastLane._Fastlane2.Collections.Mixins;
using Rogue.FastLane._Fastlane2.Infrastructure;
using Rogue.FastLane.Items;

namespace Rogue.FastLane._Fastlane2.Queries
{
    public class State<TItem, TKey>
    {
        public State()
        {
            Keys = this.CreateArray<TKey>();
            Items = this.CreateArray<ValueHolder<TItem>>();
        }

        public TKey[] Keys { get; set; }

        public ValueHolder<TItem>[] Items { get; set; }

        public State<TItem, TKey> Next { get; set; }
    }

    public class IndexNState<TItem, TKey>
    {
        public int Index { get; set; }
        public State<TItem, TKey> State { get; set; }
    }

    public class FlatUniqueKeyKeyQuery<TItem, TKey> : IQuery<TItem>, IEnumerable<ValueHolder<TItem>>
    {     
        protected int Maxlength;

        protected State<TItem, TKey> CurrentState;

        public virtual Func<TItem, TKey> SelectKey { get; set; }

        public FlatUniqueKeyKeyQuery()
        {
            CurrentState =
                new State<TItem, TKey>();
            
            var maxLengthSizeOfKeys = 
                Calculate.MaxLengthOfArray<TKey>();
            var maxLengthOfItems = 
                Calculate.MaxLengthOfArray<TItem>();
            
            Maxlength = Math.Min(maxLengthOfItems, maxLengthSizeOfKeys);
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
            if (state.Next != null)
            { return state.Next; }

            state.Next =
                new State<TItem, TKey>();

            return state;
        }

        public void Add(ValueHolder<TItem> item)
        {
            var key =
                SelectKey(item.Value);

            var result =
                GetByKey(key);

            if (result.Index < 0)
            {
                var index = ~result.Index;

                var state = index >= Maxlength ?
                    Go2Next(result.State) :
                    result.State;

                state.Keys.Insert(index, SelectKey(item.Value));
                state.Items.Insert(index, item);
            }
            else
            {
                //result.State.Keys[result.Index] = key;
                result.State.Items[result.Index] = item;
            }
        }

        public void Remove(TItem item)
        {
            var key =
                SelectKey(item);

            var result =
                GetByKey(key);

            // not found
            if (result.Index < 0) { return; }

            result.State.Keys.Remove(result.Index);
            result.State.Items.Remove(result.Index);
        }

        public string Name { get; set; }


        public IEnumerator<ValueHolder<TItem>> GetEnumerator()
        {
            var state = CurrentState;
            do
            {
                for (int i = 0; i < state.Items.Length; i++)
                { 
                    yield return state.Items[i];
                }                    
            }
            while ((state = state.Next) != null);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}