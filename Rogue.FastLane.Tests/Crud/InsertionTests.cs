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
    public class InsertionTests: BaseNodeTest
    {
        private OptmizedStructure<Coordinates> _structure;
        private UniqueKeyQuery<Coordinates, int> _query;

        [SetUp]
        public void Setup()
        {
            RightIndex = 0;

            _query =
                new UniqueKeyQuery<Coordinates, int>()
                {
                    SelectKey = item => item.Index
                };

            _structure =
                new OptmizedStructure<Coordinates>(_query);
        }

        [Test]
        public void SimpleInsertionTest()
        {
            _structure.Add(new Coordinates() { Index = 3 });
            _structure.Add(new Coordinates() { Index = 1 });
            _structure.Add(new Coordinates() { Index = 2 });

            var root = (ReferenceNode<Coordinates, int>)
                typeof(UniqueKeyQuery<,>)
                .MakeGenericType(typeof(Coordinates), typeof(int))
                .GetProperty("Root", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetGetMethod(true)
                .Invoke(_query, null);

            Assert.AreEqual(3, root.Key);
        }

        [Test]
        public void GrandInsertionTest()
        {
            var query =
                new UniqueKeyQuery<Coordinates, int>
                {
                    SelectKey = item => item.Index
                };

            var structure =
                new OptmizedStructure<Coordinates>(query);

            for (int i = 1; i < (int)(Math.Pow(33, 2)); i++)
            {
                structure.Add(new Coordinates() { Index = i });
            }

            structure.Add(new Coordinates() { Index = 0 });

            var n = query.Get(5);

            var enu =
                new LowestReferencesEnumerable<Coordinates, int>();

            foreach (var @ref in enu.AllFrom(query.Root))
            {
                ValidateOrder(@ref.Values);
            }
        }
    }
}
