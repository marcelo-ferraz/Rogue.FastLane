using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Infrastructure;
using Rogue.FastLane.Queries.States;
using Rogue.FastLane.Queries.Dispatchers;
using Rogue.FastLane.Config;

namespace Rogue.FastLane.Collections
{
    public class OptmizedStructure<TItem> : BasicStructure<TItem>
    {
        public OptmizedStructure(params IQuery<TItem>[] queries)
            : base(queries) { }
        
        protected ValueNode<TItem> CurrentNode;

        public int Count { get; set; }

        public void Add(TItem item)
        {
            var node =
                new ValueNode<TItem>
                {
                    Value = item,
                };

            if (CurrentNode != null)
            {
                CurrentNode.Next = node;
                node.Prior = CurrentNode;                
            }
            
            CurrentNode = node;

            //Parallel.ForEach(Queries, sel => sel.AfterAdd(node, newState));
            foreach (var dispatcher in Dispatchers.Where( d => d != null))
            {
                dispatcher.AddNode(this, node);
            }

            Count++;
        }


        public void Remove<TKey>(IQuery<TItem> selector)
        {
            var node =
                selector.First();

            if (node.Prior != null && node.Next != null)
            {
                var next =
                    node.Next;
                var prior =
                    node.Prior;

                next.Prior = prior;
                prior.Next = next;

                var s = 
                    StructCalculus.Calculate4UniqueKey(Count + 1, 10);

                //Parallel.ForEach(Queries, 
                //    sel => 
                //        sel.AfterRemove(node, s));

                Task.Factory.StartNew(
                    () => GC.SuppressFinalize(node));
            }
        }
    }
}

