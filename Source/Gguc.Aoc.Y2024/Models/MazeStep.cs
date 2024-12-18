namespace Gguc.Aoc.Y2024.Models;

public record MazeStep
{
    public Point Point { get; set; }

    public List<Point> Path { get; set; } = new();

    public HashSet<Point> Points { get; set; } = new();

    public int Dir { get; set; }

    public long Value { get; set; }

    public override string ToString() => $"[{new { Point, Dir, Value }}]";
}