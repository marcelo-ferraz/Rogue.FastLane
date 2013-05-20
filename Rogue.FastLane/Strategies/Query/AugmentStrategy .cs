using System;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Collections.Items;

namespace Rogue.FastLane.Strategies.Query
{
	public class AugmentStrategy
	{
        private NodeFetchStrategy _getter;

        public AugmentStrategy(NodeFetchStrategy getter)
        {
            _getter = getter;
        }

        /// <summary>
        /// Augments the level count.
        /// </summary>
        /// <param name='root'>
        /// The Root of the selector.
        /// </param>
        /// <param name='state'>
        /// the state of the selector tree.
        /// </param>
        /// <param name='itemAmmountToSum'>
        /// Item ammount to sum to the tree.
        /// </param>
        public void AugmentLevelCount<TItem, TKey>(ReferenceNode<TItem, TKey> root, UniqueKeyQueryState state, int itemAmmountToSum)
        {
            int newLength =
                state.Length + itemAmmountToSum;

            var spacesCount =
                Math.Pow(state.OptimumLenghtPerSegment, state.Levels.Length);

            //if there is enough room for this new item, return
            if (spacesCount >= newLength) { return; }

            //increases one level
            //if there is only one level bellow the root,
            if (root.Values != null)
            {
                //Increase one level, and send them to the first node of this new level
                root.References =
                    new ReferenceNode<TItem, TKey>[1];

                root.References[0].Values =
                    root.Values;

                root.Values = null;
            }
            else
            {
                //Increase one level, and send them to the first node of this new level
                var refs =
                    root.References;

                root.References =
                    new ReferenceNode<TItem, TKey>[1];

                root.References[0].References =
                    root.References;

                refs = null;
            }
        }

        public void AugmentValueCount<TItem, TKey>(ReferenceNode<TItem, TKey> root, UniqueKeyQueryState state, int itemAmmountToSum)
        {
            int newLength =
                state.Length + itemAmmountToSum;

            var spacesCount =
                Math.Pow(state.OptimumLenghtPerSegment, state.Levels.Length);

            //if there is not enough room for this new item
            if (spacesCount < newLength)
            { AugmentLevelCount(root, state, itemAmmountToSum); }

            //get the one who references the value array that can be changed
            int holderIndex = (int)
                Math.Ceiling((decimal)newLength / state.Levels.Length);

            //descobrir a referencia do referenceNode que esta mais proximo dos valores
            //por calculo, achar as coordenadas certas, do node
            /* 
             * por exemplo,
             * a estrutura tem 14 valores, e vai ser inserido um valor
             * nesse caso, não é necessario que a contagem de niveis seja alterada,
             * newLength / state.Levels.Length ->  15/16 = 0.9375, ou 93,75%
             * 
             * talvez -> arredondar de 93.75 para para 1, de 53.44 para 0,5 
            */
            var nodeFound =
                _getter.GetLastRefNodeByItsValueFlatIndex(root, holderIndex, state, 1);

            nodeFound.Values =
                nodeFound.Values.Resize(newLength);
        }
	}
}

