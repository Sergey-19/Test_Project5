using EQX.Core.Vision.Algorithms;
using Newtonsoft.Json;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public enum EDetectDirection : int
    {
        Vertical,
        Horizontal,
    }
    public class LineDetectionTool : VisionToolBase
    {
        public LineDetectionTool()
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("ThetaOffset", typeof(double)),
                new KeyType("Direction", typeof(EDetectDirection)),
                new KeyType("ROI", typeof(CRectangle)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("Line", typeof(LineDetectionResult)),
            });

            Parameters["ThetaOffset"] = 0.0;
            Parameters["Direction"] = EDetectDirection.Vertical;
            Parameters["ROI"] = new CRectangle();
        }

        [JsonConstructor]
        public LineDetectionTool(ObjectCollection parameters)
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("ThetaOffset", typeof(double)),
                new KeyType("Direction", typeof(EDetectDirection)),
                new KeyType("ROI", typeof(CRectangle)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("Line", typeof(LineDetectionResult)),
            });

            Parameters["ThetaOffset"] = parameters["ThetaOffset"];
            Parameters["Direction"] = Enum.Parse<EDetectDirection>(parameters["Direction"].ToString());
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
            Outputs["Line"] = new LineDetectionResult();

            double thetaOffset = Convert.ToDouble(Parameters["ThetaOffset"]);
            EDetectDirection direction = (EDetectDirection)(Parameters["Direction"]);
            CRectangle ROI = (CRectangle)Parameters["ROI"];

            Rect roi = new Rect(ROI.X, ROI.Y, ROI.Width, ROI.Height);

            using (Mat inMat = ((Mat)Inputs["ImageMat"]!).Clone())
            {
                Mat outMat = inMat.Clone();
                Cv2.CvtColor(outMat, outMat, ColorConversionCodes.GRAY2BGR);

                List<OpenCvSharp.Point> detectedPoints = new List<OpenCvSharp.Point>();

                roi.Width = roi.X + roi.Width < inMat.Width ? roi.Width : inMat.Width - roi.X;
                roi.Height = roi.Y + roi.Height < inMat.Height ? roi.Height : inMat.Height - roi.Y;

                using (Mat imgROI = inMat.Clone(roi))
                {
                    int[] colDetected = new int[imgROI.Width];

                    // 1. Find Points at Edge
                    switch (direction)
                    {
                        case EDetectDirection.Vertical:
                            for (int j = 1; j < imgROI.Height; j += 20)
                            {
                                for (int i = 1; i < imgROI.Width; i++)
                                {
                                    if (Math.Abs(imgROI.Get<byte>(j, i) - imgROI.Get<byte>(j, i - 1)) >= 200)
                                    {
                                        detectedPoints.Add(new OpenCvSharp.Point(i + roi.Left, j + roi.Top));
                                        break;
                                    }
                                }
                            }
                            break;
                        case EDetectDirection.Horizontal:
                            {
                                for (int i = 1; i < imgROI.Width; i += 20)
                                {
                                    for (int j = 1; j < imgROI.Height; j++)
                                    {
                                        if (Math.Abs(imgROI.Get<byte>(j, i) - imgROI.Get<byte>(j - 1, i)) >= 200)
                                        {
                                            detectedPoints.Add(new OpenCvSharp.Point(i + roi.Left, j + roi.Top));
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        default:

                            break;
                    }

                    if (detectedPoints.Count < 5)
                    {
                        ((LineDetectionResult)Outputs["Line"]).Judge = Core.Vision.EVisionJudge.NG;
                        Outputs["ImageMat"] = outMat;
                        return;
                    }

                    // FOR TEST ONLY
                    for (int i = 0; i < detectedPoints.Count; i++)
                    {
                        Cv2.Circle(outMat, detectedPoints[i], 3, Scalar.Red);
                        //Cv2.PutText(outMat, i.ToString(), detectedPoints[i] + new OpenCvSharp.Point(5, 5), HersheyFonts.HersheySimplex, 0.8, Scalar.Red);
                    }


                    //Filter by distance , angle
                    Tuple<int, double, double>[] detectedPointDistanceAngles = new Tuple<int, double, double>[detectedPoints.Count - 1];

                    for (int i = 1; i < detectedPoints.Count; i++)
                    {
                        detectedPointDistanceAngles[i - 1] = new Tuple<int, double, double>
                            (
                                i - 1,
                                OpenCvSharp.Point.Distance(detectedPoints[i - 1], detectedPoints[i]),
                                AlgorithmHelper.FindAngle(detectedPoints[i - 1], detectedPoints[i])
                            );
                    }

                    detectedPointDistanceAngles = detectedPointDistanceAngles
                        .OrderBy(da => da.Item3)
                        .ThenBy(da => da.Item2)
                        .ToArray();

                    var filteredArray = AlgorithmHelper.FindLongestSubarray(detectedPointDistanceAngles);

                    if(filteredArray.Item2 - filteredArray.Item1 < 2)
                    {
                        ((LineDetectionResult)Outputs["Line"]).Judge = Core.Vision.EVisionJudge.NG;
                        Outputs["ImageMat"] = outMat;
                        return;
                    }

                    List<OpenCvSharp.Point> filteredByDistanceAnglePoints = new List<OpenCvSharp.Point>();
                    for (int i = filteredArray.Item1; i < filteredArray.Item2; i++)
                    {
                        int index = detectedPointDistanceAngles[i].Item1;
                        if (filteredByDistanceAnglePoints.Contains(detectedPoints[index]) == false)
                            filteredByDistanceAnglePoints.Add(detectedPoints[index]);
                        if (filteredByDistanceAnglePoints.Contains(detectedPoints[index + 1]) == false)
                            filteredByDistanceAnglePoints.Add(detectedPoints[index + 1]);
                    }

                    if (filteredByDistanceAnglePoints.Count < 2)
                    {
                        ((LineDetectionResult)Outputs["Line"]).Judge = Core.Vision.EVisionJudge.NG;
                        Outputs["ImageMat"] = outMat;
                        return;
                    }

                    //Filter by Y Intercept
                    List<OpenCvSharp.Point> filteredPoints = new List<OpenCvSharp.Point>();
                    Tuple<int, double>[] detectedPointYIntercept = new Tuple<int, double>[filteredByDistanceAnglePoints.Count - 1];

                    for (int i = 1; i < filteredByDistanceAnglePoints.Count; i++)
                    {
                        detectedPointYIntercept[i - 1] = new Tuple<int, double>
                            (
                                i - 1,
                                AlgorithmHelper.FindYIntercept(filteredByDistanceAnglePoints[i - 1], filteredByDistanceAnglePoints[i])
                            );
                    }
                    detectedPointYIntercept = detectedPointYIntercept.OrderBy(x => x.Item2).ToArray();
                    var filteredYInterceptArray = AlgorithmHelper.FindLongestSubarray(detectedPointYIntercept,direction == EDetectDirection.Horizontal? 5 : 600);

                    if (filteredYInterceptArray.Item2 - filteredYInterceptArray.Item1 < 2)
                    {
                        ((LineDetectionResult)Outputs["Line"]).Judge = Core.Vision.EVisionJudge.NG;
                        Outputs["ImageMat"] = outMat;
                        return;
                    }

                    for (int i = filteredYInterceptArray.Item1; i < filteredYInterceptArray.Item2; i++)
                    {
                        int index = detectedPointYIntercept[i].Item1;
                        if (filteredPoints.Contains(filteredByDistanceAnglePoints[index]) == false)
                            filteredPoints.Add(filteredByDistanceAnglePoints[index]);
                        if (filteredPoints.Contains(filteredByDistanceAnglePoints[index + 1]) == false)
                            filteredPoints.Add(filteredByDistanceAnglePoints[index + 1]);
                    }
                    // Fitline
                    Line2D detectedLine = Cv2.FitLine(filteredPoints, DistanceTypes.L2, 0, 0.01, 0.01);
                    ((LineDetectionResult)Outputs["Line"]).Line2D = detectedLine;

                    double detectedAngle = 0.0;

                    if (direction == EDetectDirection.Vertical)
                    {
                        if (detectedLine.GetVectorAngle() <= 0)
                        {
                            detectedAngle = detectedLine.GetVectorAngle() + 90 + thetaOffset;
                        }
                        else
                        {
                            detectedAngle = detectedLine.GetVectorAngle() - 90 + thetaOffset;
                        }
                    }
                    else
                    {
                        if (detectedLine.GetVectorAngle() <= 0)
                        {
                            detectedAngle = detectedLine.GetVectorAngle() - thetaOffset;
                        }
                        else
                        {
                            detectedAngle = detectedLine.GetVectorAngle() + thetaOffset;
                        }
                    }

                    OpenCvSharp.Point pt1, pt2;

                    detectedLine.FitSize(roi.Height, roi.Width, out pt1, out pt2);

                    Cv2.Line(outMat, pt1, pt2, Scalar.Blue, 3);

                    Outputs["ImageMat"] = outMat;
                    ((LineDetectionResult)Outputs["Line"]).Angle = detectedAngle;
                    ((LineDetectionResult)Outputs["Line"]).LineSegmentPoint = new LineSegmentPoint(pt1, pt2);
                    ((LineDetectionResult)Outputs["Line"]).Judge = Core.Vision.EVisionJudge.OK;

                    for (int i = 0; i < filteredPoints.Count; i++)
                    {
                        Cv2.DrawMarker(outMat, filteredPoints[i], Scalar.Yellow, MarkerTypes.Cross, 20, 3);
                        //Cv2.PutText(outMat, detectedPointDistanceAngles[filteredArray.Item1 + i].Item1.ToString(), filteredPoints[i] + new OpenCvSharp.Point(5, 5), HersheyFonts.HersheySimplex, 1.2, Scalar.Yellow);
                    }
                }
            }
        }

    }
}
