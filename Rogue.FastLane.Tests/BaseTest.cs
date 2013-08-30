using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Infrastructure.Positioning;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries;
using NUnit.Framework;
using Rogue.FastLane.Collections.Items.Mixins;
using Rogue.FastLane.Collections;

namespace Rogue.FastLane.Tests
{
    public class BaseTest
    {
        public class MockItem : IComparable<MockItem>
        {
            public int Index { get; set; }
            public float Holder0 { get; set; }
            public float Holder1 { get; set; }
            public float Holder2 { get; set; }
            public float Holder3 { get; set; }
            public float Holder4 { get; set; }
            public float Holder5 { get; set; }
            public float Holder6 { get; set; }
            public byte[] IndexInBytes { get; set; }

            public int CompareTo(MockItem other)
            {
                return this.Index.CompareTo(other.Index);
            }
        }

        public OptimizedCollection<MockItem> Collection { get; set; }
        public UniqueKeyQuery<MockItem, int> Query { get; set; }

        protected int RightIndex = 0;

        protected void ValidateOrder(ReferenceNode<MockItem, int> node, Action<ValueHolder<MockItem>, int> expectedException = null)
        {
            var root = node.Root();
            
            var enu =
                new LowRefsEnumerable<MockItem, int>();

            foreach (var @ref in enu.AllFrom(root))
            {
                ValidateOrder(@ref.Values, expectedException);
            }
        }

        protected void ValidateOrder(ValueHolder<MockItem>[] values, Action<ValueHolder<MockItem>, int> expectedException = null)
        {
            //Assert.Less(values.Length, 130);
            Assert.Less(values.Length, 1090);            

            foreach (var val in values)
            {
                if (expectedException != null) { expectedException(val, RightIndex); }
                
                Assert.AreEqual(RightIndex, val.Value.Index, "The values in are wrong order");
                
                RightIndex++;
            }
        }
    }
}
