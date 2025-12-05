#define LOG

namespace Gguc.Aoc.Y2015.Days;

public class Day04 : Day
{
    private const int YEAR = 2015;
    private const int DAY = 04;

    private List<string> _raw;
    private List<string> _data;

    public Day04(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = false;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "_2015_00_Test_1_";
        ExpectedTest2 = "_2015_00_Test_2_";

        ExpectedProd1 = "_2015_04_1_";
        ExpectedProd2 = "_2015_04_2_";
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
        base.ProcessData();

        _data = [];

        // Gromit do something!
        foreach (var line in _raw)
        {
        }
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