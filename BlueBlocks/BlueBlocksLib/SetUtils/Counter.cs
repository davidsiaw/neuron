using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.SetUtils
{
    public class Counter<T> : IEnumerable<KeyValuePair<T, int>>
    {
        Dictionary<T, int> counter = new Dictionary<T, int>();
        public Counter()
        {
        }

        public void Increment(T item)
        {
            if (!counter.ContainsKey(item))
            {
                counter[item] = 0;
            }
            counter[item]++;
        }

        public int this[T item]
        {
            get
            {
                return counter[item];
            }
        }

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
        {
            return counter.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return counter.GetEnumerator();
        }
    }
}
