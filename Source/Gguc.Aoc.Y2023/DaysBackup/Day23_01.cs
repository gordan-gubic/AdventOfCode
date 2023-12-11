#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.DaysBackup;

public class Day23_01 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 23_01;

    private Map<char> _map;
    private int _height;
    private int _width;
    private Dictionary<Point, int> _cache = new();

    public Day23_01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "2210";
        Expected2 = "";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _map = Parser.ParseMapChar();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        _cache.Clear();
        var result = FindLongestPath();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        Result = result;
    }

    private long FindLongestPath()
    {
        var result = 0L;

        var map = _map;
        var start = new Point(1, 0);
        var target = new Point(_width - 2, _height - 1);

        var init = new ForestPath_01();
        init.Add(start);

        var queue = new Queue<ForestPath_01>(new[] { init });
        while (queue.Count > 0)
        {
            var point = queue.Dequeue();

            if (point.X == target.X && point.Y == target.Y)
            {
                result = Math.Max(result, point.Distance);
                continue;
            }

            ProcessPath(map, queue, point);
        }

        return result - 1;
    }

    private void ProcessPath(Map<char> map, Queue<ForestPath_01> queue, ForestPath_01 path)
    {
        var candidates = GetCandidates(path.Point);
        var copypath = path.Path.Points.ToList(); //.ToHashSet();

        var i = 0;
        foreach (var candidate in candidates)
        {
            if (!Validate(map, path, candidate)) continue;

            if (i == 0)
            {
                path.Add(candidate);
                queue.Enqueue(path);
            }
            else
            {
                // var newpath = new ForestPath { Path = copypath.ToHashSet() };
                var newpath = new ForestPath_01();
                newpath.Add(candidate);
                queue.Enqueue(newpath);
            }

            i++;
        }
    }

    private bool Validate(Map<char> map, ForestPath_01 path, Point point)
    {
        var x = point.X;
        var y = point.Y;

        if (!map.Contains(x, y) || map[x, y] == '#') return false;
        if (path.Path.Points.Contains(point)) return false;
        if (CheckCache(point, path.Distance + 1)) return false;

        return true;
    }

    private bool CheckCache(Point point, int distance)
    {
        if (_cache.ContainsKey(point) && _cache[point] > distance)
        {
            return true;
        }

        _cache[point] = distance;
        return false;
    }

    private IEnumerable<Point> GetCandidates(Point point)
    {
        var x = point.X;
        var y = point.Y;
        var value = _map[x, y];
        var list = new List<Point>();

        switch (value)
        {
            // (^, >, v, and <)
            case '^': { list.Add(new(x, y - 1)); break; }
            case 'v': { list.Add(new(x, y + 1)); break; }
            case '<': { list.Add(new(x - 1, y)); break; }
            case '>': { list.Add(new(x + 1, y)); break; }
            default:
                list.Add(new(x, y - 1));
                list.Add(new(x, y + 1));
                list.Add(new(x - 1, y));
                list.Add(new(x + 1, y));
                break;
        }

        return list;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _height = _map.Height;
        _width = _map.Width;
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

        // _map.MapValueToString().Dump("map", true);
    }
}

#if DUMP
        _map.MapValueToString().Dump("map", true);
#endif