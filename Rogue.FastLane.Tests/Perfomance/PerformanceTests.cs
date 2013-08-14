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

        protected void TestInsertionAgainst<T>(int qtd, T list = default(T), Action<T, int> add = null)
            where T : new()
        {
            if (list == null) { list = new T(); }

            Watch.Reset();
            Watch.Start();
            for (int i = 1; i < qtd; i++)
            { 
                add(list, i);
            }
            Watch.Stop();

            var elapsed4List = Watch.Elapsed;

            Watch.Reset();
            Watch.Start();
            for (int i = 1; i < qtd; i++)
            { Collection.Add(new MockItem() { Index = i, IndexInBytes = BitConverter.GetBytes(i) }); }
            Watch.Stop();

            var elapsed4Collection = Watch.Elapsed;

            Console.WriteLine("For the list of type {0} took \n{1} to insert {2} items, for FastLane took {3}.",
                typeof(T), elapsed4List, qtd, elapsed4Collection);
        }

        protected void TestInsertionAgainstList(double qtd, List<MockItem> list = null)
        {
            TestInsertionAgainst<List<MockItem>>((int)qtd,
            list: list,                
            add: (l, i) => {
                l.Add(new MockItem() { Index = i, IndexInBytes = BitConverter.GetBytes(i) });
                l.Sort();
            });
        }

        protected void TestInsertionAgainstSortedList(double qtd, SortedList<int,MockItem> list = null)
        {
            TestInsertionAgainst<SortedList<int, MockItem>>((int)qtd,
            list: list,
            add: (l, i) =>
                l.Add(i, new MockItem() { Index = i, IndexInBytes = BitConverter.GetBytes(i) }));
        }

        public abstract void TestAgainstListFor1089Items();
        public abstract void TestAgainstListFor35937Items();
        public abstract void TestAgainstListFor1185921Items();
        public abstract void TestAgainstSortedListFor1089Items();
        public abstract void TestAgainstSortedListFor35937Items();
        public abstract void TestAgainstSortedListFor1185921Items();
    }
}