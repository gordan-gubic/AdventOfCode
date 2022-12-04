#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day04 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 4;

    private List<string> _data;
    private List<(List<int>, List<int>)> _ranges;

    public Day04(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

        _data = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = FindIntersects1();
    }

    protected override void ComputePart2()
    {
        Result = FindIntersects2();
    }

    private long FindIntersects1()
    {
        var count = 0L;

        foreach (var (l1, l2) in _ranges)
        {
            var c = l1.Intersect(l2).Count();

            if(c == l1.Count || c == l2.Count) count++;
        }

        return count;
    }

    private long FindIntersects2()
    {
        var count = 0L;

        foreach (var (l1, l2) in _ranges)
        {
            var c = l1.Intersect(l2).Count();

            if (c > 0) count++;
        }

        return count;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _ranges = new List<(List<int>, List<int>)>();

        foreach (var value in _data)
        {
            var parts = value.Split(',', '-');
            var numbers = parts.Select(x => x.ToInt()).ToList();

            var l1 = Enumerable.Range(numbers[0], numbers[1] - numbers[0] + 1).ToList();
            var l2 = Enumerable.Range(numbers[2], numbers[3] - numbers[2] + 1).ToList();

            _ranges.Add((l1, l2));
        }

        // _ranges.DumpJson();
    }

    private int Convert(string input)
    {
        return input.ToInt();
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