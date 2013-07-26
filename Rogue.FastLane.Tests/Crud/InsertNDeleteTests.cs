using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rogue.FastLane.Queries;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Infrastructure.Positioning;

namespace Rogue.FastLane.Tests.Crud
{
    public class InsertNDeleteTests : BaseNodeTest
    {
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

            ValidateOrder(query.Root);
        }
    }
}
