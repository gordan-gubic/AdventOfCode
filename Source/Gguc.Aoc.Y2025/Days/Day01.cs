#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2025.Days;

public class Day01 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 1;

    private List<string> _raw;

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "2025-01-A";
        Expected2 = "2025-01-B";
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
        var result = 0L;

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        Result = result;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        // Gromit do something!
        foreach (var line in _raw)
        {
        }
    }

    private int Convert(string input)
    {
        return input.ToInt();
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