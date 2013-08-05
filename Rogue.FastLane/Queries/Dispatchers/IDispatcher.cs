using Rogue.FastLane.Collections;
using Rogue.FastLane.Items;

namespace Rogue.FastLane.Queries.Dispatchers
{
    public interface IDispatcher<TItem>
    {
        /// <summary>
        /// Adds a query to the dispatcher
        /// </summary>
        /// <param name="query"></param>
        void Add(IQuery<TItem> query);

        /// <summary>
        /// Adds to all queries, the value node
        /// </summary>
        /// <param name="struct"></param>
        /// <param name="item"></param>
        void AddNode(OptmizedCollection<TItem> @struct, ValueNode<TItem> item);

        /// <summary>
        /// Removes to all queries, the value node
        /// </summary>
        /// <param name="optmizedStructure"></param>
        /// <param name="node"></param>
        void RemoveNode(OptmizedCollection<TItem> optmizedStructure, ValueNode<TItem> node);
    }
}
