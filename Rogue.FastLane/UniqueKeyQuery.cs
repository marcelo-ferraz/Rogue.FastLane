using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries;

using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Strategies.Query;

namespace Rogue.FastLane
{
    public class UniqueKeyQuery<TItem, TKey> : SimpleQuery<TItem, TKey>
    {
        public override void AfterAdd(ValueNode<TItem> node, UniqueKeyQueryState state)
        {
            QueryStrategies.AugmentValueCount(Root, state, 1);
            
            var key = 
                SelectKey(node.Value);

            int[] indexes = null;

            var lowestRef =
                FirstReference(key, Root, ref indexes);

            var valueIndex = lowestRef.Values.BinarySearch(
                val =>
                    CompareKeys(key, SelectKey(val.Value)));


        }

        public override void AfterRemove(ValueNode<TItem> item, UniqueKeyQueryState state)
        {
            throw new NotImplementedException();
        }
    }
}