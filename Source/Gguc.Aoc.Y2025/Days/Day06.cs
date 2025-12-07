#define LOG

namespace Gguc.Aoc.Y2025.Days;

public class Day06 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 06;

    private List<string> _raw;
    private List<List<string>> _data;

    private List<List<long>> _values;
    private List<char> _ops;

    private int _cephalopodsIndex = 0;

    public Day06(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();

        TestExample = true;
        ExecuteTest = true;
        ExecuteProd = true;

        ExpectedTest1 = "4277556";
        ExpectedTest2 = "3263827";

        ExpectedProd1 = "7326876294741";
        ExpectedProd2 = "10756006415204";
    }

    protected override void InitParser()
    {
        _raw = Parser.Parse();
    }

    protected override void ComputePart1()
    {
        Result = CountGroups();
    }

    protected override void ComputePart2()
    {
        _cephalopodsIndex = 0;
        Result = CountCephalopods();
    }

    private long CountGroups()
    {
        var result = 0L;

        for (int i = 0; i < _ops.Count; i++)
        {
            result += CountIndex(i);
        }

        return result;
    }

    private long CountCephalopods()
    {
        var result = 0L;

        for (int i = 0; i < _ops.Count; i++)
        {
            result += CountCephalopodIndex(i);
        }

        return result;
    }

    private long CountIndex(int i)
    {
        var ops = _ops[i];

        var values = _values.Select(x => x[i]).ToList();

        if (ops == '+') return AddValues(values);
        else return MultipleValues(values);
    }

    private long CountCephalopodIndex(int i)
    {
        var ops = _ops[i];

        var values = GetCephalopods(i);

        if (ops == '+') return AddValues(values);
        else return MultipleValues(values);
    }

    private List<long> GetCephalopods(int i)
    {
        var output = new List<long>();

        var values = _data.Select(x => x[i]).ToList();
        var sizes = _data.Select(x => x[i].Length).ToList();
        var max = sizes.Max();

        values.ForEach(x => x.PadLeft(max, ' '));

        var values2 = new List<string>();
        for (int k = 0; k < values.Count; k++)
        {
            values2.Add(_raw[k][_cephalopodsIndex..(_cephalopodsIndex+max)]);
        }

        for (int j = 0; j < max; j++)
        {
            var sb = new StringBuilder();
            for (int k = 0; k < values2.Count; k++)
            {
                sb.Append(values2[k][j]);
            }

            output.Add(sb.ToString().ToLong());
        }

        _cephalopodsIndex += max + 1;
        return output;
    }

    private long AddValues(List<long> values)
    {
        var result = 0L;

        foreach (var value in values)
        {
            result += value;
        }

        return result;
    }

    private long MultipleValues(List<long> values)
    {
        var result = 1L;

        foreach (var value in values)
        {
            result *= value;
        }

        return result;
    }

    protected override void ProcessData()
    {
        _data = [];
        _values = [];
        _ops = [];

        foreach (var line in _raw)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();

            if (parts[0] == "+" || parts[0] == "*")
            {
                _ops.AddRange(parts.Select(x => x[0]).ToList());
            }
            else
            {
                _values.Add(parts.Select(x => x.ToLong()).ToList());
                _data.Add(parts.Select(x => x).ToList());
            }
        }
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

        // _raw.DumpCollection();
        _values.DumpJson();
        _ops.DumpCollection();
    }
}

#if DUMP
#endif