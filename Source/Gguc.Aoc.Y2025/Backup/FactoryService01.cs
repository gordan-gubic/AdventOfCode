#define LOG

namespace Gguc.Aoc.Y2025.Services;

using Gguc.Aoc.Y2025.Models;

internal class FactoryServiceDebug
{
    public FactoryServiceDebug(Factory factory)
    {
        Item = factory;
        JoltagesCount = factory.Joltages.Count;
    }

    public Factory Item { get; set; }

    public int JoltagesCount { get; set; }

    private Dictionary<HashSet<int>, int> _maxSwitches = new();
    private Dictionary<HashSet<int>, int> _restSwitches = new();
    private Dictionary<HashSet<int>, int> _currentSwitches = new();

    private Dictionary<int, int> _targetJoltages = new();
    private Dictionary<int, int> _restJoltages = new();
    private Dictionary<int, int> _currentJoltages = new();

    public int FindMinimumSwitches()
    {
        var result = -1;

        var candidates = new List<FactoryBranch>();
        foreach (var switches in Item.Switches)
        {
            var fb = new FactoryBranch
            {
                Item = Item, 
                Current = new BitArray(Item.Current.Count),
                NextSwitch = switches,
                UsedSwitches = [],
            };
            candidates.Add(fb);
        }

        while (candidates.Any())
        {
            var queue = new Queue<FactoryBranch>(candidates);
            candidates.Clear();

            while (queue.Any())
            {
                var branch = queue.Dequeue();
                ProcessBranch(branch);

                if (branch.IsCorrect)
                {
                    // branch.Total.Dump("Total"); branch.UsedSwitches.DumpJson("UsedSwitches");
                    return branch.Total;
                }

                candidates.AddRange(branch.Candidates);
            }

            candidates = candidates.OrderBy(c => c.UsedSwitches.Count).ToList();
        }

        return result;
    }

    public void InitSwitches()
    {
        for (var i = 0; i < Item.Joltages.Count; i++)
        {
            _targetJoltages[i] = Item.Joltages[i];
            _restJoltages[i] = Item.Joltages[i];
            _currentJoltages[i] = 0;
        }

        foreach (var sw in Item.Switches)
        {
            var max = CalcMaxSwitch(sw);
            $"sw=[{sw.ToJson()}]. max=[{max}]".Dump();

            _maxSwitches[sw] = max;
            _restSwitches[sw] = max;
            _currentSwitches[sw] = 0;
        }
    }

    public long MinMaxSwitches()
    {
        Show(_maxSwitches, "_maxSwitches");
        _targetJoltages.DumpJson("_targetJoltages");
        _currentJoltages.DumpJson("_currentJoltages");

        while (true)
        {
            var isFound = EliminateUniqueSwitches();
            Show(_maxSwitches);
            Show(_restSwitches);
            Show(_currentSwitches);

            ComputeRestSwitches();

            _currentJoltages.DumpJson("_currentJoltages");

            if (!isFound) break;
        }

        _targetJoltages.DumpJson("_targetJoltages End");
        _currentJoltages.DumpJson("_currentJoltages End");

        if (HasLeftoverJoltages())
        {
            var r1 = ForceDuplex();
            if (r1 < long.MaxValue)
            {
                $"---FOUND!!!--- Forced!!! r1=[{r1}]".Dump();
                return r1;
            }
        }

        if (HasFoundTarget())
        {
            var totalSwitches = TotalSwitches();
            $"---FOUND!!!--- totalSwitches=[{totalSwitches}]".Dump();
            return totalSwitches;
        }

        return long.MaxValue;
    }

    private bool HasFoundTarget()
    {
        for (var i = 0; i < JoltagesCount; i++)
        {
            if (_targetJoltages[i] != _currentJoltages[i]) return false;
        }

        return true;
    }

    private void ApplySwitches(HashSet<int> sw1, int v1, HashSet<int> sw2, int v2)
    {
        _restSwitches[sw1] -= v1;
        _restSwitches[sw2] -= v2;
        _currentSwitches[sw1] += v1;
        _currentSwitches[sw2] += v2;

        AddJoltage(sw1, v1);

        AddJoltage(sw2, v2);

        ComputeRestSwitches();
    }

    private void Show(Dictionary<HashSet<int>, int> switches, string title = "")
    {
        title.Dump();
        foreach (var kv in switches)
        {
            $"{kv.Key.ToJson()}: {kv.Value}".Dump();
        }
    }

    private void Show(Dictionary<int, int> switches, string title = "")
    {
        title.Dump();
        foreach (var kv in switches)
        {
            $"{kv.Key}: {kv.Value}".Dump();
        }
    }

    private void ComputeRestSwitches()
    {
        foreach (var sw in Item.Switches)
        {
            var max = CalcRestSwitch(sw);
            $"Rest. sw=[{sw.ToJson()}]. max=[{max}]".Dump();

            _restSwitches[sw] = max;
        }
    }

    private bool EliminateUniqueSwitches()
    {
        var unique = GenerateGroups();
        if(unique.IsNullOrEmpty()) return false;
        unique.DumpJson("unique");

        var uniqueFiltered = unique.Where(x => x.Value.Count == 1).Select(x => x.Value).FirstOrDefault();
        uniqueFiltered.DumpJson("uniqueFiltered");
        if (uniqueFiltered.IsNullOrEmpty()) return false;

        foreach (var sw in uniqueFiltered!)
        {
            var count = _restSwitches[sw];
            _restSwitches[sw] = 0;
            _currentSwitches[sw] += count;

            AddJoltage(sw, count);
        }

        return true;
    }

    private long ForceDuplex()
    {
        var unique = GenerateGroups();
        if (unique.IsNullOrEmpty()) return long.MaxValue;
        unique.DumpJson("unique");

        var uniqueFiltered = unique.Where(x => x.Value.Count == 2).FirstOrDefault();
        uniqueFiltered.DumpJson("uniqueFiltered");
        if (uniqueFiltered.Value.IsNullOrEmpty()) return long.MaxValue;

        var r1 = RunTest(uniqueFiltered.Key, uniqueFiltered.Value);
        return r1;
    }

    private long RunTest(int key, List<HashSet<int>> switches)
    {
        var ordered = switches.OrderByDescending(x => x.Count).ToArray();
        var maxLarger = _restSwitches[ordered[0]];
        var maxSmaller = _restSwitches[ordered[1]];

        var min = long.MaxValue;
        for (int i = maxLarger; i >= 0; i--)
        {
            var smaller = maxSmaller - i;
            if(smaller < 0) return long.MaxValue;

            var r1 = RunTest1(key, ordered[0], i, ordered[1], smaller);
            min = Math.Min(min, r1);
        }

        return min;
    }

    private long RunTest1(int key, HashSet<int> sw1, int v1, HashSet<int> sw2, int v2)
    {
        $"Test: key=[{key}] sw1=[{sw1.ToJson()}, {v1}] sw2=[{sw2.ToJson()}, {v2}]".Dump();

        var service = new FactoryService(Item);
        service.InitSwitches();
        service.ApplySwitches(sw1, v1, sw2, v2);
        var result = service.MinMaxSwitches();
        
        $"Test: result=[{result}]".Dump();
        return result;
    }

    private Dictionary<int, List<HashSet<int>>> GenerateGroups()
    {
        var unique = new Dictionary<int, List<HashSet<int>>>();
        for (var i = 0; i < Item.Joltages.Count; i++)
        {
            unique[i] = [];
        }

        var keys = _restSwitches.Where(x => x.Value > 0).Select(x => x.Key);
        if (!keys.Any()) return null;

        foreach (var sw in keys)
        {
            foreach (var s in sw)
            {
                unique[s].Add(sw);
            }
        }
        // unique.DumpJson("unique");

        return unique;
    }

    private bool HasLeftoverJoltages()
    {
        $"HasLeftoverJoltages=[{_restJoltages.Values.Sum()}]".Dump();
        return _restJoltages.Values.Sum() > 0;
    }

    private void AddJoltage(HashSet<int> sw, int count)
    {
        foreach (var s in sw)
        {
            _currentJoltages[s] += count;
            _restJoltages[s] -= count;
        }
    }

    private int CalcMaxSwitch(HashSet<int> sw)
    {
        var result = int.MaxValue;

        foreach (var i in sw)
        {
            var joltage = _targetJoltages[i];
            result = Math.Min(result, joltage);
            // $"i=[{i}]. joltage=[{joltage}] result=[{result}]".Dump();
        }

        return result;
    }

    private int CalcRestSwitch(HashSet<int> sw)
    {
        var result = int.MaxValue;

        foreach (var i in sw)
        {
            var joltage = _restJoltages[i];
            result = Math.Min(result, joltage);
        }

        return result;
    }

    private long TotalSwitches()
    {
        _maxSwitches.Values.Sum().Dump("Sum _maxSwitches");
        _restSwitches.Values.Sum().Dump("Sum _restSwitches");
        _currentSwitches.Values.Sum().Dump("Sum _currentSwitches");

        return _currentSwitches.Values.Sum();
    }

    public int ShowMinimumJoltage(int row)
    {
        var sb = new StringBuilder();
        var columns = 15;

        sb.Append($"{Item.Joltages.ToJson()}\t");

        for (var c = 0; c < columns; c++)
        {
            if (c >= Item.Switches.Count)
            {
                sb.Append($"\t\t");
            }
            else
            {
                sb.Append($"{Item.Switches[c].ToJson()}\t0\t");
            }
        }

        sb.Append($"' --- \t");

        for (var c = 0; c < columns; c++)
        {
            if (c >= Item.Joltages.Count)
            {
                sb.Append($"\t");
            }
            else
            {
                sb.Append($"=(");

                for (int j = 0; j < Item.Switches.Count; j++)
                {
                    if (Item.Switches[j].Contains(c))
                    {
                        sb.Append($"+{ColumnToId(j)}{row}");
                    }
                }

                sb.Append($")\t");
            }
        }

        sb.Append($"' --- \t");

        foreach (var jolt in Item.Joltages)
        {
            sb.Append($"{jolt}\t");
        }

        sb.ToString().Dump();

        return -1;
    }

    private string ColumnToId(int column)
    {
        return column switch
        {
            0 => "C",
            1 => "E",
            2 => "G",
            3 => "I",
            4 => "K",
            5 => "M",
            6 => "O",
            7 => "Q",
            8 => "S",
            9 => "U",
            10 => "W",
            11 => "Y",
            12 => "AA",
            13 => "AC",
            14 => "AE",
            15 => "AG",
            _ => "C"
        };
    }

    private void ProcessBranch(FactoryBranch branch)
    {
        var sw = branch.NextSwitch;
        branch.UsedSwitches.Add(sw);

        foreach (var i in sw)
        {
            branch.Current[i] = !branch.Current[i];
        }

        if (branch.Current.IsEqual(branch.Item.Target))
        {
            branch.IsCorrect = true;
            branch.Total = branch.UsedSwitches.Count;
            return;
        }

        branch.Candidates = [];
        var candidates = branch.Item.Switches.Except(branch.UsedSwitches);
        foreach (var candidate in candidates)
        {
            branch.Candidates.Add(branch.NextCandidate(candidate));
        }
    }
}

internal class FactoryBranch
{
    public Factory Item { get; set; }

    public List<HashSet<int>> UsedSwitches { get; set; }

    public BitArray Current { get; set; }

    public HashSet<int> NextSwitch { get; set; }

    public bool IsCorrect { get; set; }

    public int Total { get; set; }

    public List<FactoryBranch> Candidates { get; set; }

    public FactoryBranch NextCandidate(HashSet<int> switches)
    {
        return new FactoryBranch
        {
            Item = Item,
            UsedSwitches = UsedSwitches.ToList(),
            Current = new BitArray(Current),
            NextSwitch = switches
        };
    }

    // public override string ToString() => $"{new { NextSwitch= NextSwitch.ToJson(), Current=Current.ToJson(), UsedSwitches=UsedSwitches.Count}}";
}