#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day09 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 9;

    private List<string> _data;
    private List<(Direction, int)> _instructions;
    private MovePoint _head;
    private MovePoint _tail;
    private Dictionary<(int, int), int> _trace;
    private List<MovePoint> _tails;

    public Day09(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "5883";
        Expected2 = "2367";
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
        ProcessInstructions();

        Result = _trace.Count;
    }

    protected override void ComputePart2()
    {
        ProcessInstructions2();

        Result = _trace.Count;
    }

    private void ProcessInstructions()
    {
        _head = new MovePoint(0, 0);
        _tail = new MovePoint(0, 0);
        _trace = new ();

        ReportTail(_tail);

        foreach (var (d, v) in _instructions)
        {
            ProcessOne(d, v);
        }
    }

    private void ProcessInstructions2()
    {
        _head = new MovePoint(0, 0);
        _tails = new List<MovePoint>();

        for (var i = 0; i < 9; i++)
        {
            _tails.Add(new MovePoint(0, 0));
        }

        _trace = new();
        ReportTail(_tails[8]);

        foreach (var (d, v) in _instructions)
        {
            ProcessOne2(d, v);
        }
    }

    private void ProcessOne(Direction d, int value)
    {
        for (var i = 0; i < value; i++)
        {
            switch (d)
            {
                case Direction.Up:
                    _head.Y++;
                    break;

                case Direction.Down:
                    _head.Y--;
                    break;

                case Direction.Left:
                    _head.X--;
                    break;

                case Direction.Right:
                    _head.X++;
                    break;
            }

            MoveTail(_head, _tail);
            ReportTail(_tail);
        }
    }

    private void ProcessOne2(Direction d, int value)
    {
        for (var i = 0; i < value; i++)
        {
            switch (d)
            {
                case Direction.Up:
                    _head.Y++;
                    break;

                case Direction.Down:
                    _head.Y--;
                    break;

                case Direction.Left:
                    _head.X--;
                    break;

                case Direction.Right:
                    _head.X++;
                    break;
            }

            MoveTail(_head, _tails[0]);

            for (var j = 1; j < 9; j++)
            {
                MoveTail(_tails[j - 1], _tails[j]);
            }

            ReportTail(_tails[8]);
        }
    }

    private void MoveTail(MovePoint head, MovePoint tail)
    {
        if (tail.X >= head.X - 1 && tail.X <= head.X + 1 && tail.Y >= head.Y - 1 && tail.Y <= head.Y + 1) return;

        if (tail.X == head.X)
        {
            tail.Y = (tail.Y < head.Y) ? tail.Y + 1 : tail.Y - 1;
        }
        else if (tail.Y == head.Y)
        {
            tail.X = (tail.X < head.X) ? tail.X + 1 : tail.X - 1;
        }
        else
        {
            tail.Y = (tail.Y < head.Y) ? tail.Y + 1 : tail.Y - 1;
            tail.X = (tail.X < head.X) ? tail.X + 1 : tail.X - 1;
        }
    }

    private void ReportTail(MovePoint tail)
    {
        if (!_trace.ContainsKey((tail.X, tail.Y))) _trace[(tail.X, tail.Y)] = 0;

        _trace[(tail.X, tail.Y)]++;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _instructions = new List<(Direction, int)>();
        foreach (var line in _data)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var direction = parts[0] switch
            {
                "U" => Direction.Up,
                "D" => Direction.Down,
                "L" => Direction.Left,
                "R" => Direction.Right,
                _ => Direction.None
            };

            _instructions.Add((direction, parts[1].ToInt()));
        }

        // _instructions.DumpCollection();
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