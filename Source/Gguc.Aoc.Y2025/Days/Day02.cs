#define LOGx

namespace Gguc.Aoc.Y2025.Days;

public class Day02 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 02;

    private List<string> _raw;
    private List<(long, long)> _data;

    public Day02(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = true;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "1227775554";
        ExpectedTest2 = "4174379265";

        ExpectedProd1 = "43952536386";
        ExpectedProd2 = "54486209192";
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
        Result = CountValidPairs(IsValidNumber1);
    }

    protected override void ComputePart2()
    {
        Result = CountValidPairs(IsValidNumber2);
    }

    private long CountValidPairs(Predicate<long> action)
    {
        var result = 0L;

        foreach (var pair in _data)
        {
            var isValid = IsValidPair(pair, out var numbers, action);
            if (!isValid)
            {
                foreach (var n in numbers)
                {
                    Debug($"{new { n }}");
                    result += n;
                }
            }

            Debug($"{new { pair, isValid }}");
        }

        return result;
    }

    private bool IsValidPair((long, long) pair, out List<long> number, Predicate<long> action)
    {
        var isValid = true;
        number = [];

        for (var i = pair.Item1; i <= pair.Item2; i++)
        {
            var isValidNumber = action(i);
            if (!isValidNumber)
            {
                isValid = false;
                number.Add(i);
            }
        }

        return isValid;
    }

    private bool IsValidNumber1(long value)
    {
        var vx = value.ToString();
        var size = vx.Length;
        var isOdd = int.IsOddInteger(size);

        if (isOdd)
        {
            return true;
        }

        var half = size / 2;

        var p1 = vx[..half].ToLong();
        var p2 = vx[half..].ToLong();

        return p1 != p2;
    }

    private bool IsValidNumber2(long value)
    {
        var vx = value.ToString();
        var size = vx.Length;

        var half = size / 2;
        while (half > 0)
        {
            if (size % half != 0)
            {
                half--;
                continue;
            }

            var isValid = false;

            for (var i = 0; i < size - half; i += half)
            {
                var i1 = half + i;
                var i2 = i1 + half;

                var p1 = vx[i..i1].ToLong();
                var p2 = vx[i1..i2].ToLong();

                if (p1 != p2)
                {
                    isValid = true;
                    break;
                }
            }

            if (!isValid) return false;

            half--;
        }

        return true;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _data = [];

        // Gromit do something!
        foreach (var line in _raw)
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray();

            foreach (var part in parts)
            {
                var px = part.Split('-', StringSplitOptions.RemoveEmptyEntries).ToArray();
                var p1 = px[0].ToLong();
                var p2 = px[1].ToLong();
                _data.Add((p1, p2));
            }
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        // _raw.DumpCollection();
        _data.DumpCollection();
    }
}

#if DUMP
#endif