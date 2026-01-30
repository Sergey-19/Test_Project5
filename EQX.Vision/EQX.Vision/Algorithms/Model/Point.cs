using CommunityToolkit.Mvvm.ComponentModel;

namespace EQX.Vision.Algorithms
{
    public class Point : ObservableObject
    {
        private double x;
        private double y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X
        {
            get => x;
            set
            {
                x = value;
                OnPropertyChanged(nameof(X));
            }
        }
        public double Y
        {
            get => y;
            set
            {
                y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
    }
}
