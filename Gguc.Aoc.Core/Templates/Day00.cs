#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day00 : Day
{
    private const int YEAR = 2099;
    private const int DAY = 0;
    private List<string> _source;
    private List<string> _data;

    public Day00(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;

        _source = Parser.Parse(ConvertInput);
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;
    }

    protected override void ComputePart1()
    {
    }

    protected override void ComputePart2()
    {
    }

    private string ConvertInput(string input)
    {
        return input;
    }

    #region Dump
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data[0].Dump("Item");
        _data.DumpCollection("List");
    }
    #endregion Dump
}

#if DUMP

#endif