using Rogue.FastLane.Items;
using System.Collections.Generic;

namespace Rogue.FastLane.Queries
{
    public interface IQuery<TItem> 
    {
        string Name { get; set; }

        ValueHolder<TItem> First();

        IEnumerable<TItem> Enumerate();

        void AugmentQueryValueCount(int qtd);
        
        void AbridgeQueryValueCount(int qtd);
        
        void AugmentQueryLevelCount(int qtd);

        void AbridgeQueryLevelCount(int qtd);
    }
}
