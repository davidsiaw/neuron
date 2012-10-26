using System;
using System.Collections.Generic;
using System.Text;
using BlueBlocksLib.Endianness;
using BlueBlocksLib.SetUtils;

namespace BlueBlocksLib.WebSockets
{
    public class WebSocketConnection
    {
        Action<WebSocketConnection, WebSocketData> onData;
        System.Net.Sockets.TcpClient client;
        BigEndianBitConverter bebc = new BigEndianBitConverter();

		public WebSocketConnection(string hostname, int port, Action<WebSocketConnection, WebSocketData> onData) {

			this.onData = onData;
			client = new System.Net.Sockets.TcpClient(hostname, port);

			byte[] b = new byte[1024];
			int size = client.Client.Receive(b);
			byte[] feed = new byte[size];
			Array.Copy(b, feed, size);
			Feed(feed);
		}


        internal WebSocketConnection(Action<WebSocketConnection, WebSocketData> onData, System.Net.Sockets.TcpClient client)
        {
            this.onData = onData;
            this.client = client;
        }

        public void Send(WebSocketData data)
        {
            Frame f = new Frame();
            f.FIN = true;
            f.payloadLength = (ulong)data.bytes.Length;
            f.opcode = (data.data == WebSocketData.Type.Text) ? Frame.Opcode.Text : Frame.Opcode.Binary;
            f.payload = data.bytes;
            byte[] toSend = EncodeFrame(f);
            client.GetStream().Write(toSend, 0, toSend.Length);
        }

        Queue<Frame> fragmentsSoFar = new Queue<Frame>(); // <- could change for x-google-mux or some multiplexing method

        byte[] incompleteFrame = new byte[0];

        internal void Feed(byte[] data)
        {
            bool success = DecodeFrame(data, f =>
            {

                fragmentsSoFar.Enqueue(f);

                if (f.FIN)
                {
                    if (f.opcode == Frame.Opcode.Close)
                    {
                        Frame closeframe = new Frame();
                        closeframe.FIN = true;
                        closeframe.opcode = Frame.Opcode.Close;
                        closeframe.payloadLength = 0;
                        closeframe.payload = new byte[0];
                        byte[] toSend = EncodeFrame(closeframe);
                        client.GetStream().Write(toSend, 0, toSend.Length);
                        client.Close();

                        return;
                    }


                    WebSocketData.Type type = (fragmentsSoFar.Peek().opcode == Frame.Opcode.Text) ? WebSocketData.Type.Text : WebSocketData.Type.Binary;
                    byte[] bytes = new byte[0];

                    while (fragmentsSoFar.Count != 0)
                    {
                        Frame curr = fragmentsSoFar.Dequeue();
                        bytes = ArrayUtils.Concat(bytes, curr.GetUnmaskedPayload());
                    }

                    WebSocketData wsd = new WebSocketData(type, bytes);
                    onData(this, wsd);
                }

                incompleteFrame = new byte[0];

            });

            if (!success)
            {
                incompleteFrame = ArrayUtils.Concat(incompleteFrame, data);
            }
        }


        struct Frame
        {

            public enum Opcode
            {
                Continuation = 0x0,
                Text = 0x1,
                Binary = 0x2,

                Close = 0x8,
                Ping = 0x9,
                Pong = 0xa,
            }

            public bool FIN;
            public bool RSV1;
            public bool RSV2;
            public bool RSV3;
            public Opcode opcode;
            public bool payloadIsMasked;
            public ulong payloadLength;
            public byte[] maskingKey;
            public byte[] payload;

            public static byte[] MaskUnmask(byte[] payload, byte[] maskingKey)
            {
                byte[] unmasked = new byte[payload.Length];
                for (int i = 0; i < payload.Length; i++)
                {
                    unmasked[i] = (byte)(payload[i] ^ maskingKey[i % 4]);
                }
                return unmasked;
            }

            public byte[] GetUnmaskedPayload()
            {
                if (payloadIsMasked)
                {
                    return MaskUnmask(payload, maskingKey);
                }
                return payload;
            }
        }

        byte[] EncodeFrame(Frame f)
        {

            byte[] b = new byte[
                2 + // stuff
                2 + // size
                4 + // mask
                f.payload.Length
                ];

            b[0] = (byte)(
                (0x1 << 7) + // FIN
                ((f.RSV1 ? 1 : 0) << 6) +
                ((f.RSV2 ? 1 : 0) << 5) +
                ((f.RSV3 ? 1 : 0) << 4) +
                +((int)f.opcode)
                );

            b[1] = (1 << 7) + 126;

            byte[] lengthbytes = bebc.GetBytes((ushort)f.payload.Length);
            Array.Copy(lengthbytes, 0, b, 2, 2);

            Random r = new Random((int)DateTime.Now.Ticks);
            byte[] maskingKey = bebc.GetBytes(r.Next());
            Array.Copy(maskingKey, 0, b, 4, 4);

            for (int i = 0; i < f.payload.Length; i++)
            {
                b[i + 8] = (byte)(f.payload[i] ^ maskingKey[i % 4]);
            }

            return b;
        }

        bool DecodeFrame(byte[] data, Action<Frame> decodeSuccessful)
        {
            Frame f = new Frame();
            f.FIN = (data[0] & 0x80) != 0;
            f.RSV1 = (data[0] & 0x40) != 0;
            f.RSV2 = (data[0] & 0x20) != 0;
            f.RSV3 = (data[0] & 0x10) != 0;
            f.opcode = (Frame.Opcode)(data[0] & 0xf);
            f.payloadIsMasked = ((data[1] & 0x80) != 0) ? true : false;


            uint toplen = (uint)(data[1] & 0x7f);
            uint nextSection = 2;
            if (toplen == 126)
            {
                f.payloadLength = bebc.ToUInt16(data, 2);
                nextSection = 4;
            }
            else if (toplen == 127)
            {
                f.payloadLength = bebc.ToUInt64(data, 2);
                nextSection = 10;
            }
            else
            {
                f.payloadLength = toplen;
            }

            if (f.payloadIsMasked)
            {
                f.maskingKey = new byte[4];
                f.maskingKey[0] = data[nextSection + 0];
                f.maskingKey[1] = data[nextSection + 1];
                f.maskingKey[2] = data[nextSection + 2];
                f.maskingKey[3] = data[nextSection + 3];
                nextSection += 4;
            }


            if ((data.Length - (int)nextSection) < (int)f.payloadLength)
            {
                return false;
            }

            f.payload = new byte[f.payloadLength];

            Array.Copy(data, nextSection, f.payload, 0, f.payload.Length);
            decodeSuccessful(f);

            return true;
        }
    }
}
