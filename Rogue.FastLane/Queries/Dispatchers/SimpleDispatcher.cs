using Rogue.FastLane.Collections;
using Rogue.FastLane.Items;
using Rogue.FastLane.Collections.Mixins;
using System;

namespace Rogue.FastLane.Queries.Dispatchers
{
    public abstract class SimpleDispatcher<TItem, TQuery> : IDispatcher<TItem>
        where TQuery: IQuery<TItem>
    {
        private int CurrentQueryIndex = 0;

        protected TQuery[] Queries;
        protected TQuery CurrentQuery;
        public SimpleDispatcher() { }

        public SimpleDispatcher(params TQuery[] queries)
        {
            Queries = queries;
        }
        
        protected abstract void DeriveNewState(OptmizedStructure<TItem> @struct);

        protected abstract void TryChangeQueryValueCount();

        protected abstract void TryChangeQueryLevelCount();

        protected abstract void SaveState();

        protected internal virtual void AddNode2Query(ValueNode<TItem> item)
        {
            CurrentQuery.Add(item);            
        }

        protected internal virtual void RemoveNodeInQuery(ValueNode<TItem> item)
        {
            CurrentQuery.Remove(item);
        }

        protected virtual void AddInEach(TQuery query, ValueNode<TItem> item)
        {
            CurrentQuery = query;

            TryChangeQueryLevelCount();

            TryChangeQueryValueCount();

            AddNode2Query(item);

            SaveState();      
        }

        protected virtual void RemoveInEach(TQuery query, ValueNode<TItem> item)
        {
            CurrentQuery = query;

            RemoveNodeInQuery(item);

            TryChangeQueryValueCount();
            
            TryChangeQueryLevelCount();            

            SaveState();
        }

        public void Add(IQuery<TItem> query)
        {
            Queries =
                Queries.Resize(Queries.Length + 1);

            Queries[Queries.Length - 1] = (TQuery)query;
        }

        public virtual void AddNode(OptmizedStructure<TItem> @struct, ValueNode<TItem> item)
        {
            DeriveNewState(@struct);

            foreach (var query in Queries) 
            { AddInEach(query, item); }
        }

        public virtual void RemoveNode(OptmizedStructure<TItem> @struct, ValueNode<TItem> item)
        {
            DeriveNewState(@struct);

            foreach (var query in Queries)
            { RemoveInEach(query, item); }
        }
    }
}