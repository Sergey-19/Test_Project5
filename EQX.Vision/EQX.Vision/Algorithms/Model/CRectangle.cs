using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace EQX.Vision.Algorithms
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CRectangle : ObservableObject
    {
        private int _width = 300;
        private int _height = 200;
        private int _X = 100;
        private int _Y = 50;

        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }
        public int X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
                OnPropertyChanged();
            }
        }
        public int Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
                OnPropertyChanged();
            }
        }
    }
}
