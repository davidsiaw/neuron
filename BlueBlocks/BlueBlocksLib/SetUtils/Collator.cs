using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.SetUtils {

	abstract class Collator<T> {
		protected Dictionary<T, int> reverseLookup = new Dictionary<T, int>();
		protected List<T> forwardLookup = new List<T>();

		public Collator() { }

		public Collator(Collator<T> parent) {
			reverseLookup = new Dictionary<T, int>(parent.reverseLookup);
			forwardLookup = new List<T>(parent.forwardLookup);
		}

		public int this[T item] {
			get {
				if (reverseLookup.ContainsKey(item)) {
					return reverseLookup[item];
				}
				return ItemNotFound(item);
			}
		}

		protected abstract int ItemNotFound(T item);

		public T this[int item] {
			get {
				return forwardLookup[item];
			}
		}

		public int Count {
			get {
				return forwardLookup.Count;
			}
		}
	}

	class ExpandingCollator<T> : Collator<T> {
		protected override int ItemNotFound(T item) {
			int index = forwardLookup.Count;
			reverseLookup.Add(item, index);
			forwardLookup.Add(item);
			return index;
		}
	}

	class FixedCollator<T> : Collator<T> {

		public FixedCollator(Collator<T> coll)
			: base(coll) { }

		public const int InvalidIndex = -1;
		protected override int ItemNotFound(T item) {
			return InvalidIndex;
		}
	}
}
