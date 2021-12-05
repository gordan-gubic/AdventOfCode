#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2019.Days;

public class Day05 : Day
{
    private const int YEAR = 2019;
    private const int DAY = 5;

    private List<long> _source;
    private List<long> _data;
    private IIntcodeEngine _intcodeEngine;

    public Day05(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.ParseSequence(Converters.ToLong);
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;
        _intcodeEngine = new IntcodeEngine(_data);
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        _intcodeEngine.Reset();
        _intcodeEngine.SetInput(1);
        _intcodeEngine.Run();

        Result = _intcodeEngine.Output;

        _intcodeEngine.Dump("_intcodeEngine");
    }

    protected override void ComputePart2()
    {
        _intcodeEngine.Reset();
        _intcodeEngine.SetInput(5);
        _intcodeEngine.Run();

        Result = _intcodeEngine.Output;

        _intcodeEngine.Dump("_intcodeEngine");
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        // Part 01 *** Result: [13978427]!
        // Part 02 *** Result: [11189491]!

        Log.DebugLog(ClassId);

        _data[0].Dump("Item");
        _data.DumpJson("List");
    }
}

#if DUMP

#endif