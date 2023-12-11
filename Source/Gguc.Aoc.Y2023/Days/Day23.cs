#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

using System;

public class Day23 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 23;

    private Map<char> _map;
    private int _height;
    private int _width;
    private Dictionary<Point, long> _cache = new();
    private Dictionary<(Point, Point), List<Point>> AllPaths = new();
    private Dictionary<(Point, Point), long> ShortPaths = new();
    private Dictionary<Point, List<Point>> Crossroads = new();

    public Day23(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "2210";
        Expected2 = "6522";
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
        var result = 1L;

        Result = result;
    }

    protected override void ComputePart2()
    {
        Log.Info(" -- CreateAllPaths -- ");
        CreateAllPaths();

        Log.Info(" -- FindLongestPath -- ");
        var result = FindLongestPath();

        Result = result;
    }

    private void CreateAllPaths()
    {
        var map = _map;
        var start = new Point(1, 0);
        var target = new Point(_width - 2, _height - 1);
        Log.Debug($"start..=[{start}]");
        Log.Debug($"target.=[{target}]");

        var init = new Path();
        init.Add(start);

        var queue = new Queue<Path>(new[] { init });
        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            CreateProcessPath(map, queue, path);
        }

        // AllPaths.ForEach(x => Log.Debug($"[{x.Key}]={x.Value.Count}]"));
        // ShortPaths.ForEach(x => Log.Debug($"[{x.Key}]={x.Value}]"));
    }

    private void CreateProcessPath(Map<char> map, Queue<Path> queue, Path path)
    {
        var candidates = CreateGetCandidates(path.Point);

        var validCandidates = candidates.Where(c => CreateValidate(map, path, c)).ToList();

        if (validCandidates.Count == 1)
        {
            path.Add(validCandidates[0]);
            queue.Enqueue(path);
            return;
        }
        else
        {
            CreateAddCache(path);
        }

        foreach (var candidate in validCandidates)
        {
            if(CreateCheckCache(path.Point, candidate)) continue;

            var newpath = new Path();
            newpath.Add(path.Point);
            newpath.Add(candidate);

            queue.Enqueue(newpath);
        }
    }

    private bool CreateValidate(Map<char> map, Path path, Point point)
    {
        var x = point.X;
        var y = point.Y;

        if (!map.Contains(x, y) || map[x, y] == '#') return false;
        if (path.Points.Contains(point)) return false;

        return true;
    }

    private void CreateAddCache(Path path)
    {
        var p0 = path.Points[0];
        var p1 = path.Points[1];

        if(CreateCheckCache(p0, p1)) return;

        var p2 = path.Points.Last();
        var p3 = path.Points.SkipLast(1).Last();

        var points = path.Points.ToList();

        AllPaths[(p0, p1)] = points;
        AllPaths[(p2, p3)] = points;
        ShortPaths[(p0, p2)] = points.Count - 1;
        ShortPaths[(p2, p0)] = points.Count - 1;

        if (!Crossroads.ContainsKey(p0)) Crossroads[p0] = new();
        if (!Crossroads.ContainsKey(p2)) Crossroads[p2] = new();

        Crossroads[p0].Add(p2);
        Crossroads[p2].Add(p0);

        // Log.Debug($"Crossroad at [{path.Point}] dist [{path.Distance}]. Cache [{p0}, {p1}] [{p2}, {p3}]");
    }

    private bool CreateCheckCache(Point p1, Point p2)
    {
        return AllPaths.ContainsKey((p1, p2));
    }

    private IEnumerable<Point> CreateGetCandidates(Point point)
    {
        var x = point.X;
        var y = point.Y;
        var value = _map[x, y];
        var list = new List<Point>();

        switch (value)
        {
            // (^, >, v, and <)
            // case '^': { list.Add(new(x, y - 1)); break; }
            // case 'v': { list.Add(new(x, y + 1)); break; }
            // case '<': { list.Add(new(x - 1, y)); break; }
            // case '>': { list.Add(new(x + 1, y)); break; }
            default:
                list.Add(new(x, y - 1));
                list.Add(new(x, y + 1));
                list.Add(new(x - 1, y));
                list.Add(new(x + 1, y));
                break;
        }

        return list;
    }

    private long FindLongestPath()
    {
        var result = 0L;

        var start = new Point(1, 0);
        var target = new Point(_width - 2, _height - 1);

        var init = new ForestPath();
        init.Add(start, 0);

        var queue = new Queue<ForestPath>(new[] { init });
        while (queue.Count > 0)
        {
            var path = queue.Dequeue();

            if (path.X == target.X && path.Y == target.Y)
            {
                result = Math.Max(result, path.TotalDistance);
                continue;
            }

            ProcessPath(queue, path);
        }

        return result;
    }

    private void ProcessPath(Queue<ForestPath> queue, ForestPath path)
    {
        var candidates = GetCandidates(path.Point);
        var validCandidates = candidates.Where(c => Validate(path, c)).ToList();

        foreach (var candidate in validCandidates)
        {
            var newpath = path.Copy();
            var distance = ShortPaths[(path.Point, candidate)];

            newpath.Add(candidate, distance);
            if (CheckCache(newpath.Point, newpath.TotalDistance)) continue;

            queue.Enqueue(newpath);
        }
    }

    private bool CheckCache(Point point, long distance)
    {
        if (!_cache.ContainsKey(point))
        {
            _cache[point] = distance;
            return false;
        }

        if (_cache[point] >= distance) return true;

        _cache[point] = distance;
        return false;
    }

    private IEnumerable<Point> GetCandidates(Point point)
    {
        // if (!Crossroads.ContainsKey(point)) return null;

        return Crossroads[point];
    }

    private bool Validate(ForestPath path, Point point)
    {
        var x = point.X;
        var y = point.Y;

        if (path.Points.Contains(point)) return false;

        return true;
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