#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days;

public class Day23 : Day
{
    private List<string> _source;
    private List<string> _data;

    private List<int> _cups;

    public Day23(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2020;
        Parser.Day = 23;
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;

        _cups = new List<int>();
        _data[0].ToList().ForEach(x => _cups.Add(x.ToString().ToInt()));
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var target = 100;
        var cups = _cups.ToList();

        cups = RotateCupsForCycles(cups, target);

        Result = ComputeCups(cups);
    }

    protected override void ComputePart2()
    {
        var target = 1000;
        var cups = _cups.ToList();
        var size = cups.Count;

        for (int i = size; i < 1000; i++)
        {
            cups.Add(i + 1);
        }

        cups = RotateCupsForCycles(cups, target);

        var index0 = cups.IndexOf(1);
        var index1 = (index0 + 1) % size;
        var index2 = (index0 + 2) % size;

        Info($"i..: {cups.GetValueSafe(index0)}");
        Info($"i+1: {cups.GetValueSafe(index1)}");
        Info($"i+2: {cups.GetValueSafe(index2)}");

        Result = cups.Count;
    }

    private long ComputeCups(int[] cups)
    {
        Queue<int> qCups = new Queue<int>(cups);

        while (true)
        {
            var cup = qCups.Dequeue();

            if (cup == 1) break;

            qCups.Enqueue(cup);
        }

        var value = string.Join("", qCups);

        return value.ToLong();
    }

    private long ComputeCups(List<int> cups)
    {
        Queue<int> qCups = new Queue<int>(cups);

        while (true)
        {
            var cup = qCups.Dequeue();

            if (cup == 1) break;

            qCups.Enqueue(cup);
        }

        var value = string.Join("", qCups);

        return value.ToLong();
    }

    private List<int> RotateCupsForCycles(List<int> cups, int cycles)
    {
        var size = cups.Count;

        for (int i = 0; i < cycles; i++)
        {
            // Debug($"Turn: [{i + 1}]");

            if (i % 100000 == 0) Info($"Turn: [{i + 1}]...");

            var index = i % size;
            RotateCups(cups, index);
        }

        Debug($"Cups: {cups.ToJson()} After: [{cycles}]...");

        return cups;
    }

    private void RotateCups(List<int> cups, in int index)
    {
        var current = cups[index];
        var size = cups.Count;
        var taken = new List<int>();
        // Debug($"Cups: {cups.ToJson()} Current cup: [{current}]...");

        var newCups = new int[size];

        for (int i = 0; i < 3; i++)
        {
            var takenIndex = (index + i + 1) % size;
            taken.Add(cups[takenIndex]);
        }

        var destination = current - 1;
        while (true)
        {
            if (destination == 0) destination = size;

            if (!taken.Contains(destination)) break;

            destination--;
        }

        taken.ForEach(x => cups.Remove(x));

        var destinationIndex = cups.IndexOf(destination);

        cups.InsertRange(destinationIndex + 1, taken);

        while (true)
        {
            if (current == cups[index]) break;

            cups.Add(cups[0]);
            cups.RemoveAt(0);
        }

        // Debug($"Current cup: [{current}], Destination: [{destination}] Taken: {taken.ToJson()} Cups: {cups.ToJson()}");
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _cups.DumpJson("_cups");
    }
}

#if DUMP
#endif