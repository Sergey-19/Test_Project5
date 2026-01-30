using EQX.Core.Vision;
using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms.Model;
using EQX.Vision.Algorithms.Model.ToolResult;
using Newtonsoft.Json;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class AlignTwoLinesTool : VisionToolBase
    {
        public AlignTwoLinesTool()
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType>()
            {
                new KeyType("MinOffsetX",typeof(double)),
                new KeyType("MaxOffsetX",typeof(double)),
                new KeyType("MinOffsetY",typeof(double)),
                new KeyType("MaxOffsetY",typeof(double)),
                new KeyType("MinTheta",typeof(double)),
                new KeyType("MaxTheta",typeof(double)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("Line1", typeof(LineDetectionResult)),
                new KeyType("Line2", typeof(LineDetectionResult)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("AlignResult", typeof(AlignResult)),
            });
            
            Parameters["MinOffsetX"] = -1.0;
            Parameters["MaxOffsetX"] = 1.0;
            Parameters["MinOffsetY"] = -1.0;
            Parameters["MaxOffsetY"] = 1.0;
            Parameters["MinTheta"] = -1.0;
            Parameters["MaxTheta"] = 1.0;
        }

        [JsonConstructor]
        public AlignTwoLinesTool(ObjectCollection parameters)
        {
            Parameters = new ObjectCollection(new List<KeyType>()
            {
                new KeyType("MinOffsetX",typeof(double)),
                new KeyType("MaxOffsetX",typeof(double)),
                new KeyType("MinOffsetY",typeof(double)),
                new KeyType("MaxOffsetY",typeof(double)),
                new KeyType("MinTheta",typeof(double)),
                new KeyType("MaxTheta",typeof(double)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("Line1", typeof(LineDetectionResult)),
                new KeyType("Line2", typeof(LineDetectionResult)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("AlignResult", typeof(AlignResult)),
            });
            Parameters["MinOffsetX"] = parameters["MinOffsetX"];
            Parameters["MaxOffsetX"] = parameters["MaxOffsetX"];
            Parameters["MinOffsetY"] = parameters["MinOffsetY"];
            Parameters["MaxOffsetY"] = parameters["MaxOffsetY"];
            Parameters["MinTheta"] = parameters["MinTheta"];
            Parameters["MaxTheta"] = parameters["MaxTheta"];
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
            Outputs["AlignResult"] = new AlignResult();
            double minOffsetX = Convert.ToDouble(Parameters["MinOffsetX"]);
            double maxOffsetX = Convert.ToDouble(Parameters["MaxOffsetX"]);
            double minOffsetY = Convert.ToDouble(Parameters["MinOffsetY"]);
            double maxOffsetY = Convert.ToDouble(Parameters["MaxOffsetY"]);
            double minTheta = Convert.ToDouble(Parameters["MinTheta"]);
            double maxTheta = Convert.ToDouble(Parameters["MaxTheta"]);

            using (Mat inMat = ((Mat)Inputs["ImageMat"]!).Clone())
            {
                Mat outMat = inMat.Clone();
                Point2f IntersectionPoint = new Point2f(0, 0);

                Cv2.CvtColor(outMat, outMat, ColorConversionCodes.GRAY2BGR);

                LineDetectionResult line1 = (LineDetectionResult)Inputs["Line1"];
                LineDetectionResult line2 = (LineDetectionResult)Inputs["Line2"];

                if (line1.Judge == Core.Vision.EVisionJudge.NG || line2.Judge == Core.Vision.EVisionJudge.NG)
                {
                    ((AlignResult)Outputs["AlignResult"]).Judge = Core.Vision.EVisionJudge.NG;
                    Outputs["ImageMat"] = outMat;
                    return;
                }

                if (AlgorithmHelper.FindInterSectionPoint(line1.LineSegmentPoint, line2.LineSegmentPoint, ref IntersectionPoint) == false)
                {
                    ((AlignResult)Outputs["AlignResult"]).Judge = Core.Vision.EVisionJudge.NG;
                    Outputs["ImageMat"] = outMat;
                    return;
                }

                MatHelper.DrawExtendedLine(outMat, line1.LineSegmentPoint.P1, line1.LineSegmentPoint.P2, Scalar.Green, 3);
                MatHelper.DrawExtendedLine(outMat, line2.LineSegmentPoint.P1, line2.LineSegmentPoint.P2, Scalar.Green, 3);

                Cv2.Circle(outMat, (int)IntersectionPoint.X, (int)IntersectionPoint.Y, 10, Scalar.Green, 10);

                XYTOffset offset = new Model.XYTOffset
                {
                    X = IntersectionPoint.X - inMat.Width / 2,
                    Y = IntersectionPoint.Y - inMat.Height / 2,
                    Theta = (line1.Angle + line2.Angle) / 2,
                };

                EVisionJudge visionJudge = EVisionJudge.OK;
                if (offset.X * PixelSize <= minOffsetX || offset.X * PixelSize >= maxOffsetX)
                {
                    visionJudge = EVisionJudge.NG;
                }
                if (offset.Y * PixelSize <= minOffsetY || offset.Y * PixelSize >= maxOffsetY)
                {
                    visionJudge = EVisionJudge.NG;
                }
                if (offset.Theta <= minTheta || offset.Theta >= maxTheta)
                {
                    visionJudge = EVisionJudge.NG;
                }

                Outputs["ImageMat"] = outMat;
                Outputs["AlignResult"] = new AlignResult()
                {
                    Offset = new Model.XYTOffset
                    {
                        X = IntersectionPoint.X - inMat.Width / 2,
                        Y = IntersectionPoint.Y - inMat.Height / 2,
                        Theta = (line1.Angle + line2.Angle) / 2,
                    },
                    Judge = visionJudge,
                    DrawAction = (image) =>
                    {
                        MatHelper.DrawExtendedLine(image, line1.LineSegmentPoint.P1, line1.LineSegmentPoint.P2, Scalar.Green, 3);
                        MatHelper.DrawExtendedLine(image, line2.LineSegmentPoint.P1, line2.LineSegmentPoint.P2, Scalar.Green, 3);
                        Cv2.Circle(image, (int)IntersectionPoint.X, (int)IntersectionPoint.Y, 10, Scalar.Green, 10);
                    }
                };

                Inputs["ImageMat"] = inMat.Clone();
            }
        }
    }
}
