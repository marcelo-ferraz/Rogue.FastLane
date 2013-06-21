using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue.FastLane.Items;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Queries.States;

namespace Rogue.FastLane.Queries
{
    public interface IUniqueKeyQuery<TItem> : IQuery<TItem> 
    {
        UniqueKeyQueryState State { get; set; }
    }
}
