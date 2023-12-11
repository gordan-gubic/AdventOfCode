#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.DaysBackup;

public class Day09_01 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 0901;

    private List<string> _data;
    private List<List<long>> _history;

    public Day09_01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
        var result = SumValuesNext();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = SumValuesPrevious();

        Result = result;
    }

    private long SumValuesNext()
    {
        var sum = 0L;

        foreach (var list in _history)
        {
            sum += CalulateNext(list);
        }

        return sum;
    }

    private long SumValuesPrevious()
    {
        var sum = 0L;

        foreach (var list in _history)
        {
            sum += CalulatePrevious(list);
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

    private long CalulatePrevious(List<long> list)
    {
        var predictions = new List<List<long>>();

        FillPredictionsPrevious(predictions, list);

        CalulatePredictionsPrevious(predictions);

        // predictions.DumpJson();

        return predictions.First().First();
    }

    private void FillPredictions(List<List<long>> predictions, List<long> list)
    {
        predictions.Add(list);

        if (list.All(x => x == 0)) return;

        var newlist = new List<long>();
        var count = list.Count - 1;

        for (var i = 0; i < count; i++)
        {
            newlist.Add(list[i + 1] - list[i]);
        }

        FillPredictions(predictions, newlist);
    }

    private void FillPredictionsPrevious(List<List<long>> predictions, List<long> list)
    {
        predictions.Add(list);

        if (list.All(x => x == 0)) return;

        var newlist = new List<long>();
        var count = list.Count - 2;

        for (var i = count; i >= 0; i--)
        {
            newlist.Insert(0, list[i + 1] - list[i]);
        }

        FillPredictionsPrevious(predictions, newlist);
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

    private void CalulatePredictionsPrevious(List<List<long>> predictions)
    {
        var count = predictions.Count;
        var current = 0L;

        for (int i = count - 1; i >= 0; i--)
        {
            var value = predictions[i].First();
            value -= current;
            current = value;
            predictions[i].Insert(0, current);
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