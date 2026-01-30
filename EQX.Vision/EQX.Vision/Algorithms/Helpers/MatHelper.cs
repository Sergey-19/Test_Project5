using EQX.Core.Vision.Algorithms;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    internal static class ObjectCollectionHelper
    {
        internal static int IndexOf(this IEnumerable<KeyType> keyTypes, string key)
        {
            return keyTypes.ToList().FindIndex(kt => kt.Key == key);
        }

        internal static bool Contains(this IEnumerable<KeyType> keyTypes, string key)
        {
            return keyTypes.Any(kt => kt.Key == key);
        }

        internal static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        internal static bool IsNumericType(this Type o)
        {
            switch (Type.GetTypeCode(o))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }

    internal class MatHelper
    {
        internal static Mat RotateImage(Mat src, double angle)
        {
            Mat dst = new Mat();
            Point2f center = new Point2f(src.Width / 2, src.Height / 2);
            Mat rotMat = Cv2.GetRotationMatrix2D(center, angle, 1.0);
            Cv2.WarpAffine(src, dst, rotMat, src.Size());
            return dst;
        }
        internal static void DrawRotatedRectangle(Mat img, OpenCvSharp.Point topLeft, OpenCvSharp.Size rectSize, double angle)
        {
            Point2f[] rectPoints = new Point2f[4];
            rectPoints[0] = new Point2f(topLeft.X, topLeft.Y);
            rectPoints[1] = new Point2f(topLeft.X + rectSize.Width, topLeft.Y);
            rectPoints[2] = new Point2f(topLeft.X + rectSize.Width, topLeft.Y + rectSize.Height);
            rectPoints[3] = new Point2f(topLeft.X, topLeft.Y + rectSize.Height);

            Point2f center = new Point2f(topLeft.X + rectSize.Width / 2, topLeft.Y + rectSize.Height / 2);
            Mat rotMat = Cv2.GetRotationMatrix2D(center, angle, 1.0);

            Point2f[] rotatedRectPoints = new Point2f[4];
            for (int i = 0; i < 4; i++)
            {
                rotatedRectPoints[i] = new Point2f();
                rotatedRectPoints[i].X = (float)(rotMat.At<double>(0, 0) * rectPoints[i].X + rotMat.At<double>(0, 1) * rectPoints[i].Y + rotMat.At<double>(0, 2));
                rotatedRectPoints[i].Y = (float)(rotMat.At<double>(1, 0) * rectPoints[i].X + rotMat.At<double>(1, 1) * rectPoints[i].Y + rotMat.At<double>(1, 2));
            }

            for (int i = 0; i < 4; i++)
            {
                Cv2.Line(img, (OpenCvSharp.Point)rotatedRectPoints[i], (OpenCvSharp.Point)rotatedRectPoints[(i + 1) % 4], Scalar.Red, 2);
            }
        }
        internal static void DrawExtendedLine(Mat image, OpenCvSharp.Point point1, OpenCvSharp.Point point2, Scalar color, int thickness)
        {
            int width = image.Width;
            int height = image.Height;

            //Angle Coefficient
            double m = (double)(point2.Y - point1.Y) / (point2.X - point1.X);
            double c = point1.Y - m * point1.X;

            OpenCvSharp.Point left = new OpenCvSharp.Point(0, (int)c);
            OpenCvSharp.Point right = new OpenCvSharp.Point(width, (int)(m * width + c));
            OpenCvSharp.Point top = new OpenCvSharp.Point((int)(-c / m), 0);
            OpenCvSharp.Point bottom = new OpenCvSharp.Point((int)((height - c) / m), height);

            OpenCvSharp.Point start = left;
            OpenCvSharp.Point end = right;

            if (left.Y < 0 || left.Y > height)
            {
                start = top;
            }

            if (right.Y < 0 || right.Y > height)
            {
                end = bottom;
            }

            Cv2.Line(image, start, end, color, thickness);
        }
    }
}
