#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

using Gguc.Aoc.Core.Models;

public class Day10 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 10;

    private List<string> _data;
    private Dictionary<Point, Point> _points;

    public Day10(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "FPRBRRZA";
    }

    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    #region Parse
    protected override void ProcessData()
    {
        _points = new();

        var pattern = @"position=<(?'x'[ \-\d]+),(?'y'[ \-\d]+)> velocity=<(?'vx'[ \-\d]+),(?'vy'[ \-\d]+)>";

        // Gromit do something!
        foreach (var line in _data)
        {
            var match = line.RegexMatch(pattern);
            if (match.Success)
            {
                var x = match.GroupValue("x").ToInt();
                var y = match.GroupValue("y").ToInt();
                var vx = match.GroupValue("vx").ToInt();
                var vy = match.GroupValue("vy").ToInt();

                var p1 = new Point(x, y);
                var p2 = new Point(vx, vy);

                _points[p1] = p2;
            }
        }
    }
    #endregion

    #region Head
    protected override void ComputePart1()
    {
        Result = FindNeighbors();
    }

    protected override void ComputePart2()
    {
        Log.Info($"Skip part 2 - solved in part 1!");
        Result = 0L;
    }
    #endregion

    #region Body
    private int FindNeighbors()
    {
        var step = 0;
        var min = long.MaxValue;
        var minat = 0;

        while (true)
        {
            var (x1, y1, product) = CountNeighbors(step);

            if (product < min)
            {
                min = product;
                minat = step;
            }
            else
            {
                break;
            }

            // if (step > 20000) break;

            step++;
        }

        Log.Debug($"min={min}  at={minat}");
        ShowAt(minat);

        return minat;
    }

    private (int, int, long) CountNeighbors(int step)
    {
        var xs = new SortedSet<int>();
        var ys = new SortedSet<int>();

        foreach (var (point, vector) in _points)
        {
            var x = point.X + vector.X * step;
            var y = point.Y + vector.Y * step;

            xs.Add(x);
            ys.Add(y);
        }

        var x1 = xs.First();
        var x2 = xs.Last();
        var y1 = ys.First();
        var y2 = ys.Last();

        var rect = new Rect(x1, y1, x2, y2);
        var md = new Point(x1, y1).ManhattanDistance(new Point(x2, y2));

        // Log.Debug($"step={step}  xs={xs.Count}  ys={ys.Count}  rect={rect}  md={md}");
        return (x1, y1, md);
    }

    private void ShowAt(int step)
    {
        var xs = new SortedSet<int>();
        var ys = new SortedSet<int>();
        var points = new List<Point>();

        foreach (var (point, vector) in _points)
        {
            var x = point.X + vector.X * step;
            var y = point.Y + vector.Y * step;

            xs.Add(x);
            ys.Add(y);
            points.Add(new Point(x, y));
        }

        var x1 = xs.First();
        var x2 = xs.Last();
        var y1 = ys.First();
        var y2 = ys.Last();

        var map = new Map<bool>(x2 - x1 + 1, y2 - y1 + 1);
        foreach (var point in points)
        {
            map[point.X - x1, point.Y - y1] = true;
        }

        Log.Info($"Message\n{map.MapBoolToString()}");
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
        // _points.DumpCollection();
    }
    #endregion
}

#if DUMP
#endif