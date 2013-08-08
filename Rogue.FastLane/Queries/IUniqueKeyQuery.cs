using Rogue.FastLane.Queries.States;

namespace Rogue.FastLane.Queries
{
    public interface IUniqueKeyQuery<TItem> : IQuery<TItem>, ICrudQuery<TItem>
    {
        UniqueKeyQueryState State { get; set; }
    }
}
