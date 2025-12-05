#define LOG

namespace Gguc.Aoc.Y2025.Days;

public class Day01 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 1;

    private List<string> _raw;
    private List<(char, int)> _data;

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = true;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "3";
        ExpectedTest2 = "6";

        ExpectedProd1 = "1158";
        ExpectedProd2 = "6860";
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
        Result = CountNorth();
    }

    protected override void ComputePart2()
    {
        Result = CountAllNorth();
    }

    private long CountNorth()
    {
        var result = 0L;

        var pos = 50;
        var mt = 0;

        foreach (var line in _data)
        {
            (pos, mt) = ProcessLine(pos, line.Item1, line.Item2);
            if (pos == 0) result++;

            Debug($"{new { line, pos, mt }}");
        }

        return result;
    }

    private long CountAllNorth()
    {
        var result = 0L;

        var pos = 50;
        var mt = 0;

        foreach (var line in _data)
        {
            (pos, mt) = ProcessLine(pos, line.Item1, line.Item2);
            result += mt;
        }

        return result;
    }

    private (int, int) ProcessLine(int pos, char dir, int value)
    {
        var oldpos = pos;

        pos = dir switch
        {
            'R' => pos + value,
            'L' => pos - value,
            _ => pos
        };

        var m = 0;

        if (pos < 0)
        {
            m = pos / -100;
            pos += m * 100;
            if (pos < 0) pos = 100 + pos;

            if (oldpos > 0) m++;
        }
        else if (pos >= 100)
        {
            m = pos / 100;
            pos -= m * 100;
        }

        if (pos == 0 && m == 0) m++;

        return (pos, m);
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _data = new();

        // Gromit do something!
        foreach (var line in _raw)
        {
            var part1 = line[0];
            var part2 = line[1..].ToInt();
            _data.Add((part1, part2));
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        // _raw.DumpCollection();
        _data.DumpCollection();
    }
}

#if DUMP
#endif