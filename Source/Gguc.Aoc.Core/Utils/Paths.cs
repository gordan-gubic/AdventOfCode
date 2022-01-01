namespace Gguc.Aoc.Core.Utils;

public static class Paths
{
    //public static readonly string InputSourcePath = @"..\..\..\Gguc.Aoc.Resources\";
    public static readonly string InputSourcePath = @".";

    public static readonly string InputSourceTest = Path.Combine(InputSourcePath, "Test", "test.txt");

    public static string ZipFilePath(int year)
    {
        var yearpath = $"y{year:0000}.zip";

        return Path.Combine(InputSourcePath, $"Y{year}", yearpath);
    }

    public static string DayFilePath(int year, int day, bool example = false)
    {
        return Path.Combine(InputSourcePath, $"Y{year}", DayFileEntry(day, example));
    }

    public static string DayFileEntry(int day, bool example = false)
    {
        var daypath = example ? $"day{day:00}.example.txt" : $"day{day:00}.txt";

        return daypath;
    }
}
