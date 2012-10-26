using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.WebSockets
{

    public class WebSocketData
    {
        public enum Type
        {
            Binary,
            Text,
        }

        public readonly byte[] bytes;
        public readonly Type data;

        public WebSocketData(string contents)
        {
            data = Type.Text;
            bytes = Encoding.UTF8.GetBytes(contents);
        }

        public WebSocketData(byte[] bytes)
        {
            data = Type.Binary;
            this.bytes = bytes;
        }

        internal WebSocketData(Type data, byte[] bytes)
        {
            this.data = data;
            this.bytes = bytes;
        }

        public string DataAsString
        {
            get
            {
                if (data == Type.Binary)
                {
                    return Convert.ToBase64String(bytes);
                }
                return Encoding.UTF8.GetString(bytes);
            }
        }

    }
}
