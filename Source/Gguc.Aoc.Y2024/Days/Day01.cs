#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day01 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 1;

    private List<string> _data;
    private List<long> _list1;
    private List<long> _list2;
    private Dictionary<long, int> _dict2;

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "1830467";
        Expected2 = "26674158";
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
        Result = CalculateDistance();
    }

    protected override void ComputePart2()
    {
        Result = CalculateSimilarities();
    }

    private long CalculateDistance()
    {
        var size = _list1.Count;
        var distance = 0L;

        for (var i = 0; i < size; i++)
        {
            distance += Math.Abs(_list1[i] - _list2[i]);
        }

        return distance;
    }

    private long CalculateSimilarities()
    {
        var size = _list1.Count;
        var distance = 0L;

        for (var i = 0; i < size; i++)
        {
            var x = _list1[i];
            var multi = _dict2.ContainsKey(x) ? _dict2[x] : 0;
            distance += x * multi;
        }

        return distance;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _list1 = new();
        _list2 = new();
        _dict2 = new();

        // Gromit do something!
        foreach (var line in _data)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var v1 = parts[0].ToLong();
            var v2 = parts[1].ToLong();

            _list1.Add(v1);
            _list2.Add(v2);

            if (!_dict2.ContainsKey(v2)) _dict2[v2] = 0;
            _dict2[v2]++;
        }

        _list1.Sort();
        _list2.Sort();

        _list1.Dump();
        _list2.Dump();
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