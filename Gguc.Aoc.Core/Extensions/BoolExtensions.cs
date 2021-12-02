namespace Gguc.Aoc.Core.Extensions;

public static class BoolExtensions
{
    public static bool IsBetweenX(this int input, int min, int max)
    {
        return input >= min && input <= max;
    }

    public static bool IsBetween<T>(this T @this, T aInclusive, T bInclusive) where T : IComparable<T>
    {
        return @this.CompareTo(aInclusive) <= 0 && @this.CompareTo(bInclusive) >= 0
               || @this.CompareTo(aInclusive) >= 0 && @this.CompareTo(bInclusive) <= 0;
    }
}
