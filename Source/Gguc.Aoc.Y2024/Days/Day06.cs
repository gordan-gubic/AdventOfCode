#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day06 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 6;

    private List<string> _data;
    private Map<char> _map;
    private Map<bool> _walls;
    private int _height;
    private int _width;
    private HashSet<(int, int)> _steps;

    public Day06(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "4602";
        ExpectedProd2 = "1703";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
        _map = Parser.ParseMapChar();
        _walls = Parser.ParseMapBool();
        _height = _map.Height;
        _width = _map.Width;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = CountSteps();
    }

    protected override void ComputePart2()
    {
        Result = CountLoops();
    }

    private long CountSteps()
    {
        var guardMark = _map.Find('^');
        var guard = (X: guardMark.Item2, Y: guardMark.Item3);

        _steps = new HashSet<(int, int)> { guard };
        Walk(_steps, guard);

        return _steps.Count;
    }

    private long CountLoops()
    {
        var sum = 0L;

        var guardMark = _map.Find('^');
        var guard = (X: guardMark.Item2, Y: guardMark.Item3);

        _steps.Remove(guard);

        foreach (var step in _steps)
        {
            var walls = _walls.Clone();
            walls[step.Item1, step.Item2] = true;

            var isLoop = FindLoop(guard, walls);
            if (isLoop) sum++;
        }

        return sum;
    }

    private void Walk(HashSet<(int, int)> steps, (int X, int Y) guard)
    {
        var dir = 0;

        while (true)
        {
            var dc = dir.DegreeToDirection();
            var next = (guard.X + dc.Item1, guard.Y + dc.Item2);
            var isWall = _walls.GetValue(next.Item1, next.Item2);

            if (isWall)
            {
                dir = (dir + 90) % 360;
                dc = dir.DegreeToDirection();
                continue;
            }

            guard = (guard.X + dc.Item1, guard.Y + dc.Item2);
            
            if(guard.X < 0 || guard.X >= _width || guard.Y < 0 || guard.Y >= _height) return;
            steps.Add(guard);
        }
    }

    private bool FindLoop((int X, int Y) guard, Map<bool> walls)
    {
        var dir = 0;

        var steps = new HashSet<(int, int, int)> { (guard.X, guard.Y, dir) };

        while (true)
        {
            var dc = dir.DegreeToDirection();
            var next = (guard.X + dc.Item1, guard.Y + dc.Item2);
            var isWall = walls.GetValue(next.Item1, next.Item2);

            if (isWall)
            {
                dir = (dir + 90) % 360;
                dc = dir.DegreeToDirection();
                continue;
            }

            guard = (guard.X + dc.Item1, guard.Y + dc.Item2);
            
            if (guard.X < 0 || guard.X >= _width || guard.Y < 0 || guard.Y >= _height) return false;

            var step = (guard.X, guard.Y, dir);
            if (steps.Contains(step)) return true;
            steps.Add(step);
        }
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        // Gromit do something!
        foreach (var line in _data)
        {
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

        // _map.Dump("Map", true);
        // _map.MapValueToString().Dump("map", true);
        // _walls.MapBoolToString().Dump("map", true);
    }
}

#if DUMP
#endif