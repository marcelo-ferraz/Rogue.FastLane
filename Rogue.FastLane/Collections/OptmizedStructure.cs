using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Selectors;
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

namespace Rogue.FastLane.Collections
{
    public class OptmizedStructure<TItem>
    {
        public StructState State { get; set; }

        protected ISelector<TItem>[] Selectors;

        public int Count { get; set; }

        protected ValueNode<TItem> CurrentNode;

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
            else
            {
                CurrentNode = node;
            }

            var s =
                StructCalculus.Calculate(State, Count + 1);

            Parallel.ForEach(Selectors, sel => sel.AfterAdd(node, s));

            Count++;
        }


        public void Remove<TKey>(ISelector<TItem> selector)
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
                    StructCalculus.Calculate(State, Count + 1);

                Parallel.ForEach(Selectors, 
                    sel => 
                        sel.AfterRemove(node, s));

                Task.Factory.StartNew(
                    () => GC.SuppressFinalize(node));
            }
        }
    }
}

