#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day21 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 21;

    private List<string> _source;
    private List<string> _data;
    private IDay _part1;
    private IDay _part2;

    public Day21(ILog log, IParser parser, ILifetimeScope scope) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        _part1 = scope.ResolveKeyed<IDay>(20212101);
        _part2 = scope.ResolveKeyed<IDay>(20212102);
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
        Result = _part1.SolutionPart1();
    }

    protected override void ComputePart2()
    {
        Result = _part2.SolutionPart2();
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

#if DROP

#endif