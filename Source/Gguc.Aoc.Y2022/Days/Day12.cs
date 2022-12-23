#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day12 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 12;

    private List<string> _data;
    private int _width;
    private int _height;
    private Map<int> _map;
    private Map<int> _path;
    private Point _begin;
    private Point _end;
    private Queue<Point> _active;

    public Day12(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "412";
        Expected2 = "402";
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
        ProcessData();
        CalculatePath();

        Result = _path.GetValue(_end.X, _end.Y);
    }

    protected override void ComputePart2()
    {
        ProcessData();
        ActivateLowestPoints();
        CalculatePath();

        Result = _path.GetValue(_end.X, _end.Y);
    }

    private void CalculatePath()
    {
        while (_active.Count > 0)
        {
            ProcessPoint(_active.Dequeue());
        }

        // DumpMap(_path);
    }

    private void ProcessPoint(Point current)
    {
        var value = _path.GetValue(current.X, current.Y);
        var newValue = value + 1;

        var up = new Point(current.X, current.Y - 1);
        var down = new Point(current.X, current.Y + 1);
        var left = new Point(current.X - 1, current.Y);
        var right = new Point(current.X + 1, current.Y);

        if (Validate(current, up, newValue)) Activate(up, newValue);
        if (Validate(current, down, newValue)) Activate(down, newValue);
        if (Validate(current, left, newValue)) Activate(left, newValue);
        if (Validate(current, right, newValue)) Activate(right, newValue);
    }

    private bool Validate(Point current, Point newPoint, int newValue)
    {
        if (!_map.Contains(newPoint.X, newPoint.Y)) return false;

        if(_map.GetValue(newPoint.X, newPoint.Y) > _map.GetValue(current.X, current.Y) + 1) return false;

        var value = _path.GetValue(newPoint.X, newPoint.Y);
        if (value == -1 || value > newValue)
        {
            return true;
        }

        return false;
    }

    private void Activate(Point point, int value)
    {
        _active.Enqueue(point);

        _path[point.X, point.Y] = value;
    }

    private void ActivateLowestPoints()
    {
        _map.ForEach((x, y) =>
        {
            if (_map.GetValue(x, y) == 'a')
            {
                Activate(new Point(x, y), 0);
            }
        });
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _width = _data[0].Length;
        _height = _data.Count;
        _map = new Map<int>(_data, x => x);

        _begin = new Point();
        _end = new Point();

        _map.ForEach((x, y) =>
        {
            if (_map.GetValue(x, y) == 'S')
            {
                _begin = new Point(x, y);
                _map[x, y] = 'a';
            }

            if (_map.GetValue(x, y) == 'E')
            {
                _end = new Point(x, y);
                _map[x, y] = 'z';
            }
        });

        _path = new Map<int>(_width, _height, -1);
        _path[_begin.X, _begin.Y] = 0;

        _active = new Queue<Point>();
        _active.Enqueue(_begin);

        // DumpMap(_map);
        // _begin.Dump();
        // _end.Dump();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
    }

    [Conditional("LOG")]
    private void DumpMap(Map<int> map)
    {
        if (!Log.EnableDebug) return;

        MapValueToString2(map).Dump("Map", true);
    }

    public static string MapValueToString2(Map<int> map)
    {
        var sb = new StringBuilder();

        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                sb.Append($"{map[x, y],2},");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}

#if DUMP
#endif