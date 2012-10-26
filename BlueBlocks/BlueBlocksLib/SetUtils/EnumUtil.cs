using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace BlueBlocksLib.SetUtils {

	public abstract class EnumUtil<T> where T : class {

		// This is meant to impose a type constraint on TEnum to be Enum (see EnumUtil below)
		public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : T {
			List<TEnum> enumerations = new List<TEnum>();
			object enumInstance = Activator.CreateInstance(typeof(TEnum));
			foreach (FieldInfo fieldInfo in typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public)) {
				enumerations.Add((TEnum)fieldInfo.GetValue(enumInstance));
			}
			return enumerations;
		}
	}

	public class EnumUtil : EnumUtil<Enum> {
		private EnumUtil() { }
	}
}
