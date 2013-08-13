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
    public class InsertNDeleteTests : BaseTest
    {
        [Test]
        public void GrandInsertionNDeleteTest()
        {
            var insertionTest =
                new InsertionTests();

            insertionTest.Setup();
            insertionTest.GrandInsertionTest();

            Query = insertionTest.Query;
            
            Query.Key = 5;
            
            insertionTest.Collection.Remove<int>(Query);

            ValidateOrder(Query.Root, (item, index) =>
            {
                //if the index is 5, the one that was deleted, correct the index.
                if (index == 5) { RightIndex++; }
            });
        }
    }
}
