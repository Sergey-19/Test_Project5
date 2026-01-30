using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Basler.pyloncore
{
    internal class libpylonc
    {
        static libpylonc()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
        }

        private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == "libpylonc")
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return NativeLibrary.Load("PylonC", assembly, searchPath);
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return NativeLibrary.Load("libpylonc", assembly, searchPath);
                }
            }

            return IntPtr.Zero;
        }

        private static T Execute<T>(Func<uint> nativeFunction, Func<T> onSuccess)
        {
            uint num = nativeFunction();
            if (num != 0)
            {
                Console.WriteLine($"Failed with {num:X}");
                throw new Exception($"Failed with {num:X}");
            }

            return onSuccess();
        }

        private static void Execute(Func<uint> nativeFunction)
        {
            Execute(nativeFunction, () => 0);
        }

        private static bool ToBool(byte b)
        {
            return b != 0;
        }

        [DllImport("libpylonc")]
        private static extern void PylonInitialize();

        public static void Initialize()
        {
            PylonInitialize();
        }

        [DllImport("libpylonc")]
        private static extern void PylonTerminate();

        public static void Terminate()
        {
            PylonTerminate();
        }

        [DllImport("libpylonc")]
        private static extern uint PylonEnumerateDevices(out uint numberOfDevices);

        public static uint EnumerateDevices()
        {
            uint numberOfDevices = 0u;
            return Execute(() => PylonEnumerateDevices(out numberOfDevices), () => numberOfDevices);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonCreateDeviceByIndex(ulong deviceIndex, out IntPtr handle);

        public static CameraHandle CreateDeviceByIndex(ulong index)
        {
            IntPtr handle = IntPtr.Zero;
            return Execute(() => PylonCreateDeviceByIndex(index, out handle), () => new CameraHandle(handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDestroyDevice(IntPtr cameraHandle);

        public static void DestroyDevice(CameraHandle camera)
        {
            CameraHandle camera2 = camera;
            Execute(() => PylonDestroyDevice(camera2.Handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceClose(IntPtr cameraHandle);

        public static void DeviceClose(CameraHandle camera)
        {
            CameraHandle camera2 = camera;
            Execute(() => PylonDeviceClose(camera2.Handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceOpen(IntPtr cameraHandle, CameraAccessMode accessMode);

        public static void DeviceOpen(CameraHandle camera, CameraAccessMode accessMode)
        {
            CameraHandle camera2 = camera;
            Execute(() => PylonDeviceOpen(camera2.Handle, accessMode));
        }

        [DllImport("libpylonc")]
        private static extern byte PylonDeviceFeatureIsReadable(IntPtr cameraHandle, string feature);

        public static bool DeviceFeatureIsReadable(CameraHandle camera, string feature)
        {
            byte b = PylonDeviceFeatureIsReadable(camera.Handle, feature);
            return ToBool(b);
        }

        [DllImport("libpylonc")]
        private static extern byte PylonDeviceFeatureIsWritable(IntPtr cameraHandle, string feature);

        public static bool DeviceFeatureIsWriteable(CameraHandle camera, string feature)
        {
            byte b = PylonDeviceFeatureIsWritable(camera.Handle, feature);
            return ToBool(b);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceFeatureToString(IntPtr cameraHandle, string feature, byte[] buffer, out ulong bufferSize);

        public static string DeviceFeatureToString(CameraHandle camera, string feature)
        {
            CameraHandle camera2 = camera;
            string feature2 = feature;
            byte[] resultBuffer = new byte[256];
            ulong resultBufferLength = (ulong)resultBuffer.Length;
            return Execute(() => PylonDeviceFeatureToString(camera2.Handle, feature2, resultBuffer, out resultBufferLength), () => Encoding.UTF8.GetString(resultBuffer));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGrabSingleFrame(IntPtr cameraHandle, ulong channel, IntPtr buffer, ulong bufferSize, out GrabResult grabResult, IntPtr bufferReady, uint timeoutMs);

        public static byte[] DeviceGrabSingleFrame(CameraHandle camera, ulong bufferSize)
        {
            CameraHandle camera2 = camera;
            byte[] buffer = new byte[bufferSize];
            uint channel = 0u;
            uint timeout = 10000u;
            GrabResult grabResult = default(GrabResult);
            GCHandle gCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr bufferPointer = gCHandle.AddrOfPinnedObject();
            bool flag = false;
            GCHandle gCHandle2 = GCHandle.Alloc(flag, GCHandleType.Pinned);
            IntPtr resultPointer = gCHandle2.AddrOfPinnedObject();
            try
            {
                return Execute(() => PylonDeviceGrabSingleFrame(camera2.Handle, channel, bufferPointer, bufferSize, out grabResult, resultPointer, timeout), () => buffer);
            }
            finally
            {
                gCHandle2.Free();
                gCHandle.Free();
            }
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGetNumStreamGrabberChannels(IntPtr cameraHandle, out uint channelCount);

        public static uint DeviceGetNumStreamGrabberChannels(CameraHandle camera)
        {
            CameraHandle camera2 = camera;
            uint channelCount = 0u;
            return Execute(() => PylonDeviceGetNumStreamGrabberChannels(camera2.Handle, out channelCount), () => channelCount);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGetStreamGrabber(IntPtr cameraHandle, uint channelIndex, out IntPtr streamGrabberHandle);

        public static StreamGrabberHandle DeviceGetStreamGrabber(CameraHandle camera, uint channelIndex)
        {
            CameraHandle camera2 = camera;
            IntPtr streamGrabberHandle = IntPtr.Zero;
            return Execute(() => PylonDeviceGetStreamGrabber(camera2.Handle, channelIndex, out streamGrabberHandle), () => new StreamGrabberHandle(streamGrabberHandle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberOpen(IntPtr streamGrabberHandle);

        public static void StreamGrabberOpen(StreamGrabberHandle streamGrabber)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            Execute(() => PylonStreamGrabberOpen(streamGrabber2.Handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberClose(IntPtr streamGrabberHandle);

        public static void StreamGrabberClose(StreamGrabberHandle streamGrabber)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            Execute(() => PylonStreamGrabberClose(streamGrabber2.Handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberGetPayloadSize(IntPtr cameraHandle, IntPtr streamGrabberHandle, out uint payloadSize);

        public static uint StreamGrabberGetPayloadSize(CameraHandle camera, StreamGrabberHandle streamGrabber)
        {
            CameraHandle camera2 = camera;
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            uint payloadSize = 0u;
            return Execute(() => PylonStreamGrabberGetPayloadSize(camera2.Handle, streamGrabber2.Handle, out payloadSize), () => payloadSize);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberSetMaxNumBuffer(IntPtr streamGrabberHandle, uint maxNumberOfBuffers);

        public static void StreamGrabberSetMaxNumBuffer(StreamGrabberHandle streamGrabber, uint maxNumberOfBuffers)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            Execute(() => PylonStreamGrabberSetMaxNumBuffer(streamGrabber2.Handle, maxNumberOfBuffers));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberSetMaxBufferSize(IntPtr streamGrabberHandle, ulong maxBufferSize);

        public static void StreamGrabberSetMaxBufferSize(StreamGrabberHandle streamGrabber, ulong maxBufferSize)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            Execute(() => PylonStreamGrabberSetMaxBufferSize(streamGrabber2.Handle, maxBufferSize));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberPrepareGrab(IntPtr streamGrabberHandle);

        public static void StreamGrabberPrepareGrab(StreamGrabberHandle streamGrabber)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            Execute(() => PylonStreamGrabberPrepareGrab(streamGrabber2.Handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberRegisterBuffer(IntPtr streamGrabberHandle, IntPtr buffer, ulong bufferSize, out IntPtr streamBufferHandle);

        public static StreamBufferHandle StreamGrabberRegisterBuffer(StreamGrabberHandle streamGrabber, IntPtr buffer, ulong bufferSize)
        {
            IntPtr streamBufferHandle = IntPtr.Zero;
            uint num = PylonStreamGrabberRegisterBuffer(streamGrabber.Handle, buffer, bufferSize, out streamBufferHandle);
            if (num != 0)
            {
                Console.WriteLine("Failed with " + num.ToString("X"));
                throw new Exception("Failed with " + num.ToString("X"));
            }

            return new StreamBufferHandle(streamBufferHandle);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberQueueBuffer(IntPtr streamGrabberHandle, IntPtr streamBufferHandle, IntPtr contextData);

        public static void StreamGrabberQueueBuffer(StreamGrabberHandle streamGrabber, StreamBufferHandle streamBuffer, IntPtr contextDataHandle)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            StreamBufferHandle streamBuffer2 = streamBuffer;
            Execute(() => PylonStreamGrabberQueueBuffer(streamGrabber2.Handle, streamBuffer2.Handle, contextDataHandle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberDeregisterBuffer(IntPtr streamGrabber, IntPtr streamBuffer);

        public static void StreamGrabberDeregisterBuffer(StreamGrabberHandle streamGrabberHandle, StreamBufferHandle streamBufferHandle)
        {
            StreamGrabberHandle streamGrabberHandle2 = streamGrabberHandle;
            StreamBufferHandle streamBufferHandle2 = streamBufferHandle;
            Execute(() => PylonStreamGrabberDeregisterBuffer(streamGrabberHandle2.Handle, streamBufferHandle2.Handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberStartStreamingIfMandatory(IntPtr streamGrabberHandle);

        public static void StreamGrabberStartStreamingIfMandatory(StreamGrabberHandle streamGrabber)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            Execute(() => PylonStreamGrabberStartStreamingIfMandatory(streamGrabber2.Handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberStopStreamingIfMandatory(IntPtr streamGrabberHandle);

        public static void StreamGrabberStopStreamingIfMandatory(StreamGrabberHandle streamGrabber)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            Execute(() => PylonStreamGrabberStopStreamingIfMandatory(streamGrabber2.Handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberFlushBuffersToOutput(IntPtr streamGrabberHandle);

        public static void StreamGrabberFlushBuffersToOutput(StreamGrabberHandle streamGrabber)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            Execute(() => PylonStreamGrabberFlushBuffersToOutput(streamGrabber2.Handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberFinishGrab(IntPtr streamGrabberHandle);

        public static void StreamGrabberFinishGrab(StreamGrabberHandle streamGrabber)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            Execute(() => PylonStreamGrabberFinishGrab(streamGrabber2.Handle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberGetWaitObject(IntPtr streamGrabberHandle, out IntPtr waitHandle);

        public static WaitHandle StreamGrabberGetWaitObject(StreamGrabberHandle streamGrabber)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            IntPtr waitHandle = IntPtr.Zero;
            return Execute(() => PylonStreamGrabberGetWaitObject(streamGrabber2.Handle, out waitHandle), () => new WaitHandle(waitHandle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonWaitObjectWait(IntPtr waitHandle, uint timeoutMs, out byte isSuccesfull);

        public static bool WaitObjectWait(WaitHandle waitHandle, uint timeoutMs)
        {
            WaitHandle waitHandle2 = waitHandle;
            byte issuccessful = 0;
            return Execute(() => PylonWaitObjectWait(waitHandle2.Handle, timeoutMs, out issuccessful), () => ToBool(issuccessful));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonStreamGrabberRetrieveResult(IntPtr streamGrabberHandle, out GrabResult grabResult, out byte isSuccesfull);

        public static (bool Issuccessful, GrabResult GrabResult) StreamGrabberRetrieveResult(StreamGrabberHandle streamGrabber)
        {
            StreamGrabberHandle streamGrabber2 = streamGrabber;
            byte issuccessful = 0;
            GrabResult grabResult = default(GrabResult);
            return Execute(() => PylonStreamGrabberRetrieveResult(streamGrabber2.Handle, out grabResult, out issuccessful), () => (ToBool(issuccessful), grabResult));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceExecuteCommandFeature(IntPtr cameraHandle, string commandName);

        public static void DeviceExecuteCommandFeature(CameraHandle camera, string commandName)
        {
            CameraHandle camera2 = camera;
            string commandName2 = commandName;
            Execute(() => PylonDeviceExecuteCommandFeature(camera2.Handle, commandName2));
        }

        [DllImport("libpylonc")]
        private static extern byte PylonDeviceFeatureIsAvailable(IntPtr cameraHandle, string name);

        public static bool DeviceFeatureIsAvailable(CameraHandle camera, string name)
        {
            byte b = PylonDeviceFeatureIsAvailable(camera.Handle, name);
            return ToBool(b);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceFeatureFromString(IntPtr deviceHandle, string name, string value);

        public static void DeviceFeatureFromString(CameraHandle camera, string name, string value)
        {
            CameraHandle camera2 = camera;
            string name2 = name;
            string value2 = value;
            Execute(() => PylonDeviceFeatureFromString(camera2.Handle, name2, value2));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonGetDeviceInfoHandle(uint cameraIndex, out IntPtr deviceinfo);

        public static DeviceInfoHandle DeviceGetInfoHandle(uint cameraIndex)
        {
            IntPtr deviceInfoHandle = IntPtr.Zero;
            return Execute(() => PylonGetDeviceInfoHandle(cameraIndex, out deviceInfoHandle), () => new DeviceInfoHandle(deviceInfoHandle));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceInfoGetPropertyValueByName(IntPtr DeviceInfoHandle, string feature, byte[] buffer, out ulong bufferSize);

        public static string DeviceInfoGetPropertyValueByName(DeviceInfoHandle DeviceInfo, string feature)
        {
            DeviceInfoHandle DeviceInfo2 = DeviceInfo;
            string feature2 = feature;
            byte[] resultBuffer = new byte[256];
            ulong resultBufferLength = (ulong)resultBuffer.Length;
            return Execute(() => PylonDeviceInfoGetPropertyValueByName(DeviceInfo2.Handle, feature2, resultBuffer, out resultBufferLength), () => Encoding.UTF8.GetString(resultBuffer));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGetIntegerFeatureInt32(IntPtr cameraHandle, string FeatureName, out int minvalue);

        public static int DeviceGetIntegerFeatureInt32(CameraHandle camera, string FeatureName)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            int value = 0;
            return Execute(() => PylonDeviceGetIntegerFeatureInt32(camera2.Handle, FeatureName2, out value), () => value);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGetIntegerFeatureMinInt32(IntPtr cameraHandle, string FeatureName, out int minvalue);

        public static int DeviceGetIntegerFeatureMinInt32(CameraHandle camera, string FeatureName)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            int minvalue = 0;
            return Execute(() => PylonDeviceGetIntegerFeatureMinInt32(camera2.Handle, FeatureName2, out minvalue), () => minvalue);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGetIntegerFeatureMaxInt32(IntPtr cameraHandle, string FeatureName, out int minvalue);

        public static int DeviceGetIntegerFeatureMaxInt32(CameraHandle camera, string FeatureName)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            int maxvalue = 0;
            return Execute(() => PylonDeviceGetIntegerFeatureMaxInt32(camera2.Handle, FeatureName2, out maxvalue), () => maxvalue);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGetIntegerFeatureIncInt32(IntPtr cameraHandle, string FeatureName, out int minvalue);

        public static int DeviceGetIntegerFeatureIncInt32(CameraHandle camera, string FeatureName)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            int IncValue = 0;
            return Execute(() => PylonDeviceGetIntegerFeatureIncInt32(camera2.Handle, FeatureName2, out IncValue), () => IncValue);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceSetIntegerFeatureInt32(IntPtr cameraHandle, string FeatureName, int value);

        public static void DeviceSetIntegerFeatureInt32(CameraHandle camera, string FeatureName, int value)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            Execute(() => PylonDeviceSetIntegerFeatureInt32(camera2.Handle, FeatureName2, value));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGetFloatFeature(IntPtr cameraHandle, string FeatureName, out double minvalue);

        public static double DeviceGetFloatFeature(CameraHandle camera, string FeatureName)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            double value = double.NaN;
            return Execute(() => PylonDeviceGetFloatFeature(camera2.Handle, FeatureName2, out value), () => value);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGetFloatFeatureMin(IntPtr cameraHandle, string FeatureName, out double minvalue);

        public static double DeviceGetFloatFeatureMin(CameraHandle camera, string FeatureName)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            double minvalue = double.NaN;
            return Execute(() => PylonDeviceGetFloatFeatureMin(camera2.Handle, FeatureName2, out minvalue), () => minvalue);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGetFloatFeatureMax(IntPtr cameraHandle, string FeatureName, out double maxValue);

        public static double DeviceGetFloatFeatureMax(CameraHandle camera, string FeatureName)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            double maxvalue = double.NaN;
            return Execute(() => PylonDeviceGetFloatFeatureMax(camera2.Handle, FeatureName2, out maxvalue), () => maxvalue);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceSetFloatFeature(IntPtr cameraHandle, string FeatureName, double value);

        public static void DeviceSetFloatFeature(CameraHandle camera, string FeatureName, double value)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            Execute(() => PylonDeviceSetFloatFeature(camera2.Handle, FeatureName2, value));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceGetBooleanFeature(IntPtr cameraHandle, string FeatureName, out bool value);

        public static bool DeviceGetBooleanFeature(CameraHandle camera, string FeatureName)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            bool value = false;
            return Execute(() => PylonDeviceGetBooleanFeature(camera2.Handle, FeatureName2, out value), () => value);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonDeviceSetBooleanFeature(IntPtr cameraHandle, string FeatureName, bool value);

        public static bool DeviceSetBooleanFeature(CameraHandle camera, string FeatureName, bool value)
        {
            CameraHandle camera2 = camera;
            string FeatureName2 = FeatureName;
            return Execute(() => PylonDeviceSetBooleanFeature(camera2.Handle, FeatureName2, value), () => value);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonImageFormatConverterCreate(out IntPtr converterhandle);

        public static ConverterHandle ImageFormatConverterCreate()
        {
            IntPtr hconverter = IntPtr.Zero;
            return Execute(() => PylonImageFormatConverterCreate(out hconverter), () => new ConverterHandle(hconverter));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonImageFormatConverterSetOutputPixelFormat(IntPtr converterhandle, string PixelFormat);

        public static void ImageFormatConverterSetOutputPixelFormat(ConverterHandle hConverter, string PixelFormat)
        {
            ConverterHandle hConverter2 = hConverter;
            string PixelFormat2 = PixelFormat;
            Execute(() => PylonImageFormatConverterSetOutputPixelFormat(hConverter2.Handle, PixelFormat2));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonImageFormatConverterSetOutputPaddingX(IntPtr converterhandle, int PaddingX);

        public static void FormatConverterSetOutputPixelFormat(ConverterHandle hConverter, int PaddingX)
        {
            ConverterHandle hConverter2 = hConverter;
            Execute(() => PylonImageFormatConverterSetOutputPaddingX(hConverter2.Handle, PaddingX));
        }

        [DllImport("libpylonc")]
        private static extern uint PylonImageFormatConverterGetBufferSizeForConversion(IntPtr converterhandle, int PixelType, int SizeX, int SizeY, out uint size);

        public static ulong ImageFormatConverterGetBufferSizeForConversion(ConverterHandle hConverter, int PixelType, int SizeX, int SizeY)
        {
            ConverterHandle hConverter2 = hConverter;
            uint bufferLength = 0u;
            return Execute(() => PylonImageFormatConverterGetBufferSizeForConversion(hConverter2.Handle, PixelType, SizeX, SizeY, out bufferLength), () => bufferLength);
        }

        [DllImport("libpylonc")]
        private static extern uint PylonImageFormatConverterConvert(IntPtr hConv, IntPtr TargetBuffer, ulong bufferLength, IntPtr buffer, ulong PayloadSize, int PixelType, int SizeX, int SizeY, int PaddingX, int Orientation);

        public static void ImageFormatConverterConvert(ConverterHandle hConverter, IntPtr TargetBuffer, ulong bufferLength, IntPtr Buffer, ulong PayloadSize, int PixelType, int SizeX, int SizeY, int PaddingX, int Orientation)
        {
            ConverterHandle hConverter2 = hConverter;
            Execute(() => PylonImageFormatConverterConvert(hConverter2.Handle, TargetBuffer, bufferLength, Buffer, PayloadSize, PixelType, SizeX, SizeY, PaddingX, Orientation));
        }
    }

}
