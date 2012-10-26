// -----------------------------------------------------------------------
// <copyright file="SQLiteException.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BlueBlocksLib.Database.SQLite {
	using System;
	using System.Collections.Generic;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>

	public class SQLiteException : System.Exception {
		public SQLite3.Result Result { get; private set; }

		public SQLiteException(SQLite3.Result r, string message)
			: base(message) {
			Result = r;
		}

	}
}
