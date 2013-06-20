using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Config
{
    public static class Configuration<TItem>
    {
        static Configuration()
        {
            Dispatch = new QueryDispatchmentResolver<TItem>();
        }

        public static QueryDispatchmentResolver<TItem> Dispatch { get; set; }        
    }
}
