#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2015.Days;

public class Day03 : Day
{
    private const int YEAR = 2015;
    private const int DAY = 03;

    private List<string> _raw;
    private List<char[]> _data;

    public Day03(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "2081";
        Expected2 = "2341";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _raw = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = CountDistinct();
    }

    protected override void ComputePart2()
    {
        Result = CountDistinctRoboSanta();
    }

    private long CountDistinct()
    {
        var houses = new HashSet<Point>();

        var path = _data[0];
        var current = new Point();
        houses.Add(current);

        foreach (var op in path)
        {
            var house = ProcessOperation(op, current);
            houses.Add(house);
            current = house;
        }

        return houses.Count;
    }

    private long CountDistinctRoboSanta()
    {
        var houses = new HashSet<Point>();

        var path = _data[0];
        var current1 = new Point();
        var current2 = new Point();
        houses.Add(current1);

        for (var i = 0; i < path.Length - 1; i += 2)
        {
            var op1 = path[i];
            var op2 = path[i + 1];

            var house1 = ProcessOperation(op1, current1);
            var house2 = ProcessOperation(op2, current2);

            houses.Add(house1);
            houses.Add(house2);

            current1 = house1;
            current2 = house2;
        }

        return houses.Count;
    }

    private Point ProcessOperation(char op, Point current)
    {
        return op switch
        {
            '^' => current with { X = current.X - 1 },
            'v' => current with { X = current.X + 1 },
            '<' => current with { Y = current.Y - 1 },
            '>' => current with { Y = current.Y + 1 },
        };
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _data = [];

        // Gromit do something!
        foreach (var line in _raw)
        {
            _data.Add(line.ToCharArray());
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpJson();
    }
}

#if DUMP
#endif