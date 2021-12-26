namespace Gguc.Aoc.Core.Models;

public record struct Point3d
{
    public Point3d(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public int X { get; private set; }

    public int Y { get; private set; }

    public int Z { get; private set; }

    public override string ToString() => $"({X},{Y},{Z})";

    public static Point3d Create(string text)
    {
        var parts = text.Split(',');

        return new Point3d
        {
            X = parts[0].ToInt(),
            Y = parts[1].ToInt(),
            Z = parts[2].ToInt(),
        };
    }

    public static Point3d Create(Point3d point, List<string> axes)
    {
        return new Point3d
        {
            X = Take(point, axes[0]),
            Y = Take(point, axes[1]),
            Z = Take(point, axes[2]),
        };
    }

    private static int Take(Point3d point, string axis)
    {
        var factor = (axis[0] == '-') ? -1 : 1;

        int i = axis[1] switch
        {
            'X' => point.X,
            'Y' => point.Y,
            'Z' => point.Z,
            _ => -99999,
        };

        return i * factor;
    }
}
