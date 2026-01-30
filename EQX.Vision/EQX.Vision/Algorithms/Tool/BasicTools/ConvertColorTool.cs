using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms.Model.ToolResult;
using OpenCvSharp;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace EQX.Vision.Algorithms
{
    public class ConvertColorTool : VisionToolBase
    {
        public ObservableCollection<ColorConversionCodes> ColorConversionCodeList { get; set; }

        public ConvertColorTool()
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("ColorConversionCode", typeof(ObservableCollection<ColorConversionCodes>)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            ColorConversionCodeList = new ObservableCollection<ColorConversionCodes>();
            var enumType = typeof(ColorConversionCodes);
            var values = Enum.GetValues(enumType);

            foreach (var value in values)
            {
                ColorConversionCodeList.Add((ColorConversionCodes)value);
            }
            Parameters["ColorConversionCode"] = ColorConversionCodes.BGR2GRAY;

        }

        [JsonConstructor]
        public ConvertColorTool(ObjectCollection parameters)
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("ColorConversionCode", typeof(ObservableCollection<ColorConversionCodes>)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });

            ColorConversionCodeList = new ObservableCollection<ColorConversionCodes>();
            var enumType = typeof(ColorConversionCodes);
            var values = Enum.GetValues(enumType);

            foreach (var value in values)
            {
                ColorConversionCodeList.Add((ColorConversionCodes)value);
            }
            Parameters["ColorConversionCode"] = Enum.Parse<ColorConversionCodes>(parameters["ColorConversionCode"].ToString());
        }
        internal override bool ValidInputs()
        {
            if (Inputs["ImageMat"] == null) throw new ArgumentNullException("ImageMat");
            if (Inputs["ImageMat"]!.GetType() != typeof(Mat)) throw new ArgumentException("ImageMat");

            ((Mat)Outputs["ImageMat"])?.Dispose();

            return true;
        }

        internal override void DIPFunction()
        {
            ColorConversionCodes colorConversionCode = (ColorConversionCodes)Parameters["ColorConversionCode"];

            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Mat outMat = new Mat();

                Cv2.CvtColor(inMat, outMat, colorConversionCode);
                Outputs["ImageMat"] = outMat;
                Inputs["ImageMat"] = inMat.Clone();
            }
        }

    }
}
