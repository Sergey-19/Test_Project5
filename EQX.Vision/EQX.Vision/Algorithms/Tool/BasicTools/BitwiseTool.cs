using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms.Model.ToolResult;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class BitwiseTool : VisionToolBase
    {
        public BitwiseTool()
            : base()
        {
            Parameters = new ObjectCollection();
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
        }

        internal override bool ValidInputs()
        {
            if (Inputs["ImageMat"] == null) throw new ArgumentNullException("ImageMat");
            if (Inputs["ImageMat"]!.GetType() != typeof(Mat)) throw new ArgumentException("ImageMat");

            ((Mat)Outputs["ImageMat"])?.Dispose();

            return true;
        }

        internal override void DIPFunction()
        {
            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Mat outMat = new Mat();

                Cv2.BitwiseNot(inMat, outMat);
                Outputs["ImageMat"] = outMat;
                Inputs["ImageMat"] = inMat.Clone();
            }
        }
    }
}
