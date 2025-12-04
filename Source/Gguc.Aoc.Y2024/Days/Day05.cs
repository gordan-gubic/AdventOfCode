#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day05 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 5;

    private List<string> _data;
    private Dictionary<int, HashSet<int>> _dict;
    private List<List<int>> _updates;

    public Day05(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "5248";
        ExpectedProd2 = "4507";
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
        Result = CalculateUpdates();
    }

    protected override void ComputePart2()
    {
        Result = CalculateCorrectedUpdates();
    }

    private long CalculateUpdates()
    {
        var sum = 0L;

        foreach (var update in _updates)
        {
            var (isOk, value) = ProcessUpdate(update);

            if(isOk) sum += value;
        }

        return sum;
    }

    private long CalculateCorrectedUpdates()
    {
        var sum = 0L;

        foreach (var update in _updates)
        {
            var (isOk, value) = ProcessUpdate(update);

            if (!isOk) sum += SortUpdate(update);
        }

        return sum;
    }

    private (bool, int) ProcessUpdate(List<int> update)
    {
        for (var i = 0; i < update.Count - 1; i++)
        {
            var x = update[i];
            var y = update[i + 1];

            if (!_dict.ContainsKey(x) || !_dict[x].Contains(y)) return (false, 0);
        }

        var mid = (update.Count - 1) / 2;
        return (true, update[mid]);
    }

    private int SortUpdate(List<int> update)
    {
        var comparor = new ManualComparer(_dict);
        var sorted = update.Order(comparor).ToList();

        var mid = (sorted.Count - 1) / 2;
        return sorted[mid];
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _dict = new Dictionary<int, HashSet<int>>();
        _updates = new List<List<int>>();

        var queue = new Queue<string>(_data);

        while (queue.TryDequeue(out var line))
        {
            if (line.IsWhitespace()) break;

            var parts = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
            var x = parts[0].ToInt();
            var y = parts[1].ToInt();

            if(!_dict.ContainsKey(x)) _dict.Add(x, new HashSet<int>());
            _dict[x].Add(y);
        }

        while (queue.TryDequeue(out var line))
        {
            if (line.IsWhitespace()) break;

            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

            _updates.Add(parts.Select(x => x.ToInt()).ToList());
        }
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

        _dict.DumpJsonIndented("dict", true);
        _updates.DumpJsonIndented("updates", true);
    }

    private class ManualComparer : IComparer<int>
    {
        private readonly Dictionary<int, HashSet<int>> _map;

        public ManualComparer(Dictionary<int, HashSet<int>> map)
        {
            _map = map;
        }

        public int Compare(int x, int y)
        {
            return (!_map.ContainsKey(x) || !_map[x].Contains(y)) ? 1 : -1;
        }
    }
}

#if DUMP
#endif