using Rogue.FastLane.Items;

namespace Rogue.FastLane.Queries
{
    public interface IQuery<TItem> 
    {
        string Name { get; set; }

        ValueNode<TItem> First();

        void AugmentQueryValueCount(int qtd);
        
        void AbridgeQueryValueCount(int qtd);
        
        void AugmentQueryLevelCount(int qtd);

        void AbridgeQueryLevelCount(int qtd);

        void Add(ValueNode<TItem> item);
        
        void Remove(ValueNode<TItem> item);
    }
}
