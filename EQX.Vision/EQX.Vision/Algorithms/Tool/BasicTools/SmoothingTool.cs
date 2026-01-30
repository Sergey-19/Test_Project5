using EQX.Core.Vision.Algorithms;
using OpenCvSharp;
using System.Text.Json.Serialization;

namespace EQX.Vision.Algorithms
{
    public class SmoothingTool : VisionToolBase
    {
        public SmoothingTool() 
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("Width", typeof(double)),
                new KeyType("Height", typeof(double)),
                new KeyType("SigmaX", typeof(double)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            Parameters["Width"] = 3.0;
            Parameters["Height"] = 3.0;
            Parameters["SigmaX"] = 10.0;
        }

        [Newtonsoft.Json.JsonConstructor]
        public SmoothingTool(ObjectCollection parameters)
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("Width", typeof(double)),
                new KeyType("Height", typeof(double)),
                new KeyType("SigmaX", typeof(double)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            Parameters["Width"] = parameters["Width"];
            Parameters["Height"] = parameters["Height"];
            Parameters["SigmaX"] = parameters["SigmaX"];

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
            double width = Convert.ToDouble(Parameters["Width"]);
            double height = Convert.ToDouble(Parameters["Height"]);
            double sigmaX = Convert.ToDouble(Parameters["SigmaX"]);

            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Mat outMat = new Mat();

                Cv2.GaussianBlur(inMat, outMat, new Size(width,height), sigmaX);
                Outputs["ImageMat"] = outMat;
                Inputs["ImageMat"] = inMat.Clone();
            }
        }
    }
}
