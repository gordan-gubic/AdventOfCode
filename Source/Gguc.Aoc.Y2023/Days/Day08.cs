#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day08 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 08;

    private List<string> _data;
    private char[] _instructions;
    private Dictionary<string, (string, string)> _nodes;

    public Day08(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "16897";
        Expected2 = "16563603485021";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = CountSteps1();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = CountSteps2();

        Result = result;
    }

    private long CountSteps1()
    {
        var start = "AAA";
        var end = "ZZZ";
        var size = _instructions.Length;

        var steps = 0;
        var current = start;

        while (current != end)
        {
            var index = steps % size;
            var dir = _instructions[index];

            current = (dir == 'L') ? _nodes[current].Item1 : _nodes[current].Item2;

            steps++;
        }

        return steps;
    }

    private long CountSteps2()
    {
        var allstart = _nodes.Keys.Where(x => x[2] == 'A').Select(x => x).ToArray();
        allstart.DumpJson();

        var size = _instructions.Length;

        var dict = new Dictionary<string, long>();
        foreach (var x in allstart)
        {
            var steps = 0;

            var current = x;
            while (true)
            {
                var index = steps % size;
                var dir = _instructions[index];

                current = (dir == 'L') ? _nodes[current].Item1 : _nodes[current].Item2;

                steps++;

                if  (current[2] == 'Z') break;
            }

            dict[x] = steps;
        }

        // use LCM Calculator - Least Common Multiple on these
        // https://www.calculatorsoup.com/calculators/math/lcm.php
        dict.Values.DumpJson();

        var lcm = dict.Values.LeastCommonMultiple();
        lcm.Dump("LCM");

        return lcm;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _instructions = _data[0].ToCharArray();

        var data = _data[2..];

        var nodes = new Dictionary<string, (string, string)>();
        foreach (var line in data)
        {
            var key = line[0..3];
            var left = line[7..10];
            var right = line[12..15];

            nodes[key] = (left, right);
        }

        _nodes = nodes;
    }

    private int Convert(string input)
    {
        return input.ToInt();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        //_data.DumpCollection();

        //_instructions.DumpJson();
        //_nodes.DumpJson();
    }
}

#if DUMP
#endif