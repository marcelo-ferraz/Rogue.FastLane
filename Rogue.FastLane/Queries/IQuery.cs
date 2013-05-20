using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue.FastLane.Items;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.State;

namespace Rogue.FastLane.Queries
{
    public interface IQuery<TItem> 
    { 
        ValueNode<TItem> First();
        void AfterAdd(ValueNode<TItem> node, UniqueKeyQueryState state);
        void AfterRemove(ValueNode<TItem> item, UniqueKeyQueryState state);
    }
}
