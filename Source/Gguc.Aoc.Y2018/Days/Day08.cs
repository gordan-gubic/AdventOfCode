#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day08 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 08;

    private List<string> _data;
    private List<int> _values;
    private Dictionary<Guid, Node08> _nodes;
    private Node08 _root;

    public Day08(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "36027";
        Expected2 = "23960";
    }

    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    #region Parse
    protected override void ProcessData()
    {
        _values = new();
        _nodes = new();

        var parts = _data[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var p in parts)
        {
            _values.Add(p.ToInt());
        }
    }
    #endregion

    #region Head
    protected override void ComputePart1()
    {
        ProcessValues();

        Result = _root.TotalSum;
    }

    protected override void ComputePart2()
    {
        Result = _root.Value;
    }
    #endregion

    #region Body

    private void ProcessValues()
    {
        var queue = new Queue<int>(_values);

        while (queue.Any())
        {
            ProcessRoot(queue);
        }
    }

    private void ProcessRoot(Queue<int> queue)
    {
        _root = ProcessNextNode(queue);
    }

    private Node08 ProcessNextNode(Queue<int> queue)
    {
        var node = new Node08();
        var v1 = queue.Dequeue();
        var v2 = queue.Dequeue();

        for (var i = 0; i < v1; i++)
        {
            node.Children.Add(ProcessNextNode(queue));
        }

        for (var i = 0; i < v2; i++)
        {
            node.Values.Add(queue.Dequeue());
        }
        
        node.GetTotalSum();
        node.GetValue();
        _nodes[node.Id] = node;

        return node;
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

        // _values.DumpCollection("_values");
    }
    #endregion
}

#if DUMP
#endif