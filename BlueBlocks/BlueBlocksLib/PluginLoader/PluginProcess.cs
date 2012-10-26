using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.CodeDom.Compiler;
using System.IO;
using BlueBlocksLib.SetUtils;
using Microsoft.CSharp;

namespace BlueBlocksLib.PluginLoader {

	public class PluginProcess<TInterface> : IDisposable {

		Process proc = new Process();

		TcpListener listener;
		TcpClient client;

		static bool IsPortAvailable(int port) {
			IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
			TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

			foreach (TcpConnectionInformation tcpi in tcpConnInfoArray) {
				if (tcpi.LocalEndPoint.Port == port) {
					return false;
				}
			}
			return true;
		}

		public PluginProcess(string path, string name) {
			int port = 6901;
			while (!IsPortAvailable(port)) {
				port++;
			}

			listener = new TcpListener(port);
			proc.StartInfo = new ProcessStartInfo("PluginHost.exe", "\"" + Path.GetFullPath(path) + "\" " + port + " " + typeof(TInterface).Name);

			
			PropertyInfo[] pis = typeof(TInterface).GetProperties();
			MethodInfo[] mis = 
				
				ArrayUtils.FindAll(typeof(TInterface).GetMethods(), mi=>
				!ArrayUtils.Exists(pis, pi =>
					"get_" + pi.Name == mi.Name ||
					"set_" + pi.Name == mi.Name)
				);

			string source = @"
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using GeocodeComparer;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace PluginProcess {
	public class Implementation : " + typeof(TInterface).Name + @" {

		TcpClient client;
		public Implementation(TcpClient client) {
			this.client = client;
		}

		void Read(byte[] resp, int length) {
			int amtread = 0;
			while (amtread < length) {
				amtread += client.GetStream().Read(resp, amtread, length - amtread);
			}
			Debug.Assert(amtread == length);
		}

		"
	+ string.Join("",
		  ArrayUtils.ConvertAll(mis, mi => ImplementMethod(mi)))

	+ string.Join("",
		  ArrayUtils.ConvertAll(pis, pi => ImplementProperty(pi))) + @"
	}
}
";
			CSharpCodeProvider cs = new CSharpCodeProvider();
			CompilerParameters cp = new CompilerParameters();
			cp.IncludeDebugInformation = true;
			cp.TempFiles.KeepFiles = true;
			var names = Assembly.GetCallingAssembly().GetReferencedAssemblies();
			cp.ReferencedAssemblies.Add(Assembly.GetCallingAssembly().Location);
			foreach (var asmname in names) {
				Assembly refasm = Assembly.Load(asmname);
				cp.ReferencedAssemblies.Add(refasm.Location);
			}

			var result = cs.CompileAssemblyFromSource(cp, source);
			var asm = result.CompiledAssembly;

			Type imp = asm.GetType("PluginProcess.Implementation");

			listener.Start();
			proc.Start();
			client = listener.AcceptTcpClient();
			TInterface intf = (TInterface)Activator.CreateInstance(imp, client);
			Plugin = intf;

		}

		public TInterface Plugin { get; private set; }



		string ImplementParameter(ParameterInfo pi) {
			string type = SanitizeParamType(pi);
			return type + " " + pi.Name;
		}

		private static string SanitizeParamType(ParameterInfo pi) {
			string type = pi.ParameterType.Name;
			if (pi.ParameterType.IsByRef) {
				type = (pi.IsOut ? "out " : "ref ") + type.Replace("&", "");
			}
			return type;
		}

		string SanitizeReturnType(Type t) {
			if (t == typeof(void)) {
				return "void";
			}
			return t.Name;
		}

		string ImplementProperty(PropertyInfo pi) {


			string source = "\r\n\r\npublic "
				+ pi.PropertyType.Name + " "
				+ pi.Name + " "
				+ "{";

			MethodInfo getMi = pi.GetGetMethod();
			MethodInfo setMi = pi.GetSetMethod();

			if (getMi != null) {
				source += "get {"
					+ "BinaryFormatter ser = new BinaryFormatter();"

					+ "Call c = new Call();\r\n"
					+ "c.name = \"" + pi.Name + "\";\r\n"
					+ "c.type = CallType.Property;\r\n"
					+ "c.param = new List<object>();\r\n"

					+ "\r\nusing (MemoryStream ms = new MemoryStream()) {\r\n"
					+ "ser.Serialize(ms, c);\r\n"
					+ "client.GetStream().Write(BitConverter.GetBytes(ms.GetBuffer().Length), 0, 4);\r\n"
					+ "client.GetStream().Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);\r\n"
					+ "}\r\n";

				source += @"
				byte[] respLen = new byte[4];
				Read(respLen, 4);

				int respLength = BitConverter.ToInt32(respLen, 0);
				byte[] resp = new byte[respLength];
				Read(resp, respLength);

				using (MemoryStream ms = new MemoryStream(resp)) {
					object o = ser.Deserialize(ms);
					return (" + pi.PropertyType.Name + @")o;
				}";

				source += "}";
			}

			if (setMi != null) {
				source += "set {"
					+ "BinaryFormatter ser = new BinaryFormatter();"

					+ "Call c = new Call();\r\n"
					+ "c.name = \"" + pi.Name + "\";\r\n"
					+ "c.type = CallType.Property;\r\n"
					+ "c.param = new List<object>();\r\n"
					+ "c.param.Add(value);\r\n"

					+ "\r\nusing (MemoryStream ms = new MemoryStream()) {\r\n"
					+ "ser.Serialize(ms, c);\r\n"
					+ "client.GetStream().Write(BitConverter.GetBytes(ms.GetBuffer().Length), 0, 4);\r\n"
					+ "client.GetStream().Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);\r\n"
					+ "}\r\n";
			}

			source += "}";

			return source;
		}

		string ImplementMethod(MethodInfo mi) {

			bool returns = mi.ReturnType != typeof(void) ||
				ArrayUtils.Exists(mi.GetParameters(), pi => pi.IsOut || pi.ParameterType.IsByRef);

			string source = "\r\n\r\npublic "
				+ SanitizeReturnType(mi.ReturnType) + " "
				+ mi.Name + "("
					+ string.Join(", ", ArrayUtils.ConvertAll(mi.GetParameters(), pi => ImplementParameter(pi)))
				+ ") {"
					+ "BinaryFormatter ser = new BinaryFormatter();"

					+ "Call c = new Call();\r\n"
					+ "c.name = \"" + mi.Name + "\";\r\n"
					+ "c.type = CallType.Method;\r\n"
					+ "c.param = new List<object>();\r\n"
					+ string.Join("\r\n", ArrayUtils.ConvertAll(mi.GetParameters(), pi =>

						pi.IsOut ?

						"c.param.Add(null);\r\n"

						:

						"c.param.Add(" + pi.Name + ");\r\n"))

					+ "\r\nusing (MemoryStream ms = new MemoryStream()) {\r\n"
					+ "ser.Serialize(ms, c);\r\n"
					+ "client.GetStream().Write(BitConverter.GetBytes(ms.GetBuffer().Length), 0, 4);\r\n"
					+ "client.GetStream().Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);\r\n"
					+ "}\r\n";



			if (returns) {
				source += @"
					byte[] respLen = new byte[4];
					Read(respLen, 4);

					int respLength = BitConverter.ToInt32(respLen, 0);
					byte[] resp = new byte[respLength];
					Read(resp, respLength);

					using (MemoryStream ms = new MemoryStream(resp)) {
						Return ret = (Return)ser.Deserialize(ms);";

				ParameterInfo[] pis = mi.GetParameters();
				for (int i = 0; i < pis.Length; i++) {
					ParameterInfo pi = pis[i];
					if (pi.IsOut || pi.ParameterType.IsByRef) {
						source += pi.Name + " = (" + pi.ParameterType.Name.Replace("&", "") + ")ret.outparms[" + i + "];";
					}
				}

				if (mi.ReturnType != typeof(void)) {
					source += @"
						return (" + mi.ReturnType.Name + @")ret.retVal;";
				}

				source += "}";

			}


			source += "}";

			return source;

		}

		#region IDisposable Members

		public void Dispose() {
			listener.Stop();
			proc.Kill();
		}

		#endregion

		public string Pathpath { get; set; }
	}

		
}
