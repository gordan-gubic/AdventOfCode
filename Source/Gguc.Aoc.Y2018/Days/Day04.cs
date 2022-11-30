#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day04 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 04;

    private List<string> _source;
    private List<string> _data;

    public Day04(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    #region Parse
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Example;

        _source = Parser.Parse();
    }

    protected override void ProcessData()
    {
        _data = _source;
    }
    #endregion Parse

    protected override void ComputePart1()
    {
    }

    protected override void ComputePart2()
    {
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

#if DROP

#endif