#define LOG

namespace Gguc.Aoc.Y2025.Days;

public class Day07 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 07;

    private List<string> _raw;
    private Map<char> _map;
    private Map<long> _values;

    public Day07(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = true;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "21";
        ExpectedTest2 = "40";

        ExpectedProd1 = "1660";
        ExpectedProd2 = "305999729392659";
    }

    protected override void InitParser()
    {
        _raw = Parser.Parse();
        _map = Parser.ParseMapChar();
    }

    protected override void ComputePart1()
    {
        Result = CountBeamSplit();
    }

    protected override void ComputePart2()
    {
        Result = Count_Part02();
    }

    private long CountBeamSplit()
    {
        var result = 0L;

        var startIndex = FindStart();
        result = StartSplitting(startIndex);

        return result;
    }

    private long Count_Part02()
    {
        var result = 0L;

        var startIndex = FindStart();
        result = ProcessValues(startIndex);

        return result;
    }

    private int FindStart()
    {
        var found = _map.Find('S');
        return found.Item2;
    }

    private long StartSplitting(int startIndex)
    {
        var hits = new HashSet<(int, int)>();

        var start = (startIndex, 0);
        var queue = new Queue<(int, int)>();
        queue.Enqueue(start);
        var cache = new HashSet<(int, int)>();

        while (queue.Any())
        {
            var point = queue.Dequeue();
            ProcessPoint(point, queue, cache, hits);
        }

        if (Parser.Type == ParserFileType.Test) hits.DumpCollection("hits", true);

        return hits.Count;
    } 

    private void ProcessPoint((int, int) point, Queue<(int, int)> queue, HashSet<(int, int)> cache, HashSet<(int, int)> hits)
    {
        var next = (point.Item1, point.Item2 + 1);
        if(cache.Contains(next)) return;
        if(next.Item2 >= _map.Height) return;

        cache.Add(point);

        var target = _map.GetValue(next.Item1, next.Item2);

        if (target == '.')
        {
            if (!cache.Contains(next))
            {
                queue.Enqueue(next);
                cache.Add(next);
            }
        }
        else if(target == '^')
        {
            hits.Add(next);
            var n1 = (next.Item1 - 1, next.Item2);
            var n2 = (next.Item1 + 1, next.Item2);

            if (!cache.Contains(n1))
            {
                queue.Enqueue(n1);
                cache.Add(n1);
            }
            if (!cache.Contains(n2))
            {
                queue.Enqueue(n2);
                cache.Add(n2);
            }
        }
    }

    private long ProcessValues(int startIndex)
    {
        for (var i = _values.Height - 2; i >= 0; i--)
        {
            ProcessRow(i);
        }

        return _values.GetValue(startIndex, 0);
    }

    private void ProcessRow(int row)
    {
        var queue = new Queue<(int, int)>();

        for (var i = 0; i < _values.Width; i++)
        {
            var ch = _map.GetValue(i, row);

            if (ch == '^')
            {
                queue.Enqueue((i, row));
                continue;
            }

            _values[i, row] = _values[i, row + 1];
        }

        while (queue.Any())
        {
            var splitter = queue.Dequeue();
            _values[splitter.Item1, splitter.Item2] = _values[splitter.Item1 - 1, splitter.Item2] + _values[splitter.Item1 + 1, splitter.Item2];
        }
    }

    protected override void ProcessData()
    {
        _values = new Map<long>(_map.Width, _map.Height, 1L);
    }

    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        _raw.DumpCollection();
        _map.MapValueToString().Dump("Map", true);
    }
}

#if DUMP
#endif