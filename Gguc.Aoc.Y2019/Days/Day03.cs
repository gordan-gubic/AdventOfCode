#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2019.Days;

public class Day03 : Day
{
    private const int YEAR = 2019;
    private const int DAY = 3;

    private List<List<(char, int)>> _source;

    private List<Point> _dots1;
    private List<Point> _dots2;
    private IEnumerable<Point> _cross;

    public Day03(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse(ConvertInput);
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _ProcessData();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        foreach (var dot in _cross)
        {
            Min(dot.ManhattanDistance());
        }
    }

    protected override void ComputePart2()
    {
        foreach (var dot in _cross)
        {
            var x = _dots1.IndexOf(dot);
            var y = _dots2.IndexOf(dot);

            Min(x + y);
        }
    }

    private void _ProcessData()
    {
        _dots1 = new List<Point>();
        _dots2 = new List<Point>();

        var line1 = _source[0];
        var line2 = _source[1];

        FillDots(_dots1, line1);
        FillDots(_dots2, line2);

        _cross = _dots1.Intersect(_dots2);
    }

    private void FillDots(List<Point> dots, List<(char, int)> line)
    {
        var current = new Point(0, 0);
        dots.Add(current);

        foreach (var (op, val) in line)
        {
            current = Add(dots, current, op, val);
        }
    }

    private Point Add(List<Point> dots, Point current, in char op, in int val)
    {
        // Log.DebugLog(ClassId, $"{current} {op} {val}");

        var x = 0;
        var y = 0;

        switch (op)
        {
            case 'U': y = 1; break;
            case 'D': y = -1; break;
            case 'L': x = -1; break;
            case 'R': x = 1; break;
        }

        for (int i = 0; i < val; i++)
        {
            current = new Point(current.X + x, current.Y + y);
            dots.Add(current);
        }

        return current;
    }

    private List<(char, int)> ConvertInput(string input)
    {
        var result = new List<(char, int)>();
        var sequence = input.Split(',').ToList();

        void Add(string value)
        {
            var x = value[0];
            var y = value.Remove(0, 1).ToInt();

            result.Add((x, y));
        }

        sequence.ForEach(Add);

        return result;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        Log.DebugLog(ClassId);

        _source[0].DumpCollection("Item");
        _source[1].DumpCollection("Item");
    }
}

#if DUMP
#endif