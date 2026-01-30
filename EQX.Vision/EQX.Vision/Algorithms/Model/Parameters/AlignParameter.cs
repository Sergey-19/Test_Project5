using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.Vision.Algorithms.Model.Parameters
{
    public class AlignParameter : ObservableObject
    {
        private double _offsetX;

        public double OffsetX
        {
            get { return _offsetX; }
            set { _offsetX = value; OnPropertyChanged(); }
        }

        private double _offsetY;

        public double OffsetY
        {
            get { return _offsetY; }
            set { _offsetY = value; OnPropertyChanged(); }
        }

        private double _offsetTheta;

        public double OffsetTheta
        {
            get { return _offsetTheta; }
            set { _offsetTheta = value; OnPropertyChanged(); }
        }

    }
}
