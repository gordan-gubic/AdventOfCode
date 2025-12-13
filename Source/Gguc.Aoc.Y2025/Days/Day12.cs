#define LOG

namespace Gguc.Aoc.Y2025.Days;

using Gguc.Aoc.Y2025.Models;

public class Day12 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 12;

    private List<string> _raw;
    private List<string> _data;
    private List<Map<char>> _shapes;
    private List<Region> _regions;
    private List<int> _sizes;

    public Day12(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = false;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "_2025_00_Test_1_";
        ExpectedTest2 = "_2025_00_Test_2_";

        ExpectedProd1 = "_2025_00_Prod_1_";
        ExpectedProd2 = "_2025_00_Prod_2_";
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

        foreach (var region in _regions)
        {
            var surface = region.Surface;
            var total = 0L;
            for (int i = 0; i < region.Values.Count; i++)
            {
                total += region.Values[i] * _sizes[i];
            }

            result += (surface - total > 0) ? 1 : 0;
        }

        return result;
    }

    private long Count_Part02()
    {
        var result = 0L;

        return result;
    }

    protected override void ProcessData()
    {
        _data = [];
        _shapes = [];
        _regions = [];
        _sizes = [];

        var shapesRaw = _raw[..30];
        var regionsRaw = _raw[30..];

        for (int i = 1; i < 30; i+= 5)
        {
            var map = new Map<char>(3, 3);
            for (int j = 0; j < 3; j++)
            {
                var lx = shapesRaw[i + j].ToCharArray();
                for (int k = 0; k < 3; k++)
                {
                    map[k, j] = lx[k];
                }
            }

            _shapes.Add(map);
            _sizes.Add(map.CountValues('#'));
        }

        foreach (var r in regionsRaw)
        {
            var region = Region.Parse(r);
            _regions.Add(region);
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

        foreach (var shape in _shapes)
        {
            shape.MapValueToString().Dump(newLine: true);
        }
        _regions.DumpCollection();
        _sizes.DumpJson();
    }
}

#if DUMP
#endif