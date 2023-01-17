namespace Gguc.Aoc.Core.Extensions;

public static class BinaryExtensions
{

    public static string ToBinaryString(this int value)
    {
        return Convert.ToString(value, 2);
    }

    public static string ToBinaryString(this long value)
    {
        return Convert.ToString(value, 2);
    }

    public static int FromBinaryStringToInt(this string value)
    {
        return Convert.ToInt32(value, 2);
    }

    public static long FromBinaryStringToLong(this string value)
    {
        return Convert.ToInt64(value, 2);
    }

    public static string InverseBinaryString_01(this string input)
    {
        return new string(input.Select(InverseBinaryChar).ToArray());
    }

    public static string InverseBinaryString(this string input)
    {
        return input.Replace("1", "*").Replace("0", "1").Replace("*", "0");
    }

    public static char InverseBinaryChar(this char input)
    {
        return input == '0' ? '1' : '0';
    }

    public static List<bool> ToBinaryList(this string input, char trueValue = '1')
    {
        var array = input.ToBitArray(trueValue);

        var list = new List<bool>();
        foreach (var item in array) list.Add((bool)item);

        return list;
    }
}
