using EQX.Core.Vision.Algorithms;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class HistogramTool : VisionToolBase
    {
        public HistogramTool() 
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

                Cv2.EqualizeHist(inMat, outMat);
                Outputs["ImageMat"] = outMat;
                Inputs["ImageMat"] = inMat.Clone();
            }
        }

    }
}
