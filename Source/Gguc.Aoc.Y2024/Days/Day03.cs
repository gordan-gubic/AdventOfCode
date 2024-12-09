#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day03 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 3;

    private List<string> _data;

    public Day03(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "174336360";
        Expected2 = "88802350";
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
        Result = MultiplyValues();
    }

    protected override void ComputePart2()
    {
        Result = MultiplyValues2();
    }

    private long MultiplyValues()
    {
        var sum = 0L;
        var pattern = @"mul\(\d+,\d+\)";

        foreach (var line in _data)
        {
            var matches = line.Matches(pattern, RegexOptions.Singleline);

            foreach (var match in matches)
            {
                var temp = match.Remove(0, 4).TrimEnd(')').Split(',', StringSplitOptions.RemoveEmptyEntries);
                var x = temp[0].ToInt();
                var y = temp[1].ToInt();

                sum += x * y;
            }
        }

        return sum;
    }

    private long MultiplyValues2()
    {
        var sum = 0L;
        var pattern = @"(don't\(\)|do\(\)|mul\(\d+,\d+\)|d)";

        var enabled = true;

        foreach (var line in _data)
        {
            var matches = line.Matches(pattern, RegexOptions.Singleline);

            foreach (var match in matches)
            {
                if (match == "do()")
                {
                    enabled = true;
                    continue;
                }

                if (match == "don't()")
                {
                    enabled = false;
                    continue;
                }

                var temp = match.Remove(0, 4).TrimEnd(')').Split(',', StringSplitOptions.RemoveEmptyEntries);
                var x = temp[0].ToInt();
                var y = temp[1].ToInt();

                if(enabled) sum += x * y;
            }
        }

        return sum;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        // Gromit do something!
        foreach (var line in _data)
        {
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

        _data.DumpCollection();
    }
}

#if DUMP
#endif