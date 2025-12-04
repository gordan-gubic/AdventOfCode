#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day08 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 8;

    private List<string> _data;
    private Map<char> _map;
    private int _height;
    private int _width;

    private Dictionary<char, List<Point>> _antennas;
    private HashSet<Point> _antennaPoints;

    public Day08(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "214";
        ExpectedProd2 = "809";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
        _map = Parser.ParseMapChar();
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
        Result = CountAntinodes();
    }

    protected override void ComputePart2()
    {
        Result = CountContinuousAntinodes();
    }

    private long CountAntinodes()
    {
        var anodes = new HashSet<Point>();

        Fill(_antennas, anodes);

        return anodes.Count;
    }

    private long CountContinuousAntinodes()
    {
        var anodes = new HashSet<Point>(_antennaPoints);

        FillContinuous(_antennas, anodes);

        return anodes.Count;
    }

    private void Fill(Dictionary<char, List<Point>> antennas, HashSet<Point> anodes)
    {
        foreach (var key in _antennas.Keys)
        {
            var list = _antennas[key];

            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    var a = list[i];
                    var b = list[j];

                    var dx = a.X - b.X;
                    var dy = a.Y - b.Y;

                    var x1 = a.X + dx;
                    var y1 = a.Y + dy;

                    var x2 = b.X - dx;
                    var y2 = b.Y - dy;

                    AddAntiNode(anodes, new Point(x1, y1));
                    AddAntiNode(anodes, new Point(x2, y2));
                }
            }
        }

        // anodes.DumpJsonIndented("a", true);
    }

    private void FillContinuous(Dictionary<char, List<Point>> antennas, HashSet<Point> anodes)
    {
        foreach (var key in _antennas.Keys)
        {
            var list = _antennas[key];

            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    var a = list[i];
                    var b = list[j];

                    var dx = a.X - b.X;
                    var dy = a.Y - b.Y;

                    var x1 = a.X;
                    var y1 = a.Y;

                    var x2 = b.X;
                    var y2 = b.Y;

                    while (true)
                    {
                        x1 += dx;
                        y1 += dy;

                        var isAdded = AddAntiNode(anodes, new Point(x1, y1));
                        if(!isAdded) break;
                    }

                    while (true)
                    {
                        x2 -= dx;
                        y2 -= dy;

                        var isAdded = AddAntiNode(anodes, new Point(x2, y2));
                        if (!isAdded) break;
                    }
                }
            }
        }

        // anodes.DumpJsonIndented("a", true);
    }

    private bool AddAntiNode(HashSet<Point> anodes, Point point)
    {
        if(!_map.Contains(point.X, point.Y)) return false;

        anodes.Add(point);
        return true;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _antennas = new();
        _antennaPoints = new();

        _map.ForEach((x, y, ch) =>
        {
            if(ch == '.') return;

            var p = new Point(x, y);
            _antennaPoints.Add(p);

            if (!_antennas.ContainsKey(ch)) _antennas[ch] = new();
            _antennas[ch].Add(p);
        });
        
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
        // _antennas.DumpJsonIndented("a", true);
    }
}

#if DUMP
#endif