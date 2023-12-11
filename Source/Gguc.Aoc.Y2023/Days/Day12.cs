#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day12 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 12;

    private List<string> _data;
    private List<List<char>> _records;
    private List<List<int>> _controls;
    private int _count;

    public Day12(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "6958";
        Expected2 = "6555315065024";
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
        var result = SumArrangements(1);

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = SumArrangements(5);

        Result = result;
    }

    private long SumArrangements(int coef = 1)
    {
        var sum = 0L;
        var comb = 0L;

        for (var i = 0; i < _records.Count; i++)
        {
            var mask = UnfoldMask(_records[i], coef);
            var control = UnfoldControls(_controls[i], coef);
            
            comb = AddAllCombinations(control, mask);
            // Log.Debug($"Combination: [{comb}]");

            sum += comb;
        }

        return sum;
    }

    private long AddAllCombinations(List<int> control, List<char> mask)
    {
        var size = mask.Count;
        var variants = new Dictionary<int, long> { [size] = 1 };

        for (var i = 0; i < control.Count; i++)
        {
            var next = control[i];

            var islast = i == control.Count - 1;
            if (islast)
            {
                // Log.Debug($"Last! i=[{i}]... next=[{next}]");
            }

            var rest = 0;
            for (var j = i + 1; j < control.Count; j++)
            {
                rest += control[j] + 1;
            }

            var temp = new Dictionary<int, long>();
            foreach (var variant in variants.Keys)
            {
                var count = variants[variant];

                var combinations = AddCombinationsNext(variant, size, next, mask, rest, islast);
                // Log.Debug($"variant=[{variant}] count=[{count}] result=[{result.Count}]");

                combinations.ForEach(comb =>
                {
                    var unsolved = comb.Count(x => x == '?');
                    if (!temp.ContainsKey(unsolved)) temp[unsolved] = 0L;
                    temp[unsolved] += count;
                });
            }

            variants = temp;
        }

        return variants.Values.Sum();
    }

    private List<List<char>> AddCombinationsNext(int count, int size, int value, List<char> mask, int rest, bool isLast)
    {
        var result = new List<List<char>>();

        var ch = isLast ? '.' : '?';
        var initial = CreateInitial(count, ch);

        var combinations = AddCombinations(initial, size, value, mask, rest, isLast);
        result.AddRange(combinations);

        return result;
    }

    private List<List<char>> AddCombinations(List<char> current, int size, int value, List<char> mask, int rest, bool isLast)
    {
        var result = new List<List<char>>();
        var first = 0;
        var last = current.Count - 1;
        var currentSize = current.Count;
        var validationFirst = size - currentSize;

        if (currentSize <= 0) return result;

        for (var i = first; i <= last - value - rest + 1; i++)
        {
            var newComb = current.ToList();

            for (var k = first; k < i; k++)
            {
                newComb[k] = '.';
            }

            var j = 0;
            for (; j < value; j++)
            {
                newComb[i + j] = '#';
            }

            if (i + j < currentSize)
            {
                newComb[i + j] = '.';
            }

            var isValid = ValidateCombination(newComb, mask, validationFirst);
            if (isValid)
            {
                result.Add(newComb);
            }
        }

        return result;
    }

    private bool ValidateCombination(List<char> combination, List<char> mask, int first)
    {
        var last = combination.FindIndex(x => x == '?');

        if (last == -1) last = combination.Count;

        if (first != 0 && (mask[first - 1] == '#')) return false;

        for (var i = 0; i < last; i++)
        {
            if (mask[i + first] == '?') continue;
            if (combination[i] != mask[i + first]) return false;
        }

        return true;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var records = new List<List<char>>();
        var controls = new List<List<int>>();

        foreach (var line in _data)
        {
            var parts = line.Split(' ');
            var mx = parts.First().ToCharArray().ToList();
            var ix = parts.Last().Split(',').Select(x => x.ToInt()).ToList();

            records.Add(mx);
            controls.Add(ix);
        }

        _records = records;
        _controls = controls;
        _count = records.Count;
    }

    private List<char> CreateInitial(int size, char ch = '?')
    {
        var initial = new List<char>();
        for (var i = 0; i < size; i++) initial.Add(ch);
        return initial;
    }

    private List<char> UnfoldMask(List<char> record, int coef)
    {
        var result = new List<char>();

        for (var i = 0; i < coef; i++)
        {
            result.AddRange(record);
            result.Add('?');
        }

        result.RemoveAt(result.Count - 1);

        return result;
    }

    private List<int> UnfoldControls(List<int> control, int coef)
    {
        var result = new List<int>();

        for (var i = 0; i < coef; i++)
        {
            result.AddRange(control);
        }

        return result;
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

        // _records.DumpJson();
        // _controls.DumpJson();
    }
}

#if DUMP
#endif