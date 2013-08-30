using Rogue.FastLane.Items;
using System.Collections.Generic;

namespace Rogue._fastlane2.FastLane.Queries
{
    public interface IQuery<TItem>: IEnumerable<ValueHolder<TItem>>
    {
        string Name { get; set; }
    }
}
