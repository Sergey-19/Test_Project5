using EQX.Core.Vision.Grabber;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public static class ObjectCollectionKeyTypes
    {
        public static Dictionary<string, Type> KeyTypes = new Dictionary<string, Type>
        {
            #region Parameters
            { "ROI", typeof(CRectangle) },
            #endregion

            { "GrabData", typeof(GrabData) },
            { "Raw Image", typeof(GrabData) },
            { "Process Image", typeof(Mat) },
            { "ImageMat", typeof(Mat) },
            { "Center", typeof(OpenCvSharp.Point) },
            { "Radius", typeof(double) },
            { "LocationX", typeof(double) },
            { "LocationY", typeof(double) },
            { "Line", typeof(LineDetectionResult) },
            { "Line1", typeof(LineDetectionResult) },
            { "Line2", typeof(LineDetectionResult) },
            { "X", typeof(double) },
            { "Y", typeof(double) },
            { "Theta", typeof(double) },
        };
    }
}
