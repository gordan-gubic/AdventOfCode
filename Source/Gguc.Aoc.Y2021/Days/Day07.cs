#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day07 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 7;

    private List<string> _source;
    private List<int> _data;
    private Dictionary<int, long> _cache;

    public Day07(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
        _data = _source[0].ToIntSequence();
        _cache = new Dictionary<int, long>();
    }

    protected override void ComputePart1()
    {
        var list = _data.ToList();
        _cache.Clear();

        var (min, max) = list.MinMax();

        var r_list = new List<long>();

        for (int i = min; i <= max; i++)
        {
            var r = ProcessList(list, i, CalcDistance1);
            r_list.Add(r);
        }

        Result = r_list.Min();
    }

    protected override void ComputePart2()
    {
        var list = _data.ToList();
        _cache.Clear();

        var (min, max) = list.MinMax();

        var r_list = new List<long>();

        for (int i = min; i <= max; i++)
        {
            var r = ProcessList(list, i, CalcDistance2);
            r_list.Add(r);
        }

        Result = r_list.Min();
    }

    private long ProcessList(List<int> list, int target, Func<int, int, long> calc)
    {
        if (_cache.ContainsKey(target)) return _cache[target];

        var total = 0L;

        foreach (var n in list)
        {
            var dist = calc(n, target);
            total += dist;
        }

        _cache[target] = total;
        return total;
    }

    public long CalcDistance1(int value, int target)
    {
        return Math.Abs(value - target);
    }

    public long CalcDistance2(int value, int target)
    {
        // https://en.wikipedia.org/wiki/1_%2B_2_%2B_3_%2B_4_%2B_%E2%8B%AF

        var x = Math.Abs(value - target);
        return (x * x + 1) / 2;
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

        _data[0].Dump("Item");
        _data.DumpCollection("List");
    }
    #endregion Dump
}

#if DUMP

#endif