using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane;

namespace Nhonho
{
    class Program2
    {
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

        static void Main(string[] args)
        {
            var root = GetRef(12);

            ReferenceNode<Pair, int> node;
            root.References[0] = node = GetRefVal(4, parent: root);
            node.Values[0] = GetVal(1);
            node.Values[1] = GetVal(2);
            node.Values[2] = GetVal(3);
            node.Values[3] = GetVal(4);

            root.References[1] = node = GetRefVal(9, parent: root);
            node.Values[0] = GetVal(5);
            node.Values[1] = GetVal(6);
            node.Values[2] = GetVal(8);
            node.Values[3] = GetVal(9);

            root.References[2] = node = GetRefVal(13, parent: root);
            node.Values[0] = GetVal(10);
            node.Values[1] = GetVal(11);
            node.Values[2] = GetVal(12);
            node.Values[3] = GetVal(13);

            root.References[3] = node = GetRefVal(15, 2, root);
            node.Values[0] = GetVal(15);
            node.Values[1] = GetVal(16);
            //root.References[3].Values[2] = GetVal(13);

            var savior = new ProbableSavior() { Root = root };

            savior.Save(new Pair() { Index = 6, Length=45 });
            savior.Save(new Pair { Index = 14 });
            savior.Save(new Pair { Index = 7 });
        }
    }
}
