using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Infrastructure.Positioning
{
    public class Coordinates
    {
        public Coordinates()
        {
            this.Length = 1;
            this.OverallLength = 1;
        }

        public virtual int Length { get; set; }

        public virtual int Index { get; set; }

        public virtual int OverallIndex { get; set; }

        public virtual int OverallLength { get; set; }
    }
}