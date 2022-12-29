namespace Gguc.Aoc.Core.Extensions;

public static class RegexExtensions
{
    public static bool IsRegexMatch(this string input, string pattern)
    {
        return Regex.IsMatch(input, pattern);
    }

    /// <summary>
    /// Match regex and return value of defined group.
    /// Default group number is 1, as group 0 is always full pattern
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="groupNo"></param>
    /// <returns></returns>
    public static string RegexValue(this string input, string pattern, int groupNo = 1)
    {
        var match = Regex.Match(input, pattern);

        if (!match.Success || !match.Groups[groupNo].Success) return null;

        return match.Groups[groupNo].Value;
    }

    public static IEnumerable<string> MatchAll(this string input, string pattern)
    {
        var match = Regex.Match(input, pattern);

        if (!match.Success) return null;

        return match.Groups.Values.Select(x => x.Value);
    }

    public static string GroupValue(this string groupName, Match match)
    {
        if (!match.Success || !match.Groups[groupName].Success) return null;

        return match.Groups[groupName].Value;
    }

    public static string GroupValue(this Match match, string groupName)
    {
        if (!match.Success || !match.Groups[groupName].Success) return null;

        return match.Groups[groupName].Value;
    }
}
