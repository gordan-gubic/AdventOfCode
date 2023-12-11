#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day01 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 1;

    private List<string> _data;

    private readonly Dictionary<string, int> _numbers = new()
    {
        ["one"] = 1,
        ["two"] = 2,
        ["three"] = 3,
        ["four"] = 4,
        ["five"] = 5,
        ["six"] = 6,
        ["seven"] = 7,
        ["eight"] = 8,
        ["nine"] = 9,
    };

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "54573";
        Expected2 = "54591";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
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
        Result = SumNumbers();
    }

    protected override void ComputePart2()
    {
        Result = SumNumbers2();
    }

    protected override void ProcessData()
    {
        base.ProcessData();
    }

    private long SumNumbers()
    {
        var sum = 0L;

        _data.ForEach(x =>
        {
            sum += FindNumber(x);
        });

        return sum;
    }

    private long SumNumbers2()
    {
        var sum = 0L;

        _data.ForEach(x =>
        {
            x = ReplaceNumbers(x);
            sum += FindNumber(x);
        });

        return sum;
    }

    private string ReplaceNumbers(string s)
    {
        var sb = new StringBuilder();
        var temp = new StringBuilder();

        for (var i = 0; i < s.Length; i++)
        {
            temp.Clear();

            if (char.IsDigit(s[i]))
            {
                sb.Append(s[i]);
                continue;
            }

            for (var j = i; j < s.Length; j++)
            {
                temp.Append(s[j]);

                var x = ReplaceNumber(temp.ToString());

                if(x == "_") continue;

                sb.Append(x);

                if (char.IsDigit(x[0]) || x == "-") break;
            }
        }

        sb.Dump();

        return sb.ToString();
    }

    private string ReplaceNumber(string s)
    {
        var contains = false;

        foreach (var number in _numbers.Keys)
        {
            if (number == s)
            {
                return _numbers[number].ToString();
            }

            if (number.StartsWith(s)) contains = true;
        }

        return contains ? "_" : "-";
    }

    private long FindNumber(string s)
    {
        var list = new List<char>();

        s.ForEach(c =>
        {
            if (char.IsDigit(c)) list.Add(c);
        });

        return $"{list.First()}{list.Last()}".ToInt();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpCollection();
    }
}

#if DUMP
#endif