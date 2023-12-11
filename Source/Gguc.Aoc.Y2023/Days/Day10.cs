#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day10 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 10;

    private List<string> _data;
    private Map<char> _map;
    private int _width;
    private int _height;
    private Point _start;
    private List<Point> _points;

    private readonly char[] _allowedNorth = new[] { '|', '7', 'F' };
    private readonly char[] _allowedSouth = new[] { '|', 'J', 'L' };
    private readonly char[] _allowedWest = new[] { '-', 'L', 'F' };
    private readonly char[] _allowedEast = new[] { '-', 'J', '7' };

    public Day10(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "6773";
        Expected2 = "493";
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
        var result = FindDistance();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = CountEnclosed();

        Result = result;
    }

    private long FindDistance()
    {
        _start.Dump();

        var list = new List<MazeDirection>();
        list.Add(new MazeDirection{Point = _start, Value = 'S', Direction = 'x' });

        var current = FindNextStart(_start);
        list.Add(current);

        while (current.Value != 'S')
        {
            current = FindNext(current);
            list.Add(current);
            // current.DumpJson();

            // if(list.Count > 3) break;
        }

        // list.DumpJson("Instructions");
        _points = list.Select(x => x.Point).ToList();

        return (list.Count - 1) / 2;
    }

    private long CountEnclosed1()
    {
        var map = new Map<char>(_width, _height, '.');

        foreach (var point in _points)
        {
            map[point.X, point.Y] = _map[point.X, point.Y];
        }

        var spread = new Queue<Point>();
        spread.Enqueue(new Point());

        while (spread.Count > 0)
        {
            var next = spread.Dequeue();
            if(map.GetValue(next.X, next.Y) != '.') continue;

            map[next.X, next.Y] = ' ';

            spread.Enqueue(new Point(next.X - 1, next.Y));
            spread.Enqueue(new Point(next.X + 1, next.Y));
            spread.Enqueue(new Point(next.X, next.Y - 1));
            spread.Enqueue(new Point(next.X, next.Y + 1));
        }

        map.MapValueToString().Dump("NEW", true);
        // map.Dump("NEW", true);

        return map.CountValues(x => x == '.');
    }

    private long CountEnclosed()
    {
        var map = new Map<char>(_width * 3, _height * 3, '.');

        foreach (var point in _points)
        {
            // map[point.X, point.Y] = _map[point.X, point.Y];
            Draw(_map, map,  point.X, point.Y);
        }

        // map.MapValueToString().Dump("NEW", true);

        var spread = new Queue<Point>();
        spread.Enqueue(new Point());

        while (spread.Count > 0)
        {
            var next = spread.Dequeue();
            if (map.GetValue(next.X, next.Y) != '.') continue;

            map[next.X, next.Y] = ' ';

            spread.Enqueue(new Point(next.X - 1, next.Y));
            spread.Enqueue(new Point(next.X + 1, next.Y));
            spread.Enqueue(new Point(next.X, next.Y - 1));
            spread.Enqueue(new Point(next.X, next.Y + 1));
        }

        // map.MapValueToString().Dump("NEW", true);

        // var sum = map.CountValues(x => x == '.');
        var sum = CountBlocks(map);
        return sum;
    }

    private void Draw(Map<char> map, Map<char> mapLarge, int x, int y)
    {
        var value = map[x, y];
        var points = value switch
        {
            '.' => new []{ '.', '.', '.', '.', '.', '.', '.', '.', '.' },
            '-' => new []{ '.', '.', '.', 'x', 'x', 'x', '.', '.', '.' },
            '|' => new []{ '.', 'x', '.', '.', 'x', '.', '.', 'x', '.' },
            '7' => new []{ '.', '.', '.', 'x', 'x', '.', '.', 'x', '.' },
            'F' => new []{ '.', '.', '.', '.', 'x', 'x', '.', 'x', '.' },
            'J' => new []{ '.', 'x', '.', 'x', 'x', '.', '.', '.', '.' },
            'L' => new []{ '.', 'x', '.', '.', 'x', 'x', '.', '.', '.' },
            'S' => new []{ 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
            _ => default
        };

        mapLarge[x * 3 + 0, y * 3 + 0] = points[0];
        mapLarge[x * 3 + 1, y * 3 + 0] = points[1];
        mapLarge[x * 3 + 2, y * 3 + 0] = points[2];
        mapLarge[x * 3 + 0, y * 3 + 1] = points[3];
        mapLarge[x * 3 + 1, y * 3 + 1] = points[4];
        mapLarge[x * 3 + 2, y * 3 + 1] = points[5];
        mapLarge[x * 3 + 0, y * 3 + 2] = points[6];
        mapLarge[x * 3 + 1, y * 3 + 2] = points[7];
        mapLarge[x * 3 + 2, y * 3 + 2] = points[8];
    }
    
    private long CountBlocks(Map<char> map)
    {
        var sum = 0;
        var points = new List<char>();

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                points.Clear();

                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        points.Add(map[i * 3 + k, j * 3 + l]);
                    }
                }

                if (points.All(x => x == '.')) sum++;
            }
        }

        return sum;
    }
    
    private MazeDirection FindNextStart(Point start)
    {
        /*
            | is a vertical pipe connecting north and south.
            - is a horizontal pipe connecting east and west.
            L is a 90-degree bend connecting north and east.
            J is a 90-degree bend connecting north and west.
            7 is a 90-degree bend connecting south and west.
            F is a 90-degree bend connecting south and east.
        */

        var north = _map.GetValue(start.X, start.Y - 1);
        var south = _map.GetValue(start.X, start.Y + 1);
        var west = _map.GetValue(start.X - 1, start.Y);
        var east = _map.GetValue(start.X + 1, start.Y);

        var next = default(MazeDirection);

        if (_allowedNorth.Contains(north)) next = new MazeDirection { Point = new Point(start.X, start.Y - 1), Value = north, Direction = north switch { '7' => 'W', 'F' => 'W', _ => 'N' } };
        else if (_allowedSouth.Contains(south)) next = new MazeDirection { Point = new Point(start.X, start.Y + 1), Value = south, Direction = south switch { 'J' => 'W', 'L' => 'E', _ => 'S' } };
        else if (_allowedWest.Contains(west)) next = new MazeDirection { Point = new Point(start.X - 1, start.Y), Value = west, Direction = west switch { 'L' => 'N', 'F' => 'S', _ => 'W' } };
        else if (_allowedEast.Contains(east)) next = new MazeDirection { Point = new Point(start.X + 1, start.Y), Value = east, Direction = east switch { 'J' => 'N', '7' => 'S', _ => 'E' } };

        return next;
    }

    private MazeDirection FindNext(MazeDirection current)
    {
        var x = current.Point.X;
        var y = current.Point.Y;
        var dir = current.Direction;

        var nextPoint = current switch
        {
            { Value: '|', Direction: 'N' } => new Point(x, y - 1),
            { Value: '|', Direction: 'S' } => new Point(x, y + 1),
            { Value: '-', Direction: 'W' } => new Point(x - 1, y),
            { Value: '-', Direction: 'E' } => new Point(x + 1, y),
            { Value: '7', Direction: 'W' } => new Point(x - 1, y),
            { Value: '7', Direction: 'S' } => new Point(x, y + 1),
            { Value: 'F', Direction: 'E' } => new Point(x + 1, y),
            { Value: 'F', Direction: 'S' } => new Point(x, y + 1),
            { Value: 'J', Direction: 'W' } => new Point(x - 1, y),
            { Value: 'J', Direction: 'N' } => new Point(x, y - 1),
            { Value: 'L', Direction: 'E' } => new Point(x + 1, y),
            { Value: 'L', Direction: 'N' } => new Point(x, y - 1),
            _ => default,
        };

        var nextValue = _map.GetValue(nextPoint.X, nextPoint.Y);

        var nextDir = (dir, nextValue) switch
        {
            ( 'N', '|' ) => 'N',
            ( 'N', '7' ) => 'W',
            ( 'N', 'F' ) => 'E',
            ( 'S', '|' ) => 'S',
            ( 'S', 'J' ) => 'W',
            ( 'S', 'L' ) => 'E',
            ( 'W', '-' ) => 'W',
            ( 'W', 'L' ) => 'N',
            ( 'W', 'F' ) => 'S',
            ( 'E', '-' ) => 'E',
            ( 'E', 'J' ) => 'N',
            ( 'E', '7' ) => 'S',
            _ => default,
        };

        return new MazeDirection{ Point = nextPoint, Value = nextValue, Direction = nextDir };
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _map = new Map<char>(_data, x => x);
        _width = _map.Width;
        _height = _map.Height;

        var (r, x, y) = _map.Find('S');
        _start = new Point(x, y);
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
        // _map.MapValueToString().Dump();
        // _map.DumpJson();
    }
}

#if DUMP
#endif