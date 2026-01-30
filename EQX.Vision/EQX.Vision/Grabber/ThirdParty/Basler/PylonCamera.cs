using Basler.UserGrabResult;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Basler.pyloncore
{
    public class PylonCamera : IImageGrabber
    {
        private CameraHandle _cameraHandel = null;

        private StreamGrabberHandle _streamGrabber = null;

        private List<FrameBuffer> Framebuffers = new List<FrameBuffer>();

        private Task GrabTask = null;

        private volatile bool StopRequested = false;

        private volatile bool IsRunning = false;

        public event EventHandler<GrabResultEx> OnGrabImage = null;

        public bool SetCommandParameter(string FeatureName)
        {
            bool result = false;
            try
            {
                if (libpylonc.DeviceFeatureIsAvailable(_cameraHandel, FeatureName))
                {
                    libpylonc.DeviceExecuteCommandFeature(_cameraHandel, FeatureName);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                ShowException("GetBoolParameter", ex);
            }

            return result;
        }

        public bool SetEnumParameter(string FeatureName, string value)
        {
            bool result = false;
            try
            {
                if (libpylonc.DeviceFeatureIsAvailable(_cameraHandel, FeatureName))
                {
                    libpylonc.DeviceFeatureFromString(_cameraHandel, FeatureName, value);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                ShowException("GetBoolParameter", ex);
            }

            return result;
        }

        public string GetEnumParameter(string FeatureName)
        {
            string text = string.Empty;
            try
            {
                if (libpylonc.DeviceFeatureIsAvailable(_cameraHandel, FeatureName))
                {
                    text = libpylonc.DeviceFeatureToString(_cameraHandel, FeatureName);
                    text = text.Replace("\0", string.Empty);
                }
            }
            catch (Exception ex)
            {
                ShowException("GetBoolParameter", ex);
            }

            return text;
        }

        public bool SetBoolParameter(string FeatureName, bool value)
        {
            bool result = false;
            try
            {
                if (libpylonc.DeviceFeatureIsAvailable(_cameraHandel, FeatureName))
                {
                    result = libpylonc.DeviceSetBooleanFeature(_cameraHandel, FeatureName, value);
                }
            }
            catch (Exception ex)
            {
                ShowException("GetBoolParameter", ex);
            }

            return result;
        }

        public bool GetBoolParameter(string FeatureName)
        {
            bool result = false;
            try
            {
                if (libpylonc.DeviceFeatureIsAvailable(_cameraHandel, FeatureName))
                {
                    result = libpylonc.DeviceGetBooleanFeature(_cameraHandel, FeatureName);
                }
            }
            catch (Exception ex)
            {
                ShowException("GetBoolParameter", ex);
            }

            return result;
        }

        public bool SetFloatParameter(string FeatureName, float value)
        {
            bool result = false;
            try
            {
                if (libpylonc.DeviceFeatureIsAvailable(_cameraHandel, FeatureName))
                {
                    double num = libpylonc.DeviceGetFloatFeatureMin(_cameraHandel, FeatureName);
                    double num2 = libpylonc.DeviceGetFloatFeatureMax(_cameraHandel, FeatureName);
                    if ((double)value >= num && (double)value <= num2)
                    {
                        if (libpylonc.DeviceFeatureIsWriteable(_cameraHandel, FeatureName))
                        {
                            libpylonc.DeviceSetFloatFeature(_cameraHandel, FeatureName, value);
                            result = true;
                        }
                        else
                        {
                            ShowException("SetFloatParameter", new Exception("parameter is not writeable " + FeatureName));
                        }
                    }
                    else
                    {
                        ShowException("SetFloatParameter", new Exception($"parameter cam not be set {FeatureName} value {value}"));
                    }
                }
                else
                {
                    ShowException("SetFloatParameter", new Exception($"parameter not available {FeatureName} value {value}"));
                }
            }
            catch (Exception ex)
            {
                ShowException("SetFloatParameter", ex);
            }

            return result;
        }

        public double GetFloatParameter(string FeatureName)
        {
            double num = double.NaN;
            try
            {
                if (libpylonc.DeviceFeatureIsAvailable(_cameraHandel, FeatureName))
                {
                    num = libpylonc.DeviceGetFloatFeature(_cameraHandel, FeatureName);
                }
                else
                {
                    ShowException("GetFloatParameter", new Exception($"parameter not available {FeatureName} value {num}"));
                }
            }
            catch (Exception ex)
            {
                ShowException("GetFloatParameter", ex);
            }

            return num;
        }

        public int GetIntParameter(string FeatureName)
        {
            int num = -1;
            try
            {
                if (libpylonc.DeviceFeatureIsAvailable(_cameraHandel, FeatureName))
                {
                    num = libpylonc.DeviceGetIntegerFeatureInt32(_cameraHandel, FeatureName);
                }
                else
                {
                    ShowException("GetIntParameter", new Exception($"parameter not available {FeatureName} value {num}"));
                }
            }
            catch (Exception ex)
            {
                ShowException("SetIntParameter", ex);
            }

            return num;
        }

        public bool SetIntParameter(string FeatureName, int value)
        {
            bool result = false;
            try
            {
                if (libpylonc.DeviceFeatureIsAvailable(_cameraHandel, FeatureName))
                {
                    int num = libpylonc.DeviceGetIntegerFeatureIncInt32(_cameraHandel, FeatureName);
                    int num2 = libpylonc.DeviceGetIntegerFeatureMinInt32(_cameraHandel, FeatureName);
                    int num3 = libpylonc.DeviceGetIntegerFeatureMaxInt32(_cameraHandel, FeatureName);
                    int num4 = value - value % num;
                    if (num4 >= num2 && num4 <= num3)
                    {
                        if (libpylonc.DeviceFeatureIsWriteable(_cameraHandel, FeatureName))
                        {
                            libpylonc.DeviceSetIntegerFeatureInt32(_cameraHandel, FeatureName, num4);
                            result = true;
                        }
                        else
                        {
                            ShowException("SetIntParameter", new Exception("parameter is not writeable " + FeatureName));
                        }
                    }
                    else
                    {
                        ShowException("SetIntParameter", new Exception($"parameter cam not be set {FeatureName} value {value}"));
                    }
                }
                else
                {
                    ShowException("SetIntParameter", new Exception($"parameter not available {FeatureName} value {value}"));
                }
            }
            catch (Exception ex)
            {
                ShowException("SetIntParameter", ex);
            }

            return result;
        }

        public bool DeviceStartAcquisition(uint BufferCount = 20u)
        {
            bool result = false;
            try
            {
                if (_cameraHandel != null)
                {
                    Console.WriteLine("try DeviceFeatureIsAvailable -> EnumEntry_TriggerSelector_AcquisitionStart");
                    bool flag = libpylonc.DeviceFeatureIsAvailable(_cameraHandel, "EnumEntry_TriggerSelector_AcquisitionStart");
                    Console.WriteLine($"Can AcquisitionStartTrigger: {flag}");
                    if (flag)
                    {
                        Console.WriteLine("try DeviceFeatureFromString -> TriggerSelector = AcquisitionStart");
                        libpylonc.DeviceFeatureFromString(_cameraHandel, "TriggerSelector", "AcquisitionStart");
                        Console.WriteLine("try DeviceFeatureFromString -> TriggerMode = Off");
                        libpylonc.DeviceFeatureFromString(_cameraHandel, "TriggerMode", "Off");
                    }

                    Console.WriteLine("try DeviceFeatureIsAvailable -> EnumEntry_TriggerSelector_FrameBurstStart");
                    bool flag2 = libpylonc.DeviceFeatureIsAvailable(_cameraHandel, "EnumEntry_TriggerSelector_FrameBurstStart");
                    Console.WriteLine($"Can FrameBurstStartTrigger: {flag2}");
                    if (flag2)
                    {
                        Console.WriteLine("try DeviceFeatureFromString -> TriggerSelector = FrameBurstStart");
                        libpylonc.DeviceFeatureFromString(_cameraHandel, "TriggerSelector", "FrameBurstStart");
                        Console.WriteLine("try DeviceFeatureFromString -> TriggerMode = OFF");
                        libpylonc.DeviceFeatureFromString(_cameraHandel, "TriggerMode", "Off");
                    }

                    SwitchTriggerMode("On");
                    Console.WriteLine("try DeviceFeatureFromString -> AcquisitionMode = Continuous");
                    libpylonc.DeviceFeatureFromString(_cameraHandel, "AcquisitionMode", "Continuous");
                    Console.WriteLine("try StreamGrabberGetWaitObject");
                    WaitHandle waitHandle = libpylonc.StreamGrabberGetWaitObject(_streamGrabber);
                    ulong num = 0uL;
                    Console.WriteLine("try StreamGrabberGetPayloadSize");
                    num = libpylonc.StreamGrabberGetPayloadSize(_cameraHandel, _streamGrabber);
                    Console.WriteLine($"Grab payload size: {num:N0}");
                    Console.WriteLine("try StreamGrabberSetMaxNumBuffer");
                    libpylonc.StreamGrabberSetMaxNumBuffer(_streamGrabber, BufferCount);
                    Console.WriteLine($"Set buffer count: {BufferCount}");
                    Console.WriteLine("try StreamGrabberSetMaxBufferSize");
                    libpylonc.StreamGrabberSetMaxBufferSize(_streamGrabber, num);
                    Console.WriteLine($"Set buffer size: {num:N0}");
                    Console.WriteLine("try StreamGrabberPrepareGrabbing");
                    libpylonc.StreamGrabberPrepareGrab(_streamGrabber);
                    InitBuffers(BufferCount, num);
                    foreach (FrameBuffer framebuffer in Framebuffers)
                    {
                        framebuffer.pylonbuf = libpylonc.StreamGrabberRegisterBuffer(_streamGrabber, framebuffer.GetHandle().AddrOfPinnedObject(), num);
                        libpylonc.StreamGrabberQueueBuffer(_streamGrabber, framebuffer.pylonbuf, new IntPtr(framebuffer.BufferIndex));
                    }

                    Console.WriteLine("try StreamGrabberStartStreamingIfMandatory");
                    libpylonc.StreamGrabberStartStreamingIfMandatory(_streamGrabber);
                    Console.WriteLine("done StreamGrabberQueueBuffer");
                    Console.WriteLine("try start acquisition");
                    libpylonc.DeviceExecuteCommandFeature(_cameraHandel, "AcquisitionStart");
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                ShowException("DeviceStartAcquisition", ex);
            }

            return result;
        }

        public bool DeviceStopAcquisition()
        {
            if (IsRunning)
            {
                StopRequested = true;
                Console.WriteLine("On GrabTask waiting");
                GrabTask.Wait();
                Console.WriteLine("GrabTask returned");
            }

            bool result = false;
            try
            {
                if (_cameraHandel != null)
                {
                    libpylonc.DeviceExecuteCommandFeature(_cameraHandel, "AcquisitionStop");
                    libpylonc.StreamGrabberStopStreamingIfMandatory(_streamGrabber);
                    Console.WriteLine("try StreamGrabberFlushBuffersToOutput");
                    libpylonc.StreamGrabberFlushBuffersToOutput(_streamGrabber);
                    bool flag = true;
                    while (libpylonc.StreamGrabberRetrieveResult(_streamGrabber).Issuccessful)
                    {
                    }

                    Framebuffers.ForEach(delegate (FrameBuffer h)
                    {
                        libpylonc.StreamGrabberDeregisterBuffer(_streamGrabber, h.pylonbuf);
                    });
                    libpylonc.StreamGrabberFinishGrab(_streamGrabber);
                    Framebuffers.Clear();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                ShowException("DeviceStopAcquisition", ex);
            }

            return result;
        }

        public bool GrabContinues()
        {
            bool result = false;
            StopRequested = false;
            if (!IsRunning)
            {
                SwitchTriggerMode("Off");
                GrabTask = Task.Run(delegate
                {
                    GrabImages();
                });
            }

            return result;
        }

        private void GrabImages()
        {
            try
            {
                GrabResultEx grabResultEx = null;
                int num = -1;
                IsRunning = true;
                while (!StopRequested)
                {
                    WaitHandle waitHandle = libpylonc.StreamGrabberGetWaitObject(_streamGrabber);
                    if (libpylonc.WaitObjectWait(waitHandle, 5000u))
                    {
                        (bool Issuccessful, GrabResult GrabResult) tuple = libpylonc.StreamGrabberRetrieveResult(_streamGrabber);
                        bool item = tuple.Issuccessful;
                        GrabResult item2 = tuple.GrabResult;
                        num = (int)item2.Context;
                        if (item)
                        {
                            if (item2.Status == GrabStatus.Grabbed)
                            {
                                byte[] imageData = Framebuffers[num].GetImageData();
                                grabResultEx = new GrabResultEx(item2.SizeX, item2.SizeY, item2.BlockId, imageData);
                                grabResultEx.PixelType = item2.PixelType;
                                grabResultEx.ErrorCode = item2.ErrorCode;
                            }
                            else
                            {
                                Console.WriteLine("Grab was not successfully: " + item2.ErrorCode.ToString("X"));
                                grabResultEx = new GrabResultEx(-1, -1, ulong.MaxValue, new byte[1]);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Retrieve image was not successfully");
                            grabResultEx = new GrabResultEx(-1, -1, ulong.MaxValue, new byte[1]);
                        }

                        libpylonc.StreamGrabberQueueBuffer(_streamGrabber, Framebuffers[num].pylonbuf, new IntPtr(Framebuffers[num].BufferIndex));
                    }
                    else
                    {
                        Console.WriteLine("Wait for image was not successfully");
                        grabResultEx = new GrabResultEx(-1, -1, ulong.MaxValue, new byte[1]);
                    }

                    this.OnGrabImage?.Invoke(this, grabResultEx);
                }
            }
            catch (Exception ex)
            {
                IsRunning = false;
                ShowException("GrabImages", ex);
            }

            SwitchTriggerMode("On");
            IsRunning = false;
        }

        public void StopContinuesGrab()
        {
            StopRequested = true;
            GrabTask.Wait();
        }

        public bool InitializeCamera()
        {
            bool result = false;
            try
            {
                libpylonc.Initialize();
                Console.WriteLine("try EnumerateDevices");
                uint num = libpylonc.EnumerateDevices();
                Console.WriteLine($"Number of cameras: {num}");
                if (num == 0)
                {
                    return result;
                }

                _cameraHandel = libpylonc.CreateDeviceByIndex(0uL);
                Console.WriteLine("try DeviceOpen");
                libpylonc.DeviceOpen(_cameraHandel, CameraAccessMode.Control | CameraAccessMode.Stream);
                Console.WriteLine("try DeviceFeatureIsReadable -> DeviceModelName");
                bool value = libpylonc.DeviceFeatureIsReadable(_cameraHandel, "DeviceModelName");
                Console.WriteLine($"DeviceModelName isReadable: {value}");
                Console.WriteLine("try DeviceFeatureToString");
                string text = libpylonc.DeviceFeatureToString(_cameraHandel, "DeviceModelName");
                Console.WriteLine("DeviceModelName: " + text.Replace("\0", string.Empty));
                uint num2 = 0u;
                Console.WriteLine("try DeviceGetStreamGrabber");
                _streamGrabber = libpylonc.DeviceGetStreamGrabber(_cameraHandel, num2);
                Console.WriteLine($"Using stream grabber channel: {num2}");
                Console.WriteLine("try StreamGrabberOpen");
                libpylonc.StreamGrabberOpen(_streamGrabber);
                result = true;
            }
            catch (Exception ex)
            {
                ShowException(" InitializeCamera ", ex);
            }

            return result;
        }

        public bool InitializeCamera(string IPAddress)
        {
            bool result = false;
            try
            {
                libpylonc.Initialize();
                Console.WriteLine("try EnumerateDevices");
                uint num = libpylonc.EnumerateDevices();
                Console.WriteLine($"Number of cameras: {num}");
                if (num == 0)
                {
                    return result;
                }

                int cameraIndex = GetCameraIndex(num, IPAddress);
                if (cameraIndex >= 0)
                {
                    _cameraHandel = libpylonc.CreateDeviceByIndex((ulong)cameraIndex);
                    Console.WriteLine("try DeviceOpen");
                    libpylonc.DeviceOpen(_cameraHandel, CameraAccessMode.Control | CameraAccessMode.Stream);
                    Console.WriteLine("try DeviceFeatureIsReadable -> DeviceModelName");
                    bool value = libpylonc.DeviceFeatureIsReadable(_cameraHandel, "DeviceModelName");
                    Console.WriteLine($"DeviceModelName isReadable: {value}");
                    Console.WriteLine("try DeviceFeatureToString");
                    string text = libpylonc.DeviceFeatureToString(_cameraHandel, "DeviceModelName");
                    Console.WriteLine("DeviceModelName: " + text.Replace("\0", string.Empty));
                    uint num2 = 0u;
                    Console.WriteLine("try DeviceGetStreamGrabber");
                    _streamGrabber = libpylonc.DeviceGetStreamGrabber(_cameraHandel, num2);
                    Console.WriteLine($"Using stream grabber channel: {num2}");
                    Console.WriteLine("try StreamGrabberOpen");
                    libpylonc.StreamGrabberOpen(_streamGrabber);
                    result = true;
                }
                else
                {
                    Console.WriteLine("camera with following IP not found: " + IPAddress);
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
                libpylonc.Terminate();
            }

            return result;
        }

        public GrabResultEx GrabSingleImage()
        {
            return GrabSingleImageSW(_streamGrabber);
        }

        private GrabResultEx GrabSingleImageSW(StreamGrabberHandle streamGrabber)
        {
            GrabResultEx grabResultEx = null;
            int num = -1;
            try
            {
                libpylonc.DeviceExecuteCommandFeature(_cameraHandel, "TriggerSoftware");
                WaitHandle waitHandle = libpylonc.StreamGrabberGetWaitObject(streamGrabber);
                if (libpylonc.WaitObjectWait(waitHandle, timeoutMs: 5000u))
                {
                    (bool Issuccessful, GrabResult GrabResult) tuple = libpylonc.StreamGrabberRetrieveResult(_streamGrabber);
                    bool item = tuple.Issuccessful;
                    GrabResult item2 = tuple.GrabResult;
                    num = (int)item2.Context;
                    if (item)
                    {
                        if (item2.Status == GrabStatus.Grabbed)
                        {
                            byte[] imageData = Framebuffers[num].GetImageData();
                            grabResultEx = new GrabResultEx(item2.SizeX, item2.SizeY, item2.BlockId, imageData);
                            grabResultEx.PixelType = item2.PixelType;
                            grabResultEx.ErrorCode = item2.ErrorCode;
                        }
                        else
                        {
                            Console.WriteLine("Grab was not successful: " + item2.ErrorCode.ToString("X"));
                            grabResultEx = new GrabResultEx(-1, -1, ulong.MaxValue, new byte[1]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Retrieve image was not successful");
                        grabResultEx = new GrabResultEx(-1, -1, ulong.MaxValue, new byte[1]);
                    }

                    libpylonc.StreamGrabberQueueBuffer(_streamGrabber, Framebuffers[num].pylonbuf, new IntPtr(Framebuffers[num].BufferIndex));
                }
                else
                {
                    Console.WriteLine("Wait for image was not successful");
                    grabResultEx = new GrabResultEx(-1, -1, ulong.MaxValue, new byte[1]);
                }
            }
            catch (Exception ex)
            {
                ShowException("GrabSingleImageSW", ex);
            }

            return grabResultEx;
        }

        private void PixelConverter(GrabResultEx gr)
        {
            ConverterHandle converterHandle = libpylonc.ImageFormatConverterCreate();
            int num = 2;
        }

        public void DestroyCamera()
        {
            try
            {
                if (_cameraHandel != null)
                {
                    libpylonc.DeviceExecuteCommandFeature(_cameraHandel, "AcquisitionStop");
                    SwitchTriggerMode("Off");
                    libpylonc.StreamGrabberStopStreamingIfMandatory(_streamGrabber);
                    Console.WriteLine("try StreamGrabberFlushBuffersToOutput");
                    libpylonc.StreamGrabberFlushBuffersToOutput(_streamGrabber);
                    bool flag = true;
                    while (libpylonc.StreamGrabberRetrieveResult(_streamGrabber).Issuccessful)
                    {
                    }

                    Framebuffers.ForEach(delegate (FrameBuffer h)
                    {
                        libpylonc.StreamGrabberDeregisterBuffer(_streamGrabber, h.pylonbuf);
                    });
                    Framebuffers.Clear();
                    Console.WriteLine("try DestroyDevice");
                    libpylonc.DeviceClose(_cameraHandel);
                    libpylonc.DestroyDevice(_cameraHandel);
                    Terminate();
                }
            }
            catch (Exception ex)
            {
                ShowException("DestroyCamera", ex);
            }
        }

        private void ShowException(Exception ex)
        {
            StackTrace stackTrace = new StackTrace(ex, fNeedFileInfo: true);
            Console.WriteLine("Exception occurred in method " + (stackTrace.GetFrame(0)?.GetMethod())?.Name);
            Console.WriteLine("Error Message  : " + ex.Message);
        }

        private void ShowException(string FunctionName, Exception ex)
        {
            Console.WriteLine("Error occurred in " + FunctionName + " : " + ex.Message);
        }

        private int GetCameraIndex(uint numDevices, string IP_Address)
        {
            uint num = 0u;
            if (numDevices != 0)
            {
                for (num = 0u; num < numDevices; num++)
                {
                    try
                    {
                        DeviceInfoHandle deviceInfo = libpylonc.DeviceGetInfoHandle(num);
                        string text = libpylonc.DeviceInfoGetPropertyValueByName(deviceInfo, "DeviceClass");
                        text = text.Replace("\0", string.Empty);
                        if (text.Contains("BaslerGigE"))
                        {
                            string text2 = libpylonc.DeviceInfoGetPropertyValueByName(deviceInfo, "IpAddress");
                            text2 = text2.Replace("\0", string.Empty);
                            if (text2.Contains(IP_Address))
                            {
                                return (int)num;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowException("GetCameraIndex", ex);
                    }
                }
            }

            return -1;
        }

        private void InitBuffers(uint count, ulong PayloadSize)
        {
            bool flag = false;
            foreach (FrameBuffer framebuffer in Framebuffers)
            {
                if (framebuffer.PayloadSize != PayloadSize)
                {
                    framebuffer.Dispose();
                    flag = true;
                }
            }

            if (flag)
            {
                Framebuffers.Clear();
            }

            for (int i = 0; i < count; i++)
            {
                FrameBuffer frameBuffer = new FrameBuffer(PayloadSize);
                frameBuffer.BufferIndex = i;
                Framebuffers.Add(frameBuffer);
            }
        }

        private void Terminate()
        {
            libpylonc.Terminate();
        }

        private void SwitchTriggerMode(string Mode)
        {
            if (_cameraHandel != null)
            {
                Console.WriteLine("try DeviceFeatureIsAvailable -> EnumEntry_TriggerSelector_FrameStart");
                if (libpylonc.DeviceFeatureIsAvailable(_cameraHandel, "EnumEntry_TriggerSelector_FrameStart"))
                {
                    Console.WriteLine("try DeviceFeatureFromString -> TriggerSelector = FrameStart");
                    libpylonc.DeviceFeatureFromString(_cameraHandel, "TriggerSelector", "FrameStart");
                    libpylonc.DeviceFeatureFromString(_cameraHandel, "TriggerSource", "Software");
                    Console.WriteLine("try DeviceFeatureFromString -> TriggerMode = " + Mode);
                    libpylonc.DeviceFeatureFromString(_cameraHandel, "TriggerMode", Mode);
                }
            }
        }

        private byte[] GetSignalImage()
        {
            Console.WriteLine("try Initialize");
            libpylonc.Initialize();
            Console.WriteLine("try EnumerateDevices");
            uint value2 = libpylonc.EnumerateDevices();
            Console.WriteLine($"Number of cameras: {value2}");
            Console.WriteLine("try CreateDeviceByIndex");
            CameraHandle camera = libpylonc.CreateDeviceByIndex(0uL);
            Console.WriteLine("try DeviceOpen");
            libpylonc.DeviceOpen(camera, CameraAccessMode.Control | CameraAccessMode.Stream);
            Console.WriteLine("try DeviceFeatureIsReadable -> DeviceModelName");
            bool value3 = libpylonc.DeviceFeatureIsReadable(camera, "DeviceModelName");
            Console.WriteLine($"DeviceModelName isReadable: {value3}");
            Console.WriteLine("try DeviceFeatureToString");
            string text = libpylonc.DeviceFeatureToString(camera, "DeviceModelName");
            int num = text.IndexOf('\\');
            Console.WriteLine("DeviceModelName: " + text.Trim());
            Console.WriteLine($"MemoryUsage: {Process.GetCurrentProcess().PrivateMemorySize64:N0}");
            Console.WriteLine("try DeviceFeatureIsAvailable -> EnumEntry_PixelFormat_Mono8");
            bool flag = libpylonc.DeviceFeatureIsAvailable(camera, "EnumEntry_PixelFormat_Mono8");
            Console.WriteLine($"Can Mono8: {flag}");
            if (flag)
            {
                Console.WriteLine("try DeviceFeatureFromString -> PixelFormat = Mono8");
                libpylonc.DeviceFeatureFromString(camera, "PixelFormat", "Mono8");
            }

            Console.WriteLine("try DeviceFeatureIsAvailable -> EnumEntry_TriggerSelector_AcquisitionStart");
            bool flag2 = libpylonc.DeviceFeatureIsAvailable(camera, "EnumEntry_TriggerSelector_AcquisitionStart");
            Console.WriteLine($"Can AcquisitionStartTrigger: {flag2}");
            if (flag2)
            {
                Console.WriteLine("try DeviceFeatureFromString -> TriggerSelector = AcquisitionStart");
                libpylonc.DeviceFeatureFromString(camera, "TriggerSelector", "AcquisitionStart");
                Console.WriteLine("try DeviceFeatureFromString -> TriggerMode = Off");
                libpylonc.DeviceFeatureFromString(camera, "TriggerMode", "Off");
            }

            Console.WriteLine("try DeviceFeatureIsAvailable -> EnumEntry_TriggerSelector_FrameBurstStart");
            bool flag3 = libpylonc.DeviceFeatureIsAvailable(camera, "EnumEntry_TriggerSelector_FrameBurstStart");
            Console.WriteLine($"Can FrameBurstStartTrigger: {flag3}");
            if (flag3)
            {
                Console.WriteLine("try DeviceFeatureFromString -> TriggerSelector = FrameBurstStart");
                libpylonc.DeviceFeatureFromString(camera, "TriggerSelector", "FrameBurstStart");
                Console.WriteLine("try DeviceFeatureFromString -> TriggerMode = Off");
                libpylonc.DeviceFeatureFromString(camera, "TriggerMode", "Off");
            }

            Console.WriteLine("try DeviceFeatureIsAvailable -> EnumEntry_TriggerSelector_FrameStart");
            bool flag4 = libpylonc.DeviceFeatureIsAvailable(camera, "EnumEntry_TriggerSelector_FrameStart");
            Console.WriteLine($"Can FrameStartTrigger: {flag4}");
            if (flag4)
            {
                Console.WriteLine("try DeviceFeatureFromString -> TriggerSelector = FrameStart");
                libpylonc.DeviceFeatureFromString(camera, "TriggerSelector", "FrameStart");
                Console.WriteLine("try DeviceFeatureFromString -> TriggerMode = Off");
                libpylonc.DeviceFeatureFromString(camera, "TriggerMode", "Off");
            }

            Console.WriteLine("try DeviceFeatureFromString -> AcquisitionMode = Continuous");
            libpylonc.DeviceFeatureFromString(camera, "AcquisitionMode", "Continuous");
            Console.WriteLine("try DeviceGetNumStreamGrabberChannels");
            uint value4 = libpylonc.DeviceGetNumStreamGrabberChannels(camera);
            Console.WriteLine($"ChannelCount: {value4}");
            uint num2 = 0u;
            Console.WriteLine("try DeviceGetStreamGrabber");
            StreamGrabberHandle streamGrabber = libpylonc.DeviceGetStreamGrabber(camera, num2);
            Console.WriteLine($"Using stream grabber channel: {num2}");
            Console.WriteLine("try StreamGrabberOpen");
            libpylonc.StreamGrabberOpen(streamGrabber);
            uint payloadSize = 0u;
            try
            {
                Console.WriteLine("try StreamGrabberGetWaitObject");
                WaitHandle waitHandle = libpylonc.StreamGrabberGetWaitObject(streamGrabber);
                Console.WriteLine("try StreamGrabberGetPayloadSize");
                payloadSize = libpylonc.StreamGrabberGetPayloadSize(camera, streamGrabber);
                Console.WriteLine($"Grab payload size: {payloadSize:N0}");
                uint num3 = 10u;
                Console.WriteLine("try StreamGrabberSetMaxNumBuffer");
                libpylonc.StreamGrabberSetMaxNumBuffer(streamGrabber, num3);
                Console.WriteLine($"Set buffer count: {num3}");
                Console.WriteLine("try StreamGrabberSetMaxBufferSize");
                libpylonc.StreamGrabberSetMaxBufferSize(streamGrabber, payloadSize);
                Console.WriteLine($"Set buffer size: {payloadSize:N0}");
                Console.WriteLine("try StreamGrabberPrepareGrabbing");
                libpylonc.StreamGrabberPrepareGrab(streamGrabber);
                try
                {
                    List<byte[]> list = (from _ in Enumerable.Range(0, (int)num3)
                                         select new byte[payloadSize]).ToList();
                    List<GCHandle> list2 = list.Select((byte[] b) => GCHandle.Alloc(b, GCHandleType.Pinned)).ToList();
                    try
                    {
                        List<IntPtr> source = list2.Select((GCHandle p) => p.AddrOfPinnedObject()).ToList();
                        Console.WriteLine("try StreamGrabberRegisterBuffer");
                        List<StreamBufferHandle> list3 = source.Select((IntPtr p) => libpylonc.StreamGrabberRegisterBuffer(streamGrabber, p, payloadSize)).ToList();
                        Console.WriteLine("done StreamGrabberRegisterBuffer");
                        try
                        {
                            byte[] value5 = new byte[1] { 1 };
                            GCHandle gCHandle = GCHandle.Alloc(value5, GCHandleType.Pinned);
                            IntPtr contextPointer1 = gCHandle.AddrOfPinnedObject();
                            try
                            {
                                Console.WriteLine("try StreamGrabberQueueBuffer");
                                list3.ForEach(delegate (StreamBufferHandle h)
                                {
                                    libpylonc.StreamGrabberQueueBuffer(streamGrabber, h, contextPointer1);
                                });
                                Console.WriteLine("done StreamGrabberQueueBuffer");
                                Console.WriteLine("try StreamGrabberStartStreamingIfMandatory");
                                libpylonc.StreamGrabberStartStreamingIfMandatory(streamGrabber);
                                try
                                {
                                    Console.WriteLine($"MemoryUsage: {Process.GetCurrentProcess().PrivateMemorySize64:N0}");
                                    Console.WriteLine("try start acquisition");
                                    libpylonc.DeviceExecuteCommandFeature(camera, "AcquisitionStart");
                                    try
                                    {
                                        Console.WriteLine("Waiting...");
                                        Console.WriteLine("Waiting finished");
                                        Console.WriteLine("try WaitObjectWait");
                                        bool flag5 = libpylonc.WaitObjectWait(waitHandle, 1000u);
                                        Console.WriteLine("done WaitObjectWait");
                                        if (flag5)
                                        {
                                            Console.WriteLine("Wait successful");
                                            Console.WriteLine("try StreamGrabberRetrieveResult");
                                            var (flag6, grabResult) = libpylonc.StreamGrabberRetrieveResult(streamGrabber);
                                            Console.WriteLine("done StreamGrabberRetrieveResult");
                                            if (flag6)
                                            {
                                                if (grabResult.Status == GrabStatus.Grabbed)
                                                {
                                                    long value6 = list[0].Aggregate(0L, (long aggregate, byte value) => aggregate + value);
                                                    Console.WriteLine($"Buffer 0 sum: {value6:N0}");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Grab was not successful: " + grabResult.ErrorCode.ToString("X"));
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Retrieve image was not successful");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Wait for image was not successful");
                                        }
                                    }
                                    finally
                                    {
                                        Console.WriteLine("try stop acquisition");
                                        libpylonc.DeviceExecuteCommandFeature(camera, "AcquisitionStop");
                                    }
                                }
                                finally
                                {
                                    Console.WriteLine("try StreamGrabberStopStreamingIfMandatory");
                                    libpylonc.StreamGrabberStopStreamingIfMandatory(streamGrabber);
                                }
                            }
                            finally
                            {
                                gCHandle.Free();
                            }

                            Console.WriteLine("try StreamGrabberFlushBuffersToOutput");
                            libpylonc.StreamGrabberFlushBuffersToOutput(streamGrabber);
                            bool flag7 = true;
                            do
                            {
                                Console.WriteLine("try StreamGrabberRetrieveResult");
                            }
                            while (libpylonc.StreamGrabberRetrieveResult(streamGrabber).Issuccessful);
                        }
                        finally
                        {
                            Console.WriteLine("try StreamGrabberDeregisterBuffer");
                            list3.ForEach(delegate (StreamBufferHandle h)
                            {
                                libpylonc.StreamGrabberDeregisterBuffer(streamGrabber, h);
                            });
                            Console.WriteLine("done StreamGrabberDeregisterBuffer");
                        }
                    }
                    finally
                    {
                        list2.ForEach(delegate (GCHandle x)
                        {
                            x.Free();
                        });
                    }
                }
                finally
                {
                    Console.WriteLine("try StreamGrabberFinishGrabbing");
                    libpylonc.StreamGrabberFinishGrab(streamGrabber);
                }
            }
            finally
            {
                Console.WriteLine("try StreamGrabberClose");
                libpylonc.StreamGrabberClose(streamGrabber);
            }

            return new byte[100];
        }
    }
}
