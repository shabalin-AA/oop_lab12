using System;
using lab10;
using System.Collections;

namespace lab12
{
    public class PrintingTreeEnumerator : IEnumerator<KeyValuePair<string, Printing>>
    {
        int i = -1;
        int count;
        KeyValuePair<string, Printing>[] arr;


        public PrintingTreeEnumerator(PrintingTree t)
        {
            count = t.Count;
            arr = new KeyValuePair<string, Printing>[count];
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


        public KeyValuePair<string, Printing> Current
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

