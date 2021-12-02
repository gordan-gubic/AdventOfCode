namespace Gguc.Aoc.Core.Extensions;

public static class ConvertExtensions
{
    public static int ToInt(this bool input, int trueValue = 1, int falseValue = 0)
    {
        return (input) ? trueValue : falseValue;
    }

    public static int ToInt(this string input, int defaultValue = default)
    {
        if (int.TryParse(input, out var result)) return result;

        return defaultValue;
    }

    public static long ToLong(this string input, int defaultValue = default)
    {
        if (long.TryParse(input, out var result)) return result;

        return defaultValue;
    }

    public static bool ToBool(this string input, int defaultValue = default)
    {
        return input.IsNotWhitespace() && input != "0";
    }

    public static bool ToBool(this char input, int defaultValue = default)
    {
        return input != '0';
    }
}
