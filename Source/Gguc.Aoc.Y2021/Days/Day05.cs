#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day05 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 5;

    private List<string> _source;
    private List<Point> _data;
    private Map<int> _map;
    private int _size;

    public Day05(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = new List<Point>();

        foreach (var line in _source)
        {
            var pointsRaw = line.Split(" -> ");

            foreach (var p in pointsRaw)
            {
                var xy = p.Split(',').Select(x => x.ToInt()).ToList();

                Max(xy[0]);
                Max(xy[1]);

                _data.Add(new Point(xy[0], xy[1]));
            }
        }

        _size = (int)Result;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = 0;

        _map = new Map<int>(_size + 1, _size + 1);

        DrawLines(false);

        CountHits();

        _map.Dump("map", true);
    }

    protected override void ComputePart2()
    {
        Result = 0;

        _map = new Map<int>(_size + 1, _size + 1);

        DrawLines();

        CountHits();

        _map.Dump("map", true);
    }

    private void CountHits()
    {
        Result = 0;
        foreach (var p in _map.Values)
        {
            if (p > 1) Add(1);
        }
    }

    private void DrawLines(bool drawDiagonal = true)
    {
        for (int i = 0; i < _data.Count; i += 2)
        {
            var p1 = _data[i];
            var p2 = _data[i + 1];

            if (!drawDiagonal && p1.X != p2.X && p1.Y != p2.Y) continue;

            DrawLine(p1, p2);
        }
    }

    private void DrawLine(Point p1, Point p2)
    {
        var dirX = p2.X.CompareTo(p1.X);
        var dirY = p2.Y.CompareTo(p1.Y);

        var x = p1.X;
        var y = p1.Y;
        var lastpoint = false;

        do
        {
            lastpoint = x == p2.X && y == p2.Y;

            _map.Values[x, y]++;

            y += dirY;
            x += dirX;
        }
        while (!lastpoint);
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data[0].Dump("Item");
        // _data.DumpCollection("List");
    }
}

#if DUMP

#endif