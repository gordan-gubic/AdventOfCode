#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day02 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 02;

    private List<string> _data;
    private List<List<(int, string)>> _games;

    public Day02(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "2879";
        ExpectedProd2 = "65122";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = FindLimit(12, 13, 14);

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = FindMinimum();

        Result = result;
    }
    
    private long FindLimit(int red, int green, int blue)
    {
        var sum = 0L;
        var i = 1;

        foreach (var game in _games)
        {
            var valid = IsValid(game, red, green, blue);

            if (valid) sum += i;

            i++;
        }

        return sum;
    }

    private long FindMinimum()
    {
        var sum = 0L;

        foreach (var game in _games)
        {
            sum += Minimums(game);
        }

        return sum;
    }

    private bool IsValid(List<(int, string)> game, int red, int green, int blue)
    {
        foreach (var (x, y) in game)
        {
            if(x < 12) continue;

            if(y == "red" && x > red) return false;
            if(y == "green" && x > green) return false;
            if(y == "blue" && x > blue) return false;
        }

        return true;
    }

    private long Minimums(List<(int, string)> game)
    {
        var red = 0;
        var green = 0;
        var blue = 0;

        foreach (var (x, y) in game)
        {
            if (y == "red") red = Math.Max(x, red);
            if (y == "green") green = Math.Max(x, green);
            if (y == "blue") blue = Math.Max(x, blue);
        }

        return red * green * blue;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _games = new List<List<(int, string)>>();

        foreach (var line in _data)
        {
            var rolls = ProcessLine(line);
            _games.Add(rolls);
        }
    }

    private List<(int, string)> ProcessLine(string line)
    {
        // Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green

        var part1 = line.Split(':');
        var part2 = part1.Last().Split(";");

        var rolls = new List<(int, string)>();

        foreach (var s1 in part2)
        {
            var part3 = s1.Split(",");

            foreach (var s2 in part3)
            {
                var s3 = s2.Trim().Split(' ');
                rolls.Add((s3[0].ToInt(), s3[1]));
            }
        }

        // rolls.DumpCollection("rolls");
        return rolls;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpCollection();
    }
}

#if DUMP
#endif