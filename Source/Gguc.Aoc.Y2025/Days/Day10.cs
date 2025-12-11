#define LOG

namespace Gguc.Aoc.Y2025.Days;

using Gguc.Aoc.Y2025.Models;
using Gguc.Aoc.Y2025.Services;

public class Day10 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 10;

    private List<string> _raw;
    private List<Factory> _data;

    public Day10(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = false;
        ExecuteTest = true;
        ExecuteProd = false;

        ExpectedTest1 = "7";
        ExpectedTest2 = "33";

        ExpectedProd1 = "520";
        ExpectedProd2 = "20626";
    }

    protected override void InitParser()
    {
        _raw = Parser.Parse();
    }

    protected override void ComputePart1()
    {
        // Result = FindMinimumSwitches();
    }

    protected override void ComputePart2()
    {
        // ShowPart02();
        Result = MinMaxSwitches();
    }

    private long FindMinimumSwitches()
    {
        var result = 0L;

        foreach (var factory in _data)
        {
            var service = new FactoryService(factory);
            result += service.FindMinimumSwitches();
        }

        return result;
    }

    private long MinMaxSwitches()
    {
        var result = 0L;

        foreach (var factory in _data)
        {
            var service = new FactoryService(factory, true);
            service.InitSwitches();

            var r = service.MinMaxSwitches();

            if (r < long.MaxValue)
            {
                result += r;
                $"OK! R1=[{r:0000}]. Joltages=[{factory.Joltages.ToJson()}]. Result=[{result}]".Dump();
            }
            else
            {
                $"ER! R1=[{0:0000}]. Joltages=[{factory.Joltages.ToJson()}].".Dump();
            }

        }

        return result;
    }

    private void ShowPart02()
    {
        var row = 1;
        foreach (var factory in _data)
        {
            var service = new FactoryService(factory);
            service.ShowMinimumJoltage(row);
            row++;
        }
    }

    protected override void ProcessData()
    {
        _data = [];

        foreach (var line in _raw)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            var f = new Factory();
            _data.Add(f);

            f.SetTarget(parts[0]);
            f.SetSwitches(parts[1..^1]);
            f.SetJoltages(parts[^1]);
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

        // _data.DumpJson();
    }
}

#if DUMP
#endif