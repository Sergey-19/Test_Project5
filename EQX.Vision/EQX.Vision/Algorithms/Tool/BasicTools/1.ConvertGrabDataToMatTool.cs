using EQX.Core.Vision.Algorithms;
using EQX.Core.Vision.Grabber;
using EQX.Vision.Grabber.Helpers;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class ConvertGrabDataToMatTool : VisionToolBase
    {
        public ConvertGrabDataToMatTool()
            : base()
        {
            Parameters = new ObjectCollection();
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("GrabData", typeof(GrabData)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
        }

        internal override bool ValidInputs()
        {
            if (Inputs["GrabData"] == null) throw new ArgumentNullException("GrabData");
            if (Inputs["GrabData"]!.GetType() != typeof(GrabData)) throw new ArgumentException("GrabData");

            ((Mat)Outputs["ImageMat"])?.Dispose();

            return true;
        }

        internal override void DIPFunction()
        {
            using (GrabData grabData = (GrabData)Inputs["GrabData"]!)
            {
                Mat outMat = grabData.ToMat();
                Outputs["ImageMat"] = outMat;
            }
        }
    }
}
