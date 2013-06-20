using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Queries.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Infrastructure;

namespace Rogue.FastLane.Queries.Dispatchers
{
    public class UniqueKeyDispatcher<TItem> : SimpleDispatcher<TItem>
    {
        public UniqueKeyDispatcher(params IQuery<TItem>[] queries)
            : base(queries) 
        {
            MaxComparisons = 10;
            State = new UniqueKeyQueryState
            {
                LevelCount = 1
            };
        }

        protected UniqueKeyQueryState State;
        protected UniqueKeyQueryState NewState;
        
        public int MaxComparisons { get; set; }

        protected override void DeriveNewState(OptmizedStructure<TItem> @struct)
        {
            NewState =
                StructCalculus.Calculate4UniqueKey(@struct.Count + 1, MaxComparisons);            
        }

        protected override void TryChangeQueryLevelCount()
        {
            if (State.LevelCount < NewState.LevelCount)
            {
                foreach (var query in Queries)
                { query.AugmentQueryLevelCount(NewState.LevelCount - State.LevelCount); }
            }
            else if (State.LevelCount > NewState.LevelCount)
            {
                foreach (var query in Queries)
                { query.AbridgeQueryLevelCount(NewState.LevelCount - State.LevelCount); }
            }
        }

        protected override void TryChangeQueryValueCount()
        {
            if (State.Last == null || State.Last.TotalUsed < NewState.Last.TotalUsed)
            {
                foreach (var query in Queries)
                { query.AugmentQueryValueCount(NewState.Last.TotalUsed); }
            }
            else if (State.Last == null || State.Last.TotalUsed > NewState.Last.TotalUsed)
            {
                foreach (var query in Queries)
                { query.AbridgeQueryValueCount(NewState.Last.TotalUsed); }
            }
        }

        protected override void SaveState()
        {
            this.State = this.NewState;
        }
    }
}
