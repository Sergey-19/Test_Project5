using EQX.Core.Vision.Algorithms;
using EQX.Core.Vision.Grabber;
using EQX.Vision.Algorithms.Model.ToolResult;
using EQX.Vision.Grabber.Helpers;
using Newtonsoft.Json;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    public class GrabTool : VisionToolBase
    {
        public ICamera Camera { get; set; }
   
        public GrabTool()
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("IsUseCamera", typeof(bool)),
                new KeyType("IsUseDataBase", typeof(bool)),
                new KeyType("ImagePath", typeof(string)),
                new KeyType("ReadMode", typeof(ImreadModes)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            Parameters["IsUseDataBase"] = false;
            Parameters["IsUseCamera"] = true;
            Parameters["ReadMode"] = ImreadModes.Grayscale;
        }

        [JsonConstructor]
        public GrabTool(ObjectCollection parameters)
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("IsUseCamera", typeof(bool)),
                new KeyType("IsUseDataBase", typeof(bool)),
                new KeyType("ImagePath", typeof(string)),
                new KeyType("ReadMode", typeof(ImreadModes)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            Parameters["IsUseCamera"] = parameters["IsUseCamera"];
            Parameters["IsUseDataBase"] = parameters["IsUseDataBase"];
            Parameters["ImagePath"] = parameters["ImagePath"];
            Parameters["ReadMode"] = Enum.Parse<ImreadModes>(parameters["ReadMode"].ToString());
        }

        internal override void DIPFunction()
        {
            bool isUseDataBase = Convert.ToBoolean(Parameters["IsUseDataBase"]);
            bool isUseCamera = Convert.ToBoolean(Parameters["IsUseCamera"]);
            string imagePath = Convert.ToString(Parameters["ImagePath"]);
            ImreadModes readMode = (ImreadModes)Parameters["ReadMode"];

            if (isUseCamera)
            {
                GrabData ImageGrab = Camera.GrabSingle();
                Outputs["ImageMat"] = GrabberHelpers.ToMat(ImageGrab);
            }
            else
            {
                Outputs["ImageMat"] = new Mat(imagePath, readMode);
            }
        }
    }
}
