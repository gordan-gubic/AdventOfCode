#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day21 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 21;

    private List<string> _data;
    private Map<char> _keypad;
    private Map<char> _arrows;
    private Dictionary<(char, char), string> _kDir;
    private Dictionary<(char, char), string> _aDir;

    public Day21(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "";
        ExpectedProd2 = "";
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
        FillCache();

        Result = ComputeCodes();
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        Result = result;
    }

    private long ComputeCodes()
    {
        var sum = 0L;

        foreach (var line in _data)
        {
            sum += ComputeLine(line);
        }

        return sum;
    }

    private long ComputeLine(string line)
    {
        var sum = 0L;

        var lx = ProcessLine1(line);
        lx.Dump("lx1");
        lx = ProcessLine2(lx);
        lx.Dump("lx2");
        lx = ProcessLine2(lx);
        lx.Dump("lx3");

        var a = lx.Length;
        var b = line.Trim('A').ToLong();

        $"{a}**{b}=={a*b}".Dump("compute");

        return a * b;
    }

    private string ProcessLine1(string line)
    {
        var r = "";
        var current = 'A';
        for (var i = 0; i < line.Length; i++)
        {
            r = $"{r}{_kDir[(current, line[i])]}{'A'}";
            current = line[i];
        }

        return r;
    }

    private string ProcessLine2(string line)
    {
        var r = "";
        var current = 'A';
        for (var i = 0; i < line.Length; i++)
        {
            r = $"{r}{_aDir[(current, line[i])]}{'A'}";
            current = line[i];
        }

        return r;
    }

    private void FillCache()
    {
        var keypad = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'A' };
        var arrows = new List<char> { '^', '<', 'v', '>', 'A' };

        var kPoints = new Dictionary<char, Point>();
        var aPoints = new Dictionary<char, Point>();

        var kDir = new Dictionary<(char, char), string>();
        var aDir = new Dictionary<(char, char), string>();

        foreach (var key in keypad)
        {
            var r = _keypad.Find(key);
            kPoints[key] = new Point(r.Item2, r.Item3);
        }

        foreach (var key in arrows)
        {
            var r = _arrows.Find(key);
            aPoints[key] = new Point(r.Item2, r.Item3);
        }

        for (var i = 0; i < keypad.Count; i++)
        {
            for (var j = 0; j < keypad.Count; j++)
            {
                var p1 = kPoints[keypad[i]];
                var p2 = kPoints[keypad[j]];

                var xd = p2.X - p1.X;
                var yd = p2.Y - p1.Y;

                var diff = "";

                if (yd < 0)
                {
                    diff = $"{diff}{"".PadLeft(Math.Abs(yd), '^')}";
                }
                if (xd > 0)
                {
                    diff = $"{diff}{"".PadLeft(Math.Abs(xd), '>')}";
                }
                if (yd > 0)
                {
                    diff = $"{diff}{"".PadLeft(Math.Abs(yd), 'v')}";
                }
                if (xd < 0)
                {
                    diff = $"{diff}{"".PadLeft(Math.Abs(xd), '<')}";
                }

                kDir[(keypad[i], keypad[j])] = diff;
            }
        }

        for (var i = 0; i < arrows.Count; i++)
        {
            for (var j = 0; j < arrows.Count; j++)
            {
                var p1 = aPoints[arrows[i]];
                var p2 = aPoints[arrows[j]];

                var xd = p2.X - p1.X;
                var yd = p2.Y - p1.Y;

                var diff = "";


                if (yd > 0)
                {
                    diff = $"{diff}{"".PadLeft(Math.Abs(yd), 'v')}";
                }
                if (xd > 0)
                {
                    diff = $"{diff}{"".PadLeft(Math.Abs(xd), '>')}";
                }
                if (yd < 0)
                {
                    diff = $"{diff}{"".PadLeft(Math.Abs(yd), '^')}";
                }
                if (xd < 0)
                {
                    diff = $"{diff}{"".PadLeft(Math.Abs(xd), '<')}";
                }

                aDir[(arrows[i], arrows[j])] = diff;
            }
        }

        _kDir = kDir;
        _aDir = aDir;

        _kDir[('3', '7')] = "<<^^";
        // _aDir[('A', '<')] = "<v<";

        // kPoints.DumpJson("k");
        // aPoints.DumpJson("a");
        // 
        kDir.DumpJsonIndented("k");
        aDir.DumpJsonIndented("a");
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _keypad = new Map<char>(3, 4, '.');
        _arrows = new Map<char>(3, 2, '.');

        var keypad = new List<char> { '7', '8', '9', '4', '5', '6', '1', '2', '3', '.', '0', 'A' };
        var arrows = new List<char> { '.', '^', 'A', '<', 'v', '>' };

        var i = 0;
        _keypad.ForEach((x, y) => { _keypad[x, y] = keypad[i++]; });

        i = 0;
        _arrows.ForEach((x, y) => { _arrows[x, y] = arrows[i++]; });
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
        // _keypad.MapValueToString().Dump("keypad", true);
        // _arrows.MapValueToString().Dump("arrows", true);
    }
}

#if DUMP
#endif