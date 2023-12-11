namespace Gguc.Aoc.Core.Models;

public readonly record struct PointLong(long X, long Y)
{
    public override string ToString() => $"({X}, {Y})";
}