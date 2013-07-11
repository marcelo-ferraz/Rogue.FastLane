using System.Diagnostics;
using Rogue.FastLane;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Queries;
using System.Reflection;
using Rogue.FastLane.Queries.States;
using System;

namespace Nhonho
{
    class Program2
    {
        public class Query<TKey> :
            UniqueKeyQuery<Pair, TKey>, IUniqueKeyQuery<Pair>
        {
            public new UniqueKeyQueryState State
            {
                get { return base.State; }
                set { base.State = value; }
            }
        }

        #region
        static ReferenceNode<Pair, int> GetRef(int key, int count = 5, ReferenceNode<Pair, int> parent = null)
        {
            return new ReferenceNode<Pair, int>() { Key = key, References = new ReferenceNode<Pair, int>[count], Parent = parent };
        }

        static ReferenceNode<Pair, int> GetRefVal(int key, int count = 5, ReferenceNode<Pair, int> parent = null)
        {
            return new ReferenceNode<Pair, int>() { Key = key, Values = new ValueNode<Pair>[count], Parent = parent };
        }

        static ValueNode<Pair> GetVal(int key)
        {
            return new ValueNode<Pair>() { Value = new Pair() { Index = key } };
        }
        #endregion

        static void Main(string[] args)
        {
            var query =
                new Query<int>
                {
                    SelectKey = item => item.Index
                };

            var structure =
                new OptmizedStructure<Pair>(query);
            //count: 1
            //structure.Add(new Pair() { Index = 3 });
            //Write(3, query);//count: 2
            //structure.Add(new Pair() { Index = 1 });
            //Write(1, query);//count: 3
            //structure.Add(new Pair() { Index = 2 });
            //Write(2, query);//count: 4
            //structure.Add(new Pair() { Index = 4 });
            //Write(4, query);//count: 5
            //structure.Add(new Pair() { Index = 5 });
            //Write(5, query);//count: 6
            //structure.Add(new Pair() { Index = 7 });
            //Write(7, query);//count: 7
            //structure.Add(new Pair() { Index = 6 });
            //Write(6,query);//count: 8
            //structure.Add(new Pair() { Index = 0 });
            //Write(0, query);//count: 9
            //System.Diagnostics.Debugger.Break();
            //structure.Add(new Pair() { Index = 8 });
            //Write(8, query);//count: 10
            //structure.Add(new Pair() { Index = 10 });
            //Write(10,query);//count: 11
            //structure.Add(new Pair() { Index = 11 });
            //Write(11,query);//count: 12
            //structure.Add(new Pair() { Index = 9 });
            //Write(9, query);

            var watch = new Stopwatch();
            watch.Start();
            for (int i = 1; i < (int)(Math.Pow(33,3)); i++)
            {
                structure.Add(new Pair() { Index = i });
                //Write(i, query);
            }
            watch.Stop();
            watch.Reset();
            watch.Start();
            structure.Add(new Pair() { Index = 0 });
            watch.Stop();
            //<!-------------------------------------------->
            //System.Diagnostics.Debugger.Break(); 
            //structure.Add(new Pair() { Index = 21 });
            //Write(21, query);
            //<!-------------------------------------------->
            watch.Reset();
            watch.Start();
            var n = query.First(5);
            watch.Stop();
        }
        static int _count = 0;
        static void Write(int index, Query<int> query)
        {
            Console.WriteLine("==================================");
            Console.WriteLine("Inserted: {0}, count {1}", index, ++_count);
            if (query.Root.References != null)
            {
                Write(query.Root.References);
            }
            if (query.Root.Values != null)
            {
                Write(query.Root.Values);
            }
            Console.WriteLine("==================================");
        }

        private static void Write(ValueNode<Pair>[] values)
        {
            foreach (var val in values)
            { Console.WriteLine("key: {0}", val.Value.Index); }
        }

        static void Write(params ReferenceNode<Pair, int>[] refs)
        {
            if (refs != null)
            {
                foreach (var r in refs)
                {
                    Console.WriteLine("ref key: {0}", r.Key);
                    if (r.References != null)
                    {                        
                        Write(r.References); 
                    }
                    else 
                    {
                        Write(r.Values);
                    }
                }
            }
        }
    }
}