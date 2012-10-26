using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using BlueBlocksLib.SetUtils;
using BlueBlocksLib.BaseClasses;

namespace BlueBlocksLib.TypeUtils {


	public class InterfaceImplementor<TInterface> : IImplements<TInterface>, IDisposable {

		public InterfaceImplementor(Handler handler) {

			Type intf = typeof(TInterface);
			string intname = intf.Name;

			if (!intf.IsInterface) {
				throw new ArgumentException("TInterface is not an interface");
			}

			AssemblyName asmName = new AssemblyName();
			asmName.Name = "Temp" + intname + "Implementation";

			AppDomain ad = AppDomain.CurrentDomain;
			AssemblyBuilder ab = ad.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
			ModuleBuilder mb = ab.DefineDynamicModule(asmName.Name);
			TypeBuilder tb = mb.DefineType(
				asmName.Name + "Impl",
				TypeAttributes.Class,
				typeof(MarshalByRefObject),
				new Type[] { intf });

			FieldBuilder methodHandler = tb.DefineField(
				"handler",
				typeof(Handler),
				FieldAttributes.Private);

			ConstructorInfo baseConstructorInfo = typeof(object).GetConstructor(new Type[0]);
			ConstructorBuilder cb = tb.DefineConstructor(
				MethodAttributes.Public,
				CallingConventions.Standard,
				new Type[] { typeof(Handler) });

			// Make the constructor
			ILGenerator gen = cb.GetILGenerator();


			gen.Emit(OpCodes.Ldarg_0);
			gen.Emit(OpCodes.Call, baseConstructorInfo);

			gen.Emit(OpCodes.Ldarg_0);
			gen.Emit(OpCodes.Ldarg_1);
			gen.Emit(OpCodes.Stfld, methodHandler);
			gen.Emit(OpCodes.Ret);


			foreach (MethodInfo mi in intf.GetMethods()) {
				ParameterInfo[] pis = mi.GetParameters();

				MethodBuilder method = tb.DefineMethod(
					mi.Name,
					MethodAttributes.Public |
					MethodAttributes.Virtual |
					MethodAttributes.Final |
					MethodAttributes.HideBySig |
					MethodAttributes.NewSlot,
					CallingConventions.Standard,
					mi.ReturnType,
					ArrayUtils.ConvertAll(pis, x => x.ParameterType));

				ILGenerator mgen = method.GetILGenerator();

				LocalBuilder lb = mgen.DeclareLocal(typeof(object[]));


				mgen.Emit(OpCodes.Nop);
				mgen.Emit(OpCodes.Ldarg_0);
				mgen.Emit(OpCodes.Ldfld, methodHandler);
				mgen.Emit(OpCodes.Ldstr, mi.Name);


				// Make the object array

				if (pis.Length == 0) {
					mgen.Emit(OpCodes.Ldc_I4_0);
				} else if (pis.Length == 1) {
					mgen.Emit(OpCodes.Ldc_I4_1);
				} else if (pis.Length == 2) {
					mgen.Emit(OpCodes.Ldc_I4_2);
				} else if (pis.Length == 3) {
					mgen.Emit(OpCodes.Ldc_I4_3);
				} else if (pis.Length == 4) {
					mgen.Emit(OpCodes.Ldc_I4_4);
				} else if (pis.Length == 5) {
					mgen.Emit(OpCodes.Ldc_I4_5);
				} else if (pis.Length == 6) {
					mgen.Emit(OpCodes.Ldc_I4_6);
				} else if (pis.Length == 7) {
					mgen.Emit(OpCodes.Ldc_I4_7);
				} else if (pis.Length == 8) {
					mgen.Emit(OpCodes.Ldc_I4_8);
				} else {
					mgen.Emit(OpCodes.Ldc_I4, pis.Length);
				}
				mgen.Emit(OpCodes.Newarr, typeof(object));
				mgen.Emit(OpCodes.Stloc_0);

				for (int i = 0; i < pis.Length; i++) {
					// Load the object array
					mgen.Emit(OpCodes.Ldloc_0);

					if (i == 0) {
						mgen.Emit(OpCodes.Ldc_I4_0);
						mgen.Emit(OpCodes.Ldarg_1);
					} else {
						mgen.Emit(OpCodes.Ldc_I4, i);
						mgen.Emit(OpCodes.Ldarg, i + 1);
					}

					if (pis[i].ParameterType.IsValueType) {
						mgen.Emit(OpCodes.Box, pis[i].ParameterType);
					}
					mgen.Emit(OpCodes.Stelem_Ref);
				}

				mgen.Emit(OpCodes.Ldloc_0);

				mgen.EmitCall(
					OpCodes.Callvirt,
					typeof(Handler).GetMethod("Invoke"),
					new Type[] { typeof(object) });

				if (mi.ReturnType == typeof(void)) {
					mgen.Emit(OpCodes.Pop);
				}

				mgen.Emit(OpCodes.Ret);

				tb.DefineMethodOverride(method, mi);

			}

			Type t = tb.CreateType();

			Interface = (TInterface)Activator.CreateInstance(t, handler);
		}

		public TInterface Interface {
			get;
			private set;
		}

		#region IDisposable Members

		public void Dispose() {
		}

		#endregion
	}

}
