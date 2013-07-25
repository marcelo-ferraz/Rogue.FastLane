using System;
using System.Diagnostics;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries;
using Rogue.FastLane.Queries.States;
using Rogue.FastLane.Infrastructure.Positioning;

namespace Nhonho
{
    class Program2
    {
        public class Query<TKey> :
            UniqueKeyQuery<Coordinates, TKey>, IUniqueKeyQuery<Coordinates>
        {
            public new UniqueKeyQueryState State
            {
                get { return base.State; }
                set { base.State = value; }
            }
        }

        #region
        static ReferenceNode<Coordinates, int> GetRef(int key, int count = 5, ReferenceNode<Coordinates, int> parent = null)
        {
            return new ReferenceNode<Coordinates, int>() { Key = key, References = new ReferenceNode<Coordinates, int>[count], Parent = parent };
        }

        static ReferenceNode<Coordinates, int> GetRefVal(int key, int count = 5, ReferenceNode<Coordinates, int> parent = null)
        {
            return new ReferenceNode<Coordinates, int>() { Key = key, Values = new ValueNode<Coordinates>[count], Parent = parent };
        }

        static ValueNode<Coordinates> GetVal(int key)
        {
            return new ValueNode<Coordinates>() { Value = new Coordinates() { Index = key } };
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
                new OptmizedStructure<Coordinates>(query);
 
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 1; i < (int)(Math.Pow(33,2)); i++)
            {
                structure.Add(new Coordinates() { Index = i });
                //Write(i, query);
            }
            watch.Stop();
            watch.Reset();
            watch.Start();
            structure.Add(new Coordinates() { Index = 0 });
            structure.Add(new Coordinates() { Index = (int)(Math.Pow(33, 2)) });
            watch.Stop();
            watch.Reset();
            watch.Start();
            var n = query.First(5);
            watch.Stop();

            var enu = 
                new LowestReferencesEnumerable<Coordinates, int>();

            foreach (var @ref in enu.AllFrom(query.Root))
            {
                Write(@ref.Values);
            }

            query.Key = 5;
            structure.Remove<int>(query);
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

        static int _rightIndex = 0;

        private static void Write(ValueNode<Coordinates>[] values)
        {
            Console.WriteLine("contains {0} items, which is {1}.", values.Length, values.Length < 34 ? "right" : "wrong");
            
            if(values.Length > 33) { System.Diagnostics.Debugger.Break(); }

            foreach (var val in values)
            {
                var comp = 
                    _rightIndex.CompareTo(val.Value.Index);
                
                Console.Write("key: {0} ", val.Value.Index);

                Console.Write(comp == 0 ? "is right" : string.Concat("is wrong, should be", _rightIndex));
                
                if (comp != 0) { System.Diagnostics.Debugger.Break(); }

                Console.WriteLine();
                _rightIndex++;
            }
        }

        static void Write(params ReferenceNode<Coordinates, int>[] refs)
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