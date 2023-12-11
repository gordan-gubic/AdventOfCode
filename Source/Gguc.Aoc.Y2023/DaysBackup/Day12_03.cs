#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.DaysBackup;

public class Day12_03 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 12_03;

    private List<string> _data;
    private List<List<char>> _records;
    private List<List<int>> _controls;
    private int _count;

    public Day12_03(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "6958";
        Expected2 = "6555315065024";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

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

        for (int i = 0; i < _records.Count; i++)
        {
            var mask = UnfoldMask(_records[i], coef);
            var control = UnfoldControls(_controls[i], coef);
            comb = CalcArrangements(mask, control, i);

            // Log.Debug($"Combination: [{comb}]");

            sum += comb;
        }

        return sum;
    }

    private long CalcArrangements(List<char> mask, List<int> control, int i)
    {
        var initial = new List<char>();
        for (var j = 0; j < mask.Count; j++) initial.Add('?');

        var combinations = new List<List<char>>();
        combinations.Add(initial);

        // combinations = AddAllCombinations(combinations, control, mask);
        var result = AddAllCombinations(combinations, control, mask);

        // combinations.DumpJson("combinations");

        // return combinations.Count;
        return result;
    }

    // private List<List<char>> AddAllCombinations
    private long AddAllCombinations(List<List<char>> combinations, List<int> control, List<char> mask)
    {
        var result = combinations;
        var size = mask.Count;
        var variants = new Dictionary<int, long> { [size] = 1 };
        Log.Debug($"variants=[{variants.ToJson()}]");

        for (var i = 0; i < control.Count; i++)
        {
            var next = control[i];

            var islast = i == control.Count - 1;
            if (islast)
            {
                Log.Debug($"Last! i=[{i}]... next=[{next}]");
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

                result = AddCombinationsNext(variant, size, next, mask, rest, islast);
                Log.Debug($"variant=[{variant}] count=[{count}] result=[{result.Count}]");

                result.ForEach(comb =>
                {
                    var unsolved = comb.Count(x => x == '?');
                    if (!temp.ContainsKey(unsolved)) temp[unsolved] = 0L;
                    temp[unsolved] += count;
                });

                // foreach (var key in temp.Keys)
                // {
                //     temp[key] = temp[key] * count;
                // }
            }

            // result = AddCombinationsNext(result, next, mask, rest);
            Log.Debug($"next=[{next}] result=[{result.Count}]");

            Log.Debug($"variants=[{temp.ToJson()}]");
            variants = temp;

            // if (i >= 2) break;
        }

        // FinalValidation
        // result = result.Where(x => ValidateCombination(x, mask)).ToList();
        Log.Debug($"result=[{result.Count}]");

        Log.Debug($" ---- ---- ");
        Log.Debug($"control=[{control.ToJson()}] Sum=[{result.Count}]");
        Log.Debug($".......MASK=[{mask.ToJson()}]");
        result.ForEach(comb =>
        {
            var rest = comb.Count(x => x == '?');
            Log.Debug($"Combination=[{comb.ToJson()}]... Rest=[{rest}]");
        });

        // return result;
        return variants.Values.Sum();
    }

    private List<List<char>> AddCombinationsNext(int count, int size, int value, List<char> mask, int rest, bool isLast)
    {
        var result = new List<List<char>>();

        // Log.Debug($"foreach combination next=[{next}]");
        var ch = isLast ? '.' : '?';
        var initial = CreateInitial(count, ch); // initial - rest

        var cnext = AddCombinations(initial, size, value, mask, rest, isLast);
        result.AddRange(cnext);

        // cnext.ForEach(x => Log.Debug($"Combination=[{x.ToJson()}]"));

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

            var isValid = ValidateCombinationTemp(newComb, mask, validationFirst);
            if (isValid)
            {
                result.Add(newComb);
            }

            // Log.Debug($"Combination=[{newComb.ToJson()}]  Valid=[{isValid}]");
        }

        return result;
    }

    private bool ValidateCombinationTemp(List<char> combination, List<char> mask, int first)
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

for (int j = 1; j <= 5; j++)
            {
                var mask = UnfoldMask(_records[i], j);
                var control = UnfoldControls(_controls[i], j);
                prev = comb;
                comb = CalcArrangements(mask, control, i);
                // Log.Debug($"i=[{j}]  comb=[{comb}, {prev}]  diff=[{comb-prev}]");
            }

            /*
            // --1--
            {
                var mask = UnfoldMask(_records[i], 1);
                var control = UnfoldControls(_controls[i], 1);
                prev = CalcArrangements(mask, control, i);
            }

            // --2--
            {
                var mask = UnfoldMask(_records[i], 2);
                var control = UnfoldControls(_controls[i], 2);
                comb = CalcArrangements(mask, control, i);
            }

            var diff = comb / prev;

            var result = comb * diff * diff * diff;
            */



    private List<List<char>> AddCombinationsNext(List<List<char>> combinations, int next, List<char> mask, int rest)
    {
        var result = new List<List<char>>();

        foreach (var combination in combinations)
        {
            // Log.Debug($"foreach combination next=[{next}]");

            var cnext = AddCombinations(combination, next, mask, rest);
            result.AddRange(cnext);

            // cnext.ForEach(x => Log.Debug($"Combination=[{x.ToJson()}]"));
        }

        return result;
    }

    private List<List<char>> AddCombinations(List<char> current, int size, int value, List<char> mask, int rest, bool isLast)
    {
        var result = new List<List<char>>();
        var first = current.FindIndex(x => x == '?');
        var last = current.FindLastIndex(x => x == '?');
        var currentSize = current.Count;

        if (first < 0 || last < 0) return result;

        for (var i = first; i <= last - value - rest + 1; i++)
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

            if (i + j < currentSize)
            {
                newComb[i + j] = '.';
            }

            var isValid = ValidateCombinationTemp(newComb, mask, first);
            if (isValid)
            {
                result.Add(newComb);
            }

            // Log.Debug($"Combination=[{newComb.ToJson()}]  Valid=[{isValid}]");
        }

        return result;
    }
#endif