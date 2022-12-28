#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day24 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 24;

    private List<string> _data;
    private List<Blizzard> _blizzards;
    private Dictionary<int, List<Blizzard>> _blizzardsColumns;
    private Dictionary<int, List<Blizzard>> _blizzardsRows;
    private Map<int> _map;
    private Point _start;
    private Point _end;
    private HashSet<(Point, int)> _cache;

    public Day24(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "332";
        Expected2 = "942";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Example;
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
        _cache.Clear();

        var minute = FindPath(_start, _end, 0);

        Result = minute + 1;
    }

    protected override void ComputePart2()
    {
        _cache.Clear();

        var minute = FindPath(_start, _end, 0);
        minute = FindPath(_end, _start, minute);
        minute = FindPath(_start, _end, minute);

        Result = minute + 1;
    }

    private int FindPath(Point start, Point end, int minute)
    {
        var init = default((Point, int));
        var active = new Queue<(Point, int)>();

        while (true)
        {
            minute++;
            if (!IsBlizzards(minute, 0, 0))
            {
                // Log.Debug($"Try: minute={minute}");
                init = (start, 1);
                active.Enqueue(init);
            }

            while (active.Count > 0)
            {
                // Log.Debug($"active={active.Count} {active.ToJson()}");
                var last = active.Dequeue();
                var point = last.Item1;
                var time = last.Item2 + minute;

                if (ValidatePoint(point, end)) return time;

                if (_cache.Contains((point, time))) continue;
                _cache.Add((point, time));

                ProcessPath(active, last, minute);
            }
        }
    }

    private bool ValidatePoint(Point point, Point goal)
    {
        return point == goal;
    }

    private bool ValidatePoint(Point point, Point goal, int minute)
    {
        if (point == goal)
        {
            point.DumpJson("point");
            minute.Dump("minute");
        }

        return point == goal;
    }

    private void ProcessPath(Queue<(Point, int)> active, (Point, int) path, int offset)
    {
        var candidates = GetCandidates(path);

        foreach (var candidate in candidates)
        {
            if (Validate(candidate, offset))
            {
                active.Enqueue(candidate);
            }
        }
    }

    private List<(Point, int)> GetCandidates((Point, int) path)
    {
        var point = path.Item1;
        var minute = path.Item2 + 1;

        var x = point.X;
        var y = point.Y;

        return new List<(Point, int)>
        {
            (new (x, y), minute),
            (new (x - 1, y), minute),
            (new (x + 1, y), minute),
            (new (x, y - 1), minute),
            (new (x, y + 1), minute),
        };
    }

    private bool Validate((Point point, int minute) value, int offset)
    {
        var x = value.point.X;
        var y = value.point.Y;
        var minute = value.minute + offset;

        if (!_map.Contains(x, y)) return false;

        if (_cache.Contains((new (x,y), minute))) return false;

        if (IsBlizzards(minute, x, y)) return false;

        return true;
    }

    private bool IsBlizzards(int minute, int x, int y)
    {
        var blizzard = CountBlizzards(minute, x, y) > 0;

        return blizzard;
    }

    private int CountBlizzards(int minute, int x, int y)
    {
        var count = 0;

        if (_blizzardsColumns.ContainsKey(x))
        {
            count += _blizzardsColumns[x].Count(b => b.GetRelative(minute) == y);
        }

        if (_blizzardsRows.ContainsKey(y))
        {
            count += _blizzardsRows[y].Count(b => b.GetRelative(minute) == x);
        }

        return count;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _cache = new();
        _blizzards = new();
        _blizzardsColumns = new();
        _blizzardsRows = new();

        var width = _data[0].Length - 2;
        var height = _data.Count - 2;

        _map = new Map<int>(width, height, 0);
        _start = new Point(0, 0);
        _end = new Point(width - 1, height - 1);

        for (int i = 0; i < width; i++)
        {
            _blizzardsColumns[i] = new();
        }

        for (int i = 0; i < height; i++)
        {
            _blizzardsRows[i] = new();
        }

        var n = 0;
        var x = 0;
        var y = 0;
        foreach (var line in _data)
        {
            foreach (var ch in line)
            {
                if (ch is '<' or '>')
                {
                    var blizzard = new Blizzard { Index = n++, Sign = ch, Location = new Point(x - 1, y - 1), Row = y - 1, Size = width };
                    blizzard.Direction = (ch == '<') ? -1 : 1;
                    _blizzards.Add(blizzard);
                    _blizzardsRows[y - 1].Add(blizzard);
                }
                if (ch is '^' or 'v')
                {
                    var blizzard = new Blizzard { Index = n++, Sign = ch, Location = new Point(x - 1, y - 1), Column = x - 1, Size = height };
                    blizzard.Direction = (ch == '^') ? -1 : 1;
                    _blizzards.Add(blizzard);
                    _blizzardsColumns[x - 1].Add(blizzard);
                }

                x++;
            }

            x = 0;
            y++;
        }

        // _blizzards.DumpCollection();
        // _blizzardsRows.DumpJson();
        // _blizzardsColumns.DumpJson();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
    }
}

#if DUMP
#endif