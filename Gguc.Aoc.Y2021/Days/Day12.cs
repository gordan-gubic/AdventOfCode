#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day12 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 12;

    private List<string> _source;
    private Dictionary<string, HashSet<string>> _nodes;
    private HashSet<string> _bigCaves = new HashSet<string>();
    private HashSet<string> _smallCaves = new HashSet<string>();
    private Func<List<string>, string, bool> _repeatCheck;

    public Day12(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Day = 12;
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _nodes = new Dictionary<string, HashSet<string>>();

        foreach (var line in _source)
        {
            var parts = line.Split('-');

            AddNode(_nodes, parts[0], parts[1]);
            AddNode(_nodes, parts[1], parts[0]);
        }

        foreach (var key in _nodes.Keys)
        {
            if (key == "start" || key == "end") continue;

            if (key == key.ToUpperInvariant()) _bigCaves.Add(key);
            else _smallCaves.Add(key);
        }

        _bigCaves.DumpJson("_bigCaves");
        _smallCaves.DumpJson("_smallCaves");
    }

    private void AddNode(Dictionary<string, HashSet<string>> nodes, string node1, string node2)
    {
        if (node2 == "start" || node1 == "end") return;

        if (!nodes.ContainsKey(node1))
            nodes[node1] = new HashSet<string>();

        nodes[node1].Add(node2);
    }

    protected override void ComputePart1()
    {
        _repeatCheck = IsReapeat1;
        Result = FindPaths();
    }

    protected override void ComputePart2()
    {
        _repeatCheck = IsReapeat2;
        Result = FindPaths();
    }

    private long FindPaths()
    {
        var nodes = CloneNodes(_nodes);

        var total = ProcessStart(nodes);

        return total;
    }

    private long ProcessStart(Dictionary<string, List<string>> allnodes)
    {
        var total = 0L;
        var node = "start";
        var path = new List<string> { node };

        var nodes = CloneNodes(_nodes);

        var queue = nodes[node].ToList();

        for (int i = 0; i < queue.Count; i++)
        {
            var next = queue[i];

            total += ProcessNode(nodes, next, new List<string>(path));
        }

        return total;
    }

    private long ProcessNode(Dictionary<string, List<string>> allnodes, string node, List<string> path)
    {
        if (_repeatCheck(path, node))
        {
            // $"Break-at-path:{path.ToJson()}+{node}".Dump();
            return 0;
        }

        path.Add(node);

        if (node == "end")
        {
            // $"End-at-path:{path.ToJson()}+{node}".Dump();
            return 1;
        }

        var total = 0L;

        var queue = new Queue<string>(allnodes[node]);
        while (queue.Count > 0)
        {
            var next = queue.Dequeue();

            var newPath = new List<string>(path);
            total += ProcessNode(allnodes, next, newPath);
        }

        return total;
    }

    private bool IsReapeat1(List<string> path, string node)
    {
        return _smallCaves.Contains(node) && path.Contains(node);
    }

    private bool IsReapeat2(List<string> path, string node)
    {
        if (!_smallCaves.Contains(node) || !path.Contains(node))
        {
            return false;
        }

        if (path.Contains("*")) return true;

        if (path.Contains(node))
        {
            path.Add("*");
        }

        return false;
    }

    private Dictionary<string, List<string>> CloneNodes(Dictionary<string, HashSet<string>> nodes)
    {
        var clone = new Dictionary<string, List<string>>();
        foreach (var item in nodes)
        {
            var hs = new List<string>(item.Value);
            clone[item.Key] = hs;
        }
        return clone;
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

        _nodes.DumpJson("Nodes");
    }
    #endregion Dump
}

#if DUMP
#endif