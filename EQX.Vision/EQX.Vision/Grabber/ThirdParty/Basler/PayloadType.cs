namespace Basler.pyloncore
{
    public enum PayloadType
    {
        Undefined = -1,
        Image = 0,
        RawData = 1,
        File = 2,
        ChunkData = 3,
        GenDC = 4,
        DeviceSpecific = 32768
    }
}
