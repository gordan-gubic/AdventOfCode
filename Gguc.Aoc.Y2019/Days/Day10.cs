#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2019.Days;

public class Day10 : Day
{
    private Map<bool> _source;
    private Map<int> _visible;
    private List<Point> _allMeteors;

    public Day10(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2019;
        Parser.Day = 10;
        Parser.Type = ParserFileType.Real;

        _source = Parser.ParseMapBool();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _visible = new Map<int>(_source.Width, _source.Height);
        _allMeteors = ToList(_source);
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var (max, maxPoint) = ComputeVisible();
        Result = max;

        Info($"Max: {max}, at: ({maxPoint})");
    }

    protected override void ComputePart2()
    {
        var (max, origin) = ComputeVisible();
        Info($"Max: {max}, at: ({origin})");

        var meteorX = _ComputePart2(origin, 199);
        Result = meteorX;
    }

    private (int, Point) ComputeVisible()
    {
        ProcessVisible(_allMeteors);

        _visible.Values.DumpMap("Visible", 3);

        return MapMax(_visible);
    }

    private (int, Point) MapMax(Map<int> map)
    {
        var max = 0;
        var maxPoint = new Point(0, 0);

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                if (map.GetValue(x, y) > max)
                {
                    max = map.GetValue(x, y);
                    maxPoint = new Point(x, y);
                }
            }
        }

        return (max, maxPoint);
    }

    private void ProcessVisible(List<Point> points)
    {
        foreach (var point in points)
        {
            var visible = CountVisible(point, points);
            _visible.Values[point.X, point.Y] = visible;
        }
    }

    private int CountVisible(Point origin, List<Point> points)
    {
        var visible = new HashSet<Point>();

        foreach (var point in points)
        {
            if (point == origin) continue;

            var p = new Point(point.X - origin.X, point.Y - origin.Y);
            visible.Add(Normalize(p));
        }

        // Debug($"Point: {origin}, Visible: {visible.ToJson()}");
        return visible.Count;
    }

    private int _ComputePart2(Point origin, int target)
    {
        var meteors = FindSurroundingMeteors(origin, _allMeteors);

        // meteors.DumpJson("meteors");

        var range = FindRange(origin, meteors);

        // range.DumpJson("range");

        var shot = ShootMeteors(origin, meteors, range);

        // Print(shot);

        var x = shot[target].X * 100 + shot[target].Y;
        return x;
    }

    private Dictionary<int, List<MeteorRecord>> FindSurroundingMeteors(Point origin, List<Point> points)
    {
        var meteors = new Dictionary<int, List<MeteorRecord>>();

        foreach (var point in points)
        {
            if (point == origin) continue;

            var p = new Point(point.X - origin.X, point.Y - origin.Y);
            var n = Normalize(p);
            var a = Angle(n);
            var k = (int)(a * 1000);

            var mr = new MeteorRecord
            {
                Absolute = point,
                Point = p,
                Normalized = n,
                Distance = p.ManhattanDistance(),
                Angle = a,
            };

            if (!meteors.ContainsKey(k)) meteors[k] = new List<MeteorRecord>();
            meteors[k].Add(mr);
        }

        return meteors;
    }

    private List<int> FindRange(Point origin, Dictionary<int, List<MeteorRecord>> meteors)
    {
        var list = new List<int>(meteors.Keys.OrderBy(x => x));
        return list;
    }

    private List<Point> ShootMeteors(Point origin, Dictionary<int, List<MeteorRecord>> meteors, List<int> range)
    {
        var list = new List<Point>();

        while (meteors.Count > 0)
        {
            foreach (var point in range)
            {
                if (!meteors.ContainsKey(point)) continue;

                var min = meteors[point].Min(x => x.Distance);
                var first = meteors[point].FirstOrDefault(x => x.Distance == min);
                list.Add(first.Absolute);
                meteors[point].Remove(first);

                if (meteors[point].Count == 0) meteors.Remove(point);
            }
        }

        return list;
    }

    private Point Normalize(Point point)
    {
        if (point.X == 0 && point.Y == 0) return new Point(0, 0);
        if (point.X == 0) return new Point(0, point.Y / Math.Abs(point.Y));
        if (point.Y == 0) return new Point(point.X / Math.Abs(point.X), 0);

        var gcd = Gcd(Math.Abs(point.X), Math.Abs(point.Y));

        var x = point.X / gcd;
        var y = point.Y / gcd;

        return new Point(x, y);
    }

    public int Gcd(int a, int b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }

        return a;
    }

    public double Angle(Point point)
    {
        var r1 = Math.Atan2(point.X, -point.Y);
        var r2 = ConvertRadiansToDegrees(r1);

        return r2;
    }

    public static double ConvertRadiansToDegrees_1(double radians)
    {
        double degrees = (180 / Math.PI) * radians;
        return (degrees);
    }

    public static double ConvertRadiansToDegrees(double radians)
    {
        double degrees = (180 / Math.PI) * radians;
        if (degrees < 0) degrees += 360;
        return (degrees);
    }

    public List<Point> ToList(Map<bool> map)
    {
        var list = new List<Point>();

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                if (map.GetValue(x, y)) list.Add(new Point(x, y));
            }
        }

        return list;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data[0].Dump("Item");
        _source.Values.DumpMap("Map");
    }

    private void Print<T>(List<T> shot)
    {
        var i = 0;
        foreach (var s in shot)
        {
            Console.WriteLine($"{++i}: {s}");
        }
    }
}

#if DUMP
            /*
             * .#..##.###...#######
             * ##.############..##.
             * .#.######.########.#
             * .###.#######.####.#.
             * #####.##.#.##.###.##
             * ..#####..#.#########
             * ####################
             * #.####....###.#.#.##
             * ##.#################
             * #####.##.###..####..
             * ..######..##.#######
             * ####.##.####...##..#
             * .#####..#.######.###
             * ##...#.##########...
             * #.##########.#######
             * .####.#.###.###.#.##
             * ....##.##.###..#####
             * .#.#.###########.###
             * #.#.#.#####.####.###
             * ###.##.####.##.#..##
             *
             * The 1st asteroid to be vaporized is at 11,12.
             * The 2nd asteroid to be vaporized is at 12,1.
             * The 3rd asteroid to be vaporized is at 12,2.
             * The 10th asteroid to be vaporized is at 12,8.
             * The 20th asteroid to be vaporized is at 16,0.
             * The 50th asteroid to be vaporized is at 16,9.
             * The 100th asteroid to be vaporized is at 10,16.
             * The 199th asteroid to be vaporized is at 9,6.
             * The 200th asteroid to be vaporized is at 8,2.
             * The 201st asteroid to be vaporized is at 10,9.
             * The 299th and final asteroid to be vaporized is at 11,1.shot[0].Dump();
             * 
             * shot[0].Dump();
             * shot[1].Dump();
             * shot[2].Dump();
             * shot[9].Dump();
             * shot[19].Dump();
             * shot[49].Dump();
             * shot[99].Dump();
             * shot[198].Dump();
             * shot[199].Dump();
             * shot[200].Dump();
             * shot[298].Dump();
             *
             */
#endif