using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading;

namespace BlueBlocksLib.AsyncComms
{

    /// <summary>
    /// A synchronized channel class for message passing between
    /// threads
    /// </summary>
    /// <typeparam name="T">The type of message the channel should queue</typeparam>
    public class MessageStack<T> : IChannel<T>
    {

        Stack<T> buffer = new Stack<T>();

        /// <summary>
        /// Send inserts a message into the channel
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Send(T v)
        {
            buffer.Push(v);
            Monitor.Pulse(this);
        }

        /// <summary>
        /// Receive returns the newest item in the channel
        /// and blocks if there are no items in the channel
        /// </summary>
        /// <returns>The oldest message sent through the channel</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public T Receive()
        {
            while (buffer.Count == 0)
            {
                Monitor.Wait(this);
            }
            return buffer.Pop();
        }

		[MethodImpl(MethodImplOptions.Synchronized)]
		public int Count() {
			return buffer.Count;
		}

    }
}
