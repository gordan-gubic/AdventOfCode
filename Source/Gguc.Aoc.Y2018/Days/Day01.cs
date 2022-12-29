#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day01 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 1;

    private List<int> _data;

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "";
        Expected2 = "";
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
        var result = 0L;

        foreach (var value in _data)
        {
            result += value;
        }

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        var total = 0;
        var duplicate = false;
        var freqs = new HashSet<long>();

        while(true)
        {
            foreach (var value in _data)
            {
                total += value;

                if(freqs.Contains(total))
                {
                    duplicate = true;
                    break;
                }

                freqs.Add(total);
            }

            if(duplicate)
            {
                result = total;
                break;
            }
        }

        Result = result;
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

        //_data.DumpCollection();
    }
}

#if DUMP
#endif