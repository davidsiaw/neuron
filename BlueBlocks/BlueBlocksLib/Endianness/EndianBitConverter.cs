using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.Endianness {

	interface EndianBitConverter {
		byte[] GetBytes(int value);
		byte[] GetBytes(uint value);
		byte[] GetBytes(short value);
		byte[] GetBytes(ushort value);
		byte[] GetBytes(long value);
		byte[] GetBytes(ulong value);
		byte[] GetBytes(float value);
		byte[] GetBytes(double value);

		int ToInt32(byte[] bytes, int offset);
		uint ToUInt32(byte[] bytes, int offset);
		short ToInt16(byte[] bytes, int offset);
		ushort ToUInt16(byte[] bytes, int offset);
		long ToInt64(byte[] bytes, int offset);
		ulong ToUInt64(byte[] bytes, int offset);
		float ToSingle(byte[] bytes, int offset);
		double ToDouble(byte[] bytes, int offset);
	}

    public class LittleEndianBitConverter : EndianBitConverter
    {
		#region EndianBitConverter Members

		public byte[] GetBytes(int value) {
			return BitConverter.GetBytes(value);
		}

		public byte[] GetBytes(uint value) {
			return BitConverter.GetBytes(value);
		}

		public byte[] GetBytes(short value) {
			return BitConverter.GetBytes(value);
		}

		public byte[] GetBytes(ushort value) {
			return BitConverter.GetBytes(value);
		}

		public byte[] GetBytes(long value) {
			return BitConverter.GetBytes(value);
		}

		public byte[] GetBytes(ulong value) {
			return BitConverter.GetBytes(value);
		}

		public byte[] GetBytes(float value) {
			return BitConverter.GetBytes(value);
		}

		public byte[] GetBytes(double value) {
			return BitConverter.GetBytes(value);
		}




		public int ToInt32(byte[] bytes, int offset) {
			return BitConverter.ToInt32(bytes, offset);
		}

		public uint ToUInt32(byte[] bytes, int offset) {
			return BitConverter.ToUInt32(bytes, offset);
		}

		public short ToInt16(byte[] bytes, int offset) {
			return BitConverter.ToInt16(bytes, offset);
		}

		public ushort ToUInt16(byte[] bytes, int offset) {
			return BitConverter.ToUInt16(bytes, offset);
		}

		public long ToInt64(byte[] bytes, int offset) {
			return BitConverter.ToInt64(bytes, offset);
		}

		public ulong ToUInt64(byte[] bytes, int offset) {
			return BitConverter.ToUInt64(bytes, offset);
		}

		public float ToSingle(byte[] bytes, int offset) {
			return BitConverter.ToSingle(bytes, offset);
		}

		public double ToDouble(byte[] bytes, int offset) {
			return BitConverter.ToDouble(bytes, offset);
		}

		#endregion
	}

    public class BigEndianBitConverter : EndianBitConverter
    {

        LittleEndianBitConverter lebc = new LittleEndianBitConverter();

        byte[] Flip(byte[] input)
        {
            byte[] b = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                b[input.Length - 1 - i] = input[i];
            }
            return b;
        }

        #region EndianBitConverter Members

        public byte[] GetBytes(int value)
        {
            return Flip(lebc.GetBytes(value));
		}

        public byte[] GetBytes(uint value)
        {
            return Flip(lebc.GetBytes(value));
		}

        public byte[] GetBytes(short value)
        {
            return Flip(lebc.GetBytes(value));
		}

        public byte[] GetBytes(ushort value)
        {
            return Flip(lebc.GetBytes(value));
		}

        public byte[] GetBytes(long value)
        {
            return Flip(lebc.GetBytes(value));
		}

        public byte[] GetBytes(ulong value)
        {
            return Flip(lebc.GetBytes(value));
		}

        public byte[] GetBytes(float value)
        {
            return Flip(lebc.GetBytes(value));
		}

        public byte[] GetBytes(double value)
        {
            return Flip(lebc.GetBytes(value));
		}

        byte[] GetOffset(byte[] bytes, int offset, int length)
        {
            byte[] b = new byte[length];
            for (int i = 0; i < length; i++)
            {
                b[i] = bytes[i + offset];
            }
            return b;
        }

        public int ToInt32(byte[] bytes, int offset)
        {
            return lebc.ToInt32(Flip(GetOffset(bytes, offset, 4)), 0);
		}

        public uint ToUInt32(byte[] bytes, int offset)
        {
            return lebc.ToUInt32(Flip(GetOffset(bytes, offset, 4)), 0);
		}

        public short ToInt16(byte[] bytes, int offset)
        {
            return lebc.ToInt16(Flip(GetOffset(bytes, offset, 2)), 0);
		}

        public ushort ToUInt16(byte[] bytes, int offset)
        {
            return lebc.ToUInt16(Flip(GetOffset(bytes, offset, 2)), 0);
		}

        public long ToInt64(byte[] bytes, int offset)
        {
            return lebc.ToInt64(Flip(GetOffset(bytes, offset, 8)), 0);
		}

        public ulong ToUInt64(byte[] bytes, int offset)
        {
            return lebc.ToUInt64(Flip(GetOffset(bytes, offset, 8)), 0);
		}

        public float ToSingle(byte[] bytes, int offset)
        {
            return lebc.ToSingle(Flip(GetOffset(bytes, offset, 4)), 0);
		}

        public double ToDouble(byte[] bytes, int offset)
        {
            return lebc.ToDouble(Flip(GetOffset(bytes, offset, 8)), 0);
		}

		#endregion
	}
}
