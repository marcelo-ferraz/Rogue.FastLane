using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Queries.Mixins
{
    public static class NodeMixins
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
                    if (i == @ref.Length -1)
                    { previousRef = @ref; }
                    else
                    {
                        if (previousRef != null)
                        {
                            previousRef.Values
                                [previousRef.Length] = @ref.Values[0];
                            previousRef = null;
                        }

                        @ref.Values[i] = @ref.Values[i + 1];
                    }
                });
        }
    }
}