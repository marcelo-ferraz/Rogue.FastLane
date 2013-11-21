using System;
using System.Threading.Tasks;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Queries;

namespace Rogue.FastLane.Collections
{
    public class OptimizedCollection<TItem> : SimpleCollection<TItem>
    {
        public OptimizedCollection(params IQuery<TItem>[] queries)
            : base(queries) { }

        protected ValueNode<TItem> CurrentNode;

        public virtual int Count { get; set; }

        public virtual void Add(TItem item)
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

            foreach (var dispatcher in Dispatchers)
            {
                dispatcher.AddNode(this, node);
            }            
        }


        public virtual void Remove<TKey>(IQuery<TItem> selector)
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