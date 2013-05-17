using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue.FastLane.Items;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.State;

namespace Rogue.FastLane.Selectors
{
    public interface ISelector<TItem> 
    { 
        ValueNode<TItem> First();
        void AfterAdd(ValueNode<TItem> node, StructState state);
        void AfterRemove(ValueNode<TItem> item, StructState state);
    }
}
