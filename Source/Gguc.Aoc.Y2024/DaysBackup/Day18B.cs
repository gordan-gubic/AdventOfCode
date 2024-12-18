#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day18B : Day
{
    private const int YEAR = 2024;
    private const int DAY = 18;

    private List<string> _data;
    private Map<char> _map;
    private Map<bool> _walls;
    private List<Point> _points;

    public Day18B(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "292";
        Expected2 = "58,44";
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
        // Result = CalculateShortestPath(12, 7);
        Result = CalculateShortestPath(1024, 71);
    }

    protected override void ComputePart2()
    {
        // Result = CalculateBlockedPath(12);
        var point = CalculateBlockedPath(1024);
        Log.Info($"Result=[{point}]");
    }

    private long CalculateShortestPath(int bytes, int size)
    {
        _map = new Map<char>(size, size, '.');
        _walls = new Map<bool>(size, size);

        for (var i = 0; i < bytes; i++)
        {
            _walls[_points[i].X, _points[i].Y] = true;
            _map[_points[i].X, _points[i].Y] = '#';
        }

        var search = new RamSearch
        {
            Walls = _walls
        };

        search.Find();

        var result = search.Result.MinBy(x => x.Value).Value;

        return result;
    }

    private Point CalculateBlockedPath(int b)
    {
        var point = default(Point);

        for (var i = 12; i < _points.Count; i++)
        {
            _walls[_points[i].X, _points[i].Y] = true;

            var search = new RamSearch
            {
                Walls = _walls
            };

            search.Find();

            if (search.Result.IsNullOrEmpty())
            {
                // $"Break at {i}. {_points[i]}".Dump();
                point = _points[i];
                break;
            }
        }

        return point;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _points = new();

        foreach (var line in _data)
        {
            var a = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt()).ToList();
            _points.Add(new Point(a[0], a[1]));
        }
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

        // _data.DumpCollection();
        // _map.MapBoolToString().Dump("map", true);
    }
}

#if DUMP
#endif