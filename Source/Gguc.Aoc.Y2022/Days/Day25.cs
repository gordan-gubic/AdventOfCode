#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day25 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 25;

    private List<string> _data;
    private List<long> _numbers;
    private List<long> _numbersShifted;

    public Day25(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "2=-0=1-0012-=-2=0=01";
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
        var result = 0L;

        Result = ProcessSnafu();
    }

    protected override void ComputePart2()
    {
        Result = 0L;
    }

    private long ProcessSnafu()
    {
        var sum = _numbers.Sum();

        Log.Info($"Snafu sum: {sum}");

        var snafu = ConvertToSnafu(sum);
        Log.Info($"Snafu: {snafu}");

        return sum;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _numbers = new();
        _numbersShifted = new();

        foreach (var line in _data)
        {
            var n = ConvertFromSnafu(line);
            n.Dump();

            _numbers.Add(n);
        }

        /*
        foreach (var line in _data)
        {
            var n = ConvertToSnafu(line.ToLong());
            n.Dump();
        }
        */
    }

    private long ConvertFromSnafu(string value) => NumberConverter.ConvertFromBase(value, "=-012", 2);

    private string ConvertToSnafu(long number) => NumberConverter.ConvertToSnafu(number);

    private long ConvertFromSnafuX(string value)
    {
        var chars = "=-012";
        var nbase = chars.Length;
        var offset = 2;

        value = value.ReverseString();
        var length = value.Length;
        var sum = 0L;

        for (var i = 0; i < length; i++)
        {
            sum += (chars.IndexOf(value[i]) - offset) * (long)Math.Pow(nbase, i);
        }

        return sum;
    }

    private string ConvertToSnafuX(long number)
    {
        var result = "";
        var chars = "=-012";
        var nbase = chars.Length;
        var offset = 2;

        while (number > 0)
        {
            var remainder = number % nbase;
            number /= nbase;

            if (remainder.IsBetween(0, 2))
            {
                result = $"{remainder}{result}";
            }
            else if (remainder > 2)
            {
                var ch = chars[(int)remainder - offset - 1];
                result = $"{ch}{result}";
                number++;
            }
        }

        return result;
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