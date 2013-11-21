using System.Collections.Generic;
using Rogue.FastLane.Collections.Items;
using System;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Infrastructure.Positioning;

namespace Rogue.FastLane.Collections
{
    public class LowRefsReverseEnumerable<TItem, TKey>
    {
        public IEnumerable<ReferenceNode<TItem, TKey>> AllBellow(ReferenceNode<TItem, TKey> node)
        {
            if (node != null)
            {
                if (node.Values != null)
                {
                    yield return node;
                }
                else if (node.References != null)
                {
                    for (int i = node.References.Length - 1; i > 0; i--)
                    {
                        foreach (var grandChild in AllBellow(node.References[i]))
                        {
                            yield return grandChild;
                        }
                    }
                }
            }
        }

        public IEnumerable<ReferenceNode<TItem, TKey>> UpToHere(ReferenceNode<TItem, TKey> node, Coordinates[] offsetPerLvl, int lvlIndex = 1)
        {
            if (node != null)
            {
                if (node.Values != null)
                {
                    yield return node;
                }
                else if (node.References != null)
                {
                    int reverseIndex = 
                        offsetPerLvl[lvlIndex].OverallLength;
                    //                                                         0                                8
                    for (int i = node.References.Length - 1; i > -1 && offsetPerLvl[lvlIndex].OverallIndex < reverseIndex; i--)
                    {
                        reverseIndex--;

                        foreach (var grandChild in UpToHere(node.References[i], offsetPerLvl, lvlIndex + 1))
                        {
                            yield return grandChild;
                        }
                    }
                }
            }
        }
    }
}