using Rogue.FastLane.Collections.Items;
using System;

namespace Rogue.FastLane.Items
{
    public class ValueNode<TItem> : INode
    {
        public ValueNode<TItem> Prior { get; set; }
        public ValueNode<TItem> Next { get; set; }
        
        public TItem Value { get; set; }
    }
}
