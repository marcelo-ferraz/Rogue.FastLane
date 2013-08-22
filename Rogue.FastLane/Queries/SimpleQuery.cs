using System;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Items;
using System.Collections.Generic;

namespace Rogue.FastLane.Queries
{
    public abstract class SimpleQuery<TItem, TKey> : IQuery<TItem>, ICrudQuery<TItem>
    {
        /// <summary>
        /// Key for getting the result
        /// </summary>
        protected internal TKey Key { get; set; }

        protected internal ReferenceNode<TItem, TKey> Root { get; set; }

        /// <summary>
        /// Numero maximo de comparacoes para encontrar um valor
        /// </summary>
        protected int MaxComparisons;

        protected Func<TKey, TKey, int> _keyComparer;

        protected OptimizedCollection<TItem> Structure;

        public string Name { get; set; }  
        /// <summary>
        /// Used for caching, and therefore enhancing the performance, and keeping the inteligence 
        /// for typing variaty by keeping the type choosen comparison
        /// </summary>
        public virtual Func<TKey, TKey, int> KeyComparer {
            get { return GetKeyComparer; }
            set { _keyComparer = value; }
        }

        /// <summary>
        /// Seletor de chaves, usado para obter a chave desse registro
        /// </summary>
        public virtual Func<TItem, TKey> SelectKey { get; set; }
        
        public SimpleQuery<TItem, TKey> Select(TKey key)
        {
            Key = key;
            return this;
        }

        /// <summary>
        /// Compara duas chaves
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        protected internal virtual int GetKeyComparer(TKey key1, TKey key2)
        {
            return (_keyComparer ?? (_keyComparer =
                !typeof(IComparable).IsAssignableFrom(typeof(TKey)) ?
                (Func<TKey, TKey, int>)((k1, k2) => ((IComparable<TKey>)k1).CompareTo(k2)) :
                (k1, k2) => ((IComparable)k1).CompareTo(k2))
                )(key1, key2);
        }

        public abstract ValueNode<TItem> First();

        public abstract IEnumerable<TItem> Enumerate();

        public abstract void AugmentQueryValueCount(int qtd);

        public abstract void AbridgeQueryValueCount(int qtd);
        
        public abstract void AugmentQueryLevelCount(int qtd);
        
        public abstract void AbridgeQueryLevelCount(int qtd);

        public abstract void Add(ValueNode<TItem> item);
        
        public abstract void Remove(ValueNode<TItem> item);
    }
}
