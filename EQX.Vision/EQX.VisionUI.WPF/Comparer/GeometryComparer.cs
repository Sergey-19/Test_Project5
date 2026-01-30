using System.Windows.Media;

namespace EQX.VisionUI.WPF.Comparer
{
    public class GeometryComparer : IEqualityComparer<Geometry>
    {
        public bool Equals(Geometry x, Geometry y)
        {
            if (x == null || y == null)
                return x == y;

            return x.Bounds.BottomLeft == y.Bounds.BottomLeft &
                x.Bounds.BottomRight == y.Bounds.BottomRight &
                x.Bounds.TopLeft == y.Bounds.TopLeft &
                x.Bounds.TopRight == y.Bounds.TopRight &
                x.Bounds.X == y.Bounds.X &
                x.Bounds.Y == y.Bounds.Y &
                x.Bounds.Location == y.Bounds.Location;
        }

        public int GetHashCode(Geometry obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
