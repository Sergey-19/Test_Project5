using EQX.Core.Vision;
using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.Vision.Algorithms.Model.ToolResult
{
    public class AlignResult : IVisionResult
    {
        public EVisionJudge Judge { get; set; }
        public XYTOffset Offset { get; internal set; }
        [JsonIgnore]
        public Action<Mat> DrawAction { get ; set ; }

        public AlignResult()
        {
            Offset = new XYTOffset();
        }

        public string ToString()
        {
            return Offset.ToString();
        }

        public void Pixel2mm(double pixelSize)
        {
            Offset.X *= pixelSize;
            Offset.Y *= pixelSize;
        }
    }
}
