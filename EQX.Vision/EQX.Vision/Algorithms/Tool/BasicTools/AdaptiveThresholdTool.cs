using EQX.Core.Vision.Algorithms;
using Newtonsoft.Json;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class AdaptiveThresholdTool : VisionToolBase
    {
        public AdaptiveThresholdTool()
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("MaxValue", typeof(double)),
                new KeyType("AdaptiveThresholdType", typeof(AdaptiveThresholdTypes)),
                new KeyType("ThresholdType", typeof(ThresholdTypes)),
                new KeyType("BlockSize", typeof(int)),
                new KeyType("Constant", typeof(double)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            Parameters["AdaptiveThresholdType"] = AdaptiveThresholdTypes.GaussianC;
            Parameters["ThresholdType"] = ThresholdTypes.Binary;
            Parameters["MaxValue"] = 200;
            Parameters["BlockSize"] = 3;
            Parameters["Constant"] = 1.0;
        }

        [JsonConstructor]
        public AdaptiveThresholdTool(ObjectCollection parameters) 
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("MaxValue", typeof(double)),
                new KeyType("AdaptiveThresholdType", typeof(AdaptiveThresholdTypes)),
                new KeyType("ThresholdType", typeof(ThresholdTypes)),
                new KeyType("BlockSize", typeof(int)),
                new KeyType("Constant", typeof(double)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            Parameters["AdaptiveThresholdType"] = Enum.Parse<AdaptiveThresholdTypes>(parameters["AdaptiveThresholdType"].ToString());
            Parameters["ThresholdType"] = Enum.Parse<ThresholdTypes>(parameters["ThresholdType"].ToString());
            Parameters["MaxValue"] = parameters["MaxValue"];
            Parameters["BlockSize"] = parameters["BlockSize"];
            Parameters["Constant"] = parameters["Constant"];
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
            double maxValue = Convert.ToDouble(Parameters["MaxValue"]);
            AdaptiveThresholdTypes adaptiveThresholdType = (AdaptiveThresholdTypes)Parameters["AdaptiveThresholdType"];
            ThresholdTypes thresholdType = (ThresholdTypes)Parameters["ThresholdType"];
            int blockSize = Convert.ToInt32(Parameters["BlockSize"]);
            double constant = Convert.ToDouble(Parameters["Constant"]);

            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Mat outMat = new Mat();

                Cv2.AdaptiveThreshold(inMat, outMat, maxValue, adaptiveThresholdType, thresholdType, blockSize, constant);
                Outputs["ImageMat"] = outMat;
                Inputs["ImageMat"] = inMat.Clone();
            }
        }

    }
}
