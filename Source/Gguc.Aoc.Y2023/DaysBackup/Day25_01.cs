#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.DaysBackup;

public class Day25_01 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 25_01;

    private List<string> _data;
    private Dictionary<string, HashSet<string>> _links = new();
    private HashSet<(string, string)> _relations = new();
    private HashSet<string> _group1 = new();
    private HashSet<string> _group2 = new();
    private HashSet<string> _group3 = new();

    public Day25_01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "";
        Expected2 = "";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

        _data = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = FindGroups();

        Result = result;
    }

    protected override void ComputePart2()
    {
        Result = 0L;
    }

    private long FindGroups()
    {
        var keys = _links.Keys.ToList();

        var queue = new Queue<string>(keys);
        _group1.Add(keys[0]);
        _group3.Remove(keys[0]);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            ProcessCurrent(current);

            // break;
        }

        /*
        keys = _group3.ToList();
        queue = new Queue<string>(keys);
        _group2.Add(keys[0]);
        _group3.Remove(keys[0]);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            ProcessCurrent(current);

            // break;
        }
        */

        Log.Debug($"-1-: [{_group1.ToJson()}]");
        Log.Debug($"-2-: [{_group2.ToJson()}]");
        Log.Debug($"-R-: [{_group3.ToJson()}]");

        return 1L * _group1.Count * _group3.Count;
    }

    private void ProcessCurrent(string item)
    {
        var dict = new Dictionary<string, int>();
        dict[item] = 1;

        var cache = new HashSet<string>();
        cache.Add(item);

        ProcessLink(item, dict, cache);

        var queue = new Queue<string>(_links[item]);
        while (queue.Count > 0)
        {
            var link = queue.Dequeue();

            if (cache.Contains(link)) continue;
            cache.Add(link);

            ProcessLink(link, dict, cache);
        }

        // dict.DumpJson();

        foreach (var kv in dict.Where(x => x.Value > 2).ToList())
        {
            if (kv.Key == item) continue;

            AddGroup(item, kv.Key);
        }
    }

    private void ProcessLink(string key, Dictionary<string, int> dict, HashSet<string> cache)
    {
        var links = _links[key].ToList();

        foreach (var link in links)
        {
            var b = _links[link];

            foreach (var c in b)
            {
                if (cache.Contains(c)) continue;

                if (!dict.ContainsKey(c)) dict[c] = 0;
                dict[c]++;
            }
        }
    }

    private void AddGroup(string item1, string item2)
    {
        if (_group1.Contains(item1)) _group1.Add(item2);
        else if (_group1.Contains(item2)) _group1.Add(item1);
        else if (_group2.Contains(item1)) _group2.Add(item2);
        else if (_group2.Contains(item2)) _group2.Add(item1);

        if (_group1.Contains(item1) || _group2.Contains(item1)) _group3.Remove(item1);
        if (_group1.Contains(item2) || _group2.Contains(item2)) _group3.Remove(item2);
    }


    protected override void ProcessData()
    {
        base.ProcessData();

        _links = new();

        foreach (var line in _data)
        {
            var parts = line.Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

            var current = parts[0];
            foreach (var other in parts.Skip(1).ToList())
            {
                AddLink(current, other);
            }
        }

        _group3 = new(_links.Keys);
    }

    private void AddLink(string current, string other)
    {
        if (!_links.ContainsKey(current)) _links[current] = new();
        if (!_links.ContainsKey(other)) _links[other] = new();

        // _links[current].Add(current);
        _links[current].Add(other);
        // _links[other].Add(other);
        _links[other].Add(current);

        _relations.Add((current, other));
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
        // _links.DumpJson("links");

        // foreach (var link in _links)
        // {
        //     Log.Debug($"{link.Key}\t{string.Join('\t', link.Value)}");
        // }
    }
}

#if DUMP
#endif