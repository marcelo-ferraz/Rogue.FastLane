using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Tests.Crud;

namespace Rogue.FastLane.Tests
{
    public static class Viewer
    {
        static void Main(string[] args)
        {
            var test = new Performance.MassiveSearchingPerformanceTests();
            test.Setup();
            test.TestAgainstListFor1089Items();
        }
    }
}
