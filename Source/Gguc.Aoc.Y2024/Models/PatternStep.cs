namespace Gguc.Aoc.Y2024.Models;

public record PatternStep
{
    public string Pattern { get; set; }

    public string Path { get; set; }

    public string Path2 { get; set; }

    public string Remain { get; set; }

    public long Value { get; set; }

    public override string ToString() => $"[{new { Pattern, Path, Path2, Value }}]";
}