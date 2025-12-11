#define LOG

namespace Gguc.Aoc.Y2025.Days;

public class Day09 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 09;

    private List<string> _raw;
    private List<(int, int)> _data;

    private const int Reduce = 1;

    public Day09(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = false;
        ExecuteTest = false;
        ExecuteProd = true;

        ExpectedTest1 = "50";
        ExpectedTest2 = "24";

        ExpectedProd1 = "4767418746";
        ExpectedProd2 = "1461987144";
    }

    protected override void InitParser()
    {
        _raw = Parser.Parse();
    }

    protected override void ComputePart1()
    {
        Result = FindMaxArea();
    }

    protected override void ComputePart2()
    {
        // Result = FindLimitedArea();
        Result = FindLimitedArea(94697, 48668);
        Result = FindLimitedArea(94697, 50108);
    }

    private long FindMaxArea()
    {
        var result = 0L;

        for (var i = 0; i < _data.Count - 1; i++)
        {
            for (var j = i; j < _data.Count; j++)
            {
                var p0 = _data[i];
                var p1 = _data[j];

                var area = Area(p0.Item1, p0.Item2, p1.Item1, p1.Item2);
                result = Math.Max(result, area);
            }
        }

        return result;
    }

    private long FindLimitedArea()
    {
        var result = 0L;

        for (var i = 0; i < _data.Count - 1; i++)
        {
            for (var j = i; j < _data.Count; j++)
            {
                var p0 = _data[i];
                var p1 = _data[j];

                var area = Area(p0.Item1, p0.Item2, p1.Item1, p1.Item2);

                if (area > result && ValidateArea(p0, p1))
                {
                    Debug($"{p0} {p1} {area}");
                    result = area;
                }
            }
        }

        return result;
    }

    private long FindLimitedArea(int aX, int aY)
    {
        var result = 0L;

        var r0 = (0, 0);
        var r1 = (0, 0);

        for (var j = 0; j < _data.Count; j++)
        {
            var p0 = (aX, aY);
            var p1 = _data[j];

            var area = Area(p0.Item1, p0.Item2, p1.Item1, p1.Item2);

            if (area > result && ValidateArea(p0, p1))
            {
                r0 = p0;
                r1 = p1;
                result = area;
            }
        }

        Info($"{r0} {r1} {result}");
        return result;
    }

    private bool ValidateArea((int, int) p0, (int, int) p1)
    {
        var x1 = Math.Min(p0.Item1, p1.Item1);
        var x2 = Math.Max(p0.Item1, p1.Item1);

        var y1 = Math.Min(p0.Item2, p1.Item2);
        var y2 = Math.Max(p0.Item2, p1.Item2);

        var points = _data.Where(p => p.Item1 >= x1 && p.Item1 <= x2 && p.Item2 >= y1 && p.Item2 <= y2).ToList();
        points.Remove(p0);
        points.Remove(p1);

        return !points.Any();
    }

    private long Count_Part02()
    {
        var result = 0L;



        return result;
    }

    private long Area(long p1X, long p1Y, long p2X, long p2Y)
    {
        return (Math.Abs(p1X - p2X) + 1) * (Math.Abs(p1Y - p2Y) + 1);
    }

    protected override void ProcessData()
    {
        _data = [];

        foreach (var line in _raw)
        {
            var n = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt()).ToArray();
            _data.Add((n[0], n[1]));
        }
    }

    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpCollection();
    }
}

#if DUMP
#endif