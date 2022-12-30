#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day00 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 0;

    private List<string> _data;

    public Day00(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "";
        Expected2 = "";
    }

    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

        _data = Parser.Parse();
    }
    #region Parse

    protected override void ProcessData()
    {
        // Gromit do something!
        foreach (var line in _data)
        {
        }
    }

    private int Convert(string input)
    {
        return input.ToInt();
    }
    #endregion

    #region Head
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
    #endregion

    #region Body
    #endregion

    #region Dump
    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpCollection();
    }
    #endregion
}

#if DUMP
#endif