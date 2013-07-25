using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Infrastructure.Positioning;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries;
using NUnit.Framework;
using Rogue.FastLane.Collections.Items.Mixins;
using Rogue.FastLane.Collections;

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
        protected ReferenceNode<Coordinates, int> GetRef(ReferenceNode<Coordinates, int> parent = null, params Func<ReferenceNode<Coordinates, int>>[] childGetters)
        {
            var refs = new List<ReferenceNode<Coordinates, int>>();

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
        protected ReferenceNode<Coordinates, int> GetRef(ReferenceNode<Coordinates, int> parent = null, params Func<ValueNode<Coordinates>>[] childGetters)
        {
            var vals = new List<ValueNode<Coordinates>>();

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
        protected ReferenceNode<Coordinates, int> GetRef(int key, int count = 5, ReferenceNode<Coordinates, int> parent = null)
        {
            return new ReferenceNode<Coordinates, int>() { Key = key, References = new ReferenceNode<Coordinates, int>[count], Parent = parent };
        }

        /// <summary>
        /// Creates a reference node and adds the sum of pointers asked of referenced nodes
        /// </summary>
        /// <param name="key">the key to be added to the node</param>
        /// <param name="count">the ammount of referenced nodes</param>
        /// <param name="parent">the node Parent</param>
        /// <returns>returns a reference node</returns>
        protected ReferenceNode<Coordinates, int> GetRef2Val(int key, int count = 5, ReferenceNode<Coordinates, int> parent = null)
        {
            return new ReferenceNode<Coordinates, int>() { Key = key, Values = new ValueNode<Coordinates>[count], Parent = parent };
        }

        /// <summary>
        /// Creates a value node
        /// </summary>
        /// <param name="key"></param>
        /// <returns>returns a valued node</returns>
        protected ValueNode<Coordinates> GetVal(int key)
        {
            return new ValueNode<Coordinates>() { Value = new Coordinates() { Index = key } };
        }
        

        protected int RightIndex = 0;

        protected void ValidateOrder(ReferenceNode<Coordinates, int> node)
        {
            var root = node.Root();
            
            var enu =
                new LowestReferencesEnumerable<Coordinates, int>();

            foreach (var @ref in enu.AllFrom(root))
            {
                ValidateOrder(@ref.Values);
            }
        }

        protected void ValidateOrder(ValueNode<Coordinates>[] values)
        {
            Assert.Less(values.Length, 34);            

            foreach (var val in values)
            {                
                Assert.AreEqual(RightIndex, val.Value.Index, "The values in are wrong order");

                RightIndex++;
            }
        }
    }
}
