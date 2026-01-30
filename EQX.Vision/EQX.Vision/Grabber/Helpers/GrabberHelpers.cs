using EQX.Core.Vision.Grabber;
using OpenCvSharp;
using System.Runtime.InteropServices;

namespace EQX.Vision.Grabber.Helpers
{
    public static class GrabberHelpers
    {
        public static Mat ToMat(this GrabData grabData)
        {
            // TODO: Handle another type of Grab DataType
            if (grabData == null) throw new ArgumentNullException(nameof(grabData));
            if (grabData.ImageBuffer == null) throw new ArgumentNullException(nameof(grabData.ImageBuffer));

            var mat = new Mat(grabData.Height, grabData.Width, MatType.CV_8UC1);
            Marshal.Copy(grabData.ImageBuffer, 0, mat.Data, grabData.Height * grabData.Width);
            
            return mat;
        }
    }
}
