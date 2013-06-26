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

            structure.Add(new Pair() { Index = 3 });
            structure.Add(new Pair() { Index = 1 });
            structure.Add(new Pair() { Index = 2 });
            structure.Add(new Pair() { Index = 4 });
            structure.Add(new Pair() { Index = 5 });
            structure.Add(new Pair() { Index = 7 });
            structure.Add(new Pair() { Index = 6 }); 
            structure.Add(new Pair() { Index = 0 });
            structure.Add(new Pair() { Index = 8 });
            structure.Add(new Pair() { Index = 10 });
            structure.Add(new Pair() { Index = 11 });
            structure.Add(new Pair() { Index = 9 }); 
        }
    }
}