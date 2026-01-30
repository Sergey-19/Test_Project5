using EQX.Core.Vision.Algorithms;
using Newtonsoft.Json;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class ThresholdTool : VisionToolBase
    {
        public ThresholdTool()
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("Threshold", typeof(double)),
                new KeyType("ThresholdType", typeof(ThresholdTypes)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            Parameters["ThresholdType"] = ThresholdTypes.Binary;
            Parameters["Threshold"] = 100;
        }
        [JsonConstructor]
        public ThresholdTool(ObjectCollection parameters)
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("Threshold", typeof(double)),
                new KeyType("ThresholdType", typeof(ThresholdTypes)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            Parameters["ThresholdType"] = Enum.Parse<ThresholdTypes>(parameters["ThresholdType"].ToString());
            Parameters["Threshold"] = parameters["Threshold"];
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
            double threshold = Convert.ToDouble(Parameters["Threshold"]);
            ThresholdTypes thresholdType = (ThresholdTypes)Parameters["ThresholdType"];

            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Mat outMat = new Mat();

                Cv2.Threshold(inMat, outMat, threshold, 255, thresholdType);
                Outputs["ImageMat"] = outMat;
                Inputs["ImageMat"] = inMat.Clone();
            }
        }
    }
}
