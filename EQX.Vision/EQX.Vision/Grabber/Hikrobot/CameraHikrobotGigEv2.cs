using EQX.Core.Vision.Grabber;
using MvCameraControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace EQX.Vision.Grabber.Hikrobot
{
    public class CameraHikrobotGigEv2 : ICamera
    {
        public double ExposureTime
        {
            get
            {
                if (device == null) return double.NaN;

                IFloatValue floatValue;
                device.Parameters.GetFloatValue("ExposureTime", out floatValue);
                _exposureTime = floatValue.CurValue;
                return _exposureTime;
            }
            set
            {
                if (device == null) return;
                device.Parameters.SetFloatValue("ExposureTime", (float)value);
                _exposureTime = value;
            }
        }

        public bool IsConnected => device != null ? device.IsConnected : false;

        public int Id { get; init; }
        public string Name { get; init; }
        public string IpAddress { get; set; }

        public event EventHandler<GrabData> ContinuousImageGrabbed;
        public CameraHikrobotGigEv2(string ip, string name)
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
        }
        public bool Initialization()
        {
            List<IDeviceInfo> _devInfoList;
            result = DeviceEnumerator.EnumDevices(DeviceTLayerType.MvGigEDevice, out _devInfoList);
            if (MvError.MV_OK != result)
            {
                return false;
            }
            if (_devInfoList.Count <= 0) { return false; }
            for (int i = 0; i < _devInfoList.Count; i++)
            {
                IDeviceInfo deviceInfo = _devInfoList[i];
                IGigEDeviceInfo gigeDevInfo = deviceInfo as IGigEDeviceInfo;
                uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                string currentIp = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();
                if (currentIp == IpAddress)
                {
                    device = DeviceFactory.CreateDevice(deviceInfo);
                }
            }
            if (device == null)
            {
                return false;
            }
            return true;
        }
        public bool Connect()
        {
            if (device == null)
            {
                if (Initialization() == false) return false;
            }

            //Open device
            if (device.IsConnected == false)
            {
                result = device.Open();
                if (MvError.MV_OK != result)
                {
                    return false;
                }
            }
            if (device is IGigEDevice)
            {
                // Convert to Gige device
                IGigEDevice gigEDevice = (IGigEDevice)device;
                // en:Detection network optimal package size(It only works for the GigE camera)
                result = gigEDevice.GetOptimalPacketSize(out packetSize);
                if (MvError.MV_OK != result)
                {
                    return false;
                }
                else
                {
                    result = gigEDevice.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                    if (MvError.MV_OK != result)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            // ch:???????? | en:Set Continues Aquisition Mode
            device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
            device.Parameters.SetEnumValueByString("TriggerMode", "Off");
            return IsConnected;
        }

        public void ContinuousImageGrabStart()
        {
            if (_isLive == false)
            {
                device.Parameters.SetEnumValueByString("TriggerMode", "Off");
                try
                {
                    // ch:?????true | en:Set position bit true
                    isGrabbing = true;

                    receiveThread = new Thread(ReceiveThreadProcess);
                    receiveThread.Start();

                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Start thread failed!, " + ex.Message);
                }

                // ch:???? | en:Start Grabbing
                int result = device.StreamGrabber.StartGrabbing();
                if (result != MvError.MV_OK)
                {
                    isGrabbing = false;
                    receiveThread.Join();
                    throw new ArgumentException("Start Grabbing Fail!", result.ToString());
                }
            }
            _isLive = true;
        }

        public void ContinuousImageGrabStop()
        {
            if (_isLive == true)
            {
                isGrabbing = false;
                receiveThread?.Join();

                // ch:???? | en:Stop Grabbing
                int result = device.StreamGrabber.StopGrabbing();
                if (result != MvError.MV_OK)
                {
                    throw new ArgumentException("Stop Grabbing Fail!", result.ToString());
                }
            }
            _isLive = false;

        }

        public bool Disconnect()
        {
            if(device == null) return false;
            if ((device.IsConnected))
            {
                if (_isLive == true)
                    ContinuousImageGrabStop();
                result = device.Close();
                if (MvError.MV_OK != result)
                {
                    return false;
                    throw new ArgumentException("Close device failed:{0:x8}", result.ToString());
                }
            }
            return true;
        }
        public void ReceiveThreadProcess()
        {
            int nRet;

            Graphics graphics;   // ch:??GDI?pictureBox????? | en:Display frame using a graphics

            while (isGrabbing)
            {
                IFrameOut frameOut;

                nRet = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                if (MvError.MV_OK == nRet)
                {
                    if (isRecord)
                    {
                        device.VideoRecorder.InputOneFrame(frameOut.Image);
                    }

                    lock (saveImageLock)
                    {
                        frameForSave = frameOut.Clone() as IFrameOut;
                    }


                    GrabData grabData = new GrabData
                    {
                        Id = frameOut.FrameNum,
                        Width = (int)frameOut.Image.Width,
                        Height = (int)frameOut.Image.Height,
                        PixelFormat = (long)frameOut.Image.PixelType,
                        IsSuccess = true,
                        ImageBuffer = frameOut.Image.PixelData
                    };
                    ContinuousImageGrabbed?.Invoke(this, grabData);

                    device.StreamGrabber.FreeImageBuffer(frameOut);
                }
            }
        }

        public GrabData GrabSingle()
        {
            if (_isLive == true)
                ContinuousImageGrabStop();

            //  en: start grab image
            result = device.StreamGrabber.StartGrabbing();
            if (MvError.MV_OK != result)
            {
                throw new ArgumentException("Start grabbing failed:{0:x8}", result.ToString());
            }
            IFrameOut frameOut;
            result = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
            device.StreamGrabber.FreeImageBuffer(frameOut);
            device.StreamGrabber.StopGrabbing();
            return new GrabData
            {
                Id = frameOut.FrameNum,
                Width = (int)frameOut.Image.Width,
                Height = (int)frameOut.Image.Height,
                PixelFormat = (long)frameOut.Image.PixelType,
                IsSuccess = true,
                ImageBuffer = frameOut.Image.PixelData
            };
        }



        private int result = MvError.MV_OK;
        private IDevice device = null;
        private int packetSize;
        private bool isGrabbing = false;
        private bool isRecord = false;
        private Thread receiveThread = null;
        private IFrameOut frameForSave;
        private readonly object saveImageLock = new object();
        private bool _isLive = false;
        private double _exposureTime;


    }
}
