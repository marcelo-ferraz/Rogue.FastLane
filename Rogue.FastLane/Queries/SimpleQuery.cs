using System;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;

namespace Rogue.FastLane.Queries
{
    public abstract class SimpleQuery<TItem, TKey> : IQuery<TItem>
    {
        /// <summary>
        /// Numero maximo de comparacoes para encontrar um valor
        /// </summary>
        protected int MaxComparisons;

        protected ReferenceNode<TItem, TKey> Root;

        protected OptmizedStructure<TItem> Structure;

        protected TKey Key;

        public abstract void AfterAdd(ValueNode<TItem> node, UniqueKeyQueryState state);

        public abstract void AfterRemove(ValueNode<TItem> item, UniqueKeyQueryState state);

        /// <summary>
        /// Used for caching, and therefore enhancing the performance, and keeping the inteligence 
        /// for typing variaty by keeping the type choosen comparison
        /// </summary>
        public virtual Func<TKey, TKey, int> KeyComparer { get; set; }

        /// <summary>
        /// Seletor de chaves, usado para obter a chave desse registro
        /// </summary>
        public virtual Func<TItem, TKey> SelectKey { get; set; }

        /// <summary>
        /// Compara duas chaves
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        protected virtual int CompareKeys(TKey key1, TKey key2)
        {
            return (KeyComparer ?? (KeyComparer =
                !typeof(IComparable).IsAssignableFrom(typeof(TKey)) ?
                (Func<TKey, TKey, int>)((k1, k2) => ((IComparable<TKey>)k1).CompareTo(k2)) :
                (k1, k2) => ((IComparable)k1).CompareTo(k2))
                )(key1, key2);
        }

        protected virtual ReferenceNode<TItem, TKey> FirstReference(TKey key, ReferenceNode<TItem, TKey> node)
        {
            if (node.Values != null) { return node; }
			
            int index = 
                node.References.BinarySearch(k =>
                    CompareKeys(key, k.Key));
			
            var found =
                node.References[index < 0 ? ~index : index];

            return found != null ? FirstReference(key, found) : null;
        }
       
        public SimpleQuery<TItem, TKey> Select(TKey key)
        {
            Key = key;
            return this;
        }

        public ValueNode<TItem> First()
        {
            var found =
                FirstReference(Key, Root);

            return found.Values.BinaryGet(
                    node =>
                        CompareKeys(Key, SelectKey(node.Value))); 
        }
    }
}
