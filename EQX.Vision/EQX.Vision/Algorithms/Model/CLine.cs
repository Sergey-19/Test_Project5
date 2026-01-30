using CommunityToolkit.Mvvm.ComponentModel;

namespace EQX.Vision.Algorithms
{
    public class CLine : ObservableObject
    {
        private int _X1 = 50;
        private int _Y1 = 50;

        private int _X2 = 50;
        private int _Y2 = 200;

        public int X1
        {
            get { return _X1; }
            set
            {
                _X1 = value;
                OnPropertyChanged();
            }
        }
        public int Y1
        {
            get { return _Y1; }
            set
            {
                _Y1 = value;
                OnPropertyChanged();
            }
        }

        public int X2
        {
            get { return _X2; }
            set
            {
                _X2 = value;
                OnPropertyChanged();
            }
        }
        public int Y2
        {
            get { return _Y2; }
            set
            {
                _Y2 = value;
                OnPropertyChanged();
            }
        }

    }
}
