#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

using System.Text.RegularExpressions;

public class Day07 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 07;

    private static int _offset;
    private static int _workerCount;

    private List<string> _data;
    private HashSet<char> _nodes;
    private HashSet<char> _processing;
    private HashSet<(char, char)> _links;
    private List<Worker> _workers;

    public Day07(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "BGJCNLQUYIFMOEZTADKSPVXRHW";
        Expected2 = "1017";
    }

    protected override void InitParser()
    {
        InitTest();
        InitProd();

        _data = Parser.Parse();
    }

    private void InitTest()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Example;

        _offset = 0;
        _workerCount = 2;
    }

    private void InitProd()
    {
        Parser.Type = ParserFileType.Real;

        _offset = 60;
        _workerCount = 5;
    }

    #region Parse
    protected override void ProcessData()
    {
        var pattern = @"Step (?'a1'\w) must be finished before step (?'a2'\w) can begin.";

        _nodes = new();
        _processing = new();
        _links = new();
        _workers = new();

        // Gromit do something!
        foreach (var line in _data)
        {
            foreach (Match m in Regex.Matches(line, pattern))
            {
                var a1 = m.GroupValue("a1")[0];
                var a2 = m.GroupValue("a2")[0];

                _nodes.Add(a1);
                _nodes.Add(a2);
                _links.Add((a1, a2));
            }
        }

        foreach (var _ in Enumerable.Range(0, _workerCount))
        {
            _workers.Add(new());
        }
    }
    #endregion

    #region Head
    protected override void ComputePart1()
    {
        var order = CalculateOrder();
        Log.Info($"Order=[{order}]");

        Result = -1;
    }

    protected override void ComputePart2()
    {
        ProcessData();

        Result = CalculateTime();
    }

    #endregion

    #region Body
    private string CalculateOrder()
    {
        var response = "";

        while (_nodes.Any())
        {
            response += GetNext();
        }

        return response;
    }

    private long CalculateTime()
    {
        var current = 0;

        while (_nodes.Any())
        {
            SetNodes(current);

            current = ProcessNodes();
        }

        return current;
    }

    private char GetNext()
    {
        var ch = GetFrees().OrderBy(x => x).FirstOrDefault();
        Release(ch);
        return ch;
    }

    private List<char> GetFrees()
    {
        var temp = _links.Where(x => _nodes.Contains(x.Item2)).Select(x => x.Item2);
        var frees = _nodes.Except(temp).Except(_processing).ToList();

        return frees;
    }

    private void SetNodes(int time)
    {
        var frees = GetFrees();
        if (!frees.Any()) return;

        var available = _workers.Where(x => x.Processing == 0).ToList();
        var temp = frees.OrderBy(x => x).Take(available.Count);

        var count = Math.Min(available.Count, temp.Count());

        for (var i = 0; i < count; i++)
        {
            var ch = frees[i];
            available[i].Work(ch, time);
            _processing.Add(ch);
        }
    }

    private int ProcessNodes()
    {
        var worker = _workers.Where(x => x.Processing > 0).OrderBy(x => x.Time).First();
        var time = worker.Time;

        var ch = worker.Processing;
        Release(ch);
        worker.Processing = default;

        return time;
    }

    private void Release(char ch)
    {
        _links.RemoveWhere(x => x.Item1 == ch);
        _nodes.Remove(ch);
        _processing.Remove(ch);
    }

    internal static int CharValue(char ch)
    {
        return ch - 'A' + 1 + _offset;
    }
    #endregion

    #region Dump
    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();

        // _nodes.DumpJson("_nodes");
        // _links.DumpJson("links");

        // CharValue('A').Dump();
        // CharValue('Z').Dump();
    }
    #endregion
}

#if DUMP
#endif