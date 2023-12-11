#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day18 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 18;

    private List<string> _data;
    private List<Dig> _digs;

    public Day18(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "56923";
        Expected2 = "66296566363189";
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
        var result = CalculateVolume(_digs.ToList());

        Result = result;
    }

    protected override void ComputePart2()
    {
        var digs = ConvertDigs(_digs.ToList());

        var result = CalculateVolume(digs);

        Result = result;
    }

    private long CalculateVolume(List<Dig> digs)
    {
        var sum = 0L;

        var points = new List<PointLong>();

        var current = new PointLong();
        var prev = 'N';
        var dir = 'N';
        points.Add(current);

        for (int i = 0; i < digs.Count; i++)
        {
            var dig = digs[i];

            var j = (i + 1 >= digs.Count) ? 0 : i + 1;
            var next = CalcDir(digs[j]);

            prev = dir;
            dir = CalcDir(dig);
            current = CalcPoint(dig, current, dir, prev, next);

            points.Add(current);
        }

        // points.DumpCollection();

        var area = CalculateArea(points);
        Log.Dump($"Area: [{area}]");


        return area;
    }

    private long CalculateArea(List<PointLong> points)
    {
        /*
         * area = 0;
         * for( i = 0; i < N; i += 2 )
         *     area += x[i+1]*(y[i+2]-y[i]) + y[i+1]*(x[i]-x[i+2]);
         * area /= 2;
         */

        var area = 0L;

        for (var i = 0; i < points.Count - 2; i += 2)
        {
            area += points[i + 1].X * (points[i + 2].Y - points[i].Y) + (points[i + 1].Y) * (points[i].X - points[i + 2].X);
        }

        area /= 2;

        return area;
    }

    private char CalcDir(Dig dig)
    {
        return (dig.Dir) switch
        {
            ('U') => 'N',
            ('D') => 'S',
            ('L') => 'W',
            ('R') => 'E',
            _ => 'N',
        };
    }

    private PointLong CalcPoint(Dig dig, PointLong current, char dir, char prev, char next)
    {
        var point = new PointLong();
        var count = dig.Count;

        var zone1 = CalcZone(prev, dir);
        var zone2 = CalcZone(dir, next);

        if (dir == 'N') point = current with { Y = current.Y - count };
        else if (dir == 'S') point = current with { Y = current.Y + count };
        else if (dir == 'W') point = current with { X = current.X - count };
        else if (dir == 'E') point = current with { X = current.X + count };

        var comp = (zone1, zone2) switch
        {
            (0, 1) => (1, 0),
            (1, 0) => (-1, 0),

            (1, 2) => (0, 1),
            (2, 1) => (0, -1),

            (2, 3) => (-1, 0),
            (3, 2) => (1, 0),
        
            (3, 0) => (0, -1),
            (0, 3) => (0, 1),

            _ => (0, 0),
        };

        point = new PointLong(point.X + comp.Item1, point.Y + comp.Item2);

        // Log.Debug($"{new { current, point, dir, next, comp }}");

        return point;
    }

    private int CalcZone(char prev, char dir)
    {
        return (prev, dir) switch
        {
            ('N', 'E') => 0,
            ('N', 'W') => 3,
            ('E', 'S') => 1,
            ('E', 'N') => 0,
            ('W', 'S') => 2,
            ('W', 'N') => 3,
            ('S', 'E') => 1,
            ('S', 'W') => 2,
            _ => 0
        };
    }

    private List<Dig> ConvertDigs(List<Dig> list)
    {
       var digs = new List<Dig>();

       foreach (var x in list)
       {
           var hex = x.Color[1..6];
           var dir = x.Color.Last() switch
           {
               '0' => 'R',
               '1' => 'D',
               '2' => 'L',
               '3' => 'U',
               _ => 'R'
           };
           var count = HexToLong(hex);

           digs.Add(new Dig{Count = count, Dir = dir});
       }

       return digs;
    }

    private long HexToLong(string hex)
    {
        return long.Parse(hex, System.Globalization.NumberStyles.HexNumber);
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _digs = new();

        foreach (var line in _data)
        {
            var parts = line.Split(' ');
            var dig = new Dig
            {
                Dir = parts[0][0],
                Count = parts[1].ToInt(),
                Color = parts[2].Trim('(', ')'),
            };
            _digs.Add(dig);
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

        // _digs.DumpJson();
    }
}

#if DUMP
#endif