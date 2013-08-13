using System.Reflection;
using NUnit.Framework;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Infrastructure.Positioning;
using Rogue.FastLane.Queries;
using System;

namespace Rogue.FastLane.Tests.Crud
{
    [TestFixture]
    public class InsertionTests : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            RightIndex = 0;

            Query =
                new UniqueKeyQuery<MockItem, int>()
                {
                    SelectKey = item => item.Index
                };

            Collection =
                new OptimizedCollection<MockItem>(Query);
        }

        [Test]
        public void SimpleInsertionTest()
        {
            Collection.Add(new MockItem() { Index = 3 });
            Collection.Add(new MockItem() { Index = 1 });
            Collection.Add(new MockItem() { Index = 2 });

            Assert.AreEqual(3, Query.Root.Key);
        }

        [Test]
        public void GrandInsertionTest()
        {
            for (int i = 1; i < (int)(Math.Pow(33, 2)); i++)
            { Collection.Add(new MockItem() { Index = i }); }

            Collection.Add(new MockItem() { Index = 0 });

            var n = Query.Get(5);

            Assert.NotNull(n);
            Assert.AreEqual(n.Value.Index, 5);

            ValidateOrder(Query.Root);
        }
    }
}