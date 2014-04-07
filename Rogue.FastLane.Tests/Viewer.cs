using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Tests.Crud;
using System.Diagnostics;

namespace Rogue.FastLane.Tests
{
    public static class Viewer
    {
        static void Main(string[] args)
        {
            var test = new Performance.MassiveSearchingPerformanceTests();
            test.Setup();
            //test.TestAgainstListFor1089Items();
            //test.TestAgainstListFor35937Items();
            //test.TestAgainstListFor1185921Items();
            test.TestAgainstListForNItems(4198400);//(int)Math.Pow(2,24));

            var w = new Stopwatch();

            int rdn = new Random(new Random().Next(1089)).Next(1089);

            w.Start();
            test.Query.Get(rdn);
            w.Stop();

            var li = new List<Rogue.FastLane.Tests.BaseTest.MockItem>();
            for (int i = 0; i < 4198401; i++)
            {
                li.Add(new BaseTest.MockItem() { Index = i });
            }

            w.Restart();
            li.BinarySearch(new BaseTest.MockItem() { Index = rdn });
            w.Stop();
            
            Console.WriteLine("Seaching for index {0} took {1}", rdn, w.Elapsed); 
        }
    }
}
