namespace Gguc.Aoc.Y2024.Models;

internal record Field
{
    public string Id { get; set; }

    public HashSet<Point> Plots { get; set; } = new();

    public HashSet<Point> Borders { get; set; } = new();
}