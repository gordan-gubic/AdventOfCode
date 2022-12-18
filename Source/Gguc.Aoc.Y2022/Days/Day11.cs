#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day11 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 11;

    private List<string> _data;
    private Dictionary<int, Monkey> _monkeys;
    private Dictionary<int, long> _inspections;
    private int _count;

    public Day11(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "117640";
        Expected2 = "30616425600";
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
        ProcessData();

        RunCycles(20, 3);
        var tops = _inspections.Values.OrderByDescending(x => x).Take(2).ToList();

        Result = tops[0] * tops[1];
    }

    protected override void ComputePart2()
    {
        ProcessData();

        RunCycles(10000, 1);
        var tops = _inspections.Values.OrderByDescending(x => x).Take(2).ToList();

        Result = tops[0] * tops[1];
    }

    private void RunCycles(int value, int worry)
    {
        long supermodulo = 1L;

        foreach (var monkey in _monkeys)
        {
            supermodulo *= monkey.Value.Divisor;
        }

        for (int i = 0; i < value; i++)
        {
            RunCycle(i, worry, supermodulo);
        }

        // _monkeys.Values.DumpJsonIndented("_monkeys");
        // _inspections.DumpJsonIndented("_inspections");
    }

    private void RunCycle(int value, int worry, long supermodulo)
    {
        foreach (var (_, monkey) in _monkeys)
        {
            _inspections[monkey.Id] += monkey.Items.Count;

            while (monkey.Items.Count > 0)
            {
                var item = monkey.Items.Dequeue();

                var newItem = monkey.Operation(item) / worry;
                newItem %= supermodulo;

                var isTrue = newItem % monkey.Divisor == 0;

                if (isTrue)
                {
                    _monkeys[monkey.True].Items.Enqueue(newItem);
                }
                else
                {
                    _monkeys[monkey.False].Items.Enqueue(newItem);
                }
            }
        }
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _monkeys = new Dictionary<int, Monkey>();
        _inspections = new Dictionary<int, long>();

        var id = 0;
        for (var i = 0; i < _data.Count; i += 7)
        {
            var items = ParseItems(_data[i + 1]);
            var operation = ParseOperation(_data[i + 2]);
            var divisor = ParseDivisor(_data[i + 3]);
            var ifTrue = ParseIfTrue(_data[i + 4]);
            var ifFalse = ParseIfFalse(_data[i + 5]);

            var monkey = new Monkey
            {
                Id = id,
                Operation = operation,
                Items = items,
                Divisor = divisor,
                True = ifTrue,
                False = ifFalse,
            };

            _monkeys[id] = monkey;
            _inspections[id] = 0L;
            id++;
        }

        _count = _monkeys.Count;
    }

    private Queue<long> ParseItems(string input)
    {
        input = input.Replace("Starting items: ", "").Trim();
        var list = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToList();
        return new Queue<long>(list);
    }

    private Func<long, long> ParseOperation(string input)
    {
        input = input.Replace("Operation: new = old ", "").Trim();

        if (input == "* old") return i => i * i;
        else if (input.StartsWith("* ")) return i => i * input.Replace("* ", "").ToLong();
        else if (input.StartsWith("+ ")) return i => i + input.Replace("+ ", "").ToLong();

        return i => i;
    }

    private int ParseDivisor(string input)
    {
        input = input.Replace("Test: divisible by ", "").Trim();
        var value = input.ToInt();
        return value;
    }

    private int ParseIfTrue(string input)
    {
        input = input.Replace("If true: throw to monkey ", "").Trim();
        var value = input.ToInt();
        return value;
    }

    private int ParseIfFalse(string input)
    {
        input = input.Replace("If false: throw to monkey ", "").Trim();
        var value = input.ToInt();
        return value;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
    }
}

#if DUMP
#endif