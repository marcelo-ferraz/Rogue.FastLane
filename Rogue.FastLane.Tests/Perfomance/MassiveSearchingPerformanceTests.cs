using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Queries;

namespace Rogue.FastLane.Tests.Performance
{
    [TestFixture]
    public class MassiveSearchingPerformanceTests : PerformanceTests
    {
        protected void CompareSearch(SortedList<int, MockItem> list, OptimizedCollection<MockItem> collection)
        {
            var max = list.Count;
            
            Watch.Reset();
            int i = 0;
            Watch.Start();            
            try
            {
                for (i = 1; i < max; i++)
                {
                    var mockedInList = list[i];
                }
            }
            catch
            { 
                Console.WriteLine("index " + i);
            }
            Watch.Stop();

            var elapsed4List = Watch.Elapsed;

            Watch.Reset();
            
            var query = 
                collection.Using<UniqueKeyQuery<MockItem, int>>();
                       
            Watch.Start();
            
            for (i = 0; i < max; i++)
            {
                var mockedInCollection =
                    query.Get(i);
            }
            Watch.Stop();

            var elapsed4Collection = Watch.Elapsed;

            Console.WriteLine("For the SortedList<int, T>, took {0} to get one item, for FastLane took {1}.",
                elapsed4List, elapsed4Collection);
        }

        protected void CompareSearch(List<MockItem> list, OptimizedCollection<MockItem> collection)
        {
            var max = list.Count;
            Watch.Reset();

            var query =
                collection.Using<UniqueKeyQuery<MockItem, int>>();
            Watch.Start();
            
            for (int i = 0; i < max; i++)
            {
                var mockedInCollection = query.Get(i);
            }
            Watch.Stop();

            var elapsed4Collection = Watch.Elapsed;

            Watch.Reset();
            Watch.Start();
            for (int i = 0; i < max; i++)
            {
                var mockedInList = list.BinarySearch(new MockItem { Index = i });
            }
            Watch.Stop();

            var elapsed4List = Watch.Elapsed;

            //Watch.Reset();
            //Watch.Start();
            //for (int i = 0; i < max; i++)
            //{
            //    var mockedInList = list.FirstOrDefault(item => item.Index == i);
            //}
            //Watch.Stop();

            var elapsed4ListLinq = Watch.Elapsed;
            
            Console.WriteLine("For the list<T>, took {0} to get all items, for FastLane took {1}.",
                elapsed4List, elapsed4Collection);

            //Console.WriteLine("For the list<T> with linq, took {0} to get all items, for FastLane took {1}.",
            //    elapsed4ListLinq, elapsed4Collection);
        }

        [Test]
        public override void TestAgainstListFor1089Items()
        {
            var list = new List<MockItem>();
            base.TestInsertionAgainstList(Math.Pow(33, 2), list);
            base.ValidateOrder(Query.Root);
            CompareSearch(list, Collection);
        }

        [Test]
        public override void TestAgainstListFor35937Items()
        {
            var list = new List<MockItem>();
            base.TestInsertionAgainstList(Math.Pow(33, 3), list);
            CompareSearch(list, Collection);
        }

        public void TestAgainstListForNItems(int qtd)
        {
            var list = new List<MockItem>();
            base.TestInsertionAgainstList(qtd, list);
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