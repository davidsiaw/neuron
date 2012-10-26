using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.SetUtils
{
    public static class ArrayUtils
    {
        public static T[] Flatten<T>(T[][] arrays)
        {
            List<T> arrayElems = new List<T>();
            foreach (T[] array in arrays)
            {
                arrayElems.AddRange(array);
            }
            return arrayElems.ToArray();
        }

        public static bool All<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            foreach (T item in collection)
            {
                if (!predicate(item))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Exists<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            foreach (T item in collection)
            {
                if (predicate(item))
                {
                    return true;
                }
            }
            return false;
        }

		public static bool AreEqual<T>(T[] collection, T[] collection2) {
			if (collection.Length != collection2.Length) {
				return false;
			}

			for (int i = 0; i < collection.Length; i++) {
				if (!collection[i].Equals(collection2[i])) {
					return false;
				}
			}
			return true;
		}

        public static int FindIndex<T>(T[] collection, Predicate<T> predicate)
        {
            for (int i = 0; i < collection.Length; i++)
            {
                if (predicate(collection[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public static T[] ToArray<T>(IEnumerable<T> collection)
        {
            List<T> result = new List<T>();
            foreach (var item in collection)
            {
                result.Add(item);
            }
            return result.ToArray();
        }

        // Takes the elements that have the lowest number returned by the ranker function
        public static T[] TakeTopRanking<T>(T[] array, Func<uint, T> ranker)
        {
            uint lowest = uint.MaxValue;
            for (int i = 0; i < array.Length; i++)
            {
                if (ranker(array[i]) < lowest)
                {
                    lowest = ranker(array[i]);
                }
            }

            T[] result = ArrayUtils.FindAll(array, x => ranker(x) == lowest);
            return result;
        }

        public static bool IsNullOrEmpty(Array array)
        {
            return array == null || array.Length == 0;
        }

        public static T Find<T>(T[] array, Predicate<T> compare)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (compare(array[i]))
                {
                    return array[i];
                }
            }
            return default(T);
        }

        public static void ForEach<T>(IEnumerable<T> array, Action<T> action)
        {
            foreach (T item in array)
            {
                action(item);
            }
        }

        public static void Remove<TSource>(List<TSource> list, Predicate<TSource> remover)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (remover(list[i]))
                {
                    list.RemoveAt(i);
                    i = 0;
                }
            }
        }

        // Find out if a contains elements that are present in b
        public static bool Intersects<TSource>(IEnumerable<TSource> a, IEnumerable<TSource> b)
        {
            foreach (var elem in a)
            {
                foreach (var elem2 in b)
                {
                    if (elem.Equals(elem2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool TryFind<TSource>(IEnumerable<TSource> haystack, Predicate<TSource> pred, out TSource needle)
        {
            foreach (TSource o in haystack)
            {
                if (pred(o))
                {
                    needle = o;
                    return true;
                }
            }
            needle = default(TSource);
            return false;
        }

        public static bool Contains<TSource>(IEnumerable<TSource> source, TSource value)
        {
            foreach (TSource o in source)
            {
                if (o.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public static T[] FindAll<T>(T[] array, Predicate<T> match)
        {

            if (match == null)
            {
                throw new ArgumentException("Predicate must not be null");
            }

            List<T> list = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                if (match(array[i]))
                {
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Takes an array and calls converter on all its elements, and returns an array of the same size that
        /// contains the return value of converter for each element in the array
        /// </summary>
        /// <typeparam name="TSource">The source array type</typeparam>
        /// <typeparam name="TResult">The result array type</typeparam>
        /// <param name="array">The array whose elements are to be converted</param>
        /// <param name="converter">The function to call for each array element</param>
        /// <returns>An array of elements returned by the converter</returns>
        public static TResult[] ConvertAll<TSource, TResult>(TSource[] array, Func<TResult, TSource> converter)
        {
            TResult[] res = new TResult[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                res[i] = converter(array[i]);
            }

            return res;
        }

        public static T[] Insert<T>(T first, T[] rest)
        {
            List<T> result = new List<T>();
            result.Add(first);
            result.AddRange(rest);
            return result.ToArray();
        }

        public static T[] Concat<T>(T[] init, T[] rest)
        {
            List<T> result = new List<T>();
            result.AddRange(init);
            result.AddRange(rest);
            return result.ToArray();
        }

        public static T[] Prepend<T>(T head, T[] tail)
        {
            return Concat(new T[] { head }, tail);
        }

        public static T[] Append<T>(T[] init, T last)
        {
            return Concat(init, new T[] { last });
        }
    }
}
