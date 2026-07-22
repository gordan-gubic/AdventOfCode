namespace Gguc.Aoc.Y2015.Models;

internal record LightIns
{
    public int Operation { get; set; }

    public Point From { get; set; }

    public Point To { get; set; }
}