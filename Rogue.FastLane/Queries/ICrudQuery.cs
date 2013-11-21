using Rogue.FastLane.Collections.Items;

namespace Rogue.FastLane.Queries
{
    public interface ICrudQuery<TItem> 
    {
        void Add(ValueNode<TItem> item);
        
        void Remove(ValueNode<TItem> item);
    }
}
