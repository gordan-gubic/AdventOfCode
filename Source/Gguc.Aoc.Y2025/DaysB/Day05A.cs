#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2025.Days;

public class Day05A : Day
{
    private const int YEAR = 2025;
    private const int DAY = 0501;

    private List<string> _raw;
    private List<(long, long)> _ranges;
    private List<long> _data;

    public Day05A(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = true;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "3";
        ExpectedTest2 = "14";

        ExpectedProd1 = "707";
        ExpectedProd2 = "361615643045059";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        _raw = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = CountFresh();
    }

    protected override void ComputePart2()
    {
        Result = CountAllFresh();
    }

    private long CountFresh()
    {
        var result = 0L;

        foreach (var data in _data)
        {
            if (data == 0) continue;

            var isFresh = IsFresh(data);
            if (isFresh) result++;

            Debug($"{new { isFresh, result }}");
        }

        return result;
    }

    private bool IsFresh(long data)
    {
        foreach (var range in _ranges)
        {
            if (data >= range.Item1 && data <= range.Item2) return true;
        }

        return false;
    }

    private long CountAllFresh()
    {
        var result = 0L;

        var ranges = _ranges.OrderByDescending(r => r.Item2).OrderBy(r => r.Item1);

        var min = ranges.First().Item1;
        var max = ranges.First().Item2;

        foreach (var range in ranges)
        {
            var x1 = range.Item1;
            var x2 = range.Item2;

            if (x1 <= max || x2 <= max)
            {
                min = long.Min(min, x1);
                max = long.Max(max, x2);
                continue;
            }

            var score = max - min + 1;
            result += score;

            Debug($"{new { min, max, score, result }}");

            min = x1;
            max = x2;
        }

        var s1 = max - min + 1;
        result += s1;

        Debug($"{new { min, max, s1, result }}");
        
        return result;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var isRanges = true;
        _ranges = [];
        _data = [];

        // Gromit do something!
        foreach (var line in _raw)
        {
            if (isRanges)
            {
                if (line.IsWhitespace())
                {
                    isRanges = false;
                    continue;
                }

                var rs = line.Split('-', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToArray();
                _ranges.Add((rs[0], rs[1]));
            }

            _data.Add(line.ToLong());
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        //_ranges.DumpCollection("ranges");
        // _data.DumpCollection("data");
    }
}

#if DUMP
#endif