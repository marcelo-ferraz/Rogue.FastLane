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
            test.TestAgainstListFor35937Items();

            var w = new Stopwatch();
            w.Start();
            test.Query.Get(500);
            w.Stop();
        }
    }
}
