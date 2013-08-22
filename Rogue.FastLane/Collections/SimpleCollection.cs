using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Config;
using Rogue.FastLane.Infrastructure.Mixins;
using Rogue.FastLane.Queries;
using Rogue.FastLane.Queries.Dispatchers;

namespace Rogue.FastLane.Collections
{
    public abstract class SimpleCollection<TItem> 
    {
        public SimpleCollection(params IQuery<TItem>[] queries)
        {
            Dispatchers =
                new IDispatcher<TItem>[0];
            Queries = new List<IQuery<TItem>>();
            Add2Dispatchers(queries);
        }

        private int _dispatcherInsertionIndex;

        protected IDispatcher<TItem>[] Dispatchers;

        protected internal IList<IQuery<TItem>> Queries { get; set; }

        protected virtual TQuery Where<TQuery>(Func<IQuery<TItem>, bool> predicate)
            where TQuery: class, IQuery<TItem> 
        {
            for (int i = 0; i < Queries.Count; i++)
            {
                var query = Queries[i];
                if (predicate(query))
                {
                    return query as TQuery;
                }
            }
            return null;
        }

        protected virtual bool Is4UniqueKey(IQuery<TItem> query)
        {
            return query.IsOrInherits(typeof(UniqueKeyQuery<,>));
        }

        protected virtual bool Is4UniqueKey(IDispatcher<TItem> dispatcher)
        {
            return typeof(UniqueKeyDispatcher<TItem>).IsAssignableFrom(dispatcher.GetType());
        }

        protected virtual void Add2Dispatchers(IQuery<TItem>[] queries)
        {
            IDispatcher<TItem> dispatcher = null;
            for (int i = 0; i < queries.Length; i++)
            {
                if (Is4UniqueKey(queries[i]))
                {
                    dispatcher = Dispatchers.
                        FirstOrDefault(disp => Is4UniqueKey(disp));
                    
                    if (dispatcher == null)
                    {
                        if (Dispatchers.Length <= _dispatcherInsertionIndex)
                        {
                            Dispatchers = 
                                Dispatchers.Resize(Dispatchers.Length + 1);

                            Dispatchers[_dispatcherInsertionIndex] =
                                (dispatcher = Configuration<TItem>.Dispatch.GetDispatcher4UniqueKeyQuery());
                            _dispatcherInsertionIndex++;
                        }
                    }
                }
                else { throw new NotImplementedException("Any query other than Unique key is yet to be supported. Sorry."); }

                dispatcher.Add(queries[i]);

                Queries.Add(queries[i]);
            }
        }

        public virtual IQuery<TItem> Using(string name)
        {
            return Where<IQuery<TItem>>(q =>
                name.Equals(q.Name, StringComparison.OrdinalIgnoreCase));
        }

        public virtual TQuery Using<TQuery>()
            where TQuery: class, IQuery<TItem>
        {
            return Where<TQuery>(
                q => q.IsOrInherits<TQuery>());
        }
    }
}