using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.Mixins;
using System;

namespace Rogue.FastLane.Queries.Dispatchers
{
    public abstract class SimpleDispatcher<TItem, TQuery> : IDispatcher<TItem>
        where TQuery : IQuery<TItem>, ICrudQuery<TItem>
    {
        protected TQuery[] Queries;
        protected TQuery CurrentQuery;
        public SimpleDispatcher() { }

        public SimpleDispatcher(params TQuery[] queries)
        {
            Queries = queries;
        }

        /// <summary>
        /// When inherited, derive a new state of that query
        /// </summary>
        protected abstract void DeriveNewState(OptimizedCollection<TItem> @struct);

        /// <summary>
        /// When inherited, it tries to change the value count of that query
        /// </summary>
        protected abstract void TryChangeQueryValueCount();

        /// <summary>
        /// When inherited, it tries to change the reference count of that query
        /// </summary>
        protected abstract void TryChangeQueryLevelCount();

        /// <summary>
        /// When inherited, saves the state of that query
        /// </summary>
        protected abstract void SaveState();

        /// <summary>
        /// Adds a query to the dispatcher
        /// </summary>
        /// <param name="query"></param>
        public void Add(IQuery<TItem> query)
        {
            Queries =
                Queries.Resize(Queries.Length + 1);

            Queries[Queries.Length - 1] = (TQuery)query;
        }
        
        /// <summary>
        /// Adds to all queries, the value node
        /// </summary>
        /// <param name="struct"></param>
        /// <param name="item"></param>
        public virtual void AddNode(OptimizedCollection<TItem> @struct, ValueNode<TItem> item)
        {
            DeriveNewState(@struct);

            foreach (var query in Queries)
            { AddInEach(query, item); }
        }

        /// <summary>
        /// Adds to each query, the value node
        /// </summary>
        /// <param name="query"></param>
        /// <param name="item"></param>
        protected virtual void AddInEach(TQuery query, ValueNode<TItem> item)
        {
            CurrentQuery = query;

            TryChangeQueryLevelCount();

            TryChangeQueryValueCount();

            CurrentQuery.Add(item);

            SaveState();
        }

        /// <summary>
        /// Removes to all queries, the value node
        /// </summary>
        /// <param name="optmizedStructure"></param>
        /// <param name="node"></param>
        public virtual void RemoveNode(OptimizedCollection<TItem> @struct, ValueNode<TItem> item)
        {
            DeriveNewState(@struct);

            foreach (var query in Queries)
            { RemoveInEach(query, item); }
        }

        /// <summary>
        /// Removes in each query, the value node
        /// </summary>
        /// <param name="query"></param>
        /// <param name="item"></param>
        protected virtual void RemoveInEach(TQuery query, ValueNode<TItem> item)
        {
            CurrentQuery = query;

            CurrentQuery.Remove(item);

            TryChangeQueryValueCount();

            TryChangeQueryLevelCount();

            SaveState();
        }
    }
}