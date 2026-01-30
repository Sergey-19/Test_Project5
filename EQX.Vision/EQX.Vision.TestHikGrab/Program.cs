using EQX.Core.Vision.Grabber;
using EQX.Vision.Grabber.Hikrobot;

namespace EQX.Vision.TestHikGrab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ICamera camera1 = new CameraHikrobotGigE("169.254.139.200", "camera1");
            ICamera camera2 = new CameraHikrobotGigE("169.254.159.200", "camera2");
            bool isconnect = camera1.Connect();
            bool isconnect1 = camera2.Connect();
            Console.WriteLine(isconnect);
            GrabData grap = camera1.GrabSingle();
            GrabData grap1 = camera2.GrabSingle();
            Console.WriteLine($"{grap.Height}, {grap.Width}, {grap.IsSuccess}, {grap.PixelFormat}");
            Console.WriteLine($"{grap1.Height}, {grap1.Width}, {grap1.IsSuccess}, {grap1.PixelFormat}");
            File.WriteAllBytes("GrabImage.txt", grap.ImageBuffer);
            File.WriteAllBytes("GrabImage1.txt", grap1.ImageBuffer);
            camera1.Disconnect();
            camera2.Disconnect();
            Console.ReadKey();
        }
    }
}
