namespace Gguc.Aoc.Core.Extensions;

public static class SolutionExtensions
{
    public static long Multiply(this List<long> segments, bool ignoreNull = true)
    {
        var result = 1L;

        foreach (var segment in segments)
        {
            if (ignoreNull && segment == 0)
            {
                continue;
            }

            result *= segment;
        }

        return result;
    }

    public static long AddTo(this int value, ref long result)
    {
        result += value;
        return result;
    }

    public static long AddTo(this long value, ref long result)
    {
        result += value;
        return result;
    }

    public static long AddTo(this bool value, ref long result)
    {
        result += value ? 1 : 0;
        return result;
    }

    public static long MultiplyTo(this int value, ref long result)
    {
        result *= value;
        return result;
    }

    public static long MultiplyTo(this long value, ref long result)
    {
        result *= value;
        return result;
    }
}
