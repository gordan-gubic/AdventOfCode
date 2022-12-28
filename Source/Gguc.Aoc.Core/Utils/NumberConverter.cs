namespace Gguc.Aoc.Core.Utils;

public static class NumberConverter
{
    public static long ConvertFromBase(string value, string chars, int offset = 0)
    {
        var nbase = chars.Length;

        value = value.ReverseString();
        var length = value.Length;
        var sum = 0L;

        for (var i = 0; i < length; i++)
        {
            sum += (chars.IndexOf(value[i]) - offset) * (long)Math.Pow(nbase, i);
        }

        return sum;
    }

    public static string ConvertToBase(long number, int nbase)
    {
        var result = "";

        while (number > 0)
        {
            var remainder = number % nbase;
            number /= nbase;

            result = $"{remainder}{result}";
        }

        return result;
    }

    public static string ConvertToBase(long number, string chars)
    {
        var result = "";
        var nbase = chars.Length;

        while (number > 0)
        {
            var remainder = number % nbase;
            number /= nbase;

            var ch = chars[(int)remainder];
            result = $"{remainder}{result}";
        }

        return result;
    }

    public static string ConvertToSnafu(long number)
    {
        var chars = "=-012";
        var result = "";
        var offset = 2;

        while (number > 0)
        {
            var remainder = number % 5;
            number /= 5;

            if (remainder.IsBetween(0, 2))
            {
                result = $"{remainder}{result}";
            }
            else if (remainder > 2)
            {
                var ch = chars[(int)remainder - offset - 1];
                result = $"{ch}{result}";
                number++;
            }
        }

        return result;
    }
}
