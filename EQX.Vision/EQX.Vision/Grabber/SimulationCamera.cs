using EQX.Core.Vision.Grabber;
using System.IO;
using System.Drawing;
using Avalonia.Media.Imaging;
using OpenCvSharp;
using System.Drawing.Imaging;
using log4net;

namespace EQX.Vision.Grabber
{
    public class SimulationCamera : ICamera
    {
        public bool IsConnected { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string SimulationImageDirectory { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsLive { get; set; } = false;
        public double ExposureTime { get; set; }
        public event EventHandler<GrabData> ContinuousImageGrabbed;
        public bool Connect()
        {
            IsConnected = true;
            return true;
        }

        public void ContinuousImageGrabStart()
        {
            IsLive = true;
            Thread continuous = new Thread(() =>
            {
                while (IsLive)
                {
                    GrabData grabData = GrabSingle();

                    ContinuousImageGrabbed?.Invoke(this, grabData);
                }
            });
            continuous.Start();
        }

        public void ContinuousImageGrabStop()
        {
            IsLive = false;
        }

        public bool Disconnect()
        {
            IsConnected = false;
            return false;
        }

        public GrabData GrabSingle()
        {
            if (!Directory.Exists(SimulationImageDirectory)) throw new ArgumentNullException($"'{SimulationImageDirectory}' folder not exist");

            List<string> files = Directory.EnumerateFiles(SimulationImageDirectory, "*.*")
                                 .Where(file => file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                                 .ToList();

            if (!files.Any()) throw new ArgumentNullException($"No '.bmp' image file in '{SimulationImageDirectory}' folder");

            string randFile = files[new Random().Next(files.Count - 1)];
            System.Drawing.Image img = System.Drawing.Image.FromFile(randFile);
            byte[] buffer = ReadByte(randFile);

            return new GrabData()
            {
                ImageBuffer = buffer,
                Width = img.Width,
                Height = img.Height,
            };
        }
        public SimulationCamera(string SimulationImageDirectory)
        {
            this.SimulationImageDirectory = SimulationImageDirectory;
            Name = "SimualationCamera";
        }

        private byte[] ReadByte(string fileName)
        {
            System.Drawing.Bitmap imgo = new System.Drawing.Bitmap(fileName);
            var bitmapData = imgo.LockBits(new System.Drawing.Rectangle(0, 0, imgo.Width, imgo.Height), ImageLockMode.ReadOnly, imgo.PixelFormat);
            var length = bitmapData.Stride * bitmapData.Height;
            byte[] bytes = new byte[length];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, bytes, 0, length);
            imgo.UnlockBits(bitmapData);
            return bytes;
        }

        public bool Initialization()
        {
            return true;
        }

    }
}
