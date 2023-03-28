using System;
using lab10;

namespace lab12
{
    public class TreeNode<TKey, TValue>
    {
        public TreeNode<TKey, TValue>? left;
        public TreeNode<TKey, TValue>? right;
        public TreeNode<TKey, TValue>? parent;

        public TValue value;
        public TKey key;


        public TreeNode(TKey key, TValue p)
        {
            this.key = key;
            value = p;
        }


        public TreeNode(TreeNode<TKey, TValue> another)
        {
            if (another.key is ICloneable)
                key = (TKey)another.key;
            else key = another.key;
            
            if (another.value is ICloneable)
                value = (TValue)another.value; 
            else value = another.value;
        }


        public TreeNode(KeyValuePair<TKey, TValue> another)
        {
            if (another.Key is ICloneable)
                key = (TKey)another.Key;
            else key = another.Key;

            if (another.Value is ICloneable)
                value = (TValue)another.Value;
            else value = another.Value;
        }
    }
}

