#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2019.Days;

public class Day07 : Day
{
    private const int YEAR = 2019;
    private const int DAY = 7;

    private List<long> _source;
    private List<long> _data;
    private IEnumerable<IEnumerable<int>> _permutations1;
    private IEnumerable<IEnumerable<int>> _permutations2;

    public Day07(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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

        _ProcessData();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var output = 0L;

        /*
         *** TEST ***
        var data = new List<long> { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 };
        var list = new List<int> { 4, 3, 2, 1, 0 };

        output = ComputeOutput1(data, list.ToList());
        Max(output);

        // Part 01 *** Result: [43210]!
        */

        /*
        */
        foreach (var list in _permutations1)
        {
            output = ComputeOutput1(_data, list.ToList());
            Max(output);
        }
    }

    protected override void ComputePart2()
    {

        var output = 0L;

        /*
         *** TEST ***
        var data = new List<long> { 3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26, 27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5 };
        var list = new List<int> { 9, 8, 7, 6, 5 };

        output = ComputeOutput2(data, list.ToList());
        Max(output);
        // Part 02 *** Result: [139629729]!
        */

        /*
        */
        foreach (var list in _permutations2)
        {
            output = ComputeOutput2(_data, list.ToList());
            Max(output);
        }
    }

    private long ComputeOutput1(List<long> data, List<int> inputs)
    {
        var output = 0L;

        for (int i = 0; i < inputs.Count; i++)
        {
            var ie = new IntcodeEngine(data);
            ie.SetInput(inputs[i], output);
            ie.Run();
            output = ie.Output;
        }

        return output;
    }

    private long ComputeOutput2(List<long> data, List<int> inputs)
    {
        var output = 0L;

        var ie1 = new IntcodeEngine(data);
        var ie2 = new IntcodeEngine(data);
        var ie3 = new IntcodeEngine(data);
        var ie4 = new IntcodeEngine(data);
        var ie5 = new IntcodeEngine(data);

        ie1.SetInput(inputs[0], output);
        ie2.SetInput(inputs[1]);
        ie3.SetInput(inputs[2]);
        ie4.SetInput(inputs[3]);
        ie5.SetInput(inputs[4]);

        ie1.SetOutputEngine(ie2);
        ie2.SetOutputEngine(ie3);
        ie3.SetOutputEngine(ie4);
        ie4.SetOutputEngine(ie5);
        ie5.SetOutputEngine(ie1);

        ie1.Run();

        output = ie5.Output;
        return output;
    }

    private void _ProcessData()
    {
        var list = new List<int> { 0, 1, 2, 3, 4 };

        _permutations1 = Combinator.GetPermutations(list, 5);

        list = new List<int> { 5, 6, 7, 8, 9 };

        _permutations2 = Combinator.GetPermutations(list, 5);
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

        // Part 01 *** Result: [17790]!
        // Part 02 *** Result: [19384820]!

        _data[0].Dump("Item");
        _data.DumpJson("List");
    }
}

#if DUMP

IntcodeEngine07Async:
    ...
    Task.Run(ie1.Run);
    ...

IntcodeEngine07Callback:
private long ComputeOutput2(List<long> data, List<int> inputs)
{
    var output = 0L;

    var ie1 = new IntcodeEngine07(data);
    ie1.SetInput(inputs[0], output);

    var ie2 = new IntcodeEngine07(data);
    ie2.SetInput(inputs[1]);
    ie1.SetOutputCallback(x => ie2.AddInput(x));

    var ie3 = new IntcodeEngine07(data);
    ie3.SetInput(inputs[2]);
    ie2.SetOutputCallback(x => ie3.AddInput(x));

    var ie4 = new IntcodeEngine07(data);
    ie4.SetInput(inputs[3]);
    ie3.SetOutputCallback(x => ie4.AddInput(x));

    var ie5 = new IntcodeEngine07(data);
    ie5.SetInput(inputs[4]);
    ie4.SetOutputCallback(x => ie5.AddInput(x));
    ie5.SetOutputCallback(x => ie1.AddInput(x));

    ie1.Run();

    output = ie5.Output;
    return output;
}
#endif