#define LOG

namespace Gguc.Aoc.Y2015.Days;

public class Day02 : Day
{
    private const int YEAR = 2015;
    private const int DAY = 02;

    private List<string> _raw;
    private List<(int, int, int)> _data;

    public Day02(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = true;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "101";
        ExpectedTest2 = "48";

        ExpectedProd1 = "1586300";
        ExpectedProd2 = "3737498";
    }

    protected override void InitParser()
    {
        _raw = Parser.Parse();
    }

    protected override void ComputePart1()
    {
        Result = SumSurfaces();
    }

    protected override void ComputePart2()
    {
        Result = SumRibbons();
    }

    private long SumSurfaces()
    {
        var result = 0L;

        foreach (var surface in _data)
        {
            var sum = SumSurface(surface);
            result += sum;

            Debug($"{new { sum }}");
        }

        return result;
    }

    private long SumRibbons()
    {
        var result = 0L;

        foreach (var surface in _data)
        {
            var ribbon = SumRibbon(surface);
            result += ribbon;

            Debug($"{new { ribbon }}");
        }

        return result;
    }

    private long SumSurface((int x, int y, int z) surface)
    {
        List<long> areas = [(surface.x * surface.y), (surface.y * surface.z), (surface.x * surface.z)];
        areas.Sort();

        return areas[0] * 3 + areas[1] * 2 + areas[2] * 2;
    }

    private long SumRibbon((int x, int y, int z) surface)
    {
        List<long> areas = [surface.x, surface.y, surface.z];
        areas.Sort();

        return areas[0] * 2 + areas[1] * 2 + (surface.x * surface.y * surface.z);
    }

    protected override void ProcessData()
    {
        _data = [];

        foreach (var line in _raw)
        {
            var parts = line.Split('x', StringSplitOptions.RemoveEmptyEntries).ToArray();
            _data.Add((parts[0].ToInt(), parts[1].ToInt(), parts[2].ToInt()));
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