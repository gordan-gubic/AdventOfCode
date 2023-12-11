namespace Gguc.Aoc.Y2023.Models;

internal record Signal
{
    public string From { get; set; } 

    public string To { get; set; }

    public bool Value { get; set; }
}