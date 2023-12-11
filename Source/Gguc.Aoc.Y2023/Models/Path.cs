namespace Gguc.Aoc.Y2023.Models;

internal class Path
{
    public Point Point { get; set; }

    public List<Point> Points { get; set; } = new();

    public Point Start => Points.First();

    public Point End => Points.Last();

    public int Distance => Points.Count;

    public void Add(Point point)
    {
        Point = point;
        Points.Add(point);
    }
}