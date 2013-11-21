using Rogue.FastLane.Collections.Items;
using System;

namespace Rogue.FastLane.Collections.Items
{
    public class ReferenceNode<TItem, TKey>: INode, IComparable
    {
        public ReferenceNode<TItem, TKey> Parent { get; set; }
        public TKey Key { get; set; }
        public ReferenceNode<TItem, TKey>[] References { get; set; }
        public ValueNode<TItem>[] Values { get; set; }
        public Func<TKey, TKey, int> Comparer { get; set; }

        public INode this[int index]
        {
            get 
            {
                return ((INode[])this.Values ?? this.References)[index];
            }
        }

        public int Length
        {
            get 
            {
                var collection = 
                    ((INode[])this.Values ?? this.References);
                return collection != null ? collection.Length : 0;
            }
        }

        public int CompareTo(object obj)
        {
            return Comparer((TKey)obj, this.Key);
        }
    }
}
