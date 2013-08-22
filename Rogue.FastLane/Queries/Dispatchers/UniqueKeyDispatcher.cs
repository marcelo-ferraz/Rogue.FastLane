using Rogue.FastLane.Collections;
using Rogue.FastLane.Infrastructure;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.States;

namespace Rogue.FastLane.Queries.Dispatchers
{
    public class UniqueKeyDispatcher<TItem> : SimpleDispatcher<TItem, IUniqueKeyQuery<TItem>>
    {
        public UniqueKeyDispatcher(params IUniqueKeyQuery<TItem>[] queries)
            : base(queries) 
        {
            MaxComparisons = 14;
            State = new UniqueKeyQueryState();
        }

        protected UniqueKeyQueryState State;
        protected UniqueKeyQueryState NewState;
        
        public int MaxComparisons { get; set; }

        protected override void DeriveNewState(OptimizedCollection<TItem> @struct)
        {
            NewState =
                UniqueKeyQueryCalculus.Calculate4UniqueKey(@struct.Count, MaxComparisons);
        }

        protected override void RemoveInEach(IUniqueKeyQuery<TItem> query, ValueNode<TItem> item)
        {
            CurrentQuery = query;

            CurrentQuery.Remove(item);

            query.State = NewState;

            TryChangeQueryValueCount();

            TryChangeQueryLevelCount();

            SaveState();
        }

        protected override void AddInEach(IUniqueKeyQuery<TItem> query, ValueNode<TItem> item)
        {
            query.State = NewState;
            base.AddInEach(query, item);
        }

        protected override void TryChangeQueryLevelCount()
        {
            if (State.LevelCount < NewState.LevelCount)
            {
                CurrentQuery.AugmentQueryLevelCount(NewState.LevelCount - State.LevelCount); 
            }
            else if (State.LevelCount > NewState.LevelCount)
            {
                CurrentQuery.AbridgeQueryLevelCount(State.LevelCount - NewState.LevelCount); 
            }
        }

        protected override void TryChangeQueryValueCount()
        {
            if (State.Last == null || State.Last.TotalUsed < NewState.Last.TotalUsed)
            {
                CurrentQuery.AugmentQueryValueCount(NewState.Last.TotalUsed - State.Last.TotalUsed);                
            }
            else if (State.Last == null || State.Last.TotalUsed > NewState.Last.TotalUsed)
            {
                CurrentQuery.AbridgeQueryValueCount(State.Last.TotalUsed - NewState.Last.TotalUsed); 
            }
        }

        protected override void SaveState()
        {
            this.State = this.NewState;
        }
    }
}
