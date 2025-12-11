namespace Gguc.Aoc.Core.Models;

using System.Numerics;

public readonly record struct Point(int X, int Y)
{
    public override string ToString() => $"({X}, {Y})";
}


public readonly record struct Point<T>(T X, T Y) where T : INumber<T>
{
    public override string ToString() => $"({X}, {Y})";
}