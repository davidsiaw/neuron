using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using BlueBlocksLib.Endianness;

namespace BlueBlocksLib.FileAccess
{
    public class FormattedReader : DisposableStream
    {

        public BinaryReader BaseStream
        {
            get { return (BinaryReader)m_stream; }
        }

        public FormattedReader(Stream stream)
        {
            m_stream = new BinaryReader(stream);
        }

        public readonly string Filename;

        public FormattedReader(string filename)
        {
            byte[] bytes = File.ReadAllBytes(filename);

            m_stream = new BinaryReader(new MemoryStream(bytes));

            Filename = filename;
        }

        public T Read<T>() where T : new()
        {
            T o = new T();
            object obj = (object)o;
            Read(ref obj, true);
            return (T)obj;
        }

        public bool EndOfStream
        {
            get
            {
                return ((BinaryReader)m_stream).BaseStream.Position == ((BinaryReader)m_stream).BaseStream.Length;
            }
        }

        void Read(ref object o, bool isLittleEndian)
        {
            BinaryReader m_stream = (BinaryReader)this.m_stream;
            Type t = o.GetType();
            StructLayoutAttribute structlayout = t.StructLayoutAttribute;

            EndianBitConverter b;
            if (!isLittleEndian)
            {
                b = new BigEndianBitConverter();
            }
            else
            {
                b = new LittleEndianBitConverter();
            }

            if (t == typeof(int))
            {
                o = b.ToInt32(m_stream.ReadBytes(4), 0);

            }
            else if (t.IsEnum)
            {
                o = b.ToInt32(m_stream.ReadBytes(4), 0);

            }
            else if (t == typeof(uint))
            {
                o = b.ToUInt32(m_stream.ReadBytes(4), 0);

            }
            else if (t == typeof(short))
            {
                o = b.ToInt16(m_stream.ReadBytes(2), 0);

            }
            else if (t == typeof(ushort))
            {
                o = b.ToUInt16(m_stream.ReadBytes(2), 0);

            }
            else if (t == typeof(byte))
            {
                o = m_stream.ReadBytes(1)[0];

            }
            else if (t == typeof(long))
            {
                o = b.ToInt64(m_stream.ReadBytes(8), 0);

            }
            else if (t == typeof(ulong))
            {
                o = b.ToUInt64(m_stream.ReadBytes(8), 0);

            }
            else if (t == typeof(float))
            {
                o = b.ToSingle(m_stream.ReadBytes(4), 0);

            }
            else if (t == typeof(double))
            {
                o = b.ToDouble(m_stream.ReadBytes(8), 0);


            }
            else if (t.IsArray)
            {
                if (t.GetElementType() == typeof(byte))
                {
                    byte[] arr = (byte[])o;
                    m_stream.Read(arr, 0, arr.Length);
                }
                else
                {
                    ReadIntoArray(o, isLittleEndian);
                }

            }
            else if (structlayout != null)
            {
                ReadIntoStruct(o, m_stream);

            }
            else
            {
                throw new Exception("I don't know how to write this object:" + o.GetType().Name + " Please include the [StructLayout] attribute");
            }

        }

        private void ReadIntoArray(object o, bool isLittleEndian)
        {
            Array arr = (Array)o;

            object element = Activator.CreateInstance(o.GetType().GetElementType());

            for (int i = 0; i < arr.Length; i++)
            {
                Read(ref element, isLittleEndian);
                arr.SetValue(element, i);
            }
        }

        private void ReadIntoStruct(object o, BinaryReader m_stream)
        {

            Dictionary<FieldInfo, OffsetAttribute> fieldsWithSpecifiedOffsets = new Dictionary<FieldInfo, OffsetAttribute>();
            Type objecttype = o.GetType();
            FieldInfo[] fis = objecttype.GetFields();

            // Sort the fields into the order they were specified
            SortFieldInfoArrayByOrderSpecified(objecttype, fis);

            List<object> towrite = new List<object>();

            // Read the laid out fields
            foreach (FieldInfo fi in fis)
            {
                OffsetAttribute[] offset = (OffsetAttribute[])fi.GetCustomAttributes(typeof(OffsetAttribute), false);
                InternalUseAttribute[] internalUse = (InternalUseAttribute[])fi.GetCustomAttributes(typeof(InternalUseAttribute), false);

                if (internalUse.Length > 0)
                {
                    continue;
                }

                if (offset.Length > 0)
                {
                    // Defer reading this field until we are done with all the laid-out ones
                    fieldsWithSpecifiedOffsets[fi] = offset[0];
                    continue;
                }

                ReadIntoField(o, fi);
            }

            // Save our position
            long endofstruct = m_stream.BaseStream.Position;

            // Read the field which have specific offsets specified
            foreach (KeyValuePair<FieldInfo, OffsetAttribute> kvp in fieldsWithSpecifiedOffsets)
            {

                OffsetAttribute offattr = kvp.Value;

                // Offset is specified by a field
                m_stream.BaseStream.Seek(offattr.SpecificOffset, SeekOrigin.Begin);
                ReadIntoField(o, kvp.Key);
            }

            // Restore our position
            m_stream.BaseStream.Position = endofstruct;
        }

        private static void SortFieldInfoArrayByOrderSpecified(Type objecttype, FieldInfo[] fis)
        {
            Dictionary<string, IntPtr> fieldorder = new Dictionary<string, IntPtr>();


            object cache = objecttype.GetType().GetProperty("Cache", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic).GetValue(objecttype, null);

            cache.GetType().GetField("m_fieldInfoCache", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic).SetValue(cache, null);

            //// We make use of Marshal.OffsetOf to get the fields in order
            //// they were specified. This is why we want the structs used to
            //// be marked StructLayout.Sequential, since there is no other
            //// way to do this in .NET's reflection services
            //foreach (FieldInfo fi in fis) {

            // fieldorder[fi.Name] = Marshal.OffsetOf(objecttype, fi.Name);
            //}

            //// By sorting the array, we can process each item in order.
            //// This makes it easy to declaratively define file formats
            //Array.Sort(fis, (x, y) => fieldorder[x.Name].ToInt32() - fieldorder[y.Name].ToInt32());

            FieldInfo[] fiSorted = objecttype.GetFields();
            for (int i = 0; i < fis.Length; i++)
            {
                fis[i] = fiSorted[i];
            }
        }

        private void ReadIntoField(object o, FieldInfo fi)
        {

            object[] lil = fi.GetCustomAttributes(typeof(LittleEndianAttribute), false);
            object[] big = fi.GetCustomAttributes(typeof(BigEndianAttribute), false);
            object[] array = fi.GetCustomAttributes(typeof(ArraySizeAttribute), false);


            bool littleendian = big.Length == 0;

            Type objecttype = o.GetType();
            object field = fi.GetValue(o);

            if (fi.FieldType.IsArray)
            {
                ArraySizeAttribute arr = (ArraySizeAttribute)array[0];

                int size = GetArraySize(o, objecttype, arr);

                field = Array.CreateInstance(fi.FieldType.GetElementType(), size);

                Read(ref field, littleendian);

            }
            else
            {
                if (fi.FieldType == typeof(string))
                {
                    field = "";
                }
                else if (field == null)
                {
                    field = Activator.CreateInstance(fi.FieldType);
                }

                Read(ref field, littleendian);
            }
            fi.SetValue(o, field);
        }

        internal static int GetArraySize(object o, Type objecttype, ArraySizeAttribute arr)
        {
            int size = arr.size;

            if (!string.IsNullOrEmpty(arr.getSize))
            {

                if (arr.getSize.Contains("()"))
                {
                    // its a method
                    var meth = objecttype.GetMethod(arr.getSize.Replace("()", ""));
                    size = int.Parse(meth.Invoke(o, null).ToString());
                }
                else
                {
                    // its a field or property
                    var fld = objecttype.GetField(arr.getSize);
                    var prop = objecttype.GetProperty(arr.getSize);

                    if (fld != null)
                    {
                        size = int.Parse(fld.GetValue(o).ToString());
                    }
                    else if (prop != null)
                    {
                        size = int.Parse(prop.GetValue(o, null).ToString());
                    }
                    else
                    {
                        throw new Exception("Field or property called " + arr.getSize + " not found!");
                    }
                }
            }
            return size;
        }
    }


}
