using System.Collections.Generic;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Infrastructure.Positioning;

namespace Rogue.FastLane.Queries.Mixins
{
    public static class NodeMovingMixins
    {
        public static Stack<ReferenceNode<TItem, TKey>> MoveAll2TheRight<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, Coordinates[] coordinateSet)
        {
            ReferenceNode<TItem, TKey> previousRef = null;

            return self.ForEachValuedNodeReverse(coordinateSet,
                (@ref, i) =>
                {
                    if (i < 1)
                    { previousRef = @ref; }
                    else {
                        if (previousRef != null)
                        {
                            previousRef.Values[0] = @ref.Values[@ref.Values.Length - 1];
                            previousRef = null;                            
                        }
                        
                        @ref.Values[i] = @ref.Values[i - 1]; 
                    }
                });
        }

        public static Stack<ReferenceNode<TItem, TKey>> MoveAll2TheLeft<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, Coordinates[] coordinateSet)
        {
            ReferenceNode<TItem, TKey> previousRef = null;
            
            return self.ForEachValuedNode(coordinateSet,
                (@ref, i) =>
                {
                    if (@ref.Key.Equals(1089) || (@ref.Key.Equals(1088) && i == 32))
                    {
                        System.Diagnostics.Debugger.Break();
                    }

                    if (i == (@ref.Length - 1))
                    { previousRef = @ref; }
                    else
                    {
                        if (previousRef != null)
                        {
                            previousRef.Values[previousRef.Length - 1] = @ref.Values[0];
                            previousRef = null;
                        }

                        @ref.Values[i] = @ref.Values[i + 1];
                    }
                });
        }
    }
}