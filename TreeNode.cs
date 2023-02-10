using System;
using lab10;

namespace lab12
{
    public class TreeNode
    {
        public TreeNode? left;
        public TreeNode? right;
        public TreeNode? parent;

        public Printing value;
        public string key;


        public TreeNode(string key, Printing p)
        {
            this.key = key;
            value = p;
        }
    }
}

