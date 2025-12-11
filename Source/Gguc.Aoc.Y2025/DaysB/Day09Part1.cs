#define LOG

namespace Gguc.Aoc.Y2025.Days;

public class Day09Part1 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 09;

    private List<string> _raw;
    private List<(long, long)> _data;
    private Map<long> _map;

    public Day09Part1(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = false;
        ExecuteTest = true;
        ExecuteProd = false;

        ExpectedTest1 = "50";
        ExpectedTest2 = "_2025_09_Test_2_";

        ExpectedProd1 = "_2025_09_Prod_1_";
        ExpectedProd2 = "_2025_09_Prod_2_";
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

        var x0 = (int)_data[0].Item1;
        var y0 = (int)_data[0].Item2;
        _map[x0, y0] = 2;

        for (int i = 1; i < _data.Count - 1; i++)
        {
            var x = _data[i].Item1;
            var y = _data[i].Item2;

            
        }

        return result;
    }

    private long Area(long p1X, long p1Y, long p2X, long p2Y)
    {
        return Math.Abs(p1X - p2X + 1) * Math.Abs(p1Y - p2Y + 1);
    }

    protected override void ProcessData()
    {
        _data = [];

        var maxX = 0L;
        var maxY = 0L;

        foreach (var line in _raw)
        {
            var n = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToArray();
            _data.Add((n[0], n[1]));

            maxX = Math.Max(maxX, n[0]);
            maxY = Math.Max(maxX, n[1]);
        }

        _map = new Map<long>((int)maxX + 1, (int)maxY + 1);
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