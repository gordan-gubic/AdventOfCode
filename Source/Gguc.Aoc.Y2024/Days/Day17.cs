#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

using Gguc.Aoc.Y2024.Memory;

public class Day17 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 17;

    private List<string> _data;
    private Day17Memory _memory;

    public Day17(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "7,3,1,3,6,3,6,0,2";
        ExpectedProd2 = "105843716614554";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

        _data = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        // ComputeOperations();
        Result = 1L;
    }

    protected override void ComputePart2()
    {
        ComputeFindItself();
        Result = 1L;
    }

    private void ComputeOperations()
    {
        var computer = new ChronospatialComputer(_memory);
        _memory.Index = 0;

        while (computer.ProcessNext()) { }

        // _memory.Dump("memory");

        var output = _memory.Output.ToJson();
        Log.Info($"Result: {output}");
    }

    private void ComputeFindItself()
    {

        var a = 0L; // _memory.RegisterA;
        // var a = _memory.RegisterA;

        a = (long)(Math.Pow(8, 16) * 0.376) - 1;
        a = (long)(Math.Pow(8, 16) * 0.3760324195) - 1;
        // a = 3229999938L - 1;
        a = 105843716614553L;

        var attempt = 0;

        var computer = new ChronospatialComputer(_memory);

        Log.Info($"A    :  {a}");
        Log.Info($"Input:  {_memory.Input.ToJson()}");

        while (true)
        {
            // a.Dump("a");

            _memory.Index = 0;
            _memory.RegisterA = a;
            _memory.RegisterB = 0;
            _memory.RegisterC = 0;
            _memory.Output.Clear();

            while (computer.ProcessNext()) { }

            // _memory.Dump("memory");
            $"{a}::{_memory}::{_memory.Output.ToJson()}".Dump("o");

            var input = _memory.Input.ToJson();
            var output = _memory.Output.ToJson();
            if (input == output) break;

            //if (a > 1000) break;
            if (attempt > 10000) break;

            a++;
            attempt++;
        }

        Log.Info($"Value:  {_memory}");
        Log.Info($"Input:  {_memory.Input.ToJson()}");
        Log.Info($"Output: {_memory.Output.ToJson()}");
    }

    private void ProcessOperation()
    {
        _memory.Input[_memory.Index].Dump("Operation");
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        /*
         * Register A: 24847151
         * Register B: 0
         * Register C: 0
         *
         * Program: 2,4,1,5,7,5,1,6,0,3,4,0,5,5,3,0
         */

        var line = _data[4][9..];

        _memory = new Day17Memory
        {
            RegisterA = _data[0][12..].ToLong(),
            RegisterB = _data[1][12..].ToLong(),
            RegisterC = _data[2][12..].ToLong(),
            Input = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToList(),
        };

        _memory.Size = _memory.Input.Count;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
        // $"{_registerA}::{_registerB}::{_registerC}::{_input.ToJson()}".Dump("data");
        _memory.Dump("data");
    }
}

#if DUMP
#endif