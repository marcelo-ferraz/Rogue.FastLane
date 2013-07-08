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
        public static void MoveAllAside<TItem, TKey>(this UniqueKeyQuery<TItem, TKey> self, Coordinates[] coordinateSet, ValueNode<TItem> valNode)
        {
            ReferenceNode<TItem, TKey> previousRef = null;

            self.ForEachValuedNode(coordinateSet,
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
    }
}