using EQX.Core.Vision;
using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms.Model.Parameters;
using EQX.Vision.Algorithms.Model.ToolResult;
using Newtonsoft.Json;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OutputTool : VisionToolBase
    {
        private int _numberOfOutput;

        public int NumberOfOutput
        {
            get
            {
                int numberOfOutput = Inputs.Keys.Count();
                return _numberOfOutput = numberOfOutput;
            }
            set
            {
                if (value < 1) return;

                if (value > _numberOfOutput)
                {
                    for (int i = 0; i < value - _numberOfOutput; i++)
                    {
                        Inputs.Keys.Add(new Core.Vision.Algorithms.KeyType($"Output {Inputs.Keys.Count.ToString()}", null));
                    }
                }
                else if (_numberOfOutput > value)
                {
                    for (int i = 0; i < _numberOfOutput - value; i++)
                    {
                        Inputs.Keys.RemoveAt(Inputs.Keys.Count - 1);
                    }
                }
                _numberOfOutput = value;
            }
        }

        public bool IsAlignResult => Inputs.Keys.Contains("AlignResult");
        public void UpdateParameters()
        {
            OnPropertyChanged(nameof(IsAlignResult));
        }
        public OutputTool()
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType>()
            {
                new KeyType("AlignParameter",typeof(AlignParameter)),
            });

            Inputs = new ObjectCollection(new List<KeyType>
            {
                new KeyType("ImageMat",typeof(Mat))
            });

            Parameters["AlignParameter"] = new AlignParameter();
        }

        [JsonConstructor]
        public OutputTool(ObjectCollection inputs, ObjectCollection parameters)
        {
            Inputs = inputs;
            Parameters = new ObjectCollection(new List<KeyType>()
            {
                new KeyType("AlignParameter",typeof(AlignParameter)),
            });

            Parameters["AlignParameter"] = parameters["AlignParameter"];
        }
        internal override bool ValidInputs()
        {
            if (Inputs["ImageMat"] == null) throw new ArgumentNullException("ImageMat");
            if (Inputs["ImageMat"]!.GetType() != typeof(Mat)) throw new ArgumentException("ImageMat");

            return true;
        }

        internal override void DIPFunction()
        {
            if (Inputs.Keys.Contains("ImageMat") == false) return;
            if (Inputs["ImageMat"] is Mat == false) return;

            AlignParameter alignParameter = (AlignParameter)Parameters["AlignParameter"];
            using (Mat inMat = (Mat)Inputs["ImageMat"])
            {
                Cv2.CvtColor(inMat, inMat, ColorConversionCodes.GRAY2BGR);
                foreach (KeyType key in Inputs.Keys)
                {
                    if (Inputs[key.Key] is IVisionResult result)
                    {
                        result.DrawAction?.Invoke(inMat);
                        result.Pixel2mm(PixelSize);
                        if (result is AlignResult alignResult)
                        {
                            if(alignParameter == null) alignParameter = new AlignParameter();
                            alignResult.Offset.X -= alignParameter.OffsetX;
                            alignResult.Offset.Y -= alignParameter.OffsetY;
                            alignResult.Offset.Theta -= alignParameter.OffsetTheta;
                        }
                        Cv2.PutText(inMat, result.ToString(),
                                    new OpenCvSharp.Point(50, 100 * Inputs.Keys.IndexOf(key)),
                                    HersheyFonts.Italic,
                                    2.5, result.Judge == EVisionJudge.OK ? Scalar.GreenYellow : Scalar.Red, thickness: 5);
                    }
                }
                Inputs["ImageMat"] = inMat.Clone();
            }
        }
    }
}
