#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days;

public class Day15 : Day
{
    private List<long> _source;
    private List<long> _data;

    public Day15(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2020;
        Parser.Day = 15;
        Parser.Type = ParserFileType.Real;

        _source = Parser.ParseSequence(Converters.ToLong);
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = ComputeResult(2020L);
    }

    protected override void ComputePart2()
    {
        Result = ComputeResult(30000000L);
    }

    protected long ComputeResult(long target)
    {
        var turn = 0L;
        var queue = new Queue<long>(_data);
        var dict = new Dictionary<long, long>();
        var number = 0L;

        target--;

        long AddNumber()
        {
            Debug($"Turn=[{turn,8}]: Number=[{number}]");
            var diff = 0L;

            if (dict.ContainsKey(number))
            {
                diff = turn - dict[number];
            }

            dict[number] = turn;
            return diff;
        }

        do
        {
            turn++;

            if (queue.Count > 0)
            {
                number = queue.Dequeue();
                number = AddNumber();
                continue;
            }

            number = AddNumber();

            if (turn >= target) break;
        } while (true);

        // dict.DumpJson("dict");
        // counts.DumpJson("counts");

        return number;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data[0].Dump("Item");
        _data.DumpCollection("List");
    }
}

#if DUMP
#endif