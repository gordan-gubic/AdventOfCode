#define LOG

namespace Gguc.Aoc.Y2015.Days;

public class Day05 : Day
{
    private const int YEAR = 2015;
    private const int DAY = 05;

    private List<string> _raw;
    private List<string> _data;

    public Day05(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = false;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "2";
        ExpectedTest2 = "2";

        ExpectedProd1 = "238";
        ExpectedProd2 = "69";
    }

    protected override void InitParser()
    {
        _raw = Parser.Parse();
        _data = Parser.Parse();
    }

    protected override void ComputePart1()
    {
        Result = CountNice(IsNice1);
    }

    protected override void ComputePart2()
    {
        Result = CountNice(IsNice2);
    }

    private long CountNice(Func<string, bool> predicate)
    {
        var result = 0L;

        foreach (var value in _data)
        {
            var isNice = predicate(value);
            if (isNice) result++;

            // Debug($"{new {value, isNice}}");
        }

        return result;
    }

    private bool IsNice1(string value)
    {
        if (value.Contains("ab") || value.Contains("cd") || value.Contains("pq") || value.Contains("xy")) return false;

        var isNice = false;
        for (var i = 0; i < value.Length - 1; i++)
        {
            if (value[i] - value[i + 1] == 0)
            {
                isNice = true;
                break;
            }
        }

        var count = value.ToCharArray().Count(x => x == 'a' || x == 'e' || x == 'i' || x == 'o' || x == 'u');
        return isNice && count >= 3;
    }

    private bool IsNice2(string value)
    {
        var isNice1 = false;
        for (var i = 0; i < value.Length - 3; i++)
        {
            for (int j = i + 2; j < value.Length - 1; j++)
            {
                if (value[i] == value[j] && value[i + 1] == value[j + 1])
                {
                    isNice1 = true;
                    break;
                }
            }
        }

        var isNice2 = false;
        for (var i = 0; i < value.Length - 2; i++)
        {
            if (value[i] == value[i + 2])
            {
                isNice2 = true;
                break;
            }
        }

        return isNice1 && isNice2;
    }

    protected override void ProcessData()
    {
    }

    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        _raw.DumpCollection();
    }
}

#if DUMP
#endif