using Rogue.FastLane.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue.FastLane.Collections.Items;

namespace Rogue.FastLane.Items
{
    public class ValueNode<TItem> : INode
    {
        public ValueNode<TItem> Prior { get; set; }
        public ValueNode<TItem> Next { get; set; }
        public TItem Value { get; set; }
    }
}
