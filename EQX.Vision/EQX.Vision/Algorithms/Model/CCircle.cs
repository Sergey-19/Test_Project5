using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace EQX.Vision.Algorithms
{
    public class CCircle : ObservableObject
    {
        #region Privates
        private double _centerX = 400;
        private double _centerY = 400;
        private double _radius = 200;
        #endregion

        #region Properties
        public double CenterX
        {
            get
            {
                return _centerX;
            }
            set
            {
                _centerX = value;
                OnPropertyChanged(nameof(CenterX));
            }
        }
        public double CenterY
        {
            get
            {
                return _centerY;
            }
            set
            {
                _centerY = value;
                OnPropertyChanged(nameof(CenterY));
            }
        }
        public double Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
                OnPropertyChanged(nameof(Radius));
            }
        }
        #endregion

        #region Constructor
        public CCircle(double centerX, double centerY, double radius)
        {
            CenterX = (int)centerX;
            CenterY = (int)centerY;
            Radius = (int)radius;
        }

        [JsonConstructor]
        public CCircle()
        {

        }
        #endregion
    }
}
