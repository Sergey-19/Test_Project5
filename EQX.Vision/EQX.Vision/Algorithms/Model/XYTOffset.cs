namespace EQX.Vision.Algorithms.Model
{
    public class XYTOffset
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Theta { get; set; }

        public override string ToString()
        {
            return $"dX={X:0.###},dY={Y:0.###},dT={Theta:0.####}";
        }
    }
}
