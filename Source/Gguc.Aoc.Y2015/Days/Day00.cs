#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2015.Days;

public class Day00 : Day
{
    private const int YEAR = 2015;
    private const int DAY = 0;

    private List<string> _raw;
    private List<string> _data;

    public Day00(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "_2015_00_1_";
        ExpectedProd2 = "_2015_00_2_";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

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