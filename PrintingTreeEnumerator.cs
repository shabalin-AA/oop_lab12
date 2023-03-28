using System;
using lab10;
using System.Collections;

namespace lab12
{
    public class PrintingTreeEnumerator<TKey, TValue> : IDictionaryEnumerator
        where TKey : ICloneable
        where TValue : ICloneable
    {
        int i = -1;
        int count;
        KeyValuePair<TKey, TValue>[] arr;


        public PrintingTreeEnumerator(PrintingTree<TKey, TValue> t)
        {
            count = t.Count;
            arr = new KeyValuePair<TKey, TValue>[count];
            t.CopyTo(arr, 0);
        }


        public object Key
        {
            get { return arr[i].Key; }
        }


        public object? Value
        {
            get { return arr[i].Value; }
        }


        public DictionaryEntry Entry
        {
            get
            {
                return new DictionaryEntry(arr[i].Key, arr[i].Value);
            }
        }


        object? IEnumerator.Current
        {
            get { return arr[i]; }
        }


        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                return arr[i];
            }
        }


        public bool MoveNext()
        {
            if (i < count - 1)
            {
                i++;
                return true;
            }
            else
                return false;
        }


        public void Reset() { i = -1; }


        public void Dispose() { }
    }
}

