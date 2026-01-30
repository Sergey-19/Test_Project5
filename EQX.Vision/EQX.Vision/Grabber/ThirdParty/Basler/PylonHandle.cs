namespace Basler.pyloncore
{
    public abstract class PylonHandle
    {
        public IntPtr Handle { get; }

        protected PylonHandle(IntPtr handle)
        {
            Handle = handle;
        }
    }
}
