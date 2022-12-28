#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day2401 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 2401;

    private List<string> _data;
    private List<Blizzard> _blizzards;
    private Dictionary<int, List<Blizzard>> _blizzardsColumns;
    private Dictionary<int, List<Blizzard>> _blizzardsRows;
    private Map<int> _map;
    private Point _start;
    private Point _end;
    private HashSet<(Point, int)> _cache;

    public Day2401(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "332";
        Expected2 = "942";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Day = 24;
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
        /***
         * TODO: I don't need linked list - I just need (LastPoint, minute) as checkpoint.
         ***/

        var init = new LinkedList<Point>();
        var active = new Queue<LinkedList<Point>>();

        while (true)
        {
            minute++;
            if (!IsBlizzards(minute, 0, 0))
            {
                // Log.Debug($"Try: minute={minute}");
                init.Clear();
                init.AddFirst(start);
                active.Enqueue(init);
            }

            while (active.Count > 0)
            {
                // Log.Debug($"active={active.Count} {active.ToJson()}");
                var path = active.Dequeue();
                var last = path.Last();
                var time = path.Count + minute;

                if (ValidatePoint(last, end)) return time;
                
                if (_cache.Contains((last, time))) continue;
                _cache.Add((last, time));

                ProcessPath(active, path, minute);
            }
        }
    }

    private bool ValidatePoint(Point point, Point goal)
    {
        return point == goal;
    }

    private bool ValidatePath(LinkedList<Point> path, int minute)
    {
        var point = path.Last();

        if (point == _end)
        {
            minute.Dump("minute");
            path.Count.Dump("Count");
            path.DumpJson("Final Path");
        }

        return point == _end;
    }

    private void ProcessPath(Queue<LinkedList<Point>> active, LinkedList<Point> path, int offset)
    {
        var candidates = GetCandidates(path);
        var minute = path.Count + 1;

        foreach (var candidate in candidates)
        {
            if (Validate(candidate, minute + offset))
            {
                var list = new LinkedList<Point>(path);
                list.AddLast(candidate);
                active.Enqueue(list);
            }
        }
    }

    private List<Point> GetCandidates(LinkedList<Point> path)
    {
        var point = path.Last();
        var x = point.X;
        var y = point.Y;

        return new List<Point>
        {
            new (x, y),
            new (x - 1, y),
            new (x + 1, y),
            new (x, y - 1),
            new (x, y + 1),
        };
    }

    private bool Validate(Point point, int minute)
    {
        var x = point.X;
        var y = point.Y;

        if (!_map.Contains(x, y)) return false;

        if (_cache.Contains((new(x, y), minute))) return false;

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
    private long FindPath()
    {
        IsBlizzards(1, 2, 0);
        IsBlizzards(16, 5, 2);
        IsBlizzards(17, 5, 2);
        IsBlizzards(17, 4, 3);
        IsBlizzards(17, 5, 3);

        return 0L;
    }

    private void ShowMinute(int minute, int x, int y)
    {
        /*
        var columnBs = _blizzardsColumns[x];
        var rowBs = _blizzardsRows[y];

        var cHere = columnBs.Where(b => b.GetRelative(minute) == y).ToList();
        var rHere = rowBs.Where(b => b.GetRelative(minute) == x).ToList();

        Log.Debug($"  --  Minute={minute}  X={x}  Y={y}  --  ");
        cHere.DumpJson();
        rHere.DumpJson();
        */

        var count = CountBlizzards(minute, x, y);
        var any = IsBlizzards(minute, x, y);

        Log.Debug($"  --  Minute={minute}  X={x}  Y={y}  Count={count}  --  ");
        Log.Debug($"  --  Minute={minute}  X={x}  Y={y}  IsBlizzards={any}  --  ");
    }

#endif