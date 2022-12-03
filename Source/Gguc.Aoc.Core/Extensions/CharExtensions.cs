namespace Gguc.Aoc.Core.Extensions;

/// <summary>
/// Extension methods for char
/// </summary>
public static class CharExtensions
{
    public static bool IsUpper(this char value)
    {
        return char.IsUpper(value);
    }

    public static bool IsLower(this char value)
    {
        return char.IsLower(value);
    }

    public static int ToPriorityValue(this char value)
    {
        return value - (value.IsLower() ? 96 : 38);
    }
}
