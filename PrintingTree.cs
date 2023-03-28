using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using lab10;



namespace lab12
{
    public class PrintingTree<TKey, TValue> : IDictionary 
        where TKey   : ICloneable
        where TValue : ICloneable
    {
        TreeNode<TKey, TValue>? root = null;
        public TreeNode<TKey, TValue>? Root { get { return root; } }


        public PrintingTree() { }
        public PrintingTree(PrintingTree<TKey, TValue> another) { another.CloneTo(this); }


        string ToString(TreeNode<TKey, TValue>? t)
        {
            if (t == null)
                return "";
            else
                return ToString(t.left) + '\n' + t.value.ToString() + '\n' + ToString(t.right);
        }

        public override string ToString() { return ToString(root); }


        private void Add(TKey key, TValue value, TreeNode<TKey, TValue> q)
        {
            
            if (Comparer.Default.Compare(q.key, key) > 0)
            {
                if (q.left == null)
                {
                    TreeNode<TKey, TValue> newNode = new TreeNode<TKey, TValue>(key, value);
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
                    TreeNode<TKey, TValue> newNode = new TreeNode<TKey, TValue>(key, value);
                    newNode.parent = q;
                    q.right = newNode;
                }
                else
                    Add(key, value, q.right);
            }
        }

        public virtual void Add(object key, object? value)
        {
            if (root == null)
                root = new TreeNode<TKey, TValue>((TKey)key, (TValue)value);
            else
                Add((TKey)key, (TValue)value, root);
        }

        public virtual void Add(KeyValuePair<TKey, TValue> pair)
        {
            if (root == null)
                root = new TreeNode<TKey, TValue>(pair.Key, pair.Value);
            else
                Add(pair.Key, pair.Value, root);
        }

        public virtual void Add(KeyValuePair<TKey, TValue>[] arr)
        {
            foreach (var pair in arr)
                Add(pair);
        }


        int CountNodes(TreeNode<TKey, TValue>? t)
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


        void TreeToArray(Array array, ref int arrayIndex, TreeNode<TKey, TValue> t)
        {
            if (t.left != null)
                TreeToArray(array, ref arrayIndex, t.left);

            array.SetValue(new KeyValuePair<TKey, TValue>(t.key, t.value), arrayIndex);
            arrayIndex += 1;

            if (t.right != null)
                TreeToArray(array, ref arrayIndex, t.right);
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            if (root == null)
                throw new ArgumentNullException();
            else
                TreeToArray(array, ref arrayIndex, root);
        }


        public void CloneTo(PrintingTree<TKey, TValue> another)
        {
            KeyValuePair<TKey, TValue>[] arr = new KeyValuePair<TKey, TValue>[Count];
            CopyTo(arr, 0);

            foreach (var tn in arr)
            {
                TKey key; TValue value;
                if (tn.Key is ICloneable)
                    key = (TKey) tn.Key.Clone();
                else key = tn.Key;

                if (tn.Key is ICloneable)
                    value = (TValue) tn.Value.Clone();
                else value = tn.Value;
                
                another.Add(key, value);
            }
        }


        TreeNode<TKey, TValue>? Find(TKey key, TreeNode<TKey, TValue>? t)
        {
            if (t == null)
                return null;

            if (t.key.Equals(key))
                return t;

            if (Find(key, t.left) == null)
                return Find(key, t.right);
            else
                return Find(key, t.left);
        }


        public TValue? FindValue(TKey key)
        {
            if (Find(key, root) != null)
                return Find(key, root).value;
            else return default(TValue);
        }


        void Delete(TreeNode<TKey, TValue>? t)
        {
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

        public virtual void Remove(object obj)
        {
            if (obj is TKey)
            {
                var key = (TKey)obj;
                TreeNode<TKey, TValue>? q = Find(key, root);
                if (q != null)
                {
                    Delete(q);
                    q = null;
                    GC.Collect();
                }
            }
        }

        public virtual bool Remove(KeyValuePair<TKey, TValue> pair)
        {
            TreeNode<TKey, TValue>? q = Find(pair.Key, root);
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


        public bool ContainsKey(TKey key)
        {
            if (Find(key, root) != null)
                return true;
            else
                return false;
        }

        public bool Contains(object obj)
        {
            if (obj is KeyValuePair<TKey, TValue>)
            {
                var pair = (KeyValuePair<TKey, TValue>)obj;
                if (Find(pair.Key, root) != null)
                    return Find(pair.Key, root).value.Equals(pair.Value);
            }
            return false;
        }


        public bool TryGetValue(TKey key, out TValue? value)
        {
            TreeNode<TKey, TValue> node = Find(key, root);
            if (node == null)
            {
                value = default(TValue);
                return false;
            }
            else
            {
                value = node.value;
                return true;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PrintingTreeEnumerator<TKey, TValue>(this);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new PrintingTreeEnumerator<TKey, TValue>(this);
        }


        public virtual object this[object key]
        {
            get
            {
                if (key is TKey)
                {
                    if (Find((TKey)key, root) == null)
                        throw new ArgumentOutOfRangeException();
                    else
                        return Find((TKey)key, root).value;
                }
                else throw new ArgumentException();
            }
            set
            {
                if (key is TKey)
                {
                    var tn = Find((TKey)key, root);
                    if (tn != null)
                        tn.value = (TValue?)value;
                    else
                        Add((TKey)key, (TValue?)value);
                }
                else throw new ArgumentException();
            }
        }


        void TreeToKeyArray(TKey[] array, ref int arrayIndex, TreeNode<TKey, TValue> t)
        {
            if (t.left != null)
                TreeToKeyArray(array, ref arrayIndex, t.left);

            array[arrayIndex] = t.key;
            arrayIndex += 1;

            if (t.right != null)
                TreeToKeyArray(array, ref arrayIndex, t.right);
        }

        public ICollection Keys
        {
            get
            {
                TKey[] keys = new TKey[this.Count];
                int i = 0;
                TreeToKeyArray(keys, ref i, root);
                return keys;
            }
        }


        void TreeToValueArray(TValue[] array, ref int arrayIndex, TreeNode<TKey, TValue> t)
        {
            if (t.left != null)
                TreeToValueArray(array, ref arrayIndex, t.left);

            array[arrayIndex] = t.value;
            arrayIndex += 1;

            if (t.right != null)
                TreeToValueArray(array, ref arrayIndex, t.right);
        }

        public ICollection Values
        {
            get
            {
                TValue[] values = new TValue[this.Count];
                int i = 0;
                TreeToValueArray(values, ref i, root);
                return values;
            }
        }

        bool IDictionary.IsFixedSize => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();
    }
}

