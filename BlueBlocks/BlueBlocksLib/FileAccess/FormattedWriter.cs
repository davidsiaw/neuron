using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BlueBlocksLib.Endianness;

namespace BlueBlocksLib.FileAccess {
    public class FormattedWriter : DisposableStream {

        public BinaryWriter BaseStream {
            get { return (BinaryWriter)m_stream; }
        }


        public FormattedWriter(Stream stm)
        {
            m_stream = new BinaryWriter(stm);
        }

        public FormattedWriter(string filename) {
            m_stream = new BinaryWriter(new FileStream(filename, FileMode.Create));
        }

        public void Write(object o) {
            Write(o, true, 0);
        }

        void Write(object o, bool isLittleEndian, int arraysize) {
            BinaryWriter m_stream = (BinaryWriter)this.m_stream;

            StructLayoutAttribute structlayout = o.GetType().StructLayoutAttribute;
            
            EndianBitConverter b;
            if (!isLittleEndian) {
                b = new BigEndianBitConverter();
            } else {
                b = new LittleEndianBitConverter();
            }

            if (o.GetType() == typeof(int)) {

                m_stream.Write(b.GetBytes((int)o));
            } else if (o.GetType() == typeof(uint)) {

                m_stream.Write(b.GetBytes((uint)o));
            } else if (o.GetType() == typeof(short)) {

                m_stream.Write(b.GetBytes((short)o));
            } else if (o.GetType() == typeof(ushort)) {

                m_stream.Write(b.GetBytes((ushort)o));
            } else if (o.GetType() == typeof(long)) {

                m_stream.Write(b.GetBytes((long)o));
            } else if (o.GetType() == typeof(ulong)) {

                m_stream.Write(b.GetBytes((ulong)o));
            } else if (o.GetType() == typeof(float)) {

                m_stream.Write(b.GetBytes((float)o));

            } else if (o.GetType() == typeof(double)) {

                m_stream.Write(b.GetBytes((double)o));
            }
            else if (o.GetType().IsEnum)
            {
                m_stream.Write(b.GetBytes((int)o));
            }
            else if (o.GetType() == typeof(byte))
            {
                m_stream.Write((byte)o);
            }
            else if (o.GetType().IsArray)
            {
                Array arr = (Array)o;

                for (int i = 0; i < arraysize; i++)
                {
                    Write(arr.GetValue(i), isLittleEndian, 0);
                }

            }
            else if (structlayout != null)
            {

                Type objecttype = o.GetType();
                FieldInfo[] fis = objecttype.GetFields();

                // Sort the fields into the order they were specified
                Dictionary<string, IntPtr> fieldorder = new Dictionary<string, IntPtr>();

                foreach (FieldInfo fi in fis)
                {
                    fieldorder[fi.Name] = Marshal.OffsetOf(objecttype, fi.Name);
                }

                Array.Sort(fis, (x, y) => fieldorder[x.Name].ToInt32() - fieldorder[y.Name].ToInt32());


                List<object> towrite = new List<object>();

                foreach (FieldInfo fi in fis)
                {
                    object[] lil = fi.GetCustomAttributes(typeof(LittleEndianAttribute), false);
                    object[] big = fi.GetCustomAttributes(typeof(BigEndianAttribute), false);
                    object[] array = fi.GetCustomAttributes(typeof(ArraySizeAttribute), false);
                    InternalUseAttribute[] internalUse = (InternalUseAttribute[])fi.GetCustomAttributes(typeof(InternalUseAttribute), false);

                    if (internalUse.Length > 0)
                    {
                        continue;
                    }

                    bool littleendian = big.Length == 0;

                    object field = fi.GetValue(o);

                    if (array.Length > 0 && fi.FieldType.IsArray)
                    {
                        ArraySizeAttribute arr = (ArraySizeAttribute)array[0];

                        int size = FormattedReader.GetArraySize(o, objecttype, arr);

                        if (field == null)
                        {
                            field = Array.CreateInstance(fi.FieldType.GetElementType(), size);
                        }

                        Write(field, littleendian, size);

                    }
                    else if (fi.FieldType.IsEnum)
                    {

                        Write((int)field, littleendian, 0);

                    }
                    else
                    {
                        Write(field, littleendian, 0);
                    }
                }

            }
            else
            {
                throw new Exception("I don't know how to write this object:" + o.GetType().Name + " Please include the [StructLayout] attribute");
            }

        }




    }
}
