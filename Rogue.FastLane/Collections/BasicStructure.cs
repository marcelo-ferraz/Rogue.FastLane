using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Queries;
using Rogue.FastLane.Queries.Dispatchers;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Config;
using Rogue.FastLane.Infrastructure.Mixins;

namespace Rogue.FastLane.Collections
{
    public abstract class BasicStructure<TItem>
    {
        public BasicStructure(params IQuery<TItem>[] queries)
        {
            Dispatchers =
                new SimpleDispatcher<TItem>[0];
            Add2Dispatchers(queries);
        }

        private int _dispatcherInsertionIndex;

        protected SimpleDispatcher<TItem>[] Dispatchers;

        protected virtual bool Is4UniqueKey(IQuery<TItem> query)
        {
            return query.IsOrInherits(typeof(UniqueKeyQuery<,>));
        }

        protected virtual bool Is4UniqueKey(SimpleDispatcher<TItem> dispatcher)
        {
            return typeof(UniqueKeyDispatcher<TItem>).IsAssignableFrom(dispatcher.GetType());
        }

        protected virtual void Add2Dispatchers(IQuery<TItem>[] queries)
        {
            SimpleDispatcher<TItem> dispatcher = null;
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
                                Dispatchers.Resize(Dispatchers.Length + 2);

                            Dispatchers[_dispatcherInsertionIndex] =
                                (dispatcher = Configuration<TItem>.Dispatch.GetDispatcher4UniqueKeyQuery());
                            _dispatcherInsertionIndex++;
                        }
                    }
                }
                else { throw new NotImplementedException("Any query other than Unique key is not supported"); }

                dispatcher.Add(queries[i]);
            }
        }
    }
}