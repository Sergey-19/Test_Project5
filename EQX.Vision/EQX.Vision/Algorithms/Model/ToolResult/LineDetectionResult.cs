using EQX.Core.Vision;
using Newtonsoft.Json;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class LineDetectionResult : IVisionResult
    {
        public LineSegmentPoint LineSegmentPoint { get; set; }
        public Line2D Line2D { get; set; }
        public double Angle { get; set; }
        public EVisionJudge Judge { get; set; }
        [JsonIgnore]
        public Action<Mat> DrawAction { get; set; }

        public LineDetectionResult()
        {
            LineSegmentPoint = new LineSegmentPoint();
            Line2D = new Line2D(0, 0, 0, 0);
        }

        public string ToString()
        {
            return Angle.ToString();
        }

        public void Pixel2mm(double pixelSize)
        {
        }
    }
}
