using System;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;

namespace Rogue.FastLane.Selectors
{
    public class SimpleSelector<TItem, TKey> : ISelector<TItem>
    {
        /// <summary>
        /// Numero maximo de comparacoes para encontrar um valor
        /// </summary>
        protected int MaxComparisons;

        protected ReferenceNode<TItem, TKey> Root;

        protected OptmizedStructure<TItem> Structure;

        protected TKey Key;

        /// <summary>
        /// Used for caching, and therefore enhancing the performance, and keeping the inteligence 
        /// for typing variaty by keeping the type choosen comparison
        /// </summary>
        public Func<TKey, TKey, int> KeyComparer { get; set; }

        /// <summary>
        /// Seletor de chaves, usado para obter a chave desse registro
        /// </summary>
        public Func<TItem, TKey> SelectKey { get; set; }

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

        public R Switch<T, R>(T graph) 
        {
            return (R)Convert.ChangeType(graph, typeof(R));
        }
        
        public SimpleSelector<TItem, TKey> Select(TKey key)
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

        public void AfterAdd(ValueNode<TItem> node, StructState state)
        {
            /*
             * Sao tres casos que podem ocorrer aqui:
             * 1 - a qtd de niveis eh a mesma, mas eh necessario mover os ponteiros dos itens
             * 2 - eh necessario aumentar ou diminuir a quantidade de niveis e mover os ponteiros dos itens
             * 3 - a qtd de niveis eh a mesma, e nao eh necessario mover os ponteiros dos itens (talvez nao aconteca)
             * 4 - o elemento existe e eh atualizado
             */
            bool sameLevelCount = 
                Structure.State.Levels.Length == state.Levels.Length;

            //if (!sameLevelCount) { RearrangeLevels(Root, state) }

            InsertNode(node, state); 

            //Case the number of levels differs, alter structure
            //if(Structure.State.Levels.Length != state.Levels.Length)
            //{

            //}

            
            //KeySelector(node.Value)
        }

        private void InsertNode(ValueNode<TItem> node, StructState state)
        {
            var key = 
                SelectKey(node.Value);

            var refNode = 
                this.FirstReference(key, Root);

            int index = refNode.Values.BinarySearch(n =>
                        CompareKeys(key, SelectKey(n.Value)));

            if (index > -1) 
            { 
                refNode.Values[index] = node;
                return;
            }
            
            Insert(refNode, node, ~index, state);
            
            var oldValue = refNode.Values[~index];
        }

        private void Insert(ReferenceNode<TItem, TKey> parent, ValueNode<TItem> node, int startIndex, StructState state)
        {
            ValueNode<TItem> lastNode = null;

            bool hasMoreThanItShould = 
                parent.Values.Length > state.OptimumLenghtPerSegment;

            if (!hasMoreThanItShould)
            {
                var values = parent.Values;
                Array.Resize(ref values, parent.Values.Length + 1);
                parent.Values = values;
                values = null;
            }
            else { lastNode = parent.Values[parent.Values.Length - 1]; }
            
            for (int i = startIndex; i < parent.Values.Length; i++)
            {
                parent.Values[i + 1] = parent.Values[i];
            }

            parent.Values[startIndex] = node;

            if(hasMoreThanItShould)
            {
                var key = SelectKey(
                    parent.Values[parent.Values.Length - 1].Value);

                ChangeParentKey(key, parent);

                var refs = 
                    parent.Parent.References;

                var parentIndex = refs.BinarySearch(
                    n => CompareKeys(n.Key, parent.Key));
                
                int proposedIndex = 
                    parentIndex + 1;

                //O node acima do parent tem menos referencias que pode ter
                if (state.OptimumLenghtPerSegment > proposedIndex)
                {
                    var uncle =
                        refs[parentIndex + 1];

                    Insert(uncle, lastNode, 0, state);
                }
                else //O node acima do parent tem a quantidade de referencias que pode ter, logo devo
                { 
                    //achar o node seguinte, no mesmo nivel,
                    //inserir dentro
                }
            }
        }

        private void ChangeParentKey(TKey newKey, ReferenceNode<TItem, TKey> node)
        { 
            if(CompareKeys(node.Key, newKey) > 0)
            {
                node.Key = newKey;

                if (node.Parent != null)
                { ChangeParentKey(newKey, node.Parent); }
            }
        }

        private void RearrangeLevels(ReferenceNode<TItem, TKey> Root, StructState state)
        {
            throw new NotImplementedException();
        }

        public void AfterRemove(ValueNode<TItem> item, StructState state)
        {
            throw new NotImplementedException();
        }
    }
    /*
    public class SimpleSelector<TKey, TItem> : ISelector<TItem>
    {
        protected class Pair
        {
            public int Length { get; set; }
            public int Index { get; set; }
        }

        /// <summary>
        /// Numero maximo de comparacoes para encontrar um valor
        /// </summary>
        protected int MaxComparisons;

        protected ReferenceNode<TItem, TKey> Root;

        protected OptmizedStructure<TItem> Structure;

        /// <summary>
        /// Used for caching, and therefore enhancing the performance, and keeping the inteligence 
        /// for typing variaty by keeping the type choosen comparison
        /// </summary>
        public Func<TKey, TKey, int> KeyComparer { get; set; }

        /// <summary>
        /// Seletor de chaves, usado para obter a chave desse registro
        /// </summary>
        public Func<TItem, TKey> KeySelector { get; set; }

        public void ProduceIndexMap()
        {
            /* criar o mapa, baseado nos nodes de valor que a estrtura tem
             1 ultimo nivel, ordenar pelo comparador
             2 subir todos os niveis da arvore ate chegar ao root, organizando pela chave e sempre passando ao no pai, a maior
             3 pensar em como saber que é o node raiz,
             4 pensar se deve ser feito do root para os filhos ou deve ser feito da base para o topo.
                
             consideracoes
             a. se for feito da base para o root, eu posso primeiramente criar a estrutura e popular os ponteiros depois.
             b. Caso ja exista uma estrutura,  e os cálculos ulo de profundidade e tamanho não afetarem a formação da árvore, reutilizar o que já existe. 
             C. Pensar em usar ponteiros para as chaves
             D. Caso exista mudança avaliar se ela não pode ser feita no cluster específico e subir até o no onde o valor da chave ainda satisfaça mais que o valor da chave nova. 
             E. Lista flat steps=log2(length-1), para a árvore:? 
            * /

            //considerar: caso exista uma lista, e ela tiver poucos itens, nao criar a estrutura (talvez)
            //if (BypassWorstScenario(RawList = rawList)) { return; }

            double lastLevelLength = Structure.Count;

            int optimumLength;

            int levelCount = FindHowManyLevelsShouldExist(
                ref lastLevelLength, optimumLength = GetOptimumLength());

            var qtdOfNodesPerLevel = 
                GetQtdOfNodesPerLevel(Structure.Count, optimumLength, levelCount);

            Root = 
                new ReferenceNode<TItem,TKey>()
                { SubNodes = new INode[qtdOfNodesPerLevel[0].Length] };

            CreateTree(Root, optimumLength, qtdOfNodesPerLevel);
        }

        protected virtual bool TryCreateChild(int optimumLength, Pair[] indexNLenght, int currentLevel, int nextLvl, out INode child)
        {
            child = null;

            var brokenLength =
                indexNLenght[nextLvl].Length - indexNLenght[nextLvl].Index;

            var moreThanMaxLenghtForLevel =
                indexNLenght[nextLvl].Length <= (optimumLength + indexNLenght[nextLvl].Index);

            indexNLenght[currentLevel].Index++;

            if (moreThanMaxLenghtForLevel && brokenLength < 0) { return false; }

            var length = moreThanMaxLenghtForLevel ? brokenLength : optimumLength;

            bool nextIsLastLevel =
                nextLvl == (indexNLenght.Length - 1);

            child = nextIsLastLevel ?
                (INode)new ValueNode<TItem>() { Items = new TItem[length] } :
                new ReferenceNode<TItem, TKey>() { SubNodes = new INode[length] };

            return true;
        }


        protected virtual void CreateTree(INode node, int optimumLength, Pair[] indexNLenght, int currentLevel = 0)
        {
            if (node is ReferenceNode<TItem, TKey>)
            {
                var refNode = 
                    (ReferenceNode<TItem, TKey>)node;
                
                var nextLvl = currentLevel + 1;

                for (long i = 0; i < refNode.SubNodes.LongLength && indexNLenght[currentLevel].Index < indexNLenght[currentLevel].Length; i++)
                {
                    INode child;

                    if (TryCreateChild(optimumLength, indexNLenght, currentLevel, nextLvl, out child))
                    {
                        refNode.SubNodes[i] = child;
                        refNode.SubNodes[i].Parent = node;

                        CreateTree(child, optimumLength, indexNLenght, currentLevel + 1);
                    }
                }
                node.Key = refNode.SubNodes[refNode.SubNodes.Length - 1].Key;
            }
            else
            {
                var val = (ValueNode)node;

                for (int i = 0; i < val.Items.Length && indexNLenght[currentLevel].Index < RawList.Count; i++)
                {
                    var current = indexNLenght[currentLevel];

                    val.Items[i] = RawList[current.Index];
                    node.Key = KeySelector(
                        RawList[current.Index]);

                    indexNLenght[currentLevel].Index++;
                }
                node.Key = KeySelector(val.Items[val.Items.Length - 1]);
            }
        }

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

        protected virtual bool BypassWorstScenario(List<TItem> rawList)
        {
            double worstCaseNumberOfComparisons =
                2 * Math.Log(rawList.Count - 1, 2);

            /*
             * se no pior caso, a busca binaria vai ter até o numero maximo comparações
             * devo usar a lista crua.
             * /
            return worstCaseNumberOfComparisons < MaxComparisons;
        }

        protected virtual int GetOptimumLength()
        {
            return //função inversa para achar a quantidade de interações no pior cenário
                (int)Math.Round(Math.Pow(2, (MaxComparisons / 2)) + 1);
        }

        protected virtual int FindHowManyLevelsShouldExist(ref double length, int optimumLength, int level = 2)
        {
            length = Math.Ceiling(length / optimumLength);

            return length > optimumLength ?
                FindHowManyLevelsShouldExist(ref length, optimumLength, ++level) :
                level;
        }

        protected virtual Pair[] GetQtdOfNodesPerLevel(int structureLenght, int optimumLength, int levelCount)
        {
            var ammountOfNodesPerLevel = new Pair[levelCount];

            double percentage = structureLenght / Math.Pow(optimumLength, levelCount);

            for (int i = 0; i < ammountOfNodesPerLevel.Length; i++)
            {
                ammountOfNodesPerLevel[i] = new Pair { Length = (int)Math.Ceiling(Math.Pow(optimumLength, i + 1) * percentage) };
            }
            return ammountOfNodesPerLevel;
        }
    }*/
}
