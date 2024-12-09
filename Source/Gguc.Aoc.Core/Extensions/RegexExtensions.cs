namespace Gguc.Aoc.Core.Extensions;

public static class RegexExtensions
{
    public static bool IsRegexMatch(this string input, string pattern)
    {
        return Regex.IsMatch(input, pattern);
    }

    public static Match RegexMatch(this string input, string pattern)
    {
        return Regex.Match(input, pattern);
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

    public static string RegexValue(this string input, string pattern, string groupName)
    {
        var match = Regex.Match(input, pattern);

        if (!match.Success || !match.Groups[groupName].Success) return null;

        return match.Groups[groupName].Value;
    }

    public static IEnumerable<string> MatchAll(this string input, string pattern, RegexOptions options = RegexOptions.None)
    {
        var match = Regex.Match(input, pattern, options);

        if (!match.Success) return null;

        return match.Groups.Values.Select(x => x.Value);
    }

    public static IEnumerable<string> Matches(this string input, string pattern, RegexOptions options = RegexOptions.None)
    {
        var matches = Regex.Matches(input, pattern, options);

        if (matches.Count == 0) return null;

        matches.ToList();

        return matches.ToList().Select(x => x.Value);
    }

    public static string GroupValue(this string groupName, Match match)
    {
        if (!match.Success || !match.Groups[groupName].Success) return null;

        return match.Groups[groupName].Value;
    }

    public static string GroupValue(this Match match, int groupNo = 1)
    {
        if (!match.Success || !match.Groups[groupNo].Success) return null;

        return match.Groups[groupNo].Value;
    }

    public static string GroupValue(this Match match, string groupName)
    {
        if (!match.Success || !match.Groups[groupName].Success) return null;

        return match.Groups[groupName].Value;
    }
}
