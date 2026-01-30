using EQX.Core.Vision.Algorithms;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class CreateCircleTool : VisionToolBase
    {
        public CreateCircleTool()
            : base()
        {
            Parameters = new ObjectCollection();
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("Center", typeof(Point2f)),
                new KeyType("Radius", typeof(double)),
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
            Point2f center = (Point2f)Inputs["Center"];
            double radius = Convert.ToDouble(Inputs["Radius"]);

            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Mat outMat = new Mat();
                outMat = inMat.Clone();
                Cv2.CvtColor(outMat, outMat, ColorConversionCodes.GRAY2BGR);
                Cv2.Circle(outMat, (int)center.X, (int)center.Y, (int)radius, Scalar.Red, 2);
                Outputs["ImageMat"] = outMat;
                Inputs["ImageMat"] = inMat.Clone();
            }
        }

    }
}
