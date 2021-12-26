namespace Gguc.Aoc.Y2021.Models;

public record DistanceCompareResult
{
    public bool IsFound { get; set; }

    public bool IsReversed { get; set; }

    public string Key1 { get; set; }

    public string Key2 { get; set; }

    public List<string> Axes { get; set; }
}
