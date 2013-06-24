using System.Collections.Generic;
using Rogue.FastLane.Collections.Items;

namespace Rogue.FastLane.Collections
{
    public class LowestReferencesEnumerable<TItem, TKey>
    {
        public IEnumerable<ReferenceNode<TItem, TKey>> LastNLowestRefs(ReferenceNode<TItem, TKey> node)
        {
            if (node.References != null)
            {
                for (int i = 0; i < node.References.Length; i++)
                {
                    if (node.References[i].Values != null)
                    {
                        yield return node.References[i = node.References.Length - 1];
                    }

                    foreach (var grandChild in AllFrom(node.References[i]))
                    {
                        yield return grandChild;
                    }
                }
            }
            else if (node.Values != null)
            { 
                yield return node;
            }
        }

        public IEnumerable<ReferenceNode<TItem, TKey>> AllFrom(ReferenceNode<TItem, TKey> node)
        {
            // aqui só funciona do root em diante. Pensar em como deveria ser se pegasse de um refnode em diante
            if (node != null)
            {
                if (node.Values != null)
                {
                    yield return node;
                }
                else if (node.References != null)
                {
                    foreach (var child in node.References)
                    {
                        foreach (var grandChild in AllFrom(child))
                        {
                            yield return grandChild;
                        }
                    }
                }
            }
        }

        public IEnumerable<ReferenceNode<TItem, TKey>> FromHereOn(ReferenceNode<TItem, TKey> node, OneTimeValue[] offsetPerLvl, int lvlIndex = 0)
        {
            if (node != null)
            {
                if (node.Values != null)
                {
                    yield return node;
                }
                else if (node.References != null)
                {
                    for (int i = offsetPerLvl[lvlIndex].Value; i < node.References.Length; i++)
                    {
                        foreach (var child in FromHereOn(node.References[i], offsetPerLvl, lvlIndex))
                        {
                            yield return child;
                        }
                    }
                }
            }
        }
    }
}