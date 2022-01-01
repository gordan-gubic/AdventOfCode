namespace Gguc.Aoc.Core.Extensions;

/// <summary>
/// Extension methods for string
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>Returns true, if string value is null or whitespace.</returns>
    public static bool IsWhitespace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>Returns true, if string value is not null or whitespace.</returns>
    public static bool IsNotWhitespace(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public static string ReverseString(this string value)
    {
        return string.Join("", value.Reverse());
    }

    public static string JoinToString(this IEnumerable<char> values, string separator = "")
    {
        return string.Join(separator, values);
    }

    public static List<int> ToIntSequence(this string input, char separator = ',')
    {
        var list = new List<int>();
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries).ForEach(x => list.Add(x.ToInt()));
        return list;
    }

    public static List<long> ToSequence(this string input, char separator = ',')
    {
        var list = new List<long>();
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries).ForEach(x => list.Add(x.ToLong()));
        return list;
    }
}
