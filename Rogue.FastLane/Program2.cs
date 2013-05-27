using System.Diagnostics;
using Rogue.FastLane;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Queries;
using System.Reflection;

namespace Nhonho
{
    class Program2
    {
        #region
        static ReferenceNode<Pair, int> GetRef(int key, int count = 4, ReferenceNode<Pair, int> parent = null)
        {
            return new ReferenceNode<Pair, int>() { Key = key, References = new ReferenceNode<Pair, int>[count], Parent = parent };
        }

        static ReferenceNode<Pair, int> GetRefVal(int key, int count = 4, ReferenceNode<Pair, int> parent = null)
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
            

            structure.Add(new Pair() { Index = 3 });
            structure.Add(new Pair() { Index = 1 });
            structure.Add(new Pair() { Index = 2 });            
		}

        private static void TestReverseEnumerable()
        {

            var otherRoot = GetRef(12);

            ReferenceNode<Pair, int> node;
            otherRoot.References[0] = node = GetRefVal(4, parent: otherRoot);
            node.Values[0] = GetVal(1);
            node.Values[1] = GetVal(2);
            node.Values[2] = GetVal(3);
            node.Values[3] = GetVal(4);

            otherRoot.References[1] = node = GetRefVal(9, parent: otherRoot);
            node.Values[0] = GetVal(5);
            node.Values[1] = GetVal(6);
            node.Values[2] = GetVal(8);
            node.Values[3] = GetVal(9);

            otherRoot.References[2] = node = GetRefVal(13, parent: otherRoot);
            node.Values[0] = GetVal(10);
            node.Values[1] = GetVal(11);
            node.Values[2] = GetVal(12);
            node.Values[3] = GetVal(13);

            otherRoot.References[3] = node = GetRefVal(15, 2, otherRoot);
            node.Values[0] = GetVal(15);
            node.Values[1] = GetVal(16);
            //root.References[3].Values[2] = GetVal(13);


            var @enum = new LowestReverseReferencesEnumerable<Pair, int>().
                UpToHere(otherRoot, new[] { new Pair() { Length = 4, Index = 3 }, new Pair() { Length = 12, Index = 10 } });
                //AllBellow(otherRoot);


            foreach (var item in @enum)
            {
                System.Console.Write("Id: {0}" , item.Key);
            }
        }
    }
}
