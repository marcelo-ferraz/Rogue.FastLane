using Rogue.FastLane.Collections.Items;
using System;

namespace Rogue.FastLane.Collections.Items
{
    public class ValueNode<TItem> : INode
    {
        public ValueNode<TItem> Prior { get; set; }
        public ValueNode<TItem> Next { get; set; }

        public Func<TItem, TItem, int> Compare { get; set; }

        public TItem Value { get; set; }
    }
}
