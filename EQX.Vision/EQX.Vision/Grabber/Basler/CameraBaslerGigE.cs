using Basler.pyloncore;
using Basler.UserGrabResult;
using EQX.Core.Vision.Grabber;
using System.Text.RegularExpressions;
using System.Windows.Media.Media3D;

namespace EQX.Vision.Grabber
{
    public class CameraBaslerGigE : ICamera
    {
        #region Properties
        /// <summary>
        /// IP of the camera
        /// </summary>
        public int Id { get; init; }
        public string Name { get; init; }
        public bool IsConnected { get; private set; }
        public string IpAddress { get; set; }
        public event EventHandler<GrabData>? ContinuousImageGrabbed;

        public double ExposureTime
        {
            get 
            {
                ExposureTime = camera.GetFloatParameter("ExposureTime");
                return _ExposureTime; 
            }
            set 
            {
                camera.SetFloatParameter("ExposureTime", (float)value);
                _ExposureTime = value; 
            }
        }

        #endregion

        #region Constructor(s)
        public CameraBaslerGigE(string ip, string name)
        {
            #region Guard clause
            Regex validateIPv4Regex = new Regex("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

            if (ip == null) throw new ArgumentNullException(nameof(ip));
            if (ip.GetType() != typeof(string))
                throw new ArgumentException("Argument type exception, expected string", nameof(ip));
            if (validateIPv4Regex.IsMatch(ip) == false)
                throw new ArgumentException(message: "Argument format exception, expected ip format", nameof(ip));

            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Argument null or empty");
            #endregion

            this.IpAddress = ip;
            this.Name = name;
            Id = Convert.ToInt32(IpAddress.Substring(IpAddress.LastIndexOf(".") + 1));

            camera = new PylonCamera();
        }
        #endregion

        #region Public Methods
        public bool Connect()
        {
            camera.InitializeCamera(IpAddress);
            camera.DeviceStartAcquisition();

            return true;
        }

        public bool Disconnect()
        {
            camera.DestroyCamera();

            return true;
        }

        public GrabData GrabSingle()
        {
            Thread.Sleep(1);

            GrabResultEx gr = camera.GrabSingleImage();
            return new GrabData
            {
                Id = gr.ImageNumber,
                ImageBuffer = gr.ImageRawBuffer,
                Width = gr.Width,
                Height = gr.Height,
                IsSuccess = gr.ErrorCode == 0,
                PixelFormat = gr.PixelType,
            };
        }

        public void ContinuousImageGrabStart()
        {
            // TODO: This is not thread safe code
            camera.OnGrabImage -= OnContinuousImageGrabbed;
            camera.OnGrabImage += OnContinuousImageGrabbed;
            camera.GrabContinues();
        }

        public void ContinuousImageGrabStop()
        {
            camera.StopContinuesGrab();
        }
        #endregion

        private void OnContinuousImageGrabbed(object? sender, GrabResultEx e)
        {
            ContinuousImageGrabbed?.Invoke(
                this,
                new GrabData
                {
                    Id = e.ImageNumber,
                    ImageBuffer = e.ImageRawBuffer,
                    Width = e.Width,
                    Height = e.Height,
                    IsSuccess = e.ErrorCode == 0,
                    PixelFormat = e.PixelType,
                });
        }

        public bool Initialization()
        {
            throw new NotImplementedException();
        }

        #region Privates
        private double _ExposureTime;
        private readonly PylonCamera camera;
        #endregion
    }
}
