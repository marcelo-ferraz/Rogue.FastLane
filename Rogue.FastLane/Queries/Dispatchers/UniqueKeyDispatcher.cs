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
                StructCalculus.Calculate4UniqueKey(@struct.Count, MaxComparisons);
        }

        protected override void ApplyToEach(ValueNode<TItem> item)
        {
            CurrentQuery.State = NewState;
            base.ApplyToEach(item);
        }

        protected override void TryChangeQueryLevelCount()
        {
            if (State.LevelCount < NewState.LevelCount)
            {
                CurrentQuery.AugmentQueryLevelCount(NewState.LevelCount - State.LevelCount); 
            }
            else if (State.LevelCount > NewState.LevelCount)
            {
                CurrentQuery.AbridgeQueryLevelCount(NewState.LevelCount - State.LevelCount); 
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
                CurrentQuery.AbridgeQueryValueCount(NewState.Last.TotalUsed - State.Last.TotalUsed); 
            }
        }

        protected override void SaveState()
        {
            this.State = this.NewState;
        }
    }
}
