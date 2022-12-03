#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day03 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 3;

    private List<char[]> _data;
    private List<(char[], char[])> _rucksacks;

    public Day03(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
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
        Result = FindIntersect1().Sum();
    }

    protected override void ComputePart2()
    {
        Result = FindIntersect2().Sum();
    }

    private List<int> FindIntersect1()
    {
        var values = new List<int>();

        foreach (var (p1, p2) in _rucksacks)
        {
            var value = p1.Intersect(p2).FirstOrDefault();
            values.Add(value.ToPriorityValue());
        }

        return values;
    }

    private List<int> FindIntersect2()
    {
        var values = new List<int>();

        for (int i = 0; i < _data.Count; i += 3)
        {
            var set0 = _data[i];
            var set1 = _data[i + 1];
            var set2 = _data[i + 2];

            var value = set0.Intersect(set1).Intersect(set2).FirstOrDefault();
            values.Add(value.ToPriorityValue());
        }

        return values;
    }

    /*
     * As extension method
     * private int PriorityValue(char value) => value - ((char.IsLower(value)) ? 96 : 38);
     */

    protected override void ProcessData()
    {
        base.ProcessData();

        _rucksacks = new();

        foreach (var values in _data)
        {
            var count = values.Count() / 2;
            var part1 = values.Take(count).ToArray();
            var part2 = values.Skip(count).Take(count).ToArray();
            _rucksacks.Add((part1, part2));
        }
    }

    private char[] Convert(string input)
    {
        return input.ToCharArray();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();

        "Day[202203] - Part 01 *** Result: [7691]!".Dump();
        "Day[202203] - Part 02 *** Result: [2508]!".Dump();
    }
}

#if DUMP
#endif