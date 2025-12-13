#define LOG

namespace Gguc.Aoc.Y2025.Days;

using Gguc.Aoc.Y2025.Services;

public class Day11 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 11;

    private List<string> _raw;
    private Dictionary<string, HashSet<string>> _data;
    private Dictionary<string, HashSet<string>> _reversed;

    public Day11(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = false;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "5";
        ExpectedTest2 = "2";

        ExpectedProd1 = "753";
        ExpectedProd2 = "450854305019580";
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
        var service = new Path11Service(_data);

        var result = service.CountAllPaths("you", "out");

        return result;
    }

    private long Count_Part02()
    {
        var service0 = new Path11Service(_data);
        var result1 = service0.CountAllPaths("svr", "fft", "dac", "out");
        result1.Dump("result1");

        var service1 = new Path11Service(_data);
        var result2 = service1.CountAllPaths("svr", "dac", "fft", "out");
        result2.Dump("result2");

        return result1 + result2;
    }

    protected override void ProcessData()
    {
        _data = [];
        _reversed = [];

        foreach (var line in _raw)
        {
            var parts = line.Split([' ', ':'], StringSplitOptions.RemoveEmptyEntries).ToList();

            var part0 = parts[0];
            foreach (var part in parts[1..])
            {
                if (!_data.ContainsKey(part0)) _data[part0] = [];
                if (!_reversed.ContainsKey(part)) _reversed[part] = [];
                
                _data[part0].Add(part);
                _reversed[part].Add(part0);
            }
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

        _data.DumpJson();
    }
}

#if DUMP
#endif