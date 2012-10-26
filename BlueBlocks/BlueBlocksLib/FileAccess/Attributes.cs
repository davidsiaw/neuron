using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.FileAccess {

    // Attribute for saying this is stored as little endian
    [AttributeUsage(AttributeTargets.Field)]
    public class LittleEndianAttribute : Attribute { }

    // Attribute for saying this is stored as big endian
    [AttributeUsage(AttributeTargets.Field)]
    public class BigEndianAttribute : Attribute { }

    // Attribute for marking how big an array should be
    [AttributeUsage(AttributeTargets.Field)]
    public class ArraySizeAttribute : Attribute {
        // When writing: if the array is null then assume all zero

        internal int size;
        public ArraySizeAttribute(int size) { this.size = size; }

        // This is for property, field or method extraction of size
        internal string getSize = null;
        public ArraySizeAttribute(string sizeFunction) { this.getSize = sizeFunction; }
    }

    // Attribute for saying where the start offset of the item should be
    [AttributeUsage(AttributeTargets.Field)]
    public class OffsetAttribute : Attribute {
        internal long SpecificOffset;

        public OffsetAttribute(long offset) {
            SpecificOffset = offset;
        }

        // This is for property, field or method extraction of the offset
        public OffsetAttribute(string offsetFunction) {
            getOffset = offsetFunction;
        }

        internal string getOffset = null;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class InternalUseAttribute : Attribute
    {
    }

    
}
