using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms.Model.ToolResult;
using Newtonsoft.Json;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HoughCircleTool : VisionToolBase
    {
        [JsonProperty]
        public CRectangle ROI { get; set; }
        public HoughCircleTool()
            : base()
        {
            ROI = new CRectangle();

            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("Dp", typeof(double)),
                new KeyType("Threshold", typeof(double)),
                new KeyType("MinDistance", typeof(double)),
                new KeyType("MinRadius", typeof(double)),
                new KeyType("MaxRadius", typeof(double)),
                new KeyType("Accumulator", typeof(double)),
                new KeyType("ROI", typeof(CRectangle)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("Center", typeof(Point2f)),
                new KeyType("Circles", typeof(List<CircleDetectionResult>)),
            });

            Parameters["Dp"] = 1.0;
            Parameters["Threshold"] = 200.0;
            Parameters["MinDistance"] = 100.0;
            Parameters["MinRadius"] = 200.0;
            Parameters["MaxRadius"] = 300.0;
            Parameters["Accumulator"] = 51.0;
            Parameters["ROI"] = new CRectangle();
        }

        [JsonConstructor]
        public HoughCircleTool(ObjectCollection parameters)
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("Dp", typeof(double)),
                new KeyType("Threshold", typeof(double)),
                new KeyType("MinDistance", typeof(double)),
                new KeyType("MinRadius", typeof(double)),
                new KeyType("MaxRadius", typeof(double)),
                new KeyType("Accumulator", typeof(double)),
                new KeyType("ROI", typeof(CRectangle)),
            });

            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("Center", typeof(Point2f)),
                new KeyType("Circles", typeof(List<CircleDetectionResult>)),
            });

            Parameters["Dp"] = parameters["Dp"];
            Parameters["Threshold"] = parameters["Threshold"];
            Parameters["MinDistance"] = parameters["MinDistance"];
            Parameters["MinRadius"] = parameters["MinRadius"];
            Parameters["MaxRadius"] = parameters["MaxRadius"];
            Parameters["Accumulator"] = parameters["Accumulator"];
            Parameters["ROI"] = parameters["ROI"];

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
            Outputs["Circles"] = new List<CircleDetectionResult>();

            double dp = Convert.ToDouble(Parameters["Dp"]);
            double threshold = Convert.ToDouble(Parameters["Threshold"]);
            double minDist = Convert.ToDouble(Parameters["MinDistance"]);
            int minRadius = Convert.ToInt32(Parameters["MinRadius"]);
            int maxRadius = Convert.ToInt32(Parameters["MaxRadius"]);
            double accumulator = Convert.ToDouble(Parameters["Accumulator"]);

            CRectangle ROI = (CRectangle)Parameters["ROI"];

            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Rect roi = new Rect(new OpenCvSharp.Point(ROI.X, ROI.Y), new Size(ROI.Width, ROI.Height));
                using (Mat imgROI = inMat.SubMat(roi))
                {
                    CircleSegment[] circles;
                    Mat outMat = new Mat();
                    outMat = inMat.Clone();

                    circles = Cv2.HoughCircles(imgROI, HoughModes.Gradient, dp, minDist, threshold, accumulator, minRadius, maxRadius);

                    Cv2.CvtColor(outMat, outMat, ColorConversionCodes.GRAY2BGR);
                    foreach (var circle in circles)
                    {
                        Cv2.Circle(outMat, (int)(circle.Center.X + ROI.X), (int)(circle.Center.Y + ROI.Y), (int)circle.Radius, Scalar.Red, 3);
                        // Draw center
                        Cv2.Circle(outMat, (int)circle.Center.X, (int)circle.Center.Y, 1, Scalar.Blue, -1);

                        ((List<CircleDetectionResult>)Outputs["Circles"]).Add(new CircleDetectionResult()
                        {
                            Circle = new CCircle(circle.Center.X + ROI.X, circle.Center.Y + ROI.Y, circle.Radius),
                            Judge = Core.Vision.EVisionJudge.OK
                        });

                    }
                    Outputs["ImageMat"] = outMat;
                    Inputs["ImageMat"] = inMat.Clone();
                }
            }
        }
    }
}
