namespace Gguc.Aoc.Y2015.Models;

internal record Box
{
    public Guid Id { get; } = Guid.NewGuid();

    public Point Point1 { get; set; }

    public Point Point2 { get; set; }
}