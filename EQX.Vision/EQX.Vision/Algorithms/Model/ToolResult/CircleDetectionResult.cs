using EQX.Core.Vision;
using Newtonsoft.Json;
using OpenCvSharp;

namespace EQX.Vision.Algorithms.Model.ToolResult
{
    public class CircleDetectionResult : IVisionResult
    {
        public EVisionJudge Judge { get; set; }

        public CCircle Circle { get; set; }
        [JsonIgnore]
        public Action<Mat> DrawAction { get; set; }

        public void Pixel2mm(double pixelSize)
        {
            Circle.CenterX *= pixelSize;
            Circle.CenterY *= pixelSize;
            Circle.Radius *= pixelSize;
        }

        public string ToString()
        {
            return $"X:{Circle.CenterX: 0.###}, Y:{Circle.CenterY:0.###}, Radius:{Circle.Radius:0.###}";
        }
    }
}
