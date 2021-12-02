#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day02 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 2;

    private List<(DiveOperation, int)> _data;

    public Day02(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Example;

        _data = Parser.Parse(Convert);
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
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

    private (DiveOperation, int) Convert(string input)
    {
        var x = input.Split(' ');
        
        Enum.TryParse<DiveOperation>(x[0], true, out var e);
        int.TryParse(x[1], out var i);

        return (e, i);
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpCollection();
    }
}

#if DUMP
#endif