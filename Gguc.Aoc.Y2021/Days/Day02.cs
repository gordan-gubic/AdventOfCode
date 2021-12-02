#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day02 : Day
{
    private List<(DiveOperation, int)> _data;

    public Day02(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2021;
        Parser.Day = 2;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse(Convert);
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        // _data.DumpCollection("List");
    }

    protected override void ComputePart1()
    {
        var result = 0;

        var distance = 0;
        var depth = 0;

        foreach (var item in _data)
        {
            switch(item.Item1)
            {
                case DiveOperation.Forward:
                    distance += item.Item2;
                    break;

                case DiveOperation.Up:
                    depth -= item.Item2;
                    break;

                case DiveOperation.Down:
                    depth += item.Item2;
                    break;
            }
        }

        distance.Dump();
        depth.Dump();

        Result = distance * depth;
    }

    protected override void ComputePart2()
    {
        var result = 0;

        var aim = 0;
        var distance = 0;
        var depth = 0;

        foreach (var item in _data)
        {
            switch (item.Item1)
            {
                case DiveOperation.Forward:
                    distance += item.Item2;
                    depth += item.Item2 * aim;
                    break;

                case DiveOperation.Up:
                    aim -= item.Item2;
                    break;

                case DiveOperation.Down:
                    aim += item.Item2;
                    break;
            }
        }

        distance.Dump();
        depth.Dump();

        Result = distance * depth;
    }

    private int Three(int v)
    {
        return 0; // _data[v] + _data[v + 1] + _data[v + 2];
    }

    private (DiveOperation, int) Convert(string input)
    {
        var x = input.Split(' ');
        Enum.TryParse<DiveOperation>(x[0], true, out var e);
        return (e, int.Parse(x[1]));
    }

}

#if DUMP
#endif