#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

using System.Threading.Channels;

public class Day17 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 17;

    private Map<int> _map;
    // private HashSet<(int, int, long, char, int)> _cache;
    private Dictionary<(int, int, char, int), long> _cache;

    public Day17(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "1110";
        Expected2 = "1294";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _map = Parser.ParseMapInt();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        _cache.Clear();

        var result = 0L; // SumHeatLoss();

        Result = result;
    }

    protected override void ComputePart2()
    {
        _cache.Clear();

        var result = SumHeatLoss();

        Result = result;
    }

    private long SumHeatLoss()
    {
        var sum = 0L;

        var start = new Point(0, 0);
        var end = new Point(_map.Width - 1, _map.Height - 1);

        var result = FindPath(start, end);

        return result;
    }

    private long FindPath(Point start, Point end)
    {
        var result = long.MaxValue;
        var init1 = new Crucible { Point = start, Sum = 0L, Dir = 'E', Count = 0 };
        var init2 = new Crucible { Point = start, Sum = 0L, Dir = 'S', Count = 0 };
        var active = new Queue<Crucible>();

        // init.Sum = _map.GetValue(0, 0);
        active.Enqueue(init1);
        active.Enqueue(init2);

        while (active.Count > 0)
        {
            // Log.Debug($"active={active.Count} {active.ToJson()}");
            
            var last = active.Dequeue();
            var point = last.Point;
            var sum = last.Sum;
            var dir = last.Dir;
            var count = last.Count;

            if (count >= 4 && ValidatePoint(point, end))
            {
                // _cache.Add((point.X, point.Y, dir ));
                result = Math.Min(result, last.Sum);
                continue;
            }

            // if (_cache.Contains((point.X, point.Y, sum, dir, count))) continue;
            // _cache.Add(((point.X, point.Y, sum, dir, count)));
            
            if (CheckCache(last)) continue;
            _cache[(point.X, point.Y, dir, count)] = last.Sum;

            ProcessPath(active, last, result);
        }

        return result;
    }
    private bool CheckCache(Crucible crucible)
    {
        if (_cache.ContainsKey((crucible.Point.X, crucible.Point.Y, crucible.Dir, crucible.Count)))
        {
            if (_cache[(crucible.Point.X, crucible.Point.Y, crucible.Dir, crucible.Count)] <= crucible.Sum)
            {
                return true;
            }
        }

        return false;
    }

    private bool ValidatePoint(Point point, Point goal)
    {
        return point == goal;
    }

    private void ProcessPath(Queue<Crucible> active, Crucible path, long result)
    {
        var candidates = GetCandidates(path);

        foreach (var candidate in candidates)
        {
            if (Validate(candidate, path, result))
            {
                active.Enqueue(candidate);
            }
        }
    }

    private IEnumerable<Crucible> GetCandidates(Crucible path)
    {
        var x = path.Point.X;
        var y = path.Point.Y;
        var sum = path.Sum;

        return new List<Crucible>
        {
            new() {Point = new Point(x, y - 1), Sum = sum + _map.GetValue(x, y - 1), Dir = 'N', Count = -1 },
            new() {Point = new Point(x, y + 1), Sum = sum + _map.GetValue(x, y + 1), Dir = 'S', Count = -1 },
            new() {Point = new Point(x - 1, y), Sum = sum + _map.GetValue(x - 1, y), Dir = 'W', Count = -1 },
            new() {Point = new Point(x + 1, y), Sum = sum + _map.GetValue(x + 1, y), Dir = 'E', Count = -1 },
        };
    }

    private bool Validate(Crucible candidate, Crucible path, long result)
    {
        var x = candidate.Point.X;
        var y = candidate.Point.Y;

        if (!_map.Contains(x, y)) return false;

        if (candidate.Sum >= result) return false;

        if (path.Count < 4 && path.Dir != candidate.Dir) return false;

        if ((path.Dir == 'N' && candidate.Dir == 'S')
           || (path.Dir == 'S' && candidate.Dir == 'N')
           || (path.Dir == 'W' && candidate.Dir == 'E')
           || (path.Dir == 'E' && candidate.Dir == 'W')) return false;

        candidate.Count = (path.Dir == candidate.Dir) ? path.Count + 1 : 1;

        if(candidate.Count > 10) return false;

        // if (_cache.Contains((candidate.Point.X, candidate.Point.Y, candidate.Sum, candidate.Dir, candidate.Count))) return false;
        if (CheckCache(candidate)) return false;

        return true;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _cache = new();
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

        // var candidates = new Dictionary<char, Crucible>
        // {
        //     ['N'] = new Crucible(),
        //     ['S'] = new Crucible(),
        //     ['W'] = new Crucible(),
        //     ['E'] = new Crucible(),
        // };

#endif