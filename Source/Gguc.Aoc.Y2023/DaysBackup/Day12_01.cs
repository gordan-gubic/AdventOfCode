#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2023.DaysBackup;

public class Day12_01 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 12_01;

    private List<string> _data;
    private List<List<char>> _records;
    private List<List<int>> _controls;
    private int _count;

    public Day12_01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "6958";
        Expected2 = "";
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
        var result = SumArrangements();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = SumArrangements2();

        Result = result;
    }

    private long SumArrangements()
    {
        var sum = 0L;

        for (int i = 0; i < _records.Count; i++)
        {
            var mask = _records[i];
            var control = _controls[i];

            var comb = CalcArrangements(mask, control, i);
            sum += comb;
        }

        return sum;
    }

    private long SumArrangements2()
    {
        var sum = 0L;

        for (int i = 0; i < _records.Count; i++)
        {
            var mask = UnfoldMask(_records[i]);
            var control = UnfoldControls(_controls[i]);

            mask.DumpJson("mask");
            control.DumpJson("control");

            var comb = CalcArrangements(mask, control, i);
            sum += comb;
        }

        return sum;
    }

    private List<char> UnfoldMask(List<char> record)
    {
        var result = new List<char>();

        for (var i = 0; i < 5; i++)
        {
            result.AddRange(record);
            result.Add('?');
        }

        result.RemoveAt(result.Count - 1);

        return result;
    }

    private List<int> UnfoldControls(List<int> control)
    {
        var result = new List<int>();

        for (var i = 0; i < 5; i++)
        {
            result.AddRange(control);
        }

        return result;
    }

    private long CalcArrangements(List<char> mask, List<int> control, int i)
    {
        var initial = new List<char>();
        for (var j = 0; j < mask.Count; j++) initial.Add('?');

        var combinations = new List<List<char>>();
        combinations.Add(initial);

        combinations = AddAllCombinations(combinations, control, mask);

        // combinations.DumpJson("combinations");

        return combinations.Count;
    }

    private List<List<char>> AddAllCombinations(List<List<char>> combinations, List<int> control, List<char> mask)
    {
        var result = combinations;
        var queue = new Queue<int>(control);

        while (queue.Count > 0)
        {
            var next = queue.Dequeue();

            // Log.Debug($"next=[{next}]");

            result = AddCombinationsNext(result, next, mask);
        }

        // FinalValidation
        result = result.Where(x => ValidateCombination(x, mask)).ToList();

        Log.Debug($" ---- ---- ");
        Log.Debug($"control=[{control.ToJson()}] Sum=[{result.Count}]");
        Log.Debug($".......MASK=[{mask.ToJson()}]");
        result.ForEach(x => Log.Debug($"Combination=[{x.ToJson()}]"));

        return result;
    }

    private List<List<char>> AddCombinationsNext(List<List<char>> combinations, int next, List<char> mask)
    {
        var result = new List<List<char>>();

        foreach (var combination in combinations)
        {
            // Log.Debug($"foreach combination next=[{next}]");

            var cnext = AddCombinations(combination, next, mask);
            result.AddRange(cnext);

            // cnext.ForEach(x => Log.Debug($"Combination=[{x.ToJson()}]"));
        }

        return result;
    }

    private List<List<char>> AddCombinations(List<char> current, int value, List<char> mask)
    {
        var result = new List<List<char>>();
        var first = current.FindIndex(x => x == '?');
        var last = current.FindLastIndex(x => x == '?');
        var size = current.Count;

        if (first < 0 || last < 0) return result;

        for (var i = first; i <= last - value + 1; i++)
        {
            var newComb = current.ToList();

            for (int k = first; k < i; k++)
            {
                newComb[k] = '.';
            }

            var j = 0;
            for (; j < value; j++)
            {
                newComb[i + j] = '#';
            }

            if (i + j < size)
            {
                newComb[i + j] = '.';
            }

            var isValid = ValidateCombinationTemp(newComb, mask);
            if (isValid)
            {
                result.Add(newComb);
            }

            // Log.Debug($"Combination=[{newComb.ToJson()}]  Valid=[{isValid}]");
        }

        return result;
    }

    private bool ValidateCombinationTemp(List<char> combination, List<char> mask)
    {
        for (var i = 0; i < mask.Count; i++)
        {
            if (combination[i] == '?' || mask[i] == '?') continue;
            if (combination[i] != mask[i]) return false;
        }

        return true;
    }

    private bool ValidateCombination(List<char> combination, List<char> mask)
    {
        for (var i = 0; i < mask.Count; i++)
        {
            if (mask[i] == '?') continue;
            if (mask[i] == '.' && combination[i] == '?') continue;
            if (combination[i] != mask[i]) return false;
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