using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Collections.State
{
    public class UniqueKeyQueryState
    {
        public int Length { get; set; }
        public int PercentageUsed { get; set; }
        public int MaxLenghtPerSegment { get; set; }
        public int MaxNumberOfIteractions { get; set; }
        
        public Pair[] Levels { get; set; }

        public UniqueKeyQueryState PassOn()
        {
            var newState = 
                new UniqueKeyQueryState
                {
                    Length = Length,
                    PercentageUsed = PercentageUsed,
                    MaxLenghtPerSegment = MaxLenghtPerSegment,
                    Levels = new Pair[Levels.Length]
                };
            
            for(int i =0; i < Levels.Length; i++)
            {
                newState.Levels[i] = Levels[i].PassOn();
            }
            return newState;
        }
    }
}
