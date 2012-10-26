using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using BlueBlocksLib.Collections;
using System.Net;
using System.IO;
using System.Threading;

namespace BlueBlocksLib.AsyncComms
{
    public class TCPServer
    {
        int port;
        public TCPServer(int port)
        {
            this.port = port;
        }

        Agent<Action> agent = null;
        bool running = false;
        public void Start(Action<TcpClient, byte[]> onData, Action<TcpClient> onDisconnect)
        {
            TcpListener listener = new TcpListener(port);
            listener.Start();
            running = true;
            AutoResetEvent are = new AutoResetEvent(false);
            agent = new Agent<Action>(
                () => { },
                () => { },
                nextaction =>
                {

                    nextaction();

                    if (running)
                    {
                        return NextAction.WaitForNextMessage;
                    }
                    are.Set();
                    return NextAction.Finish;
                });

            agent.Start();

            agent.SendMessage(() => { StartAccepting(listener, onData, onDisconnect); });
            are.WaitOne();

            listener.Stop();
        }

        class Client
        {
            public Client(TcpClient client, byte[] buffer, Agent<Action> agent, Action<TcpClient, byte[]> onData, Action<TcpClient> onDisconnect)
            {
                this.client = client;
                this.agent = agent;
                this.buffer = buffer;
                this.onData = onData;
                this.onDisconnect = onDisconnect;
            }

            Action<TcpClient, byte[]> onData;
            byte[] buffer;
            TcpClient client;
            Agent<Action> agent;
            Action<TcpClient> onDisconnect;

            public void StartReceiving()
            {
                if (client.Client == null || !client.Connected)
                {
                    onDisconnect(client);
                    return;
                }
                client.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, x =>
                {
                    TcpClient c = (TcpClient)x.AsyncState;
                    int length = c.Client.EndReceive(x);
                    if (length == 0)
                    {
                        onDisconnect(c);
                        return;
                    }
                    byte[] b = new byte[length];
                    Array.Copy(buffer, b, length);

                    agent.SendMessage(() => { onData(c, b); });

                    agent.SendMessage(StartReceiving);

                }, client);
            }
        }

        private void StartAccepting(TcpListener listener, Action<TcpClient, byte[]> onData, Action<TcpClient> onDisconnect)
        {
            listener.BeginAcceptSocket(x =>
            {
                TcpListener li = (TcpListener)x.AsyncState;
                TcpClient client = li.EndAcceptTcpClient(x);

                Client c = new Client(client, new byte[10240], agent, onData, onDisconnect);

                agent.SendMessage(c.StartReceiving);

                agent.SendMessage(() => { StartAccepting(listener, onData, onDisconnect); });

            }, listener);
        }


        public void Stop()
        {
            running = false;
            agent.SendMessage(() => { });
        }
    }

}

