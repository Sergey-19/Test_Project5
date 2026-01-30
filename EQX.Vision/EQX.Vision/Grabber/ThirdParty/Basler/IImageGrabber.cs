using Basler.UserGrabResult;

namespace Basler.pyloncore
{
    internal interface IImageGrabber
    {
        event EventHandler<GrabResultEx> OnGrabImage;

        bool InitializeCamera(string IPAddress);

        bool InitializeCamera();

        GrabResultEx GrabSingleImage();

        bool DeviceStartAcquisition(uint BufferCount = 20u);

        bool DeviceStopAcquisition();

        bool GrabContinues();

        void StopContinuesGrab();

        bool SetIntParameter(string FeatureName, int value);

        int GetIntParameter(string FeatureName);

        bool SetFloatParameter(string FeatureName, float value);

        double GetFloatParameter(string FeatureName);

        bool SetBoolParameter(string FeatureName, bool value);

        bool GetBoolParameter(string FeatureName);

        bool SetEnumParameter(string FeatureName, string value);

        string GetEnumParameter(string FeatureName);

        bool SetCommandParameter(string FeatureName);
    }

}
