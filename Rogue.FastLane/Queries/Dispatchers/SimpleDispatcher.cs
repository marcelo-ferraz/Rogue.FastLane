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

        public SimpleDispatcher() { }

        public SimpleDispatcher(params TQuery[] queries)
        {
            Queries = queries;
        }

        protected abstract void DeriveNewState(OptmizedStructure<TItem> @struct);

        protected abstract void TryChangeQueryValueCount(TQuery query);

        protected abstract void TryChangeQueryLevelCount(TQuery query);

        protected abstract void SaveState();

        protected internal virtual void AddNode2Queries(ValueNode<TItem> item, Action<TQuery> resizeValueCount)
        {
            foreach (var query in Queries)
            {
                query.Add(item, q => resizeValueCount((TQuery)q));
            }
        }

        protected virtual void ApplyToEach(TQuery query, ValueNode<TItem> item)
        {
            TryChangeQueryLevelCount(query);

            AddNode2Queries(item, TryChangeQueryValueCount);

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
            {
                ApplyToEach(query, item);
            }
        }
    }
}