namespace Gguc.Aoc.Core.Extensions;

public static class PointsExtensions
{
    public static long ManhattanDistance(this Point dot)
    {
        return Math.Abs(dot.X) + Math.Abs(dot.Y);
    }

    public static long ManhattanDistance(this Point point1, Point point2)
    {
        return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
    }

    public static long ManhattanDistance(this Point3d point1, Point3d point2)
    {
        return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y) + Math.Abs(point1.Z - point2.Z);
    }
}
