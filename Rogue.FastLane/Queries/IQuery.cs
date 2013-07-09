using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue.FastLane.Items;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Queries.States;

namespace Rogue.FastLane.Queries
{
    public interface IQuery<TItem> 
    {
        ValueNode<TItem> First();

        void AugmentQueryValueCount(int qtd);
        
        void AbridgeQueryValueCount(int qtd);
        
        void AugmentQueryLevelCount(int qtd);

        void AbridgeQueryLevelCount(int qtd);

        void Add(ValueNode<TItem> item);
        
        void Remove(ValueNode<TItem> item);
    }
}
