#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day15A : Day
{
    private const int YEAR = 2024;
    private const int DAY = 15;

    private List<string> _data;
    private Map<char> _map;
    private Map<bool> _walls;
    private Map<bool> _containers;
    private List<char> _instructions;

    public Day15A(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "1426855";
        Expected2 = "";
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
        Result = CalculateCoordinates();
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        Result = result;
    }

    private long CalculateCoordinates()
    {
        var (z, x, y) = _map.Find('@');
        var point = new Point(x, y);
        // robot.Dump("robot");

        foreach (var instruction in _instructions)
        {
            point = ProcessInstruction(instruction, point);
        }

        // _containers.MapBoolToString('O', '.').Dump("_containers", true);

        var sum = 0L;

        var items = _containers.FindAll(true);
        items.ForEach((i) => sum += i.Item1 + i.Item2 * 100);

        return sum;
    }

    private Point ProcessInstruction(char instruction, Point point)
    {
        var dir = instruction.SignToDirection();
        
        var x = point.X + dir.Item1;
        var y = point.Y + dir.Item2;
        var next = new Point(x, y);

        var wall = _walls.GetValue(x, y);
        var item = _containers.GetValue(x, y);

        if (wall) return point;
        if (!item) return next;

        var i = 1;
        while (true)
        {
            var x2 = point.X + dir.Item1 * i;
            var y2 = point.Y + dir.Item2 * i;

            wall = _walls.GetValue(x2, y2);
            item = _containers.GetValue(x2, y2);

            if (wall) return point;

            if (!item)
            {
                // move container
                _containers[x2, y2] = true;
                _containers[x, y] = false;
                return next;
            }

            i++;
        }
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var mapLines = new List<string>();
        var instructionLines = new List<string>();
        var isMap = true;

        // Gromit do something!
        foreach (var line in _data)
        {
            if (line.IsWhitespace())
            {
                isMap = false;
                continue;
            }

            if(isMap) mapLines.Add(line);
            else instructionLines.Add(line);
        }

        _map = new Map<char>(mapLines, x => x);
        _walls = new Map<bool>(mapLines, x => x == '#');
        _containers = new Map<bool>(mapLines, x => x == 'O');

        _instructions = new List<char>();
        foreach (var line in instructionLines)
        {
            _instructions.AddRange(line.ToCharArray());
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

        _map.MapValueToString().Dump("map", true);
        _walls.MapBoolToString().Dump("_walls", true);
        _containers.MapBoolToString().Dump("_containers", true);
        // _instructions.DumpCollection();
    }
}

#if DUMP
#endif