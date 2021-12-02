#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2019.Days;

public class Day02 : Day
{
    private List<long> _source;
    private List<long> _data;
    private IIntcodeEngine _intcodeEngine;

    public Day02(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2019;
        Parser.Day = 2;
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
        _intcodeEngine.WriteMemory(1, 12);
        _intcodeEngine.WriteMemory(2, 2);

        _intcodeEngine.Run();

        Result = _intcodeEngine.ReadMemory(0);

        _intcodeEngine.Dump("_intcodeEngine");
    }

    protected override void ComputePart2()
    {
        var target = 19690720;

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                _intcodeEngine.Reset();
                _intcodeEngine.WriteMemory(1, i);
                _intcodeEngine.WriteMemory(2, j);

                _intcodeEngine.Run();
                var r = _intcodeEngine.ReadMemory(0);

                if (r == target)
                {
                    Result = i * 100 + j;
                    Info($"[{i}, {j}]: {r}");
                    Info($"Result = [{Result}]");
                    break;
                }
            }
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        Debug();

        // Part1: 10566835
        // Part2: 2347

        _data[0].Dump("Item");
        _data.DumpJson("List");
    }
}

#if DUMP

#endif