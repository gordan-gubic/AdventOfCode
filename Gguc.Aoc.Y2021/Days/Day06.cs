#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day06 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 6;
    private List<List<int>> _source;
    private List<int> _data;

    private Dictionary<(int, int), long> _cache;

    public Day06(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse(ConvertInput);
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source[0];
    }

    protected override void ComputePart1()
    {
        var lanterns = _data.ToList();
        _cache = new Dictionary<(int, int), long>();

        Result = ProcessLanterns(lanterns, 80);
    }

    protected override void ComputePart2()
    {
        var lanterns = _data.ToList();
        _cache = new Dictionary<(int, int), long>();

        Result = ProcessLanterns(lanterns, 256);
    }


    private long ProcessLanterns(List<int> lanterns, int days)
    {
        var total = 0L;
        var count = lanterns.Count;

        for (int i = 0; i < count; i++)
        {
            total += ProcessLantern(lanterns[i], days);
        }

        return total;
    }

    private long ProcessLantern(int value, int days)
    {
        if (_cache.ContainsKey((value, days))) return _cache[(value, days)];

        if (days <= 0) return 1;

        var total = 1L;
        var l = value;

        var n = (days + (6 - l)) / 7;
        
        while(n > 0)
        {
            var rest = days - (n * 7) + (6 - value);
            // $"n: {n} - rest: {rest}".Dump();
            total += ProcessLantern(8, rest);
            n--;
        }

        // $"Lantern {l}: {n}".Dump();

        _cache[(value, days)] = total;
        return total;
    }

    private List<int> ConvertInput(string input)
    {
        return input.Split(',').Select(x => x.ToInt()).ToList();
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