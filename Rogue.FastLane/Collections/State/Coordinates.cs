using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Collections.State
{
    public class Coordinates :Pair
    {
        public Coordinates()
        {
            Length = 1;
            OverallLength = 1;
        }

        public int OverallIndex { get; set; }

        public int OverallLength { get; set; }
    }
}