#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day01 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 1;

    private List<int> _data;

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
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
        var result = 0;
        var previous = 0;

        foreach (var item in _data)
        {
            if (previous == 0)
            {
                previous = item;
                continue;
            }

            if (item > previous)
            {
                result++;
            }

            previous = item;
        }

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = 0;
        var previous = Three(0);
        var current = previous;

        for (int i = 1; i < _data.Count - 2; i++)
        {
            current = Three(i);

            if (current > previous)
            {
                result++;
            }

            previous = current;
        }

        Result = result;
    }

    private int Three(int v)
    {
        return _data[v] + _data[v + 1] + _data[v + 2];
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