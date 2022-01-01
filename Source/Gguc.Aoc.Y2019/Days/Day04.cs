#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2019.Days;

public class Day04 : Day
{
    private const int YEAR = 2019;
    private const int DAY = 4;

    private List<(int, int)> _source;
    private List<(int, int)> _data;

    private int _min;
    private int _max;

    public Day04(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse(ConvertInput);
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;

        _min = _data[0].Item1;
        _max = _data[0].Item2;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        for (int i = _min; i <= _max; i++)
        {
            Add(VerifyNumber1(i));
        }
    }

    protected override void ComputePart2()
    {
        for (int i = _min; i <= _max; i++)
        {
            Add(VerifyNumber2(i));
        }
    }

    private bool VerifyNumber1(in int input)
    {
        var chars = input.ToString().ToList();
        var doubles = new List<bool>();
        var highers = new List<bool>();

        for (int i = 1; i < chars.Count; i++)
        {
            doubles.Add(chars[i] == chars[i - 1]);
            highers.Add(chars[i] >= chars[i - 1]);
        }

        var r = doubles.Any(x => x) && highers.All(x => x);

        return r;
    }

    private bool VerifyNumber2(in int input)
    {
        var chars = input.ToString().ToList();
        var doubles = new Dictionary<int, int>();
        var highers = new List<bool>();

        doubles[chars[0]] = 1;

        for (int i = 1; i < chars.Count; i++)
        {
            if (doubles.ContainsKey(chars[i]))
            {
                doubles[chars[i]] = doubles[chars[i]] + 1;
            }
            else
            {
                doubles[chars[i]] = 1;
            }

            highers.Add(chars[i] >= chars[i - 1]);
        }

        var r = doubles.Any(x => x.Value == 2) && highers.All(x => x);

        // if(r) Log.Trace($"{input}");
        // if(!r) Log.Trace($"{input}");

        return r;
    }

    private (int, int) ConvertInput(string input)
    {
        var parts = input.Split('-', StringSplitOptions.RemoveEmptyEntries);
        return (parts[0].ToInt(), parts[1].ToInt());
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        Log.DebugLog(ClassId);

        _data[0].DumpJson("Item");
    }
}

#if DUMP

#endif