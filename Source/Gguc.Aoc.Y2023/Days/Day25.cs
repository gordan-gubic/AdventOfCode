#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day25 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 25;

    private List<string> _data;
    private Dictionary<string, HashSet<string>> _links = new();
    private HashSet<string> _elements = new();
    private HashSet<(string, string)> _relations = new();

    public Day25(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "568214";
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
        var relations = _elements.ToList();
        var group1 = new HashSet<string>();
        var filter = new Filter();
        var size = _elements.Count - 3;

        for (var i = 1; i < relations.Count - 2; i++)
        {
            Log.Debug($"Find I: [{i}/{relations[i]}/{relations.Count}]");

            for (var j = i + 1; j < relations.Count - 1; j++)
            {
                // Log.Debug($"Find J: [{j}/{relations[j]}]");

                for (var k = j + 1; k < relations.Count; k++)
                {
                    var a = relations[i];
                    var b = relations[j];
                    var c = relations[k];

                    var group = new HashSet<string>();
                    var hit = 0;

                    var targets = new HashSet<string>();
                    _links[a].ForEach(x => targets.Add(x));
                    _links[b].ForEach(x => targets.Add(x));
                    _links[c].ForEach(x => targets.Add(x));

                    FindItem5(relations[0], group, (a, b, c), targets);
                    // var (x, y) = FindGroup3(relations[0], (a, b, c));
                    // Log.Debug($"Find I: Count=[{group.Count}] Hit=[{hit}] [{i}/{relations[i]}] [{j}/{relations[j]}] [{k}/{relations[k]}]");
                    // Log.Debug($"Find I: Count=[{group.Count}] X=[{x}] X=[{y}] [{i}/{relations[i]}] [{j}/{relations[j]}] [{k}/{relations[k]}]");

                    if (group.Count < size && targets.Any())
                    {
                        Log.Debug($"Result: Count=[{group.Count}] Hit=[{hit}] [{i}/{relations[i]}] [{j}/{relations[j]}] [{k}/{relations[k]}]");
                        // Log.Debug($"Result: Count=[{group.Count}] X=[{x}] X=[{y}] [{i}/{relations[i]}] [{j}/{relations[j]}] [{k}/{relations[k]}]");

                        var prod = group.Count * (size - group.Count + 3);
                        // var prod = y * (size - y + 3);
                        result = Math.Max(result, prod);
                        Log.Debug($"Result=[{result}]");

                        // return result;
                    }
                }
            }
        }

        return result;
    }

    private void FindItem5(string item1, HashSet<string> group, (string a, string b, string c) filter, HashSet<string> targets)
    {
        if(!targets.Any()) return;

        if (targets.Contains(item1)) targets.Remove(item1);
        group.Add(item1);
        var items = _links[item1];

        foreach (var item2 in items)
        {
            if (group.Contains(item2)) continue;

            if (item2 == filter.a || item2 == filter.b || item2 == filter.c) continue;

            FindItem5(item2, group, filter, targets);
        }
    }

    private void AddLink(Dictionary<string, HashSet<string>> links, Filter filter)
    {
        links[filter.A1].Add(filter.A2);
        links[filter.A2].Add(filter.A1);
        links[filter.B1].Add(filter.B2);
        links[filter.B2].Add(filter.B1);
        links[filter.C1].Add(filter.C2);
        links[filter.C2].Add(filter.C1);
    }

    private void RemoveLink(Dictionary<string, HashSet<string>> links, Filter filter)
    {
        links[filter.A1].Remove(filter.A2);
        links[filter.A2].Remove(filter.A1);
        links[filter.B1].Remove(filter.B2);
        links[filter.B2].Remove(filter.B1);
        links[filter.C1].Remove(filter.C2);
        links[filter.C2].Remove(filter.C1);
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
    }
}

#if DUMP



    private HashSet<string> FindGroups(Filter filter)
    {
        var init = filter.A1;
        var queue = new Queue<string>();
        queue.Enqueue(init);

        var group = new HashSet<string>();

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            if (group.Contains(item)) continue;

            ProcessItem(item, filter, group, queue);

            if (group.Contains(filter.A2))
            {
                return null;
            }
            // if (group.Contains(filter.B1) && group.Contains(filter.B2)) return null;
            // if (group.Contains(filter.C1) && group.Contains(filter.C2)) return null;
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

            // if (filter.A1 == item1 && filter.A2 == item2 || filter.A1 == item2 && filter.A2 == item1) continue;
            // if (filter.B1 == item1 && filter.B2 == item2 || filter.B1 == item2 && filter.B2 == item1) continue;
            // if (filter.C1 == item1 && filter.C2 == item2 || filter.C1 == item2 && filter.C2 == item1) continue;

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


    private bool FindItem(Filter filter)
    {
        var item1 = filter.A1;
        var item2 = filter.A2;

        var group = new HashSet<string>();

        var found = FindItem(item1, item2, group);
        return found;

        // FindItem1(item1, group);
        // return group.Contains(item2);
    }

    private bool FindItem(string item1, string target, HashSet<string> group)
    {
        group.Add(item1);
        var items = _links[item1];

        if (items.Contains(target))
        {
            return true;
        }

        foreach (var item2 in items)
        {
            if (group.Contains(item2)) continue;
            var found = FindItem(item2, target, group);
            if (found) return found;
        }

        return false;
    }

    private void FindItem2(string item1, HashSet<string> group, (string a, string b, string c) filter, ref int hit)
    {
        group.Add(item1);
        var items = _links[item1];

        foreach (var item2 in items)
        {
            if (group.Contains(item2)) continue;

            if (item2 == filter.a || item2 == filter.b || item2 == filter.c)
            {
                hit++;
                continue;
            }

            FindItem2(item2, group, filter, ref hit);
        }
    }

    private (bool, int) FindGroup3(string init, (string a, string b, string c) filter)
    {
        var queue = new Queue<string>();
        queue.Enqueue(init);

        var group = new HashSet<string>();
        var hit = 0;

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            if (group.Contains(item)) continue;

            ProcessItem3(item, filter, group, queue, ref hit);
        }

        return (hit < 4, group.Count);
    }

    private void ProcessItem3(string item1, (string a, string b, string c) filter, HashSet<string> group, Queue<string> queue, ref int hit)
    {
        if(hit > 3) return;

        group.Add(item1);
        var items = _links[item1];

        foreach (var item2 in items)
        {
            if (group.Contains(item2)) continue;

            if (item2 == filter.a || item2 == filter.b || item2 == filter.c)
            {
                hit++;
                continue;
            }

            queue.Enqueue(item2);
        }
    }

    private void FindItem4(string item1, HashSet<string> group, (string a, string b, string c) filter, ref int hit)
    {
        if (hit > 3) return;

        group.Add(item1);
        var items = _links[item1];

        foreach (var item2 in items)
        {
            if (group.Contains(item2)) continue;

            if (item2 == filter.a || item2 == filter.b || item2 == filter.c)
            {
                hit++;
                continue;
            }

            FindItem4(item2, group, filter, ref hit);
        }
    }
#endif