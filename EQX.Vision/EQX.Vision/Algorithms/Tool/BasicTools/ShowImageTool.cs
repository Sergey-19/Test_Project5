using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms.Model.ToolResult;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class ShowImageTool : VisionToolBase
    {
        public ShowImageTool() 
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

            return true;
        }

        internal override void DIPFunction()
        {
            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Thread.Sleep(1);

                Console.WriteLine($"Output image Width = {inMat.Width}; Height = {inMat.Height}");
                //Cv2.ImShow($"OutputImage {Id}", inMat);
                //Cv2.ResizeWindow($"OutputImage {Id}", new Size(1000, 800));
                //Cv2.WaitKey();
            }
        }

    }
}
