using System.Diagnostics;

namespace Basler.UserGrabResult;

public class GrabResultEx : EventArgs, IDisposable
{
    public int Width { get; set; } = 0;

    public int Height { get; set; } = 0;

    public ulong ImageNumber { get; set; } = 0uL;

    public byte[] ImageRawBuffer { get; set; } = null;

    public long PixelType { get; set; } = 0L;

    public ulong TimeStamp { get; set; } = 0uL;

    public int OffsetX { get; set; } = 0;

    public int OffsetY { get; set; } = 0;

    public int PaddingX { get; set; } = 0;

    public int PaddingY { get; set; } = 0;

    public ulong PayloadSize { get; set; } = 0uL;

    public uint ErrorCode { get; set; } = 0u;

    public ulong BlockId { get; set; } = 0uL;

    public GrabResultEx(int W, int H, ulong ImageNumberIn, byte[] byteArray)
    {
        Width = W;
        Height = H;
        ImageNumber = ImageNumberIn;
        ImageRawBuffer = byteArray;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Array.Clear(ImageRawBuffer);
            ImageRawBuffer = null;
            Debug.WriteLine("Buffer freeing now");
        }
    }

    ~GrabResultEx()
    {
        Dispose(disposing: false);
    }
}
