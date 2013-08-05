using System.Collections.Generic;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Infrastructure.Positioning;

namespace Rogue.FastLane.Collections
{
    public class LowRefsEnumerable<TItem, TKey>
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

        public IEnumerable<ReferenceNode<TItem, TKey>> FromHereOn(ReferenceNode<TItem, TKey> node, Coordinates[] coordinates, int lvlIndex = 1)
        {
            if (node != null)
            {
                if (node.Values != null)
                {
                    yield return node;
                }
                else if (node.References != null)
                {
                    int overallIndex =
                        coordinates[lvlIndex].OverallIndex;

                    for (int i = coordinates[lvlIndex].Index; i < node.Length && overallIndex < coordinates[lvlIndex].OverallLength; i++)
                    {
                        overallIndex++;

                        foreach (var grandChild in FromHereOn(node.References[i], coordinates, lvlIndex + 1))
                        {
                            yield return grandChild;
                        }
                    }
                }
            }
        }
    }
}