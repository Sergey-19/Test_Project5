using EQX.Core.Vision.Algorithms;
using Newtonsoft.Json;
using OpenCvSharp;

namespace EQX.Vision.Algorithms
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TemplateMatchingTool : VisionToolBase
    {
        #region Privates
        private bool _isSettingTemplate = true;
        private Mat _templateMat;
        private double _matchScore;
        #endregion

        #region Properties
        [JsonProperty]
        public CRectangle RectangleTemplate { get; set; } = new CRectangle();
        public bool IsSettingTemplate
        {
            get
            {
                return _isSettingTemplate;
            }
            set
            {
                _isSettingTemplate = value;
                OnPropertyChanged(nameof(IsVisibleRectangleTemplate));
            }
        }
        public bool IsVisibleRectangleTemplate => IsSettingTemplate & ((Inputs["ImageMat"] as Mat) != null);
        public Mat TemplateMat
        {
            get
            {
                return _templateMat;
            }
            set
            {
                _templateMat = value;
                OnPropertyChanged();
            }
        }
        public double MatchScore
        {
            get
            {
                return _matchScore;
            }
            set
            {
                _matchScore = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public TemplateMatchingTool()
            : base()
        {
            Parameters = new ObjectCollection(new List<KeyType> {
                new KeyType("IsUseDetectTemplateRotation", typeof(Mat)),
                new KeyType("AngleStart", typeof(double)),
                new KeyType("AngleEnd", typeof(double)),
                new KeyType("AngleStep", typeof(double)),
            });
            Inputs = new ObjectCollection(new List<KeyType> {
                new KeyType("ImageMat", typeof(Mat)),
            });
            Outputs = new ObjectCollection(new List<KeyType> {
                new KeyType("LocationX", typeof(int)),
                new KeyType("LocationY", typeof(int)),
                new KeyType("ImageMat", typeof(Mat)),
            });

            Parameters["IsUseDetectTemplateRotation"] = true;
            Parameters["AngleStart"] = 0;
            Parameters["AngleEnd"] = 360;
            Parameters["AngleStep"] = 1;
        }
        #endregion

        #region Overrides
        internal override bool ValidInputs()
        {
            if (Inputs["ImageMat"] == null) throw new ArgumentNullException("ImageMat");
            if (Inputs["ImageMat"]!.GetType() != typeof(Mat)) throw new ArgumentException("ImageMat");

            ((Mat)Outputs["ImageMat"])?.Dispose();

            return true;
        }
        internal override void DIPFunction()
        {
            if (TemplateMat == null) return;
            using (Mat inMat = (Mat)Inputs["ImageMat"]!)
            {
                Mat outMat = inMat.Clone();
                Cv2.CvtColor(outMat, outMat, ColorConversionCodes.GRAY2BGR);
                Mat result = new Mat();

                double minVal, maxVal;
                OpenCvSharp.Point minLoc, maxLoc;

                if ((bool)Parameters["IsUseDetectTemplateRotation"])
                {
                    OpenCvSharp.Point bestMatchLoc = new OpenCvSharp.Point();
                    double bestMatchVal = 0;
                    double bestAngle = 0;

                    int angleStart = Convert.ToInt32(Parameters["AngleStart"]);
                    int angleEnd = Convert.ToInt32(Parameters["AngleEnd"]);
                    int angleStep = Convert.ToInt32(Parameters["AngleStep"]);
                    for (double angle = angleStart; angle <= angleEnd; angle += angleStep)
                    {
                        Mat rotatedTemplate = MatHelper.RotateImage(TemplateMat, angle);

                        Cv2.MatchTemplate(inMat, rotatedTemplate, result, TemplateMatchModes.CCoeffNormed);

                        Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                        if (maxVal > bestMatchVal)
                        {
                            bestMatchVal = maxVal;
                            bestMatchLoc = maxLoc;
                            bestAngle = angle;
                        }
                    }
                    MatchScore = bestMatchVal;
                    MatHelper.DrawRotatedRectangle(outMat, bestMatchLoc, TemplateMat.Size(), bestAngle);
                    Outputs["LocationX"] = bestMatchLoc.X;
                    Outputs["LocationY"] = bestMatchLoc.Y;
                }
                else
                {
                    Cv2.MatchTemplate(inMat, TemplateMat, result, TemplateMatchModes.CCoeffNormed);
                    Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);
                    Rect rect = new Rect(maxLoc.X, maxLoc.Y, TemplateMat.Width, TemplateMat.Height);
                    Cv2.Rectangle(outMat, rect, Scalar.Red, 2);
                    Outputs["LocationX"] = maxLoc.X;
                    Outputs["LocationY"] = maxLoc.Y;
                    MatchScore = maxVal;
                }

                Inputs["ImageMat"] = inMat.Clone();
                Outputs["ImageMat"] = outMat;
            }
        }
        #endregion
    }
}
