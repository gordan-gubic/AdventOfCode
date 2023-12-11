namespace Gguc.Aoc.Y2023.Models;

internal record Machine
{
    public int X { get; set; } 

    public int M { get; set; }

    public int A { get; set; }

    public int S { get; set; }

    public long Value => X + M + A + S;
}