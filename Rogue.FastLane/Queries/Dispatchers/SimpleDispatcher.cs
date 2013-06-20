using Rogue.FastLane.Collections;
using Rogue.FastLane.Items;
using Rogue.FastLane.Collections.Mixins;

namespace Rogue.FastLane.Queries.Dispatchers
{
    public abstract class SimpleDispatcher<TItem>
    {
        private int CurrentQueryIndex = 0;

        protected IQuery<TItem>[] Queries;

        public SimpleDispatcher() { }

        public SimpleDispatcher(params IQuery<TItem>[] queries)
        {
            Queries = queries;
        }

        protected abstract void DeriveNewState(OptmizedStructure<TItem> @struct);

        protected abstract void TryChangeQueryValueCount();

        protected abstract void TryChangeQueryLevelCount();

        protected abstract void SaveState();        

        protected internal void Add(IQuery<TItem> query)
        {
            Queries = 
                Queries.Resize(Queries.Length + 1);

            Queries[Queries.Length-1] = query;
        }

        public virtual void ChangeStructures4Queries(OptmizedStructure<TItem> @struct)
        {
            DeriveNewState(@struct);

            TryChangeQueryLevelCount();

            TryChangeQueryValueCount();

            SaveState();
        }

        public virtual void Add2QueriesStructures(ValueNode<TItem> item)
        {
            foreach (var query in Queries) 
            {
                query.Add(item);
            }
        }
    }
}