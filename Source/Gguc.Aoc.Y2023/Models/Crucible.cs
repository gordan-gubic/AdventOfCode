namespace Gguc.Aoc.Y2023.Models;

internal record Crucible
{
    public Point Point { get; set; } 

    public long Sum { get; set; }

    public char Dir { get; set; }

    public int Count { get; set; }
}