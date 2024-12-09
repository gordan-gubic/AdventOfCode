#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day07A : Day
{
    private const int YEAR = 2024;
    private const int DAY = 7;

    private List<string> _data;
    private List<List<long>> _equations;

    public Day07A(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
        Result = CountValid();
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        Result = result;
    }

    private long CountValid()
    {
        var sum = 0L;

        foreach (var equation in _equations)
        {
            var isOk = CheckEquation(equation);
            if (isOk) sum += equation[0];
        }

        return sum;
    }

    private bool CheckEquation(List<long> equation)
    {
        var target = equation[0];

        var root = new Node(null, equation[1], 1);
        var tree = new BinaryTree(root);
        var current = root;

        var found = FillTree(tree, current, equation, target);

        if (found.Item2)
        {
            equation[0].Dump("found");
        }

        return found.Item2;
    }

    private (bool, bool) FillTree(BinaryTree tree, Node current, List<long> equation, long target)
    {
        var result = AddLevel(tree, current, equation, target);

        return result;
    }

    private (bool, bool) AddLevel(BinaryTree tree, Node current, List<long> equation, long target)
    {
        var size = equation.Count - 1;
        if(current.Level >= size) return (false, false);
        
        var level = current.Level + 1;

        var valueLeft = current.Value + equation[level];
        var valueRight = current.Value * equation[level];

        // if (level == size && (valueLeft == target || valueRight == target)) return (true, true);

        var result1 = (false, false);
        var result2 = (false, false);

        if (valueLeft < target)
        {
            var node = new Node(current, valueLeft, level);
            current.Left = node;
            result1 = AddLevel(tree, node, equation, target);
        }

        if (valueRight < target)
        {
            var node = new Node(current, valueRight, level);
            current.Right = node;
            result2 = AddLevel(tree, node, equation, target);
        }

        return (result1.Item1 ||  result2.Item1, result1.Item2 || result2.Item2);
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _equations = new();

        // Gromit do something!
        foreach (var line in _data)
        {
            var e = line.Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToList();
            _equations.Add(e);
        }
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

        long.MaxValue.Dump();

        // _data.DumpCollection();
        // _equations.DumpJsonIndented("eq", true);
    }
}

#if DUMP
#endif