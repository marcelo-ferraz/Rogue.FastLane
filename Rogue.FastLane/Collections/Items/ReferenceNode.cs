using Rogue.FastLane.Items;

namespace Rogue.FastLane.Collections.Items
{
    public class ReferenceNode<TItem, TKey> : Node<TItem, TKey>
    {        
        public TKey Key { get; set; }
        public ReferenceNode<TItem, TKey>[] References { get; set; }
        public ValueNode<TItem>[] Values { get; set; }

        public INode this[int index]
        {
            get {
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
    }
}
