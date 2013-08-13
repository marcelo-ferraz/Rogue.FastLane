using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Infrastructure.Primitives;

namespace Rogue.FastLane.Infrastructure.Positioning
{
    public class EphemeralCoordinates : Coordinates
    {
        public EphemeralCoordinates()
            : base()
        {
            _index =
                new OneTimeValue<int>();

            this.Index = 0;
        }

        private OneTimeValue<int> _index;

        public override int Index
        {
            get { return _index.Value; }
            set { _index.Value = value; }
        }
    }
}