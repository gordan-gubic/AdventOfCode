#define LOG

namespace Gguc.Aoc.Y2015.Days;

public class Day01 : Day
{
    private const int YEAR = 2015;
    private const int DAY = 1;

    private List<string> _raw;
    private List<char[]> _data;

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = true;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "_2015_00_Test_1_";
        ExpectedTest2 = "_2015_00_Test_2_";

        ExpectedProd1 = "138";
        ExpectedProd2 = "1771";
    }

    protected override void InitParser()
    {
        _raw = Parser.Parse();
    }

    protected override void ComputePart1()
    {
        Result = CalculateFloors();
    }

    protected override void ComputePart2()
    {
        Result = FindBasement();
    }

    private long CalculateFloors()
    {
        var result = 0L;

        foreach (var floor in _data)
        {
            var value = CalculateFloor(floor);
            result += value;
        }

        return result;
    }

    private long CalculateFloor(char[] floor)
    {
        var result = 0L;

        foreach (var ch in floor)
        {
            result += ch == '(' ? 1 : -1;
        }

        return result;
    }

    private long FindBasement()
    {
        var result = 0L;

        foreach (var floor in _data)
        {
            var value = FindBasement(floor);
            result += value;
        }

        return result;
    }

    private long FindBasement(char[] floor)
    {
        var result = 0L;
        var index = 0L;

        foreach (var ch in floor)
        {
            result += ch == '(' ? 1 : -1;
            index++;

            if (result == -1) break;
        }

        return index;
    }

    protected override void ProcessData()
    {
        _data = new();

        foreach (var line in _raw)
        {
            _data.Add(line.ToCharArray());
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

        _raw.DumpCollection();
        _data.DumpJson();
    }
}

#if DUMP
#endif