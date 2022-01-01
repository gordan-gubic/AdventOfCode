#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day19 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 19;

    private const int HitHeuristic = 10;
    private List<string> _source;
    private List<List3d> _data;
    private Day19Memory _mem;

    public Day19(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    #region Parse
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    protected override void ProcessData()
    {
        _data = new List<List3d>();

        var queue = new Queue<string>(_source);
        var temp = default(List3d);

        while (queue.Count > 0)
        {
            var line = queue.Dequeue();
            if (line.StartsWith("---"))
            {
                if (temp != null) _data.Add(temp);

                temp = new List3d();
                continue;
            }

            if (line.IsWhitespace()) continue;

            var point = Point3d.Create(line);
            temp.Add(point);
        }
        _data.Add(temp);
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        _mem = new Day19Memory();

        for (int id = 0; id < _data.Count; id++)
        {
            var scanner = _data[id];
            _mem.Scanners[id] = scanner;

            var dist = CalcDistances(scanner, id);
            _mem.Distances[id] = dist;
        }

        FindBeacons();
    }

    protected override void ComputePart2()
    {
        Reset();

        var scannerCount = _mem.Scanners.Count;

        for (int i = 0; i < scannerCount; i++)
        {
            var s1 = _mem.ScannerPositions[i];

            for (int j = 0; j < scannerCount; j++)
            {
                var s2 = _mem.ScannerPositions[j];

                Max(s1.ManhattanDistance(s2));
            }
        }
    }

    private void FindBeacons()
    {
        $"Scanners---{_mem.Scanners}---".Dump();

        var scannerIds = _mem.Scanners.Keys.ToList();
        var scannerCount = _mem.Scanners.Count;

        _mem.ScannerPositions[0] = new Point3d(0, 0, 0);

        List<int> normalized = new List<int>();
        Queue<int> queue = new Queue<int>();
        List<int> notNormalized = new List<int>(scannerIds);

        normalized.Add(0);
        queue.Enqueue(0);
        notNormalized.Remove(0);

        while (queue.Count > 0)
        {
            var currentId = queue.Dequeue();
            notNormalized.Remove(currentId);

            var currentScanner = _mem.Scanners[currentId];
            var currentDistances = _mem.Distances[currentId];

            // $"--CurrentScanner-{currentId}--".Dump();

            for (int i = 0; i < notNormalized.Count; i++)
            {
                var id = notNormalized[i];
                if (normalized.Contains(id)) continue;
                // $"--NextScanner-{id}--".Dump();

                var nextScanner = _mem.Scanners[id];
                var nextDistances = _mem.Distances[id];

                var axes = CompareDistances(currentDistances, nextDistances);
                // axes.DumpJson();

                if (axes.IsNullOrEmpty()) continue;

                // $"!!!--Match-{id}--!!!".Dump();

                Normalize(nextScanner, axes, id);

                CompareScanners(currentScanner, nextScanner, id);

                Translate(id);

                queue.Enqueue(id);
                normalized.Add(id);
            }
        }

        // $"--notNormalized-{notNormalized.ToJson()}--".Dump();
        // $"--Beacons-{_mem.Beacons.Count()}--".Dump();
        // $"--Scanners-{_mem.ScannerPositions.ToJson()}--".Dump();

        foreach (var key in _mem.Scanners.Keys)
        {
            if (notNormalized.Contains(key)) continue;

            var points = _mem.Scanners[key];
            foreach (var point in points)
            {
                AddBeacon(point);
            }
        }

        Result = _mem.Beacons.Count();
    }

    private DistancesCollection CalcDistances(List3d beacons, int id)
    {
        var dc = new DistancesCollection();

        var xlist = beacons.Select(x => x.X).ToList();
        var ylist = beacons.Select(x => x.Y).ToList();
        var zlist = beacons.Select(x => x.Z).ToList();

        var cx = CalcDistances(xlist, id);
        var cy = CalcDistances(ylist, id);
        var cz = CalcDistances(zlist, id);

        dc.XPoints = cx.Item1;
        dc.YPoints = cy.Item1;
        dc.ZPoints = cz.Item1;

        dc.XDist = cx.Item2;
        dc.YDist = cy.Item2;
        dc.ZDist = cz.Item2;

        return dc;
    }

    private (Dictionary<string, List<int>>, Dictionary<string, List<int>>) CalcDistances(List<int> list, int id)
    {
        var points = new Dictionary<string, List<int>>();
        var distances = new Dictionary<string, List<int>>();

        for (int i = 0; i < list.Count; i++)
        {
            var ps = new List<int>();
            var dist = new List<int>();
            for (int j = 0; j < list.Count; j++)
            {
                ps.Add(list[j]);
                var dAbs = Math.Abs(list[i] - list[j]);
                var dRel = (list[i] > list[j]) ? dAbs : -dAbs;
                dist.Add(dRel);
            }
            dist.Sort();

            string key = $"{id}-{list[i]}-{Guid.NewGuid()}";
            points[key] = ps;
            distances[key] = dist;
        }

        return (points, distances);
    }

    private List<string> CompareDistances(DistancesCollection current, DistancesCollection next)
    {
        var isFound = false;
        var result = default(DistanceCompareResult);
        var axes = new List<string>();

        do
        {
            result = CompareDistances(current.XDist, next.XDist);
            isFound = ProcessResult(result, axes, "X", "X");
            if (isFound) break;

            result = CompareDistances(current.XDist, next.YDist);
            isFound = ProcessResult(result, axes, "X", "Y");
            if (isFound) break;

            result = CompareDistances(current.XDist, next.ZDist);
            isFound = ProcessResult(result, axes, "X", "Z");
            if (isFound) break;
        }
        while (false);

        if (!isFound) return null;

        do
        {
            result = CompareDistances(current.YDist, next.XDist);
            isFound = ProcessResult(result, axes, "Y", "X");
            if (isFound) break;

            result = CompareDistances(current.YDist, next.YDist);
            isFound = ProcessResult(result, axes, "Y", "Y");
            if (isFound) break;

            result = CompareDistances(current.YDist, next.ZDist);
            isFound = ProcessResult(result, axes, "Y", "Z");
            if (isFound) break;
        }
        while (false);

        if (!isFound) return null;

        do
        {
            result = CompareDistances(current.ZDist, next.XDist);
            isFound = ProcessResult(result, axes, "Z", "X");
            if (isFound) break;

            result = CompareDistances(current.ZDist, next.YDist);
            isFound = ProcessResult(result, axes, "Z", "Y");
            if (isFound) break;

            result = CompareDistances(current.ZDist, next.ZDist);
            isFound = ProcessResult(result, axes, "Z", "Z");
            if (isFound) break;
        }
        while (false);

        if (!isFound) return null;

        return axes;
    }

    private bool ProcessResult(DistanceCompareResult result, List<string> axes, string axis1, string axis2)
    {
        if (!result.IsFound) return false;

        /*
        $"Found match in: {axis1}-{axis2}. Reversed={result.IsReversed}. key1={result.Key1}. key2={result.Key2}.".Dump();

        $"Points1: {_mem.AllPoints[result.Key1].ToJson()}".Dump();
        $"Points2: {_mem.AllPoints[result.Key2].ToJson()}".Dump();

        $"Dist1: {_mem.AllDistances[result.Key1].ToJson()}".Dump();
        $"Dist2: {_mem.AllDistances[result.Key2].ToJson()}".Dump();
        */

        var newAxis = (result.IsReversed) ? "-" : "+";
        newAxis += axis2;

        axes.Add(newAxis);

        return true;
    }

    private DistanceCompareResult CompareDistances(Dictionary<string, List<int>> distances1, Dictionary<string, List<int>> distances2)
    {
        var isFound = false;
        var isReversed = false;
        var key1 = default(string);
        var key2 = default(string);

        foreach (var currentKey in distances1.Keys)
        {
            var currentList = distances1[currentKey].Select(x => Math.Abs(x));

            foreach (var nextKey in distances2.Keys)
            {
                var nextList = distances2[nextKey].Select(x => Math.Abs(x));

                var hits = currentList.Intersect(nextList);
                var hitCounts = currentList.Intersect(nextList).Count();

                if (hitCounts > HitHeuristic)
                {
                    isFound = true;
                    key1 = currentKey;
                    key2 = nextKey;
                    break;
                }
            }
            if (isFound) break;
        }

        if (isFound)
        {
            isReversed = distances1[key1].Intersect(distances2[key2]).Count() < HitHeuristic;
        }

        return new DistanceCompareResult
        {
            IsFound = isFound,
            IsReversed = isReversed,
            Key1 = key1,
            Key2 = key2,
        };
    }

    private void Normalize(List3d scanner, List<string> axes, int id)
    {
        if (axes[0] == "+X" && axes[0] == "+Y" && axes[0] == "+Z") return;

        // $"Normalize-Scanner-id-{id}".Dump();
        // $"Normalize-Scanner-01-{scanner.ToJson()}".Dump();
        // $"Normalize-Axes-{axes.ToJson()}".Dump();

        var oldPoints = scanner.Clone();
        scanner.Clear();

        foreach (var p in oldPoints)
        {
            var point = Point3d.Create(p, axes);
            scanner.Add(point);
        }

        // $"Normalize-Scanner-02-{scanner.ToJson()}".Dump();

        var dist = CalcDistances(scanner, id);
        _mem.Distances[id] = dist;
    }

    private void Translate(int id)
    {
        var scanner = _mem.Scanners[id];
        var p0 = _mem.ScannerPositions[id];

        // $"Translate-Scanner-id-{id}".Dump();
        // $"Translate-Scanner-01-{scanner.ToJson()}".Dump();

        var oldPoints = scanner.Clone();
        scanner.Clear();

        foreach (var p in oldPoints)
        {
            var x = p.X + p0.X;
            var y = p.Y + p0.Y;
            var z = p.Z + p0.Z;

            var point = new Point3d(x, y, z);

            scanner.Add(point);
        }

        // $"Translate-Scanner-02-{scanner.ToJson()}".Dump();

        var dist = CalcDistances(scanner, id);
        _mem.Distances[id] = dist;
    }

    private void CompareScanners(List3d scanner1, List3d scanner2, int id)
    {
        Dictionary<Point3d, List<(Point3d, Point3d)>> dict = new Dictionary<Point3d, List<(Point3d, Point3d)>>();

        foreach (var p1 in scanner1)
        {
            foreach (var p2 in scanner2)
            {
                var x = p1.X - p2.X;
                var y = p1.Y - p2.Y;
                var z = p1.Z - p2.Z;

                var diff = new Point3d(x, y, z);

                if (!dict.ContainsKey(diff)) dict[diff] = new List<(Point3d, Point3d)>();
                dict[diff].Add((p1, p2));
            }
        }

        var dict1 = dict.Where(x => x.Value.Count > 11).Select(x => x).FirstOrDefault();
        var key = dict1.Key;
        var values = dict1.Value;

        // $"CompareScanners: key: {key}".Dump();
        // $"CompareScanners: values: {values.Count().ToJson()}".Dump();

        _mem.ScannerPositions[id] = key;
    }

    private void AddBeacon(Point3d point)
    {
        _mem.Beacons.Add((point.X, point.Y, point.Z));
    }

    #region Dump
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data[0].Dump("Item");
        // _data.DumpCollection("List");
    }
    #endregion Dump
}

#if DUMP

#endif