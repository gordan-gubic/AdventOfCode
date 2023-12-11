namespace Gguc.Aoc.Y2023.Models;

internal class ForestPath
{
    public Point Point { get; set; }

    public HashSet<Point> Points { get; set; } = new();

    public int X => Point.X;

    public int Y => Point.Y;

    public long TotalDistance { get; set; }

    public void Add(Point point, long distance)
    {
        Point = point;
        Points.Add(point);
        TotalDistance += distance;
    }

    public ForestPath Copy()
    {
        return new ForestPath { Point = Point, Points = Points.ToHashSet(), TotalDistance = TotalDistance };
    }
}