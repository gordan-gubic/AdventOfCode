#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day12 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 12;

    private List<string> _data;
    private Map<char> _map;
    private int _height;
    private int _width;

    private Dictionary<string, Field> _fields;
    private HashSet<Point> _used;
    private HashSet<Point> _next;
    private Queue<Point> _active;
    private char _current;
    private string _id;

    public Day12(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "1471452";
        Expected2 = "863366";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
        _map = Parser.ParseMapChar();
        _height = _map.Height;
        _width = _map.Width;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = ProcessFields1();
    }

    protected override void ComputePart2()
    {
        Result = ProcessFields2();
    }

    private long ProcessFields1()
    {
        var sum = 0L;

        foreach (var field in _fields)
        {
            var area = field.Value.Plots.Count;
            var perimeter = CalculatePerimeter(field.Value.Plots);
            sum += area * perimeter;
        }

        return sum;
    }

    private long ProcessFields2()
    {
        var sum = 0L;

        foreach (var field in _fields)
        {
            // field.Key.Dump();

            var area = field.Value.Plots.Count;
            var borders = CalculateBorders(field.Value.Borders);

            // $"{field.Key}::{area}::{borders}".Dump();

            sum += area * borders;
        }

        return sum;
    }

    private void PlotFields()
    {
        _fields = new();
        _used = new();
        _next = new();

        _map.ForEach(PlotField);
    }

    private void PlotField(int x, int y, char ch)
    {
        var p = new Point(x, y);
        if (_used.Contains(p))
        {
            _current = _map[p.X, p.Y] == _current ? _current : default;
            return;
        }

        _active = new Queue<Point>();
        _active.Enqueue(p);

        CalculatePath();
    }

    private void CalculatePath()
    {
        while (_active.Count > 0)
        {
            ProcessPoint(_active.Dequeue());

            _active.ForEach(a => _next.Add(a));
            _active.Clear();
            _next.ForEach(c => _active.Enqueue(c));
            _next.Clear();
        }
    }

    private void ProcessPoint(Point point)
    {
        var value = _map[point.X, point.Y];
        if (value != _current)
        {
            _id = $"{value}-{Guid.NewGuid()}";
            _current = _map[point.X, point.Y];
            _fields[_id] = new();

            // $"{_current}::{_id}::{point}".Dump();
        }

        _fields[_id].Plots.Add(point);
        _used.Add(point);

        var candidates = GetCandidates(point);

        var x = ValidateCandidates(candidates, point);

        x.Item1.ForEach(c => _next.Add(c));
        x.Item2.ForEach(c => _fields[_id].Borders.Add(c));
    }

    private List<Point> GetCandidates(Point source)
    {
        var candidates = new List<Point>();

        var x = source.X;
        var y = source.Y;

        var value = _map.GetValue(x, y);

        for (var i = 0; i < 360; i += 90)
        {
            var dir = i.DegreeToDirection();
            var point = new Point(x + dir.Item1, y + dir.Item2);

            candidates.Add(point);
        }

        return candidates;
    }

    private (List<Point>, List<Point>) ValidateCandidates(List<Point> candidates, Point source)
    {
        var validated = new List<Point>();
        var invalid = new List<Point>();

        foreach (var candidate in candidates)
        {
            var isValid = ValidateCandidate(candidate, source);
            if (isValid) validated.Add(candidate);
            else if (_map.GetValue(candidate.X, candidate.Y) != _map.GetValue(source.X, source.Y))
            {
                var x = (source.X * 10) + (source.X - candidate.X);
                var y = (source.Y * 10) + (source.Y - candidate.Y);

                invalid.Add(new Point(x, y));
            }
        }

        return (validated, invalid);
    }

    private bool ValidateCandidate(Point candidate, Point source)
    {
        var x = candidate.X;
        var y = candidate.Y;

        if (!_map.Contains(x, y)) return false;
        
        if (_map[x, y] != _map[source.X, source.Y]) return false;

        if (_used.Contains(candidate)) return false;

        if (_fields[_id].Plots.Contains(candidate)) return false;

        return true;
    }

    private long CalculatePerimeter(HashSet<Point> field)
    {
        var sum = 0L;

        foreach (var point in field)
        {
            sum += CountNeighbours(point);
        }

        return sum;
    }

    private long CalculateBorders(HashSet<Point> borders)
    {
        var sum = 0L;

        var xs = new Dictionary<int, HashSet<int>>();
        var ys = new Dictionary<int, HashSet<int>>();

        foreach (var border in borders)
        {
            var x = border.X;
            var y = border.Y;

            if (x % 10 == 0)
            {
                if (!ys.ContainsKey(y)) ys[y] = new();
                ys[y].Add(x);
            }
            else
            {
                if (!xs.ContainsKey(x)) xs[x] = new();
                xs[x].Add(y);
            }
        }

        // xs.DumpJson();
        // ys.DumpJson();

        sum += CountBorders(xs);
        sum += CountBorders(ys);

        return sum;
    }

    private long CountBorders(Dictionary<int, HashSet<int>> xs)
    {
        var sum = 0L;

        foreach (var key in xs.Keys)
        {
            var values = xs[key].ToList();
            values.Sort();

            var current = values[0];
            sum++;

            for (int i = 1; i < values.Count; i++)
            {
                var v = values[i];
                if (v - current > 10) sum++;
                current = v;
            }
        }

        return sum;
    }

    private long CountNeighbours(Point point)
    {
        var sum = 0L;
        var x = point.X;
        var y = point.Y;
        var value = _map.GetValue(x, y);

        for (var i = 0; i < 360; i += 90)
        {
            var dir = i.DegreeToDirection();
            var n = new Point(x + dir.Item1, y + dir.Item2);
            var nv = _map.GetValue(n.X, n.Y);
            if (value != nv) sum++;
        }

        return sum;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        PlotFields();
        // _fields.DumpJsonIndented("fences");
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
        // _map.DumpJsonIndented("map", true);
        // _map.MapValueToString().Dump("map", true);
    }
}

#if DUMP
#endif