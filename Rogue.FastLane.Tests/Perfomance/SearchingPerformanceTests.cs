using System;
using NUnit.Framework;
using System.Collections.Generic;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Queries;

namespace Rogue.FastLane.Tests.Performance
{
    [TestFixture]
    public class SearchingPerformanceTests : PerformanceTests
    {
        protected void CompareSearch(List<MockItem> list, OptimizedCollection<MockItem> collection)
        {
            int key = new Random(new Random().Next(0, list.Count - 1)).Next(0, list.Count - 1);

            Watch.Reset();
            var query = 
                collection.Using<UniqueKeyQuery<MockItem, int>>();
            Watch.Start();

            var mockedInCollection = query.Get(key);
            Watch.Stop();

            var elapsed4Collection = Watch.Elapsed;

            Watch.Reset();
            Watch.Start();
            var mockedInList = list.BinarySearch(new MockItem { Index = key });
            Watch.Stop();

            var elapsed4List = Watch.Elapsed;


            Console.WriteLine("For the list<T>, took {0} to get one item, \n FastLane took {1}.",
                elapsed4List, elapsed4Collection);
        }

        protected void CompareSearch(SortedList<int, MockItem> list, OptimizedCollection<MockItem> collection)
        {
            int key = new Random(new Random().Next(0, list.Count - 1)).Next(0, list.Count - 1);

            Watch.Reset();
            Watch.Start();
            var mockedInList = list[key];
            Watch.Stop();

            var elapsed4List = Watch.Elapsed;

            Watch.Reset();
            Watch.Start();
            var mockedInCollection =
                collection.Using<UniqueKeyQuery<MockItem, int>>().Get(key);
            Watch.Stop();

            var elapsed4Collection = Watch.Elapsed;

            Console.WriteLine("For the SortedList<int, T>, took {0} to get one item, for FastLane took {1}.",
                elapsed4List, elapsed4Collection);
        }

        [Test]
        public override void TestAgainstListFor1089Items()
        {
            var list = new List<MockItem>();
            base.TestInsertionAgainstList(Math.Pow(33, 2), list);
            CompareSearch(list, Collection);
        }

        [Test]
        public override void TestAgainstListFor35937Items()
        {
            var list = new List<MockItem>();
            base.TestInsertionAgainstList(Math.Pow(33, 3), list);
            CompareSearch(list, Collection);
        }

        [Test]
        public override void TestAgainstListFor1185921Items()
        {
            var list = new List<MockItem>();
            base.TestInsertionAgainstList(Math.Pow(33, 4), list);
            CompareSearch(list, Collection);
        }

        [Test]
        public override void TestAgainstSortedListFor1089Items()
        {
            var list = new SortedList<int, MockItem>();
            base.TestInsertionAgainstSortedList(Math.Pow(33, 2), list);
            CompareSearch(list, Collection);
        }

        [Test]
        public override void TestAgainstSortedListFor35937Items()
        {
            var list = new SortedList<int, MockItem>();
            base.TestInsertionAgainstSortedList(Math.Pow(33, 3), list);
            CompareSearch(list, Collection);
        }

        [Test]
        public override void TestAgainstSortedListFor1185921Items()
        {
            var list = new SortedList<int, MockItem>();
            base.TestInsertionAgainstSortedList(Math.Pow(33, 4), list);
            CompareSearch(list, Collection);
        }

        
    }
}