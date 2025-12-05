#define LOG

namespace Gguc.Aoc.Y2015.Days;

public class Day05 : Day
{
    private const int YEAR = 2015;
    private const int DAY = 05;

    private List<string> _raw;
    private List<string> _data;

    public Day05(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = false;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "_2015_05_Test_1_";
        ExpectedTest2 = "_2015_05_Test_2_";

        ExpectedProd1 = "_2015_05_Prod_1_";
        ExpectedProd2 = "_2015_05_Prod_2_";
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

        foreach (var line in _raw)
        {
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
    }
}

#if DUMP
#endif