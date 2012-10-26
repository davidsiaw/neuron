// -----------------------------------------------------------------------
// <copyright file="SQLite3.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BlueBlocksLib.Database.SQLite {
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Runtime.InteropServices;

	/// <summary>
	/// stolen from sqlite.net and augmented
	/// </summary>

	public static class SQLite3 {
		public enum Result : int {
			OK = 0,
			Error = 1,
			Internal = 2,
			Perm = 3,
			Abort = 4,
			Busy = 5,
			Locked = 6,
			NoMem = 7,
			ReadOnly = 8,
			Interrupt = 9,
			IOError = 10,
			Corrupt = 11,
			NotFound = 12,
			TooBig = 18,
			Constraint = 19,
			Row = 100,
			Done = 101
		}

		public enum ConfigOption : int {
			SingleThread = 1,
			MultiThread = 2,
			Serialized = 3
		}

		public enum DestructorBehaviour {
			SQLITE_STATIC = 0,
			SQLITE_TRANSIENT = -1
		}

		[DllImport("sqlite3", EntryPoint = "sqlite3_open")]
		public static extern Result Open(string filename, out IntPtr db);

		[DllImport("sqlite3", EntryPoint = "sqlite3_open_v2")]
		public static extern Result Open(string filename, out IntPtr db, SQLiteFlags flags, IntPtr vfs);

		[DllImport("sqlite3", EntryPoint = "sqlite3_close")]
		public static extern Result Close(IntPtr db);

		[DllImport("sqlite3", EntryPoint = "sqlite3_config")]
		public static extern Result Config(ConfigOption option);

		[DllImport("sqlite3", EntryPoint = "sqlite3_busy_timeout")]
		public static extern Result BusyTimeout(IntPtr db, int milliseconds);

		[DllImport("sqlite3", EntryPoint = "sqlite3_changes")]
		public static extern int Changes(IntPtr db);

		[DllImport("sqlite3", EntryPoint = "sqlite3_prepare_v2")]
		public static extern Result Prepare2(IntPtr db, string sql, int numBytes, out IntPtr stmt, IntPtr pzTail);

		[DllImport("sqlite3", EntryPoint = "sqlite3_step")]
		public static extern Result Step(IntPtr stmt);

		[DllImport("sqlite3", EntryPoint = "sqlite3_reset")]
		public static extern Result Reset(IntPtr stmt);

		[DllImport("sqlite3", EntryPoint = "sqlite3_finalize")]
		public static extern Result Finalize(IntPtr stmt);

		[DllImport("sqlite3", EntryPoint = "sqlite3_last_insert_rowid")]
		public static extern long LastInsertRowid(IntPtr db);

		[DllImport("sqlite3", EntryPoint = "sqlite3_errmsg16")]
		public static extern IntPtr Errmsg(IntPtr db);

		public static Result StepAndCheck(IntPtr stmt) {
			var res = Step(stmt);
			return res;
		}

		public static string GetErrmsg(IntPtr db) {
			if (db == IntPtr.Zero) {
				return "Could not open database file";
			}
			return Marshal.PtrToStringUni(Errmsg(db));
		}

		public static void CheckResult(IntPtr db, Result res){
			if (res != Result.OK && res != Result.Done){
				throw new SQLiteException(res, GetErrmsg(db));
			}
		}

		[DllImport("sqlite3", EntryPoint = "sqlite3_bind_parameter_index")]
		public static extern int BindParameterIndex(IntPtr stmt, string name);

		[DllImport("sqlite3", EntryPoint = "sqlite3_bind_null")]
		public static extern int BindNull(IntPtr stmt, int index);

		[DllImport("sqlite3", EntryPoint = "sqlite3_bind_int")]
		public static extern int BindInt(IntPtr stmt, int index, int val);

		[DllImport("sqlite3", EntryPoint = "sqlite3_bind_int64")]
		public static extern int BindInt64(IntPtr stmt, int index, long val);

		[DllImport("sqlite3", EntryPoint = "sqlite3_bind_double")]
		public static extern int BindDouble(IntPtr stmt, int index, double val);

		[DllImport("sqlite3", EntryPoint = "sqlite3_bind_text")]
		public static extern int BindText(IntPtr stmt, int index, byte[] val, int n, DestructorBehaviour free);

		[DllImport("sqlite3", EntryPoint = "sqlite3_bind_blob")]
		public static extern int BindBlob(IntPtr stmt, int index, byte[] val, int n, DestructorBehaviour free);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_count")]
		public static extern int ColumnCount(IntPtr stmt);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_name")]
		public static extern IntPtr ColumnName(IntPtr stmt, int index);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_name16")]
		public static extern IntPtr ColumnName16(IntPtr stmt, int index);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_type")]
		public static extern ColType ColumnType(IntPtr stmt, int index);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_int")]
		public static extern int ColumnInt(IntPtr stmt, int index);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_int64")]
		public static extern long ColumnInt64(IntPtr stmt, int index);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_double")]
		public static extern double ColumnDouble(IntPtr stmt, int index);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_text")]
		public static extern IntPtr ColumnText(IntPtr stmt, int index);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_text16")]
		public static extern IntPtr ColumnText16(IntPtr stmt, int index);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_blob")]
		public static extern IntPtr ColumnBlob(IntPtr stmt, int index);

		[DllImport("sqlite3", EntryPoint = "sqlite3_column_bytes")]
		public static extern int ColumnBytes(IntPtr stmt, int index);

		public static string ColumnString(IntPtr stmt, int index) {
			return Marshal.PtrToStringUni(SQLite3.ColumnText16(stmt, index));
		}

		public static byte[] ColumnByteArray(IntPtr stmt, int index) {
			int length = ColumnBytes(stmt, index);
			byte[] result = new byte[length];
			if (length > 0)
				Marshal.Copy(ColumnBlob(stmt, index), result, 0, length);
			return result;
		}

		public enum ColType : int {
			INTEGER = 1,
			FLOAT = 2,
			TEXT = 3,
			BLOB = 4,
			NULL = 5
		}

		public delegate Result BindFunc(IntPtr stmt, int index, object val);
		public delegate object ReadFunc(IntPtr stmt, int index);
		static Dictionary<ColType, BindFunc> colTypeToFunc = new Dictionary<ColType, BindFunc>();
		static Dictionary<Type, ReadFunc> typeToReadFunc = new Dictionary<Type, ReadFunc>();
		static Dictionary<Type, ColType> typeToColtype = new Dictionary<Type, ColType>();
		static SQLite3() {
			typeToColtype[typeof(float)] = ColType.FLOAT;
			typeToColtype[typeof(double)] = ColType.FLOAT;
			typeToColtype[typeof(int)] = ColType.INTEGER;
			typeToColtype[typeof(short)] = ColType.INTEGER;
			typeToColtype[typeof(long)] = ColType.INTEGER;
			typeToColtype[typeof(uint)] = ColType.INTEGER;
			typeToColtype[typeof(ushort)] = ColType.INTEGER;
			typeToColtype[typeof(ulong)] = ColType.INTEGER;
			typeToColtype[typeof(byte)] = ColType.INTEGER;
			typeToColtype[typeof(bool)] = ColType.INTEGER;
			typeToColtype[typeof(byte[])] = ColType.BLOB;
			typeToColtype[typeof(string)] = ColType.TEXT;

			colTypeToFunc[ColType.BLOB] = (stmt, index, val) => (Result)BindBlob(stmt, index, (byte[])val, ((byte[])val).Length, DestructorBehaviour.SQLITE_TRANSIENT);
			colTypeToFunc[ColType.INTEGER] = (stmt, index, val) => {
				if (val.GetType().IsEnum) {
					return (Result)BindInt64(stmt, index, (int)Enum.Parse(val.GetType(), val.ToString()));
				}
				if (val.GetType() == typeof(bool)) {
					return (Result)BindInt64(stmt, index, (bool)val ? 1 : 0);
				}
				long value = long.Parse(val.ToString());
				return (Result)BindInt64(stmt, index, value); 
			};
			colTypeToFunc[ColType.FLOAT] = (stmt, index, val) => (Result)BindDouble(stmt, index, (double)val);
			colTypeToFunc[ColType.TEXT] = (stmt, index, val) =>  {
				byte[] bytes = Encoding.UTF8.GetBytes((string)val ?? "");
				return (Result)BindText(stmt, index, bytes, bytes.Length, DestructorBehaviour.SQLITE_TRANSIENT);
			};

			typeToReadFunc[typeof(byte[])] = (stmt, index) => ColumnByteArray(stmt, index);
			typeToReadFunc[typeof(int)] = (stmt, index) => (int)ColumnInt64(stmt, index);
			typeToReadFunc[typeof(short)] = (stmt, index) => (short)ColumnInt64(stmt, index);
			typeToReadFunc[typeof(long)] = (stmt, index) => ColumnInt64(stmt, index);
			typeToReadFunc[typeof(uint)] = (stmt, index) => (uint)ColumnInt64(stmt, index);
			typeToReadFunc[typeof(ushort)] = (stmt, index) => (ushort)ColumnInt64(stmt, index);
			typeToReadFunc[typeof(ulong)] = (stmt, index) => (ulong)ColumnInt64(stmt, index);
			typeToReadFunc[typeof(byte)] = (stmt, index) => (byte)ColumnInt64(stmt, index);
			typeToReadFunc[typeof(float)] = (stmt, index) => (float)ColumnDouble(stmt, index);
			typeToReadFunc[typeof(double)] = (stmt, index) => ColumnDouble(stmt, index);
			typeToReadFunc[typeof(string)] = (stmt, index) => ColumnString(stmt, index);
			typeToReadFunc[typeof(bool)] = (stmt, index) => ColumnInt(stmt, index) == 0 ? false : true;
		}

		public static BindFunc GetBindFunc(ColType coltype) {
			return colTypeToFunc[coltype];
		}

		public static ReadFunc GetReadFunc(Type type) {
			if (type.IsEnum) {
				return typeToReadFunc[typeof(int)];
			}
			return typeToReadFunc[type];
		}

		public static ColType GetSQLType(Type type) {
			if (type.IsEnum) {
				return typeToColtype[typeof(int)];
			}
			return typeToColtype[type];
		}

		public static IntPtr Prepare2(IntPtr db, string query) {
			IntPtr stmt;
			var r = Prepare2(db, query, query.Length, out stmt, IntPtr.Zero);
			if (r != Result.OK) {
				throw new SQLiteException(r, GetErrmsg(db));
			}
			return stmt;
		}
	}
}
