using EQX.Core.Vision.Algorithms;
using EQX.Core.Vision.Grabber;
using EQX.Vision.Algorithms;
using EQX.Vision.Grabber;
using OpenCvSharp;
using System.Diagnostics;



//CameraGrabTest();
VisionFlowTest();

void CameraGrabTest()
{
    Console.WriteLine("Camera Grab Test!");

    ICamera Camera = new CameraBaslerGigE("192.168.0.100", "Basler Cam.");

    bool connectStatus = Camera.Connect();

    GrabData grabData;

    for (int i = 0; i < 20; i++)
    {
        int startTime = Environment.TickCount;

        Debug.WriteLine($"Loop #{i}");
        grabData = Camera.GrabSingle();

        if (i == 10)
        {
            File.WriteAllBytes("GrabImage.txt", grabData.ImageBuffer!);
        }

        int costTime = Environment.TickCount - startTime;
        Debug.WriteLine($"[{costTime} ms] Image size = {grabData.ImageBuffer?.Length}");
    }

    Camera.Disconnect();

    Console.ReadLine();

    return;
}

void VisionToolRelation()
{

}

async void VisionFlowTest()
{
    VisionFlow visionFlow = new VisionFlow("Flow Test");

    IVisionTool convertColorTool = new ConvertColorTool()
    {
        Id = 2,
        Name = "ConvertColorTool"
    };
    convertColorTool.Parameters["ColorConversionCode"] = ColorConversionCodes.BGR2GRAY;

    IVisionTool inRangeTool = new InRangeTool()
    {
        Id = 3,
        Name = "InRangeTool"
    };
    inRangeTool.Parameters["Threshold1"] = 50.0;
    inRangeTool.Parameters["Threshold2"] = 100.0;

    IVisionTool thresholdTool = new ThresholdTool()
    {
        Id = 4,
        Name = "ThresholdTool"
    };
    thresholdTool.Parameters["Threshold"] = 40.0;
    thresholdTool.Parameters["MaxValue"] = 255.0;
    thresholdTool.Parameters["ThresholdType"] = ThresholdTypes.Binary;

    IVisionTool bitwiseTool = new BitwiseTool()
    {
        Id = 5,
        Name = "BitwiseTool"
    };

    IVisionTool showImageTool1 = new ShowImageTool()
    {
        Id = 6,
        Name = "ShowImageTool1"
    };

    IVisionTool showImageTool2 = new ShowImageTool()
    {
        Id = 7,
        Name = "ShowImageTool2"
    };

    visionFlow.VisionTools.Add(loadImageTool);
    visionFlow.VisionTools.Add(convertColorTool);
    visionFlow.VisionTools.Add(inRangeTool);
    visionFlow.VisionTools.Add(thresholdTool);
    visionFlow.VisionTools.Add(bitwiseTool);
    visionFlow.VisionTools.Add(showImageTool1);
    visionFlow.VisionTools.Add(showImageTool2);

    int i = 0;
    do
    {
        int tick = Environment.TickCount;
        //visionFlow.ClearTool();

        //visionFlow.AddConnection(1, 2);
        //visionFlow.AddConnection(2, 3);
        //visionFlow.AddConnection(3, 6);
        //visionFlow.AddConnection(2, 4);
        //visionFlow.AddConnection(4, 5);
        //visionFlow.AddConnection(5, 7);

        await visionFlow.RunAsync(5000);
        Console.WriteLine($"#{++i} Run cost {Environment.TickCount - tick}ms");
        Thread.Sleep(100);
    }
    while (i < 1000);
}
