using EQX.Core.Vision.Grabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace EQX.Vision.Grabber
{
    public class CameraHikrobotGigE : ICamera
    {
        private double _ExposureTime;

        public bool IsConnected { get => camera.MV_CC_IsDeviceConnected_NET(); private set { } }
        public int Id { get; init; }
        public string Name { get; init; }
        public string IpAddress { get; set; }

        public event EventHandler<GrabData> ContinuousImageGrabbed;
        public double ExposureTime
        {
            get 
            {
                MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
                int nRet = camera.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
                if (MyCamera.MV_OK == nRet)
                {
                    _ExposureTime = stParam.fCurValue;
                }
                return _ExposureTime; 
            }
            set 
            {
                camera.MV_CC_SetFloatValue_NET("ExposureTime", (float)value);
                _ExposureTime = value;
            }
        }

        public CameraHikrobotGigE(string ip, string name)
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

        public bool Connect()
        {
            //Id = Convert.ToInt32(IpAddress.Substring(IpAddress.LastIndexOf(".")));
            MyCamera.MV_CC_DEVICE_INFO_LIST m_stDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE, ref m_stDeviceList);
            if (0 != nRet)
                throw new ArgumentException(message: "Enumerate devices fail!");
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                UInt32 nIp1 = (gigeInfo.nCurrentIp & 0xFF000000) >> 24;
                UInt32 nIp2 = (gigeInfo.nCurrentIp & 0x00FF0000) >> 16;
                UInt32 nIp3 = (gigeInfo.nCurrentIp & 0x0000FF00) >> 8;
                UInt32 nIp4 = (gigeInfo.nCurrentIp & 0x000000FF);
                string currentIp = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();
                if (currentIp == IpAddress)
                {
                    if (null == camera)
                    {
                        camera = new MyCamera();
                        if (null == camera)
                        {
                            throw new ArgumentException("Applying resource fail!");
                        }
                    }
                    int createDevice = camera.MV_CC_CreateDevice_NET(ref device);
                    if (MyCamera.MV_OK != createDevice)
                    {
                        throw new ArgumentException("Create device fail!");
                    }
                }
            }

            if (camera.MV_CC_IsDeviceConnected_NET() == false)
            {
                nRet = camera.MV_CC_OpenDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    camera.MV_CC_CloseDevice_NET();
                    throw new ArgumentException("Device open fail!");
                }
            } 
            return IsConnected;
        }

        public void ContinuousImageGrabStart()
        {
            _isLive = true;
            Thread continuous = new Thread(() =>
            {
                while(_isLive)
                {
                    GrabData grabData = GrabSingle();

                    ContinuousImageGrabbed?.Invoke(this, grabData);
                }
            });
            continuous.Start();
        }

        public void ContinuousImageGrabStop()
        {
            _isLive = false;
        }
        public bool Disconnect()
        {
            camera.MV_CC_CloseDevice_NET();
            return true;
        }

        public GrabData GrabSingle()
        {
            int nRet = MyCamera.MV_OK;
            int nPacketSize = camera.MV_CC_GetOptimalPacketSize_NET();
            if (nPacketSize > 0)
            {
                nRet = camera.MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", nPacketSize);
                if (nRet != MyCamera.MV_OK)
                {
                    throw new ArgumentException("Warning: Set Packet Size failed {0:x8}");
                }
            }
            else
            {
                throw new ArgumentException("Warning: Get Packet Size failed {0:x8}", nPacketSize.ToString());
            }
            if (MyCamera.MV_OK != camera.MV_CC_SetEnumValue_NET("TriggerMode", 0))
            {
                throw new ArgumentException("Warning: Get Packet Size failed {0:x8}", nPacketSize.ToString());
            }
            if (MyCamera.MV_OK != camera.MV_CC_SetEnumValue_NET("TriggerMode", 0))
            {
                throw new ArgumentException("Set TriggerMode failed:{0:x8}");
            }
            nRet = camera.MV_CC_StartGrabbing_NET();
            if (MyCamera.MV_OK != nRet)
            {
                throw new ArgumentException("Start grabbing failed:{0:x8}", nRet.ToString());
            }
            MyCamera.MV_FRAME_OUT stImageOut = new MyCamera.MV_FRAME_OUT();
            nRet = camera.MV_CC_GetImageBuffer_NET(ref stImageOut, 1000);
            if (nRet != MyCamera.MV_OK) throw new ArgumentException("Get Image failed:{0:x8}", nRet.ToString());
            byte[] imageBuffer = new byte[stImageOut.stFrameInfo.nFrameLen];
            Marshal.Copy(stImageOut.pBufAddr, imageBuffer, 0, imageBuffer.Length);
            camera.MV_CC_FreeImageBuffer_NET(ref stImageOut);
            nRet = camera.MV_CC_StopGrabbing_NET();
            if (MyCamera.MV_OK != nRet)
            {

                throw new ArgumentException("Stop grabbing failed:{0:x8}", nRet.ToString());
            }
            return new GrabData
            {
                Id = stImageOut.stFrameInfo.nFrameNum,
                Width = stImageOut.stFrameInfo.nWidth,
                Height = stImageOut.stFrameInfo.nHeight,
                PixelFormat = (long)stImageOut.stFrameInfo.enPixelType,
                IsSuccess = true,
                ImageBuffer = imageBuffer
            };

        }

        public bool Initialization()
        {
            return true;
        }

        private bool _isLive = false;
        private MyCamera camera = new MyCamera();
    }
}
