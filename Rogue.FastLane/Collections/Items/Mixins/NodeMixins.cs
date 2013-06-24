using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Collections.Mixins;

namespace Rogue.FastLane.Collections.Items.Mixins
{
    public static class NodeMixins
    {
        public static ReferenceNode<TItem, TKey> Next<TItem, TKey>(this ReferenceNode<TItem, TKey> self, Coordinates[] coordinateSet)
        {
            if (self.Parent == null)
            { throw new InvalidOperationException("Cannot move to a next node. There`s no parent"); }

            if (self.Parent.References == null)
            { throw new InvalidOperationException("Cannot move to a next node. Parent has no references."); }

            if (coordinateSet.Length < 2)
            { throw new InvalidOperationException("Cannot move to a next node. Coordinates are too shallow."); }

            var penultimateCoord =
                coordinateSet[coordinateSet.Length - 2];
            
            Func<ReferenceNode<TItem,TKey>[], ReferenceNode<TItem,TKey>> tryNext = 
                @refs =>
                    @refs[penultimateCoord.Index] ??
                    (@refs[penultimateCoord.Index] = new ReferenceNode<TItem, TKey>());

            if((++penultimateCoord.Index) < self.Parent.References.Length)
            { return tryNext(self.Parent.References); }
            else
            { return tryNext(self.Parent.References = self.Parent.References.Resize(penultimateCoord.Index + 1)); }
                            
        }
    }
}
