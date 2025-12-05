#define LOG

namespace Gguc.Aoc.Y2025.Days;

public class Day03 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 03;

    private List<string> _raw;
    private List<List<int>> _data;

    public Day03(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = true;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "357";
        ExpectedTest2 = "3121910778619";

        ExpectedProd1 = "17229";
        ExpectedProd2 = "170520923035051";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        _raw = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = TotalJoltage(2);
    }

    protected override void ComputePart2()
    {
        Result = TotalJoltage(12);
    }

    private long TotalJoltage(int digits)
    {
        var result = 0L;

        foreach (var list in _data)
        {
            var max = MaxJoltage(list, digits);
            result += max;

            Debug($"{new { max }}");
        }

        return result;
    }

    private long MaxJoltage(List<int> list, int digits)
    {
        var joltage = 0L;
        var index = 0;

        while (digits > 0)
        {
            var l1 = list[index..^(digits - 1)];
            var m1 = l1.Max();
            index += l1.IndexOf(m1) + 1;

            joltage += m1 * (long)Math.Pow(10, digits - 1);

            digits--;
        }

        return joltage;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _data = [];

        // Gromit do something!
        foreach (var line in _raw)
        {
            var numbers = line.ToCharArray().Select(x => x.ToInt()).ToList();
            _data.Add(numbers);
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpJson();
    }
}

#if DUMP
#endif