using EQX.Core.Vision.Algorithms;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class InRangeTool : VisionToolBase
    {
        public InRangeTool()
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("Lower", typeof(double)),
                new KeyType("Upper", typeof(double)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            Parameters["Lower"] = 20;
            Parameters["Upper"] = 200;
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
            double threshold1 = Convert.ToDouble(Parameters["Lower"]);
            double threshold2 = Convert.ToDouble(Parameters["Upper"]);

            Scalar lower = new Scalar(threshold1);
            Scalar upper = new Scalar(threshold2);

            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Mat outMat = new Mat();

                Cv2.InRange(inMat, lower, upper,outMat);
                Outputs["ImageMat"] = outMat;
                Inputs["ImageMat"] = inMat.Clone();
            }
        }

    }
}
