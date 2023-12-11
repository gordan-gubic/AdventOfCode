#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day04 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 04;

    private List<string> _data;
    private List<List<int>> _cards;

    private int _head = 10;

    public Day04(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "24160";
        Expected2 = "5659035";
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
        var result = PowerCards();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = SumCards();

        Result = result;
    }

    private long PowerCards()
    {
        var sum = 0L;

        foreach (var card in _cards)
        {
            var intersect = PowerCard(card);
            var result = (long)Math.Pow(2, intersect - 1);
            sum += result;
        }

        return sum;
    }

    private long SumCards()
    {
        var sum = 0L;

        var wins = new Dictionary<int, int>();
        var counts = new Dictionary<int, int>();

        foreach (var card in _cards)
        {
            var intersect = PowerCard(card);

            wins[card[0]] = intersect;
            counts[card[0]] = 1;
        }

        foreach (var (key, count) in wins)
        {
            var current = key;

            if(count == 0)continue;

            var coef = counts[current];

            for (var i = 1; i <= count; i++)
            {
                counts[current + i] += coef;
            }
        }

        sum = counts.Values.Sum();

        return sum;
    }

    private int PowerCard(List<int> card)
    {
        var set1 = new HashSet<int>();
        var set2 = new HashSet<int>();

        for (var i = 1; i <= _head; i++)
        {
            set1.Add(card[i]);
        }

        for (var i = _head + 1; i < card.Count; i++)
        {
            set2.Add(card[i]);
        }

        var intersect = set2.Intersect(set1).Count();
        return intersect;
    }

    protected override void ProcessData()
    {
        // Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53

        base.ProcessData();

        _cards = new List<List<int>>();

        foreach (var line in _data)
        {
            var list = new List<int>();

            var x = line.Replace("Card ", "").Replace(":", "").Replace("|", "");
            var xs = x.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            xs.ForEach(i => list.Add(i.ToInt()));

            _cards.Add(list);
        }

        _head = Parser.Type == ParserFileType.Real ? 10 : 5;
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

        // _data.DumpCollection();
        // _cards.DumpJson();
    }
}

#if DUMP
#endif