namespace Gguc.Aoc.Y2023.Models;

internal class ForestPath_01
{
    public Point Point { get; set; }

    public Path Path { get; set; } = new();

    public List<Path> Paths { get; set; } = new();

    public int X => Point.X;

    public int Y => Point.Y;

    public int Distance => Path.Distance;

    public int TotalDistance { get; set; }

    public void Add(Point point)
    {
        Point = point;
        Path.Add(point);
    }

    public void AddPath(Path path)
    {
        Path = path;
        Paths.Add(path);
    }
}