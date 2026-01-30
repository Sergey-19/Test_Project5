using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.FlowUILibraryWPF.MVVM.ViewModels;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EQX.FlowUILibraryWPF.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ConnectionData : ObservableObject
    {
        [JsonIgnore]
        public EventHandler DeletePathEvent;

        #region Properties
        public ToolViewModel OriginTool
        {
            get
            {
                return _originTool;
            }
            set
            {
                _originTool = value;
                OnPropertyChanged();
            }
        }
        public ToolViewModel TargetTool
        {
            get
            {
                return _targetTool;
            }
            set
            {
                _targetTool = value;
                OnPropertyChanged();
            }
        }
        public string KeyOutputOriginTool
        {
            get
            {
                return _keyOutputOriginTool;
            }
            set
            {
                _keyOutputOriginTool = value;
                OnPropertyChanged();
            }
        }
        public string KeyInputTargetTool
        {
            get
            {
                return _keyInputTargetTool;
            }
            set
            {
                _keyInputTargetTool = value;
                OnPropertyChanged();
            }
        }
        public Point StartPoint
        {
            get
            {
                int index = OriginTool.VisionTool.Outputs.Keys.ToList().IndexOf(KeyOutputOriginTool) + 1;
                return OriginTool.GetCenterOutputPoint(index);
            }
        }
        public Point EndPoint
        {
            get
            {
                int index = TargetTool.VisionTool.Inputs.Keys.ToList().IndexOf(KeyInputTargetTool) + 1;
                return TargetTool.GetCenterInputPoint(index);
            }
        }
        [JsonIgnore]
        public Geometry GeometryPathData
        {
            get
            {
                return _geometryPathData;
            }
            set
            {
                _geometryPathData = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        [JsonIgnore]
        public ICommand DeletePathCommand { get;set; }
        #endregion

        #region Privates
        private ToolViewModel _originTool;
        private ToolViewModel _targetTool;
        private string _keyOutputOriginTool;
        private string _keyInputTargetTool;
        private Geometry _geometryPathData;
        #endregion

        #region Private Methods
        public void GenerateLinkPath(double sc)
        {
            string svgPathString;
            int indexKeyInputTail = (int)(TargetTool.VisionTool.Inputs.Keys!.ToList().IndexOf(KeyInputTargetTool) + 1);
            int indexKeyOutputHead = (int)(OriginTool.VisionTool.Outputs.Keys!.ToList().IndexOf(KeyOutputOriginTool) + 1);

            double origX = OriginTool.GetCenterOutputPoint(indexKeyOutputHead).X;
            double origY = OriginTool.GetCenterOutputPoint(indexKeyOutputHead).Y;

            double destX = TargetTool.GetCenterInputPoint(indexKeyInputTail).X;
            double destY = TargetTool.GetCenterInputPoint(indexKeyInputTail).Y;

            double dy = destY - origY;
            double dx = destX - origX;
            double delta = Math.Sqrt(dy * dy + dx * dx);
            double scale = 0.75; // Default scale
            double scaleY = 0;

            if (dx * sc > 0)
            {
                if (delta < 200)
                {
                    scale = 0.75 - 0.75 * ((200 - delta) / 200);
                }
            }
            else
            {
                scale = 0.4 - 0.2 * (Math.Max(0, (200 - Math.Min(Math.Abs(dx), Math.Abs(dy))) / 200));
            }
            if (dx * sc > 0)
            {
                svgPathString = "M " + origX + " " + origY +
                " C " + (origX + sc * (200 * scale)) + " " + (origY + scaleY * 50) + " " +
                (destX - sc * (scale) * 200) + " " + (destY - scaleY * 50) + " " +
                destX + " " + destY;
                svgPathString = svgPathString.Replace(",", ".");
                Geometry pathGeometry = Geometry.Parse(svgPathString);
                GeometryPathData = pathGeometry;
            }
            else
            {
                double midX = Math.Floor(destX - dx / 2);
                double midY = Math.Floor(destY - dy / 2);
                if (dy == 0)
                {
                    midY = destY + 50;
                }
                double cp_height = 25; //node_height/2
                double y1 = (destY + midY) / 2;

                double topX = origX + sc * 200 * scale; //node_width = 200
                double topY = dy > 0 ? Math.Max(y1 - dy / 2, origY + cp_height) : Math.Max(y1 - dy / 2, origY - cp_height);

                double bottomX = destX - sc * 200 * scale;
                double bottomY = dy > 0 ? Math.Max(y1, destY - cp_height) : Math.Min(y1, destY + cp_height);

                double x1 = (origX + topX) / 2;
                double scy = dy > 0 ? 1 : -1;

                double[,] cp = new double[5, 2];
                cp[0, 0] = x1;
                cp[0, 1] = origY;

                cp[1, 0] = topX;
                cp[1, 1] = (dy > 0) ? Math.Max(origY, topY - cp_height) : Math.Min(origY, topY + cp_height);

                cp[2, 0] = x1;
                cp[2, 1] = (dy > 0) ? Math.Min(midY, topY + cp_height) : Math.Max(midY, topY - cp_height);

                cp[3, 0] = bottomX;
                cp[3, 1] = (dy > 0) ? Math.Max(midY, bottomY - cp_height) : Math.Min(midY, bottomY + cp_height);

                cp[4, 0] = (destX + bottomX) / 2;
                cp[4, 1] = destY;

                if (cp[2, 1] == topY + scy * cp_height)
                {
                    if (Math.Abs(dy) < cp_height * 10)
                    {
                        cp[1, 1] = topY - scy * cp_height / 2;
                        cp[3, 1] = bottomY - scy * cp_height / 2;
                    }
                    cp[2, 0] = topX;
                }
                // Create a PathFigure
                // Create a PathGeometry and add the PathFigure to it

                // Create a Path and set its Data to the PathGeometry

                svgPathString = "M " + origX + " " + origY +
                    " C " +
                       cp[0, 0] + " " + cp[0, 1] + " " +
                       cp[1, 0] + " " + cp[1, 1] + " " +
                       topX + " " + topY +
                    " S " +
                       cp[2, 0] + " " + cp[2, 1] + " " +
                       midX + " " + midY +
                   " S " +
                      cp[3, 0] + " " + cp[3, 1] + " " +
                      bottomX + " " + bottomY +
                    " S " +
                        cp[4, 0] + " " + cp[4, 1] + " " +
                        destX + " " + destY;

                svgPathString = svgPathString.Replace(",", ".");
                Geometry pathGeometry = Geometry.Parse(svgPathString);
                GeometryPathData = pathGeometry;
            }
        }
        public void GenerateLinkPath(Point startPoint, Point destPoint, double sc)
        {
            Geometry result;
            string svgPathString = "";
            double origX = startPoint.X;
            double origY = startPoint.Y;

            double destX = destPoint.X;
            double destY = destPoint.Y;

            double dy = destY - origY;
            double dx = destX - origX;
            double delta = Math.Sqrt(dy * dy + dx * dx);
            double scale = 0.75; // Default scale
            double scaleY = 0;
            // ... (rest of your logic)
            if (dx * sc > 0)
            {
                if (delta < 200)
                {
                    scale = 0.75 - 0.75 * ((200 - delta) / 200);
                }
            }
            else
            {
                scale = 0.4 - 0.2 * (Math.Max(0, (200 - Math.Min(Math.Abs(dx), Math.Abs(dy))) / 200));
            }
            if (dx * sc > 0)
            {
                svgPathString = "M " + origX + " " + origY +
                " C " + (origX + sc * (200 * scale)) + " " + (origY + scaleY * 50) + " " +
                (destX - sc * (scale) * 200) + " " + (destY - scaleY * 50) + " " +
                destX + " " + destY;
                svgPathString = svgPathString.Replace(",", ".");
                result = Geometry.Parse(svgPathString);
                GeometryPathData = result;
                return;
            }
            else
            {
                double midX = Math.Floor(destX - dx / 2);
                double midY = Math.Floor(destY - dy / 2);
                if (dy == 0)
                {
                    midY = destY + 50;
                }
                double cp_height = 25; //node_height/2
                double y1 = (destY + midY) / 2;

                double topX = origX + sc * 200 * scale; //node_width = 200
                double topY = dy > 0 ? Math.Max(y1 - dy / 2, origY + cp_height) : Math.Max(y1 - dy / 2, origY - cp_height);

                double bottomX = destX - sc * 200 * scale;
                double bottomY = dy > 0 ? Math.Max(y1, destY - cp_height) : Math.Min(y1, destY + cp_height);

                double x1 = (origX + topX) / 2;
                double scy = dy > 0 ? 1 : -1;

                double[,] cp = new double[5, 2];
                cp[0, 0] = x1;
                cp[0, 1] = origY;

                cp[1, 0] = topX;
                cp[1, 1] = (dy > 0) ? Math.Max(origY, topY - cp_height) : Math.Min(origY, topY + cp_height);

                cp[2, 0] = x1;
                cp[2, 1] = (dy > 0) ? Math.Min(midY, topY + cp_height) : Math.Max(midY, topY - cp_height);

                cp[3, 0] = bottomX;
                cp[3, 1] = (dy > 0) ? Math.Max(midY, bottomY - cp_height) : Math.Min(midY, bottomY + cp_height);

                cp[4, 0] = (destX + bottomX) / 2;
                cp[4, 1] = destY;

                if (cp[2, 1] == topY + scy * cp_height)
                {
                    if (Math.Abs(dy) < cp_height * 10)
                    {
                        cp[1, 1] = topY - scy * cp_height / 2;
                        cp[3, 1] = bottomY - scy * cp_height / 2;
                    }
                    cp[2, 0] = topX;
                }
                // Create a PathFigure
                // Create a PathGeometry and add the PathFigure to it

                // Create a Path and set its Data to the PathGeometry

                svgPathString = "M " + origX + " " + origY +
                    " C " +
                       cp[0, 0] + " " + cp[0, 1] + " " +
                       cp[1, 0] + " " + cp[1, 1] + " " +
                       topX + " " + topY +
                    " S " +
                       cp[2, 0] + " " + cp[2, 1] + " " +
                       midX + " " + midY +
                   " S " +
                      cp[3, 0] + " " + cp[3, 1] + " " +
                      bottomX + " " + bottomY +
                    " S " +
                        cp[4, 0] + " " + cp[4, 1] + " " +
                        destX + " " + destY;

                svgPathString = svgPathString.Replace(",", ".");
                result = Geometry.Parse(svgPathString);
                GeometryPathData = result;
            }
        }
        #endregion

        #region Constructors
        public ConnectionData()
        {
            DeletePathCommand = new RelayCommand(() =>
            {
                DeletePathEvent?.Invoke(this, EventArgs.Empty);
            });
        }

        [Newtonsoft.Json.JsonConstructor]
        public ConnectionData(ToolViewModel originTool, ToolViewModel targetTool, string keyOutputOriginTool, string keyInputTargetTool)
        {
            OriginTool = originTool;
            TargetTool = targetTool;
            KeyInputTargetTool = keyInputTargetTool;
            KeyOutputOriginTool = keyOutputOriginTool;
            GenerateLinkPath(1);
            DeletePathCommand = new RelayCommand(() =>
            {
                DeletePathEvent?.Invoke(this, EventArgs.Empty);
            });
        }
        #endregion

        public ConnectionData Clone()
        {
            try
            {
                ConnectionData connectionClone = new ConnectionData();
                connectionClone.OriginTool = OriginTool;
                connectionClone.TargetTool = TargetTool;
                connectionClone.KeyInputTargetTool = KeyInputTargetTool;
                connectionClone.KeyOutputOriginTool = KeyOutputOriginTool;
                connectionClone.GeometryPathData = GeometryPathData;
                connectionClone.DeletePathEvent = DeletePathEvent;

                return connectionClone;
            }
            catch (Exception e) 
            {

            }
            return null;
        }
        public void Dispose()
        {
            OriginTool = null;
            TargetTool = null;
            GeometryPathData = null;
        }
    }
}
