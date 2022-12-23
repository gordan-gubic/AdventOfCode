#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day14 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 14;

    private List<string> _data;
    private List<List<Point>> _points;
    private Map<bool> _map;

    public Day14(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "763";
        Expected2 = "23921";
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
        Result = DropSands(_map, 500);
    }

    protected override void ComputePart2()
    {
        var map = _map.Clone();

        for (int i = 0; i < map.Width; i++)
        {
            map[i, map.Height - 1] = true;
        }

        // Log.Debug($"Map:\n{map.MapBoolToString()}\n");

        Result = DropSands(map, 500);
    }

    private int DropSands(Map<bool> map, int x)
    {
        var total = 0;
        var result = false;

        map = map.Clone();
        var sands = new Map<bool>(map.Width, map.Height);
        var init = new Point(x, 0);

        do
        {
            result = DropSand(map, sands, init);

            if (result) total++;
        } while (result);

        // Log.Debug($"Map:\n{sands.MapBoolToString()}\n");

        return total;
    }

    private bool DropSand(Map<bool> map, Map<bool> sands, Point current)
    {
        while (true)
        {
            current = new Point(current.X, current.Y + 1);

            if (sands[current.X, current.Y])
            {
                return false;
            }

            if ( current.Y >= map.Height - 1 )
            {
                return false;
            }

            var (t1, t2, t3) = GetBelow(map, current);

            if (t1 && t2 && t3)
            {
                map[current.X, current.Y] = true;
                sands[current.X, current.Y] = true;
                return true;
            }

            if(!t2) current = new Point(current.X, current.Y);
            else if(!t1) current = new Point(current.X - 1, current.Y );
            else current = new Point(current.X + 1, current.Y);
        }
    }

    private (bool, bool, bool) GetBelow(Map<bool> map, Point current)
    {
        var x = current.X;
        var y = current.Y + 1;

        var t1 = map[x - 1, y];
        var t2 = map[x, y];
        var t3 = map[x + 1, y];

        return (t1, t2, t3);
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _points = new();
        var width = 0;
        var height = 0;

        foreach (var line in _data)
        {
            var parts = line.Split(new[] { ' ', '-', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);

            var points = new List<Point>();
            for (int i = 0; i < parts.Length; i += 2)
            {
                var x = parts[i].ToInt();
                var y = parts[i + 1].ToInt() + 1;

                width = Math.Max(width, x);
                height = Math.Max(height, y);

                points.Add(new Point(x, y));
            }

            _points.Add(points);
        }

        // _points.DumpJson();

        // Draw map
        width *= 2;
        height += 3;

        _map = new Map<bool>(width, height);

        foreach (var points in _points)
        {
            var current = points[0];
            _map[current.X, current.Y] = true;

            foreach (var point in points)
            {
                var diff = Math.Abs(current.X - point.X) + Math.Abs(current.Y - point.Y);

                if (diff == 0) continue;

                if (current.X > point.X) DrawLeft(_map, current, diff);
                else if (current.X < point.X) DrawRight(_map, current, diff);
                else if (current.Y > point.Y) DrawDown(_map, current, diff);
                else if (current.Y < point.Y) DrawUp(_map, current, diff);

                current = point;
            }
        }

        // Log.Debug($"Map:\n{_map.MapBoolToString()}\n");
    }

    private void DrawLeft(Map<bool> map, Point current, int count)
    {
        for (var i = 1; i <= count; i++) map[current.X - i, current.Y] = true;
    }

    private void DrawRight(Map<bool> map, Point current, int count)
    {
        for (var i = 1; i <= count; i++) map[current.X + i, current.Y] = true;
    }

    private void DrawDown(Map<bool> map, Point current, int count)
    {
        for (var i = 1; i <= count; i++) map[current.X, current.Y - i] = true;
    }

    private void DrawUp(Map<bool> map, Point current, int count)
    {
        for (var i = 1; i <= count; i++) map[current.X, current.Y + i] = true;
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
#endif