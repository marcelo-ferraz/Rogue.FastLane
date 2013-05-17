using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Collections.State
{
    public class Pair
    {
        public int Length { get; set; }
        public int Index { get; set; }

        public Pair PassOn()
        {
            return 
                new Pair
                {
                    Index = Index,
                    Length = Length
                };
        }
    }
}