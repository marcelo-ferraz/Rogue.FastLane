using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.FastLane.Collections.Items
{
    public abstract class Node<TItem, TKey>: INode
    {
        public ReferenceNode<TItem, TKey> Parent { get; set; }
    }
}
