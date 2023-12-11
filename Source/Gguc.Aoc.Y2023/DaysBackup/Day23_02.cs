#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.DaysBackup;

public class Day23_02 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 23_02;

    private Map<char> _map;
    private int _height;
    private int _width;
    private Dictionary<Point, int> _cache = new();
    private Dictionary<(Point, Point), Path> _cachePath = new();

    public Day23_02(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "2210";
        Expected2 = "";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

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

    private void ProcessPath(Map<char> map, Queue<ForestPath_01> queue, ForestPath_01 fPath)
    {
        var candidates = GetCandidates(fPath.Point);

        var i = 0;
        var validCandidates = candidates.Where(c => Validate(map, fPath, c)).ToList();

        if (validCandidates.Count == 1)
        {
            fPath.Add(validCandidates[0]);
            queue.Enqueue(fPath);
            return;
        }

        foreach (var candidate in validCandidates)
        {
            var copypath = fPath.Path.Points.ToList();

            Log.Debug($"Crossroad at [{fPath.Point}] dist [{copypath.Count}]");
            var newpath = new Path(); // { StartPoint = copypath.First(), EndPoint = copypath.Last(), Points = copypath };
            CheckCache(newpath);

            var newforest = new ForestPath_01();
            newforest.Add(candidate);
            newforest.AddPath(newpath);

            queue.Enqueue(newforest);
        }
    }

    private bool Validate(Map<char> map, ForestPath_01 path, Point point)
    {
        var x = point.X;
        var y = point.Y;

        if (!map.Contains(x, y) || map[x, y] == '#') return false;
        if (path.Path.Points.Contains(point)) return false;
        // if (CheckCache(point, path.Distance + 1)) return false;

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

    private void CheckCache(Path path)
    {
        var p1 = path.Points.First();
        var p2 = path.Points.Last();
        var points = path.Points.ToList();

        if (!_cachePath.ContainsKey((p1, p2)))
        {
            _cachePath[(p1, p2)] = path;
        }

        if (!_cachePath.ContainsKey((p2, p1)))
        {
            _cachePath[(p2, p1)] = path;
        }
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