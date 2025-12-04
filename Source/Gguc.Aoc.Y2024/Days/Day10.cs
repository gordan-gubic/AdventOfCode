#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day10 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 10;

    private List<string> _data;
    private Map<int> _map;
    private List<Point> _heads;

    // a-star
    private Queue<Trail> _active;
    private HashSet<Trail> _cache;
    private HashSet<Point> _final;
    private HashSet<Trail> _finalTrail;
    private int _sum;

    public Day10(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "607";
        ExpectedProd2 = "1384";
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
        Init();
        Result = CountTrails();
    }

    protected override void ComputePart2()
    {
        Result = _finalTrail.Count;
    }

    private long CountTrails()
    {
        foreach (var head in _heads)
        {
            ProcessHead(head);
        }

        return _sum;
    }

    private void Init()
    {
        _active = new();
        _cache = new();
        _final = new();
        _finalTrail = new();
        _sum = 0;
    }

    private void ProcessHead(Point head)
    {
        _active.Clear();
        _final.Clear();

        var trail = new Trail { Begin = head, Current = head };
        trail.Path.Add(head);

        _active.Enqueue(trail);

        CalculatePath();

        _sum += _final.Count;
    }

    private void CalculatePath()
    {
        while (_active.Count > 0)
        {
            ProcessPoint(_active.Dequeue());
        }
    }

    private void ProcessPoint(Trail trail)
    {
        trail.Points.Add(trail.Current);

        if (trail.Value == 9)
        {
            _final.Add(trail.Current);
            _finalTrail.Add(trail);
            return;
        }

        var candidates = GetCandidates(trail);

        candidates = ValidateCandidates(candidates, trail);

        candidates.ForEach(c => _active.Enqueue(c));
    }

    private List<Trail> GetCandidates(Trail trail)
    {
        var candidates = new List<Trail>();

        var x = trail.Current.X;
        var y = trail.Current.Y;

        var value = _map.GetValue(x, y);

        for (var i = 0; i < 360; i += 90)
        {
            var dir = i.DegreeToDirection();
            var point = new Point(x + dir.Item1, y + dir.Item2);
            
            var candidate = new Trail { Begin = trail.Begin, Current = point, Value = _map.GetValue(point.X, point.Y)};
            candidate.Path = trail.Path.ToList();
            candidate.Path.Add(point);
            candidate.Points = trail.Points.ToHashSet();

            candidates.Add(candidate);
        }

        return candidates;
    }

    private List<Trail> ValidateCandidates(List<Trail> candidates, Trail trail)
    {
        var validated = new List<Trail>();

        foreach (var candidate in candidates)
        {
            var isValid = ValidateCandidate(candidate, trail);
            if (isValid) validated.Add(candidate);
        }

        return validated;
    }

    private bool ValidateCandidate(Trail candidate, Trail trail)
    {
        var x = candidate.Current.X;
        var y = candidate.Current.Y;
        
        if(!_map.Contains(x, y)) return false;
        
        if(candidate.Points.Contains(candidate.Current)) return false;

        if (candidate.Value - trail.Value != 1) return false;

        return true;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _heads = new();

        _map.ForEach((x, y, value) => { if(value == 0) _heads.Add(new Point(x, y));});
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
        // _map.MapValueToString().Dump("map", true);
        // _heads.DumpCollection();
    }
}

#if DUMP
#endif