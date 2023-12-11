#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day07 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 07;

    private List<string> _data;
    private List<CamelCards> _hands;

    public Day07(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "249748283";
        Expected2 = "248029057";
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
        var hands = _hands.ToList();
        hands.ForEach(h => h.CalculateHand());

        var result = CalculateEarnings();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var hands = _hands.ToList();
        hands.ForEach(h => h.CalculateHand(true));

        var result = CalculateEarnings();

        Result = result;
    }

    private long CalculateEarnings()
    {
        var sum = 0L;
        var count = _hands.Count;

        var ordered = _hands.OrderBy(x => x.Value).ThenBy(x => x.Raw).ToList();

        for (var i = count; i > 0; i--)
        {
            var hand = ordered[i - 1];
            var value = hand.Bet * i;

            // (i, hand.Bet, hand.Hand, hand.Value, hand.High, hand.Raw, value).Dump();

            sum += value;
        }

        return sum;
    }

    private void UpdateCards()
    {

    }

    protected override void ProcessData()
    {
        /*
32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
         */
        var hands = new List<CamelCards>();

        base.ProcessData();

        foreach (var line in _data)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var x = parts[0];
            var y = parts[1].ToLong();

            var hand = new CamelCards { Hand = x, Bet = y };

            hands.Add(hand);
        }

        _hands = hands;
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
        // _hands.DumpJson();
    }
}

#if DUMP
#endif