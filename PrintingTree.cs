using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using lab10;



namespace lab12
{
    public class PrintingTree : IDictionary<string, Printing>
    {
        TreeNode? root = null;

        public TreeNode? Root { get { return root; } }


        public PrintingTree() { }

        public PrintingTree(PrintingTree another) { root = another.Root; }


        public void CloneTo(PrintingTree another)
        {
            KeyValuePair<string, Printing>[] arr = new KeyValuePair<string, Printing>[Count];
            CopyTo(arr, 0);

            foreach (var tn in arr)
                another.Add(tn);
        }


        string ToString(TreeNode? t)
        {
            if (t == null)
                return "";
            else
                return ToString(t.left) + '\n' + t.value.ToString() + '\n' + ToString(t.right);
        }

        public override string ToString() { return ToString(root); }


        private void Add(string key, Printing value, TreeNode q)
        {
            if (q.key.CompareTo(key) > 0)
            {
                if (q.left == null)
                {
                    TreeNode newNode = new TreeNode(key, value);
                    newNode.parent = q;
                    q.left = newNode;
                }
                else
                    Add(key, value, q.left);
            }
            else
            {
                if (q.right == null)
                {
                    TreeNode newNode = new TreeNode(key, value);
                    newNode.parent = q;
                    q.right = newNode;
                }
                else
                    Add(key, value, q.right);
            }
        }


        public void Add(string key, Printing value)
        {
            if (root == null)
                root = new TreeNode(key, value);
            else
                Add(key, value, root);
        }


        public void Add(KeyValuePair<string, Printing> pair)
        {
            if (root == null)
                root = new TreeNode(pair.Key, pair.Value);
            else
                Add(pair.Key, pair.Value, root);
        }


        public void Add(KeyValuePair<string, Printing>[] arr)
        {
            foreach (var pair in arr)
                Add(pair);
        }


        int CountNodes(TreeNode? t)
        {
            if (t == null)
                return 0;
            else
                return 1 + CountNodes(t.left) + CountNodes(t.right);
        }


        public int Count
        {
            get { return CountNodes(root); }
        }


        public bool IsReadOnly { get { return false; } }


        void TreeToArray(KeyValuePair<string, Printing>[] array, ref int arrayIndex, TreeNode t)
        {
            if (t.left != null)
                TreeToArray(array, ref arrayIndex, t.left);

            array[arrayIndex] = new KeyValuePair<string, Printing>(t.value.ToString(), t.value);
            arrayIndex += 1;

            if (t.right != null)
                TreeToArray(array, ref arrayIndex, t.right);
        }


        public void CopyTo(KeyValuePair<string, Printing>[] array, int arrayIndex)
        {
            if (root == null)
                throw new ArgumentNullException();
            else
                TreeToArray(array, ref arrayIndex, root);
        }


        TreeNode? Find(string key, TreeNode? t)
        {
            if (t == null)
                return null;

            if (t.key == key)
                return t;

            if (Find(key, t.left) == null)
                return Find(key, t.right);
            else
                return Find(key, t.left);
        }


        public Printing? FindValue(string key)
        {
            if (Find(key, root) != null)
                return Find(key, root).value;
            else return null;
        }


        void Delete(TreeNode t)
        {
            //Console.WriteLine(t.value);
            if (t.left != null)
                Delete(t.left);
            t.left = null;

            if (t.right != null)
                Delete(t.right);
            t.right = null;

            if (t.parent != null)
            {
                if (t == t.parent.left)
                    t.parent.left = null;

                if (t == t.parent.right)
                    t.parent.right = null;
            }
        }


        public bool Remove(string key)
        {
            TreeNode? q = Find(key, root);
            if (q == null)
                return false;
            else
            {
                Delete(q);
                GC.Collect();
                return true;
            }
        }


        public bool Remove(KeyValuePair<string, Printing> pair)
        {
            TreeNode? q = Find(pair.Key, root);
            if (q == null)
                return false;
            else
            {
                Delete(q);
                GC.Collect();
                return true;
            }
        }


        public void Clear()
        {
            if (root != null)
            {
                Delete(root);
                root = null;
                GC.Collect();
            }
        }


        public bool ContainsKey(string key)
        {
            if (Find(key, root) != null)
                return true;
            else
                return false;
        }


        public bool Contains(KeyValuePair<string, Printing> pair)
        {
            if (Find(pair.Key, root) != null)
                return Find(pair.Key, root).value == pair.Value;
            else
                return false;
        }


        public bool TryGetValue(string key, out Printing value)
        {
            value = Find(key, root).value;
            if (value == null)
                return false;
            else
                return true;
        }


        IEnumerator<KeyValuePair<string, Printing>> IEnumerable<KeyValuePair<string, Printing>>.GetEnumerator()
        {
            return new PrintingTreeEnumerator(this);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }


        public Printing this[string key]
        {
            get
            {
                if (Find(key, root) == null)
                    throw new ArgumentOutOfRangeException();
                else
                    return Find(key, root).value;
            }
            set
            {
                Add(key, value, root);
            }
        }


        void TreeToKeyArray(string[] array, ref int arrayIndex, TreeNode t)
        {
            if (t.left != null)
                TreeToKeyArray(array, ref arrayIndex, t.left);

            array[arrayIndex] = t.key;
            arrayIndex += 1;

            if (t.right != null)
                TreeToKeyArray(array, ref arrayIndex, t.right);
        }


        public ICollection<string> Keys
        {
            get
            {
                string[] keys = new string[this.Count];
                int i = 0;
                TreeToKeyArray(keys, ref i, root);
                return keys;
            }
        }


        void TreeToValueArray(Printing[] array, ref int arrayIndex, TreeNode t)
        {
            if (t.left != null)
                TreeToValueArray(array, ref arrayIndex, t.left);

            array[arrayIndex] = t.value;
            arrayIndex += 1;

            if (t.right != null)
                TreeToValueArray(array, ref arrayIndex, t.right);
        }


        public ICollection<Printing> Values
        {
            get
            {
                Printing[] values = new Printing[this.Count];
                int i = 0;
                TreeToValueArray(values, ref i, root);
                return values;
            }
        }
    }
}

