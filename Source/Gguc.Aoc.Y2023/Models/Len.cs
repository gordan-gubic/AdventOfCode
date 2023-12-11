namespace Gguc.Aoc.Y2023.Models;

internal record Len
{
    public string Id { get; set; } 

    public int Length { get; set; }

    public int Box { get; set; }

    public char Operation { get; set; }
}