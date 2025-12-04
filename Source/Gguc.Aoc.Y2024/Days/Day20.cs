#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day20 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 20;

    private List<string> _data;
    private Map<char> _map;
    private Map<bool> _walls;
    private MazeSearch20 _mazeSearch;
    private long _real;
    private List<Point> _path;
    private List<Point> _pathReverse;
    private Dictionary<Point, int> _pathValue;

    public Day20(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "1389";
        ExpectedProd2 = "1005068";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();

        _map = Parser.ParseMapChar();
        _walls = Parser.ParseMapBool();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        CalculateBestPath();

        var found = _mazeSearch.Result.MinBy(x => x.Value);
        var foundMin = found?.Value ?? 0L;
        _real = foundMin;

        // ShowPath();
        Result = CheatPath();
    }

    protected override void ComputePart2()
    {
        Result = CheatMore();
    }

    private void CalculateBestPath()
    {
        var s = _map.Find('S');
        var start = new Point(s.Item2, s.Item3);

        _mazeSearch = new MazeSearch20();
        _mazeSearch.Map = _map;
        _mazeSearch.Walls = _walls;

        _mazeSearch.Find(start);

        _path = _mazeSearch.Result[0].Path;
        _pathReverse = _path.ToList();
        _pathReverse.Reverse();

        _pathValue = new();
        for (var i = 0; i < _path.Count; i++)
        {
            var p = _path[i];
            _pathValue[p] = i;
        }
    }

    private void ShowPath()
    {
        _mazeSearch.Result.Count.Dump("result");
        _mazeSearch.Result[0].Path.DumpCollection("path");

        var map = _map.Clone();
        foreach (var point in _mazeSearch.Result[0].Path)
        {
            map[point.X, point.Y] = 'O';
        }
        
        map.MapValueToString().Dump("map", true);
    }

    private long CheatPath()
    {
        var path = _path.ToList();
        var saves = new Dictionary<long, int>();

        var i = 0;
        foreach (var point in path)
        {
            var jumps = SkipWalls(point);
            
            foreach (var jump in jumps)
            {
                var value = _pathValue[jump];
                var total = _real - (value - i - 2);
                
                var diff = (_real - total);
                if (diff > 99)
                {
                    if (!saves.ContainsKey(diff)) saves[diff] = 0;
                    saves[diff]++;
                }
            }

            i++;
        }

        // saves.DumpJson("saves", true);

        var sum = 0L;
        saves.Values.ForEach(s => sum += s);
        return sum;
    }

    private long CheatMore()
    {
        var saves = new Dictionary<long, int>();
        var size = _path.Count;

        for (var i = 0; i < size; i++)
        {
            var point = _path[i];

            for (var j = 0; j < size - i; j++)
            {
                var reverse = _pathReverse[j];
                var md = point.ManhattanDistance(reverse);
                if(md > 20) continue;

                var value = _pathValue[reverse];
                var total = _real - (value - i - md);
                var diff = (_real - total);

                if (diff > 99)
                {
                    if (!saves.ContainsKey(diff)) saves[diff] = 0;
                    saves[diff]++;
                }
            }
        }

        // saves.DumpJson("saves", true);

        var sum = 0L;
        saves.Values.ForEach(s => sum += s);
        return sum;
    }

    private List<Point> SkipWalls(Point point)
    {
        var jumps = new List<Point>();
        var x = point.X;
        var y = point.Y;

        if (_walls[x - 1, y]) jumps.Add(new Point(x - 2, y));
        if (_walls[x + 1, y]) jumps.Add(new Point(x + 2, y));
        if (_walls[x, y - 1]) jumps.Add(new Point(x, y - 2));
        if (_walls[x, y + 1]) jumps.Add(new Point(x, y + 2));

        jumps.RemoveAll(p => !_map.Contains(p.X, p.Y) || _walls.GetValue(p.X, p.Y));

        return jumps;
    }

    protected override void ProcessData()
    {
        base.ProcessData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
        // _map.MapValueToString().Dump("map", true);
        // _walls.MapBoolToString().Dump("_walls", true);
    }
}

#if DUMP
#endif