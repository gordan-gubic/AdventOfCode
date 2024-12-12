namespace Gguc.Aoc.Y2024.Models;

internal record Trail
{
    public Point Begin { get; set; }

    public Point Current { get; set; }

    public List<Point> Path { get; set; } = new();

    public HashSet<Point> Points { get; set; } = new();

    public int Value { get; set; }

    public override string ToString() => $"[{new {Current, Value, Path=Path.ToJson(), Begin }}]";
}