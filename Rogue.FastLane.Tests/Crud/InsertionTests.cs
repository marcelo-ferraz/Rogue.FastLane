using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Queries;
using System.Reflection;
using Rogue.FastLane.Collections.Items;

namespace Rogue.FastLane.Tests.Crud
{
    [TestFixture]
    public class InsertionTests: BaseNodeTest
    {
        private OptmizedStructure<Pair> _structure;
        private UniqueKeyQuery<Pair, int> _query;

        [SetUp]
        public void Setup()
        {
            _query =
                new UniqueKeyQuery<Pair, int>()
                {
                    SelectKey = item => item.Index
                };

            _structure =
                new OptmizedStructure<Pair>(_query);
        }

        public void SimpleInsertionTest()
        {
            _structure.Add(new Pair() { Index = 3 });
            _structure.Add(new Pair() { Index = 1 });
            _structure.Add(new Pair() { Index = 2 });

            var root = (ReferenceNode<Pair, int>)
                typeof(UniqueKeyQuery<,>)
                .MakeGenericType(typeof(Pair), typeof(int))
                .GetProperty("Root", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetGetMethod(true)
                .Invoke(_query, null);

            Assert.AreEqual(3, root.Key);
        }
    }
}
