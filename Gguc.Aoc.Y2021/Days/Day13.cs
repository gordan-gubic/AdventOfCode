#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day13 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 13;

    private List<string> _source;
    private List<string> _data;
    private List<(int, int)> _points;
    private List<(string, int)> _instructions;
    private int _width;
    private int _height;
    private Map<bool> _map;

    public Day13(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    #region Parse
    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;

        var queue = new Queue<string>(_source);

        var points = new List<(int, int)>();
        var xs = new List<int>();
        var ys = new List<int>();

        var instructions = new List<(string, int)>();

        while (true)
        {
            var value = queue.Dequeue();
            if (value.IsWhitespace()) break;

            var parts = value.Split(',');
            var x = parts[0].ToInt();
            var y = parts[1].ToInt();
            points.Add((x, y));
            xs.Add(x);
            ys.Add(y);
        }

        while (queue.Count > 0)
        {
            var value = queue.Dequeue();
            value = value.Replace("fold along ", "");

            var parts = value.Split('=');
            var x = parts[0];
            var y = parts[1].ToInt();
            instructions.Add((x, y));
        }

        _points = points;
        _instructions = instructions;

        _width = xs.Max() + 1;
        _height = ys.Max() + 1;
        _map = new Map<bool>(_width, _height);

        foreach (var p in points)
        {
            _map[p.Item1, p.Item2] = true;
        }
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        var map = _map.Clone();
        var fold = _instructions[0];

        map = Fold(map, fold.Item1, fold.Item2);
        Result = map.CountValues(true);
    }

    protected override void ComputePart2()
    {
        var map = _map.Clone();
        
        foreach(var fold in _instructions)
        {
            map = Fold(map, fold.Item1, fold.Item2);
        }

        Log.Info($"\n{map.MapBoolToString()}");
    }

    private Map<bool> Fold(Map<bool> map, string item1, int item2)
    {
        if(item1 == "x")
            return FoldHorizontally(map, item2);
        else
            return FoldVertically(map, item2);
    }

    private Map<bool> FoldHorizontally(Map<bool> map, int item2)
    {
        var sub1 = map.Sub(0, 0, (item2 - 1), map.Height - 1);
        var sub2 = map.Sub((item2 + 1), 0, map.Width - 1, map.Height - 1);

        sub2 = sub2.FlipHorizontally();
        sub1 = sub1.Union(sub2);

        return sub1;
    }

    private Map<bool> FoldVertically(Map<bool> map, int item2)
    {
        var sub1 = map.Sub(0, 0, map.Width - 1, (item2 - 1));
        var sub2 = map.Sub(0, (item2 + 1), map.Width - 1, map.Height - 1);

        sub2 = sub2.FlipVertically();
        sub1 = sub1.Union(sub2);

        return sub1;
    }

    #region Dump
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _points.DumpCollection("instructions");
        _instructions.DumpCollection("instructions");
        // _width.Dump();
        // _height.Dump();
        _map.Dump("map", true);
    }
    #endregion Dump
}

#if DUMP
#endif