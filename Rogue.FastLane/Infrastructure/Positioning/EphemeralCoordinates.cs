using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Infrastructure.Primitives;

namespace Rogue.FastLane.Infrastructure.Positioning
{
    public class EphemeralCoordinates: Coordinates
    {
        public EphemeralCoordinates() : base() 
        {
            _index = 
                new OneTimeValue<int>();
            //_overallIndex = 
            //    new OneTimeValue<int>();

            this.Index = 0;
            //this.OverallIndex = 0;
        }

        private OneTimeValue<int> _index;
        
        //private OneTimeValue<int> _overallIndex;  

        public override int Index 
        {
            get { return _index.Value; }
            set { _index.Value = value; } 
        }

        //public override int OverallIndex
        //{
        //    get { return _overallIndex.Value; }
        //    set { _overallIndex.Value = value; } 
        //}
    }
}