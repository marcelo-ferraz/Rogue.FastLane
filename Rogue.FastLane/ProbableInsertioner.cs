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
using Rogue.FastLane.Selectors;

using Rogue.FastLane.Collections.Mixins;

namespace Rogue.FastLane
{
    public class ProbableSavior 
        : SimpleSelector<Pair, int> 
    {
        public ReferenceNode<Pair, int> Root { get; set; }

        

        public void Save(Pair item)
        {
            SelectKey = 
                p => p.Index;

            var key = SelectKey(item);

            var firstRef = 
                FirstReference(SelectKey(item), Root);

            var index = firstRef.Values.BinarySearch(val =>
                key.CompareTo(SelectKey(val.Value)));

            //inserção
            if(index > -1)
            {
                firstRef.Values[index].Value = item;
                return;
            }

            if(firstRef.Parent == null)
            {
                throw new NotImplementedException("o firstRef é raiz, esta situaçào ainda não foi implementada");
            }

            if(firstRef.Parent.References == null)
            {
                throw new Exception("O node atual nào está nas referencias do parent.");
            }

            var valueNode = new ValueNode<Pair> { Value = item };

            //caso caiba na colecao atual
            if (firstRef.Values.Length + 1 < 4 /*Optimum size*/)
            {
                InsertInto(valueNode, firstRef, index);
            }
            else
            {
                var iterator = 
                    new StructureIterator4LowerLevels<Pair, int>();
                
                foreach (var node in iterator.ModifyStructureIfNeeded(firstRef, plus: 1))
                {
                    valueNode = 
                        InsertInto(valueNode, node, index);
                    if (iterator.WasModified) { break; }
                }
            }
        }

        private IEnumerable<ReferenceNode<Pair, int>> IterateNCreateStructureIfNeeded(ReferenceNode<Pair, int> node, bool modified, int plus = 0, int minus = 0)
        {
            if (node.Values != null)
            {
                int newLength = node.Values.Length
                    + plus
                    - minus;

                if (newLength < 4 /*Optimum size*/)
                {
                    node.Values =
                        node.Values.Resize(newLength);
                    modified = true;
                }
                yield return node;
            }
            else if (node.References != null)
            {
                for (int i = 0; i < node.References.Length; i++)
                {
                    foreach (var n in IterateNCreateStructureIfNeeded(node.References[i], modified, plus, minus)) 
                    {
                        yield return n;
                    }
                }
            }
        }

        private static ValueNode<Pair> InsertInto(ValueNode<Pair> item, ReferenceNode<Pair, int> nodeRef, int index)
        {
            ValueNode<Pair> lastNode = null;

            if (nodeRef.Values.Length < 4/*optimum lenght*/)
            {
                nodeRef.Values = nodeRef.Values.Resize(
                    nodeRef.Values.Length + 1);
            }

            for (int i = ~index; i < nodeRef.Values.Length; i++)
            {
                if ((i + 1) < nodeRef.Values.Length)
                { nodeRef.Values[i + 1] = nodeRef.Values[i]; }
                else 
                { lastNode = nodeRef.Values[i]; }
            }
            nodeRef.Values[~index] = item;
            return lastNode;
        }
    }

    public class StructureIterator4LowerLevels<Titem, Tkey>
    {
        public bool WasModified { private set;  get; }

        public IEnumerable<ReferenceNode<Pair, int>> ModifyStructureIfNeeded(ReferenceNode<Pair, int> node, int plus = 0, int minus = 0)
        {
            if (node.Values != null)
            {
                int newLength = node.Values.Length
                    + plus
                    - minus;

                if (newLength < 4 /*Optimum size*/)
                {
                    node.Values =
                        node.Values.Resize(newLength);
                    WasModified = true;
                }
                yield return node;
                
                foreach (var n in ModifyStructureIfNeeded(node.Parent, plus, minus))
                {
                    yield return n;
                }
            }
            else if (node.References != null)
            {
                for (int i = 0; i < node.References.Length; i++)
                {
                    foreach (var n in ModifyStructureIfNeeded(node.References[i], plus, minus))
                    {
                        yield return n;
                    }
                }
            }
        }
    }
}