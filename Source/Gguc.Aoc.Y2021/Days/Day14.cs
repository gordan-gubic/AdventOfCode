#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day14 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 14;

    private List<string> _source;
    private string _init;
    private Dictionary<string, string> _dict;
    private Dictionary<(string, int), Dictionary<char, long>> _cache;

    public Day14(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    #region Parse
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    protected override void ProcessData()
    {
        _cache = new Dictionary<(string, int), Dictionary<char, long>>();

        _init = _source[0];
        _dict = new Dictionary<string, string>();

        for (int i = 2; i < _source.Count; i++)
        {
            var value = _source[i];
            var parts = value.Split(" -> ");
            _dict[parts[0]] = parts[1];
        }
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        var polymer = _init;

        var result = ProcessPolymerLoop(polymer, 10);

        Result = CalculateResult(result);
    }

    protected override void ComputePart2()
    {
        var polymer = _init;

        var result = ProcessPolymerLoop(polymer, 40);

        Result = CalculateResult(result);
    }

    private Dictionary<char, long> ProcessPolymerLoop(string polymer, int steps)
    {
        var results1 = new Dictionary<char, long>();
        AddResult(results1, polymer[0]);

        for (int i = 0; i < polymer.Length - 1; i++)
        {
            var chunk = polymer.Substring(i, 2);
            var results2 = ProcessChunk(chunk, steps);
            AddResults(results1, results2);
        }

        return results1;
    }

    private Dictionary<char, long> ProcessChunk(string chunk, int steps)
    {
        var results = new Dictionary<char, long>();

        if (_cache.ContainsKey((chunk, steps))) return _cache[(chunk, steps)];
        
        if (steps == 0)
        {
            AddResult(results, chunk[1]);
            return results;
        }

        var value = _dict[chunk];
        var chunk1 = $"{chunk[0]}{value}";
        var chunk2 = $"{value}{chunk[1]}";

        var r1 = ProcessChunk(chunk1, steps - 1);
        var r2 = ProcessChunk(chunk2, steps - 1);

        AddResults(results, r1);
        AddResults(results, r2);

        _cache[(chunk, steps)] = results;

        return results;
    }

    private void AddResult(Dictionary<char, long> results, char ch, long value = 1L)
    {
        if (!results.ContainsKey(ch)) results[ch] = 0L;
        results[ch] += value;
    }

    private void AddResults(Dictionary<char, long> results1, Dictionary<char, long> results2)
    {
        foreach (var key in results2.Keys)
        {
            if (!results1.ContainsKey(key)) results1[key] = 0L;
            results1[key] += results2[key];
        }
    }

    private long CalculateResult(Dictionary<char, long> results)
    {
        var values = results.Values.ToList();
        var (min, max) = values.MinMax();
        return max - min;
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

        _init.Dump("_init");
        _dict.DumpJson("_dict");
    }
    #endregion Dump
}

#if DUMP

#endif