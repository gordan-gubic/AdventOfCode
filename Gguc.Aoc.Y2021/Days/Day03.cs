#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day03 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 3;

    private List<int> _data;

    public Day03(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Example;

        _data = Parser.Parse(Converters.ToInt);
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
        return 0;
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