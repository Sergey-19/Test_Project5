using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    internal class AlgorithmHelper
    {
        internal static void Get2PointLine(EDetectDirection Detect_Direction, Rect rectROI, Line2D DetectedLine, ref OpenCvSharp.Point[] DetectedPointofLine)
        {
            DetectedPointofLine = new OpenCvSharp.Point[2];
            if (Detect_Direction == EDetectDirection.Horizontal)
            {
                DetectedPointofLine[0] = new OpenCvSharp.Point(DetectedLine.X1 - DetectedLine.Vx * rectROI.Width, DetectedLine.Y1 - DetectedLine.Vy * rectROI.Width); // Right Point.
                DetectedPointofLine[1] = new OpenCvSharp.Point(DetectedLine.X1 + DetectedLine.Vx * rectROI.Width, DetectedLine.Y1 + DetectedLine.Vy * rectROI.Width); // Left Point.
            }
            else
            {
                DetectedPointofLine[0] = new OpenCvSharp.Point(DetectedLine.X1 - DetectedLine.Vx * rectROI.Width, DetectedLine.Y1 - DetectedLine.Vy * rectROI.Width); // Right Point.
                DetectedPointofLine[1] = new OpenCvSharp.Point(DetectedLine.X1 + DetectedLine.Vx * rectROI.Width, DetectedLine.Y1 + DetectedLine.Vy * rectROI.Width); // Left Point.
            }
        }

        internal static (double a, double b, double c) MakeLineEquation(LineSegmentPoint line)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            //https://content.byui.edu/file/b8b83119-9acc-4a7b-bc84-efacf9043998/1/Math-2-11-2.html
            ////////////////////////////////////////////////////////////////////////////////////////

            double a, b, c;

            if (line.P2.X - line.P1.X != 0)
            {
                a = (double)(line.P2.Y - line.P1.Y) / (line.P2.X - line.P1.X);
                b = -1;
                c = -a * line.P2.X + line.P2.Y;
            }
            else
            {
                a = (double)(line.P2.Y - line.P1.Y);
                b = -1;
                c = -a * line.P2.X + line.P2.Y;
            }

            return (a, b, c);
        }

        internal static bool FindInterSectionPoint(LineSegmentPoint line1, LineSegmentPoint line2, ref Point2f intersectionPoint)
        {
            double a1 = 0;
            double b1 = 0;
            double c1 = 0;
            (a1, b1, c1) = MakeLineEquation(line1);

            double a2 = 0;
            double b2 = 0;
            double c2 = 0;
            (a2, b2, c2) = MakeLineEquation(line2);

            if (a1 == a2)
            {
                intersectionPoint.X = 0;
                intersectionPoint.Y = 0;
                return false;
            }

            double x = (c2 - c1) / (a1 - a2);
            double y = a1 * x + c1;
            intersectionPoint.X = (float)x;
            intersectionPoint.Y = (float)y;
            return true;
        }

        internal static double FindAngleCoefficientLine(OpenCvSharp.Point startPoint, OpenCvSharp.Point endPoint)
        {
            if ((endPoint.X - startPoint.X) == 0) return 0;

            return (double)(endPoint.Y - startPoint.Y) / (double)(endPoint.X - startPoint.X);
        }

        internal static double FindYIntercept(OpenCvSharp.Point startPoint, OpenCvSharp.Point endPoint)
        {
            return startPoint.Y - FindAngleCoefficientLine(startPoint, endPoint) * startPoint.X;
        }

        internal static double FindAngle(OpenCvSharp.Point startPoint, OpenCvSharp.Point endPoint)
        {
            if ((endPoint.X - startPoint.X) == 0) return 90;

            return Math.Atan(FindAngleCoefficientLine(startPoint, endPoint)) * 180 / Math.PI;
        }

        internal static OpenCvSharp.Point GetRotatePoint(OpenCvSharp.Point2f corner, OpenCvSharp.Point2f center, double theta)
        {
            double xTrans = corner.X - center.X;
            double yTrans = corner.Y - center.Y;

            double xRot = xTrans * Math.Cos(theta * Math.PI / 180) - yTrans * Math.Sin(theta * Math.PI / 180);
            double yRot = xTrans * Math.Sin(theta * Math.PI / 180) + yTrans * Math.Cos(theta * Math.PI / 180);

            double xNew = xRot + center.X;
            double yNew = yRot + center.Y;

            return new OpenCvSharp.Point(xNew, yNew);
        }

        internal static (int, int) FindStableRange(double[] numbers, double threshold)
        {
            int start = -1;
            int end = -1;

            for (int i = 0; i < numbers.Length; i++)
            {
                int tempStart = i;
                int tempEnd = i;

                while (tempEnd < numbers.Length - 1 && Math.Abs(numbers[tempEnd + 1] - numbers[tempStart]) <= threshold)
                {
                    tempEnd++;
                }

                if (tempEnd > tempStart)
                {
                    if (tempEnd - tempStart > end - start)
                    {
                        start = tempStart;
                        end = tempEnd;
                    }
                }
            }

            return (start, end);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array">Index, Point to point distance, angle</param>
        /// <returns></returns>
        internal static Tuple<int, int> FindLongestSubarray(Tuple<int, double, double,double>[] array)
        {
            int start = -1, end = -1;
            for (int i = 1; i < array.Length; i++)
            {
                int tempStart = i;
                int tempEnd = i;

                while (tempEnd < array.Length - 1 &&
                    Math.Abs(array[tempEnd + 1].Item2 - array[tempStart].Item2) < 20 * (Math.Sqrt(2) - 1) &&
                    Math.Abs(array[tempEnd+1].Item3 - array[tempStart].Item3) < 2.5 &&
                    Math.Abs(array[tempEnd + 1].Item4 - array[tempStart].Item4) < 80.0)
                {
                    tempEnd++;
                }

                if (tempEnd > tempStart)
                {
                    if (tempEnd - tempStart > end - start)
                    {
                        start = tempStart;
                        end = tempEnd;
                    }
                }
            }
            return new Tuple<int, int>(start, end);
        }

        internal static Tuple<int, int> FindLongestSubarray(Tuple<int, double, double>[] array)
        {
            int start = -1, end = -1;
            for (int i = 1; i < array.Length; i++)
            {
                int tempStart = i;
                int tempEnd = i;

                while (tempEnd < array.Length - 1 &&
                    Math.Abs(array[tempEnd + 1].Item2 - array[tempStart].Item2) < 20 * (Math.Sqrt(2) - 1) &&
                    Math.Abs(array[tempEnd + 1].Item3 - array[tempStart].Item3) < 2.5)
                {
                    tempEnd++;
                }

                if (tempEnd > tempStart)
                {
                    if (tempEnd - tempStart > end - start)
                    {
                        start = tempStart;
                        end = tempEnd;
                    }
                }
            }
            return new Tuple<int, int>(start, end);
        }

        internal static Tuple<int, int> FindLongestSubarray(Tuple<int, double>[] array, double threshold)
        {
            int start = -1, end = -1;
            for (int i = 1; i < array.Length; i++)
            {
                int tempStart = i;
                int tempEnd = i;

                while (tempEnd < array.Length - 1 && Math.Abs(array[tempEnd + 1].Item2 - array[tempStart].Item2) <= threshold)
                {
                    tempEnd++;
                }

                if (tempEnd > tempStart)
                {
                    if (tempEnd - tempStart > end - start)
                    {
                        start = tempStart;
                        end = tempEnd;
                    }
                }
            }
            return new Tuple<int, int>(start, end);
        }

        internal static Tuple<int, int> FindLongestSubarray(Tuple<CCircle, List<OpenCvSharp.Point2d>, double>[] array, double threshold)
        {
            int start = -1, end = -1;
            for (int i = 1; i < array.Length; i++)
            {
                int tempStart = i;
                int tempEnd = i;

                while (tempEnd < array.Length - 1 && Math.Abs(array[tempEnd + 1].Item3 - array[tempStart].Item3) <= threshold)
                {
                    tempEnd++;
                }

                if (tempEnd > tempStart)
                {
                    if (tempEnd - tempStart > end - start)
                    {
                        start = tempStart;
                        end = tempEnd;
                    }
                }
            }
            return new Tuple<int, int>(start, end);

        }
    }
}
