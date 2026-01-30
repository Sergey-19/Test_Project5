using System.Windows.Media;

namespace EQX.Vision.Algorithms
{
    public static class VisionFlowHelpers
    {
        public static Point GetOriginPoint(this VisionToolDescription visionToolDescription, int keyIndex)
        {
            if (visionToolDescription.NumberOfInputKeys <= visionToolDescription.NumberOfOutputKeys)
            {
                return new Point(visionToolDescription.Position.X + 200 + 7.5 - 2, visionToolDescription.Position.Y + 15 * keyIndex + 20 * (keyIndex - 1) + 15 / 2 + 30);
            }
            else
            {
                return new Point(visionToolDescription.Position.X + 200 + 7.5 - 2, (double)(visionToolDescription.Position.Y + (visionToolDescription.NumberOfInputKeys * 35 + 7 - visionToolDescription.NumberOfOutputKeys * 35) / 2.0 + 35.0 / 2 * keyIndex + 20 * (keyIndex - 1) + 30));
            }
        }

        public static Point GetTargetPoint(this VisionToolDescription visionToolDescription, int keyIndex)
        {
            if (visionToolDescription.NumberOfInputKeys >= visionToolDescription.NumberOfOutputKeys)
            {
                return new Point(visionToolDescription.Position.X - 7.5 + 2, visionToolDescription.Position.Y + 15 * keyIndex + 20 * (keyIndex - 1) + 15 / 2 + 30);
            }
            else
            {
                return new Point(visionToolDescription.Position.X - 7.5 + 2, (double)(visionToolDescription.Position.Y + (visionToolDescription.NumberOfOutputKeys * 35 + 7 - visionToolDescription.NumberOfInputKeys * 35) / 2.0 + 35.0 / 2 * keyIndex + 20 * (keyIndex - 1) + 30));
            }
        }

        public static Geometry CreateGeometryPath(Point startPoint, Point endPoint, double sc)
        {
            Geometry result;
            string svgPathString = "";
            double origX = startPoint.X;
            double origY = startPoint.Y;

            double destX = endPoint.X;
            double destY = endPoint.Y;

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
                scale = 0.4 - 0.2 * Math.Max(0, (200 - Math.Min(Math.Abs(dx), Math.Abs(dy))) / 200);
            }
            if (dx * sc > 0)
            {
                svgPathString = "M " + origX + " " + origY +
                " C " + (origX + sc * (200 * scale)) + " " + (origY + scaleY * 50) + " " +
                (destX - sc * scale * 200) + " " + (destY - scaleY * 50) + " " +
                destX + " " + destY;
                svgPathString = svgPathString.Replace(",", ".");
                result = Geometry.Parse(svgPathString);
                return result;
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
                cp[1, 1] = dy > 0 ? Math.Max(origY, topY - cp_height) : Math.Min(origY, topY + cp_height);

                cp[2, 0] = x1;
                cp[2, 1] = dy > 0 ? Math.Min(midY, topY + cp_height) : Math.Max(midY, topY - cp_height);

                cp[3, 0] = bottomX;
                cp[3, 1] = dy > 0 ? Math.Max(midY, bottomY - cp_height) : Math.Min(midY, bottomY + cp_height);

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
                return result;
            }
        }
    }
}
