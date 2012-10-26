using System;
using System.Collections.Generic;
using System.Text;
using BlueBlocksLib.SetUtils;
using BlueBlocksLib.BaseClasses;

namespace BlueBlocksLib.Collections
{
    public class Set<T> : ICollection<T>
    {
        Dictionary<T, bool> set = new Dictionary<T, bool>();

        public void Add(T item)
        {
            set[item] = true;
        }

        public void Clear()
        {
            set.Clear();
        }

        public bool Contains(T item)
        {
            return set.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array = ArrayUtils.ToArray(set.Keys);
        }

        public int Count
        {
            get { return set.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return set.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return set.Keys.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return set.Keys.GetEnumerator();
        }
    }
}
