#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day23 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 23;

    private List<string> _source;
    private List<string> _data;

    public Day23(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    #region Parse
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Example;

        _source = Parser.Parse(ConvertInput);
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