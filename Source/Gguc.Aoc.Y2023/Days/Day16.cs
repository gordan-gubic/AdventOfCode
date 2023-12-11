#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day16 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 16;

    private Map<char> _data;

    public Day16(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "7798";
        Expected2 = "8026";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.ParseMapChar();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = SumEnergized();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = FindMaxEnergized();

        Result = result;
    }

    private long FindMaxEnergized()
    {
        var max = 0L;

        // North
        for (var i = 0; i < _data.Width; i++)
        {
            var current = new Beam { X = i, Y = 0, Dir = 'S' };
            var map = EnergizeMap(current);
            var count = map.CountValues(true);
            max = Math.Max(max, count);
        }
        // South
        for (var i = 0; i < _data.Width; i++)
        {
            var current = new Beam { X = i, Y = _data.Width - 1, Dir = 'N' };
            var map = EnergizeMap(current);
            var count = map.CountValues(true);
            max = Math.Max(max, count);
        }
        // West
        for (var i = 0; i < _data.Height; i++)
        {
            var current = new Beam { X = 0, Y = i, Dir = 'E' };
            var map = EnergizeMap(current);
            var count = map.CountValues(true);
            max = Math.Max(max, count);
        }
        // East
        for (var i = 0; i < _data.Height; i++)
        {
            var current = new Beam { X = _data.Height - 1, Y = i, Dir = 'W' };
            var map = EnergizeMap(current);
            var count = map.CountValues(true);
            max = Math.Max(max, count);
        }

        return max;
    }

    private long SumEnergized()
    {
        var current = new Beam { Dir = 'E' };
        var map = EnergizeMap(current);

        return map.CountValues(true);
    }

    private Map<bool> EnergizeMap(Beam current)
    {
        var queue = new Queue<Beam>();
        var next = default((Beam b1, Beam b2));
        var map = new Map<bool>(_data.Width, _data.Height);
        var set = new HashSet<(int, int, char)>();

        queue.Enqueue(current);

        while (queue.Count > 0)
        {
            current = queue.Dequeue();

            if (!_data.Contains(current.X, current.Y)) continue;
            if (set.Contains((current.X, current.Y, current.Dir))) continue;

            map[current.X, current.Y] = true;
            set.Add((current.X, current.Y, current.Dir));

            next = ProcessBeam(map, current);

            if (next.b1 != null)
            {
                queue.Enqueue(next.b1);
            }

            if (next.b2 != null)
            {
                queue.Enqueue(next.b2);
            }
        }

        // map.MapBoolToString(falsech: '.').Dump("map", true);
        return map;
    }

    private (Beam b1, Beam b2) ProcessBeam(Map<bool> map, Beam current)
    {
        var b1 = default(Beam);
        var b2 = default(Beam);

        var x = current.X;
        var y = current.Y;

        var value = _data[x, y];
        var dir = current.Dir;

        if (value == '.'
            || (value == '-' && (dir == 'W' || dir == 'E'))
            || (value == '|' && (dir == 'N' || dir == 'S')))
        {
            var next = GetNextPoint(current);
            b1 = new Beam { X = next.x, Y = next.y, Dir = dir };
        }
        else if (value == '|' && (dir == 'W' || dir == 'E'))
        {
            b1 = new Beam { X = x, Y = y - 1, Dir = 'N' };
            b2 = new Beam { X = x, Y = y + 1, Dir = 'S' };
        }
        else if (value == '-' && (dir == 'N' || dir == 'S'))
        {
            b1 = new Beam { X = x - 1, Y = y, Dir = 'W' };
            b2 = new Beam { X = x + 1, Y = y, Dir = 'E' };
        }
        else if (value == '\\')
        {
            b1 = dir switch
            {
                'N' => new Beam { X = x - 1, Y = y, Dir = 'W' },
                'S' => new Beam { X = x + 1, Y = y, Dir = 'E' },
                'W' => new Beam { X = x, Y = y - 1, Dir = 'N' },
                'E' => new Beam { X = x, Y = y + 1, Dir = 'S' },
            };
        }
        else if (value == '/')
        {
            b1 = dir switch
            {
                'N' => new Beam { X = x + 1, Y = y, Dir = 'E' },
                'S' => new Beam { X = x - 1, Y = y, Dir = 'W' },
                'W' => new Beam { X = x, Y = y + 1, Dir = 'S' },
                'E' => new Beam { X = x, Y = y - 1, Dir = 'N' },
            };
        }

        return (b1, b2);
    }

    private (int x, int y) GetNextPoint(Beam current)
    {
        var x = current.X;
        var y = current.Y;

        return current.Dir switch
        {
            'N' => (x, y - 1),
            'S' => (x, y + 1),
            'W' => (x - 1, y),
            'E' => (x + 1, y),
            _ => (-1, -1)
        };
    }

    protected override void ProcessData()
    {
        base.ProcessData();


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

        // _data.MapValueToString().Dump("map", true);
    }
}

#if DUMP
#endif