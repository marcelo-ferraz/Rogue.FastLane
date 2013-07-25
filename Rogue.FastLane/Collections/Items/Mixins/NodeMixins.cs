using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Infrastructure.Positioning;

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

            return self.Parent.References[
                coordinateSet[coordinateSet.Length - 2].Index];                            
        }


        public static ReferenceNode<TItem, TKey> Root<TItem, TKey>(this ReferenceNode<TItem, TKey> node)
        {
            return node.Parent != null ? node.Parent.Root() : node;
        }
    }
}
