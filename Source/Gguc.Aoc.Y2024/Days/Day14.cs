#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day14 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 14;

    private List<string> _data;
    private List<Robot> _robots;
    private int _width;
    private int _height;

    public Day14(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "220971520";
        Expected2 = "6355";
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
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = CalculateSafetyFactor();
    }

    protected override void ComputePart2()
    {
        Result = FindTree();
    }

    private long CalculateSafetyFactor()
    {
        var seconds = 100;
        var finals = new List<(int, int)>();

        foreach (var robot in _robots)
        {
            var (x, y) = MoveRobot(robot, seconds);
            finals.Add((x, y));
        }

        return CalculateQuadrants(finals);
    }

    private long FindTree()
    {
        var seconds = 0;
        var lines = new Dictionary<int, List<int>>();

        for (var i = 0; i < 100000; i++)
        {
            // if(i % 1000 == 0) i.Dump();

            lines.Clear();

            foreach (var robot in _robots)
            {
                var (x, y) = MoveRobot(robot, i);

                if (!lines.ContainsKey(y)) lines[y] = new();
                lines[y].Add(x);
            }

            var match = DetectLine(lines);
            if(!match) continue;

            var map = new Map<bool>(_width, _height);
            foreach (var robot in _robots)
            {
                var (x, y) = MoveRobot(robot, i);
                map[x, y] = true;
            }

            i.Dump("seconds");
            map.MapBoolToString().Dump("tree", true);
            seconds = i;
            break;
        }

        return seconds;
    }

    private long FindTreeManually()
    {
        var map0 = new Map<bool>(_width, _height);

        for (var i = 194; i < 10000; i += 101)
        {
            var map = map0.Clone();

            foreach (var robot in _robots)
            {
                var (x, y) = MoveRobot(robot, i);
                map[x, y] = true;
            }

            i.Dump("seconds");
            map.MapBoolToString().Dump("tree", true);
        }

        return 1L;
    }

    private bool DetectLine(Dictionary<int, List<int>> lines, int target = 20)
    {
        var count = 0;
        var max = 0;

        foreach (var line in lines.Values.Where(l => l.Count >= target))
        {
            line.Sort();

            for (var i = 1; i < line.Count; i++)
            {
                if (line[i] - line[i - 1] == 1) count++;
                else
                {
                    max = Math.Max(count, max);
                    count = 0;
                }
            }

            max = Math.Max(count, max);
            count = 0;
        }

        return max >= target;
    }

    private (int, int) MoveRobot(Robot robot, int seconds)
    {
        var pX = (robot.X + robot.Vx * seconds) % _width;
        var pY = (robot.Y + robot.Vy * seconds) % _height;

        pX = pX >= 0 ? pX : _width + pX;
        pY = pY >= 0 ? pY : _height + pY;

        // $"{pX}::{pY}".Dump(); // --DUMP--

        return (pX, pY);
    }

    private long CalculateQuadrants(List<(int, int)> finals)
    {
        var halfWidth = (_width - 1) / 2;
        var halfHeight = (_height - 1) / 2;

        var valids = finals.Where(r => r.Item1 != halfWidth && r.Item2 != halfHeight);

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