#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day15 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 15;

    private List<string> _data;
    private List<Point> _sensors;
    private List<Point> _beacons;
    private HashSet<Point> _beaconsDistinct;

    private int _target1 = 0;
    private int _target2 = 0;

    public Day15(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "5256611";
        Expected2 = "13337919186981";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        InitTest();
        InitReal();

        _data = Parser.Parse();
    }

    private void InitTest()
    {
        Parser.Type = ParserFileType.Test;

        _target1 = 10;
        _target2 = 20;
    }

    private void InitReal()
    {
        Parser.Type = ParserFileType.Real;

        _target1 = 2000000;
        _target2 = 4000000;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = CalculateSensorsCovering1(_target1);
    }

    protected override void ComputePart2()
    {
        Result = FindDistressBeacon(0, _target2);
    }

    private long FindDistressBeacon(int min, int max)
    {
        for (var i = min; i <= max; i++)
        {
            var (ok, x, y) = CalculateSensorsCovering2(i);
            if (ok)
            {
                return 4000000L * x + y;
            }
        }

        return -1;
    }

    private long CalculateSensorsCovering1(int row)
    {
        var minX = long.MaxValue;
        var maxX = long.MinValue;

        for (var i = 0; i < _sensors.Count; i++)
        {
            var sensor = _sensors[i];
            var beacon = _beacons[i];

            var md = sensor.ManhattanDistance(beacon);
            var rowDelta = md - Math.Abs(sensor.Y - row);

            var top = sensor.Y - md;
            var bottom = sensor.Y + md;

            if (row < top || row > bottom) continue;

            minX = Math.Min(minX, sensor.X - rowDelta);
            maxX = Math.Max(maxX, sensor.X + rowDelta);
        }

        var total1 = maxX - minX + 1;
        var total2 = total1;

        foreach (var beacon in _beaconsDistinct)
        {
            if (beacon.Y != row || beacon.X < minX || beacon.X > maxX) continue;
            total2--;
        }

        Log.Debug($"Min=[{minX}]. Max=[{maxX}]. Total1=[{total1}]. Total2=[{total2}]");

        return total2;
    }

    private (bool, int, int) CalculateSensorsCovering2(int row)
    {
        var cover = new List<Point>();

        for (var i = 0; i < _sensors.Count; i++)
        {
            var sensor = _sensors[i];
            var beacon = _beacons[i];

            var md = sensor.ManhattanDistance(beacon);
            var rowDelta = md - Math.Abs(sensor.Y - row);

            var top = sensor.Y - md;
            var bottom = sensor.Y + md;

            if (row < top || row > bottom) continue;

            var x1 = sensor.X - rowDelta;
            var x2 = sensor.X + rowDelta;

            cover.Add(new Point((int)x1, (int)x2));
        }

        cover = cover.OrderBy(x => x.X).ToList();
        // cover.DumpCollection();
     
        var (ok, column) = FindGap(cover);

        if (ok)
        {
            Log.Debug($"Row=[{row}]. Column=[{column}]");
            return (true, column, row);
        }

        return (false, -1, -1);
    }

    private (bool, int) FindGap(List<Point> cover)
    {
        var current = cover[0].Y;

        for (var i = 1; i < cover.Count; i++)
        {
            if (cover[i].X <= current + 1) current = Math.Max(current, cover[i].Y);
            else return (true, current + 1);
        }

        return (false, -1);
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _sensors = new List<Point>();
        _beacons = new List<Point>();
        _beaconsDistinct = new HashSet<Point>();

        var pattern = @"Sensor at x=(?'x1'[\-\d]+), y=(?'y1'[\-\d]+): closest beacon is at x=(?'x2'[\-\d]+), y=(?'y2'[\-\d]+)";

        foreach (var line in _data)
        {
            foreach (Match m in Regex.Matches(line, pattern))
            {
                var x1 = m.GroupValue("x1").ToInt();
                var x2 = m.GroupValue("x2").ToInt();
                var y1 = m.GroupValue("y1").ToInt();
                var y2 = m.GroupValue("y2").ToInt();

                _sensors.Add(new Point(x1, y1));
                _beacons.Add(new Point(x2, y2));
                _beaconsDistinct.Add(new Point(x2, y2));
            }
        }

        // _sensors.DumpCollection();
        // _beacons.DumpCollection();
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
    private long CalculateSensorsCoveringFull(int row)
    {
        var cover = new List<Point>();

        for (var i = 0; i < _sensors.Count; i++)
        {
            var sensor = _sensors[i];
            var beacon = _beacons[i];

            var md = sensor.ManhattanDistance(beacon);
            var hd = Math.Abs(sensor.Y - beacon.Y);
            var wd = Math.Abs(sensor.X - beacon.X);
            var deltaW = md - hd;
            var deltaH = md - wd;
            var rowDelta = md - Math.Abs(sensor.Y - row);

            var top = sensor.Y - md;
            var bottom = sensor.Y + md;
            var left = sensor.X - md;
            var right = sensor.X + md;

            if (row < top || row > bottom)
            {
                Log.Debug($"sensor=[{sensor}]. beacon=[{beacon}]. md=[{md}]. row=[{row}]. rowDelta=[{rowDelta}]. top=[{top}]. bottom=[{bottom}]. left=[{left}]. right=[{right}] -- removed");
                continue;
            }

            var x1 = sensor.X - rowDelta;
            var x2 = sensor.X + rowDelta;

            Log.Debug($"sensor=[{sensor}]. beacon=[{beacon}]. md=[{md}]. row=[{row}]. rowDelta=[{rowDelta}]. top=[{top}]. bottom=[{bottom}]. left=[{left}]. right=[{right}] -- Added. x1=[{x1}]. x2=[{x2}]");

            cover.Add(new Point((int)x1, (int)x2));
        }

        cover = cover.OrderBy(x => x.X).ToList();

        cover.DumpCollection();

        var minX = cover.Min(x => x.X);
        var maxX = cover.Max(x => x.Y);

        var total1 = maxX - minX + 1;
        var total2 = total1;

        foreach (var beacon in _beaconsDistinct)
        {
            if (beacon.Y != row || beacon.X < minX || beacon.X > maxX)
            {
                continue;
            }

            Log.Debug($"reduce beacon=[{beacon}]...");
            total2--;
        }

        // total2 = Math.Abs(

        Log.Debug($"Min=[{minX}]. Max=[{maxX}]. Total1=[{total1}]. Total2=[{total2}]");

        return total2;
    }
#endif