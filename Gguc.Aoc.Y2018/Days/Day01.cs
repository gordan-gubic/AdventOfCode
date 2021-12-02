#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day01 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 1;

    private List<string> _source;
    private List<string> _data;

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Example;

        _source = Parser.Parse();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = 0;

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = 0;

        Result = result;
    }

    private int Convert(string input)
    {
        return input.ToInt();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpCollection();
    }

}

#if DUMP
#endif