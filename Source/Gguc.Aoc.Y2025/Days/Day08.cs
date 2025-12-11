#define LOG

namespace Gguc.Aoc.Y2025.Days;

public class Day08 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 08;

    private List<string> _raw;
    private List<Point3d> _data;

    private Dictionary<double, (Point3d, Point3d)> _cache2 = new();

    public Day08(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = true;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "40";
        ExpectedTest2 = "25272";

        ExpectedProd1 = "181584";
        ExpectedProd2 = "8465902405";
    }

    protected override void InitParser()
    {
        _raw = Parser.Parse();
    }

    protected override void ComputePart1()
    {
        ClearCache();
        FillCache();

        Result = Count_Part01();

    }

    protected override void ComputePart2()
    {
        Result = Count_Part02();

        ClearCache();
    }

    private long Count_Part01()
    {
        var target = Parser.Type == ParserFileType.Real ? 1000 : 10;

        return MultipleThreeCircuits(target);
    }

    private long Count_Part02()
    {
        return SingularCircuit();
    }

    private void ClearCache()
    {
        _cache2 = new();
    }

    private void FillCache()
    {
        for (var i = 0; i < _data.Count - 1; i++)
        {
            for (var j = i + 1; j < _data.Count; j++)
            {
                var p0 = _data[i];
                var p1 = _data[j];
                var distance = p0.ShortestDistance(p1);

                _cache2[distance] = (p0, p1);
            }
        }
    }

    private long MultipleThreeCircuits(int target)
    {
        Dictionary<Point3d, Guid> circuits = new();
        Dictionary<Guid, long> groups = new();

        InitCircuits(circuits, groups);

        foreach (var pair in _cache2.OrderBy(x => x.Key).Take(target))
        {
            var p0 = pair.Value.Item1;
            var p1 = pair.Value.Item2;

            ProcessPair(p0, p1, circuits, groups);
        }

        var list = groups.Values.OrderByDescending(x => x).Take(3).ToList();
        return list[0] * list[1] * list[2];
    }

    private long SingularCircuit()
    {
        var result = 0L;
       
        Dictionary<Point3d, Guid> circuits = new();
        Dictionary<Guid, long> groups = new();

        InitCircuits(circuits, groups);

        foreach (var pair in _cache2.OrderBy(x => x.Key))
        {
            var p0 = pair.Value.Item1;
            var p1 = pair.Value.Item2;

            ProcessPair(p0, p1, circuits, groups);

            if (groups.Count <= 1)
            {

                Debug($"{p0} {p1}");
                result = (long)p0.X * (long)p1.X;
                break;
            }
        }

        return result;
    }

    private void InitCircuits(Dictionary<Point3d, Guid> circuits, Dictionary<Guid, long> groups)
    {
        foreach (var point in _data)
        {
            var id = Guid.NewGuid();
            circuits[point] = id;
            groups[id] = 1;
        }
    }

    private void ProcessPair(Point3d p0, Point3d p1, Dictionary<Point3d, Guid> circuits, Dictionary<Guid, long> groups)
    {
        var id = Guid.NewGuid();

        if (circuits.ContainsKey(p0) && circuits.ContainsKey(p1))
        {
            if (circuits[p0] != circuits[p1]) Merge(circuits, groups, p0, p1);
        }
        else if (circuits.ContainsKey(p0))
        {
            id = circuits[p0];
            circuits[p1] = id;
            AddId(groups, id);
        }
        else if (circuits.ContainsKey(p1))
        {
            id = circuits[p1];
            circuits[p0] = id;
            AddId(groups, id);
        }
        else
        {
            circuits[p0] = id;
            circuits[p1] = id;
            AddId(groups, id, 2);
        }
    }

    private void Merge(Dictionary<Point3d, Guid> circuits, Dictionary<Guid, long> groups, Point3d p1, Point3d p2)
    {
        var guid1 = circuits[p1];
        var guid2 = circuits[p2];

        foreach (var key in circuits.Where(x => x.Value == guid2).Select(x => x.Key).ToList())
        {
            circuits[key] = guid1;
        }

        groups[guid1] = groups[guid1] + groups[guid2];
        groups.Remove(guid2);
    }

    private void AddId(Dictionary<Guid, long> groups, Guid id, int value = 1)
    {
        if (!groups.ContainsKey(id)) groups[id] = 0;
        groups[id] += value;
    }

    protected override void ProcessData()
    {
        _data = [];

        foreach (var line in _raw)
        {
            var list = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt()).ToList();
            _data.Add(new Point3d(list[0], list[1], list[2]));
        }
    }

    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpCollection();
    }
}

#if DUMP
#endif