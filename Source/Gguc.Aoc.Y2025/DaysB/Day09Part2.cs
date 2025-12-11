#define LOG

namespace Gguc.Aoc.Y2025.Days;

using System.Security.Cryptography;

public class Day09Part2 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 09;

    private List<string> _raw;
    private List<(int, int)> _data;
    private Map<int> _map;

    private const int Reduce = 2000;

    public Day09Part2(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = false;
        ExecuteTest = false;
        ExecuteProd = true;

        ExpectedTest1 = "50";
        ExpectedTest2 = "_2025_09_Test_2_";

        ExpectedProd1 = "4767418746";
        ExpectedProd2 = "1461987144";
    }

    protected override void InitParser()
    {
        _raw = Parser.Parse();
    }

    protected override void ComputePart1()
    {
        Result = Count_Part01();
    }

    protected override void ComputePart2()
    {
        Result = Count_Part02();
    }

    private long Count_Part01()
    {
        var result = 0L;

        for (int i = 0; i < _data.Count - 1; i++)
        {
            for (int j = i; j < _data.Count; j++)
            {
                var p0 = _data[i];
                var p1 = _data[j];

                var area = Area(p0.Item1, p0.Item2, p1.Item1, p1.Item2);
                result = Math.Max(result, area);
            }
        }

        return result;
    }

    private long Count_Part02()
    {
        var result = 0L;

        var x0 = _data[0].Item1 / Reduce;
        var y0 = _data[0].Item2 / Reduce;
        _map[x0, y0] = 2;

        for (int i = 1; i < _data.Count; i++)
        {
            var x = _data[i].Item1 / Reduce;
            var y = _data[i].Item2 / Reduce;

            _map[x, y] = 2;

            Line(_map, x0, y0, x, y);

            x0 = x;
            y0 = y;
        }

        Line(_map, x0, y0, _data[0].Item1 / Reduce, _data[0].Item2 / Reduce);

        _map.MapValueToString().Dump("map", true);

        return result;
    }

    private long Area(long p1X, long p1Y, long p2X, long p2Y)
    {
        return (Math.Abs(p1X - p2X) + 1) * (Math.Abs(p1Y - p2Y) + 1);
    }

    private void Line(Map<int> map, int x0, int y0, int x, int y)
    {
        if (y0 == y)
        {
            var min = Math.Min(x, x0);
            var max = Math.Max(x, x0);

            for (int i = min + 1; i < max; i++)
            {
                map[i, y] = 1;
            }

        }
        else
        {
            var min = Math.Min(y, y0);
            var max = Math.Max(y, y0);

            for (int i = min + 1; i < max; i++)
            {
                map[x, i] = 1;
            }
        }
    }

    protected override void ProcessData()
    {
        _data = [];

        var maxX = 0;
        var maxY = 0;

        foreach (var line in _raw)
        {
            var n = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt()).ToArray();
            _data.Add((n[0], n[1]));

            maxX = Math.Max(maxX, n[0]);
            maxY = Math.Max(maxX, n[1]);
        }

        _map = new Map<int>(maxX / Reduce + 2, maxY / Reduce + 2);
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