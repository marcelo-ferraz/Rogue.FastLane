using Rogue.FastLane.Collections.Items;
using System;

namespace Rogue.FastLane.Items
{
    public class ValueHolder<TItem> : INode
    {
        internal ValueHolder(ValueHolder<TItem> prior, ValueHolder<TItem> next, Func<TItem, TItem, int> compare, TItem value) { 
            Prior = prior; 
            Next = next;
            Compare = compare;
            Value = value;
        }
        
        public ValueHolder<TItem> Prior { get; set; }
        public ValueHolder<TItem> Next { get; set; }

        public Func<TItem, TItem, int> Compare { get; set; }

        public TItem Value { get; set; }
    }
}
