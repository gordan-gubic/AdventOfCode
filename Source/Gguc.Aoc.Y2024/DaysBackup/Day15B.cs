#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day15B : Day
{
    private const int YEAR = 2024;
    private const int DAY = 15;

    private List<string> _data;
    private Map<char> _map;
    private Map<bool> _mapWalls;
    private Map<Box> _mapBoxes;
    private List<char> _instructions;
    private HashSet<Box> _boxes;

    public Day15B(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        parser.Day = 15;

        EnableDebug();
        Initialize();

        Expected1 = "1426855";
        Expected2 = "1404917";
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
        Result = 0L;
    }

    protected override void ComputePart2()
    {
        Result = CalculateCoordinates();
    }

    private long CalculateCoordinates()
    {
        var (z, x, y) = _map.Find('@');
        var point = new Point(x, y);

        foreach (var instruction in _instructions)
        {
            point = ProcessInstruction(instruction, point);

            // instruction.Dump("instruction");
            // DumpBoxes();
        }

        DumpBoxes();

        var sum = 0L;

        _boxes.ForEach((b) =>
        {
            sum += b.Point1.X + b.Point1.Y * 100;
        });

        return sum;
    }

    private Point ProcessInstruction(char instruction, Point point)
    {
        var dir = instruction.SignToDirection();
        
        var x = point.X + dir.Item1;
        var y = point.Y + dir.Item2;
        var next = new Point(x, y);

        var wall = _mapWalls.GetValue(x, y);
        var box = _mapBoxes.GetValue(x, y);

        if (wall) return point;
        if (box == null) return next;

        var moveBoxes = new HashSet<Box>();
        var queue = new Queue<Box>();
        queue.Enqueue(box);

        var isFree = FindBoxes(moveBoxes, queue, dir);
        if (isFree)
        {
            MoveBoxes(moveBoxes, dir);
            return next;
        }

        return point;
    }

    private bool FindBoxes(HashSet<Box> moveBoxes, Queue<Box> queue, (int, int) dir)
    {
        while (queue.Count > 0)
        {
            var box = queue.Dequeue();
            moveBoxes.Add(box);

            var x1 = box.Point1.X + dir.Item1;
            var x2 = box.Point2.X + dir.Item1;

            var y1 = box.Point1.Y + dir.Item2;
            var y2 = box.Point2.Y + dir.Item2;

            var w1 = _mapWalls[x1, y1];
            var w2 = _mapWalls[x2, y2];

            var b1 = _mapBoxes[x1, y1];
            var b2 = _mapBoxes[x2, y2];

            if (w1 || w2) return false;
            if ((b1 == null || b1 == box) && (b2 == null || b2 == box)) continue;
            if (b1 != null && b1 != box) queue.Enqueue(b1);
            if (b2 != null && b2 != box) queue.Enqueue(b2);
        }

        return true;
    }

    private void MoveBoxes(HashSet<Box> moveBoxes, (int, int) dir)
    {
        foreach (var box in moveBoxes)
        {
            _mapBoxes[box.Point1.X, box.Point1.Y] = null;
            _mapBoxes[box.Point2.X, box.Point2.Y] = null;
        }

        foreach (var box in moveBoxes)
        {
            var x1 = box.Point1.X + dir.Item1;
            var x2 = box.Point2.X + dir.Item1;

            var y1 = box.Point1.Y + dir.Item2;
            var y2 = box.Point2.Y + dir.Item2;

            box.Point1 = new Point(x1, y1);
            box.Point2 = new Point(x2, y2);

            _mapBoxes[x1, y1] = box;
            _mapBoxes[x2, y2] = box;
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

        var map = new Map<char>(mapLines, x => x);

        _map = new Map<char>(map.Width * 2, map.Height);
        _mapWalls = new Map<bool>(map.Width * 2, map.Height);
        _mapBoxes = new Map<Box>(map.Width * 2, map.Height);
        _boxes = new();

        map.ForEach((x, y, value) =>
        {
            if (value == '#')
            {
                _map[x * 2, y] = '#';
                _map[x * 2 + 1, y] = '#';

                _mapWalls[x * 2, y] = true;
                _mapWalls[x * 2 + 1, y] = true;
            }
            else if (value == 'O')
            {
                _map[x * 2, y] = '[';
                _map[x * 2 + 1, y] = ']';

                var box = new Box { Point1 = new Point(x * 2, y), Point2 = new Point(x * 2 + 1, y) };
                _boxes.Add(box);
                _mapBoxes[x * 2, y] = box;
                _mapBoxes[x * 2 + 1, y] = box;
            }
            else if (value == '.')
            {
                _map[x * 2, y] = '.';
                _map[x * 2 + 1, y] = '.';
            }
            else if (value == '@')
            {
                _map[x * 2, y] = '@';
                _map[x * 2 + 1, y] = '.';
            }
        });

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
        _mapWalls.MapBoolToString().Dump("_walls", true);
        // _boxes.MapValueToString().Dump("_containers", true);
        DumpBoxes();
        // _instructions.DumpCollection();
    }

    private void DumpBoxes()
    {
        var map = _mapBoxes;
        var sb = new StringBuilder();

        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                var value = map[x, y];

                if(value == null) sb.Append('.');
                else sb.Append('O');
            }
            sb.AppendLine();
        }

        sb.ToString().Dump("boxes", true);
    }
}

#if DUMP
#endif