#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day01 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 1;

    private List<int> _data;
    private readonly Dictionary<int, long> _elves = new ();

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "69528";
        Expected2 = "206152";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse(Converters.ToInt);
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = _elves.Values.Max();
    }

    protected override void ComputePart2()
    {
        Result = _elves.Values.OrderDescending().Take(3).Sum();
    }

    protected override void ProcessData()
    {
        base.ProcessData();
    
        var index = 0;
        _elves[index] = 0;

        foreach (var value in _data)
        {
            if (value == 0)
            {
                _elves[++index] = 0;
            }

            _elves[index] += value;
        }

        _elves.DumpCollection();
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