namespace Gguc.Aoc.Core.Extensions;

public static class PointsExtensions
{
    public static long ManhattanDistance(this Point dot)
    {
        return Math.Abs(dot.X) + Math.Abs(dot.Y);
    }

    public static long ManhattanDistance(this (int x, int y) point, int x, int y)
    {
        return Math.Abs(point.x - x) + Math.Abs(point.y - y);
    }

    public static long ManhattanDistance(this (int x, int y) point1, Point point2)
    {
        return Math.Abs(point1.x - point2.X) + Math.Abs(point1.y - point2.Y);
    }

    public static long ManhattanDistance(this Point point1, Point point2)
    {
        return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
    }

    public static long ManhattanDistance(this Point3d point1, Point3d point2)
    {
        return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y) + Math.Abs(point1.Z - point2.Z);
    }

    public static long ManhattanDistance(this PointLong point1, PointLong point2)
    {
        return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
    }
}
