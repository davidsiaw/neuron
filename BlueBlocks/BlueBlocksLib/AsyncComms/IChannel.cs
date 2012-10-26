using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.AsyncComms
{
    public interface IChannel<T>
    {
        void Send(T v);
        T Receive();
		int Count();
    }
}
