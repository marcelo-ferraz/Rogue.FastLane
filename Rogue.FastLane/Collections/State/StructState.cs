using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Collections.State
{
    public class StructState
    {
        public int Lenght { get; set; }
        public int PercentageUsed { get; set; }
        public int OptimumLenghtPerSegment { get; set; }
        public int MaxNumberOfIteractions { get; set; }
        
        public Pair[] Levels { get; set; }

        public StructState PassOn()
        {
            var newState = 
                new StructState
                {
                    Lenght = Lenght,
                    PercentageUsed = PercentageUsed,
                    OptimumLenghtPerSegment = OptimumLenghtPerSegment,
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
