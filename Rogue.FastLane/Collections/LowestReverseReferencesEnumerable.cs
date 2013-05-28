using System.Collections.Generic;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;

namespace Rogue.FastLane.Collections
{
    public class LowestReferencesReverseEnumerable<TItem, TKey>
    {
        public IEnumerable<ReferenceNode<TItem, TKey>> AllBellow(ReferenceNode<TItem, TKey> node)
        {
            if (node.Values != null)
            {
                yield return node;
            }
            else if (node.References != null)
            {
                for (int i = node.References.Length -1; i > 0; i--)
                {
                    foreach (var grandChild in AllBellow(node.References[i]))
                    {
                        yield return grandChild;
                    }
                }
            }
        }        

        public IEnumerable<ReferenceNode<TItem, TKey>> UpToHere(ReferenceNode<TItem, TKey> node, Pair[] offsetPerLvl, int lvlIndex = 0)
        {
            if (node.Values != null)
            {
                yield return node;
            }
            else if (node.References != null)
            {
                for (int i = node.References.Length - 1; i > 0 && offsetPerLvl[lvlIndex].Index < offsetPerLvl[lvlIndex].Length; i--)
                {
                    offsetPerLvl[lvlIndex].Index++;

                    foreach (var grandChild in UpToHere(node.References[i], offsetPerLvl, lvlIndex + 1))
                    {
                        yield return grandChild;
                    }
                }
            }
        }
    }
}