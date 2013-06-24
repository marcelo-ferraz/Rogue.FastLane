using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Queries.States;
using Rogue.FastLane.Items;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Infrastructure;

namespace Rogue.FastLane.Queries.Dispatchers
{
    public class UniqueKeyDispatcher<TItem> : SimpleDispatcher<TItem, IUniqueKeyQuery<TItem>>
    {
        public UniqueKeyDispatcher(params IUniqueKeyQuery<TItem>[] queries)
            : base(queries) 
        {
            MaxComparisons = 4;
            State = new UniqueKeyQueryState();
        }

        protected UniqueKeyQueryState State;
        protected UniqueKeyQueryState NewState;
        
        public int MaxComparisons { get; set; }

        protected override void DeriveNewState(OptmizedStructure<TItem> @struct)
        {
            NewState =
                StructCalculus.Calculate4UniqueKey(@struct.Count + 1, MaxComparisons);
        }

        protected override void ApplyToEach(IUniqueKeyQuery<TItem> query, ValueNode<TItem> item)
        {
            query.State = NewState;
            base.ApplyToEach(query, item);
        }

        protected override void TryChangeQueryLevelCount(IUniqueKeyQuery<TItem> query)
        {
            if (State.LevelCount < NewState.LevelCount)
            {
                query.AugmentQueryLevelCount(NewState.LevelCount - State.LevelCount); 
            }
            else if (State.LevelCount > NewState.LevelCount)
            {
                query.AbridgeQueryLevelCount(NewState.LevelCount - State.LevelCount); 
            }
        }

        protected override void TryChangeQueryValueCount(IUniqueKeyQuery<TItem> query)
        {
            if (State.Last == null || State.Last.TotalUsed < NewState.Last.TotalUsed)
            {
                query.AugmentQueryValueCount(NewState.Last.TotalUsed - State.Last.TotalUsed);                
            }
            else if (State.Last == null || State.Last.TotalUsed > NewState.Last.TotalUsed)
            {
                query.AbridgeQueryValueCount(NewState.Last.TotalUsed - State.Last.TotalUsed); 
            }
        }

        protected override void SaveState()
        {
            this.State = this.NewState;
        }
    }
}
