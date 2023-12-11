#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day11 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 11;

    private List<string> _data;

    private Map<bool> _map;
    private Map<bool> _expandedMap;
    private int _width;
    private int _height;

    public Day11(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "10173804";
        Expected2 = "634324905172";
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
        var result = SumDistances(_map, 2);

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = SumDistances(_map, 1000000);

        Result = result;
    }

    private long SumDistances(Map<bool> map, int factor = 2)
    {
        var sum = 0L;

        var list = map.FindAll(true);
        var points = new List<(long, long)>();

        var rc = FindRowsAndColumns(map);

        foreach (var p1 in list)
        {
            var p2 = (Expand(p1.Item1, rc.Columns, factor), Expand(p1.Item2, rc.Rows, factor));
            points.Add(p2);
        }

        for (var i = 0; i < points.Count - 1; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                var p1 = new PointLong(points[i].Item1, points[i].Item2);
                var p2 = new PointLong(points[j].Item1, points[j].Item2);

                sum += p1.ManhattanDistance(p2);
            }
        }

        return sum;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var lines = _data.ToList();

        var width = lines.Select(x => x.Length).Max();
        var height = lines.Count;

        _map = new(width, height, false);
        _map.ForEach((x, y) =>
        {
            var line = lines[y];

            _map[x, y] = lines[y][x] switch
            {
                '#' => true,
                _ => false,
            };
        });

        _width = _map.Width;
        _height = _map.Height;

        ExpandMap();
    }

    private void ExpandMap()
    {
        var rc = FindRowsAndColumns(_map);

        var rows = rc.Rows;
        var columns = rc.Columns;

        var map = new Map<bool>(_width + columns.Count, _height + rows.Count);
        _map.ForEach((x, y) =>
        {
            map[(int)Expand(x, columns), (int)Expand(y, rows)] = _map[x, y];
        });

        _expandedMap = map;
    }

    private (List<int> Rows, List<int> Columns) FindRowsAndColumns(Map<bool> map)
    {
        var width = map.Width;
        var height = map.Height;

        var rows = new List<int>();
        var columns = new List<int>();

        // expand rows
        for (int row = 0; row < height; row++)
        {
            var founded = false;

            for (int i = 0; i < width; i++)
            {
                if (map[i, row])
                {
                    founded = true;
                    break;
                }
            }

            if (!founded) rows.Add(row);
        }

        // expand columns
        for (int column = 0; column < width; column++)
        {
            var founded = false;

            for (int i = 0; i < height; i++)
            {
                if (map[column, i])
                {
                    founded = true;
                    break;
                }
            }

            if (!founded) columns.Add(column);
        }

        return (rows, columns);
    }

    private long Expand(int i, List<int> list, int factor = 2)
    {
        return i + list.Count(x => x < i) * (factor - 1);
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

        // _map.MapBoolToString().Dump("map", true);
        // _expandedMap.MapBoolToString('#', '.').Dump("map", true);
    }
}

#if DUMP
#endif