#define LOG

namespace Gguc.Aoc.Y2025.Services;

using Gguc.Aoc.Y2025.Models;

internal class FactoryService
{
    private readonly bool _doLog;

    public FactoryService(Factory factory, bool doLog = false)
    {
        Item = factory;
        JoltagesCount = factory.Joltages.Count;
        _doLog = doLog;
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
        for (var i = 0; i < JoltagesCount; i++)
        {
            _targetJoltages[i] = Item.Joltages[i];
            _restJoltages[i] = Item.Joltages[i];
            _currentJoltages[i] = 0;
        }

        foreach (var sw in Item.Switches)
        {
            var max = CalcMaxSwitch(sw);
            // $"sw=[{sw.ToJson()}]. max=[{max}]".Dump();

            _maxSwitches[sw] = max;
            _restSwitches[sw] = max;
            _currentSwitches[sw] = 0;
        }
    }

    private void CloneSwitches(FactoryService master)
    {
        for (var i = 0; i < JoltagesCount; i++)
        {
            _targetJoltages[i] = master._targetJoltages[i];
            _restJoltages[i] = master._restJoltages[i];
            _currentJoltages[i] = master._currentJoltages[i];
        }

        _maxSwitches = master._maxSwitches.ToDictionary(k => k.Key, v => v.Value);
        _restSwitches = master._restSwitches.ToDictionary(k => k.Key, v => v.Value);
        _currentSwitches = master._currentSwitches.ToDictionary(k => k.Key, v => v.Value);
    }

    public long MinMaxSwitches()
    {
        // Show(_maxSwitches, "_maxSwitches");
        if(_doLog) _targetJoltages.DumpJson("_targetJoltages");
        // _currentJoltages.DumpJson("_currentJoltages");

        while (true)
        {
            var isFound = EliminateUniqueSwitches();
            if (!isFound) break;

            // Show(_maxSwitches);
            // Show(_restSwitches);
            // Show(_currentSwitches);

            ComputeRestSwitches();
        }

        if (HasLeftoverJoltages())
        {
            /*
            for (int i = 2; i < 7; i++)
            {
                var r1 = ForceTest(i);
                if (r1 == -1) continue;

                return r1;
            }
            */

            int[] ranges = [6, 2, 3, 4, 5];
            foreach (var i in ranges)
            {
                var r1 = ForceTest(i);
                if (r1 == -1) continue;

                if(r1 > 0 && r1 < long.MaxValue)
                {
                    $"---FOUND!!!--- r1=[{r1}]".Dump();
                }

                return r1;
            }

            // var r2 = ForceTestX();
            // return r2;

            /*
            for (int i = 6; i > 2; i--)
            {
                var r1 = ForceTest(i);
                if (r1 == -1) continue;

                return r1;
            }
            */
        }

        if (HasFoundTarget())
        {
            var totalSwitches = TotalSwitches();
            $"---FOUND!!!--- totalSwitches=[{totalSwitches}]".Dump();
            return totalSwitches;
        }

        // _targetJoltages.DumpJson("_targetJoltages End");
        // _currentJoltages.DumpJson("_currentJoltages End");

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

    private void ApplySwitches(HashSet<int> sw1, int v1, HashSet<int> sw2 = null, int v2 = 0, HashSet<int> sw3 = null, int v3 = 0, HashSet<int> sw4 = null, int v4 = 0, HashSet<int> sw5 = null, int v5 = 0, HashSet<int> sw6 = null, int v6 = 0)
    {
        if (sw1 != null)
        {
            _restSwitches[sw1] -= v1;
            _currentSwitches[sw1] += v1;
            AddJoltage(sw1, v1);
        }

        if (sw2 != null)
        {
            _restSwitches[sw2] -= v2;
            _currentSwitches[sw2] += v2;
            AddJoltage(sw2, v2);
        }

        if (sw3 != null)
        {
            _restSwitches[sw3] -= v3;
            _currentSwitches[sw3] += v3;
            AddJoltage(sw3, v3);
        }

        if (sw4 != null)
        {
            _restSwitches[sw4] -= v4;
            _currentSwitches[sw4] += v4;
            AddJoltage(sw4, v4);
        }

        if (sw5 != null)
        {
            _restSwitches[sw5] -= v5;
            _currentSwitches[sw5] += v5;
            AddJoltage(sw5, v5);
        }

        if (sw6 != null)
        {
            _restSwitches[sw6] -= v6;
            _currentSwitches[sw6] += v6;
            AddJoltage(sw6, v6);
        }

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
            // $"Rest. sw=[{sw.ToJson()}]. max=[{max}]".Dump();

            _restSwitches[sw] = max;
        }
    }

    private bool EliminateUniqueSwitches()
    {
        var unique = GenerateGroups();
        if (unique.IsNullOrEmpty()) return false;
        // unique.DumpJson("unique");

        var uniqueFiltered = unique.Where(x => x.Value.Count == 1).Select(x => x.Value).FirstOrDefault();
        // uniqueFiltered.DumpJson("uniqueFiltered");
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

    private long ForceTest(int n)
    {
        var unique = GenerateGroups();
        if (unique.IsNullOrEmpty()) return -1;
        // unique.DumpJson("unique");

        var uniqueFiltered = unique.Where(x => x.Value.Count == n);
        // uniqueFiltered.DumpJson("uniqueFiltered");
        if (uniqueFiltered.IsNullOrEmpty()) return -1;

        var minVar = new List<(int, int, int, HashSet<int>, List<HashSet<int>>)> ();

        foreach (var uf in uniqueFiltered!)
        {
            var key = uf.Key;
            foreach (var sw in uf.Value)
            {
                // var rest = _restSwitches[sw];
                var rest = _restJoltages[key];
                var count = sw.Count;
                minVar.Add((key, rest, count, sw, uf.Value));
            }
        }

        // var first = minVar.OrderBy(x => x.Item2).ThenByDescending(x => x.Item3).FirstOrDefault();
        // var first = minVar.OrderByDescending(x => x.Item2).ThenByDescending(x => x.Item3).FirstOrDefault();
        var first = minVar.OrderBy(x => x.Item2).FirstOrDefault();

        var r1 = RunTestN(n, first.Item1, first.Item5);
        return r1;
    }

    private long ForceTestX()
    {
        var unique = GenerateGroups();
        if (unique.IsNullOrEmpty()) return -1;
        // unique.DumpJson("unique");

        var uniqueFiltered = unique.Where(x => x.Value.Count > 3);
        // uniqueFiltered.DumpJson("uniqueFiltered");
        if (uniqueFiltered.IsNullOrEmpty()) return -1;

        var minVar = new List<(int, int, int, HashSet<int>, List<HashSet<int>>)>();

        foreach (var uf in uniqueFiltered!)
        {
            var key = uf.Key;
            foreach (var sw in uf.Value)
            {
                // var rest = _restSwitches[sw];
                var rest = _restJoltages[key];
                var count = sw.Count;
                minVar.Add((key, rest, count, sw, uf.Value));
            }
        }

        // var first = minVar.OrderBy(x => x.Item2).ThenByDescending(x => x.Item3).FirstOrDefault();
        // var first = minVar.OrderByDescending(x => x.Item2).ThenByDescending(x => x.Item3).FirstOrDefault();
        var first = minVar.OrderBy(x => x.Item2).FirstOrDefault();

        var r1 = RunTestN(first.Item5.Count, first.Item1, first.Item5);
        return r1;
    }

    private Dictionary<int, List<HashSet<int>>> GenerateGroups()
    {
        var unique = new Dictionary<int, List<HashSet<int>>>();
        for (var i = 0; i < JoltagesCount; i++)
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

    private long RunTestN(int n, int key, List<HashSet<int>> switches)
    {
        return n switch
        {
            2 => RunTest2(key, switches),
            3 => RunTest3(key, switches),
            4 => RunTest4(key, switches),
            5 => RunTest5(key, switches),
            6 => RunTest6(key, switches),
        };
    }

    private long RunTest2(int key, List<HashSet<int>> switches)
    {
        // var ordered = switches.OrderByDescending(x => x.Count).ToArray();
        var ordered = switches.OrderBy(x => _restSwitches[x]).ThenByDescending(x => x.Count).ToArray();
        var maxLarger = _restSwitches[ordered[0]];
        var maxSmaller = _restSwitches[ordered[1]];

        var min = long.MaxValue;
        for (int i = maxLarger; i >= 0; i--)
        {
            var smaller = maxSmaller - i;
            if (smaller < 0) return long.MaxValue;

            var r1 = RunTestSingleN(key, ordered[0], i, ordered[1], smaller);
            min = Math.Min(min, r1);
            //if (r1 < long.MaxValue) return r1;
        }

        return min;
    }

    private long RunTest3(int key, List<HashSet<int>> switches)
    {
        // var ordered = switches.OrderByDescending(x => x.Count).ToArray();
        var ordered = switches.OrderBy(x => _restSwitches[x]).ThenByDescending(x => x.Count).ToArray();
        var max0 = _restSwitches[ordered[0]];
        var max1 = _restSwitches[ordered[1]];
        var max2 = _restSwitches[ordered[2]];

        var min = long.MaxValue;
        for (int i = max0; i >= 0; i--)
        {
            if (max1 - i < 0) break;

            for (int j = max1 - i; j >= 0; j--)
            {
                var k = max2 - j - i;
                if (k < 0) break;

                var r1 = RunTestSingleN(key, ordered[0], i, ordered[1], j, ordered[2], k);
                min = Math.Min(min, r1);
                // if (r1 < long.MaxValue) return r1;
            }
        }

        return min;
    }

    private long RunTest4(int key, List<HashSet<int>> switches)
    {
        // var ordered = switches.OrderByDescending(x => x.Count).ToArray()
        var ordered = switches.OrderBy(x => _restSwitches[x]).ThenByDescending(x => x.Count).ToArray();
        var max0 = _restSwitches[ordered[0]];
        var max1 = _restSwitches[ordered[1]];
        var max2 = _restSwitches[ordered[2]];
        var max3 = _restSwitches[ordered[3]];

        var min = long.MaxValue;
        for (int i = max0; i >= 0; i--)
        {
            for (int j = max1 - i; j >= 0; j--)
            {
                for (int k = max2 - i - j; k >= 0; k--)
                {
                    var m = max3 - i - j - k;
                    if (m < 0) break;

                    var r1 = RunTestSingleN(key, ordered[0], i, ordered[1], j, ordered[2], k, ordered[3], m);
                    min = Math.Min(min, r1);
                    // if (r1 < long.MaxValue) return r1;
                }
            }
        }

        return min;
    }



    private long RunTest4X(int key, List<HashSet<int>> switches)
    {
        // var ordered = switches.OrderByDescending(x => x.Count).ToArray()
        var ordered = switches.OrderBy(x => _restSwitches[x]).ThenByDescending(x => x.Count).ToArray();
        var max0 = _restSwitches[ordered[0]];
        var max1 = _restSwitches[ordered[1]];
        var max2 = _restSwitches[ordered[2]];
        var max3 = _restSwitches[ordered[3]];

        var min = long.MaxValue;
        for (int i = max0; i >= 0; i--)
        {
            for (int j = max1 - i; j >= 0; j--)
            {
                for (int k = max2 - i - j; k >= 0; k--)
                {
                    var m = max3 - i - j - k;
                    if (m < 0) break;

                    var r1 = RunTestSingleN(key, ordered[0], i, ordered[1], j, ordered[2], k, ordered[3], m);
                    min = Math.Min(min, r1);
                    // if (r1 < long.MaxValue) return r1;
                }
            }
        }

        return min;
    }

    private long RunTest5(int key, List<HashSet<int>> switches)
    {
        // var ordered = switches.OrderByDescending(x => x.Count).ToArray();
        var ordered = switches.OrderBy(x => _restSwitches[x]).ThenByDescending(x => x.Count).ToArray();
        // var ordered = switches.OrderByDescending(x => _restSwitches[x]).ThenByDescending(x => x.Count).ToArray();
        var max0 = _restSwitches[ordered[0]];
        var max1 = _restSwitches[ordered[1]];
        var max2 = _restSwitches[ordered[2]];
        var max3 = _restSwitches[ordered[3]];
        var max4 = _restSwitches[ordered[4]];

        var min = long.MaxValue;
        for (int i = max0; i >= 0; i--)
        {
            for (int j = max1 - i; j >= 0; j--)
            {
                for (int k = max2 - i - j; k >= 0; k--)
                {
                    for (int m = max3 - i - j - k; m >= 0; m--)
                    {
                        var n = max4 - i - j - k - m;
                        if (n < 0) break;

                        var r1 = RunTestSingleN(key, ordered[0], i, ordered[1], j, ordered[2], k, ordered[3], m, ordered[4], n);
                        min = Math.Min(min, r1);
                        // if (r1 < long.MaxValue) return r1;
                    }
                }
            }
        }

        return min;
    }

    private long RunTest6(int key, List<HashSet<int>> switches)
    {
        // var ordered = switches.OrderByDescending(x => x.Count).ToArray();
        var ordered = switches.OrderBy(x => _restSwitches[x]).ThenByDescending(x => x.Count).ToArray();
        // var ordered = switches.OrderByDescending(x => _restSwitches[x]).ThenByDescending(x => x.Count).ToArray();
        var max0 = _restSwitches[ordered[0]];
        var max1 = _restSwitches[ordered[1]];
        var max2 = _restSwitches[ordered[2]];
        var max3 = _restSwitches[ordered[3]];
        var max4 = _restSwitches[ordered[4]];
        var max5 = _restSwitches[ordered[5]];

        var min = long.MaxValue;
        for (int i = max0; i >= 0; i--)
        {
            for (int j = max1 - i; j >= 0; j--)
            {
                if (_doLog) $"Test: key=[{key}] -- sw1={i:###} sw2={j:###}".Dump();
                for (int k = max2 - i - j; k >= 0; k--)
                {
                    for (int m = max3 - i - j - k; m >= 0; m--)
                    {
                        for (int n = max4 - i - j - k - m; n >= 0; n--)
                        {
                            var p = max5 - i - j - k - m - n;
                            if (p < 0) break;

                            var r1 = RunTestSingleN(key, ordered[0], i, ordered[1], j, ordered[2], k, ordered[3], m, ordered[4], n, ordered[5], p);
                            min = Math.Min(min, r1);
                            // if (r1 < long.MaxValue) return r1;
                        }
                    }
                }
            }
        }

        return min;
    }

    private long RunTestSingleN(int key, HashSet<int> sw1, int v1, HashSet<int> sw2 = null, int v2 = 0, HashSet<int> sw3 = null, int v3 = 0, HashSet<int> sw4 = null, int v4 = 0, HashSet<int> sw5 = null, int v5 = 0, HashSet<int> sw6 = null, int v6 = 0)
    {
        // if(_doLog) $"Test: key=[{key}] sw1=[{sw1.ToJson()}, {v1}] sw2=[{sw2.ToJson()}, {v2}]  sw3=[{sw3.ToJson()}, {v3}]  sw4=[{sw4.ToJson()}, {v4}]  sw5=[{sw5.ToJson()}, {v5}]  sw6=[{sw6.ToJson()}, {v6}]".Dump();

        var service = new FactoryService(Item);
        service.CloneSwitches(this);
        service.ApplySwitches(sw1, v1, sw2, v2, sw3, v3, sw4, v4, sw5, v5, sw6, v6);
        var result = service.MinMaxSwitches();

        // $"Test: result=[{result}]".Dump();
        // if (_doLog) $"Test: key=[{key}] -- Result=[{result}] -- sw1=[{sw1.ToJson()}, {v1}] sw2=[{sw2.ToJson()}, {v2}]  sw3=[{sw3.ToJson()}, {v3}]  sw4=[{sw4.ToJson()}, {v4}]  sw5=[{sw5.ToJson()}, {v5}]  sw6=[{sw6.ToJson()}, {v6}]".Dump();

        return result;
    }

    private bool HasLeftoverJoltages()
    {
        // $"HasLeftoverJoltages=[{_restJoltages.Values.Sum()}]".Dump();
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
        // _maxSwitches.Values.Sum().Dump("Sum _maxSwitches");
        // _restSwitches.Values.Sum().Dump("Sum _restSwitches");
        // _currentSwitches.Values.Sum().Dump("Sum _currentSwitches");

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

internal class FactoryCache
{

}