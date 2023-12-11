#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day21 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 21;

    private Map<bool> _map;
    private Point _start;
    private int _target;

    public Day21(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "3649";
        Expected2 = "612941134797232";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _map = Parser.ParseMapBool();

        _target = (Parser.Type == ParserFileType.Test) ? 6 : 64;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var map = ExtendMap(_map, 1);
        // map.MapBoolToString(falsech:'.').Dump("map", true);
        var start = new Point(map.Width / 2, map.Height / 2);

        var result = CountPositions(map, start, _target);

        Result = result;
    }

    protected override void ComputePart2()
    {
        if (Parser.Type == ParserFileType.Test) return;

        var steps = 26501365;
        var map = ExtendMap(_map, 5);
        var start = new Point(map.Width / 2, map.Height / 2);

        // map.MapBoolToString(falsech:'.').Dump("map", true);

        var targets = CountMultipleTargets(map, start);

        var result = CalulatePredictions(targets, steps);

        Result = result;
    }

    private long CalulatePredictions(Dictionary<long, long> targets, int steps)
    {
        var sum = 0L;
        var keys = targets.Keys.Order().ToList();

        var target0 = keys[0];
        var target1 = keys[1];
        var target2 = keys[2];

        var step0 = targets[target0];
        var step1 = targets[target1];
        var step2 = targets[target2];

        var diff0 = step0;
        var diff1 = step1 - step0;
        var diff2 = step2 - step1 - diff1;

        Log.Debug($"{new { step0, step1, step2, diff0, diff1, diff2 }}");

        var blocks = (steps - target0) / (target1 - target0);
        sum = diff0 + (blocks * diff1) + ((blocks - 1).TriangularSequence() * diff2);
        Log.Debug($"{new { steps, blocks, sum }}");

        return sum;
    }

    private Map<bool> ExtendMap(Map<bool> map, int factor)
    {
        var newMap = new Map<bool>(map.Width * factor, map.Height * factor);
        for (int y = 0; y < newMap.Height; y++)
        {
            for (int x = 0; x < newMap.Width; x++)
            {
                newMap[x, y] = map[x % map.Width, y % map.Height];
            }
        }

        return newMap;
    }

    private Dictionary<long, long> CountMultipleTargets(Map<bool> map, Point start)
    {
        var targets = GetTargets(_map);

        CountPositions(map, start, targets);

        Log.Debug($"Targets: [{targets.ToJson()}]");

        return targets;
    }

    private Dictionary<long, long> GetTargets(Map<bool> map)
    {
        var length = map.Height;
        var half = length / 2;

        var targets = new Dictionary<long, long>();
        for (var i = 0; i < 3; i++)
        {
            targets[half + length * i] = 0L;
        }

        return targets;
    }

    private long CountPositions(Map<bool> map, Point start, long target)
    {
        var queue = new Queue<Point>();
        var list = new HashSet<Point> { start };

        for (var i = 0; i < target; i++)
        {
            queue = new Queue<Point>(list);
            list.Clear();

            while (queue.Count > 0)
            {
                var point = queue.Dequeue();
                ProcessPoint(map, list, point);
            }

            // Log.Debug($"Step=[{i + 1}]... list=[{list.Count}]");
        }

        return list.Count;
    }

    private long CountPositions(Map<bool> map, Point start, Dictionary<long, long> targets)
    {
        var queue = new Queue<Point>();
        var list = new HashSet<Point> { start };
        var target = targets.Keys.Max();

        for (var i = 0; i < target; i++)
        {
            queue = new Queue<Point>(list);
            list.Clear();

            while (queue.Count > 0)
            {
                var point = queue.Dequeue();
                ProcessPoint(map, list, point);
            }

            if (targets.ContainsKey(i + 1)) targets[i + 1] = list.Count;

            // Log.Debug($"Step=[{i + 1}]... list=[{list.Count}]");
        }

        return list.Count;
    }

    private void ProcessPoint(Map<bool> map, HashSet<Point> list, Point point)
    {
        var candidates = GetCandidates(point);

        foreach (var candidate in candidates)
        {
            if (Validate(map, list, candidate))
            {
                list.Add(candidate);
            }
        }
    }

    private IEnumerable<Point> GetCandidates(Point point)
    {
        var x = point.X;
        var y = point.Y;

        return new List<Point>
        {
            new(x, y - 1),
            new(x, y + 1),
            new(x - 1, y),
            new(x + 1, y),
        };
    }

    private bool Validate(Map<bool> map, HashSet<Point> list, Point point)
    {
        var x = point.X;
        var y = point.Y;

        if (!map.Contains(x, y) || map[x, y]) return false;

        return true;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _start = new Point(_map.Width / 2, _map.Height / 2);
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
        // _map.MapBoolToString(falsech:'.').Dump("map", true);
        _start.Dump();
    }
}

#if DUMP
        var md = start.ManhattanDistance(new Point(0, 0)) + 10;

#endif