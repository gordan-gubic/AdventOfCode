#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

using Gguc.Aoc.Core.Models;

public class Day06 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 06;

    private List<string> _data;
    private List<Point> _points;
    private int _width;
    private int _height;
    private Map<Point> _map;
    private Map<int> _values;
    private Dictionary<Point, int> _dict;
    private int _target;

    public Day06(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "3909";
        Expected2 = "36238";
    }

    protected override void InitParser()
    {
        InitTest();
        InitProd();

        _data = Parser.Parse();
    }

    private void InitTest()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Example;

        _target = 32;
    }

    private void InitProd()
    {
        Parser.Type = ParserFileType.Real;

        _target = 10000;
    }

    #region Parse
    protected override void ProcessData()
    {
        _points = new();
        _width = 0;
        _height = 0;

        // Gromit do something!
        foreach (var line in _data)
        {
            var parts = line.Split(',', ' ');
            var x = parts[0].ToInt();
            var y = parts[2].ToInt();
            var point = new Point(x, y);
            _points.Add(point);

            _width = Math.Max(x, _width);
            _height = Math.Max(y, _height);
        }

        _width += 2;
        _height += 2;
        _map = new Map<Point>(_width, _height);
        _values = new Map<int>(_width, _height);

        _dict = new();

        // Log.Debug($"_width={_width}  _height={_height}");
        // _map.MapValueToString().Dump("map", true);
    }
    #endregion

    #region Head
    protected override void ComputePart1()
    {
        Result = CalculateLargestFinite();
    }

    protected override void ComputePart2()
    {
        Result = CalculateLargestSafe();
    }

    #endregion

    #region Body

    private long CalculateLargestFinite()
    {
        FillMap();
        var points = ProcessEdges();
        var max = CalculateLargest(points);

        Log.Debug($"max={max}");

        return max;
    }

    private long CalculateLargestSafe()
    {
        FillSafeMap();
        var sum = CalculateSafe(_target);

        Log.Debug($"sum={sum}");

        return sum;
    }

    private void FillMap()
    {
        _map.ForEach((x, y) =>
        {
            var md = long.MaxValue;
            var xy = new Point(x, y);
            var mem = new Point();

            foreach (var point in _points)
            {
                var temp = xy.ManhattanDistance(point);
                if (temp < md)
                {
                    md = temp;
                    mem = point;
                }
                else if (temp == md)
                {
                    mem = new Point();
                }
            }

            AddToDict(mem);
            _map[x, y] = mem;
        });

        // Log.Debug($"...");
        // _map.MapValueToString().Dump("map", true);
    }

    private void FillSafeMap()
    {
        _values.ForEach((x, y) =>
        {
            var md = 0;
            var xy = new Point(x, y);

            foreach (var point in _points)
            {
                md += (int)xy.ManhattanDistance(point);
            }

            _values[x, y] = md;
        });

        // Log.Debug($"...");
        // _values.MapValueToString().Dump("_values", true);
    }

    private List<Point> ProcessEdges()
    {
        var edges = new HashSet<Point>();

        for (var i = 0; i < _width - 1; i++)
        {
            edges.Add(_map.GetValue(i, 0));
            edges.Add(_map.GetValue(i, _height - 1));
        }

        for (var i = 0; i < _height - 1; i++)
        {
            edges.Add(_map.GetValue(0, i));
            edges.Add(_map.GetValue(_width - 1, i));
        }

        var rest = _points.Except(edges).ToList();

        // edges.DumpCollection("edges");
        // rest.DumpCollection("rest");

        return rest;
    }

    private int CalculateLargest(List<Point> points)
    {
        var max = 0;

        points.ForEach(p => max = Math.Max(max, _dict[p]));

        return max;
    }

    private int CalculateSafe(int target)
    {
        var sum = 0;

        _values.ForEach((x, y) =>
        {
            if (_values.GetValue(x, y) < target) sum++;
        });

        return sum;
    }

    private void AddToDict(Point point)
    {
        if (!_dict.ContainsKey(point)) _dict[point] = 0;
        _dict[point]++;
    }
    #endregion

    #region Dump
    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
        // _points.DumpJson();
    }
    #endregion
}

#if DUMP
#endif