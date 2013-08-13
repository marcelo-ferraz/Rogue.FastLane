using System;
using NUnit.Framework;

namespace Rogue.FastLane.Tests.Performance
{
    [TestFixture]
    public class PerformanceInsertionTests : PerformanceTests
    {
        [Test]
        public override void TestAgainstListFor1089Items()
        {
            TestInsertionAgainstList(Math.Pow(33, 2));
        }

        [Test]
        public override void TestAgainstListFor35937Items()
        {
            TestInsertionAgainstList(Math.Pow(33, 2));
        }

        [Test]
        public override void TestAgainstListFor1185921Items()
        {
            TestInsertionAgainstList(Math.Pow(33, 2));
        }

        [Test]
        public override void TestAgainstSortedListFor1089Items()
        {
            TestInsertionAgainstSortedList(Math.Pow(33, 2));
        }

        [Test]
        public override void TestAgainstSortedListFor35937Items()
        {
            TestInsertionAgainstSortedList(Math.Pow(33, 2));
        }

        [Test]
        public override void TestAgainstSortedListFor1185921Items()
        {
            TestInsertionAgainstSortedList(Math.Pow(33, 2));
        }
    }
}
