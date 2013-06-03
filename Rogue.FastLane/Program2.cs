using System.Diagnostics;
using Rogue.FastLane;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Queries;
using Rogue.FastLane.Queries.Mixins;
using System.Reflection;

namespace Nhonho
{
    class Program2
    {
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
            TestReverseEnumerable();

            var query =
                new UniqueKeyQuery<Pair, int>() { 
                     SelectKey = item => item.Index
                };

            var structure = 
                new OptmizedStructure<Pair>(query);

            var root = (ReferenceNode<Pair, int>)
                typeof(UniqueKeyQuery<,>)
                .MakeGenericType(typeof(Pair), typeof(int))
                .GetProperty("Root", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetGetMethod(true)
                .Invoke(query, null);

            query.Root = root = GetRoot();

            var iterator =
                new LowestReferencesEnumerable<Pair, int>().AllBellow(query.Root);

            foreach (var inp in iterator)
            {
                foreach (var val in inp.Values)
                {
                    structure.Add(val.Value);
                }
            }

            query.State = structure.State;

            //typeof(OptmizedStructure<>)
            //    .MakeGenericType(typeof(Pair))
            //    .GetProperty("Root", BindingFlags.NonPublic | BindingFlags.Instance)
            //    .GetSetMethod(true)
            //    .Invoke(structure, new [] { root });

            query.Root = root;


            Coordinates[] coordinates = null;

            query.Key = 3;
            var first = query.FirstRefByUniqueKey(ref coordinates);

            structure.Add(new Pair() { Index = 3 });
            structure.Add(new Pair() { Index = 1 });
            structure.Add(new Pair() { Index = 2 });            
		}

        private static void TestReverseEnumerable()
        {

            var otherRoot = GetRoot();
            //root.References[3].Values[2] = GetVal(13);

            
            var @enum = new LowestReferencesReverseEnumerable<Pair, int>().
                //UpToHere(otherRoot, new[] { new Pair() { Length = 4, Index = 3 }, new Pair() { Length = 12, Index = 10 } });
                UpToHere(otherRoot, new[] { new Pair() { Length = 5, Index = 1 }, new Pair() { Length = 12, Index = 10 } });
                //AllBellow(otherRoot);


            foreach (var item in @enum)
            {
                System.Console.WriteLine("Id: {0}" , item.Key);
            }


            //var query = new UniqueKeyQuery<Pair, int>();
            //query.Root = otherRoot;

            //Coordinates[] coordinates = null;

            //query.Key = 4;
            //var first = query.FirstRefByUniqueKey(ref coordinates);
        }

        private static ReferenceNode<Pair, int> GetRoot()
        {
            var otherRoot = GetRef(15);

            ReferenceNode<Pair, int> node;
            otherRoot.References[0] = node = GetRefVal(5, parent: otherRoot);
            node.Values[0] = GetVal(0);
            node.Values[1] = GetVal(1);
            node.Values[2] = GetVal(2);
            node.Values[3] = GetVal(3);
            node.Values[4] = GetVal(4);

            otherRoot.References[1] = node = GetRefVal(10, parent: otherRoot);
            node.Values[0] = GetVal(5);
            node.Values[1] = GetVal(6);
            node.Values[2] = GetVal(8);
            node.Values[3] = GetVal(9);
            node.Values[4] = GetVal(10);

            otherRoot.References[2] = node = GetRefVal(15, parent: otherRoot);
            node.Values[0] = GetVal(11);
            node.Values[1] = GetVal(12);
            node.Values[2] = GetVal(13);
            node.Values[3] = GetVal(14);
            node.Values[4] = GetVal(15);

            otherRoot.References[3] = node = GetRefVal(18, 2, otherRoot);
            node.Values[0] = GetVal(17);
            node.Values[1] = GetVal(18);
            return otherRoot;
        }
    }
}
