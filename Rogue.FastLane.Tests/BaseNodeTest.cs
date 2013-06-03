using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Tests
{
    public class BaseNodeTest
    {
        /// <summary>
        /// Creates a reference node and adds the sum of pointers asked of referenced nodes
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childGetters">An array of functions that returns the children</param>
        /// <returns></returns>
        protected ReferenceNode<Pair, int> GetRef(ReferenceNode<Pair, int> parent = null, params Func<ReferenceNode<Pair, int>>[] childGetters)
        {
            var refs = new List<ReferenceNode<Pair, int>>();

            foreach (var getChild in childGetters)
            {
                refs.Add(getChild());
            }

            refs = refs.OrderBy(n => n.Key).ToList();

            var @ref = GetRef(refs.Last().Key, refs.Count, parent);
            @ref.References = refs.ToArray();

            return @ref;
        }

        /// <summary>
        /// Creates a reference node and adds the sum of pointers asked of referenced nodes
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childGetters">An array of functions that returns the children</param>
        /// <returns></returns>
        protected ReferenceNode<Pair, int> GetRef(ReferenceNode<Pair, int> parent = null, params Func<ValueNode<Pair>>[] childGetters)
        {
            var vals = new List<ValueNode<Pair>>();

            foreach (var getChild in childGetters)
            {
                vals.Add(getChild());
            }

            vals = vals.OrderBy(n => n.Value.Index).ToList();

            var @ref = GetRef(vals.Last().Value.Index, vals.Count, parent);
            @ref.Values = vals.ToArray();

            return @ref;
        }

        /// <summary>
        /// Creates a reference node and adds the sum of pointers asked of referenced nodes
        /// </summary>
        /// <param name="key">the key to be added to the node</param>
        /// <param name="count">the ammount of referenced nodes</param>
        /// <param name="parent">the node Parent</param>
        /// <returns>returns a reference node</returns>
        protected ReferenceNode<Pair, int> GetRef(int key, int count = 5, ReferenceNode<Pair, int> parent = null)
        {
            return new ReferenceNode<Pair, int>() { Key = key, References = new ReferenceNode<Pair, int>[count], Parent = parent };
        }

        /// <summary>
        /// Creates a reference node and adds the sum of pointers asked of referenced nodes
        /// </summary>
        /// <param name="key">the key to be added to the node</param>
        /// <param name="count">the ammount of referenced nodes</param>
        /// <param name="parent">the node Parent</param>
        /// <returns>returns a reference node</returns>
        protected ReferenceNode<Pair, int> GetRef2Val(int key, int count = 5, ReferenceNode<Pair, int> parent = null)
        {
            return new ReferenceNode<Pair, int>() { Key = key, Values = new ValueNode<Pair>[count], Parent = parent };
        }

        /// <summary>
        /// Creates a value node
        /// </summary>
        /// <param name="key"></param>
        /// <returns>returns a valued node</returns>
        protected ValueNode<Pair> GetVal(int key)
        {
            return new ValueNode<Pair>() { Value = new Pair() { Index = key } };
        }
    }
}
