using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rogue.FastLane.Queries;
using Rogue.FastLane.Collections;
using System.Diagnostics;

namespace Rogue.FastLane.Tests.Performance
{
    public abstract class PerformanceTests : BaseTest
    {
        protected Stopwatch Watch;
        
        [SetUp]
        public virtual void Setup()
        {
            RightIndex = 0;
            Watch = new Stopwatch();
            Query =
                new UniqueKeyQuery<MockItem, int>()
                {
                    SelectKey = item => item.Index
                };

            Collection =
                new OptimizedCollection<MockItem>(Query);
        }

        protected void TestInsertionAgainst<T>(int qtd, T list = default(T), Action<T, MockItem> add = null)
            where T : new()
        {
            if (list == null) { list = new T(); }

            Watch.Reset();
            for (int i = 1; i < qtd; i++)
            {
                Watch.Stop();
                var item = 
                    new MockItem() { Index = i, IndexInBytes = BitConverter.GetBytes(i) };
                Watch.Start();
                add(list, item);
            }
            Watch.Stop();

            var elapsed4List = Watch.Elapsed;

            Watch.Reset();
            
            for (int i = 0; i < qtd; i++)
            {
                var mock = new MockItem() { Index = i, IndexInBytes = BitConverter.GetBytes(i) };
                Watch.Stop();

                Collection.Add(mock);

                Watch.Start();
            }
            Watch.Stop();

            var elapsed4Collection = Watch.Elapsed;

            Console.WriteLine("For the list of type {0} took \n{1} to insert {2} items, for FastLane took {3}.",
                typeof(T), elapsed4List, qtd, elapsed4Collection);
        }

        protected void TestInsertionAgainstList(double qtd, List<MockItem> list = null)
        {
            TestInsertionAgainst<List<MockItem>>((int)qtd,
            list: list,                
            add: (l, item) => {
                var index = l.BinarySearch(item);
                l.Insert(~index, item);
                //l.Sort();
            });
        }

        protected void TestInsertionAgainstSortedList(double qtd, SortedList<int,MockItem> list = null)
        {
            //TestInsertionAgainst<SortedList<int, MockItem>>((int)qtd,
            //list: list,
            //add: (l, item) =>
            //    l.Add(i, item));
        }

        public abstract void TestAgainstListFor1089Items();
        public abstract void TestAgainstListFor35937Items();
        public abstract void TestAgainstListFor1185921Items();
        public abstract void TestAgainstSortedListFor1089Items();
        public abstract void TestAgainstSortedListFor35937Items();
        public abstract void TestAgainstSortedListFor1185921Items();
    }
}