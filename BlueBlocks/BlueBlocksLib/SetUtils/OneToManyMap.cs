using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.SetUtils
{

    public class OneToManyMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>>
    {
        Dictionary<TKey, Dictionary<TValue, bool>> map = new Dictionary<TKey, Dictionary<TValue,bool>>();

        public IEnumerable<TValue> this[TKey key]
        {
            get
            {
                return map[key].Keys;
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (!map.ContainsKey(key))
            {
                map[key] = new Dictionary<TValue, bool>();
            }
            map[key][value] = true;
        }

        public IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>> GetEnumerator()
        {
            foreach (var kvpair in map)
            {
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(kvpair.Key, kvpair.Value.Keys);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
