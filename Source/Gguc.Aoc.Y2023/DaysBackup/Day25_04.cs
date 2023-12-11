#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.DaysBackup;

public class Day25_04 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 25_04;

    private List<string> _data;
    private Dictionary<string, HashSet<string>> _links = new();
    private HashSet<string> _elements = new();
    private HashSet<(string, string)> _relations = new();
    private HashSet<string> _group1 = new();
    private HashSet<string> _group2 = new();
    private HashSet<string> _group3 = new();

    public Day25_04(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "";
        Expected2 = "";
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
        // var groups = FindGroups1();
        // groups.DumpJson();

        var result = FindGroupsMax();

        Result = result;
    }

    protected override void ComputePart2()
    {
        Result = 0L;
    }

    private long FindGroupsMax()
    {
        var result = 0L;
        var relations = _relations.ToList();
        var group1 = new HashSet<string>();
        var filter = new Filter();

        for (var i = 0; i < relations.Count - 2; i++)
        {
            filter.AddA(relations[i].Item1, relations[i].Item2);
            Log.Debug($"Find I: [{i}/{relations.Count}] [{relations[i].Item1}] [{relations[i].Item2}]");

            for (var j = i + 1; j < relations.Count - 1; j++)
            {
                Log.Debug($"Find J: [{j}]");
                filter.AddB(relations[j].Item1, relations[j].Item2);
                if (!filter.IsValid) continue;

                for (var k = j + 1; k < relations.Count; k++)
                {
                    //Log.Debug($"Find K: [{k}]");
                    filter.AddC(relations[k].Item1, relations[k].Item2);
                    if (!filter.IsValid) continue;

                    // var groups = FindGroups1();
                    // groups.Where(x=> x.Value < 2).ToList().DumpJson();


                    group1 = FindGroups(filter);

                    var group2 = _elements.Except(group1).ToList();

                    if (group2.Any())
                    {
                        // Log.Debug($"-1-: [{group1.ToJson()}]");
                        // Log.Debug($"-2-: [{group2.ToJson()}]");

                        var prod = group1.Count * group2.Count;
                        result = Math.Max(result, prod);
                        return result;
                    }
                    /*
                    */
                }

                break;
            }

            break;
        }

        return result;
    }

    private HashSet<string> FindGroups(Filter filter)
    {
        var keys = _links.Keys.ToList();
        var init = keys[0];
        var queue = new Queue<string>();
        queue.Enqueue(init);

        var group = new HashSet<string>();

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            if (group.Contains(item)) continue;

            ProcessItem(item, filter, group, queue);
        }

        return group;
    }

    private void ProcessItem(string item1, Filter filter, HashSet<string> group, Queue<string> queue)
    {
        group.Add(item1);
        var items = _links[item1];

        foreach (var item2 in items)
        {
            if (group.Contains(item2)) continue;

            if (filter.A1 == item1 && filter.A2 == item2 || filter.A1 == item2 && filter.A2 == item1) continue;
            if (filter.B1 == item1 && filter.B2 == item2 || filter.B1 == item2 && filter.B2 == item1) continue;
            if (filter.C1 == item1 && filter.C2 == item2 || filter.C1 == item2 && filter.C2 == item1) continue;

            queue.Enqueue(item2);
        }
    }

    private Dictionary<string, int> FindGroups1()
    {
        var keys = _links.Keys.ToList();
        var init = keys[0];
        var queue = new Queue<string>();
        queue.Enqueue(init);

        var group = new Dictionary<string, int>();

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();

            ProcessItem1(item, group, queue);
        }

        return group;
    }

    private void ProcessItem1(string item1, Dictionary<string, int> group, Queue<string> queue)
    {
        if (!group.ContainsKey(item1)) group[item1] = 0;

        group[item1]++;
        var items = _links[item1];

        foreach (var item2 in items)
        {
            if (group.ContainsKey(item2)) continue;
            queue.Enqueue(item2);
        }
    }

    private void RemoveLink(Dictionary<string, HashSet<string>> links, (string, string) rel)
    {
        links[rel.Item1].Remove(rel.Item2);
        links[rel.Item2].Remove(rel.Item1);
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
        // _links[other].Add(other);

        _links[current].Add(other);
        _links[other].Add(current);

        _elements.Add(current);
        _elements.Add(other);
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
        // _relations.DumpCollection();

        // foreach (var link in _links)
        // {
        //     Log.Debug($"{link.Key}\t{string.Join('\t', link.Value)}");
        // }
    }
}

#if DUMP



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
            
            if(cache.Contains(link)) continue;
            cache.Add(link);

            ProcessLink(link, dict, cache);
        }

        // dict.DumpJson();

        foreach (var kv in dict.Where(x => x.Value > 2).ToList())
        {
            if(kv.Key == item) continue;

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

#endif