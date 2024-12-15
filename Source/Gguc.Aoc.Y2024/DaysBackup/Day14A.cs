#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day14A : Day
{
    private const int YEAR = 2024;
    private const int DAY = 14;

    private List<string> _data;
    private List<Robot> _robots;
    private int _width;
    private int _height;

    private List<(int, int)> _finals;

    public Day14A(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "";
        Expected2 = "";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();

        // _width = 11;
        // _height = 7;

        _width = 101;
        _height = 103;


        _finals = new();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        _finals.Clear();

        Result = CalculateSafetyFactor();
    }

    protected override void ComputePart2()
    {
        Result = FindTree();
    }

    private long CalculateSafetyFactor()
    {
        var seconds = 100;

        foreach (var robot in _robots)
        {
            MoveRobot(robot, seconds);
        }

        return CalculateQuadrants();
    }

    private long FindTree()
    {
        var map0 = new Map<bool>(_width, _height);

        for (int i = 194; i < 10000; i+=101)
        {
            var map = map0.Clone();

            _finals.Clear();
            foreach (var robot in _robots)
            {
                MoveRobot(robot, i);
            }

            foreach (var r in _finals)
            {
                map[r.Item1, r.Item2] = true;
            }

            i.Dump("seconds");
            map.MapBoolToString().Dump("tree", true);
        }

        return 1L;
    }

    private void MoveRobot(Robot robot, int seconds)
    {
        var pX = (robot.X + robot.Vx * seconds) % _width;
        var pY = (robot.Y + robot.Vy * seconds) % _height;

        pX = pX >= 0 ? pX : _width + pX;
        pY = pY >= 0 ? pY : _height + pY;

        // $"{pX}::{pY}".Dump(); // --DUMP--

        _finals.Add((pX, pY));
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _robots = new();

        // Gromit do something!
        var pattern = @"p=(\d+),(\d+) v=(-?\d+),(-?\d+)";

        foreach (var line in _data)
        {
            var m = line.MatchAll(pattern).ToList();

            var r = new Robot();
            r.X = m[1].ToInt();
            r.Y = m[2].ToInt();
            r.Vx = m[3].ToInt();
            r.Vy = m[4].ToInt();

            _robots.Add(r);
        }
    }

    private long CalculateQuadrants()
    {
        var halfWidth = (_width - 1) / 2;
        var halfHeight = (_height - 1) / 2;

        var valids = _finals.Where(r => r.Item1 != halfWidth && r.Item2 != halfHeight);

        // valids.DumpCollection(); // --DUMP--

        var q1 = 0;
        var q2 = 0;
        var q3 = 0;
        var q4 = 0;

        foreach (var r in valids)
        {
            if (r.Item1 < halfWidth && r.Item2 < halfHeight) q1++;
            else if (r.Item1 > halfWidth && r.Item2 < halfHeight) q2++;
            else if (r.Item1 < halfWidth && r.Item2 > halfHeight) q3++;
            else q4++;
        }

        // $"{q1}::{q2}::{q3}::{q4}".Dump(); // --DUMP--

        return q1 * q2 * q3 * q4;
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
        // _robots.DumpCollection();
    }
}

#if DUMP
#endif