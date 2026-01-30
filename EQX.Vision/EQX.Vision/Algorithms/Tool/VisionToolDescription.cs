using EQX.Core.Vision.Algorithms;
using Newtonsoft.Json;

namespace EQX.Vision.Algorithms
{
    public class VisionToolDescription
    {
        public int Id { get; }

        public int Width { get; }
        public int Height { get; }
        public int NumberOfInputKeys => _visionTool.Inputs.Keys.Count;
        public int NumberOfOutputKeys => _visionTool.Outputs.Keys.Count;
        public Point Position { get; }

        public VisionToolDescription(IVisionTool visionTool, Point position)
        {
            Id = visionTool.Id;
            _visionTool = visionTool;
            Position = position;
        }

        [JsonConstructor]
        public VisionToolDescription(int id,int width,int height,Point position,IVisionTool _visionTool)
        {
            Id= id;
            Width = width;
            Height = height;
            Position = position;
            this._visionTool = _visionTool;
        }
        [JsonProperty]
        private IVisionTool _visionTool;
    }
}
