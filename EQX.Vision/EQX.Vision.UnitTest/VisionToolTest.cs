using EQX.Core.Common;
using EQX.Core.Vision.Algorithms;
using EQX.Core.Vision.Grabber;
using EQX.Vision.Algorithms;
using EQX.Vision.LightController;
using OpenCvSharp;

namespace EQX.Vision.UnitTest
{
    public class VisionToolTest
    {
        IVisionTool rawImageToMatTool;
        IVisionTool bitwiseTool;
        private void initVisionTool()
        {
            byte[] rawData = File.ReadAllBytes(@"GrabImage.txt");

            rawImageToMatTool = new ConvertGrabDataToMatTool();
            bitwiseTool = new BitwiseTool();

            rawImageToMatTool.Inputs["GrabData"] = new GrabData()
            {
                ImageBuffer = rawData,
                Height = 1944,
                Width = 2592,
            };
        }

        [Fact]
        public void HIKVisionLightControllerTest()
        {
            LightControllerMV lightController = new LightControllerMV(1, "Align Light", "COM2");
            lightController.Connect();

            lightController.SetLightLevel(1, 200);
            lightController.Disconnect();
        }

        [Fact]
        public void VisionToolIdUniqueTest()
        {
            VisionToolBase tool1 = new VisionToolBase();
            VisionToolBase tool2 = new VisionToolBase();
            VisionToolBase tool3 = new VisionToolBase();

            Assert.NotEqual(tool1.Id, tool2.Id);
            Assert.NotEqual(tool2.Id, tool3.Id);
            Assert.NotEqual(tool1.Id, tool3.Id);
        }

        [Fact]
        public void TestRawImageToMatTool()
        {
            initVisionTool();

            rawImageToMatTool.RunAsync(1000);
            rawImageToMatTool.ToolRunFinished = ((errorCode, outputs) =>
            {
                Assert.Equal(ERunState.Done, rawImageToMatTool.State);
                Assert.NotNull(rawImageToMatTool.Outputs["ImageMat"]);
                Assert.IsType<Mat>(rawImageToMatTool.Outputs["ImageMat"]);

                bitwiseTool.Inputs["ImageMat"] = rawImageToMatTool.Outputs["ImageMat"];
                bitwiseTool.RunAsync(1000);
                bitwiseTool.ToolRunFinished = ((errorCode, outputs) =>
                {
                    Assert.Equal(ERunState.Done, bitwiseTool.State);
                    Assert.NotNull(bitwiseTool.Outputs["ImageMat"]);
                    Assert.IsType<Mat>(bitwiseTool.Outputs["ImageMat"]);

                    using (new Window("Output Image Mat", (Mat)bitwiseTool.Outputs["ImageMat"]!))
                    {
                        Cv2.WaitKey();
                    }
                });
            });
        }
    }
}
