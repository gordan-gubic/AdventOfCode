namespace Gguc.Aoc.Core.Extensions;

public static class PointsExtensions
{
    private  static readonly Dictionary<int, (int, int)> Directions = new()
    {
          [0] = (  0, -1),
         [45] = ( +1, -1),
         [90] = ( +1,  0),
        [135] = ( +1, +1),
        [180] = (  0, +1),
        [225] = ( -1, +1),
        [270] = ( -1,  0),
        [315] = ( -1, -1),
    };

    private static readonly Dictionary<char, (int, int)> Signs = new()
    {
        ['^'] = (0, -1),
        ['>'] = (+1, 0),
        ['v'] = (0, +1),
        ['V'] = (0, +1),
        ['<'] = (-1, 0),
    };

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

    public static (int, int) DegreeToDirection(this int degree)
    {
        if(!Directions.ContainsKey(degree)) return (0, 0);

        return Directions[degree];
    }

    public static Point DegreeToPoint(this Point point, int degree)
    {
        if (!Directions.ContainsKey(degree)) return point;

        var dir = Directions[degree];
        return new Point(point.X + dir.Item1, point.Y + dir.Item2);
    }

    public static (int, int) SignToDirection(this char sign)
    {
        if (!Signs.ContainsKey(sign)) return (0, 0);

        return Signs[sign];
    }

    public static Point SignToPoint(this Point point, char sign)
    {
        if (!Signs.ContainsKey(sign)) return point;

        var dir = Signs[sign];
        return new Point(point.X + dir.Item1, point.Y + dir.Item2);
    }
}
