using System;
using System.Threading.Tasks;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries;

namespace Rogue.FastLane.Collections
{
    public class OptmizedCollection<TItem> : SimpleCollection<TItem>
    {
        public OptmizedCollection(params IQuery<TItem>[] queries)
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

            Count++;

            //Parallel.ForEach(Queries, sel => sel.AfterAdd(node, newState));
            foreach (var dispatcher in Dispatchers)
            {
                dispatcher.AddNode(this, node);
            }            
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

                Count--;

                foreach (var dispatcher in Dispatchers)
                {
                    dispatcher.RemoveNode(this, node);
                }

                Task.Factory.StartNew(
                    () => GC.SuppressFinalize(node));
            }
        }
    }
}