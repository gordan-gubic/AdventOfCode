#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day03 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 03;

    private List<string> _data;
    private Map<char> _map;
    private int _width;
    private int _height;
    private List<Point> _simbols;
    private List<Point> _gears;
    private List<(int, Point, Point)> _sources;

    private List<int> _sn = new();
    private List<int> _sy = new();


    public Day03(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "535351";
        Expected2 = "87287096";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
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
        var result = SumSources();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = SumGears();

        Result = result;
    }

    private long SumSources()
    {
        var sum = 0L;

        foreach (var (i, x, y) in _sources)
        {
            if (!Intersect(x) && !Intersect(y))
            {
                continue;
            }

            sum += i;
        }

        return sum;
    }

    private long SumGears()
    {
        var sum = 0L;

        var dict = SourcesToDict(_sources);

        foreach (var g in _gears)
        {
            var border = GetBorder(g);

            var intersect = border.Intersect(dict.Keys);

            var values = new HashSet<int>();
            foreach (var v in intersect) values.Add(dict[v]);

            if (values.Count == 2)
            {
                sum += values.First() * values.Last();
            }
        }

        return sum;
    }

    private bool Intersect(Point point)
    {
        var border = GetBorder(point);

        return border.Intersect(_simbols).Any();
    }

    private List<Point> GetBorder(Point point)
    {
        var border = new List<Point>();

        for (int j = point.Y - 1; j <= point.Y + 1; j++)
        {
            for (int i = point.X - 1; i <= point.X + 1; i++)
            {
                border.Add(new Point(i, j));
            }
        }

        return border;
    }

    private Dictionary<Point, int> SourcesToDict(List<(int, Point, Point)> sources)
    {
        var dict = new Dictionary<Point, int>();

        foreach (var (i, x, y) in sources)
        {
            dict[x] = i;
            dict[y] = i;
        }

        return dict;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _map = new Map<char>(_data, x => x);
        _width = _map.Width;
        _height = _map.Height;

        _simbols = FindSimbols(_map);
        _gears = FindGears(_map);

        _sources = new List<(int, Point, Point)>();
        _sources = FindSources(_map);
    }

    private List<Point> FindSimbols(Map<char> map)
    {
        var simbols = new List<Point>();

        map.ForEach((x, y, c) =>
        {
            if (c == '.' || char.IsDigit(c)) return;

            simbols.Add(new Point(x, y));
        });

        return simbols;
    }

    private List<Point> FindGears(Map<char> map)
    {
        var gears = new List<Point>();

        map.ForEach((x, y, c) =>
        {
            if (c != '*') return;

            gears.Add(new Point(x, y));
        });

        return gears;
    }

    private List<(int, Point, Point)> FindSources(Map<char> map)
    {
        var sources = new List<(int, Point, Point)>();

        for (var j = 0; j < _height; j++)
        {
            for (var i = 0; i < _width; i++)
            {
                var value = _map[i, j];
                if (!char.IsDigit(value)) continue;

                var raw = $"{_map[i, j]}";
                var next1 = _map[i + 1, j];
                var next2 = _map[i + 2, j];

                if (char.IsDigit(next1))
                {
                    raw += next1;

                    if (char.IsDigit(next2))
                    {
                        raw += next2;
                    }
                }

                var number = raw.ToInt();
                var first = new Point(i, j);

                if (raw.Length > 1) i++;
                if (raw.Length > 2) i++;

                var last = new Point(i, j);

                sources.Add((number, first, last));
            }
        }

        return sources;
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
        //_map.MapValueToString().Dump();

        // _simbols.DumpCollection();
        // _gears.DumpCollection();
        // _sources.DumpCollection();
    }
}

#if DUMP
#endif