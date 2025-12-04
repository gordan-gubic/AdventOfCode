#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day07 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 7;

    private List<string> _data;
    private List<List<long>> _equations;

    public Day07(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "1298103531759";
        ExpectedProd2 = "140575048428831";
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
        Result = CountValid1();
    }

    protected override void ComputePart2()
    {
        Result = CountValid2();
    }

    private long CountValid1()
    {
        var sum = 0L;

        foreach (var equation in _equations)
        {
            var isOk = CheckEquation2X(equation);
            if (isOk) sum += equation[0];
        }

        return sum;
    }

    private long CountValid2()
    {
        var sum = 0L;

        foreach (var equation in _equations)
        {
            var isOk = CheckEquation3X(equation);
            if (isOk) sum += equation[0];
        }

        return sum;
    }

    private bool CheckEquation2X(List<long> equation)
    {
        return Add2X(equation[1], equation, equation[0], 1);
    }

    private bool CheckEquation3X(List<long> equation)
    {
        return Add3X(equation[1], equation, equation[0], 1);
    }

    private bool Add2X(long current, List<long> equation, long target, int level)
    {
        if (level >= equation.Count - 1) return current == target;

        level++;

        var valueLeft = current + equation[level];
        var valueRight = current * equation[level];

        var result1 = Add2X(valueLeft, equation, target, level);
        var result2 = Add2X(valueRight, equation, target, level);

        return result1 || result2;
    }

    private bool Add3X(long current, List<long> equation, long target, int level)
    {
        if (level >= equation.Count - 1) return current == target;

        level++;

        var value1 = current + equation[level];
        var value2 = current * equation[level];
        var value3 = $"{current}{equation[level]}".ToLong();

        var result1 = Add3X(value1, equation, target, level);
        var result2 = Add3X(value2, equation, target, level);
        var result3 = Add3X(value3, equation, target, level);

        return result1 || result2 || result3;
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