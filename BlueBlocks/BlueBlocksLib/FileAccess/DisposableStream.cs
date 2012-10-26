using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BlueBlocksLib.FileAccess {
    public abstract class DisposableStream : IDisposable {

        protected IDisposable m_stream;

        #region IDisposable Members

        ~DisposableStream() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
        }

        bool disposed = false;

        protected void Dispose(bool now) {
            if (now) {
                if (disposed) {
                    throw new Exception("Object already disposed");
                }
                m_stream.Dispose();
                disposed = true;
            } else {
                if (!disposed) {
                    m_stream.Dispose();
                }
            }
        }

        #endregion
    }
}
