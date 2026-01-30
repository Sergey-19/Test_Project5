using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms.Model.ToolResult;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class ErodeTool : VisionToolBase
    {
        public ErodeTool() 
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("KernalSizeWidth", typeof(int)),
                new KeyType("KernalSizeHeight", typeof(int)),
                new KeyType("MorphShape", typeof(MorphShapes)),
                new KeyType("Iterations", typeof(int)),
            });
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
            int kernalSizeWidth = (int)Parameters["KernalSizeWidth"];
            int kernalSizeHeight = (int)Parameters["KernalSizeHeight"];
            MorphShapes morphShape = (MorphShapes)Parameters["MorphShape"];
            int interations = (int)Parameters["Iterations"];

            using (Mat Kernal = Cv2.GetStructuringElement(morphShape, new Size(kernalSizeWidth, kernalSizeHeight)))
            {
                using (Mat inMat = (Mat)Inputs["ImageMat"]!)
                {
                    Mat outMat = new Mat();

                    Cv2.Erode(inMat, outMat, Kernal, new OpenCvSharp.Point(-1, -1), interations);
                    Outputs["ImageMat"] = outMat;
                    Inputs["ImageMat"] = inMat.Clone();
                }
            }
        }

    }
}
