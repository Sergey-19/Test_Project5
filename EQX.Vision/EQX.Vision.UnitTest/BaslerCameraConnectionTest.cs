using EQX.Core.Vision.Grabber;
using EQX.Vision.Grabber;
using System.Diagnostics;
using System.Security.Cryptography;

namespace EQX.Vision.UnitTest
{
    public class BaslerCameraConnectionTest
    {
        [Fact]
        public void TestCameraBasicConnectGrab()
        {
            ICamera Camera = new CameraBaslerGigE("192.168.0.100", "Basler Cam.");

            bool connectStatus = Camera.Connect();

            Assert.True(connectStatus);

            GrabData grabData;

            for (int i = 0; i < 1000; i++)
            {
                int startTime = Environment.TickCount;

                Debug.WriteLine($"Loop #{i}");
                grabData = Camera.GrabSingle();

                int costTime = Environment.TickCount - startTime;
                Debug.WriteLine($"[{costTime} ms] Image size = {grabData.ImageBuffer?.Length}");

                Assert.True(grabData.IsSuccess);
                grabData.Dispose();
            }

            Camera.Disconnect();
        }
    }
}
