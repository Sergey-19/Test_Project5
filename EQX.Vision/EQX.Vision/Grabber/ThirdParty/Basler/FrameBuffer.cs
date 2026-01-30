using System.Runtime.InteropServices;

namespace Basler.pyloncore
{
    public class FrameBuffer : IDisposable
    {
        private bool disposed;

        private GCHandle handle;

        private byte[] data;

        public int BufferIndex { get; set; }

        public StreamBufferHandle pylonbuf { get; set; }

        public ulong PayloadSize { get; set; }

        public GCHandle GetHandle()
        {
            return handle;
        }

        public byte[] GetImageData()
        {
            return data;
        }

        public FrameBuffer(ulong PayloadSizeIn)
        {
            data = new byte[PayloadSizeIn];
            PayloadSize = PayloadSizeIn;
            handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    handle.Free();
                    data = null;
                    pylonbuf = null;
                }

                disposed = true;
            }
        }
    }
}
