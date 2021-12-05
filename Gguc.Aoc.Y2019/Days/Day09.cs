#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2019.Days;

public class Day09 : Day
{
    private const int YEAR = 2019;
    private const int DAY = 9;

    private List<long> _source;
    private List<long> _data;

    public Day09(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        // Test01();

        Result = ComputeOutput(_data, 1L);
    }

    protected override void ComputePart2()
    {
        Result = ComputeOutput(_data, 2L);
    }

    private void Test01()
    {
        /*
         * *** TEST ***
         */

        var data = new List<long>();

        data = new List<long> { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 };
        data = new List<long> { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 };
        data = new List<long> { 104, 1125899906842624, 99 };

        Result = ComputeOutput(data);
    }

    private long ComputeOutput(List<long> data, params long[] inputs)
    {
        var output = 0L;

        var ie = new IntcodeEngine(data);
        // Debug($"Memory: {ie.Memory.ToJson()}");

        ie.SetInput(inputs);
        ie.Run();
        output = ie.Output;

        // Debug($"Memory: {ie.Memory.ToJson()}");

        return output;
    }

    private string ConvertInput(string input)
    {
        return input;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data[0].Dump("Item");
        // _data.DumpCollection("List");

        // Part 01 *** Result: [3518157894]!
        // Part 02 *** Result: [80379]!
    }
}

#if DUMP

#endif