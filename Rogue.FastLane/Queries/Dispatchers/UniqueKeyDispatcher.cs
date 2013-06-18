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
        }

        protected UniqueKeyQueryState OldState;
        protected UniqueKeyQueryState NewState;
        
        public int MaxComparisons { get; set; }

        protected override void DeriveANewState(OptmizedStructure<TItem> @struct)
        {
            var newState =
                StructCalculus.Calculate4UniqueKey(@struct.Count + 1, MaxComparisons);
        }

        protected override void TryChangeQueryValueCount()
        {
            if (OldState.Last.TotalUsed < NewState.Last.TotalUsed)
            {
                foreach (var query in Queries)
                { query.AugmentQueryValueCount(NewState); }
            }
            else if (OldState.Last.TotalUsed > NewState.Last.TotalUsed)
            {
                foreach (var query in Queries)
                { query.AbridgeQueryValueCount(NewState); }
            }
        }

        protected override void TryChangeQueryLevelCount()
        {
            if (OldState.LevelCount < NewState.LevelCount)
            {
                foreach (var query in Queries)
                { query.AugmentQueryLevelCount(NewState); }
            }
            else if (OldState.LevelCount > NewState.LevelCount)
            {
                foreach (var query in Queries)
                { query.AbridgeQueryLevelCount(NewState); }
            }
        }
    }
}
