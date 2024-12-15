namespace Gguc.Aoc.Core.Extensions;

public static class DoubleExtensions
{
    public static bool IsRound(this double value, int digits = 4)
    {
        var f = Math.Pow(10, digits);

        return (long)(double.Round(value, digits) * f) == (long)value * f;
    }
}
