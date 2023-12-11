#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.DaysBackup;

public class Day18_01 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 18_01;

    private List<string> _data;
    private List<Dig> _digs;

    public Day18_01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "";
        Expected2 = "";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

        _data = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = CalculateVolume();

        Result = result;
    }


    protected override void ComputePart2()
    {
        var result = 0L;

        Result = result;
    }
    private long CalculateVolume()
    {
        var sum = 0L;
        
        var points = new List<Point>();

        var x0 = int.MaxValue;
        var y0 = int.MaxValue;
        var x1 = 0;
        var y1 = 0;

        var current = new Point();
        var dir = 'N';
        points.Add(current);

        foreach (var dig in _digs)
        {
            dir = CalcDir(dig, dir);
            // current = CalcPoint(dig, current, dir);
            current = new Point();
            x0 = Math.Min(x0, current.X);
            y0 = Math.Min(y0, current.Y);
            x1 = Math.Max(x1, current.X);
            y1 = Math.Max(y1, current.Y);
            points.Add(current);
        }

        points.DumpCollection();

        CalculateArea(points);

        var map = new Map<bool>(x1 - x0 + 1, y1 - y0 + 1);

        // map[-x0, -y0] = true;

        for (var i = 0; i < points.Count - 1; i++)
        {
            var p1 = new Point(points[i].X - x0, points[i].Y - y0);
            var p2 = new Point(points[i + 1].X - x0, points[i + 1].Y - y0);

            DigMap(map, p1, p2);
        }

        // map.MapBoolToString(falsech: '.').Dump("dig-A", true);

        FillMap(map, x0, y0);

        map.MapBoolToString(falsech: '.').Dump("dig-B", true);

        return map.CountValues(x => x);
    }

    private void CalculateArea(List<Point> points)
    {
        /*
         *
         * area = 0;
for( i = 0; i < N; i += 2 )
   area += x[i+1]*(y[i+2]-y[i]) + y[i+1]*(x[i]-x[i+2]);
area /= 2;
         */

        var area = 0L;

        for (var i = 0; i < points.Count - 2; i += 2)
        {
            area += points[i + 1].X * (points[i + 2].Y - points[i].Y) + (points[i + 1].Y) * (points[i].X - points[i + 2].X);
        }

        area /= 2;

        Log.Dump($"Area: [{area}]");
    }

    private void DigMap(Map<bool> map, Point point1, Point point2)
    {
        if (point1.X != point2.X)
        {
            // dig east
            var y = point2.Y;
            var x0 = Math.Min(point1.X, point2.X);
            var x1 = Math.Max(point1.X, point2.X);

            for (var i = x0; i <= x1; i++)
            {
                map[i, y] = true;
            }
        }
        else
        {
            // dig south
            var x = point2.X;
            var y0 = Math.Min(point1.Y, point2.Y);
            var y1 = Math.Max(point1.Y, point2.Y);

            for (var i = y0; i <= y1; i++)
            {
                map[x, i] = true;
            }
        }
    }

    private void FillMap(Map<bool> map, int x, int y)
    {
        var queue = new Queue<Point>();
        var init = new Point(1 - x, 1 - y);
        queue.Enqueue(init);
        var count = 0;

        var cache = new HashSet<(int, int)>();
        map.FindAll(true).ForEach(p => { cache.Add((p.Item1, p.Item2)); });

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            map[current.X, current.Y] = true;

            var candidates = CreateCandidates(map, current, cache);
            candidates.ForEach(c => queue.Enqueue(c));

            // if (!map[current.X - 1, current.Y]) queue.Enqueue(new Point(current.X - 1, current.Y));
            // if (!map[current.X + 1, current.Y]) queue.Enqueue(new Point(current.X + 1, current.Y));
            // if (!map[current.X, current.Y - 1]) queue.Enqueue(new Point(current.X, current.Y - 1));
            // if (!map[current.X, current.Y + 1]) queue.Enqueue(new Point(current.X, current.Y + 1));

            count++;

            // if(count >= 10000) break;
        }
    }

    private List<Point> CreateCandidates(Map<bool> map, Point current, HashSet<(int, int)> cache)
    {
        var candidates = new List<Point>();

        var temp = new List<Point>()
        {
            current with {X = current.X - 1},
            current with {X = current.X + 1},
            current with {Y = current.Y - 1},
            current with {Y = current.Y + 1},
        };

        foreach (var point in temp)
        {
            // if(!map.Contains(point.X, point.Y) || map[point.X, point.Y]) continue;
            if (cache.Contains((point.X, point.Y))) continue;
            cache.Add((point.X, point.Y));

            candidates.Add(point);
        }

        return candidates;
    }

    private char CalcDir(Dig dig, char dir)
    {
        return (dig.Dir) switch
        {
            ('U') => 'N',
            ('D') => 'S',
            ('L') => 'W',
            ('R') => 'E',
            _ => 'N',
        };
    }

    private PointLong CalcPoint(Dig dig, PointLong current, char dir)
    {
        var point = new PointLong();
        var count = dig.Count;

        if (dir == 'N') point = current with { Y = current.Y - count };
        if (dir == 'S') point = current with { Y = current.Y + count };
        if (dir == 'W') point = current with { X = current.X - count };
        if (dir == 'E') point = current with { X = current.X + count };

        return point;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _digs = new();

        foreach (var line in _data)
        {
            var parts = line.Split(' ');
            var dig = new Dig
            {
                Dir = parts[0][0],
                Count = parts[1].ToInt(),
                Color = parts[2].Trim('(', ')'),
            };
            _digs.Add(dig);
        }
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

        // _digs.DumpJson();
    }
}

#if DUMP
        return (dir, dig.Dir) switch
        {
            (_, 'U') => 'N',
            (_, 'D') => 'S',
            ('N', 'L') => 'W',
            ('N', 'R') => 'E',
            ('S', 'L') => 'E',
            ('S', 'R') => 'W',
            _ => 'N',
        };

#endif