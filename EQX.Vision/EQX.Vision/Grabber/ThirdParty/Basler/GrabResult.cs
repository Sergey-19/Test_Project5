namespace Basler.pyloncore
{
    public struct GrabResult
    {
        public IntPtr Context;

        public IntPtr StreamBufferHandle;

        public IntPtr Buffer;

        public GrabStatus Status;

        public PayloadType PayloadType;

        public long PixelType;

        public ulong TimeStamp;

        public int SizeX;

        public int SizeY;

        public int OffsetX;

        public int OffsetY;

        public int PaddingX;

        public int PaddingY;

        public ulong PayloadSize;

        public uint ErrorCode;

        public ulong BlockId;
    }
}
