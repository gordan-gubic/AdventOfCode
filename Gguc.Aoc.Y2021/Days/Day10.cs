#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day10 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 10;

    private List<string> _source;
    private List<List<int>> _data;

    public Day10(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = new List<List<int>>();

        var map = new Dictionary<char, int>
        {
            ['('] = 3,
            [')'] = -3,
            ['['] = 57,
            [']'] = -57,
            ['{'] = 1197,
            ['}'] = -1197,
            ['<'] = 25137,
            ['>'] = -25137,
        };

        foreach (var line in _source)
        {
            var chline = new List<int>();
            foreach (var ch in line)
            {
                chline.Add(map[ch]);
            }
            _data.Add(chline);
        }
    }

    protected override void ComputePart1()
    {
        var lines = _data.ToList();

        var r = ProcessLines(lines);
        Result = r.Item1;
    }

    protected override void ComputePart2()
    {
        var lines = _data.ToList();

        var r = ProcessLines(lines);
        Result = r.Item2;
    }

    private (long, long) ProcessLines(List<List<int>> lines)
    {
        var count = lines.Count;
        var opens = new Stack<int>();
        var results1 = new List<long>();
        var results2 = new List<long>();

        for (int i = count - 1; i >= 0; i--)
        {
            opens.Clear();

            var corrupted = 0L;

            foreach (var value in lines[i])
            {
                corrupted = ProcessValue(value, opens);

                if (corrupted > 0)
                {
                    lines.RemoveAt(i);
                    results1.Add(corrupted);
                    break;
                }
            }

            if (corrupted == 0)
                results2.Add(CalcScore(opens));
        }

        var r1 = results1.Sum();
        var r2 = CalcMiddleScore(results2);

        return (r1, r2);
    }

    private long CalcMiddleScore(List<long> scores)
    {
        var list = scores.OrderBy(x => x).ToList();
        list.Dump();

        var mid = (list.Count + 1) / 2;
        return list[mid - 1];
    }

    private long CalcScore(Stack<int> opens)
    {
        var map = new Dictionary<int, long>
        {
            [3] = 1,
            [57] = 2,
            [1197] = 3,
            [25137] = 4,
        };

        var total = 0L;
        foreach (var value in opens)
        {
            total *= 5L;
            total += map[value];
        }

        return total;
    }

    private long ProcessValue(int value, Stack<int> opens)
    {
        if (value > 0)
        {
            opens.Push(value);
            return 0;
        }

        var expected = -opens.Peek();
        if (expected != value)
        {
            return -value;
        }
        opens.Pop();

        return 0;
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

        _data.DumpJson("List");
    }
    #endregion Dump
}

#if DUMP

#endif