using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms;
using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.Vision.UnitTest
{
    public class VisionFlowTest
    {
        IVisionFlow Flow;

        private void InitFlow()
        {
            Flow = new VisionFlow("Flow Test");

            IVisionTool smoothingTool = new SmoothingTool();
            smoothingTool.Parameters["Width"] = 13.0;
            smoothingTool.Parameters["Height"] = 13.0;
            smoothingTool.Parameters["SigmaX"] = 10.0;
            Flow.VisionTools.Add(smoothingTool);

            IVisionTool thresholdTool = new ThresholdTool();
            thresholdTool.Parameters["Threshold"] = 50;
            thresholdTool.Parameters["ThresholdType"] = ThresholdTypes.BinaryInv;
            Flow.VisionTools.Add(thresholdTool);

            IVisionTool lineDetectionTool = new LineDetectionTool();

            //lineDetectionTool.Parameters["Direction"] = EDetectDirection.Top2Bottom;
            //lineDetectionTool.Parameters["ROI"] = new CRectangle() { X = 911, Y = 949, Width = 843, Height = 121 };
            Flow.VisionTools.Add(lineDetectionTool);

            VisionToolConnection connection2 = new VisionToolConnection(smoothingTool.Id, thresholdTool.Id, "ImageMat");
            VisionToolConnection connection3 = new VisionToolConnection(thresholdTool.Id, lineDetectionTool.Id, "ImageMat");

            Flow.VisionToolConnections.Add(connection2);
            Flow.VisionToolConnections.Add(connection3);
        }

        [Fact]
        public async Task FlowTest()
        {
            InitFlow();
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            };
            File.WriteAllText("Flow.json", JsonConvert.SerializeObject(Flow, settings));

            await Flow.RunAsync(1000);

            Debug.WriteLine(Flow.VisionTools.Last().Outputs["Angle"].ToString());
            using (new Window("Output Image Mat", (Mat)Flow.VisionTools.Last().Outputs["ImageMat"]!, WindowFlags.AutoSize))
            {
                Cv2.WaitKey();
            }
        }

        [Fact]
        public void InitToolList()
        {
            List<IVisionTool> ToolList = new List<IVisionTool>()
            {
                new GrabTool(),
                new SmoothingTool(),
                new ThresholdTool(),
                new AdaptiveThresholdTool(),

                new LineDetectionTool(),
                new AlignTwoLinesTool(),

                new OutputTool(),
            };

            JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            };
            File.WriteAllText("ToolList.json", JsonConvert.SerializeObject(ToolList, Formatting.Indented, jsonSettings));
        }
    }
}
