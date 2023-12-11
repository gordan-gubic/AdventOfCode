#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day09 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 09;

    private List<string> _data;
    private List<List<long>> _history;

    public Day09(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "1772145754";
        Expected2 = "867";
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
        var history = _history.ToList();
        var result = SumValues(history);

        Result = result;
    }

    protected override void ComputePart2()
    {
        var history = _history.ToList();
        var result = SumValues(history, true);

        Result = result;
    }

    private long SumValues(List<List<long>> history, bool reverse = false)
    {
        var sum = 0L;

        if (reverse)
        {
            for (int i = 0; i < history.Count; i++)
            {
                history[i].Reverse();
            }
        }

        foreach (var list in history)
        {
            sum += CalulateNext(list);
        }

        return sum;
    }

    private long CalulateNext(List<long> list)
    {
        var predictions = new List<List<long>>();

        FillPredictions(predictions, list);

        CalulatePredictions(predictions);

        // predictions.DumpJson();

        return predictions.First().Last();
    }

    private void FillPredictions(List<List<long>> predictions, List<long> list)
    {
        predictions.Add(list);

        if(list.All(x => x == 0)) return;

        var newlist = new List<long>();
        var count = list.Count - 1;

        for (var i = 0; i < count; i++)
        {
            newlist.Add(list[i+1] - list[i]);
        }

        FillPredictions(predictions, newlist);
    }

    private void CalulatePredictions(List<List<long>> predictions)
    {
        var count = predictions.Count;
        var current = 0L;
        
        for (int i = count - 1; i >= 0; i--)
        {
            var value = predictions[i].Last();
            value += current;
            current = value;
            predictions[i].Add(current);
        }
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var history = new List<List<long>>();
        foreach (var line in _data)
        {
            history.Add(line.Split(' ').Select(x => x.ToLong()).ToList());
        }

        _history = history;
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

        // _history.DumpJson();
    }
}

#if DUMP
#endif