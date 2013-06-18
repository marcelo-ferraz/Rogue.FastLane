using Rogue.FastLane.Collections;
using Rogue.FastLane.Items;

namespace Rogue.FastLane.Queries.Dispatchers
{
    public abstract class SimpleDispatcher<TItem>
    { 
        protected IQuery<TItem>[] Queries;

        public SimpleDispatcher(params IQuery<TItem>[] queries)
        {
            Queries = queries;
        }

        protected abstract void DeriveANewState(OptmizedStructure<TItem> @struct);

        protected abstract void TryChangeQueryValueCount();

        protected abstract void TryChangeQueryLevelCount();

        public virtual void ChangeStructures4Queries(OptmizedStructure<TItem> @struct)
        {
            DeriveANewState(@struct);

            TryChangeQueryLevelCount();

            TryChangeQueryValueCount();
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