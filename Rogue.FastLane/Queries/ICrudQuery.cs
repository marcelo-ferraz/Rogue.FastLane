using Rogue.FastLane.Items;

namespace Rogue.FastLane.Queries
{
    public interface ICrudQuery<TItem> 
    {
        void Add(ValueHolder<TItem> item);
        
        void Remove(ValueHolder<TItem> item);
    }
}
