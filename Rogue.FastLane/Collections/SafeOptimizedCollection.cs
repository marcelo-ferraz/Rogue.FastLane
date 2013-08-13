using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Rogue.FastLane.Queries;

namespace Rogue.FastLane.Collections
{
    public class SafeOptimizedCollection<TItem> : OptimizedCollection<TItem>
    {
        public SafeOptimizedCollection(params IQuery<TItem>[] queries)
            : base(queries) { 

        }
        protected ReaderWriterLockSlim Lock;

        public override void Remove<TKey>(Queries.IQuery<TItem> selector)
        {
            try
            {
                if (Lock.TryEnterWriteLock(500))
                { base.Remove<TKey>(selector); }
            }
            finally 
            {
                if (Lock.IsWriteLockHeld)
                { Lock.ExitWriteLock(); }
            }
        }

        public override void Add(TItem item)
        {
            try
            {
                if (Lock.TryEnterWriteLock(500))
                { base.Add(item); }
            }
            finally
            {
                if (Lock.IsWriteLockHeld)
                { Lock.ExitWriteLock(); }
            }
        }

        protected override TQuery Where<TQuery>(Func<Queries.IQuery<TItem>, bool> predicate)
        {
            TQuery query = default(TQuery);
            try
            {
                if (Lock.TryEnterReadLock(500))
                { query = base.Where<TQuery>(predicate); }
            }
            finally
            {
                if (Lock.IsReadLockHeld)
                { Lock.ExitReadLock(); }
            }
            return query;   
        }

        
        public override int Count
        {
            get
            {
                int count;
                try
                {
                    if (Lock.TryEnterReadLock(500))
                    { count = base.Count; }
                }
                finally
                {
                    if (Lock.IsReadLockHeld)
                    { Lock.ExitReadLock(); }
                }
                return base.Count;
            }
            set
            {
                try
                {
                    if (Lock.TryEnterWriteLock(500))
                    { base.Count = value; }
                }
                finally
                {
                    if (Lock.IsWriteLockHeld)
                    { Lock.ExitWriteLock(); }
                }
            }
        }
    }
}
