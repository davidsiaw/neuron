using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using BlueBlocksLib.AsyncComms;

namespace BlueBlocksLib.WebSockets
{
    public class WebSocketServer
    {
        int port;
        public WebSocketServer(int port)
        {
            this.port = port;
        }

        public void Start(Action<WebSocketConnection, WebSocketData> onData, Action<WebSocketConnection> onDisconnect)
        {
            Dictionary<System.Net.Sockets.TcpClient, WebSocketConnection> conn = new Dictionary<System.Net.Sockets.TcpClient, WebSocketConnection>();

            TCPServer serv = new TCPServer(1000);
            serv.Start(
            (client, data) =>
            {
                if (!conn.ContainsKey(client))
                {
                    PerformHandshake(client, data);
                    conn[client] = new WebSocketConnection(onData, client);
                }
                else
                {
                    conn[client].Feed(data);
                }
            },

            disconnected =>
            {
                onDisconnect(conn[disconnected]);
                conn.Remove(disconnected);
            }
            );

            foreach (var kvpair in conn)
            {
                kvpair.Key.Close();
            }
        }

        private static void PerformHandshake(System.Net.Sockets.TcpClient client, byte[] data)
        {
            //TODO support different versions?

            Dictionary<string, string> headers = new Dictionary<string, string>();
            DecodeHandshake(data, headers);

            string kkey = headers["Sec-WebSocket-Key"].Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            byte[] combined = Encoding.UTF8.GetBytes(kkey);

            var sha = SHA1.Create();
            sha.Initialize();
            sha.TransformFinalBlock(combined, 0, combined.Length);


            Dictionary<string, string> respHeaders = new Dictionary<string, string>();
            respHeaders["Upgrade"] = "websocket";
            respHeaders["Connection"] = "Upgrade";
            respHeaders["Sec-WebSocket-Accept"] = Convert.ToBase64String(sha.Hash);

            string responseString = BuildResponseString(respHeaders);

            Console.WriteLine(responseString);

            byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);
            client.GetStream().Write(responseBytes, 0, responseBytes.Length);
        }

        private static void DecodeHandshake(byte[] data, Dictionary<string, string> headers)
        {
            string all = Encoding.UTF8.GetString(data, 0, data.Length);
            Console.WriteLine(all);

            string[] line = all.Split('\n');
            for (int i = 1; i < line.Length; i++)
            {
                string[] pair = line[i].Split(new string[] { ": " }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (pair.Length == 2)
                {
                    headers[pair[0]] = pair[1];
                }
            }
        }

        private static string BuildResponseString(Dictionary<string, string> respHeaders)
        {
            string responseString = "HTTP/1.1 101 Web Socket Protocol Handshake";
            foreach (var kvpair in respHeaders)
            {
                responseString += "\r\n" + kvpair.Key + ": " + kvpair.Value;
            }
            responseString += "\r\n\r\n";
            return responseString;
        }

    }

}
