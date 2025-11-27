#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day22 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 22;

    private List<long> _data;

    public Day22(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "13753970725";
        Expected2 = "";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse(Convert);
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = 0L;

        Result = ComputeSecretNumbers();
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        Result = result;
    }

    private long ComputeSecretNumbers()
    {
        var sum = 0L;

        foreach (var number in _data)
        {
            // sum += ComputeSecretNumber(number);
            var x = ComputeSecretNumber(number);
            // x.Dump();
            sum += x;
        }

        return sum;
    }

    private long ComputeSecretNumber(long number, int count = 2000)
    {
        var r = number;
        for (var i = 0; i < count; i++)
        {
            r = MixPrune(r * 64, r);
            r = MixPrune(r / 32, r);
            r = MixPrune(r * 2048, r);
            // r.Dump();
        }
        return r;
    }

    private long MixPrune(long number, long secret)
    {
        return (number ^ secret) % 16777216;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        // Gromit do something!
        foreach (var line in _data)
        {
        }
    }

    private long Convert(string input)
    {
        return input.ToLong();
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