namespace Rogue.FastLane.Collections.Items.Mixins
{
    public static class NodeMixins
    {
        public static ReferenceNode<TItem, TKey> Root<TItem, TKey>(this ReferenceNode<TItem, TKey> node)
        {
            return node.Parent != null ? node.Parent.Root() : node;
        }
    }
}
