using EQX.Vision.Algorithms.Model.ToolResult;
using EQX.Core.Vision.Algorithms;
using Newtonsoft.Json;
using OpenCvSharp;
using Avalonia;
using System.Linq;
using System.Windows.Media.Media3D;
using System;

namespace EQX.Vision.Algorithms
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FindCircleTool : VisionToolBase
    {
        [JsonProperty]
        public CCircle ExpectedCircle { get; set; }
        public FindCircleTool()
            : base()
        {
            ExpectedCircle = new CCircle();
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("PointOffset", typeof(int)),
                new KeyType("Threshold", typeof(int)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("Circle", typeof(CircleDetectionResult)),
            });

            Parameters["PointOffset"] = 20.0;
            Parameters["Threshold"] = 50.0;
        }

        [JsonConstructor]
        public FindCircleTool(ObjectCollection parameters, CCircle expectedCircle)
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("PointOffset", typeof(int)),
                new KeyType("Threshold", typeof(int)),
            });

            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
                new KeyType("Circle", typeof(CircleDetectionResult)),
            });

            Parameters["PointOffset"] = parameters["PointOffset"];
            Parameters["Threshold"] = parameters["Threshold"];
            ExpectedCircle = expectedCircle;
        }
        internal override void DIPFunction()
        {
            int pointOffset = Convert.ToInt32(Parameters["PointOffset"]);
            int threshold = Convert.ToInt32(Parameters["Threshold"]);

            Outputs["Circle"] = new CircleDetectionResult();

            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Mat outMat = inMat.Clone();
                Cv2.CvtColor(outMat, outMat, ColorConversionCodes.GRAY2BGR);
                //                #region OLD
                //                List<Point2d> circlePoints = new List<Point2d>();
                //                double angleStep = (angleEnd - angleStart) / numberOfCapliers;
                //                for (int j = 0; j < numberOfCapliers; j++)
                //                {
                //                    double angle = (j * angleStep + angleStart + angleOffset) * Math.PI / 180;

                //                    int x = (int)(ExpectedCircle.CenterX + XOffset + ExpectedCircle.Radius * Math.Cos(angle));
                //                    int y = (int)(ExpectedCircle.CenterY + YOffset + ExpectedCircle.Radius * Math.Sin(angle));

                //                    OpenCvSharp.LineSegmentPoint line = new LineSegmentPoint(new OpenCvSharp.Point(x + searchLength / 2 * Math.Cos(angle), y + searchLength / 2 * Math.Sin(angle)),
                //                        new OpenCvSharp.Point(x - searchLength / 2 * Math.Cos(angle), y - searchLength / 2 * Math.Sin(angle)));

                //                    line.P1.X = line.P1.X < 0 ? 0 : line.P1.X;
                //                    line.P1.X = line.P1.X > inMat.Width ? inMat.Width : line.P1.X;
                //                    line.P1.Y = line.P1.Y < 0 ? 0 : line.P1.Y;
                //                    line.P1.Y = line.P1.Y > inMat.Height ? inMat.Height : line.P1.Y;

                //                    line.P2.X = line.P2.X < 0 ? 0 : line.P2.X;
                //                    line.P2.X = line.P2.X > inMat.Width ? inMat.Width : line.P2.X;
                //                    line.P2.Y = line.P2.Y < 0 ? 0 : line.P2.Y;
                //                    line.P2.Y = line.P2.Y > inMat.Height ? inMat.Height : line.P2.Y;

                //                    //Draw Line To Find MaxDiffPoint
                //                    Cv2.Line(outMat, line.P1, line.P2, Scalar.Blue, 5);

                //                    OpenCvSharp.Point[] pointInLine = GetPointsInLine(line, inMat).ToArray();
                //                    if (pointInLine.Length < 5)
                //                    {
                //                        Inputs["ImageMat"] = inMat.Clone();

                //                        ((CircleDetectionResult)Outputs["Circle"]).Judge = Core.Vision.EVisionJudge.NG;
                //                        ((CircleDetectionResult)Outputs["Circle"]).Circle = new CCircle();
                //                        return;
                //                    }
                //                    OpenCvSharp.Point pointMaxDiff = pointInLine[0];

                //                    int maxDiff = Math.Abs((inMat.At<byte>(pointInLine[0].Y, pointInLine[0].X)) - (inMat.At<byte>(pointInLine[1].Y, pointInLine[1].X)));
                //                    for (int k = 0; k < pointInLine.Count() - 1; k++)
                //                    {
                //                        if (Math.Abs((inMat.At<byte>(pointInLine[k].Y, pointInLine[k].X)) - (inMat.At<byte>(pointInLine[k + 1].Y, pointInLine[k + 1].X))) > maxDiff)
                //                        {
                //                            maxDiff = Math.Abs((inMat.At<byte>(pointInLine[k].Y, pointInLine[k].X)) - (inMat.At<byte>(pointInLine[k + 1].Y, pointInLine[k + 1].X)));
                //                            pointMaxDiff = pointInLine[k];
                //                        }
                //                    }
                //                    circlePoints.Add(pointMaxDiff);

                //                }

                //                if (circlePoints.Count < 0)
                //                {
                //                    Inputs["ImageMat"] = inMat.Clone();

                //                    ((CircleDetectionResult)Outputs["Circle"]).Judge = Core.Vision.EVisionJudge.NG;
                //                    ((CircleDetectionResult)Outputs["Circle"]).Circle = new CCircle();
                //                    return;
                //                }
                //                //Test
                //                foreach (OpenCvSharp.Point point in circlePoints)
                //                {
                //                    Cv2.Line(outMat, new OpenCvSharp.Point(point.X - 5, point.Y), new OpenCvSharp.Point(point.X + 5, point.Y), Scalar.Red, 2);
                //                    Cv2.Line(outMat, new OpenCvSharp.Point(point.X, point.Y - 5), new OpenCvSharp.Point(point.X, point.Y + 5), Scalar.Red, 2);
                //                    Cv2.PutText(outMat, circlePoints.FindIndex(p => p.X == point.X && p.Y == point.Y).ToString(),
                //                                new OpenCvSharp.Point(point.X - 30, point.Y),
                //                                HersheyFonts.Italic,
                //                                0.3, Scalar.Red, thickness: 1);

                //                }

                //                Tuple<int, double>[] detectedPointDistance = new Tuple<int, double>[circlePoints.Count - 1];

                //                for (int i = 1; i < circlePoints.Count; i++)
                //                {
                //                    detectedPointDistance[i - 1] = new Tuple<int, double>
                //                        (
                //                            i - 1,
                //                            OpenCvSharp.Point.Distance((OpenCvSharp.Point)circlePoints[i - 1], (OpenCvSharp.Point)circlePoints[i])
                //                        );
                //                }

                //                detectedPointDistance = detectedPointDistance.OrderBy(x => x.Item2).ToArray();
                //                var filteredArray = AlgorithmHelper.FindLongestSubarray(detectedPointDistance, 2);

                //                List<OpenCvSharp.Point2d> circleFilteredPoints = new List<OpenCvSharp.Point2d>();
                //                for (int i = 0; i < filteredArray.Item2; i++)
                //                {
                //                    int index = detectedPointDistance[filteredArray.Item1 + i].Item1;

                //                    if (circleFilteredPoints.Contains(circlePoints[index]) == false)
                //                        circleFilteredPoints.Add(circlePoints[index]);

                //                    if (circleFilteredPoints.Contains(circlePoints[index + 1]) == false)
                //                        circleFilteredPoints.Add(circlePoints[index + 1]);
                //                }

                //                foreach (OpenCvSharp.Point point in circleFilteredPoints)
                //                {
                //                    Cv2.Line(outMat, new OpenCvSharp.Point(point.X - 5, point.Y), new OpenCvSharp.Point(point.X + 5, point.Y), Scalar.Green, 2);
                //                    Cv2.Line(outMat, new OpenCvSharp.Point(point.X, point.Y - 5), new OpenCvSharp.Point(point.X, point.Y + 5), Scalar.Green, 2);
                //                }

                //                Point2d center = new Point2d();
                //                double radius = 0;
                //                FitCircleLMS(circleFilteredPoints, out center, out radius);
                //                CCircle resultCircle = new CCircle(center.X, center.Y, radius);

                //                ((CircleDetectionResult)Outputs["Circle"]).Circle = resultCircle;

                //                if (resultCircle.Radius == 0)
                //                {
                //                    Inputs["ImageMat"] = inMat.Clone();

                //                    ((CircleDetectionResult)Outputs["Circle"]).Judge = Core.Vision.EVisionJudge.NG;
                //                    ((CircleDetectionResult)Outputs["Circle"]).Circle = resultCircle;
                //                    return;
                //                }

                //                            ((CircleDetectionResult)Outputs["Circle"]).DrawAction = (image) =>
                //                            {
                //                                foreach (OpenCvSharp.Point point in circleFilteredPoints)
                //                                {
                //                                    Cv2.DrawMarker(image, point, Scalar.Yellow, MarkerTypes.Cross, 15, 2);
                //                                }

                //                                Cv2.Circle(image, new OpenCvSharp.Point((int)resultCircle.CenterX, (int)resultCircle.CenterY), (int)resultCircle.Radius, Scalar.Blue, 2);
                //                                Cv2.Circle(image, new OpenCvSharp.Point(resultCircle.CenterX, resultCircle.CenterY), 10, Scalar.Yellow, 5);
                //                            };

                //                Cv2.Circle(outMat, new OpenCvSharp.Point((int)resultCircle.CenterX, (int)resultCircle.CenterY), (int)resultCircle.Radius, Scalar.Red, 2);
                //                ((CircleDetectionResult)Outputs["Circle"]).Judge = Core.Vision.EVisionJudge.OK;
                //                //Test
                //                //Cv2.Circle(outMat, new OpenCvSharp.Point((int)ExpectedCircle.CenterX, (int)ExpectedCircle.CenterY), (int)ExpectedCircle.Radius, Scalar.Red, 2);
                //#endregion

                #region TEST
                List<OpenCvSharp.Point2d> detectedPoints = new List<OpenCvSharp.Point2d>();
                int width = inMat.Width;
                int height = inMat.Height;

                double slope = (double)inMat.Height / (double)inMat.Width;

                for (int i = width ; i > 0; i = i - pointOffset)
                {
                    double YP2 = height - (i * slope);
                    OpenCvSharp.Point pt1 = new OpenCvSharp.Point(i, 0);
                    OpenCvSharp.Point pt2 = new OpenCvSharp.Point(width, YP2);

                    pt2.X = pt2.X < inMat.Width ? pt2.X : inMat.Width;

                    List<OpenCvSharp.Point> pointInLine = GetPointsInLine(new LineSegmentPoint(pt1, pt2), inMat);
                    if (pointInLine.Count < 5) continue;

                    for (int k = 0; k < pointInLine.Count() - 51; k++)
                    {
                        if (Math.Abs(inMat.Get<byte>(pointInLine[k].Y, pointInLine[k].X) - inMat.Get<byte>(pointInLine[k + 1].Y, pointInLine[k + 1].X)) >= 15)
                        {
                            List<OpenCvSharp.Point> points = pointInLine.ToList().GetRange(k, 50);

                            int value = inMat.Get<byte>(pointInLine[k].Y, pointInLine[k].X);

                            int count = 0;
                            for (int j = 0; j < points.Count(); j++)
                            {
                                if (Math.Abs(inMat.Get<byte>(points[j].Y, points[j].X) - value) > 15)
                                {
                                    count++;
                                }
                            }
                            if (count > 10)
                            {
                                detectedPoints.Add(new OpenCvSharp.Point(pointInLine[k + 1].X, pointInLine[k + 1].Y));
                                break;
                            }
                        }
                    }

                    //For Test
                    Cv2.Line(outMat, pt1, pt2, Scalar.Green, 1);
                }

                for (int i = 1; i < height ; i = i + pointOffset)
                {
                    double XP2 = (height - i) / slope;

                    OpenCvSharp.Point pt1 = new OpenCvSharp.Point(0, i);
                    OpenCvSharp.Point pt2 = new OpenCvSharp.Point(XP2, height);

                    List<OpenCvSharp.Point> pointInLine = GetPointsInLine(new LineSegmentPoint(pt1, pt2), inMat);
                    if (pointInLine.Count < 5) continue;

                    for (int k = 0; k < pointInLine.Count() - 51; k++)
                    {
                        if (Math.Abs(inMat.Get<byte>(pointInLine[k].Y, pointInLine[k].X) - inMat.Get<byte>(pointInLine[k + 1].Y, pointInLine[k + 1].X)) >= 15)
                        {
                            List<OpenCvSharp.Point> points = pointInLine.ToList().GetRange(k, 50);

                            int value = inMat.Get<byte>(pointInLine[k].Y, pointInLine[k].X);

                            int count = 0;
                            for (int j = 0; j < points.Count(); j++)
                            {
                                if (Math.Abs(inMat.Get<byte>(points[j].Y, points[j].X) - value) > 15)
                                {
                                    count++;
                                }
                            }
                            if (count > 10)
                            {
                                detectedPoints.Add(new OpenCvSharp.Point(pointInLine[k + 1].X, pointInLine[k + 1].Y));
                                break;
                            }
                        }
                    }

                    //For Test
                    Cv2.Line(outMat, pt1, pt2, Scalar.Green, 1);
                }

                if (detectedPoints.Count < 10)
                {
                    SetResultNG(inMat, outMat);
                    return;
                }

                List<OpenCvSharp.Point2d> filteredDistancePoints = new List<OpenCvSharp.Point2d>();
                filteredDistancePoints = RemovePointInLine(detectedPoints);
                filteredDistancePoints = RemovePointInLine(filteredDistancePoints);
                if (filteredDistancePoints.Count < 10)
                {
                    SetResultNG(inMat, outMat);
                    return;
                }

                ////For Test
                //for (int i = 0; i < filteredDistancePoints.Count; i++)
                //{
                //    Cv2.DrawMarker(outMat, (OpenCvSharp.Point)filteredDistancePoints[i], Scalar.Red, MarkerTypes.Cross, 15, 2);
                //}

                //Filter by radius
                Tuple<int, CCircle, List<OpenCvSharp.Point2d>>[] detectedPointCircles = new Tuple<int, CCircle, List<OpenCvSharp.Point2d>>[filteredDistancePoints.Count - 10];

                for (int i = 0; i < filteredDistancePoints.Count - 10; i++)
                {
                    List<OpenCvSharp.Point2d> points = filteredDistancePoints.GetRange(i, 10);
                    Point2d ct = new Point2d(0, 0);
                    double r = 0;
                    FitCircleLMS(points, out ct, out r);

                    detectedPointCircles[i] = new Tuple<int, CCircle, List<OpenCvSharp.Point2d>>(
                        i,
                        new CCircle(ct.X, ct.Y, r),
                        points
                        );
                }

                detectedPointCircles = detectedPointCircles.Where(dpc => Math.Abs(dpc.Item2.Radius - ExpectedCircle.Radius) < threshold * 2 &&
                    dpc.Item2.CenterX > 0 &&
                    dpc.Item2.CenterY > 0 &&
                    dpc.Item2.CenterX < inMat.Width &&
                    dpc.Item2.CenterY < inMat.Height).ToArray();

                if (detectedPointCircles.Count() < 3)
                {
                    SetResultNG(inMat, outMat);
                    return;
                }

                //Filter by distance of center
                Tuple<CCircle, List<OpenCvSharp.Point2d>, double>[] detectedCircleDistance = new Tuple<CCircle, List<OpenCvSharp.Point2d>, double>[detectedPointCircles.Count() - 1];
                for (int i = 1; i < detectedPointCircles.Count(); i++)
                {
                    detectedCircleDistance[i - 1] = new Tuple<CCircle, List<OpenCvSharp.Point2d>, double>
                        (
                            detectedPointCircles[i - 1].Item2,
                            detectedPointCircles[i - 1].Item3,
                            OpenCvSharp.Point.Distance(new OpenCvSharp.Point(detectedPointCircles[i - 1].Item2.CenterX, detectedPointCircles[i - 1].Item2.CenterY),
                            new OpenCvSharp.Point(detectedPointCircles[i].Item2.CenterX, detectedPointCircles[i].Item2.CenterY))
                        );
                }

                detectedCircleDistance = detectedCircleDistance.OrderBy(dcd => dcd.Item3).ToArray();
                Tuple<int, int> filteredDistanceCenterArray = AlgorithmHelper.FindLongestSubarray(detectedCircleDistance, 20);
                Tuple<CCircle, List<OpenCvSharp.Point2d>, double>[] filteredCircleDistance = new Tuple<CCircle, List<OpenCvSharp.Point2d>, double>[filteredDistanceCenterArray.Item2 - filteredDistanceCenterArray.Item1];
                if (filteredDistanceCenterArray.Item2 - filteredDistanceCenterArray.Item1 < 3)
                {
                    SetResultNG(inMat, outMat);
                    return;
                }
                filteredCircleDistance = detectedCircleDistance.ToList().GetRange(filteredDistanceCenterArray.Item1, filteredDistanceCenterArray.Item2 - filteredDistanceCenterArray.Item1).ToArray();

                ////For Test
                //foreach (var dpc in filteredCircleDistance)
                //{
                //    Cv2.Circle(outMat, (int)dpc.Item1.CenterX, (int)dpc.Item1.CenterY, (int)dpc.Item1.Radius, Scalar.Red, 2);
                //}

                List<Point2d> filteredPoints = new List<Point2d>();
                filteredPoints = filteredCircleDistance.SelectMany(dpc => dpc.Item2).ToList().ToHashSet().ToList();

                if (filteredPoints.Count < 3)
                {
                    SetResultNG(inMat, outMat);
                    return;
                }

                Point2d center = new Point2d(0, 0);
                double radius = 0;
                FitCircleLMS(filteredPoints, out center, out radius);

                CCircle resultCircle = new CCircle(center.X, center.Y, radius);

                if (Math.Abs(ExpectedCircle.Radius - radius) > threshold)
                {
                    Inputs["ImageMat"] = inMat.Clone();
                    Outputs["ImageMat"] = outMat;
                    ((CircleDetectionResult)Outputs["Circle"]).Judge = Core.Vision.EVisionJudge.NG;
                    ((CircleDetectionResult)Outputs["Circle"]).Circle = new CCircle(center.X, center.Y, radius);
                    Cv2.Circle(outMat, (int)center.X, (int)center.Y, (int)radius, Scalar.Green, 2);

                    ((CircleDetectionResult)Outputs["Circle"]).DrawAction = (image) =>
                    {
                        foreach (OpenCvSharp.Point point in filteredPoints)
                        {
                            Cv2.DrawMarker(image, point, Scalar.Yellow, MarkerTypes.Cross, 15, 2);
                        }

                        Cv2.Circle(image, new OpenCvSharp.Point((int)resultCircle.CenterX, (int)resultCircle.CenterY), (int)resultCircle.Radius, Scalar.Red, 2);
                        Cv2.Circle(image, new OpenCvSharp.Point(resultCircle.CenterX, resultCircle.CenterY), 10, Scalar.Yellow, 5);
                    };
                    return;
                }
                ((CircleDetectionResult)Outputs["Circle"]).Circle = resultCircle;

                if (resultCircle.Radius == 0)
                {
                    Inputs["ImageMat"] = inMat.Clone();

                    ((CircleDetectionResult)Outputs["Circle"]).Judge = Core.Vision.EVisionJudge.NG;
                    ((CircleDetectionResult)Outputs["Circle"]).Circle = resultCircle;
                    return;
                }

                ((CircleDetectionResult)Outputs["Circle"]).DrawAction = (image) =>
                {
                    foreach (OpenCvSharp.Point point in filteredPoints)
                    {
                        Cv2.DrawMarker(image, point, Scalar.Yellow, MarkerTypes.Cross, 15, 2);
                    }

                    Cv2.Circle(image, new OpenCvSharp.Point((int)resultCircle.CenterX, (int)resultCircle.CenterY), (int)resultCircle.Radius, Scalar.Blue, 2);
                    Cv2.Circle(image, new OpenCvSharp.Point(resultCircle.CenterX, resultCircle.CenterY), 10, Scalar.Yellow, 5);
                };
                ((CircleDetectionResult)Outputs["Circle"]).Judge = Core.Vision.EVisionJudge.OK;
                ((CircleDetectionResult)Outputs["Circle"]).Circle = resultCircle;

                Cv2.Circle(outMat, (int)center.X, (int)center.Y, (int)radius, Scalar.Green, 2);
                #endregion

                Inputs["ImageMat"] = inMat.Clone();
                Outputs["ImageMat"] = outMat;
            }
        }
        private List<OpenCvSharp.Point> GetPointsInLine(LineSegmentPoint line, Mat imageMat)
        {
            OpenCvSharp.Point p1 = line.P1;
            OpenCvSharp.Point p2 = line.P2;
            List<OpenCvSharp.Point> points = new List<OpenCvSharp.Point>();
            int dx = Math.Abs(p2.X - p1.X);
            int dy = Math.Abs(p2.Y - p1.Y);
            int sx = p1.X < p2.X ? 1 : -1;
            int sy = p1.Y < p2.Y ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                if (p1.X > 0 && p1.X < imageMat.Width && p1.Y > 0 && p1.Y < imageMat.Height)
                {
                    points.Add(new OpenCvSharp.Point(p1.X, p1.Y));
                }
                if (p1.X == p2.X && p1.Y == p2.Y)
                    break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    p1.X += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    p1.Y += sy;
                }
            }

            return points;
        }

        private void FitCircleLMS(List<Point2d> points, out Point2d center, out double radius)
        {
            int N = points.Count;
            Mat A = new Mat(N, 3, MatType.CV_64FC1, Scalar.All(0));
            Mat Y = new Mat(N, 1, MatType.CV_64FC1, Scalar.All(0));

            for (int i = 0; i < N; i++)
            {
                A.Set<double>(i, 0, 2 * points[i].X);
                A.Set<double>(i, 1, 2 * points[i].Y);
                A.Set<double>(i, 2, 1);
                Y.Set<double>(i, 0, Math.Pow(points[i].X, 2) + Math.Pow(points[i].Y, 2));
            }

            Mat X = (A.T() * A).Inv() * A.T() * Y;

            double x0 = X.Get<double>(0, 0);
            double y0 = X.Get<double>(1, 0);
            double C = X.Get<double>(2, 0);

            double radiusSquared = C + Math.Pow(x0, 2) + Math.Pow(y0, 2);
            if (radiusSquared < 0)
            {
                center = new Point2d(0, 0);
                radius = 0;
            }
            else
            {
                center = new Point2d(x0, y0);
                radius = Math.Sqrt(radiusSquared);
            }
        }

        private void FitCircleLMS(List<OpenCvSharp.Point> points, out Point2d center, out double radius)
        {
            int N = points.Count;
            Mat A = new Mat(N, 3, MatType.CV_64FC1, Scalar.All(0));
            Mat Y = new Mat(N, 1, MatType.CV_64FC1, Scalar.All(0));

            for (int i = 0; i < N; i++)
            {
                A.Set<double>(i, 0, 2 * points[i].X);
                A.Set<double>(i, 1, 2 * points[i].Y);
                A.Set<double>(i, 2, 1);
                Y.Set<double>(i, 0, Math.Pow(points[i].X, 2) + Math.Pow(points[i].Y, 2));
            }

            Mat X = (A.T() * A).Inv() * A.T() * Y;

            double x0 = X.Get<double>(0, 0);
            double y0 = X.Get<double>(1, 0);
            double C = X.Get<double>(2, 0);

            double radiusSquared = C + Math.Pow(x0, 2) + Math.Pow(y0, 2);
            if (radiusSquared < 0)
            {
                center = new Point2d(0, 0);
                radius = 0;
            }
            else
            {
                center = new Point2d(x0, y0);
                radius = Math.Sqrt(radiusSquared);
            }
        }

        private List<OpenCvSharp.Point2d> RemovePointInLine(List<OpenCvSharp.Point2d> points)
        {
            Tuple<int, double, double>[] detectedPointDistanceAngles = new Tuple<int, double, double>[points.Count - 1];

            for (int i = 1; i < points.Count; i++)
            {
                detectedPointDistanceAngles[i - 1] = new Tuple<int, double, double>
                    (
                        i - 1,
                        OpenCvSharp.Point2d.Distance(points[i - 1], points[i]),
                        AlgorithmHelper.FindAngle((OpenCvSharp.Point)points[i - 1], (OpenCvSharp.Point)points[i])
                    );
            }

            detectedPointDistanceAngles = detectedPointDistanceAngles
                .OrderBy(da => da.Item3)
                .ThenBy(da => da.Item2)
                .ToArray();

            var filteredArray = AlgorithmHelper.FindLongestSubarray(detectedPointDistanceAngles);

            List<OpenCvSharp.Point2d> filteredPoints = new List<OpenCvSharp.Point2d>();
            for (int i = filteredArray.Item1; i < filteredArray.Item2; i++)
            {
                int index = detectedPointDistanceAngles[i].Item1;
                if (filteredPoints.Contains(points[index]) == false)
                    filteredPoints.Add(points[index]);
                if (filteredPoints.Contains(points[index + 1]) == false)
                    filteredPoints.Add(points[index + 1]);
            }

            foreach (var point in filteredPoints)
            {
                points.Remove(point);
            }
            return points;
        }

        private void SetResultNG(Mat inMat, Mat outMat)
        {
            Inputs["ImageMat"] = inMat.Clone();
            Outputs["ImageMat"] = outMat;
            ((CircleDetectionResult)Outputs["Circle"]).Judge = Core.Vision.EVisionJudge.NG;
            ((CircleDetectionResult)Outputs["Circle"]).Circle = new CCircle(0, 0, 0);
        }
    }
}
