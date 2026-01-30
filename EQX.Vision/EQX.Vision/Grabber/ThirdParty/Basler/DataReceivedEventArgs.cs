using Basler.UserGrabResult;

namespace Basler.pyloncore
{
    public class DataReceivedEventArgs : EventArgs
    {
        public GrabResultEx GrabResult = null;

        public bool Status { get; set; } = false;

        internal DataReceivedEventArgs(bool status, GrabResultEx gr)
        {
            Status = status;
            GrabResult = gr;
        }
    }
}
