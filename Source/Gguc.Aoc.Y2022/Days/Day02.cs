#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day02 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 2;

    private const int Rock = 1;
    private const int Paper = 2;
    private const int Scissors = 3;

    private const int Win = 6;
    private const int Draw = 3;
    private const int Loss = 0;

    private List<string> _data;
    private List<(string, string)> _games = new();

    public Day02(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "10310";
        Expected2 = "14859";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
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
        var result = 0L;

        result = CountPoints(Count1);

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        result = CountPoints(Count2);

        Result = result;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        foreach (var value in _data)
        {
            var split = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            _games.Add((split[0], split[1]));
        }

        _games.DumpCollection();
    }

    private long CountPoints(Func<string, string, int> func)
    {
        var points = 0L;

        foreach (var value in _games)
        {
            points += func(value.Item1, value.Item2);
        }

        return points;
    }

    private int Count1(string p1, string p2)
    {
        return (p1, p2) switch
        {
            // A => Rock
            ("A", "X") => Rock + Draw,
            ("A", "Y") => Paper + Win,
            ("A", "Z") => Scissors + Loss,
            // A => Paper
            ("B", "X") => Rock + Loss,
            ("B", "Y") => Paper + Draw,
            ("B", "Z") => Scissors + Win,
            // A => Scissors
            ("C", "X") => Rock + Win,
            ("C", "Y") => Paper + Loss,
            ("C", "Z") => Scissors + Draw,
            _ => 0
        };
    }

    private int Count2(string p1, string p2)
    {
        return (p1, p2) switch
        {
            // A => Rock
            ("A", "X") => Loss + Scissors,
            ("A", "Y") => Draw + Rock,
            ("A", "Z") => Win + Paper,
            // A => Paper
            ("B", "X") => Loss + Rock,
            ("B", "Y") => Draw + Paper,
            ("B", "Z") => Win + Scissors,
            // A => Scissors
            ("C", "X") => Loss + Paper,
            ("C", "Y") => Draw + Scissors,
            ("C", "Z") => Win + Rock,
            _ => 0
        };
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
    }
}

#if DUMP
#endif