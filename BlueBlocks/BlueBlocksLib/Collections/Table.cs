using System;
using System.Collections.Generic;
using System.Text;
using BlueBlocksLib.BaseClasses;

namespace BlueBlocksLib.Collections {

	class Table<TRows, TColumns, TVal> {
		Dictionary<Pair<TRows, TColumns>, TVal> table = new Dictionary<Pair<TRows, TColumns>, TVal>();

		public Table() { }

		public TVal this[TRows rows, TColumns columns] {
			get {
				Pair<TRows, TColumns> key = Makekey(rows, columns);
				if (table.ContainsKey(key)) {
					return table[key];
				}
				return default(TVal);
			}
			set {
				Pair<TRows, TColumns> key = Makekey(rows, columns);
				table[key] = value;
			}
		}

		private static Pair<TRows, TColumns> Makekey(TRows rows, TColumns columns) {
			Pair<TRows, TColumns> key = new Pair<TRows, TColumns>() {
				a = rows,
				b = columns
			};
			return key;
		}
	}
}
