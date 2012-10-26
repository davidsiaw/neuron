using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using BlueBlocksLib.BaseClasses;

namespace BlueBlocksLib.TypeUtils {
	public class TypeTools {

		// An attempt to make reflection 'faster'
		// Fields will always be in order of declaration
		Dictionary<Type, FieldInfo[]> typeToField = new Dictionary<Type, FieldInfo[]>();

		public FieldInfo[] GetFieldsOfTypeInOrderOfDeclaration(Type objecttype) {

			FieldInfo[] fiSorted;

            if (!typeToField.ContainsKey(objecttype))
            {
                Dictionary<string, IntPtr> fieldorder = new Dictionary<string, IntPtr>();

                // HAX0R These comments are for the next two lines only.
                // we kill the cache msdn talks so much about that is the entire reason
                // field entries of a class cannot come back in order. This forces it to reload
                // the fieldinfo entries in the order that they were written in. If the
                // compiler changes to deliberately mess up the order of metadata entries of
                // fields inside a class, this will completely and utterly break. At which point
                // we will need to find better ways to do this.

                // The reason we do this is because 
                // 1) Marshal.OffsetOf will throw exceptions when a type A has a member b that
                // is of type A itself.
                //
                // 2) MS has no other method that guarantees we can
                // get the fieldinfo entries in the order of definition in the source code, 
                // which is so central to allowing this to work. It even goes as far as saying
                // that we should not depend on the order of definition.
                // http://msdn.microsoft.com/en-us/library/ch9714z3.aspx
                object cache = objecttype.GetType().GetProperty("Cache", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic).GetValue(objecttype, null);
                cache.GetType().GetField("m_fieldInfoCache", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic).SetValue(cache, null);

				fiSorted = objecttype.GetFields();
				typeToField[objecttype] = fiSorted;
			} else {
				fiSorted = typeToField[objecttype];
			}

			return fiSorted;
		}


		public static Handler MakeHardForwarder<TTarget>(TTarget target) {
			return new Handler((name, parms) => {
				return typeof(TTarget).GetMethod(name).Invoke(target, parms);
			});
		}

		public static Handler MakeSoftForwarder<TTarget>(TTarget target) {
			return new Handler((name, parms) => {
				MethodInfo mi = typeof(TTarget).GetMethod(name);
				if (mi != null) {
					return mi.Invoke(target, parms);
				}
				// Stub otherwise
				return null;
			});
		}

		public static TInterface HardCast<TInterface, TTarget>(TTarget target) {
			return new InterfaceImplementor<TInterface>(MakeHardForwarder(target)).Interface;
		}

		public static TInterface SoftCast<TInterface, TTarget>(TTarget target) {
			return new InterfaceImplementor<TInterface>(MakeSoftForwarder(target)).Interface;
		}

		public static TContainer XMLFunnel<TContainer>(XmlNode xml) where TContainer : new() {
			return (TContainer)XMLFunnel(xml, typeof(TContainer), new TypeTools());
		}

        public static void AsType<T>(object o, Action<T> action)
        {
            if (o is T)
            {
                action((T)o);
            }
        }

		static object XMLFunnel(XmlNode xml, Type type, TypeTools typetools) {

			object o;
			if (type == typeof(int)) {
				o = int.Parse(xml.InnerText);

			} else if (type == typeof(string)) {
				o = xml.InnerText;

			} else if (type == typeof(bool)) {
				o = bool.Parse(xml.InnerText);

			} else {
				o = Activator.CreateInstance(type);

				FieldInfo[] fis = typetools.GetFieldsOfTypeInOrderOfDeclaration(type);
				foreach (FieldInfo fi in fis) {

					if (fi.FieldType.IsArray) {
						XmlNodeList list = xml.SelectNodes(fi.Name);

						Array arr = Array.CreateInstance(fi.FieldType.GetElementType(), list.Count);
						for (int i = 0; i < arr.Length; i++) {
							XmlNode n = list[i];
							arr.SetValue(XMLFunnel(n, fi.FieldType.GetElementType(), typetools), i);
						}
						fi.SetValue(o, arr);
					} else {
						XmlNode n = xml.SelectSingleNode(fi.Name);
						if (n != null) {
							fi.SetValue(o, XMLFunnel(n, fi.FieldType, typetools));
						}
					}
				}
			}

			return o;
		}
	}
}
