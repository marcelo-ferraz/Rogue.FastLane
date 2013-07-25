using System;
using Rogue.FastLane.Items;
using Rogue.FastLane.Collections;
namespace Rogue.FastLane.Queries.Dispatchers
{
    public interface IDispatcher<TItem>
    {
        void Add(IQuery<TItem> query);
        void AddNode(OptmizedStructure<TItem> @struct, ValueNode<TItem> item);
        void RemoveNode(OptmizedStructure<TItem> optmizedStructure, ValueNode<TItem> node);
    }
}
